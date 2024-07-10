using System.ComponentModel.DataAnnotations;

namespace Fina.Shared.Requests.Categories;

public class CreateCategoryRequest : Request
{
    [Required(ErrorMessage = "Título inválido")]
    [MaxLength(80, ErrorMessage ="O titulo deve conter até 80 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição inválida")]
    [MinLength(1, ErrorMessage ="Descriçã")]
    public string Description { get; set; } = string.Empty;
}
