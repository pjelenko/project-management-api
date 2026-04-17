using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;

namespace ProjectManagement.Repositories;

public class PMDbContext(DbContextOptions<PMDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Models.Task> Tasks => Set<Models.Task>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}
