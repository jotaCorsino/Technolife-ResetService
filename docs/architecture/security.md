# Reset Service — Security Architecture

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Arquitetura Técnica de Segurança  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/security-requirements.md`, `docs/product/user-access-spec.md`, `docs/product/backup-recovery-spec.md`, `docs/architecture/architecture.md`, `docs/architecture/data-model.md`

---

## 1. Objetivo

Este documento transforma os requisitos funcionais de segurança do Reset Service em decisões técnicas compatíveis com a arquitetura aprovada.

Abrange:

- autenticação;
- autorização;
- credenciais;
- cookies;
- sessões;
- lockout;
- rate limiting;
- antiforgery;
- SignalR;
- HTTPS;
- Data Protection;
- identidade interativa do processo no Windows;
- permissões de filesystem;
- SQLite;
- uploads;
- headers HTTP;
- logs;
- segurança da fila de comandos.

---

## 2. Princípio arquitetural

A segurança será aplicada em camadas.

```text
HTTPS
  ↓
Rate Limiting
  ↓
Autenticação
  ↓
Antiforgery
  ↓
Autorização
  ↓
Validação
  ↓
Command Queue
  ↓
Regras de domínio
  ↓
Controle de concorrência
  ↓
Persistência
```

Nenhuma camada isolada será considerada suficiente.

---

## 3. ASP.NET Core Identity

A infraestrutura de autenticação será baseada em:

```text
ASP.NET Core Identity
```

Identity será responsável por:

- armazenamento protegido de credenciais;
- password hashing;
- autenticação;
- cookies;
- lockout;
- security stamp;
- roles;
- suporte à redefinição de senha.

A interface continuará sendo própria do Reset Service.

Não haverá páginas públicas de cadastro fornecidas pelo Identity.

---

## 4. Recursos de autenticação não utilizados

A versão 1.0 não terá:

- cadastro público;
- login social;
- Google;
- Microsoft Account;
- autenticação em nuvem;
- confirmação de e-mail;
- recuperação automática por e-mail;
- autenticação externa obrigatória.

Toda autenticação será local ao Reset Service.

---

## 5. Política de senha

A política aprovada será:

```text
Mínimo: 8 caracteres
```

Não haverá obrigatoriedade artificial de:

- maiúscula;
- minúscula;
- número;
- símbolo;

em combinação fixa.

Frases-senha serão permitidas.

Senhas longas deverão ser aceitas dentro de limite técnico adequado.

---

## 6. Senhas administrativas

Não haverá:

```text
admin / admin
```

ou qualquer outra credencial padrão conhecida.

Não existirá senha mestre, conta secreta ou superusuário oculto.

---

## 7. Primeiro Administrador

A criação do primeiro Administrador será um fluxo especial de bootstrap.

Enquanto ainda não existir nenhum usuário válido:

```text
Servidor local
   ↓
Initial Setup
   ↓
Criar primeiro Administrador
```

A configuração inicial deverá ser acessível somente localmente no computador servidor.

Não deverá ficar disponível livremente para qualquer computador da LAN.

Depois que o primeiro Administrador for criado, o fluxo normal de bootstrap será desativado.

---

## 8. Alteração da própria senha

O fluxo deverá solicitar:

```text
Senha atual
Nova senha
Confirmação
```

A senha atual deverá ser validada antes da troca.

---

## 9. Redefinição administrativa

Administradores poderão redefinir a credencial de um usuário.

Nunca poderão visualizar a senha atual.

A credencial definida administrativamente será temporária.

O usuário deverá alterá-la no próximo login.

---

## 10. Lockout

A política inicial será:

```text
5 falhas consecutivas
        ↓
