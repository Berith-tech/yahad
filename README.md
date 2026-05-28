# Yahad – Documentação Técnica

Documento técnico de arquitetura e decisões do projeto Yahad.

## 1. Visão Geral

O Yahad é uma plataforma modular para organização da Escola Bíblica Dominical, com arquitetura preparada para expansão futura para outros módulos da igreja.

## 2. Stack Tecnológica

- **Frontend**: Angular (SPA)
- **Backend**: ASP.NET Core 10 — Minimal API
- **ORM**: Entity Framework Core 10 (provider Npgsql)
- **Banco de Dados**: PostgreSQL (servidor local, porta 5432)
- **Email transacional**: Resend (reset de senha)

## 3. Autenticação e Segurança

Autenticação baseada em **JWT** (JSON Web Token).

- Login via `POST /auth/login` — retorna `{ token }` assinado com HMAC-SHA256.
- Todas as requisições protegidas exigem o header `Authorization: Bearer <token>`.
- Claims no token: `sub` (userId), `email`, `role`.
- Tokens expiram em **2 horas**.
- Senhas armazenadas como hash **SHA-256**. O texto puro nunca é persistido.
- Fluxo de reset de senha via email (Resend): token aleatório de 64 bytes hex, expira em 60 minutos (configurável), gravado na tabela `password_reset_tokens`, invalidado após uso.

## 4. Backend (.NET)

Arquitetura: **Minimal API** com módulos separados por domínio em `Modules/`.

- **DTOs** (`records`) — contratos de entrada/saída desacoplados das entidades. Campos sensíveis (ex.: `SenhaHash`) nunca aparecem no JSON.
- **Entidades** — classes de domínio em `Modules/<Dominio>/Domain/`.
- **DbContext** (`AppDbContext`) em `Infrastructure/Persistence/` — mapeamento Fluent API com `snake_case` nas colunas.
- **Repositórios** — interface em `Modules/<Dominio>/Repositories/`, implementação prefixada `Ef` no mesmo pacote.
- **Endpoints** — extension methods em `Modules/<Dominio>/Endpoints/`, registrados no `Program.cs`.
- **Validação** via `Results.ValidationProblem` (Problem Details RFC 7807).
- **Async** ponta a ponta com `CancellationToken`.

### 4.1. Endpoints

#### Health
- `GET /` → `{ status: "ok", servico: "yahad-api" }`

#### Auth (`/auth`)
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/auth/login` | Autentica usuário e retorna JWT | Pública |
| POST | `/auth/forgot-password` | Dispara email de reset de senha | Pública |
| POST | `/auth/reset-password` | Redefine senha com token válido | Pública |

**`POST /auth/login`** — corpo: `{ email, password }` → resposta: `{ token }`

**`POST /auth/forgot-password`** — corpo: `{ email }` → sempre retorna 200 (resposta genérica para não revelar existência de email)

**`POST /auth/reset-password`** — corpo: `{ token, newPassword }` → 200 ou 400 se token inválido/expirado

#### Roles (`/roles`)
| Método | Rota | Descrição |
|---|---|---|
| GET | `/roles` | Lista todas |
| GET | `/roles/{id}` | Busca por id |
| POST | `/roles` | Cria role |
| PUT | `/roles/{id}` | Atualiza role |
| DELETE | `/roles/{id}` | Remove role |

#### Usuários (`/usuarios`)
| Método | Rota | Descrição |
|---|---|---|
| GET | `/usuarios` | Lista todos (com `roleNome` via `Include`) |
| GET | `/usuarios/{id}` | Busca por id |
| POST | `/usuarios` | Cria usuário (gera hash da senha) |
| PUT | `/usuarios/{id}` | Atualiza dados (não altera senha) |
| DELETE | `/usuarios/{id}` | Remove usuário |

### 4.2. Estrutura de pastas

```
back_yahad/
├── Program.cs
├── back_yahad.csproj
├── appsettings.json
├── appsettings.Local.example.json   # template de config local
├── Infrastructure/
│   ├── DependencyInjection/         # extensões de registro no DI
│   └── Persistence/
│       └── AppDbContext.cs
├── Migrations/
├── Modules/
│   ├── Auth/
│   │   ├── Domain/PasswordResetToken.cs
│   │   ├── DTOs/                    # LoginRequest, LoginResponse, ForgotPasswordRequest, ResetPasswordRequest
│   │   ├── Endpoints/AuthEndpoints.cs
│   │   ├── Extensions/              # AddAuthModule, AddAuthenticationModule
│   │   ├── Repositories/            # IPasswordResetTokenRepository, EfPasswordResetTokenRepository
│   │   └── Services/                # AuthService, JwtTokenService, IEmailService, ResendEmailService
│   └── Users/
│       ├── Domain/                  # Usuario, Role
│       ├── Endpoints/
│       ├── Repositories/            # IUsuarioRepository, EfUsuarioRepository
│       └── UsersModule.cs
└── Shared/
    └── Utils/PasswordHasher.cs
