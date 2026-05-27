# Skill: Criar Componente

## Passos
1. `ng generate component <modulo>/<nome> --standalone`
2. Adicione `ChangeDetectionStrategy.OnPush`
3. Injete serviços com `inject()`, nunca pelo construtor
4. Se for página nova, adicione rota com lazy loading em `app.routes.ts`
5. Rode `ng build`

## Convenções

| Elemento | Padrão | Exemplo |
|---|---|---|
| Arquivo | kebab-case | `usuario-lista.component.ts` |
| Classe | PascalCase + sufixo | `UsuarioListaComponent` |
| Selector | `app-` + kebab-case | `app-usuario-lista` |
| Input | camelCase | `usuarioId` |
| Output | camelCase + verbo passado | `usuarioCriado` |
