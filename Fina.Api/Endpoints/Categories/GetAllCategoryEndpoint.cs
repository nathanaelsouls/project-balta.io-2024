using Fina.Api.Common.Api;
using Fina.Shared;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Fina.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

public class GetAllCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/", HandleAsync)
        .WithName("Categories: Get All")
        .WithSummary("Busca todas as categoria")
        .WithDescription("Busca todas as categoria")
        .WithOrder(5)
        .Produces<PagedResponse<List<Category>?>>();

    private static async Task<IResult> HandleAsync(ICategoryHandler handler,
                                                   [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
                                                   [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllCategoryRequest
        {
            UserId = ApiConfiguration.UserId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await handler.GetAllAsync(request);
        return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
    }
}
