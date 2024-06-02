using Fina.Api.Common.Api;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Transactions;
using Fina.Shared.Responses;

namespace Fina.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("/{id}", HandleAsync)
        .WithName("Transaction: Delete")
        .WithSummary("Deleta uma transacao")
        .WithDescription("Deleta uma transacao")
        .WithOrder(3)
        .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new DeleteTransactionRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.DeleteAsync(request);
        return result.IsSuccess
        ? TypedResults.Ok(result)
        : TypedResults.BadRequest(result);
    }
}
