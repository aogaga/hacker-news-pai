using Api.Models;
using Refit;

namespace Api.Services
{
    public interface IHackerNewsApi
    {
        [Get("/v0/newstories.json")]
        Task<int[]> GetNewStoryIdsAsync();

        [Get("/v0/item/{id}.json")]
        Task<NewsStory> GetStoryByIdAsync(int id);
    }
}
