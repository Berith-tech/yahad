# Skill: Padrão de Serviço HTTP

## Onde criar
`src/app/core/services/<dominio>.service.ts`
Modelos em: `src/app/core/models/<dominio>.model.ts`

## Padrão

```typescript
@Injectable({ providedIn: 'root' })
export class NomeDominioService {
  private readonly http = inject(HttpClient);
  private readonly base = '/api/nome-dominio';

  listar(): Observable<NomeDominio[]> {
    return this.http.get<NomeDominio[]>(this.base);
  }
}
```

## Regras
- Nunca use `any`
- Nunca `.subscribe()` dentro do serviço
- Prefixo `/api/` — o proxy do dev server redireciona para `localhost:5014`
- Token JWT vai no interceptor, não aqui
