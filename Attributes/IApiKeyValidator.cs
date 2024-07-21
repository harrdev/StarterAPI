namespace StarterAPI.Attributes
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
