using System;
using MobileApp.Models;
using SQLite;

namespace MobileApp.Services;

public class FavoriteService
{
    private readonly SQLiteAsyncConnection _database;

    public FavoriteService()
    {
        var dbPath
            = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder
                    .LocalApplicationData), "favorites.db");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<FavoriteModel>().Wait();
    }

    public async Task<FavoriteModel> ReadAsync(Guid id)
    {
        return await _database.Table<FavoriteModel>()
            .Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<FavoriteModel>> ReadAllAsync()
    {
        return await _database.Table<FavoriteModel>().ToListAsync();
    }

    public async Task CreateAsync(FavoriteModel favoritePlace)
    {
        await _database.InsertAsync(favoritePlace);
    }

    public async Task DeleteAsync(Guid id)
    {
        var favoritePlace = await ReadAsync(id);
        if (favoritePlace != null)
        {
            await _database.DeleteAsync(favoritePlace);
        }
    }
}
