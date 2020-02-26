using System.Collections.Generic;

namespace EntityFrameworkIncludeBuilder
{
    public interface ILoadCollection<out TEntity, out TProperty> : IEnumerable<TEntity>
        where TEntity : class
        where TProperty : class
    {
    }
}
