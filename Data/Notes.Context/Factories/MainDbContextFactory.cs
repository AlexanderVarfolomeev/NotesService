using Microsoft.EntityFrameworkCore;
using Notes.Context.Context;

namespace Notes.Context.Factories;

public class MainDbContextFactory
{
    private readonly DbContextOptions<MainDbContext> opts;

    public MainDbContextFactory(DbContextOptions<MainDbContext> opts)
    {
        this.opts = opts;
    }

    public MainDbContext Create()
    {
        return new MainDbContext(opts);
    }
}