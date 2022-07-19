using Microsoft.EntityFrameworkCore;
using Notes.Entities;

namespace Notes.Context.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<TaskType> Tasks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>().ToTable("notes");
        modelBuilder.Entity<TaskType>().ToTable("taskTypes");

        modelBuilder.Entity<Note>().HasOne(x => x.Type).WithMany(x => x.Notes).HasForeignKey(x => x.TaskTypeId);
        modelBuilder.Entity<TaskType>().HasMany(x => x.Notes).WithOne(x => x.Type).HasForeignKey(x => x.TaskTypeId);
        
    }
}