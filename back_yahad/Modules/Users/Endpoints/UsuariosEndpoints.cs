using back_yahad.Modules.Users.Domain;
using back_yahad.Modules.Users.DTOs;
using back_yahad.Modules.Users.Repositories;
using back_yahad.Modules.Users.Validators;
using back_yahad.Shared.Utils;

namespace back_yahad.Modules.Users.Endpoints;

public static class UsuariosEndpoints
{
    public static IEndpointRouteBuilder MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/usuarios").WithTags("Usuarios");

        grupo.MapGet("/", async (IUsuarioRepository repo, CancellationToken ct) =>
            Results.Ok((await repo.GetAllAsync(ct)).Select(ToResponse)));

        grupo.MapGet("/{id:int}", async (int id, IUsuarioRepository repo, CancellationToken ct) =>
            await repo.GetByIdAsync(id, ct) is { } u ? Results.Ok(ToResponse(u)) : Results.NotFound());

        grupo.MapPost("/", async (UsuarioCreateDto dto, IUsuarioRepository repo, IRoleRepository roles, CancellationToken ct) =>
        {
            var erros = await UsuarioValidator.ValidarCriacaoAsync(dto, repo, roles, ct);
            if (erros.Count > 0) return Results.ValidationProblem(erros);

            var usuario = await repo.AddAsync(new Usuario
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                SenhaHash = PasswordHasher.Hash(dto.Senha),
                RoleId = dto.RoleId
            }, ct);

            var criado = await repo.GetByIdAsync(usuario.Id, ct);
            return Results.Created($"/usuarios/{usuario.Id}", ToResponse(criado!));
        });

        grupo.MapPut("/{id:int}", async (int id, UsuarioUpdateDto dto, IUsuarioRepository repo, IRoleRepository roles, CancellationToken ct) =>
        {
            if (await repo.GetByIdAsync(id, ct) is null) return Results.NotFound();

            var erros = await UsuarioValidator.ValidarAtualizacaoAsync(dto, id, repo, roles, ct);
            if (erros.Count > 0) return Results.ValidationProblem(erros);

            await repo.UpdateAsync(id, new Usuario
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                RoleId = dto.RoleId
            }, ct);
            return Results.NoContent();
        });

        grupo.MapDelete("/{id:int}", async (int id, IUsuarioRepository repo, CancellationToken ct) =>
            await repo.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());

        return app;
    }

    private static UsuarioResponse ToResponse(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.RoleId, u.Role?.Nome);
}
