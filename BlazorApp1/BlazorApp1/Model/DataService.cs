using Microsoft.Extensions.Caching.Memory;

namespace BlazorApp1.Model
{
    public class DataService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public DataService(IHttpClientFactory httpClientFactory, ILoggerFactory logger, IMemoryCache cache)
        {
            _logger = logger!.CreateLogger<DataService>();
            _httpClient = httpClientFactory.CreateClient("ApiGestaoEnergia");
            _cache = cache;
        }

        public async Task<IEnumerable<string>> ObterEstadoAsync(string query, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
            {
                return Enumerable.Empty<string>();
            }

            // Verifica se o valor já está em cache
            if (_cache.TryGetValue(query, out IEnumerable<string>? cacheResult))
            {
                return cacheResult!;
            }

            // Simula um pequeno atraso para busca (substitua com a chamada real de API)
            await Task.Delay(100, token);

            var result = new List<string>
            {
                "Alabama", "Alaska", "American Samoa", "Arizona",
                "Washington", "West Virginia", "Wisconsin", "Wyoming",
            }
            .Where(x => x.Contains(query, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

            // Armazena no cache com tempo de expiração de 5 minutos
            _cache.Set(query, result, TimeSpan.FromMinutes(5));

            return result;
        }
    }
}
