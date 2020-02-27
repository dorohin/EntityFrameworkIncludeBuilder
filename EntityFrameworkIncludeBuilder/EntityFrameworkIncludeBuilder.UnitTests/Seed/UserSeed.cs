using EntityFrameworkIncludeBuilder.UnitTests.Context;
using EntityFrameworkIncludeBuilder.UnitTests.Models;

namespace EntityFrameworkIncludeBuilder.UnitTests.Seed
{
    internal class UserSeed : BaseSeed<User>
    {
        public UserSeed(MockDbContext dbContext) : base(dbContext)
        {
        }

        internal override void AddDefaultSeed()
        {
            var user1 = new User
            {
                FirstName = "Samuel",
                LastName = "Hayden"
            };
            var user2 = new User
            {
                FirstName = "Doom",
                LastName = "Slayer"
            };
            var user3 = new User
            {
                FirstName = "Olivia",
                LastName = "Pierce"
            };

            Add(user1);
            Add(user2);
            Add(user3);
        }
    }
}
