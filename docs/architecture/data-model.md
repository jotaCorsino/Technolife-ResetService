# Reset Service — Data Model and Persistence

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Modelo de Dados e Persistência  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/architecture.md`

---

## 1. Objetivo

Este documento define o modelo conceitual de persistência do Reset Service.

Abrange:

- entidades;
- relacionamentos;
- identidade;
- modelos e revisões;
- serviços;
- roteiros;
- observações;
- histórico;
- conclusões;
- configurações;
- concorrência;
- integridade;
- transações.

Não define ainda migrations, nomes físicos definitivos de tabelas ou implementação EF Core.

---

## 2. Princípio

Dados operacionais serão armazenados de forma estruturada.

JSON não será utilizado como substituto genérico do modelo relacional.

Seu principal uso será em:

- snapshots históricos de conclusão;
- payloads pequenos e variáveis de eventos.

---

## 3. Identificadores

Entidades principais possuirão identificador interno próprio, preferencialmente UUID/GUID.

Exemplos:

```text
Service.Id
ServiceTemplate.Id
TemplateRevision.Id
ServiceObservation.Id
ServiceConclusion.Id
```

Identificadores internos não serão utilizados como identificação humana principal.

O serviço continuará utilizando:

```text
RS-AAAA-NNNNN
```

---

## 4. Modelos de Serviço

## 4.1 ServiceTemplate

Representa a identidade permanente do modelo.

Campos conceituais:

```text
ServiceTemplate
├── Id
├── Name
├── AdministrativeDescription
├── Status
├── CurrentRevisionId
├── CreatedAtUtc
├── CreatedByUserId
├── UpdatedAtUtc
└── Version
```

Estados:

```text
Draft
Active
Archived
```

---

## 4.2 TemplateRevision

Representa uma versão do procedimento.

```text
TemplateRevision
├── Id
├── TemplateId
├── RevisionNumber
├── Status
├── ChangeSummary
├── CreatedAtUtc
├── CreatedByUserId
├── PublishedAtUtc
├── PublishedByUserId
└── Version
```

Estados:

```text
Draft
Published
```

Um modelo poderá possuir no máximo um rascunho simultâneo.

---

## 4.3 Numeração

Rascunhos não recebem número de revisão.

```text
RevisionNumber = null
```

O número será atribuído somente na publicação.

Exemplo:

```text
Revisão publicada: 4
Rascunho
      ↓ publicar
Revisão publicada: 5
```

A combinação:

```text
TemplateId + RevisionNumber
```

deverá possuir unicidade para revisões publicadas.

---

## 4.4 Imutabilidade

Uma revisão Published será imutável.

Qualquer mudança posterior deverá ocorrer através de novo Draft e nova publicação.

---

## 4.5 TemplateStage

```text
TemplateStage
├── Id
├── TemplateRevisionId
├── Title
├── Instructions
└── Position
```

Cada revisão possui suas próprias etapas.

---

## 4.6 TemplateStep

```text
TemplateStep
├── Id
├── TemplateStageId
├── Title
├── Instructions
└── Position
```

Passos de modelo não possuem estado operacional.

---

## 5. Serviço

## 5.1 Service

Entidade operacional principal.

```text
Service
├── Id
├── ServiceNumber
├── ServiceYear
├── SequenceNumber
├── Title
├── Status
│
├── ClientName
├── ClientCompany
├── ClientPhone
├── ClientEmail
├── ClientReference
│
├── EquipmentDescription
├── EquipmentManufacturer
├── EquipmentModel
├── EquipmentSerialNumber
├── EquipmentAssetTag
├── EquipmentHostname
├── EquipmentOperatingSystem
├── EquipmentNotes
│
├── ResponsibleUserId
├── CreatedByUserId
├── CreatedAtUtc
├── StartedAtUtc
├── CurrentCompletedAtUtc
│
├── SourceTemplateId
├── SourceTemplateName
├── SourceRevisionId
├── SourceRevisionNumber
│
├── IsRouteCustomized
└── Version
```

---

## 5.2 Status

Estados persistidos:

```text
Draft
InProgress
Waiting
Completed
Cancelled
```

O estado do serviço faz parte efetiva de seu ciclo de vida e será armazenado.

---

## 5.3 Número do serviço

Exemplo:

```text
ServiceYear     = 2026
SequenceNumber  = 142
ServiceNumber   = RS-2026-00142
```

Restrições:

```text
ServiceNumber UNIQUE

