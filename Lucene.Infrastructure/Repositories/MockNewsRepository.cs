using Lucene.Logic.Dto;
using Lucene.Logic.Interfaces;
using Lucene.Logic.query;

namespace Lucene.Infrastructure.Repositories
{
    public class MockNewsRepository : INewsRepository
    {
        private List<NewsDto> _news = new List<NewsDto>()
        {
            new NewsDto()
            {
                Id = 1,
                Title = "安心、營養、美味！有機寶寶粥的最佳選擇",
                Description = "許多媽媽選擇購買市面上已經烹煮好的寶寶粥，在家加熱給寶寶吃！"
            },
            new NewsDto()
            {
                Id = 2,
                Title = "【傳說中的澎湖米粉湯】農粉私房食譜",
                Description = "來自農粉的私房食譜，澎湖米粉湯也可在家輕鬆做！"
            },
        };

        public List<NewsDto> GetAll()
        {
            return _news;
        }

        public List<NewsDto> Get(GetNewsQuery query)
        {
            return _news.Where(w => w.Title.Contains(query.Title) || w.Content.Contains(query.Content) || w.Description.Contains(query.Description))
                .ToList();
        }

        public NewsDto Get(int id)
        {
            return _news.FirstOrDefault(f => f.Id == id);
        }
    }
}
