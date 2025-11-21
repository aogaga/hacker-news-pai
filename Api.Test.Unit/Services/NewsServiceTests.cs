using Api.Models;
using Api.Service;
using Api.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Test.Unit.Services
{
    public class NewsServiceTests
    {
            [Fact]
            public async Task GetNewestStoriesAsync_ReturnsPagedStories()
            {
                // Arrange
                var mockApi = new Mock<IHackerNewsApi>();
                var mockRedis = new Mock<IRedisService>();
                var mockLogger = new Mock<ILogger<NewsService>>();

                var stories = new List<NewsStory>
                {
                    new NewsStory { Id = 1, Title = "Story 1" },
                    new NewsStory { Id = 2, Title = "Story 2" },
                    new NewsStory { Id = 3, Title = "Story 3" },
                    new NewsStory { Id = 4, Title = "Story 4" },
                    new NewsStory { Id = 5, Title = "Story 5" },
                    new NewsStory { Id = 6, Title = "Story 6" }
                };

                mockRedis.Setup(r => r.GetAsync(It.IsAny<string>()))
                    .ReturnsAsync(string.Empty);

                mockApi.Setup(a => a.GetNewStoryIdsAsync())
                    .ReturnsAsync([1, 2, 3, 4, 5, 6]);

                mockApi.Setup(a => a.GetStoryByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync((int id) => stories.Find(s => s.Id == id));

                // If NewsService allows null for IStoryMapper, pass null
                var service = new NewsService(
                    mockApi.Object,
                    null, // No IStoryMapper
                    mockRedis.Object,
                    mockLogger.Object);

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
                var mockApi = new Mock<IHackerNewsApi>();
                var mockRedis = new Mock<IRedisService>();
                var mockLogger = new Mock<ILogger<NewsService>>();

                mockRedis.Setup(r => r.GetAsync(It.IsAny<string>()))
                    .ReturnsAsync(string.Empty);

                mockApi.Setup(a => a.GetNewStoryIdsAsync())
                    .ThrowsAsync(new Exception("API error"));

                var service = new NewsService(
                    mockApi.Object,
                    null,
                    mockRedis.Object,
                    mockLogger.Object);

                // Act
                var result = await service.GetNewestStoriesAsync(page: 1, pageSize: 2);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
    }
}


