# Yahad — Agente Frontend

> Leia também `../CLAUDE.md` para as regras globais.

## Padrões de código

### Componentes
Sempre standalone com `ChangeDetectionStrategy.OnPush` quando possível.

### Serviços HTTP
Sempre em `core/services/`, com `HttpClient` tipado. Retornam `Observable`. Nunca use `any`.

### Modelos
Sempre em `core/models/`, refletindo exatamente os DTOs do backend.

### Roteamento
Lazy loading obrigatório para rotas de feature.

## Comandos

```bash
ng serve                          # dev server
ng generate component nome --standalone
ng generate service core/services/nome
ng build                          # verificar antes de concluir qualquer tarefa
```

## Skills disponíveis

- `../skills/frontend/criar-componente.md`
- `../skills/frontend/padrao-servico.md`

## Regras

1. Sempre rode `ng build` antes de concluir uma tarefa.
2. Nunca use `any` — use `unknown` e type guard se necessário.
3. Lógica de token fica em interceptors, não nos serviços.
4. Não implemente telas de auth enquanto `/auth/login` estiver pendente no backend.
