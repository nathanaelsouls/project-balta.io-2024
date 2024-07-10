using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Fina.Shared.Responses;
using System.Net.Http.Json;

namespace Fina.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(WebConfiguration.HttpClientName);

    public async Task<Response<Category?>> CreatedAsync(CreateCategoryRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao Criar Categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _client.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao Excluir a Categoria");
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        return await _client.GetFromJsonAsync<PagedResponse<List<Category>?>>("v1/categories")
               ?? new PagedResponse<List<Category>?>(null, 400, "Falha ao Buscar Categorias");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        return await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
               ?? new Response<Category?>(null, 400, "Falha ao Buscar Categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao Editar a Categoria");
    }
}
