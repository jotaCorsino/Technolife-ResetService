# Reset Service — Architecture

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Arquitetura e Stack Tecnológica  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** documentos em `docs/product/`

---

## 1. Objetivo

Este documento define a arquitetura técnica principal do Reset Service.

Seu escopo inclui:

- modelo de implantação;
- arquitetura da aplicação;
- backend;
- frontend;
- persistência;
- concorrência;
- atualização em tempo real;
- autenticação;
- geração de PDF;
- execução no Windows;
- organização inicial da solução;
- princípios técnicos obrigatórios.

---

## 2. Visão arquitetural

O Reset Service será uma:

> **Aplicação web local, centralizada, monolítica modular e multiusuário, hospedada em Windows e acessada pelos computadores da rede interna através de navegador.**

A aplicação não dependerá de internet para seu funcionamento normal.

---

## 3. Topologia

```text id="cv2f4n"
                    REDE LOCAL TECHNOLIFE

 ┌─────────────────┐
 │ PC Técnico 1    │
 │ Edge / Chrome   │
 └────────┬────────┘
          │
 ┌────────▼────────┐
 │ PC Técnico 2    │
 │ Edge / Chrome   │
 └────────┬────────┘
          │
 ┌────────▼────────┐
 │ Administração   │
 │ Edge / Chrome   │
 └────────┬────────┘
          │
          │ HTTPS / LAN
          ▼
 ┌───────────────────────────────────────┐
 │ RESET SERVICE                         │
 │                                       │
 │ Windows Service                       │
 │ ASP.NET Core                          │
 │ Razor Pages                           │
 │ SignalR                               │
 │ ASP.NET Core Identity                 │
 │ Command Queue                         │
 │ EF Core                               │
 └───────────────────┬───────────────────┘
                     │
                     ▼
              SQLite local / WAL
```

---

## 4. Acesso dos usuários

Não será necessário instalar o Reset Service nos computadores dos usuários.

O acesso ocorrerá através de navegador moderno.

Exemplo:

```text id="n03mcn"
https://resetservice/
```

ou outro nome interno definido na implantação.

O objetivo é permitir:

```text id="hjccgv"
qualquer computador autorizado da LAN
               ↓
            navegador
               ↓
     endereço do Reset Service
               ↓
              login
```

---

## 5. Nome de rede

A utilização cotidiana não deverá depender de endereço IP.

Deverá ser configurado um nome interno estável.

Preferência:

```text id="abzcmf"
https://resetservice/
```

ou, conforme infraestrutura de nomes adotada:

```text id="1zz02j"
https://resetservice.technolife.local/
```

Mudanças futuras de IP do servidor não deverão exigir alteração do endereço utilizado pelos usuários.

---

## 6. HTTPS

A implantação oficial deverá utilizar HTTPS dentro da LAN.

O certificado utilizado deverá ser confiável pelos computadores autorizados da Technolife.

Não será utilizado certificado de desenvolvimento em produção.

A estratégia concreta de certificado e resolução de nome será definida na documentação de implantação.

---

## 7. Stack principal

A stack definida para a versão 1.0 será:

| Área | Tecnologia |
|---|---|
| Linguagem | C# |
| Runtime | .NET 10 LTS |
| Backend web | ASP.NET Core 10 |
| Frontend | Razor Pages |
| Interatividade | JavaScript nativo / Fetch |
| Tempo real | ASP.NET Core SignalR |
| Fila interna | System.Threading.Channels |
| Persistência | Entity Framework Core 10 |
| Banco | SQLite |
| Autenticação | ASP.NET Core Identity |
| Sessão | Cookie |
| PDF | PDFsharp + MigraDoc |
| Servidor HTTP | Kestrel |
| Ambiente | Windows |
| Execução | Windows Service |
| Deploy | Self-contained `win-x64` |

---

## 8. Tipo de arquitetura

A aplicação será um:

> **Monólito modular.**

Não serão utilizados microsserviços na versão 1.0.

Teremos:

```text id="5sbziu"
uma aplicação
um processo principal
uma implantação
um banco operacional
```

mas com responsabilidades internas separadas.

---

## 9. Organização inicial da solução

Estrutura proposta:

```text id="tyap6a"
src/
├── ResetService.Web/
├── ResetService.Core/
└── ResetService.Infrastructure/

tests/
└── ResetService.Tests/

docs/
├── product/
├── architecture/
├── planning/
└── development/
```

