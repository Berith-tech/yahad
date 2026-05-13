using back_yahad.Infrastructure.Persistence;
using back_yahad.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_yahad.Modules.Users.Repositories;

public class EfRoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;
    public EfRoleRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Roles.AsNoTracking().OrderBy(r => r.Id).ToListAsync(ct);

    public Task<Role?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<Role> AddAsync(Role role, CancellationToken ct = default)
    {
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(ct);
        return role;
    }

    public async Task<bool> UpdateAsync(int id, Role role, CancellationToken ct = default)
    {
        var existing = await _db.Roles.FindAsync([id], ct);
        if (existing is null) return false;
        existing.Nome = role.Nome;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Roles.FindAsync([id], ct);
        if (existing is null) return false;
        _db.Roles.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
