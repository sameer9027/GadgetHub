using GadgetHub.Web.MVC.ViewModels.Categories;

namespace GadgetHub.Web.MVC.ViewModels.Products;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class ProductFormViewModel
{
    public int Id { get; set; } // 0 for create, >0 for edit
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public List<CategoryViewModel> Categories { get; set; } = new();

    // Helper property to check if it's create mode
    public bool IsCreate => Id == 0;
}