A estrutura poderá ser refinada durante a criação inicial da solução sem quebrar os princípios deste documento.

---

## 10. ResetService.Web

Responsável por:

- Razor Pages;
- endpoints HTTP;
- autenticação da interface;
- autorização;
- composição da aplicação;
- SignalR;
- JavaScript;
- CSS;
- apresentação;
- integração entre interface e casos de uso.

---

## 11. ResetService.Core

Representará o núcleo das regras do produto.

Poderá conter módulos conceituais como:

```text id="pmp0yn"
Services
Templates
Workflow
Users
Documents
Settings
Backup
```

Será responsável principalmente por:

- regras de negócio;
- casos de uso;
- modelos do domínio;
- validações;
- contratos necessários à infraestrutura.

---

## 12. ResetService.Infrastructure

Responsável por detalhes técnicos como:

- EF Core;
- SQLite;
- ASP.NET Core Identity;
- persistência;
- filesystem;
- geração de PDF;
- backup;
- logs;
- integração com Windows.

---

## 13. Frontend

A versão 1.0 utilizará Razor Pages.

Não será construída uma SPA separada com React, Angular ou Vue.

A interface utilizará:

```text id="fzfw7p"
Razor Pages
+
HTML
+
CSS
+
JavaScript nativo
```

O objetivo é reduzir:

- dependências;
- toolchain;
- implantação;
- manutenção;
- complexidade.

---

## 14. Recursos offline

Nenhum recurso obrigatório da interface deverá depender de CDN ou internet.

Assets necessários serão entregues junto com a aplicação.

Isso inclui, quando aplicável:

- JavaScript;
- CSS;
- ícones;
- fontes utilizadas pelo produto;
- bibliotecas client-side necessárias.

---

## 15. Interatividade

A aplicação não dependerá de recarregar uma página inteira para toda operação.

Ações frequentes poderão usar chamadas assíncronas.

Exemplo:

```text id="v3os2z"
Usuário marca passo
      ↓
Fetch HTTP
      ↓
Servidor
      ↓
Validação
      ↓
Persistência
      ↓
Resposta
      ↓
Interface atualizada
```

---

## 16. Uso simultâneo do mesmo serviço

Será requisito oficial que dois ou mais usuários possam abrir e trabalhar no mesmo serviço simultaneamente.

Não haverá bloqueio exclusivo do tipo:

```text id="lbai2f"
Serviço bloqueado porque João está utilizando.
```

O objetivo será permitir:

```text id="5cs1ri"
João
→ executa uma ação

Carlos
→ executa outra ação

ambos permanecem sincronizados
```

---

## 17. Atualização em tempo real

Alterações confirmadas deverão ser propagadas automaticamente aos outros usuários que estiverem visualizando o mesmo contexto.

Será utilizado ASP.NET Core SignalR.

Exemplos de alterações:

```text id="9efn0r"
Passo concluído
Passo marcado Não aplicável
Observação adicionada
Responsável alterado
Status alterado
Dados do serviço alterados
Roteiro alterado
```

---

## 18. Grupos SignalR

As conexões poderão ser agrupadas por contexto.

Exemplo conceitual:

```text id="32nquh"
service:142
```

Todos os navegadores atualmente acompanhando esse serviço poderão receber eventos relacionados a ele.

Outros usuários não precisam receber eventos desnecessários.

---

## 19. SignalR não é a fonte de verdade

SignalR servirá para comunicação.

Não armazenará o estado oficial.

A regra será:

```text id="x0ee5h"
Banco
= fonte de verdade

SignalR
= mecanismo de propagação
```

Se existir qualquer dúvida sobre o estado, o frontend poderá solicitar novamente os dados atuais ao servidor.

---

## 20. Eventos de atualização

A aplicação poderá trabalhar com eventos conceituais como:

```text id="rv1mci"
StepStateChanged
ObservationAdded
ResponsibleChanged
ServiceStatusChanged
ServiceDetailsChanged
RouteChanged
```

Eventos pequenos poderão atualizar diretamente a interface.

Mudanças maiores poderão apenas instruir o navegador a recarregar determinada área.

---

## 21. Fila de alterações

Operações persistentes que alteram o estado da aplicação deverão passar por uma fila interna de comandos.

A primeira implementação utilizará:

```text id="7yg601"
System.Threading.Channels
```

Não serão necessários na versão 1.0:

```text id="t80ox4"
RabbitMQ
Redis
Kafka
service bus externo
```

---

## 22. Fluxo de escrita

