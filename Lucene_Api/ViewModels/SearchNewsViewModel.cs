using Lucene.Logic.Dto;

namespace Lucene_Api.ViewModels
{
    public class SearchNewsViewModel
    {
        public int Count { get; set; }
        public long SearchTime { get; set; }
        public List<NewsDto> newsDtos { get; set; }
    }
}
