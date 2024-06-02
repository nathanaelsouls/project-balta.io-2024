using Fina.Api.Data;
using Fina.Shared.Common;
using Fina.Shared.Enums;
using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Transactions;
using Fina.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreatedAsync(CreateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        var transaction = new Transaction()
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.Now,
            Amount = request.Amount,
            PaidOrReceivedAt = request.PaidOrReceivedAt,
            Title = request.Title,
            Type = request.Type,
        };

        try
        {            
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, StatusCodes.Status201Created, "Transação Criada com sucesso");
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "Não foi possível criar uma Transação");
            throw;
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, StatusCodes.Status404NotFound, "Transação não encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "Não foi excluída a Transação");
            throw;
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transacao = await context.Transactions
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transacao is null
                   ? new Response<Transaction?>(null, StatusCodes.Status404NotFound, "Transação não encontrada!")
                   : new Response<Transaction?>(transacao);
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "Não foi possível recuperar a Categoria");
            throw;
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??=  DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();

            var query = context.Transactions
                        .AsNoTracking()
                        .Where(x => x.PaidOrReceivedAt >= request.StartDate
                                    && x.PaidOrReceivedAt <= request.EndDate
                                    && x.UserId == request.UserId)
                        .OrderBy(x => x.PaidOrReceivedAt);

            var transacao = await query
                             .Skip((request.PageNumber - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(transacao, count, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new PagedResponse<List<Transaction>?> (null, StatusCodes.Status500InternalServerError, "Não foi possível recuperar a Categoria");
            throw;
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is { Type: ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;        

        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, StatusCodes.Status404NotFound, "Transação não encontrada");

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação alterada com sucesso");
        }
        catch (Exception ex)
        {
            // Serilog, Opentelem
            Console.WriteLine(ex.Message);
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "Não foi possível alterar a Transação");
            throw;
        }
    }
}
