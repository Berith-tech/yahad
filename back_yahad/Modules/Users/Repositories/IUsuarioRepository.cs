using back_yahad.Modules.Users.Domain;

namespace back_yahad.Modules.Users.Repositories;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default);
    Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<bool> EmailExisteAsync(string email, int? ignorarId = null, CancellationToken ct = default);
    Task<Usuario> AddAsync(Usuario usuario, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, Usuario usuario, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default);
}
