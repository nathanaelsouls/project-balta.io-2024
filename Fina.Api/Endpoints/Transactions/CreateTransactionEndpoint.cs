using Fina.Api.Common.Api;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Transactions;
using Fina.Shared.Responses;

namespace Fina.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/", HandleAsync)
        .WithName("Transaction: Create")
        .WithSummary("Cria uma nova transacao")
        .WithDescription("Cria uma nova transacao")
        .WithOrder(1)
        .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, CreateTransactionRequest request)
    {
        request.UserId = ApiConfiguration.UserId;
        var result = await handler.CreatedAsync(request);
        return result.IsSuccess
                ? TypedResults.Created($"v1/transaction/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result);
    }
}
