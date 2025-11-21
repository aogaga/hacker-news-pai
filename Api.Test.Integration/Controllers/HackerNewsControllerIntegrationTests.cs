using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Test.Integration.Controllers
{
  public class HackerNewsControllerIntegrationTests(WebApplicationFactory<Program> factory)
      : IClassFixture<WebApplicationFactory<Program>>
  {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Get_ReturnsStories()
        {
            var response = await _client.GetAsync("/HackerNews?page=1&pageSize=2");
            response.EnsureSuccessStatusCode();

            var stories = await response.Content.ReadFromJsonAsync<IEnumerable<NewsStory>>();
            Assert.NotNull(stories);
            Assert.True(stories.Count() <= 2);
        }

        [Fact]
        public async Task Search_ReturnsFilteredStories()
        {
            var response = await _client.GetAsync("/HackerNews/search?term=Apple&page=1&pageSize=2");
            response.EnsureSuccessStatusCode();

            var stories = await response.Content.ReadFromJsonAsync<IEnumerable<NewsStory>>();
            Assert.NotNull(stories);
            // Optionally check for expected content if your test data is predictable
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_OnInvalidParameters()
        {
            var response = await _client.GetAsync("/HackerNews?page=-1&pageSize=0");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
