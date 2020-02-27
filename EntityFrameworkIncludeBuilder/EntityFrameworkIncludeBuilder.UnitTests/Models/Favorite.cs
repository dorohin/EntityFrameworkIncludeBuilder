using System;

namespace EntityFrameworkIncludeBuilder.UnitTests.Models
{
    internal class Favorite
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
