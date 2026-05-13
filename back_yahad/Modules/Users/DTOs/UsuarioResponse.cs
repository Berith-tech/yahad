namespace back_yahad.Modules.Users.DTOs;

public record UsuarioResponse(int Id, string Nome, string Email, int RoleId, string? RoleNome);
