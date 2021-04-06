using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Contexts
{
    public class ContextFactory : IDesignTimeDbContextFactory<SQLiteDBContext>
    {
        public SQLiteDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SQLiteDBContext>();
            optionsBuilder.UseSqlite("Data Source=ArandaDB.db;");

            return new SQLiteDBContext(optionsBuilder.Options);
        }
    }
}
