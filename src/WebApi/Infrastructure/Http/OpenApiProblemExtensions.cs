using System.Globalization;
using Microsoft.OpenApi;

namespace WebApi.Infrastructure.Http;

public static class OpenApiProblemExtensions
{
    /// <summary>
    /// Adiciona uma resposta de erro (Problem) ao endpoint, com uma descrição customizada
    /// exibida no OpenAPI/Swagger.
    /// </summary>
    public static RouteHandlerBuilder ProducesProblemWithDescription(
        this RouteHandlerBuilder builder,
        int statusCode,
        string description
    )
    {
        return builder
            .ProducesProblem(statusCode)
            .AddOpenApiOperationTransformer(
                (operation, context, cancellationToken) =>
                {
                    string key = statusCode.ToString(CultureInfo.InvariantCulture);

                    if (
                        operation.Responses != null
                        && operation.Responses.TryGetValue(key, out IOpenApiResponse? response)
                    )
                    {
                        response.Description = description;
                    }
                    else
                    {
                        operation.Responses?.Add(
                            key,
                            new OpenApiResponse { Description = description }
                        );
                    }

                    return Task.CompletedTask;
                }
            );
    }

    /// <summary>
    /// Atalho para o erro 409 Conflict, já com nomenclatura explícita.
    /// </summary>
    public static RouteHandlerBuilder ProducesConflict(
        this RouteHandlerBuilder builder,
        string description
    )
    {
        return builder.ProducesProblemWithDescription(StatusCodes.Status409Conflict, description);
    }

    /// <summary>
    /// Atalho para o erro 500 Internal Server Error, com descrição padrão opcional.
    /// </summary>
    public static RouteHandlerBuilder ProducesInternalServerError(
        this RouteHandlerBuilder builder,
        string description = "Erro interno inesperado no servidor."
    )
    {
        return builder.ProducesProblemWithDescription(
            StatusCodes.Status500InternalServerError,
            description
        );
    }

    /// <summary>
    /// Atalho para o erro 404 Not Found, com nomenclatura explícita.
    /// </summary>
    public static RouteHandlerBuilder ProducesNotFound(
        this RouteHandlerBuilder builder,
        string description
    )
    {
        return builder.ProducesProblemWithDescription(StatusCodes.Status404NotFound, description);
    }

    /// <summary>
    /// Atalho para o erro 400 Bad Request de validação (ProducesValidationProblem),
    /// com descrição customizada.
    /// </summary>
    public static RouteHandlerBuilder ProducesValidationProblemWithDescription(
        this RouteHandlerBuilder builder,
        string description = "Um ou mais campos informados são inválidos."
    )
    {
        return builder
            .ProducesValidationProblem()
            .AddOpenApiOperationTransformer(
                (operation, _, _) =>
                {
                    string key = StatusCodes.Status400BadRequest.ToString(
                        CultureInfo.InvariantCulture
                    );

                    if (
                        operation.Responses != null
                        && operation.Responses.TryGetValue(key, out IOpenApiResponse? response)
                    )
                    {
                        response.Description = description;
                    }

                    return Task.CompletedTask;
                }
            );
    }
}