(ServiceYear, SequenceNumber) UNIQUE
```

---

## 5.4 Cliente e equipamento

Não existirão entidades centrais `Customer` ou `Equipment` na versão 1.0.

Essas informações serão armazenadas diretamente no serviço para preservar sua fotografia histórica.

O Reset Service não funcionará como CRM ou inventário central.

---

## 5.5 Origem do roteiro

O serviço preservará:

```text
SourceTemplateId
SourceTemplateName
SourceRevisionId
SourceRevisionNumber
```

O nome histórico permanecerá mesmo que o modelo seja renomeado futuramente.

---

## 6. Cópia do roteiro

Um serviço nunca executará diretamente o roteiro armazenado na revisão do modelo.

Durante a criação:

```text
TemplateRevision
       ↓ cópia
ServiceStage
       ↓
ServiceStep
```

Depois da cópia, o roteiro pertence ao serviço.

---

## 6.1 ServiceStage

```text
ServiceStage
├── Id
├── ServiceId
├── SourceTemplateStageId
├── Title
├── Instructions
├── Position
├── DeletedAtUtc
├── DeletedByUserId
└── Version
```

`SourceTemplateStageId` poderá ser nulo para etapas adicionadas diretamente ao serviço.

---

## 6.2 Estado da etapa

O estado de uma etapa não será armazenado como fonte independente.

Será calculado através de seus passos.

Isso evita inconsistências entre:

```text
Etapa
```

e:

```text
Passos
```

---

## 6.3 Progresso

O progresso também será calculado.

```text
Concluídos aplicáveis
──────────────────────
Total de aplicáveis
```

Não será inicialmente persistido como fonte de verdade.

---

## 6.4 ServiceStep

```text
ServiceStep
├── Id
├── ServiceStageId
├── SourceTemplateStepId
├── Title
├── Instructions
├── Position
├── Status
├── UpdatedAtUtc
├── UpdatedByUserId
├── DeletedAtUtc
├── DeletedByUserId
└── Version
```

Estados:

```text
Pending
Completed
NotApplicable
```

---

## 7. Personalização

`Service.IsRouteCustomized` inicia como:

```text
false
```

Alterações estruturais fazem o valor tornar-se:

```text
true
```

Depois disso, ele não retorna automaticamente para `false`, mesmo que o roteiro seja manualmente modificado de volta para uma estrutura equivalente à original.

---

## 8. Observações

Será utilizada uma única entidade:

```text
ServiceObservation
├── Id
├── ServiceId
├── StageId
├── StepId
├── Scope
├── Visibility
├── ClientType
├── Text
├── CreatedAtUtc
├── CreatedByUserId
├── UpdatedAtUtc
├── UpdatedByUserId
├── DeletedAtUtc
├── DeletedByUserId
└── Version
```

---

## 8.1 Scope

Valores:

```text
Service
Stage
Step
```

As referências deverão ser coerentes com o nível selecionado.

---

## 8.2 Visibility

Valores:

```text
Internal
Client
```

Quando `Client`, poderá existir:

```text
Information
Recommendation
```

Quando `Internal`, `ClientType` será nulo.

---

## 8.3 Remoção

Observações utilizarão soft delete.

Uma remoção operacional deixa de apresentar a observação normalmente, mas preserva informação suficiente para rastreabilidade técnica.

---

## 9. Histórico funcional

Será utilizada a entidade:

```text
ServiceEvent
├── Id
├── ServiceId
├── EventType
├── OccurredAtUtc
├── ActorUserId
├── ActorDisplayName
├── Reason
└── DataJson
```

---

## 9.1 Eventos

Exemplos:

```text
ServiceCreated
ServiceStarted
ServiceWaiting
ServiceResumed
ServiceCompleted
ServiceCancelled
ServiceReopened
ResponsibleChanged
RouteCustomized
```

Não será necessário gerar evento histórico para todo clique ou mudança de checkbox.

---

## 9.2 DataJson

Eventos poderão possuir pequenos dados adicionais.

Exemplo:

```text
ResponsibleChanged
```

poderá preservar:

```json
{
  "previousResponsibleUserId": "...",
  "newResponsibleUserId": "..."
}
```

O JSON não substituirá os campos principais do evento.

---

## 10. Motivos

Motivos de:

- espera;
- cancelamento;
- reabertura;

pertencerão aos respectivos `ServiceEvent`.

Não serão criadas várias colunas específicas no `Service`.

---

## 11. Conclusões

Cada conclusão terá registro imutável.

```text
ServiceConclusion
├── Id
├── ServiceId
├── ConclusionNumber
├── CreatedAtUtc
├── CreatedByUserId
├── CreatedByDisplayName
├── SnapshotSchemaVersion
├── SnapshotJson
└── SnapshotHash
```

Restrição:

```text
(ServiceId, ConclusionNumber) UNIQUE
```

---

## 11.1 Snapshot

O snapshot conterá os dados necessários para reconstruir os documentos daquela conclusão.

Inclui conceitualmente:

```text
Service
Client
Equipment
Responsible
Route
Stages
Steps
Observations
Company
DocumentSettings
```

Depois da conclusão, o snapshot será imutável.

---

## 11.2 Versionamento do snapshot

O formato possuirá:

```text
SnapshotSchemaVersion
```

para permitir evolução futura sem perder capacidade de interpretar conclusões antigas.

---

## 11.3 Integridade

Poderá ser armazenado:

```text
SnapshotHash
```

para verificar integridade do conteúdo histórico.

Não representa assinatura digital.

---

## 11.4 PDFs

O binário PDF não será armazenado dentro do SQLite por padrão.

Regeneração utilizará:

```text
ServiceConclusion.SnapshotJson
```

O estado atual do serviço não será utilizado para regenerar uma conclusão histórica.

---

## 12. Usuários

A infraestrutura de autenticação utilizará ASP.NET Core Identity.

O usuário da aplicação será estendido conceitualmente com:

```text
ApplicationUser
├── Id
├── DisplayName
├── IsActive
├── MustChangePassword
├── LastLoginAtUtc
└── Version
```

Perfis:

```text
Administrator
Technician
```

---

## 12.1 Preservação histórica

Usuários historicamente referenciados não serão excluídos.

Registros importantes poderão armazenar simultaneamente:

```text
ActorUserId
ActorDisplayName
```

Isso preserva:

- vínculo com a identidade;
- nome exibido naquele momento.

---

## 13. Configuração institucional

## 13.1 CompanySettings

```text
CompanySettings
├── Id
├── DisplayName
├── LegalName
├── TaxId
├── Phone
├── Email
├── Website
├── Address
├── LogoAssetId
├── UpdatedAtUtc
├── UpdatedByUserId
└── Version
```

A instalação representa uma única Technolife.

Não haverá multiempresa.

---

## 13.2 DocumentSettings

Separado das informações empresariais.

Poderá conter:

```text
DocumentSettings
├── Id
├── ShowLogoInHeader
├── ShowCompanyName
├── ShowTaxId
├── ShowPhone
├── ShowEmail
├── ShowWebsite
├── ShowAddress
├── FooterSettings
├── DefaultConclusionText
├── UpdatedAtUtc
├── UpdatedByUserId
└── Version
```

---

## 14. Assets

Arquivos institucionais utilizarão entidade controlada.

```text
StoredAsset
├── Id
├── Kind
├── OriginalFileName
├── InternalFileName
├── ContentType
├── Size
├── Hash
├── CreatedAtUtc
└── CreatedByUserId
```

O conteúdo físico ficará no filesystem controlado da aplicação.

---

## 14.1 Logo histórica

Trocar a logo criará novo asset.

Assets utilizados por conclusões históricas não deverão ser sobrescritos.

Snapshots poderão referenciar o asset imutável correspondente.

---

## 15. SystemSettings

Configurações funcionais importantes serão tipadas.

Exemplo:

```text
SystemSettings
├── SessionIdleTimeoutMinutes
├── AutomaticBackupEnabled
├── BackupTime
├── BackupRetentionCount
├── BackupDestination
├── UpdatedAtUtc
├── UpdatedByUserId
└── Version
```

Não será criada uma estrutura genérica de chave/valor para todas as configurações sem necessidade.

---

## 16. Backups

Poderá existir catálogo administrativo:

```text
BackupRecord
├── Id
├── Type
├── CreatedAtUtc
├── CreatedByUserId
├── ApplicationVersion
├── FilePath
├── FileSize
├── Status
├── ValidationStatus
└── FailureMessage
```

O pacote físico de backup também possuirá manifesto próprio para que possa ser interpretado mesmo quando o banco original não estiver disponível.

---

## 17. Sequência do serviço

```text
ServiceNumberSequence
├── Year
└── LastNumber
```

A atualização da sequência e a criação do serviço ocorrerão na mesma transação.

---

## 18. Concorrência

Entidades mutáveis relevantes possuirão token:

```text
Version
```

Principais exemplos:

- Service;
- ServiceStage;
- ServiceStep;
- ServiceObservation;
- ServiceTemplate;
- TemplateRevision Draft;
- ApplicationUser;
- CompanySettings;
- DocumentSettings;
- SystemSettings.

O valor será utilizado na concorrência otimista definida em `architecture.md`.

---

## 19. Soft delete estrutural

`ServiceStage` e `ServiceStep` poderão utilizar soft delete quando já possuírem:

- execução;
- observações;
- relevância histórica.

Um elemento ainda Pendente e nunca utilizado poderá ser fisicamente removido quando a regra de negócio permitir.

---

## 20. Cascades

Cascade delete será utilizado de maneira conservadora.

Não poderá apagar indiretamente entidades históricas como:

- usuário;
- serviço;
- conclusão;
- modelo publicado;
- revisão publicada.

Exclusão física será reservada a situações expressamente permitidas, como um modelo Draft nunca publicado.

---

## 21. Datas

Eventos utilizarão horário determinado no servidor.

Instantes técnicos serão persistidos de maneira consistente, preferencialmente UTC.

A apresentação será convertida para o contexto operacional da Technolife.

---

## 22. Pesquisa e índices

A persistência deverá possuir índices adequados aos campos de consulta frequente.

Principais candidatos:

```text
ServiceNumber
Status
ResponsibleUserId
SourceTemplateId
CreatedAtUtc
CurrentCompletedAtUtc

