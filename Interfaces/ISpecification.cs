using System.Linq.Expressions;

namespace eShop.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Predicate { get; }
    Expression<Func<T, object>>? OrderByAscending { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }

}
