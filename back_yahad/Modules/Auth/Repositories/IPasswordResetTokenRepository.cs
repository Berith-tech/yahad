using back_yahad.Modules.Auth.Domain;

namespace back_yahad.Modules.Auth.Repositories;

public interface IPasswordResetTokenRepository
{
    Task AddAsync(PasswordResetToken token, CancellationToken ct = default);
    Task<PasswordResetToken?> GetValidTokenAsync(string token, CancellationToken ct = default);
    Task MarkAsUsedAsync(PasswordResetToken token, CancellationToken ct = default);
}