ClientName
ClientCompany
ClientReference

EquipmentSerialNumber
EquipmentAssetTag
EquipmentHostname
```

Na versão 1.0, SQLite será suficiente para pesquisa sem motor externo.

---

## 23. Restrições de integridade

Deverão existir restrições no banco para regras essenciais.

Exemplos:

```text
ServiceNumber UNIQUE

(ServiceYear, SequenceNumber) UNIQUE

(TemplateId, RevisionNumber) UNIQUE

(ServiceId, ConclusionNumber) UNIQUE

UserName UNIQUE
```

Regras de domínio continuarão existindo também no backend.

---

## 24. Transações críticas

## Criar serviço

```text
obter/incrementar sequência
+
criar Service
+
copiar roteiro
+
registrar evento
+
COMMIT
```

---

## Publicar revisão

```text
validar Draft
+
determinar próximo número
+
publicar
+
atualizar CurrentRevisionId
+
COMMIT
```

---

## Concluir serviço

```text
validar passos
+
criar snapshot
+
criar ServiceConclusion
+
alterar status
+
registrar conclusão vigente
+
criar evento
+
COMMIT
```

---

## Cancelar

```text
alterar status
+
registrar evento/motivo
+
COMMIT
```

---

## Reabrir

```text
alterar status
+
limpar conclusão vigente
+
registrar evento/motivo
+
COMMIT
```

Conclusões anteriores permanecem preservadas.

---

## 25. DbContext e fila de comandos

A fila de comandos não utilizará um `DbContext` compartilhado permanentemente.

Cada comando terá seu próprio escopo.

```text
Command
   ↓
