using Microsoft.EntityFrameworkCore;

class NoteDbContext : DbContext
{
    public DbSet<User> Users
    {
        get { return Set<User>(); }
    }

    public DbSet<Note> Notes
    {
        get { return Set<Note>(); }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=milestone04-data.db;Foreign Keys=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => user.NormalizedUsername)
            .IsUnique();
    }
}