O fluxo conceitual será:

```text id="5kp44g"
Usuário
   ↓
Requisição
   ↓
Validação inicial
   ↓
Command Queue
   ↓
Processamento ordenado
   ↓
Validação do estado atual
   ↓
Transação
   ↓
SQLite
   ↓
COMMIT
   ↓
SignalR
   ↓
Frontends atualizados
```

---

## 23. Confirmação da operação

Uma operação não deverá ser apresentada como salva antes da persistência real.

Fluxo obrigatório:

```text id="rp1866"
Usuário executa ação
        ↓
Comando é processado
        ↓
Banco confirma COMMIT
        ↓
Servidor confirma sucesso
        ↓
Evento SignalR
```

Se uma operação falhar antes do COMMIT, não será considerada confirmada.

---

## 24. Fila em memória

A fila inicial será interna ao processo.

Isso é suficiente porque a arquitetura prevê:

```text id="gwy1r4"
uma única instância ativa
do Reset Service
```

Não haverá cluster ou múltiplas instâncias concorrentes do backend na versão 1.0.

Comandos ainda não persistidos não serão considerados concluídos.

---

## 25. Serialização das escritas

A fila permitirá controlar as operações mutáveis de maneira previsível, reduzindo contenção de escrita no SQLite.

A aplicação deverá manter as transações curtas.

Não deverão ser colocadas dentro de uma transação longa operações como:

- geração de PDF;
- espera por comunicação com navegador;
- processamento demorado;
- acesso desnecessário a arquivos.

---

## 26. Fila não substitui concorrência

A fila ordena operações recebidas pelo servidor, mas não elimina a possibilidade de um navegador trabalhar com um estado antigo.

Por isso também haverá controle de concorrência otimista.

---

## 27. Concorrência otimista

Entidades mutáveis relevantes possuirão um token de versão gerenciado pela aplicação.

Exemplo conceitual:

```text id="cu0oic"
Service

Id
...
Version
```

Fluxo:

```text id="jk6oxf"
Carlos carrega Version 15

João altera
Version passa para 16

Carlos envia operação baseada em 15
          ↓
servidor detecta divergência
          ↓
não sobrescreve silenciosamente
```

---

## 28. Tratamento de conflito

Quando a operação puder ser aplicada com segurança ao estado atual, o backend poderá tratá-la adequadamente.

Quando houver conflito real, a operação será rejeitada e a interface receberá estado atualizado.

Exemplo:

```text id="ujmx6t"
Este serviço foi alterado por outro usuário.

Os dados foram atualizados.
Revise a informação antes de tentar novamente.
```

---

## 29. Atualização contínua

A combinação será:

```text id="r7ac5i"
Command Queue
     ↓
ordena alterações

Optimistic Concurrency
     ↓
protege contra estado antigo

SignalR
     ↓
mantém navegadores sincronizados
```

Esses três mecanismos são complementares.

---

## 30. Operações idempotentes

Operações críticas deverão, quando adequado, possuir identificação de operação.

Exemplo conceitual:

```text id="y9ehs2"
OperationId
```

Isso poderá impedir que reenvios ou duplos cliques produzam efeitos duplicados.

Especialmente:

- criação de serviço;
- conclusão;
- publicação de revisão;
- cancelamento;
- restauração.

---

## 31. Banco de dados

A versão 1.0 utilizará:

```text id="lmd2o2"
SQLite
+
Entity Framework Core
```

O arquivo ativo do banco permanecerá no armazenamento local do servidor.

---

## 32. Banco não ficará em compartilhamento de rede

Não será permitido utilizar o arquivo SQLite operacional diretamente em:

```text id="5zidqe"
\\servidor\pasta\resetservice.db
```

O banco deverá permanecer no disco local da máquina que executa o Reset Service.

Estações nunca acessam diretamente o arquivo.

---

## 33. WAL

O SQLite será configurado para operar em:

```text id="f0afhd"
WAL
```

quando aplicável à implementação final.

Isso melhora a convivência entre leituras e escrita.

---

## 34. Razão para SQLite

A escala prevista é:

- aproximadamente 1–20 usuários cadastrados;
- aproximadamente 1–10 simultâneos;
- operações pequenas;
- histórico de dezenas de milhares de serviços.

A combinação:

```text id="rhh2xa"
SQLite
+
transações curtas
+
fila de escrita
+
controle de concorrência
```

é considerada proporcional ao problema.

---

## 35. Critério de reavaliação

