using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Auth.DataAccess.AppDbContexts;

public class DesignTimeDbContextFacrory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("host=localhost; port=5432; username=postgres; password=bekzod28072009; Database=EnglishUp;");

        return new AppDbContext(optionsBuilder.Options);
    }
}
