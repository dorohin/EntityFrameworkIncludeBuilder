using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EntityFrameworkIncludeBuilder.UnitTests")]

namespace EntityFrameworkIncludeBuilder
{
    internal class LoadCollection<TEntity, TProperty> :
        ILoadCollection<TEntity, TProperty>
        where TEntity : class
        where TProperty : class
    {
        private readonly IEnumerable<TEntity> _source;
        private readonly ICollection<string> _loadings;

        public LoadCollection(IEnumerable<TEntity> source, ICollection<string> involves)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _loadings = involves ?? throw new ArgumentNullException(nameof(involves));
        }

        public IEnumerator<TEntity> GetEnumerator() => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();

        internal ICollection<string> GetLoadings() => _loadings;
    }
}
