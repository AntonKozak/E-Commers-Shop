
using System.Linq.Expressions;

namespace Core.Specification;

public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }


    /// Gets or sets the criteria expression to filter the data.
    public System.Linq.Expressions.Expression<Func<T, bool>> Criteria { get; }

    /// Gets or sets the list of expressions to include related entities.
    public List<System.Linq.Expressions.Expression<Func<T, object>>> Includes { get; } =
    new List<Expression<Func<T, object>>>();

    // The lambda expression should take a parameter of type T and return a property or expression
    // based on which the sorting should be performed. The return type is object, allowing flexibility
    // in specifying properties of various types for ordering.
    public Expression<Func<T, object>> OrderBy { get; private set; }

    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    /// Adds an include expression to the list of includes.
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }
    // Sets the order criteria for ascending sorting of a collection of objects of type T.
    //orderByExpression or orderByDescExpression Lambda expression specifying the property or expression for sorting.
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    // Sets the pagination criteria for the query.
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    
}
