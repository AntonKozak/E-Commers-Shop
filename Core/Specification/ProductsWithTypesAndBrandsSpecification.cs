using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    // Sorting by "priceAsc", "priceDesc"
    //filtering by brandId and / or typeId
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams) : base(x => 
    (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) && // Search by name
        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && // Search by brandId
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) // Search by typeId
    )
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderBy(x => x.Name);
        
        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
    // Replacing: Expression<Func<T, bool>> criteria to this: (int id) : base(x => x.Id == id)
    //  : base(x => x.Id == id) Find the product by id.
    public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}
