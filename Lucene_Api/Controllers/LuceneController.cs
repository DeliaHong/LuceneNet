using Lucene.Logic.query;
using Lucene.Logic.Services;
using Microsoft.AspNetCore.Mvc;

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
            _newsService.Create();
            return Ok();
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
            var dto = _newsService.SearchByIndex(query);
            return Ok(dto);
        }

        [HttpPost("SearchByIndexWithLogic")]
        public IActionResult SearchByIndexWithLogic(string[] query)
        {
            var dto = _newsService.SearchByIndexWithLogic(query);
            return Ok(dto);
        }

        [HttpGet("/SearchBySql/{query}")]
        public async Task<IActionResult> SearchBySql(string query)
        {
            await _newsService.SearchBySql(new GetNewsQuery { Title = query , Description = query , Content = query });
            return Ok();
        }
    }
}