using System.Net.Http.Json;

namespace TestUtilities.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<TError> GetErrorResponse<TError>(this HttpResponseMessage response)
        where TError : class
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Expected an error response.");
        }

        TError? error = await response.Content.ReadFromJsonAsync<TError>();

        error.ShouldNotBeNull();

        return error;
    }
}
