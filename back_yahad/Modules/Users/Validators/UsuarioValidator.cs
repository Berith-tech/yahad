using back_yahad.Modules.Users.DTOs;
using back_yahad.Modules.Users.Repositories;

namespace back_yahad.Modules.Users.Validators;

public static class UsuarioValidator
{
    public static async Task<Dictionary<string, string[]>> ValidarCriacaoAsync(
    UsuarioCreateDto dto,
    IUsuarioRepository usuarios,
    IRoleRepository roles,
    CancellationToken ct)
    {
        var erros = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(dto.Nome))
            erros["Nome"] = ["O nome é obrigatório."];

        if (string.IsNullOrWhiteSpace(dto.Email))
            erros["Email"] = ["O email é obrigatório."];
        else if (await usuarios.EmailExisteAsync(dto.Email, null, ct))
            erros["Email"] = ["Email já cadastrado."];

        if (string.IsNullOrWhiteSpace(dto.Senha) || dto.Senha.Length < 6)
            erros["Senha"] = ["A senha deve ter no mínimo 6 caracteres."];

        var roleId = dto.RoleId ?? 1;
        if (await roles.GetByIdAsync(roleId, ct) is null)
            erros["RoleId"] = ["Role inexistente."];

        return erros;
    }

    public static async Task<Dictionary<string, string[]>> ValidarAtualizacaoAsync(
        UsuarioUpdateDto dto,
        int id,
        IUsuarioRepository usuarios,
        IRoleRepository roles,
        CancellationToken ct)
    {
        var erros = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(dto.Nome))
            erros["Nome"] = ["O nome é obrigatório."];

        if (string.IsNullOrWhiteSpace(dto.Email))
            erros["Email"] = ["O email é obrigatório."];
        else if (await usuarios.EmailExisteAsync(dto.Email, id, ct))
            erros["Email"] = ["Email já cadastrado."];

        if (await roles.GetByIdAsync(dto.RoleId, ct) is null)
            erros["RoleId"] = ["Role inexistente."];

        return erros;
    }

    public static Dictionary<string, string[]>? ValidarRole(RoleCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            return new Dictionary<string, string[]> { ["Nome"] = ["O nome é obrigatório."] };
        return null;
    }
}
