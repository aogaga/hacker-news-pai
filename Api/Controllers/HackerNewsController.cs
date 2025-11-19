using Api.Models;
using Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController(INewsService _newsService, ILogger<HackerNewsController> _logger)
        : ControllerBase
    {



        [HttpGet]
        public async Task<IEnumerable<NewsStory>> Get(int page = 1, int pageSize = 20)
        {

            try
            {
              return await _newsService.GetNewestStoriesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Hacker News stories");
                throw;
            }
        }

        [HttpGet("search")]
        public async Task<IEnumerable<NewsStory>> Search([FromQuery] string term, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                return await _newsService.SearchStoriesAsync(term, page, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Hacker News stories");
                throw;
            }
        }

    }
}