5 minutos de bloqueio
```

Um login bem-sucedido reiniciará a contagem correspondente.

O mecanismo será implementado através dos recursos do ASP.NET Core Identity.

---

## 11. Mensagem de login inválido

A interface não deverá revelar se o nome do usuário existe.

Mensagem preferencial:

```text
Usuário ou senha inválidos.
```

---

## 12. Rate limiting do login

Além do lockout da conta, o endpoint de login possuirá proteção complementar contra rajadas de requisições.

Configuração inicial proposta:

```text
aproximadamente
10 tentativas por minuto
por endereço de origem
```

O valor poderá ser refinado pelos testes.

Não será aplicado rate limiting agressivo indiscriminadamente sobre toda a aplicação.

---

## 13. Cookie de autenticação

A autenticação da aplicação utilizará cookie.

Configuração funcional:

| Configuração | Valor |
|---|---|
| `HttpOnly` | Sim |
| `Secure` | Sempre |
| `SameSite` | Strict |
| Expiração por inatividade | 8 horas |
| Sliding expiration | Sim |
| Remember me | Não |
| Domínio amplo | Não |

---

## 14. HttpOnly

O cookie de autenticação deverá utilizar:

```text
HttpOnly = true
```

Scripts normais executados no navegador não deverão ter acesso ao seu valor.

---

## 15. Secure

O cookie utilizará:

```text
Secure = Always
```

e será enviado somente através de HTTPS.

---

## 16. SameSite

Será utilizado inicialmente:

```text
SameSite = Strict
```

O Reset Service não depende de autenticação cross-site ou integração federada.

Qualquer necessidade futura de relaxamento dessa política deverá ser analisada explicitamente.

---

## 17. Remember me

A versão 1.0 não oferecerá opção:

```text
Manter conectado
```

O objetivo é evitar sessões persistentemente armazenadas em computadores compartilhados.

---

## 18. Expiração por inatividade

Valor inicial:

```text
8 horas
```

A aplicação utilizará sliding expiration para manter uma sessão ativa durante uso normal.

Inatividade suficiente provoca expiração.

---

## 19. Security Stamp

A aplicação utilizará o mecanismo de Security Stamp do Identity.

Intervalo inicial de revalidação:

```text
1 minuto
```

O objetivo é impedir que mudanças sensíveis demorem horas para surtir efeito.

---

## 20. Eventos que invalidam autorização

Ações como:

- desativação do usuário;
- redefinição de senha;
- alteração de perfil;
- mudanças críticas de credencial;

deverão provocar atualização do estado de segurança da conta.

Sessões antigas não deverão manter permissões indefinidamente.

---

## 21. Logout

A ação Sair deverá invalidar a sessão utilizada.

O navegador não poderá continuar enviando operações autenticadas com aquele ticket depois do logout.

---

## 22. Restauração

Após restauração de backup:

```text
todas as sessões
→ invalidadas
```

Todos os usuários deverão autenticar-se novamente.

---

## 23. Roles

Serão utilizadas as roles:

```text
Administrator
Technician
```

Apresentadas funcionalmente como:

```text
Administrador
Técnico
```

Não haverá sistema de ACL individual na versão 1.0.

---

## 24. Policies

Operações importantes utilizarão políticas de autorização do ASP.NET Core.

Exemplos conceituais:

```text
ManageUsers
ManageTemplates
ManageCompanySettings
ManageBackups
ReopenService
```

As policies poderão mapear para as roles e requisitos apropriados.

---

## 25. Autorização no backend

A autorização efetiva sempre ocorrerá no servidor.

```text
Frontend
→ esconde controles não aplicáveis

Backend
→ impede realmente a operação
```

Manipular HTML, JavaScript ou construir manualmente uma requisição não poderá contornar permissões.

---

## 26. Regras de domínio

Autorização de perfil não substitui validação do estado do negócio.

Exemplo:

```text
Administrador
+
serviço em estado incompatível
→ operação continua podendo ser rejeitada
```

A fila revalidará as condições relevantes no momento da execução.

---

## 27. Antiforgery

Razor Pages utilizará a proteção antiforgery integrada.

Formulários mutáveis deverão possuir token válido.

---

## 28. Chamadas Fetch

Chamadas JavaScript que modificarem dados também utilizarão antiforgery.

Header conceitual:

```text
X-CSRF-TOKEN
```

O token será emitido pelo servidor e enviado nas operações mutáveis.

---

## 29. Fluxo das operações HTTP

```text
POST / PUT / DELETE
       ↓
