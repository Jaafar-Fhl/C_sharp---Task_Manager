using TaskManager;
using Microsoft.EntityFrameworkCore;

public class database_context : DbContext
{
    public DbSet<taskItem> tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    	optionsBuilder.UseSqlite("Data Source=tasks.db");
    }
}