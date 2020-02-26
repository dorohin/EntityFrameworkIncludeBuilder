using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkIncludeBuilder
{
    public static class ILoadCollectionExtensions
    {
        #region Public methods

        public static ILoadCollection<TEntity, TProperty> Load<TEntity, TProperty>(
            this IEnumerable<TEntity> entities,
            Expression<Func<TEntity, TProperty>> expression)
            where TEntity : class
            where TProperty : class
        {
            var includes = new List<string>();
            if (entities is ILoadCollection<TEntity, object> prevInclude)
                includes.AddRange(prevInclude.Build());

            includes.Add(GetInvolvedPropertyName(expression));
            return new LoadCollection<TEntity, TProperty>(entities, includes);
        }

        public static ILoadCollection<TEntity, TProperty> ThenLoad<TEntity, TPreviousProperty, TProperty>(
            this ILoadCollection<TEntity, ICollection<TPreviousProperty>> entities,
            Expression<Func<TPreviousProperty, TProperty>> expression)
            where TEntity : class
            where TProperty : class
        {
            var includes = new List<string>(entities.Build());
            includes[includes.Count - 1] = $"{includes.Last()}.{GetInvolvedPropertyName(expression)}";
            return new LoadCollection<TEntity, TProperty>(entities, includes);
        }

        public static ILoadCollection<TEntity, TProperty> ThenLoad<TEntity, TPreviousProperty, TProperty>(
            this ILoadCollection<TEntity, TPreviousProperty> entities,
            Expression<Func<TPreviousProperty, TProperty>> expression)
            where TEntity : class
            where TPreviousProperty : class
            where TProperty : class
        {
            var includes = new List<string>(entities.Build());
            includes[includes.Count - 1] = $"{includes.Last()}.{GetInvolvedPropertyName(expression)}";
            return new LoadCollection<TEntity, TProperty>(entities, includes);
        }

        #endregion

        #region Internal methods

        internal static IEnumerable<string> Build<TEntity, TProperty>(
            this ILoadCollection<TEntity, TProperty> collection)
            where TEntity : class
            where TProperty : class
        {
            var type = collection.GetType();
            var method = type.GetMethod(nameof(LoadCollection<TEntity, TProperty>.GetLoadings),
                             BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new MethodAccessException();

            var loadings = method.Invoke(collection, null) as List<string>;
            return loadings;
        }

        #endregion

        #region Private methods

        private static string GetInvolvedPropertyName<TEntity, TProperty>(
            Expression<Func<TEntity, TProperty>> expression)
        {
            var body = expression.Body.ToString();
            var index = body.IndexOf('.');

            if (index < 0)
                throw new InvalidExpressionException($"Parameter {nameof(expression)} is invalid.");

            return body.Remove(0, index + 1);
        }

        #endregion
    }
}
