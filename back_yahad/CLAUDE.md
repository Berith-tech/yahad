# Yahad — Agente Backend

> Leia também `../CLAUDE.md` para as regras globais.

## Padrões de código

### Endpoints
Sempre em extension methods em `Modules/`. Nunca inline no `Program.cs`.

### Repositórios
Interface em `Infrastructure/Interfaces/`, implementação em `Infrastructure/Repositories/`.

### DTOs
Sempre `record` em `Shared/DTOs/`. Nunca exponha `SenhaHash`.

### Validação
`Results.ValidationProblem` com dicionário de erros. Nunca lance exceção para validação de negócio.

## Comandos

```bash
dotnet run          # sobe a API em localhost:5014
dotnet build        # verificar antes de concluir qualquer tarefa
dotnet ef migrations add <NomeDescritivo>
dotnet ef database update
```

## Skills disponíveis

- `../skills/backend/criar-endpoint.md`
- `../skills/backend/criar-migration.md`
- `../skills/backend/padrao-repositorio.md`
- `../skills/backend/atualizar-readme.md`

## Regras

1. Sempre rode `dotnet build` antes de concluir uma tarefa.
2. Nunca edite migrations existentes.
3. Nunca use `DbContext` diretamente nos endpoints.
4. Tabelas e colunas sempre em snake_case via Fluent API.
