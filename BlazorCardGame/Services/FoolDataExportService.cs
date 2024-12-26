using System.Text.Json;
using System.Text.Json.Serialization;
using BlazorCardGame.Contexts;
using BlazorCardGame.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorCardGame.Services;

public class DataExportService
{
    private readonly FoolGameContext _context = null!;

    public DataExportService(FoolGameContext context)
    {
        _context = context;
    }

    public async Task<string> ExportDatabaseAsync()
    {
        var userData = await _context.Users.ToListAsync();
        var playerData = await _context.Players.ToListAsync();
        var scoreData = await _context.Scores.ToListAsync();

        var exportData = new
        {
            Users = userData,
            Players = playerData,
            Scores = scoreData
        };

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        var jsonData = JsonSerializer.Serialize(exportData, options);

        var exportFilePath = Path.Combine(Environment.CurrentDirectory, "export_data.json");

        await File.WriteAllTextAsync(exportFilePath, jsonData);

        return exportFilePath;
    }
    public async Task RestoreDatabaseAsync(string path)
    {

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Файл {path} базы данных не найден", path);
        }

        var jsonData = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        var data = JsonSerializer.Deserialize<DatabaseExportData>(jsonData, options);

        if (data is not null)
        {
            _context.Scores.RemoveRange(_context.Scores);
            _context.Players.RemoveRange(_context.Players);
            _context.Users.RemoveRange(_context.Users);

            await _context.Scores.AddRangeAsync(data.Scores);
            await _context.Users.AddRangeAsync(data.Users);
            await _context.Players.AddRangeAsync(data.Players);

            await _context.SaveChangesAsync();
        }
    }

}