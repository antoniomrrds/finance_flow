using System.Globalization;
using Microsoft.OpenApi;

namespace WebApi.Extensions.Docs;

public static class OpenApiProblemExtensions
{
    /// <summary>
    /// Atalho para o sucesso 201 Created, com descrição customizada.
    /// </summary>
    public static RouteHandlerBuilder ProducesCreated<TResponse>(
        this RouteHandlerBuilder builder,
        string description = "Recurso criado com sucesso."
    )
    {
        return builder
            .Produces<TResponse>(StatusCodes.Status201Created)
            .WithResponseDescription(StatusCodes.Status201Created, description);
    }

    /// <summary>
    /// Atalho para o sucesso 204 No Content, com descrição customizada.
    /// </summary>
    public static RouteHandlerBuilder ProducesNoContent(
        this RouteHandlerBuilder builder,
        string description = "Operação realizada com sucesso."
    )
    {
        return builder
            .Produces(StatusCodes.Status204NoContent)
            .WithResponseDescription(StatusCodes.Status204NoContent, description);
    }

    /// <summary>
    /// Atalho para sucesso 200 OK com corpo tipado, com descrição customizada.
    /// </summary>
    public static RouteHandlerBuilder ProducesOk<TResponse>(
        this RouteHandlerBuilder builder,
        string description = "Operação realizada com sucesso."
    )
    {
        return builder
            .Produces<TResponse>(StatusCodes.Status200OK)
            .WithResponseDescription(StatusCodes.Status200OK, description);
    }

    /// <summary>
    /// Atalho para sucesso 200 OK com corpo de mensagem (<see cref="MessageResponse"/>),
    /// com descrição customizada.
    /// </summary>
    public static RouteHandlerBuilder ProducesOkWithMessage(
        this RouteHandlerBuilder builder,
        string description = "Operação realizada com sucesso."
    )
    {
        return builder
            .Produces<MessageResponse>()
            .WithResponseDescription(StatusCodes.Status200OK, description);
    }

    /// <summary>
    /// Atalho para o erro 409 Conflict, já com nomenclatura explícita.
    /// </summary>
    public static RouteHandlerBuilder ProducesConflict(
        this RouteHandlerBuilder builder,
        string description
    )
    {
        return builder
            .ProducesProblem(StatusCodes.Status409Conflict)
            .WithResponseDescription(StatusCodes.Status409Conflict, description);
    }

    /// <summary>
    /// Atalho para o erro 500 Internal Server Error, com descrição padrão opcional.
    /// </summary>
    public static RouteHandlerBuilder ProducesInternalServerError(
        this RouteHandlerBuilder builder,
        string description = "Erro interno inesperado no servidor."
    )
    {
        return builder
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithResponseDescription(StatusCodes.Status500InternalServerError, description);
    }

    /// <summary>
    /// Atalho para o erro 404 Not Found, já com nomenclatura explícita.
    /// </summary>
    public static RouteHandlerBuilder ProducesNotFound(
        this RouteHandlerBuilder builder,
        string description
    )
    {
        return builder
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithResponseDescription(StatusCodes.Status404NotFound, description);
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
            .WithResponseDescription(StatusCodes.Status400BadRequest, description);
    }

    /// <summary>
    /// Define a descrição customizada de uma resposta no OpenAPI/Swagger para o
    /// status code informado. Centraliza a lógica de "achar ou adicionar" usada
    /// por todos os atalhos acima, evitando repetição.
    /// </summary>
    private static RouteHandlerBuilder WithResponseDescription(
        this RouteHandlerBuilder builder,
        int statusCode,
        string description
    )
    {
        return builder.AddOpenApiOperationTransformer(
            (operation, _, _) =>
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
}

/// <summary>
/// Corpo de resposta padrão para respostas de sucesso que carregam apenas uma mensagem.
/// </summary>
public sealed record MessageResponse(string Message);
