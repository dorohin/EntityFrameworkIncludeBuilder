using System.Collections.Generic;

namespace EntityFrameworkIncludeBuilder
{
    /// <summary>
    /// Supports Load/ThenLoad chaining operators.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    public interface ILoadCollection<out TEntity, out TProperty> : IEnumerable<TEntity>
        where TEntity : class
        where TProperty : class
    {
    }
}
