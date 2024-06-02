using Fina.Api.Data;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Fina.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreatedAsync(CreateCategoryRequest request)
    {
        try
        {
            var category = new Category()
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
            };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, StatusCodes.Status201Created, "Categoria Criada com sucesso");
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "Não foi possível criar uma Categoria");
            throw;
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)
                return new Response<Category?>(null, StatusCodes.Status404NotFound, "Categoria não encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria excluída com sucesso!");
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "Não foi possível excluir a Categoria");
            throw;
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoryRequest request)
    {
        try
        {
            var query = context.Categories
                            .AsNoTracking()
                            .Where(x => x.UserId == request.UserId)
                            .OrderBy(x => x.Title);

            var categories = await query
                             .Skip((request.PageNumber-1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Category>?>(categories, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new PagedResponse<List<Category>?>(null, StatusCodes.Status500InternalServerError, "Não foi possível recuperar a Categoria");
            throw;
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category is null 
                   ? new Response<Category?>(null, StatusCodes.Status404NotFound, "Categoria não encontrada!")
                   : new Response<Category?>(category);
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "Não foi possível recuperar a Categoria");
            throw;
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category is null)             
                return new Response<Category?>(null, StatusCodes.Status404NotFound, "Categoria não encontrada");

            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message:"Categoria alterada com sucesso");
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "Não foi possível criar uma Categoria");
            throw;
        }
    }
}
