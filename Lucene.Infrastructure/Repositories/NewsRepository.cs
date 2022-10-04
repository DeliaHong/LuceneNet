using Dapper;
using Lucene.Infrastructure.pojo;
using Lucene.Logic.Dto;
using Lucene.Logic.Interfaces;
using Lucene.Logic.query;
using Lucene.Net.Search;

namespace Lucene.Infrastructure.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly DapperContext _context;

        public NewsRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<List<NewsDto>> GetAll()
        {
            using (var connection = _context.CreateConnection())
            {

                var news = await connection.QueryAsync<News>("SELECT * FROM News");
                var dto = news.Select(s => new NewsDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    Description = s.Description
                }).ToList();

                return dto;
            }
        }

        public async Task<List<NewsDto>> Get(GetNewsQuery query)
        {
            string queryString = "SELECT * FROM News WHERE Title LIKE @Title OR Content LIKE @Content OR Description LIKE @Description";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("Title", $"%{query.Title}%");
            dynamicParams.Add("Description", $"%{query.Description}%");
            dynamicParams.Add("Content", $"%{query.Content}%");

            using (var connection = _context.CreateConnection())
            {
                var news = await connection.QueryAsync<News>(queryString, dynamicParams);
                var dto = news.Select(s => new NewsDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    Content = s.Content,
                    Description = s.Description
                }).ToList();

                return dto;
            }
        }

        public NewsDto Get(int id)
        {
            string queryString = "SELECT * FROM News WHERE Id = @Id ";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("Id", id);

            using (var connection = _context.CreateConnection())
            {
                var news = connection.QueryFirstOrDefault<News>(queryString, dynamicParams);
                var dto = new NewsDto()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Content = news.Content,
                    Description = news.Description
                };

                return dto;
            }
        }
    }
}
