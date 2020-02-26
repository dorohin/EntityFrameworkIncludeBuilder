using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFrameworkIncludeBuilder
{
    public static class IQueryableExtensions
    {
        #region Public methods

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
