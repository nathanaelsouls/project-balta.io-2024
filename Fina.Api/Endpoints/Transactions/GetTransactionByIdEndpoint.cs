using Fina.Api.Common.Api;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Transactions;
using Fina.Shared.Responses;

namespace Fina.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/{id}", HandleAsync)
        .WithName("Transaction: Get By Id")
        .WithSummary("Recupera uma transacao")
        .WithDescription("Recupera uma transacao")
        .WithOrder(4)
        .Produces<Response<Transaction?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var request = new GetTransactionByIdRequest
        {
            UserId = ApiConfiguration.UserId,
            Id = id
        };

        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess
        ? TypedResults.Ok(result)
        : TypedResults.BadRequest(result);
    }
}