SQLite não será tratado como decisão dogmática.

Antes da implantação definitiva deverão existir testes de concorrência representativos.

Se houver contenção incompatível com os requisitos de experiência, o banco poderá ser reavaliado.

Possíveis candidatos futuros incluem:

- SQL Server;
- PostgreSQL.

Não será adicionada abstração excessiva apenas para suportar teoricamente vários bancos.

---

## 36. IDs de serviço

O formato continuará:

```text id="u3dru0"
RS-AAAA-NNNNN
```

A geração será transacional.

Deverá existir proteção no banco contra duplicação.

Exemplo conceitual:

```text id="dnljiq"
ServiceNumberSequence

Year
LastNumber
```

---

## 37. Revisões

A publicação de revisões também será transacional.

A combinação:

```text id="ghfign"
ModelId
RevisionNumber
```

deverá possuir unicidade garantida na persistência.

---

## 38. Datas

Eventos serão registrados usando fonte temporal do servidor.

A persistência utilizará representação temporal consistente, preferencialmente UTC para instantes técnicos.

A interface apresentará horário adequado ao contexto da Technolife.

---

## 39. Autenticação

Será utilizado:

```text id="oekfid"
ASP.NET Core Identity
```

com interface própria do Reset Service.

Não haverá:

- cadastro público;
- login Google;
- login Microsoft;
- recuperação via e-mail;
- dependência externa.

---

## 40. Identity como infraestrutura

ASP.NET Core Identity será utilizado para mecanismos como:

- usuários;
- hash de senha;
- autenticação;
- bloqueio temporário;
- credenciais temporárias;
- invalidação de segurança.

Os perfis funcionais continuam sendo somente:

```text id="l6nbyu"
Administrador
Técnico
```

---

## 41. Sessões

A autenticação utilizará cookie protegido.

Regras de segurança definidas em `security-requirements.md` deverão ser implementadas, incluindo:

- expiração;
- logout;
- invalidação;
- bloqueio;
- troca obrigatória de credenciais temporárias.

---

## 42. PDF

A implementação inicialmente utilizará:

```text id="7axcri"
PDFsharp
+
MigraDoc
```

A biblioteca deverá ser utilizada exclusivamente no servidor.

Nenhum computador cliente precisará possuir gerador de PDF instalado.

---

## 43. Snapshot de conclusão

Cada conclusão deverá possuir fotografia histórica imutável.

Estrutura conceitual:

```text id="mgkftu"
ServiceConclusion
│
├── ConclusionNumber
├── Date
├── Responsible
├── Client
├── Equipment
├── Route
├── Steps
├── Observations
├── CompanyData
└── DocumentSettings
```

---

## 44. Persistência do snapshot

O snapshot poderá ser persistido como documento JSON versionado associado à conclusão.

Exemplo conceitual:

```text id="4abp00"
ServiceConclusion

Id
ServiceId
ConclusionNumber
CreatedAtUtc
CreatedByUserId
SnapshotSchemaVersion
SnapshotJson
```

Esse conteúdo será imutável depois da conclusão correspondente.

---

## 45. Geração histórica

Fluxo:

```text id="at9fng"
Serviço
   ↓
Conclusão
   ↓
Snapshot imutável
   ↓
Gerador documental
   ↓
PDF
```

Regenerar um documento antigo utilizará o snapshot da conclusão correspondente, não o estado atual do serviço.

---

## 46. Backup SQLite

Backup do banco não será realizado simplesmente copiando arbitrariamente o arquivo SQLite durante uso.

Será utilizado mecanismo consistente de backup do SQLite.

Depois, o pacote poderá incluir:

```text id="b5xnk5"
snapshot do banco
+
manifesto
+
arquivos persistentes essenciais
```

conforme `backup-recovery-spec.md`.

---

## 47. Hosting

A aplicação será hospedada diretamente pelo Kestrel.

Não será requisito inicial utilizar IIS.

Topologia:

```text id="3p0ssa"
Browser
   ↓
HTTPS
   ↓
Kestrel
   ↓
ASP.NET Core
```

---

## 48. Serviço do Windows

A aplicação será instalada como Serviço do Windows.

Nome conceitual:

```text id="2q4vdr"
Technolife Reset Service
```

Inicialização:

```text id="hh5m87"
Automatic
```

Após reinicialização normal da máquina:

```text id="5d50t6"
Windows inicia
      ↓
Reset Service inicia
      ↓
endereço volta a responder na LAN
```

