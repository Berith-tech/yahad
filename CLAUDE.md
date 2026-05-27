# Yahad — Agente Principal

Você está trabalhando no **Yahad**, uma plataforma modular para organização da Escola Bíblica Dominical (EBD) de igrejas.

## Stack

| Camada | Tecnologia |
|---|---|
| Frontend | Angular (SPA) |
| Backend | ASP.NET Core 10 — Minimal API |
| ORM | Entity Framework Core 10 (Npgsql) |
| Banco | PostgreSQL local, porta 5432 |
| Auth | JWT (pendente) |

## Estrutura do repositório

```
yahad/
├── CLAUDE.md
├── skills/
│   ├── backend/
│   └── frontend/
├── back_yahad/
│   ├── CLAUDE.md
│   └── tasks/
└── front_yahad/
    ├── CLAUDE.md
    └── tasks/
```

## Regras globais — nunca viole

1. Nunca exponha `SenhaHash` em DTOs de resposta.
2. Nunca comite credenciais em `appsettings.json` ou `appsettings.Development.json`.
3. Sempre use repositórios para acesso a dados — nunca acesse `DbContext` diretamente nos endpoints.
4. Sempre use DTOs como contrato de entrada/saída.
5. Todos os métodos que tocam banco ou I/O devem ser async com `CancellationToken`.
6. Nunca modifique uma migration já aplicada ao banco. Crie sempre uma nova.
7. Use `Results.ValidationProblem` (RFC 7807) para erros de validação.
8. snake_case nas colunas do banco, PascalCase em C#, camelCase no TypeScript.

## Contexto atual

- CRUD `/roles` e `/usuarios`: ✅ implementado
- Hash de senha SHA-256: ✅ implementado
- Autenticação JWT: ⏳ pendente
- Módulo EBD (turmas, presença, lições): ⏳ não iniciado

> Não implemente JWT, BCrypt ou módulo EBD sem uma task correspondente em `tasks/`.

## Como trabalhar

- Consulte a skill correspondente em `skills/` antes de criar endpoints, componentes ou migrations.
- Ao concluir uma task, marque-a como feita e atualize o contexto neste arquivo se necessário.
- Sempre prefira a solução mais simples que resolve o problema.
