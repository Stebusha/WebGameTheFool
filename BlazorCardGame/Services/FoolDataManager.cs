using BlazorCardGame.Contexts;
using BlazorCardGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorCardGame.Services;

public class FoolDataManager
{
    private readonly FoolGameContext context;

    //get context db
    public FoolDataManager(IDbContextFactory<FoolGameContext> factory)
    {
        context = factory.CreateDbContext();
    }

    //add to database
    public async Task AddScoreAsync(FoolGameScore score)
    {
        context.Scores.Add(score);
        await context.SaveChangesAsync();
    }

    public async Task AddScoresAsync(FoolGameScore score1, FoolGameScore score2)
    {
        context.Scores.AddRange(score1, score2);
        await context.SaveChangesAsync();
    }

    public async Task AddUserAsync(ApplicationUser user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task AddPlayerAsync(PlayerInfo player)
    {
        context.Players.Add(player);
        await context.SaveChangesAsync();
    }

    public async Task AddPlayersAsync(PlayerInfo player1, PlayerInfo player2)
    {
        context.Players.AddRange(player1, player2);
        await context.SaveChangesAsync();
    }

    public async Task AddGameAsync(FoolGame game)
    {
        context.Games.Add(game);
        await context.SaveChangesAsync();
    }

    //get all from database
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }

    public async Task<List<PlayerInfo>> GetAllPlayersAsync()
    {
        var players = await context.Players.ToListAsync();
        return players;
    }

    public async Task<List<FoolGameScore>> GetAllScoresAsync()
    {
        var scores = await context.Scores.ToListAsync();
        return scores;
    }

    public async Task<ApplicationUser?> GetUserByLoginAsync(string login)
    {
        var user = await context.Users.FirstOrDefaultAsync(e => e.Login == login);
        return user;
    }

    public async Task<PlayerInfo?> GetPlayerByNameAsync(string name)
    {
        var player = await context.Players.FirstOrDefaultAsync(e => e.Name == name);
        return player;
    }

    public async Task<FoolGameScore?> GetScoreByNameAsync(string name)
    {
        var score = await context.Scores.FirstOrDefaultAsync(e => e.PlayerInfoName == name);
        return score;
    }

    public async Task<FoolGame?> GetFoolGameByKeyAsync(string playerName)
    {
        var game = await context.Games.FirstOrDefaultAsync(g => g.PlayerInfoName == playerName);
        return game;
    }

    //update table in database
    public async Task UpdateUserAsync(ApplicationUser user)
    {
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdatePlayerAsync(PlayerInfo player)
    {
        context.Entry(player).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
    public async Task UpdateScoreAsync(FoolGameScore score)
    {
        context.Entry(score).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdateScoresAsync(FoolGameScore score1, FoolGameScore score2)
    {
        context.Entry(score1).State = EntityState.Modified;
        context.Entry(score2).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task UpdateGameAsync(FoolGame game)
    {
        context.Entry(game).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    //delete 
    public async Task DeleteUserByLoginAsync(string login)
    {
        var user = await GetUserByLoginAsync(login);

        if (user is not null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteGameByKeyAsync(string playerName)
    {
        var game = await GetFoolGameByKeyAsync(playerName);

        if(game is not null)
        {
            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }
    }
}