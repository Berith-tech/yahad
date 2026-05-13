using back_yahad.Modules.Users.Domain;

namespace back_yahad.Modules.Users.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);
    Task<Role?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Role> AddAsync(Role role, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, Role role, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
