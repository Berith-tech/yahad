# Skill: Criar Migration

> Nunca edite uma migration já aplicada. Sempre crie uma nova.

## Passos
1. Altere a entidade em `models/`
2. Atualize o mapeamento no `AppDbContext` se necessário
3. `dotnet ef migrations add <NomeDescritivo>` — ex: `AddDescricaoToTurma`, `CreatePresencaTable`
4. Revise o arquivo gerado em `Migrations/`
5. `dotnet ef database update`
6. `dotnet build`

## Convenções do banco

| Elemento | Padrão | Exemplo |
|---|---|---|
| Tabela | snake_case plural | `turmas` |
| Coluna | snake_case | `role_id`, `criado_em` |
| FK | `<tabela>_id` | `turma_id` |
| PK | `id` serial | `id` |
| Índice único | `ix_<tabela>_<coluna>` | `ix_usuarios_email` |
