
using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Config;

    /// Gets the query with applied specifications.
public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    /// name="inputQuery">The input query to apply specifications on
    /// "spec" The specification to apply.
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        //gonna be iqueryble of <Product> if it is ProductSpecification
        var query = inputQuery;

        if (spec.Criteria != null)
        {
            //Give me a Product(query) where a Product has specifided with criteria (Id, Name, City, Country, Category ...)
            query = query.Where(spec.Criteria);//where p => p.ProductTypeId == typeId
        }
        // Apply includes if specified
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        /// returns The query with applied specifications
        return query;
    }
}
