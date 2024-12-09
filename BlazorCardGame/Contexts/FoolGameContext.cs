using Microsoft.EntityFrameworkCore;
using BlazorCardGame.DataMangerAPI.Entities;

namespace BlazorCardGame.DataMangerAPI.Contexts;

public class FoolGameContext : DbContext
{
    public FoolGameContext(DbContextOptions<FoolGameContext> options) : base(options)
    {

    }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<FoolGameState> LastGames { get; set; }
    public DbSet<FoolGameScores> Scores { get; set; }
}