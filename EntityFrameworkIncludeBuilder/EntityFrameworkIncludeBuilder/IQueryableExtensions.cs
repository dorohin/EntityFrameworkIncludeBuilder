using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkIncludeBuilder
{
    /// <summary>
    /// Extension methods for LINQ.
    /// </summary>
    public static class IQueryableExtensions
    {
        #region Public methods

        /// <summary>
        /// Provide ability to include all loaded navigation properties via <see cref="ILoadCollection{TEntity,TProperty}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="query">The source query.</param>
        /// <param name="expression">A lambda expression representing the navigation property to be loaded (<c>x =&gt; x.Property1</c>).</param>
        /// <returns>A new query with the related data included.</returns>
        public static IQueryable<TEntity> Include<TEntity, TProperty>(this IQueryable<TEntity> query,
            Expression<Func<IEnumerable<TEntity>, ILoadCollection<TEntity, TProperty>>> expression)
            where TEntity : class
            where TProperty : class
        {
            var navigationProperties = expression.Compile().Invoke(null).Build();
            query = navigationProperties
                .Aggregate(query,
                    (current, propertySelector) => current.Include(propertySelector));

            return query;
        }

        #endregion
    }
}
