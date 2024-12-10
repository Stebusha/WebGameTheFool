using Microsoft.EntityFrameworkCore;
using BlazorCardGame.DataMangerAPI.Entities;

namespace BlazorCardGame.Contexts;

public class FoolGameContext : DbContext
{
    public FoolGameContext(DbContextOptions<FoolGameContext> options) : base(options)
    {

    }
    public DbSet<ApplicationUser> Users { get; set; } = null!;
    public DbSet<FoolGameState> LastGames { get; set; } = null!;
    public DbSet<FoolGameScores> Scores { get; set; } = null!;
}