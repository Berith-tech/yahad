using back_yahad.Infrastructure.Persistence;
using back_yahad.Modules.Auth.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_yahad.Modules.Auth.Repositories;

public class EfPasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly AppDbContext _db;
    public EfPasswordResetTokenRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(PasswordResetToken token, CancellationToken ct = default)
    {
        _db.PasswordResetTokens.Add(token);
        await _db.SaveChangesAsync(ct);
    }

    public Task<PasswordResetToken?> GetValidTokenAsync(string token, CancellationToken ct = default) =>
        _db.PasswordResetTokens
            .FirstOrDefaultAsync(t =>
                t.Token == token &&
                !t.Used &&
                t.ExpiresAt > DateTimeOffset.UtcNow, ct);

    public async Task MarkAsUsedAsync(PasswordResetToken token, CancellationToken ct = default)
    {
        token.Used = true;
        await _db.SaveChangesAsync(ct);
    }
}
