
using Microsoft.EntityFrameworkCore;

namespace ACE.Database.Models.Shard
{
    public class ShardDbContextWithFinalize : ShardDbContext
    {
        public ShardDbContextWithFinalize()
        {
        }

        public ShardDbContextWithFinalize(DbContextOptions<ShardDbContext> options)
            : base(options)
        {
        }

        ~ShardDbContextWithFinalize()
        {
            Dispose();
        }
    }
}
