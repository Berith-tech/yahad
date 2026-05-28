using back_yahad.Modules.Auth.Domain;
using back_yahad.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace back_yahad.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Nome).HasColumnName("nome").HasMaxLength(50).IsRequired();
            e.HasIndex(r => r.Nome).IsUnique();
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id");
            e.Property(u => u.Nome).HasColumnName("nome").HasMaxLength(120).IsRequired();
            e.Property(u => u.Email).HasColumnName("email").HasMaxLength(160).IsRequired();
            e.Property(u => u.SenhaHash).HasColumnName("senha_hash").HasMaxLength(256).IsRequired();
            e.Property(u => u.RoleId).HasColumnName("role_id");
            e.HasIndex(u => u.Email).IsUnique();
            e.HasOne(u => u.Role)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PasswordResetToken>(e =>
        {
            e.ToTable("password_reset_tokens");
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            e.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
            e.Property(t => t.Token).HasColumnName("token").HasMaxLength(256).IsRequired();
            e.Property(t => t.ExpiresAt).HasColumnName("expires_at").IsRequired();
            e.Property(t => t.Used).HasColumnName("used").IsRequired().HasDefaultValue(false);
            e.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired()
                .HasDefaultValueSql("now()");
            e.HasIndex(t => t.Token).IsUnique();
        });
    }
}
