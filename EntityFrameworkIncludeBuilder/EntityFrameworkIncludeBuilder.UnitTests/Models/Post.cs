using System;
using System.Collections.Generic;

namespace EntityFrameworkIncludeBuilder.UnitTests.Models
{
    internal class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public Guid CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
