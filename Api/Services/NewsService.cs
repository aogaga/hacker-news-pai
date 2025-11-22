using Api.Controllers;
using Api.Models;
using Api.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using Api.Config;
using Microsoft.Extensions.Options;

namespace Api.Service
{
    public class NewsService: INewsService
    {

        private readonly IHackerNewsApi _newsApi;
        private readonly IRedisService _redisService;
        private readonly string _newsCacheKey;
        private IOptions<NewsServiceConfig> config;
        private readonly ILogger<NewsService> _logger;
        public NewsService(IHackerNewsApi api, IRedisService redisService, ILogger<NewsService> logger, IOptions<NewsServiceConfig> config)
        {
            _newsApi = api;
            _redisService = redisService;
            _logger = logger;
            _newsCacheKey = config.Value.NewsCacheKey;
        }


        public async Task<IEnumerable<NewsStory>> GetNewestStoriesAsync(int page = 1, int pageSize = 20)
        {
            var stories = await GetCachedStoriesAsync();
            return stories.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<NewsStory>> SearchStoriesAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
           var stories = await GetCachedStoriesAsync();
            var filteredStories = stories
                 .Where(s => s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                 .ToList();
            return filteredStories.Skip((page - 1) * pageSize).Take(pageSize);
        }


        private async Task<List<NewsStory>> GetCachedStoriesAsync()
        {
            var cachedStories = await _redisService.GetAsync(_newsCacheKey);

            if (!string.IsNullOrEmpty(cachedStories))
            {
                _logger.LogInformation("Fetching news stories from cache");
                var deserialized = JsonSerializer.Deserialize<List<NewsStory>>(cachedStories);
                return deserialized ?? [];
            }

            try
            {
                var storyIds = await _newsApi.GetNewStoryIdsAsync();
                var stories = new List<NewsStory>();
                var firstTwoHundred = storyIds.Take(200);
                foreach (var id in firstTwoHundred)
                {
                    var story = await _newsApi.GetStoryByIdAsync(id);
                    if (story != null)
                    {
                        stories.Add(story);
                    }
                }

                _logger.LogInformation("Saving news stories to cache");
                await _redisService.SetAsync(_newsCacheKey, JsonSerializer.Serialize(stories), TimeSpan.FromMinutes(10));

                return stories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching stories from Hacker News API");
                return [];
            }
        }
    }
}
