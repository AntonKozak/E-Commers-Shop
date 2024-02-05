using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    // Sorting by "priceAsc", "priceDesc"
    //filtering by brandId and / or typeId
    public ProductsWithTypesAndBrandsSpecification(string sort, int? brandId, int? typeId) : base(x => 
        (!brandId.HasValue || x.ProductBrandId == brandId) &&
        (!typeId.HasValue || x.ProductTypeId == typeId)
    )
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
        AddOrderBy(x => x.Name);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
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
