
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

    /// Adds an include expression to the list of includes.
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

}
