using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Infrastructure.Persistencia
{
    public class BancoDbContextFactory : IDesignTimeDbContextFactory<BancoDbContext>
    {

        public BancoDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()                
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<BancoDbContext>();


            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    "Cadena conexion 'Default' no encontrada.");

            optionsBuilder.UseNpgsql(
                configuration.GetConnectionString("Default"));

            return new BancoDbContext(optionsBuilder.Options);
        }

    }
}
