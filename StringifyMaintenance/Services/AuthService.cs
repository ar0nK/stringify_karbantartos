using Microsoft.EntityFrameworkCore;
using StringifyMaintenance.Data;
using StringifyMaintenance.Models;

namespace StringifyMaintenance.Services;

public class AuthService
{
    private readonly IDbContextFactory<StringifyDbContext> _contextFactory;

    public AuthService(IDbContextFactory<StringifyDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || user.Aktiv != 1)
        {
            return null;
        }

        return PasswordHasher.VerifyPassword(password, user.Salt, user.Jelszo) ? user : null;
    }
}
