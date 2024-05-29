using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Fina.Shared.Responses;

namespace Fina.Shared.Handlers;
public interface ICategoryHandler
{
    Task<Response<Category?>> CreatedAsync(CreateCategoryRequest request);
    Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request);
    Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request);
    Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request);
    Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request);
}
