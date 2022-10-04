using Lucene.Logic.Dto;
using Lucene.Logic.query;

namespace Lucene.Logic.Interfaces
{
    public interface INewsRepository
    {
        Task<List<NewsDto>> GetAll();
        NewsDto Get(int id);
        Task<List<NewsDto>> Get(GetNewsQuery query);
    }
}
