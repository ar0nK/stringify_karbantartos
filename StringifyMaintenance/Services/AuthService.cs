using Microsoft.EntityFrameworkCore;
using StringifyMaintenance.Data;
using StringifyMaintenance.Models;
using System.Windows;

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

        var user = await db.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            MessageBox.Show("Nem található felhasználó ezzel az email címmel.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
        }

        if (user.Aktiv != 1)
        {
            MessageBox.Show("Ez a fiók inaktív.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
        }

        if (user.Jogosultsag != 9)
        {
            MessageBox.Show("Nem megfelelõ a jogosultságod.\nEz a felület kizárólag adminisztrátoroknak elérhetõ.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        bool passwordValid = PasswordHasher.VerifyPassword(password, user.Salt, user.Jelszo);

        if (!passwordValid)
        {
            MessageBox.Show("Helytelen jelszó.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            return null;
        }

        return user;
    }
}