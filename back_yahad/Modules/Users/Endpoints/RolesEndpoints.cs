using back_yahad.Modules.Users.Domain;
using back_yahad.Modules.Users.DTOs;
using back_yahad.Modules.Users.Repositories;
using back_yahad.Modules.Users.Validators;

namespace back_yahad.Modules.Users.Endpoints;

public static class RolesEndpoints
{
    public static IEndpointRouteBuilder MapRolesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/roles").WithTags("Roles");

        grupo.MapGet("/", async (IRoleRepository repo, CancellationToken ct) =>
            Results.Ok((await repo.GetAllAsync(ct)).Select(ToResponse)));

        grupo.MapGet("/{id:int}", async (int id, IRoleRepository repo, CancellationToken ct) =>
            await repo.GetByIdAsync(id, ct) is { } r ? Results.Ok(ToResponse(r)) : Results.NotFound());

        grupo.MapPost("/", async (RoleCreateDto dto, IRoleRepository repo, CancellationToken ct) =>
        {
            if (UsuarioValidator.ValidarRole(dto) is { } erros)
                return Results.ValidationProblem(erros);

            var role = await repo.AddAsync(new Role { Nome = dto.Nome.Trim() }, ct);
            return Results.Created($"/roles/{role.Id}", ToResponse(role));
        });

        grupo.MapPut("/{id:int}", async (int id, RoleCreateDto dto, IRoleRepository repo, CancellationToken ct) =>
        {
            if (UsuarioValidator.ValidarRole(dto) is { } erros)
                return Results.ValidationProblem(erros);

            return await repo.UpdateAsync(id, new Role { Nome = dto.Nome.Trim() }, ct)
                ? Results.NoContent()
                : Results.NotFound();
        });

        grupo.MapDelete("/{id:int}", async (int id, IRoleRepository repo, CancellationToken ct) =>
            await repo.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());

        return app;
    }

    private static RoleResponse ToResponse(Role r) => new(r.Id, r.Nome);
}