```

## 5. Banco de Dados

PostgreSQL relacional. Versionamento via EF Core Migrations.

### 5.1. Schema atual

**`roles`**
| Coluna | Tipo | Constraints |
|---|---|---|
| id | `serial` | PK |
| nome | `varchar(50)` | NOT NULL, único |

**`usuarios`**
| Coluna | Tipo | Constraints |
|---|---|---|
| id | `serial` | PK |
| nome | `varchar(120)` | NOT NULL |
| email | `varchar(160)` | NOT NULL, único |
| senha_hash | `varchar(256)` | NOT NULL |
| role_id | `integer` | NOT NULL, FK → `roles.id` (`ON DELETE RESTRICT`) |

**`password_reset_tokens`**
| Coluna | Tipo | Constraints |
|---|---|---|
| id | `serial` | PK |
| user_id | `integer` | NOT NULL |
| token | `varchar(256)` | NOT NULL, único |
| expires_at | `timestamptz` | NOT NULL |
| used | `boolean` | NOT NULL, default `false` |
| created_at | `timestamptz` | NOT NULL, default `now()` |

### 5.2. Migrations aplicadas
| Migration | Descrição |
|---|---|
| `first-migration` | Schema inicial: `roles` + `usuarios` |
| `AddPasswordResetTokens` | Tabela `password_reset_tokens` |

## 6. Frontend (Angular)

SPA Angular com roteamento standalone.

### 6.1. Estrutura de pastas

```
front_yahad/src/app/
├── core/
│   └── services/
│       ├── auth.service.ts        # forgotPassword, resetPassword, login
│       └── user.service.ts
├── home/
│   ├── home.component.ts
│   └── components/welcome-banner/
├── reset-password/
│   ├── reset-password.component.ts
│   └── components/reset-password-form/
└── app.routes.ts
```

### 6.2. Rotas disponíveis
| Rota | Componente | Descrição |
|---|---|---|
| `/home` | `HomeComponent` | Página inicial |
| `/reset-password` | `ResetPasswordComponent` | Formulário de reset de senha |

### 6.3. Serviços
- `AuthService` — consome `/auth/forgot-password` e `/auth/reset-password`. URL base via `environment.apiUrl`.
- `UserService` — consome `/usuarios`.

### 6.4. Ambiente

O arquivo `src/environments/environment.ts` define `apiUrl` (padrão: `http://localhost:5014`). Não versionar arquivos de ambiente com URLs de produção.

## 7. Como subir o ambiente local

### 7.1. Pré-requisitos
- .NET SDK 10
- Node.js 20+ e Angular CLI (`npm install -g @angular/cli`)
- PostgreSQL acessível em `localhost:5432`
- Conta Resend com API Key (para testar o fluxo de reset de senha)
- CLI do EF Core: `dotnet tool install --global dotnet-ef`

