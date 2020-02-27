using EntityFrameworkIncludeBuilder.UnitTests.Context;
using EntityFrameworkIncludeBuilder.UnitTests.Models;
using System;
using System.Linq;

namespace EntityFrameworkIncludeBuilder.UnitTests.Seed
{
    internal class PostSeed : BaseSeed<Post>
    {
        public PostSeed(MockDbContext dbContext) : base(dbContext)
        {
        }

        internal override void AddDefaultSeed()
        {
            var users = _dbContext.Users.ToList();
            var post1 = new Post
            {
                Title = "Mission 01: Rip & Tear",
                CreatedById = users.OrderBy(x => Guid.NewGuid()).First().Id
            };
            var post2 = new Post
            {
                Title = "Mission 02: Know Your Enemy.",
                CreatedById = users.OrderBy(x => Guid.NewGuid()).First().Id
            };
            var post3 = new Post
            {
                Title = "Mission 03: Meltdown.",
                CreatedById = users.OrderBy(x => Guid.NewGuid()).First().Id
            };
            var post4 = new Post
            {
                Title = "Mission 04: Beginning of the End.",
                CreatedById = users.OrderBy(x => Guid.NewGuid()).First().Id
            };
            var post5 = new Post
            {
                Title = "Mission 05: Argent Tower.",
                CreatedById = users.OrderBy(x => Guid.NewGuid()).First().Id
            };

            Add(post1);
            Add(post2);
            Add(post3);
            Add(post4);
            Add(post5);
        }
    }
}
