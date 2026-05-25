namespace back_yahad.Modules.Users.DTOs;

public record UsuarioCreateDto(string Nome, string Email, string Senha, int? RoleId);
