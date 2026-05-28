# Skill: Criar Endpoint

## Checklist
- [ ] Existe task descrevendo o endpoint?
- [ ] O módulo do domínio existe em `Modules/`?
- [ ] O repositório existe em `Infrastructure/`?
- [ ] O DTO de resposta expõe campos sensíveis?

## Passos
1. Crie `Modules/<Dominio>/Endpoints/<Dominio>Endpoints.cs` com extension method `Map<Dominio>Endpoints`
2. Registre no `Program.cs`: `app.Map<Dominio>Endpoints()`
3. Crie DTOs como `record` em `Modules/<Dominio>/DTOs/`
4. Implemente handlers com validação via `Results.ValidationProblem`
5. Rode `dotnet build`

## Códigos HTTP

| Situação | Código | Método |
|---|---|---|
| Listagem | 200 | `Results.Ok(lista)` |
| Encontrado | 200 | `Results.Ok(dto)` |
| Não encontrado | 404 | `Results.NotFound()` |
| Criado | 201 | `Results.Created(url, dto)` |
| Atualizado | 200 | `Results.Ok(dto)` |
| Removido | 204 | `Results.NoContent()` |
| Validação falhou | 422 | `Results.ValidationProblem(erros)` |
