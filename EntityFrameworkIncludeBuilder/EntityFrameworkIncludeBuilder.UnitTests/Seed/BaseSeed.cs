using EntityFrameworkIncludeBuilder.UnitTests.Context;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkIncludeBuilder.UnitTests.Seed
{
    internal abstract class BaseSeed<TEntity>
    where TEntity : class
    {
        protected readonly MockDbContext _dbContext;
        protected readonly DbSet<TEntity> _table;

        protected BaseSeed(MockDbContext dbContext)
        {
            _dbContext = dbContext;
            _table = _dbContext.Set<TEntity>();
        }

        internal abstract void AddDefaultSeed();

        internal void Add(TEntity entity)
        {
            _table.Add(entity);
            _dbContext.SaveChanges();
        }
    }
}