Autenticação
       ↓
Antiforgery
       ↓
Autorização
       ↓
Validação
       ↓
Command Queue
```

Operações GET não deverão alterar estado do negócio.

---

## 30. IgnoreAntiforgeryToken

A aplicação não utilizará:

```text
IgnoreAntiforgeryToken
```

como solução genérica para facilitar endpoints.

Exceções futuras deverão possuir justificativa específica.

---

## 31. Same-origin

Frontend, endpoints e SignalR permanecerão sob a mesma origem.

Exemplo:

```text
https://resetservice/
├── páginas
├── endpoints
└── hubs
```

---

## 32. CORS

Não será habilitado CORS globalmente na versão 1.0.

Se futuramente uma origem diferente precisar acessar o Reset Service, deverá existir uma allowlist explícita.

Não será adotado:

```text
AllowAnyOrigin
```

por conveniência.

---

## 33. SignalR

SignalR utilizará a mesma identidade autenticada do navegador.

Hubs relevantes exigirão autenticação.

---

## 34. SignalR como canal de sincronização

Operações de negócio continuarão preferencialmente usando HTTP.

```text
HTTP
→ comando / alteração

SignalR
→ propagação da alteração confirmada
```

Isso mantém a superfície de escrita concentrada e previsível.

---

## 35. Grupos SignalR

Grupos não serão tratados como mecanismo de autorização.

Exemplo:

```text
service:142
```

serve para direcionar eventos.

Antes de associar conexão a um contexto, o servidor continuará validando a identidade e o acesso aplicável.

---

## 36. Reconexão SignalR

Depois de desconexão:

```text
Reconectar
   ↓
revalidar autenticação
   ↓
reassociar grupos necessários
   ↓
sincronizar estado atual
```

O cliente não presumirá ter recebido todos os eventos enquanto esteve desconectado.

---

## 37. Desativação e conexões SignalR

Quando possível, desativação de conta ou outra mudança de autorização relevante deverá provocar encerramento das conexões SignalR daquele usuário.

O próximo acesso exigirá estado de autenticação válido.

---

## 38. HTTPS

O ambiente oficial de produção utilizará:

```text
HTTPS
```

mesmo dentro da LAN.

---

## 39. Endereço

Exemplo preferencial:

```text
https://resetservice/
```

ou endereço interno equivalente configurado pela infraestrutura.

---

## 40. Certificado

O certificado utilizado deverá ser confiável pelos computadores da Technolife.

Não será utilizado certificado de desenvolvimento em produção.

A preparação do host deverá evitar avisos comuns de navegador como certificado não confiável.

---

## 41. HTTP

O projeto não dependerá de receber credenciais por HTTP para depois redirecionar.

O endereço divulgado aos usuários será diretamente HTTPS.

Redirecionamento poderá existir como conveniência secundária, mas não será considerado proteção suficiente para a primeira transmissão.

---

## 42. HSTS

A implantação de produção utilizará política apropriada de HSTS quando compatível com o hostname e configuração interna escolhida.

O comportamento deverá ser validado no ambiente de implantação.

---

## 43. Data Protection

ASP.NET Core Data Protection será utilizado para mecanismos internos como:

- cookies;
- antiforgery;
- tokens transitórios do framework.

---

## 44. Persistência das chaves

As chaves não deverão ficar dependentes de diretórios temporários.

Local conceitual:

```text
C:\ProgramData\Technolife\ResetService\keys\
```

---

## 45. DPAPI

No Windows, o key ring persistido deverá ser protegido em repouso utilizando DPAPI em escopo da máquina quando adequado, por exemplo com `ProtectKeysWithDpapi(protectToLocalMachine: true)`.

---

## 46. Uso do Data Protection

Data Protection não será usado como mecanismo de criptografia genérica das entidades permanentes do domínio.

Não será usado para criptografar arbitrariamente:

- serviços;
- observações;
- snapshots;
- SQLite completo.

---

## 47. Recuperação e Data Protection

Após uma restauração completa em outro servidor, o novo ambiente poderá utilizar novo key ring.

Consequência aceitável:

```text
cookies antigos
→ inválidos
```

Esse comportamento está alinhado ao requisito de encerrar sessões após restauração.

---

## 48. Identidade do processo no Windows

O Reset Service será executado sob demanda pela conta interativa que abrir `ResetService.exe` na máquina hospedeira.

Não haverá requisito de conta virtual `NT SERVICE`, serviço registrado ou identidade residente. A conta operacional deverá possuir somente as permissões necessárias nos diretórios persistentes e nos recursos explicitamente configurados.

---

## 49. LocalSystem

Não deverá ser utilizado:

```text
LocalSystem
```

sem necessidade técnica comprovada.

A aplicação não precisa dos privilégios amplos dessa conta.

---

## 50. Princípio de menor privilégio no Windows

A identidade interativa que executar a aplicação receberá apenas os acessos necessários.

Exemplo:

```text
pasta local escolhida para ResetService
→ leitura / execução

