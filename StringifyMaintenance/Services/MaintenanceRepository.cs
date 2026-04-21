using Microsoft.EntityFrameworkCore;
using StringifyMaintenance.Data;
using StringifyMaintenance.Models;

namespace StringifyMaintenance.Services;

public class MaintenanceRepository
{
    private readonly IDbContextFactory<StringifyDbContext> _contextFactory;

    public MaintenanceRepository(IDbContextFactory<StringifyDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Termek>> GetProductsAsync()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Termekek.OrderBy(t => t.Id).ToListAsync();
    }

    public async Task<Termek> AddProductAsync(Termek product)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Termekek.Add(product);
        await db.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(Termek product)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Termekek.Update(product);
        await db.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Termekek.Remove(new Termek { Id = id });
        await db.SaveChangesAsync();
    }

    public async Task<TermekKepek?> GetProductImagesAsync(int productId)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.TermekKepek.FirstOrDefaultAsync(k => k.TermekId == productId);
    }

    public async Task UpsertProductImagesAsync(int productId, TermekKepek images)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var existing = await db.TermekKepek.FirstOrDefaultAsync(k => k.TermekId == productId);

        if (existing == null)
        {
            existing = new TermekKepek { TermekId = productId };
            db.TermekKepek.Add(existing);
        }

        existing.Kep1 = images.Kep1;
        existing.Kep2 = images.Kep2;
        existing.Kep3 = images.Kep3;
        existing.Kep4 = images.Kep4;
        existing.Kep5 = images.Kep5;

        await db.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Users.OrderBy(u => u.Id).ToListAsync();
    }

    public async Task<User> AddUserAsync(User user, string password)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        string salt = PasswordHasher.GenerateSalt();
        user.Salt = salt;
        user.Jelszo = PasswordHasher.HashWithSalt(password, salt);
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user, string? newPassword)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            string salt = PasswordHasher.GenerateSalt();
            user.Salt = salt;
            user.Jelszo = PasswordHasher.HashWithSalt(newPassword, salt);
        }
        db.Users.Update(user);
        await db.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Users.Remove(new User { Id = id });
        await db.SaveChangesAsync();
    }

    public async Task<List<Rendeles>> GetOrdersAsync()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        return await db.Rendelesek.OrderBy(r => r.Id).ToListAsync();
    }

    public async Task<Rendeles> AddOrderAsync(Rendeles order)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Rendelesek.Add(order);
        await db.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrderAsync(Rendeles order)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Rendelesek.Update(order);
        await db.SaveChangesAsync();
    }

    public async Task DeleteOrderAsync(int id)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        db.Rendelesek.Remove(new Rendeles { Id = id });
        await db.SaveChangesAsync();
    }
}
