namespace back_yahad.Modules.Auth.DTOs;

public record ResetPasswordRequest(string Token, string NewPassword);