ProgramData\Technolife\ResetService\data
→ leitura / escrita

...\logs
→ leitura / escrita

...\assets
→ leitura / escrita

...\keys
→ leitura / escrita

...\backups
→ leitura / escrita quando utilizado
```

O uso diário não exigirá `LocalSystem`, identidade `NT SERVICE` nem privilégios administrativos contínuos. ACLs restritivas protegerão SQLite, chaves, logs, assets, backups e configurações em `ProgramData`.

---

## 51. Compartilhamentos de rede

Se o backup utilizar pasta de rede, o acesso deverá ser concedido pelo Windows à identidade apropriada.

O Reset Service não terá como configuração comum:

```text
Usuário da rede
Senha da rede
```

armazenados internamente apenas para acessar uma pasta.

---

## 52. Diretórios públicos e privados

Somente arquivos explicitamente públicos deverão ficar em `wwwroot`.

```text
wwwroot/
├── CSS
├── JavaScript
└── recursos públicos
```

Dados privados permanecerão fora dele.

```text
ProgramData/.../data
ProgramData/.../keys
ProgramData/.../backups
ProgramData/.../assets privados
```

---

## 53. SQLite

O arquivo SQLite não será servido diretamente pelo Kestrel.

Não haverá endpoint público para baixar banco operacional.

Estações acessam somente o backend.

---

## 54. Proteção em repouso

A versão 1.0 utilizará principalmente:

- ACLs do Windows;
- conta interativa de menor privilégio;
- segurança do host.

Criptografia proprietária de todo o SQLite não será requisito.

Recursos de infraestrutura, como BitLocker, poderão complementar a proteção quando disponíveis.

---

## 55. Backup

Backups também deverão permanecer fora de diretórios públicos.

Somente Administradores poderão:

- listar;
- importar;
- exportar;
- restaurar;
- remover quando permitido.

---

## 56. Upload de logo

Formatos inicialmente aceitos:

```text
PNG
JPEG
```

Limite inicial:

```text
5 MB
```

---

## 57. Validação de upload

A aplicação não confiará apenas em:

```text
arquivo.png
```

como evidência de que o conteúdo realmente é uma imagem válida.

O backend deverá:

- verificar formato suportado;
- validar conteúdo;
- validar tamanho;
- gerar nome interno próprio;
- controlar o destino físico.

---

## 58. Nome original

O nome original poderá ser armazenado como metadado.

Não será utilizado diretamente como caminho ou nome físico confiável.

---

## 59. Importação de backup

Importação de backup possuirá endpoint administrativo separado.

Seu limite de tamanho será próprio e adequado ao uso.

A regra de 5 MB da logo não será aplicada a backups.

---

## 60. Content Security Policy

A aplicação deverá utilizar Content Security Policy restritiva compatível com sua interface.

Como não haverá dependência normal de conteúdo externo, a política deverá privilegiar:

```text
self
```

como origem.

---

## 61. JavaScript inline

Scripts inline deverão ser evitados quando razoável.

Preferência:

```text
arquivos JS próprios
```

Isso simplifica uma CSP mais forte.

---

## 62. Framing

A aplicação deverá impedir incorporação não necessária em frames de outras origens.

Não existe requisito de utilização do Reset Service dentro de outro portal.

---

## 63. MIME sniffing

Headers apropriados deverão reduzir interpretação indevida de tipos de conteúdo pelo navegador.

---

## 64. Headers de segurança

A configuração final deverá revisar pelo menos:

- Content-Security-Policy;
- proteção contra framing;
- MIME sniffing;
- políticas adequadas de referrer;
- HSTS quando aplicável.

Valores específicos serão testados antes da implantação.

---

## 65. Erros

Em produção não será exibida Developer Exception Page para usuários.

A interface receberá mensagens adequadas ao contexto.

Detalhes técnicos ficam nos logs.

---

## 66. Stack traces

Stack traces:

```text
logs técnicos → permitido
interface     → proibido
```

---

## 67. Logs

Os logs poderão conter identificadores úteis:

```text
timestamp
userId
serviceId
operationId
event
result
```

---

## 68. Informações proibidas em logs

Não deverão ser registrados normalmente:

- senha;
- cookie de autenticação;
- token antiforgery;
- key material;
- conteúdo integral de backup;
- segredos;
- observações completas sem necessidade.

---

## 69. Eventos técnicos relevantes

Poderão ser registrados:

- login bem-sucedido quando necessário;
- falhas repetidas;
- lockout;
- tentativa administrativa não autorizada;
- alteração de perfil;
- desativação;
- restauração;
- erros de persistência;
- falhas críticas de segurança.

O Reset Service não se transformará em sistema SIEM.

---

## 70. Segredos

Segredos não poderão ser:

- hardcoded;
- commitados no GitHub;
- exibidos em UI;
- gravados em logs.

Configuração sensível deverá utilizar mecanismos adequados ao ambiente de implantação.

---

## 71. Repositório

O GitHub não deverá receber:

```text
appsettings de produção com segredos
banco real
backup real
certificado privado
chaves Data Protection
dados de clientes
credenciais
```

---

## 72. Segurança da Command Queue

A fila não substitui as camadas anteriores.

Fluxo:

```text
Requisição
   ↓