---

## 49. Deploy

A aplicação será publicada inicialmente para:

```text id="s58w7y"
win-x64
```

como implantação self-contained.

O ambiente de produção não deverá depender obrigatoriamente de instalação separada do runtime .NET.

---

## 50. Separação de aplicação e dados

Estrutura conceitual:

```text id="3hl0av"
C:\Program Files\Technolife\ResetService\
└── aplicação

C:\ProgramData\Technolife\ResetService\
├── data\
├── logs\
├── assets\
├── backups\
└── config\
```

Arquivos binários e dados persistentes deverão permanecer separados.

---

## 51. Migrações

Mudanças no schema serão gerenciadas através de migrations do EF Core.

Produção não deverá executar mudanças de schema de maneira invisível e descontrolada apenas porque a aplicação iniciou.

Atualizações deverão possuir processo explícito.

---

## 52. Fluxo de atualização

Conceitualmente:

```text id="nrqtkp"
Nova versão
     ↓
Backup recomendado
     ↓
Modo de manutenção
     ↓
Parar serviço
     ↓
Aplicar migração
     ↓
Atualizar binários
     ↓
Iniciar serviço
     ↓
Verificar funcionamento
```

---

## 53. API

Não haverá frontend e backend como dois produtos independentes.

A aplicação será única.

Endpoints internos poderão existir para:

- checklist;
- observações;
- status;
- SignalR;
- operações administrativas.

Todos pertencem ao mesmo backend.

---

## 54. Dependências principais

O conjunto base deverá permanecer pequeno:

```text id="p7wvxx"
.NET
ASP.NET Core
EF Core
SQLite
ASP.NET Core Identity
SignalR
PDFsharp / MigraDoc
```

Dependências adicionais deverão possuir justificativa concreta.

---

## 55. Componentes não utilizados

A versão 1.0 não utilizará sem necessidade comprovada:

```text id="z3zv9h"
Docker
Kubernetes
Redis
RabbitMQ
Kafka
Elasticsearch
React
Angular
Vue
Node.js como runtime
microsserviços
banco em nuvem
autenticação externa
serviços externos obrigatórios
```

---

## 56. Princípios arquiteturais

1. Uma única implantação central.
2. Navegadores são clientes.
3. Nenhum acesso direto das estações ao banco.
4. Nenhuma dependência de internet em runtime.
5. Backend é a autoridade das regras de negócio.
6. Banco é a fonte persistente de verdade.
7. SignalR distribui mudanças, mas não substitui persistência.
8. Escritas passam por fila controlada.
9. Fila não substitui controle de concorrência.
10. Alterações só são confirmadas depois do COMMIT.
11. Usuários podem trabalhar simultaneamente no mesmo serviço.
12. Estado atualizado deve chegar automaticamente aos navegadores interessados.
13. Operações concorrentes não podem causar sobrescrita silenciosa.
14. Transações devem permanecer curtas.
15. Arquitetura deverá continuar proporcional ao porte real da aplicação.

---

## 57. Decisão Final

A arquitetura-base aprovada para o Reset Service v1.0 será:

```text id="7muv3j"
                    LAN / HTTPS
                         │
            ┌────────────▼────────────┐
            │    ResetService.Web     │
            │                         │
            │ ASP.NET Core 10         │
            │ Razor Pages             │
            │ Identity                │
            │ SignalR                 │
            └────────────┬────────────┘
                         │
            ┌────────────▼────────────┐
            │    Command Processing   │
            │                         │
            │ System.Threading.       │
            │ Channels                │
            │                         │
            │ Optimistic Concurrency  │
            └────────────┬────────────┘
                         │
            ┌────────────▼────────────┐
            │    ResetService.Core    │
            │                         │
            │ Regras de negócio       │
            │ Casos de uso            │
            └────────────┬────────────┘
                         │
            ┌────────────▼────────────┐
            │ Infrastructure          │
            │                         │
            │ EF Core                 │
            │ SQLite / WAL            │
            │ PDFsharp / MigraDoc     │
            │ Backup / Filesystem     │
            └────────────┬────────────┘
                         │
                         ▼
                    SQLite local
```

---

## 58. Estado da decisão

**PLANNING-012 — Arquitetura e Stack Tecnológica: CONCLUÍDA E APROVADA.**

As decisões sobre uso simultâneo, sincronização em tempo real, fila de alterações e acesso por endereço amigável na LAN fazem parte formal da arquitetura.