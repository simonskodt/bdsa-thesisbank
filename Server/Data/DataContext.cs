using Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class DataContext : DbContext {

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Thesis> ThesesPosts { get; set; }
    
}