# Reset Service — Modelo de Dados

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Versão:** 2.0  
**Status:** Aprovado para implementação

## 1. Objetivo

Definir o modelo mínimo de persistência da base de conhecimento técnico.

A prioridade é manter o domínio pequeno, legível e suficiente para:

- criar e editar documentos;
- organizar conteúdo;
- pesquisar;
- manter histórico;
- restaurar versões;
- usar lixeira;
- registrar autoria;
- proteger contra sobrescrita silenciosa.

## 2. Entidades principais

```text
ApplicationUser

Category

Document
├── DocumentVersion
├── DocumentTag ── Tag
└── Attachment

Favorite
RecentDocument
```

Entidades administrativas adicionais só serão criadas quando uma necessidade concreta exigir.

## 3. ApplicationUser

A autenticação utilizará ASP.NET Core Identity.

Extensão conceitual:

```text
ApplicationUser
├── Id
├── DisplayName
├── IsActive
├── CreatedAtUtc
└── LastLoginAtUtc
```

Perfis iniciais:

```text
User
Administrator
```

## 4. Category

```text
Category
├── Id
├── Name
├── Slug
├── Description
├── ParentId
├── Position
├── IsActive
├── CreatedAtUtc
└── UpdatedAtUtc
```

`ParentId` permite subcategorias sem criar tipos diferentes de entidade.

A interface deverá desencorajar árvores excessivamente profundas.

## 5. Document

Entidade central do sistema.

```text
Document
├── Id
├── Title
├── Slug
├── Summary
├── Content
├── ContentFormat
├── Type
├── CategoryId
├── Status
├── CreatedByUserId
├── UpdatedByUserId
├── CreatedAtUtc
├── UpdatedAtUtc
├── DeletedAtUtc
├── DeletedByUserId
└── Version
```

### 5.1 Type

Valores iniciais:

```text
Procedure
Troubleshooting
Configuration
Checklist
Reference
Free
```

### 5.2 Status

A primeira versão deverá manter o ciclo simples:

```text
Active
```

A remoção operacional é representada por `DeletedAtUtc`, não por um fluxo complexo de publicação.

Caso a necessidade de rascunho/publicação apareça no uso real, poderá ser adicionada posteriormente.

### 5.3 Content

`Content` armazena o corpo editável do documento.

`ContentFormat` identifica o formato utilizado pelo editor, por exemplo HTML sanitizado ou JSON estruturado.

A escolha concreta será fechada junto com o editor. O banco não deve impor antecipadamente um editor específico.

## 6. DocumentVersion

Toda alteração relevante deverá poder gerar uma versão histórica.

```text
DocumentVersion
├── Id
├── DocumentId
├── VersionNumber
├── Title
├── Summary
├── Content
├── ContentFormat
├── CategoryId
├── DocumentType
├── CreatedAtUtc
├── CreatedByUserId
├── CreatedByDisplayName
└── ChangeSummary
```

Restrição:

```text
(DocumentId, VersionNumber) UNIQUE
```

Restauração nunca apaga histórico. Restaurar uma versão antiga cria uma nova versão atual.

## 7. Tag

```text
Tag
├── Id
├── Name
├── Slug
├── CreatedAtUtc
└── CreatedByUserId
```

`Slug` deverá ser único para evitar tags equivalentes apenas por variação de capitalização ou acentuação quando a normalização permitir.

## 8. DocumentTag

Relacionamento N:N:

```text
DocumentTag
├── DocumentId
└── TagId
```

Chave composta:

```text
(DocumentId, TagId)
```

## 9. Attachment

Imagens e outros anexos permitidos serão armazenados no filesystem controlado da aplicação.

```text
Attachment
├── Id
├── DocumentId
├── OriginalFileName
├── StoredFileName
├── ContentType
├── Size
├── Hash
├── CreatedAtUtc
└── CreatedByUserId
```

O banco guarda metadados e vínculo. O binário não será armazenado no SQLite por padrão.

## 10. Favorite

Favoritos são pessoais.

```text
Favorite
├── UserId
├── DocumentId
└── CreatedAtUtc
```

Restrição:

```text
(UserId, DocumentId) UNIQUE
```

## 11. RecentDocument

```text
RecentDocument
├── UserId
├── DocumentId
└── LastViewedAtUtc
```

A aplicação poderá manter apenas os registros mais recentes por usuário, sem necessidade de histórico completo de navegação.

## 12. Lixeira

Excluir um documento normalmente significa preencher:

```text
DeletedAtUtc
DeletedByUserId
```

Documentos excluídos deixam de aparecer em busca e navegação normais, mas continuam disponíveis na Lixeira.

Restauração limpa esses campos e cria registro de versão/auditoria quando aplicável.

Exclusão física definitiva será administrativa e deverá ser explícita.

## 13. Concorrência otimista

`Document.Version` protege contra edição baseada em estado obsoleto.

```text
Usuário A lê Version 7
Usuário B salva Version 8
Usuário A envia alteração baseada em 7
        ↓
backend rejeita sobrescrita silenciosa
```

A resposta deve permitir à interface preservar o conteúdo local e apresentar o estado atualizado.

Não haverá Command Queue para todas as gravações.

## 14. Pesquisa

A pesquisa inicial deverá considerar dados persistidos em:

- `Document.Title`;
- `Document.Summary`;
- `Document.Content` em representação textual pesquisável;
- `Category.Name`;
- `Tag.Name`;
- `Document.Type`.

A implementação poderá evoluir para recursos de full-text do SQLite se necessário, sem serviço externo obrigatório.

## 15. Auditoria

Não será criado inicialmente um event sourcing ou histórico de cada clique.

O histórico documental será atendido principalmente por `DocumentVersion` e pelos campos de autoria/data.

Se necessário, poderá ser criada posteriormente uma entidade pequena `AuditEntry` para ações administrativas relevantes.

## 16. Backup

O conjunto mínimo de backup é:

```text
SQLite database
+
uploads/
```

Esses elementos devem ser tratados como unidade lógica de restauração.

## 17. Princípios de integridade

- nomes e tamanhos terão limites explícitos;
- relacionamentos obrigatórios serão validados no backend;
- foreign keys serão habilitadas;
- operações de versionamento ocorrerão na mesma transação da atualização do documento quando aplicável;
- arquivos só serão considerados vinculados após persistência válida;
- exclusão definitiva não será usada como operação cotidiana.

## 18. Fora do modelo inicial

Não serão criadas entidades para:

- Service;
- ServiceTemplate;
- TemplateRevision;
- ServiceStage;
- ServiceStep;
- ServiceConclusion;
- ServiceObservation;
- fila de comandos;
- eventos de sincronização;
- clientes;
- equipamentos;
- tickets;
- CRM.

O domínio deverá permanecer centrado em conhecimento técnico reutilizável.
