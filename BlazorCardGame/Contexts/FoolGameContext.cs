using Microsoft.EntityFrameworkCore;
using BlazorCardGame.Entities;

namespace BlazorCardGame.Contexts;

public class FoolGameContext : DbContext
{
    public FoolGameContext(DbContextOptions<FoolGameContext> options) : base(options)
    {
        
    }
    public DbSet<ApplicationUser> Users { get; set; } = null!;
    public DbSet<FoolGameScore> Scores { get; set; } = null!;
    public DbSet<PlayerInfo> Players { get; set; } = null!;
}