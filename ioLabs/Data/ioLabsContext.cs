using ioLabs.Models;
using Microsoft.EntityFrameworkCore;

namespace ioLabs.Data
{
    public class IoLabsContext : DbContext
    {
        public IoLabsContext(DbContextOptions<IoLabsContext> options)
            : base(options)
        {
        }

        public DbSet<DataModel> DataModels { get; set; }
    }
}
