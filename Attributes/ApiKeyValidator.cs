namespace StarterAPI.Attributes
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiKeyValidator> _logger;

        public ApiKeyValidator(IConfiguration configuration, ILogger<ApiKeyValidator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool IsValid(string apiKey)
        {
            var configuredApiKey = _configuration["ApiKey"];
            if (configuredApiKey == null)
            {
                _logger.LogError("API key is not configured.");
                return false;
            }

            if (configuredApiKey == apiKey)
            {
                return true;
            }

            _logger.LogInformation($"Invalid, or no API key provided: {apiKey}");
            return false;
        }
    }
}