using EntityFrameworkIncludeBuilder.UnitTests.Context;
using EntityFrameworkIncludeBuilder.UnitTests.Models;
using EntityFrameworkIncludeBuilder.UnitTests.Seed;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EntityFrameworkIncludeBuilder.UnitTests
{
    [TestFixture]
    internal class ILoadCollectionExtensionsTest
    {
        [SetUp]
        public void BeforeTest()
        {
            AddSeedData();
        }

        [TearDown]
        public void AfterTets()
        {
            ClearDbContext();
        }

        [Test]
        public void TestWithoudInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                var users = mockDbContext.Users.ToList();

                Assert.IsNotEmpty(users);
                foreach (var user in users)
                {
                    Assert.IsEmpty(user.MyPosts);
                    Assert.IsEmpty(user.Favorites);
                }
            }
        }

        [Test]
        public void TestOneInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                var users = mockDbContext.Users
                    .Include(x => x.Load(u => u.MyPosts))
                    .ToList();

                Assert.IsNotEmpty(users);

                var writters = users.Where(x => x.MyPosts.Any());
                Assert.IsNotEmpty(writters);

                foreach (var user in users)
                    Assert.IsEmpty(user.Favorites);
            }
        }

        [Test]
        public void TestSeveralInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                var users = mockDbContext.Users
                    .Include(x => x
                        .Load(u => u.MyPosts)
                        .Load(u => u.Favorites))
                    .ToList();

                Assert.IsNotEmpty(users);

                var writters = users.Where(x => x.MyPosts.Any());
                Assert.IsNotEmpty(writters);

                foreach (var user in users)
                    Assert.IsNotEmpty(user.Favorites);
            }
        }

        [Test]
        public void TestOneIncludeSeveralThenInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                var users = mockDbContext.Users
                    .Include((IEnumerable<User> x) => x
                        .Load(u => u.Favorites)
                        .ThenLoad(f => f.Post)
                        .ThenLoad(p => p.CreatedBy))
                    .ToList();

                Assert.IsNotEmpty(users);

                var writters = users.Where(x => x.MyPosts.Any());
                Assert.IsNotEmpty(writters);

                foreach (var user in users)
                {
                    Assert.IsNotEmpty(user.Favorites);
                    foreach (var favorite in user.Favorites)
                    {
                        Assert.IsNotNull(favorite.Post?.CreatedBy);
                    }
                }
            }
        }

        [Test]
        public void TestIncorrectInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                Assert.Throws<InvalidExpressionException>(() =>
                {
                    _ = mockDbContext.Users
                        .Include(x => x.Load(u => u))
                        .ToList();
                });
            }
        }

        [Test]
        public void TestIncorrectThenInclude()
        {
            using (var mockDbContext = CreateContext())
            {
                Assert.Throws<InvalidExpressionException>(() =>
                {
                    _ = mockDbContext.Users
                        .Include((IEnumerable<User> x) => x
                            .Load(u => u.Favorites)
                            .ThenLoad(f => f.Post)
                            .ThenLoad(p => p))
                        .ToList();
                });
            }
        }

        #region Private methods

        private void AddSeedData()
        {
            using (var mockDbContext = CreateContext())
            {
                var userSeed = new UserSeed(mockDbContext);
                var postSeed = new PostSeed(mockDbContext);
                var favoriteSeed = new FavoriteSeed(mockDbContext);

                userSeed.AddDefaultSeed();
                postSeed.AddDefaultSeed();
                favoriteSeed.AddDefaultSeed();
            }
        }

        private MockDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<MockDbContext>()
                .UseInMemoryDatabase("MockDb")
                .Options;
            return new MockDbContext(options);
        }

        private void ClearDbContext()
        {
            using (var mockDbContext = CreateContext())
            {
                mockDbContext.Favorites.RemoveRange(mockDbContext.Favorites);
                mockDbContext.Posts.RemoveRange(mockDbContext.Posts);
                mockDbContext.Users.RemoveRange(mockDbContext.Users);
                mockDbContext.SaveChanges();
            }
        }

        #endregion
    }
}
