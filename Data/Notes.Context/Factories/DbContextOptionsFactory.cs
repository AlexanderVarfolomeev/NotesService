using Microsoft.EntityFrameworkCore;
using Notes.Context.Context;

namespace Notes.Context.Factories;

public class DbContextOptionsFactory
{
    public static DbContextOptions<MainDbContext> Create(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<MainDbContext>();
        Configure(connectionString).Invoke(builder);
        return builder.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connectionString)
    {
        return (contextOptionsBuilder) => contextOptionsBuilder.UseSqlServer(connectionString);
    }
}