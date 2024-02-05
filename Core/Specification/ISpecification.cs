
using System.Linq.Expressions;

namespace Core.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }

    // OrderBy and OrderByDescending use TKey for type safety, ability to catch type-related errors at compile-time
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }

    // Pagination, take 5, skip 5 and take 5 more and skip 10 and so on
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }


}
