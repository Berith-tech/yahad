# Skill: Padrão de Serviço HTTP

## Onde criar
`src/app/core/services/<dominio>.service.ts`
Modelos em: `src/app/core/models/<dominio>.model.ts`

## Padrão

```typescript
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class NomeDominioService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/nome-dominio`;

  listar(): Observable<NomeDominio[]> {
    return this.http.get<NomeDominio[]>(this.base);
  }
}
```

## Regras
- Nunca use `any` — use interfaces tipadas em `core/models/`
- Nunca `.subscribe()` dentro do serviço
- Use `environment.apiUrl` como base — definido em `src/environments/environment.ts`
- Token JWT vai no interceptor, não aqui