Autenticação
   ↓
Antiforgery
   ↓
Autorização
   ↓
Validação
   ↓
Command Queue
   ↓
Revalidação do estado
   ↓
Transação
   ↓
COMMIT
```

---

## 73. Identidade dentro do comando

O comando deverá receber internamente o ID do usuário autenticado.

Não confiará em dados enviados pelo navegador como:

```text
actor = "Carlos"
```

O autor será derivado da sessão validada.

---

## 74. Operações aguardando na fila

Uma operação válida quando recebida poderá se tornar inválida antes de ser executada.

Exemplo:

```text
Comando A:
Concluir serviço

Comando B:
Alterar passo
```

Se A for processado primeiro e tornar o serviço `Completed`, B deverá ser reavaliado e rejeitado.

---

## 75. Concorrência

O token de versão definido em `data-model.md` continuará sendo utilizado.

A fila ordena o processamento, enquanto o token protege contra comandos baseados em estados antigos.

---

## 76. SignalR após COMMIT

Eventos em tempo real somente serão enviados depois da confirmação da persistência.

```text
COMMIT
   ↓
SignalR
```

Nunca:

```text
SignalR
   ↓
tentar salvar
```

---

## 77. Arquitetura consolidada de segurança

```text
                    HTTPS
                      │
                      ▼
              ┌───────────────┐
              │ Rate Limiting │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Identity      │
              │ Cookie Auth   │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Antiforgery   │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Authorization │
              │ Policies      │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Validation    │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Command Queue │
              └───────┬───────┘
                      ▼
              ┌───────────────┐
              │ Domain Rules  │
              │ Concurrency   │
              └───────┬───────┘
                      ▼
                    SQLite
                      │
                    COMMIT
                      │
                      ▼
                   SignalR
