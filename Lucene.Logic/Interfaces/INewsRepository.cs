using Lucene.Logic.Dto;
using Lucene.Logic.query;

namespace Lucene.Logic.Interfaces
{
    public interface INewsRepository
    {
        List<NewsDto> GetAll();
        NewsDto Get(int id);
        List<NewsDto> Get(GetNewsQuery query);
    }
}
