using Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;


public static class TestDbContextFactory
{
    public static BancoDbContext Create()
    {
        var options = new DbContextOptionsBuilder<BancoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BancoDbContext(options);
    }
}