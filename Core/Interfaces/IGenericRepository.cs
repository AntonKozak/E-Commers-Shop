
using Core.Entities;
using Core.Specification;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    
        /// Gets a single entity based on the provided specification.
        /// name="spec" The specification to apply(by id, name, city or country, category ...).
    Task<T> GetEntityWithSpec(ISpecification<T> spec);
    //return list of entities based on the provided specification.(orderings, pagination, filtering ...)
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    //return the count of entities based on the provided specification.
    Task<int> CountAsync(ISpecification<T> spec);
}
