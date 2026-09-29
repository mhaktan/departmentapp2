using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DepartmentApp2.EntityFrameworkCore
{
    public class DepartmentApp2DbContextFactory : IDesignTimeDbContextFactory<DepartmentApp2DbContext>
    {
        public DepartmentApp2DbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DepartmentApp2.Web.Host"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connStr = configuration.GetConnectionString("Default");
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connStr);
            return new DepartmentApp2DbContext(builder.Options);
        }
    }
}
