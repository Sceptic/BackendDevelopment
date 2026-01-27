using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.ExternalApi.Helpers;

//Defines the errors that can be thrown by API clients.
public class ExternalApiException : Exception
{
    public string ApiName { get; }

    public ExternalApiException(string apiName, string message)
        : base(message)
    {
        ApiName = apiName;
    }

    public ExternalApiException(string apiName, string message, Exception innerException)
        : base(message, innerException)
    {
        ApiName = apiName;
    }
}

public class ResourceNotFoundException : ExternalApiException
{
    public string ResourceType { get; }
    public object ResourceId { get; }

    public ResourceNotFoundException(
        string apiName,
        string resourceType,
        object resourceId)
        : base(apiName, $"{resourceType} with id '{resourceId}' was not found.")
    {
        ResourceType = resourceType;
        ResourceId = resourceId;
    }
}