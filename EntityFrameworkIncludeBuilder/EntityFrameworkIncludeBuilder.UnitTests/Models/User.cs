using System;
using System.Collections.Generic;

namespace EntityFrameworkIncludeBuilder.UnitTests.Models
{
    internal class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<Post> MyPosts { get; set; } = new List<Post>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
