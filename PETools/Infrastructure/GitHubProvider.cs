using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class GitHubProvider
    {
        private readonly HttpClient _httpClient;

        public GitHubProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRepositoryAsync(string owner, string repo)
        {
            var url = $"https://api.github.com/repos/{owner}/{repo}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "PETools");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // Add more methods for interacting with GitHub as needed
    }
}