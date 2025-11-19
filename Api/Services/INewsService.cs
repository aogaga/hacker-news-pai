using Api.Models;

namespace Api.Service
{
    public interface INewsService
    {
        Task<IEnumerable<NewsStory>> GetNewestStoriesAsync(int page = 1, int pageSize = 20);
        Task<IEnumerable<NewsStory>> SearchStoriesAsync(string searchTerm, int page = 1, int pageSize = 20);
    }
}
