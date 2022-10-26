using Lucene.Logic.query;
using Lucene.Logic.Services;
using Lucene_Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lucene_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LuceneController : ControllerBase
    {
        private readonly NewsService _newsService;
        public LuceneController(NewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("Create")]
        public IActionResult CreateIndex()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            _newsService.Create();
            stopWatch.Stop();
            return Ok($"產生索引檔共花費【{stopWatch.ElapsedMilliseconds}】毫秒");
        }

        [HttpGet("Delete")]
        public IActionResult DeleteIndex(int id)
        {
            _newsService.DeleteById(id);
            return Ok();
        }

        [HttpGet("DeleteAll")]
        public IActionResult DeleteIndex()
        {
            _newsService.DeleteAll();
            return Ok();
        }

        [HttpGet("Update")]
        public IActionResult Update(int id)
        {
            _newsService.Update(id);
            return Ok();
        }

        [HttpGet("/SearchByIndex/{query}")]
        public IActionResult SearchByIndex(string query)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var dto = _newsService.SearchByIndex(query);
            stopWatch.Stop();

            return Ok(new SearchNewsViewModel
            {
                Count = dto.Count,
                newsDtos = dto,
                SearchTime = stopWatch.ElapsedMilliseconds
            });
        }

        [HttpPost("SearchByIndexWithLogic")]
        public IActionResult SearchByIndexWithLogic(string[] query)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var dto = _newsService.SearchByIndexWithLogic(query);
            stopWatch.Stop();
            
            return Ok(new SearchNewsViewModel 
            { 
                Count = dto.Count,
                newsDtos = dto,
                SearchTime = stopWatch.ElapsedMilliseconds
            });
        }

        [HttpGet("/SearchBySql/{query}")]
        public IActionResult SearchBySql(string query)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var dto = _newsService.SearchBySql(new GetNewsQuery { Title = query , Description = query , Content = query });
            stopWatch.Stop();

            return Ok(new SearchNewsViewModel
            {
                Count = dto.Count,
                newsDtos = dto,
                SearchTime = stopWatch.ElapsedMilliseconds
            });
        }
    }
}