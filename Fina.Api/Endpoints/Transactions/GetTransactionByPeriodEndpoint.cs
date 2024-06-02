using Fina.Api.Common.Api;
using Fina.Shared;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Transactions;
using Fina.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class GetTransactionByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/", HandleAsync)
        .WithName("Transaction: Get All")
        .WithSummary("Busca todas as transacoes")
        .WithDescription("Busca todas as transacoes")
        .WithOrder(5)
        .Produces<PagedResponse<List<Transaction>?>>();

    private static async Task<IResult> HandleAsync(ITransactionHandler handler,
                                                   [FromQuery] DateTime? startDate = null,
                                                   [FromQuery] DateTime? endDate = null,
                                                   [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
                                                   [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = ApiConfiguration.UserId,
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await handler.GetByPeriodAsync(request);
        return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
    }
}
