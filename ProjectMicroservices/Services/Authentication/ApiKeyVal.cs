using ProjectMicroservices.Util;

namespace ProjectMicroservices.Services.Authentication;

public class ApiKeyVal : IApiKeyVal
{
    public ApiKeyVal()
    {
    }
    
    // This method compares the inputted apikey with the value of apikeyname in the static
    // constant class, not a very secure way to authenticate though
    public bool IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            return false;
        }
        
        // Get API key from environment variable inside docker environment
        string? apiKey = Environment.GetEnvironmentVariable(StaticConstant.ApiKeyName);
        if (apiKey == null || apiKey != userApiKey)
        {
            return false;
        }
        
        return true;
    }
}