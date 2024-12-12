using BlazorCardGame.Contexts;
using BlazorCardGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorCardGame.Services;

public class DataManager
{
    private readonly FoolGameContext context;

    //get context db
    public DataManager(IDbContextFactory<FoolGameContext> factory)
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
    public async Task AddCardAsync(CardInfo card)
    {
        context.Cards.Add(card);
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

    public async Task<List<FoolGameScore>> GetAllScoresAsync()
    {
        var scores = await context.Scores.ToListAsync();
        return scores;
    }

    public async Task<List<CardInfo>> GetAllCardsAsync()
    {
        var cards = await context.Cards.ToListAsync();
        return cards;
    }

    //get from database by id
    public async Task<FoolGame?> GetGameByIdAsync(int id)
    {
        var game = await context.Games.FirstOrDefaultAsync(e => e.Id == id);
        return game;
    }

    public async Task<ApplicationUser?> GetUserByLoginAsync(string login)
    {
        var user = await context.Users.FirstOrDefaultAsync(e => e.Login == login);
        return user;
    }

    public async Task<FoolGameScore?> GetScoreByLoginAsync(string login)
    {
        var score = await context.Scores.FirstOrDefaultAsync(e => e.UserLogin == login);
        return score;
    }

    //update table in database
    public async Task UpdateUserAsync(ApplicationUser user)
    {
        context.Entry(user).State = EntityState.Modified;
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
    public async Task UpdateCardAsync(CardInfo card)
    {
        context.Entry(card).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
    public async Task UpdateGameAsync(FoolGame game)
    {
        context.Entry(game).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    //delete 
    public async Task DeleteUserByIdAsync(string login)
    {
        var user = await GetUserByLoginAsync(login);

        if (user is not null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteGameByIdAsync(int id)
    {
        var game = await GetGameByIdAsync(id);

        if (game is not null)
        {
            context.Games.Remove(game);
            await context.SaveChangesAsync();
        }
    }
}