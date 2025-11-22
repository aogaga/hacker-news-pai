using Api.Config;
using Api.Models;
using Api.Service;
using Api.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Api.Test.Unit.Services
{
    public class NewsServiceTests
    {
        private readonly Mock<IHackerNewsApi> _mockApi;
        private readonly Mock<IRedisService> _mockRedis;
        private readonly Mock<ILogger<NewsService>> _mockLogger;
        private readonly Mock<IOptions<NewsServiceConfig>> _mockConfig;

        private readonly List<NewsStory> _stories;

        public NewsServiceTests()
        {
            _mockApi = new Mock<IHackerNewsApi>();
            _mockRedis = new Mock<IRedisService>();
            _mockLogger = new Mock<ILogger<NewsService>>();
            _mockConfig = new Mock<IOptions<NewsServiceConfig>>();

            // Config setup
            _mockConfig.Setup(x => x.Value).Returns(new NewsServiceConfig
            {
                NewsCacheKey = "newestStoriesCache"
            });

            // Shared stories
            _stories = new List<NewsStory>
            {
                new NewsStory { Id = 1, Title = "Story 1" },
                new NewsStory { Id = 2, Title = "Story 2" },
                new NewsStory { Id = 3, Title = "Story 3" },
                new NewsStory { Id = 4, Title = "Story 4" },
                new NewsStory { Id = 5, Title = "Story 5" },
                new NewsStory { Id = 6, Title = "Story 6" }
            };

            // Default redis returns no cache
            _mockRedis.Setup(r => r.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(string.Empty);

            // Default API behavior
            _mockApi.Setup(a => a.GetNewStoryIdsAsync())
                .ReturnsAsync(new[] { 1, 2, 3, 4, 5, 6 });

            _mockApi.Setup(a => a.GetStoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _stories.Find(s => s.Id == id));
        }

        
        [Fact]
        public async Task GetNewestStoriesAsync_ReturnsPagedStories()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.GetNewestStoriesAsync(page: 2, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, s => s.Title == "Story 3");
            Assert.Contains(result, s => s.Title == "Story 4");
        }


        [Fact]
        public async Task GetNewestStoriesAsync_ReturnsEmptyList_OnApiException()
        {
            // Arrange
            _mockApi.Setup(a => a.GetNewStoryIdsAsync())
                .ThrowsAsync(new Exception("API error"));

            var service = CreateService();

            // Act
            var result = await service.GetNewestStoriesAsync(page: 1, pageSize: 2);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        
        private NewsService CreateService()
        {
            return new NewsService(
                _mockApi.Object,
                _mockRedis.Object,
                _mockLogger.Object,
                _mockConfig.Object);
        }

    }
}




