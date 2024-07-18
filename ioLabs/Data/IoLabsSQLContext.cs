using ioLabs.Models;
using Microsoft.EntityFrameworkCore;

namespace ioLabs.Data
{
    public class IoLabsSQLContext : DbContext
    {
        public IoLabsSQLContext(DbContextOptions<IoLabsSQLContext> options)
            : base(options)
        {
        }

        public DbSet<DataModel> DataModels { get; set; }
    }
}
