namespace ProjectMicroservices.Services.Authentication;

// An interface for creating Api Key validation class
public interface IApiKeyVal
{
    bool IsValidApiKey(string userApiKey);
}