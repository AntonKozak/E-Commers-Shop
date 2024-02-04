using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification;

public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpecification()
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
    // Replacing: Expression<Func<T, bool>> criteria to this: (int id) : base(x => x.Id == id)
    //  : base(x => x.Id == id) Find the product by id.
    public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}