```

---

## 78. Parâmetros iniciais aprovados

| Item | Valor inicial |
|---|---|
| Senha mínima | 8 caracteres |
| Falhas para lockout | 5 |
| Duração do lockout | 5 minutos |
| Rate limit inicial de login | ~10/min/origem |
| Cookie HttpOnly | Sim |
| Cookie Secure | Sempre |
| SameSite | Strict |
| Remember me | Não |
| Expiração por inatividade | 8 horas |
| Sliding expiration | Sim |
| Security Stamp validation | 1 minuto |
| HTTPS | Obrigatório em produção |
| Identidade de execução no Windows | Conta interativa com menor privilégio |
| Logo | PNG/JPEG |
| Limite inicial de logo | 5 MB |
| CORS global | Não |
| Superusuário oculto | Não |

---

## 79. Regras Fundamentais

1. ASP.NET Core Identity será a infraestrutura de autenticação.
2. Não haverá cadastro público.
3. Primeiro Administrador será criado em bootstrap local protegido.
4. Não haverá credencial administrativa padrão.
5. Não haverá senha mestre.
6. Senhas terão mínimo de 8 caracteres.
7. Lockout inicial será de 5 falhas por 5 minutos.
8. Login possuirá rate limiting complementar.
9. Autenticação utilizará cookie protegido.
10. Cookie será `HttpOnly`.
11. Cookie será `Secure`.
12. `SameSite` será inicialmente `Strict`.
13. Não haverá `Remember me`.
14. Sessões terão expiração por inatividade.
15. Security Stamp será revalidado frequentemente.
16. Mudanças críticas deverão invalidar autorização antiga.
17. Roles serão `Administrator` e `Technician`.
18. Policies serão preferidas para operações protegidas.
19. Autorização efetiva será aplicada no backend.
20. Regras de domínio serão revalidadas durante execução.
21. Operações mutáveis terão antiforgery.
22. Não haverá CORS global aberto.
23. SignalR exigirá autenticação.
24. Grupos SignalR não serão autorização.
25. Alterações de negócio usarão preferencialmente HTTP.
26. SignalR distribuirá o estado confirmado.
27. HTTPS será obrigatório no ambiente oficial.
28. Certificado deverá ser confiável pelos clientes.
29. Data Protection terá key ring persistido.
30. Chaves serão protegidas no Windows.
31. O processo sob demanda utilizará conta interativa com somente os acessos necessários aos dados persistentes.
32. Banco, chaves e backups ficarão fora de diretórios públicos.
33. SQLite não será disponibilizado diretamente aos navegadores.
34. Uploads serão validados por conteúdo, tipo e tamanho.
35. CSP e demais headers de segurança serão configurados.
36. Stack traces não serão exibidos aos usuários.
37. Logs não armazenarão credenciais ou tokens.
38. Segredos não serão commitados no GitHub.
39. A fila receberá identidade derivada da sessão.
40. A fila revalidará o estado no momento da execução.
41. SignalR somente será notificado após COMMIT.

---

## 80. Estado da decisão

**PLANNING-014 — Arquitetura Técnica de Segurança: CONCLUÍDA E APROVADA.**

Este documento passa a orientar configuração do ASP.NET Core, Identity, Kestrel, SignalR, filesystem, execução sob demanda no Windows, deployment, testes de segurança e revisão de código.
