# Task: Password Reset with Resend

## Overview

Implement the full password reset flow in Yahad, integrating the **Resend** email service via its official .NET SDK.

---

## Prerequisites

- Account created at [resend.com](https://resend.com) (free tier: 3,000 emails/month, 100/day)
- API Key generated in the Resend dashboard
- **Domain**: for this phase we will use Resend's sandbox domain — fixed sender `onboarding@resend.dev`, emails delivered only to your Resend account address. No DNS configuration required. Custom domain verification is deferred to production.

---

## Database Changes

### New table: `password_reset_tokens`

| Column       | Type           | Constraints                                      |
|--------------|----------------|--------------------------------------------------|
| id           | `serial`       | PK                                               |
| user_id      | `integer`      | NOT NULL, FK → `usuarios.id` (ON DELETE CASCADE) |
| token        | `varchar(256)` | NOT NULL, unique                                 |
| expires_at   | `timestamptz`  | NOT NULL                                         |
| used         | `boolean`      | NOT NULL, default `false`                        |
| created_at   | `timestamptz`  | NOT NULL, default `now()`                        |

> Token must be invalidated after use (`used = true`). Expired tokens can be cleaned up by a background job in a future phase.

### Migration

```bash
cd back_yahad
dotnet ef migrations add AddPasswordResetTokens
dotnet ef database update
```

---

## .NET Dependencies

```bash
dotnet add package Resend
```

---

## Configuration

Add to `appsettings.Local.json` (never commit):

```json
{
  "Resend": {
    "ApiKey": "re_YOUR_API_KEY",
    "FromEmail": "onboarding@resend.dev",
    "FromName": "Yahad"
  },
  "PasswordReset": {
    "ExpirationMinutes": 60,
    "BaseUrl": "http://localhost:4200"
  }
}
```

---

## Endpoints

### `POST /auth/forgot-password`

**Payload:**
```json
{ "email": "john@example.com" }
```

**Flow:**
1. Look up user by email
2. If not found, return `200 OK` without revealing that information (anti-enumeration)
3. Generate a secure token (`Guid.NewGuid().ToString("N")` or `RandomNumberGenerator`)
4. Save to `password_reset_tokens` with `expires_at = now() + 60min`
5. Build reset link: `{BaseUrl}/reset-password?token={token}`
6. Send email via Resend with the link
7. Return `200 OK`

**Response (always the same, regardless of whether the email exists):**
```json
{ "message": "If this email is registered, you will receive instructions shortly." }
```

---

### `POST /auth/reset-password`

**Payload:**
```json
{
  "token": "abc123...",
  "newPassword": "myNewP@ssw0rd"
}
```

**Flow:**
1. Look up token in `password_reset_tokens`
2. Validate: token exists + `used = false` + `expires_at > now()`
3. If invalid or expired → `400 Bad Request`
4. Hash the new password via `PasswordHasher.Hash(newPassword)`
5. Update `senha_hash` in `usuarios`
6. Mark token as `used = true`
7. Return `200 OK`

**Success response:**
```json
{ "message": "Password reset successfully." }
```

**Error response:**
```json
{ "message": "Invalid or expired token." }
```

---

## Suggested Code Structure

```
back_yahad/
├── models/
│   └── PasswordResetTokenModel.cs          # new entity
├── repositories/
│   ├── IPasswordResetTokenRepository.cs
│   └── PasswordResetTokenRepository.cs
├── services/
│   ├── IEmailService.cs
│   └── ResendEmailService.cs               # Resend SDK wrapper
└── endpoints/
    └── AuthEndpoints.cs                    # MapAuthEndpoints()
```

---

## Email Template

Subject: `Password Reset — Yahad`

Body (simple HTML):

```html
<p>Hi,</p>
<p>We received a request to reset the password for your <strong>Yahad</strong> account.</p>
<p>Click the link below to set a new password. The link expires in <strong>60 minutes</strong>.</p>
<p><a href="{LINK}">Reset my password</a></p>
<p>If you did not request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
<p>— The Yahad Team</p>
```

---

## Frontend (Angular)

Two new routes, no token logic in the component:

| Route                         | Component                 | Responsibility                                              |
|-------------------------------|---------------------------|-------------------------------------------------------------|
| `/forgot-password`            | `ForgotPasswordComponent` | Email form → `POST /auth/forgot-password`                  |
| `/reset-password?token=...`   | `ResetPasswordComponent`  | New password form → `POST /auth/reset-password` (reads token from query string) |

---

## Acceptance Criteria

- [ ] `forgot-password` endpoint sends email when user exists and always returns `200`
- [ ] Token expires after 60 minutes
- [ ] Token cannot be reused after it has been consumed
- [ ] `reset-password` endpoint rejects invalid/expired tokens with `400`
- [ ] New password is saved via `PasswordHasher.Hash()` (BCrypt already implemented)
- [ ] Resend API Key is not present in any versioned file
- [ ] Angular routes created and reachable from the login screen

---

## Notes

- **Rate limiting**: consider throttling calls to `forgot-password` by IP/email in a future phase to prevent abuse of the Resend free tier quota.
- **Token cleanup**: expired tokens will accumulate in the table over time. Plan a cleanup job (Hangfire or a hosted service) for a future phase.
