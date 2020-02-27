using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkIncludeBuilder
{
    /// <summary>
    /// Extension methods for LINQ.
    /// </summary>
    public static class ILoadCollectionExtensions
    {
        #region Public methods

        /// <summary>
        /// Provides ability to specify which navigation properties should be load. If you wish to load additional types based on the navigation
        /// properties of the type being loaded, then chain a call to <see cref="ThenLoad{TEntity,TPreviousProperty,TProperty}(EntityFrameworkIncludeBuilder.ILoadCollection{TEntity,System.Collections.Generic.ICollection{TPreviousProperty}},System.Linq.Expressions.Expression{System.Func{TPreviousProperty,TProperty}})"/> after this call.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be loaded.</typeparam>
        /// <param name="entities">The source collection.</param>
        /// <param name="expression">A lambda expression representing the navigation property to be loaded (<c>x =&gt; x.Property1</c>).</param>
        /// <returns>A new query with the related data loaded.</returns>
        public static ILoadCollection<TEntity, TProperty> Load<TEntity, TProperty>(
            this IEnumerable<TEntity> entities,
            Expression<Func<TEntity, TProperty>> expression)
            where TEntity : class
            where TProperty : class
        {
            var includes = new List<string>();
            if (entities is ILoadCollection<TEntity, object> prevInclude)
                includes.AddRange(prevInclude.Build());

            includes.Add(GetLoadedPropertyName(expression));
            return new LoadCollection<TEntity, TProperty>(entities, includes);
        }

        /// <summary>
        /// Specifies additional related data to be further loaded based on a related type that was just loaded.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just loaded.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be loaded.</typeparam>
        /// <param name="collection">The source collection.</param>
        /// <param name="expression">A lambda expression representing the navigation property to be loaded (<c>x =&gt; x.Property1</c>).</param>
        /// <returns>A new query with the related data loaded.</returns>
        public static ILoadCollection<TEntity, TProperty> ThenLoad<TEntity, TPreviousProperty, TProperty>(
            this ILoadCollection<TEntity, ICollection<TPreviousProperty>> collection,
            Expression<Func<TPreviousProperty, TProperty>> expression)
            where TEntity : class
            where TProperty : class
        {
            var includes = new List<string>(collection.Build());
            includes[includes.Count - 1] = $"{includes.Last()}.{GetLoadedPropertyName(expression)}";
            return new LoadCollection<TEntity, TProperty>(collection, includes);
        }

        /// <summary>
        /// Specifies additional related data to be further loaded based on a related type that was just loaded.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPreviousProperty">The type of the entity that was just loaded.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be loaded.</typeparam>
        /// <param name="collection">The source collection.</param>
        /// <param name="expression">A lambda expression representing the navigation property to be loaded (<c>x =&gt; x.Property1</c>).</param>
        /// <returns>A new query with the related data loaded.</returns>
        public static ILoadCollection<TEntity, TProperty> ThenLoad<TEntity, TPreviousProperty, TProperty>(
            this ILoadCollection<TEntity, TPreviousProperty> collection,
            Expression<Func<TPreviousProperty, TProperty>> expression)
            where TEntity : class
            where TPreviousProperty : class
            where TProperty : class
        {
            var includes = new List<string>(collection.Build());
            includes[includes.Count - 1] = $"{includes.Last()}.{GetLoadedPropertyName(expression)}";
            return new LoadCollection<TEntity, TProperty>(collection, includes);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Build path of specified navigation properties to load.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be loaded.</typeparam>
        /// <param name="collection">The source collection.</param>
        /// <returns>List of specified navigation properties to load.</returns>
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

        /// <summary>
        /// Gets name of user specified navigation property.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be loaded.</typeparam>
        /// <param name="expression">A lambda expression representing the navigation property to be loaded (<c>x =&gt; x.Property1</c>).</param>
        /// <returns>Name of navigation property.</returns>
        private static string GetLoadedPropertyName<TEntity, TProperty>(
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
