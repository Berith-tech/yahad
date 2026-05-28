using Resend;

namespace back_yahad.Modules.Auth.Services;

public class ResendEmailService : IEmailService
{
    private readonly IResend _resend;
    private readonly IConfiguration _config;

    public ResendEmailService(IResend resend, IConfiguration config)
    {
        _resend = resend;
        _config = config;
    }

    public async Task SendPasswordResetAsync(string toEmail, string resetLink, CancellationToken ct = default)
    {
        var from = $"{_config["Resend:FromName"]} <{_config["Resend:FromEmail"]}>";

        var message = new EmailMessage
        {
            From = from,
            Subject = "Password Reset — Yahad",
            HtmlBody = $"""
                <p>Hi,</p>
                <p>We received a request to reset the password for your <strong>Yahad</strong> account.</p>
                <p>Click the link below to set a new password. The link expires in <strong>60 minutes</strong>.</p>
                <p><a href="{resetLink}">Reset my password</a></p>
                <p>If you did not request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
                <p>— The Yahad Team</p>
                """
        };
        message.To.Add(toEmail);

        await _resend.EmailSendAsync(message, ct);
    }
}
