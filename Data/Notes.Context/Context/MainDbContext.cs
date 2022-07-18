using Microsoft.EntityFrameworkCore;
using Notes.Entities;

namespace Notes.Context.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) {}

    public DbSet<Note> Notes { get; set; }

}