using Fina.Shared.Handlers;
using Fina.Shared.Models;
using Fina.Shared.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Fina.Web.Pages.Categories;

public partial class GetAllCategoriesPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public bool hover = true;
    public List<Category> Categories { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IDialogService Dialog { get; set; } = null!;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
                Categories = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);        
        }
        finally 
        { 
            IsBusy = false;
        }
    }

    #endregion

    #region Métodos

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await Dialog.ShowMessageBox("Atenção", 
                                    $"Ao prosseguir a categoria {title} será removida, deseja continuar?",
                                    cancelText: "Cancelar",
                                    yesText: "Excluir");

        if(result is true)
        {
            await OnDeleteAsync(id, title);
        }

        StateHasChanged();
    }

    public async void OnUpdateButtonClickedAsync(long id, string title, string description)
    {
        var result = await Dialog.ShowMessageBox("Atenção",
                                    $"Ao prosseguir a categoria {title} será atualizada, deseja continuar?",
                                    cancelText: "Cancelar",
                                    yesText: "Excluir");
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            var request = new DeleteCategoryRequest
            {
                Id = id,
            };

            await Handler.DeleteAsync(request);
            Categories.RemoveAll(x => x.Id == id);

            Snackbar.Add($"Categoria {title} removida", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            throw;
        }
    }

    public async Task OnUpdateAsync(long id, string title, string description)
    {
        try
        {
            var request = new UpdateCategoryRequest
            {
                Id = id,
                Title = title,
                Description = description,
            };

            await Handler.UpdateAsync(request);            

            Snackbar.Add($"Categoria {title} Atualizada", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
            throw;
        }
    }

    #endregion
}
