using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Todo.Services
{
    public class GravatarUserService : IExternalUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private ILogger<GravatarUserService> _logger;

        public GravatarUserService(
            IHttpClientFactory httpClientFactory,
            ILogger<GravatarUserService> logger,
            IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetNameByEmailAsync(string email)
        {
            if (_memoryCache.TryGetValue(email, out string cachedUserName))
            {
                return cachedUserName;
            }

            var hash = Gravatar.GetHash(email);
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                try
                {
                    var requestUrl = $"https://gravatar.com/{hash}.json";
                    var response = await httpClient.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<GravatarUserProfile>(responseBody);
                    var userName = profile.Entry.FirstOrDefault()?.DisplayName;

                    if (!string.IsNullOrWhiteSpace(userName))
                    {
                        _memoryCache.Set(email, userName);
                    }

                    return userName;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return string.Empty;
        }
    }
}
