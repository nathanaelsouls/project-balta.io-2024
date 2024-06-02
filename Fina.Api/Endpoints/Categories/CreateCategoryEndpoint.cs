using Fina.Api.Common.Api;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Fina.Shared.Responses;

namespace Fina.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/", HandleAsync)
        .WithName("Categories: Create")
        .WithSummary("Cria uma nova categoria")
        .WithDescription("Cria uma nova categoria")
        .WithOrder(1)
        .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, CreateCategoryRequest request)
    {
        request.UserId = ApiConfiguration.UserId;
        var result = await handler.CreatedAsync(request);
        return result.IsSuccess 
                ? TypedResults.Created($"v1/categories/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result);        
    }
}