Scope
   ↓
DbContext
   ↓
Transaction
   ↓
Commit
   ↓
Dispose
```

Isso reduz acoplamento e problemas de rastreamento entre operações independentes.

---

## 26. SignalR

SignalR somente deverá publicar alteração depois de confirmação do banco.

```text
COMMIT
  ↓
SignalR
  ↓
clientes
```

Nunca o contrário.

---

## 27. Outbox

A versão 1.0 não terá outbox durável.

SignalR tem finalidade de sincronização da interface e não constitui integração externa crítica.

Se ocorrer:

```text
COMMIT
↓
processo falha
↓
evento SignalR não é enviado
```

os dados continuam corretos no banco.

Após reconexão ou atualização, o frontend recuperará o estado atual.

---

## 28. Reconexão em tempo real

Depois de perda e recuperação da conexão SignalR, o frontend deverá sincronizar novamente os dados relevantes.

Não deverá assumir que recebeu todos os eventos durante a desconexão.

---

## 29. Diagrama conceitual

```text
ApplicationUser
      │
      ├──────── autoria / responsabilidade
      │
      ▼
    Service
      │
      ├── ServiceStage
      │       └── ServiceStep
      │
      ├── ServiceObservation
      │
      ├── ServiceEvent
      │
      └── ServiceConclusion
      │       └── SnapshotJson
      │
      └── origem
             │
             ▼
       ServiceTemplate
             │
             └── TemplateRevision
                     └── TemplateStage
                             └── TemplateStep