### 7.2. Configuração do backend

Credenciais **não** ficam no `appsettings.json`. Use `appsettings.Local.json`:

```bash
cp back_yahad/appsettings.Local.example.json back_yahad/appsettings.Local.json
```

Edite com seus dados:

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=yahadDb;Username=SEU_USUARIO;Password=SUA_SENHA"
  },
  "Jwt": {
    "Key": "CHAVE_COM_NO_MINIMO_32_CARACTERES",
    "Issuer": "yahad-api",
    "Audience": "yahad-client"
  },
  "Resend": {
    "ApiKey": "re_SUA_API_KEY",
    "FromEmail": "onboarding@resend.dev",
    "FromName": "Yahad"
  },
  "PasswordReset": {
    "ExpirationMinutes": 60,
    "BaseUrl": "http://localhost:4200"
  }
}
```

> `appsettings.Local.json` está no `.gitignore` e nunca será commitado.

### 7.3. Aplicar migrations

```bash
cd back_yahad
dotnet ef database update
```

### 7.4. Rodar a API

```bash
cd back_yahad
dotnet run
```

API sobe em `http://localhost:5014`. Swagger disponível em `http://localhost:5014/swagger`.

### 7.5. Rodar o frontend

```bash
cd front_yahad
npm install
ng serve
```

Frontend sobe em `http://localhost:4200`.

### 7.6. Smoke tests

```bash
# 1) Cria roles
curl -X POST http://localhost:5014/roles -H "Content-Type: application/json" -d '{"nome":"admin"}'
curl -X POST http://localhost:5014/roles -H "Content-Type: application/json" -d '{"nome":"usuario"}'

# 2) Cria usuário
curl -X POST http://localhost:5014/usuarios \
  -H "Content-Type: application/json" \
  -d '{"nome":"João Teste","email":"joao@yahad.dev","senha":"senha123","roleId":1}'

# 3) Login → retorna JWT
curl -X POST http://localhost:5014/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@yahad.dev","password":"senha123"}'

# 4) Dispara email de reset de senha
curl -X POST http://localhost:5014/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@yahad.dev"}'
```

## 8. Status do projeto

| Feature | Status |
|---|---|
| CRUD `/roles` | ✅ |
| CRUD `/usuarios` | ✅ |
| Hash de senha (SHA-256) | ✅ |
| Autenticação JWT | ✅ |
| Reset de senha via email (Resend) | ✅ |
| Tela de reset de senha (Angular) | ✅ |
| Módulo EBD (turmas, presença, lições) | ⏳ não iniciado |

## 9. Princípios de Arquitetura

Modularidade, segurança, escalabilidade gradual, baixa dependência e manutenção simples.

## 10. Histórico

**2026-05-28 — Auth completo + frontend inicial**
- JWT login (`POST /auth/login`) implementado com `JwtTokenService` e `AuthService`.
- Fluxo de reset de senha: `POST /auth/forgot-password` envia email via Resend; `POST /auth/reset-password` valida token e redefine senha.
- Nova entidade `PasswordResetToken` e migration `AddPasswordResetTokens`.
- Repositório `IPasswordResetTokenRepository` / `EfPasswordResetTokenRepository`.
- Métodos `UpdatePasswordAsync` e `GetByEmailAsync` adicionados em `IUsuarioRepository`.
- Backend refatorado para estrutura modular em `Modules/`.
- Frontend: `HomeComponent`, `ResetPasswordComponent`, `AuthService`, `UserService`, `environment.ts`.
- CORS configurado para `http://localhost:4200`.

**2026-05-03 — Backend inicial**
- Minimal API com endpoints CRUD de `/roles` e `/usuarios`.
- Integração EF Core 10 + Npgsql.
- Primeira migration (`first-migration`) — schema `roles` + `usuarios`.
- Hash de senha (SHA-256) e DTOs sem exposição de `SenhaHash`.
