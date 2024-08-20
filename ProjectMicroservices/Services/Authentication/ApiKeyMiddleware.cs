using System.Net;
using Microsoft.VisualBasic;
using ProjectMicroservices.Util;

namespace ProjectMicroservices.Services.Authentication;

// This is a custom middleware for handling authentication with api key
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IApiKeyVal _apiKeyVal;
    
    // Getting the req delegate through dependency injection, for calling the next object in the execution pipeline
    // also getting the Api validation instance 
    public ApiKeyMiddleware(RequestDelegate next, IApiKeyVal apiKeyVal)
    {
        _next = next;
        _apiKeyVal = apiKeyVal;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // This code is checking if there is an api key proviced in the request and will stop
        // the request flow and return immediately if none is provided
        if (string.IsNullOrWhiteSpace(context.Request.Headers[StaticConstant.ApiKeyHeaderName]))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        
        string? userApiKey = context.Request.Headers[StaticConstant.ApiKeyHeaderName];
        
        // Checking if the api key is valid and return immediately if not
        if (!_apiKeyVal.IsValidApiKey(userApiKey!))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        // if all the checks passed, it will continue to the next layer of the pipeline
        await _next(context);
    }
}