CompanySettings
DocumentSettings
SystemSettings
StoredAsset
BackupRecord
ServiceNumberSequence
```

---

## 30. Regras Fundamentais

1. Identidade interna e identificador humano serão diferentes.
2. O número `RS-AAAA-NNNNN` continuará único.
3. Modelo e revisão serão entidades diferentes.
4. Revisão Draft recebe número somente ao ser publicada.
5. Revisões publicadas são imutáveis.
6. Serviço recebe cópia independente do roteiro.
7. Estado de etapa e progresso são calculados.
8. Estado do passo é persistido.
9. Cliente e equipamento pertencem diretamente ao serviço.
10. Não haverá CRM ou inventário central na v1.0.
11. Observações usarão estrutura única com Scope e Visibility.
12. Histórico funcional usará `ServiceEvent`.
13. Histórico não registrará indiscriminadamente todo checkbox.
14. Cada conclusão terá snapshot histórico imutável.
15. Snapshot será versionado.
16. PDFs históricos derivarão do snapshot.
17. Usuários históricos não serão excluídos.
18. Identidade atual e nome histórico do ator poderão ser preservados.
19. Empresa, documentos e sistema terão configurações separadas.
20. Assets históricos não serão sobrescritos.
21. Sequências e revisões terão garantias de unicidade no banco.
22. Entidades mutáveis relevantes usarão controle de versão.
23. Exclusões em cascata serão conservadoras.
24. Estruturas já utilizadas poderão utilizar soft delete.
25. `IsRouteCustomized` não retornará automaticamente para falso.
26. Motivos de ciclo de vida serão preservados nos eventos.
27. Operações de negócio críticas serão transacionais.
28. Cada comando utilizará seu próprio escopo de persistência.
29. SignalR somente será acionado após COMMIT.
30. Não haverá Outbox durável sem necessidade concreta.
31. Reconexões deverão provocar sincronização do estado real.

---

## 31. Estado da decisão

**PLANNING-013 — Modelo de Dados e Persistência: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência para entidades, mapeamentos EF Core, migrations, serviços de domínio, testes de persistência e implementação da fila de comandos.