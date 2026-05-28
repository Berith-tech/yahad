using back_yahad.Infrastructure.Persistence;
using back_yahad.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_yahad.Modules.Users.Repositories;

public class EfUsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;
    public EfUsuarioRepository(AppDbContext db) => _db = db;

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Usuarios.AsNoTracking().Include(u => u.Role).OrderBy(u => u.Id).ToListAsync(ct);

    public Task<Usuario?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Usuarios.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<bool> EmailExisteAsync(string email, int? ignorarId = null, CancellationToken ct = default)
    {
        var lower = email.ToLower();
        return _db.Usuarios.AnyAsync(u =>
            u.Email.ToLower() == lower &&
            (ignorarId == null || u.Id != ignorarId), ct);
    }

    public async Task<Usuario> AddAsync(Usuario usuario, CancellationToken ct = default)
    {
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task<bool> UpdateAsync(int id, Usuario usuario, CancellationToken ct = default)
    {
        var existing = await _db.Usuarios.FindAsync([id], ct);
        if (existing is null) return false;
        existing.Nome = usuario.Nome;
        existing.Email = usuario.Email;
        existing.RoleId = usuario.RoleId;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Usuarios.FindAsync([id], ct);
        if (existing is null) return false;
        _db.Usuarios.Remove(existing);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        _db.Usuarios.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);

    public async Task<bool> UpdatePasswordAsync(int id, string senhaHash, CancellationToken ct = default)
    {
        var existing = await _db.Usuarios.FindAsync([id], ct);
        if (existing is null) return false;
        existing.SenhaHash = senhaHash;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
