using BlazorCardGame.Entities;

namespace BlazorCardGame.Models;

public record class DatabaseExportData
{
    public List<ApplicationUser> Users { get; set; } = [];
    public List<PlayerInfo> Players { get; set; } = [];
    public List<FoolGameScore> Scores { get; set; } = [];
}