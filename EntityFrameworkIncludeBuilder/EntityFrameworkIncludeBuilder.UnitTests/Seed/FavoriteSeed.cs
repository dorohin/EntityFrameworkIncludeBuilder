using EntityFrameworkIncludeBuilder.UnitTests.Context;
using EntityFrameworkIncludeBuilder.UnitTests.Models;
using System;
using System.Linq;

namespace EntityFrameworkIncludeBuilder.UnitTests.Seed
{
    internal class FavoriteSeed : BaseSeed<Favorite>
    {
        public FavoriteSeed(MockDbContext dbContext) : base(dbContext)
        {
        }

        internal override void AddDefaultSeed()
        {
            var users = _dbContext.Users.ToList();
            var posts = _dbContext.Posts.ToList();
            var favorites = users.Select(user => new Favorite
            {
                UserId = user.Id,
                PostId = posts.OrderBy(x => Guid.NewGuid()).First().Id
            });

            foreach (var favorite in favorites)
            {
                Add(favorite);
            }
        }
    }
}
