using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GadgetHub.Infrastructure.Data
{
    public class MyDBcontextFactory : IDesignTimeDbContextFactory<MyDBcontext>
    {
        public MyDBcontext CreateDbContext(string[] args)
        {
            // ✅ Hardcoded connection string for design-time only
            var connectionString = "Server=SAMEER\\SQLEXPRESS;Database=GadgetHub_DB;Trusted_Connection=True;TrustServerCertificate=True;";


            var optionsBuilder = new DbContextOptionsBuilder<MyDBcontext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MyDBcontext(optionsBuilder.Options);
        }
    }
}