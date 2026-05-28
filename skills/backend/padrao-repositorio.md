# Skill: Padrão de Repositório

## Estrutura

```
Modules/<Dominio>/
└── Repositories/
    ├── INomeDominioRepository.cs
    └── EfNomeDominioRepository.cs
```

## Interface

```csharp
public interface INomeDominioRepository
{
    Task<List<NomeDominio>> ListarAsync(CancellationToken ct);
    Task<NomeDominio?> BuscarPorIdAsync(int id, CancellationToken ct);
    Task AdicionarAsync(NomeDominio entidade, CancellationToken ct);
    Task AtualizarAsync(NomeDominio entidade, CancellationToken ct);
    Task RemoverAsync(NomeDominio entidade, CancellationToken ct);
}
```

## Implementação

```csharp
public class NomeDominioRepository(AppDbContext ctx) : INomeDominioRepository
{
    public async Task<List<NomeDominio>> ListarAsync(CancellationToken ct)
        => await ctx.NomeDominios.AsNoTracking().ToListAsync(ct);

    public async Task<NomeDominio?> BuscarPorIdAsync(int id, CancellationToken ct)
        => await ctx.NomeDominios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AdicionarAsync(NomeDominio entidade, CancellationToken ct)
    { ctx.NomeDominios.Add(entidade); await ctx.SaveChangesAsync(ct); }

    public async Task AtualizarAsync(NomeDominio entidade, CancellationToken ct)
    { ctx.NomeDominios.Update(entidade); await ctx.SaveChangesAsync(ct); }

    public async Task RemoverAsync(NomeDominio entidade, CancellationToken ct)
    { ctx.NomeDominios.Remove(entidade); await ctx.SaveChangesAsync(ct); }
}
```

## Registro no DI

Registre via extension method em `Modules/<Dominio>/` ou em `Infrastructure/DependencyInjection/`:

```csharp
builder.Services.AddScoped<INomeDominioRepository, EfNomeDominioRepository>();
```

## Regras
- `AsNoTracking()` em toda query de leitura
- `Include()` fica aqui, não no endpoint
- Nunca injete `AppDbContext` direto nos endpoints
