using TextsCorpus.WebApi.Endpoints;

namespace TextsCorpus.WebApi.Extensions;

public static class EndpointExtensions
{
    public static void AddApplicationEndpoints(this WebApplication app)
    {
        app.AddUploadJsonExtensions();
    }

}
