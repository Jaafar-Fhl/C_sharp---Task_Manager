public class database_context : DbContext
{
    public DbSet<task> tasks { get; set; };
    
}