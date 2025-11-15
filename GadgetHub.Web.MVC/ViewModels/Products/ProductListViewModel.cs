using GadgetHub.Web.MVC.ViewModels.Categories;

namespace GadgetHub.Web.MVC.ViewModels.Products;

public class ProductListViewModel
{
    public IEnumerable<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
    public ProductFilterViewModel Filter { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
    public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
}

public class ProductFilterViewModel
{
    public int? CategoryId { get; set; }
    public string? SortBy { get; set; } = "name";
    public bool? SortAsc { get; set; } = true;
    public string? SearchTerm { get; set; }
}

public class PaginationViewModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public int StartItem => ((Page - 1) * PageSize) + 1;
    public int EndItem => Math.Min(Page * PageSize, TotalCount);
}