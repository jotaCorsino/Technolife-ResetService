# Reset Service

Reset Service é o sistema interno da Technolife para registrar, acompanhar e documentar serviços técnicos de reset e configuração de equipamentos.

O produto v1.0 será uma aplicação web local, hospedada centralmente em uma máquina hospedeira Windows compatível e acessada pelos computadores da rede interna via navegador. A documentação deste repositório define o escopo aprovado antes do início da implementação.

## Estado do Projeto

Este repositório está em fase documental pré-implementação.

O planejamento da v1.0 está consolidado em `docs/`. O arquivo operacional futuro `docs/planning/current-state.md` deverá ser criado somente quando a implementação começar.

## Direção Técnica

- ASP.NET Core com interface web.
- SQLite como persistência inicial.
- ASP.NET Core Identity para autenticação e autorização.
- Roles oficiais: `Administrator` e `Technician`.
- Operação local em LAN, sem dependência de internet para funções essenciais.
- Estratégia multiusuário baseada em Command Queue, Optimistic Concurrency / Version e SignalR.
- Backup manual disponível e backup automático disponível, opcional e controlado pelo Administrator.

## Hospedagem e Clientes

A aplicação poderá ser hospedada em uma máquina hospedeira Windows compatível, incluindo desktop, notebook ou Windows Server.

Windows Server não é obrigatório.

Windows 10 faz parte da estratégia de compatibilidade e deverá ser validado conforme a versão do .NET adotada e os critérios internos do Reset Service. Clientes Windows antigos, como Windows 7, 8 e 8.1, serão tratados apenas como compatibilidade legada / melhor esforço.

## Documentação Principal

### Produto

- [Product Specification](docs/product/product-spec.md)
- [Service Workflow Specification](docs/product/service-workflow-spec.md)
- [Service Lifecycle Specification](docs/product/service-lifecycle-spec.md)
- [Service Template Specification](docs/product/service-template-spec.md)
- [Service Data Specification](docs/product/service-data-spec.md)
- [Document Generation Specification](docs/product/document-generation-spec.md)
- [User Access Specification](docs/product/user-access-spec.md)
- [UX Navigation Specification](docs/product/ux-navigation-spec.md)
- [Non-Functional Requirements](docs/product/non-functional-requirements.md)
- [Backup and Recovery Specification](docs/product/backup-recovery-spec.md)
- [Security Requirements](docs/product/security-requirements.md)
- [Product Destination and Implementation Readiness](docs/product/product-destination.md)

### Arquitetura

- [Architecture](docs/architecture/architecture.md)
- [Data Model](docs/architecture/data-model.md)
- [Security Architecture](docs/architecture/security.md)
- [Deployment and Operations](docs/architecture/deployment-operations.md)

### Planejamento

- [Development Roadmap](docs/planning/roadmap.md)
- [Backlog](docs/planning/backlog.md)
- [Sprint Plan](docs/planning/sprint-plan.md)
- [Current State Specification](docs/planning/current-state-spec.md)
- [README and Documentation Consolidation](docs/planning/documentation-consolidation-spec.md)

### Desenvolvimento

- [Testing Strategy and Quality Criteria](docs/development/testing-strategy.md)
- [Development Guide](docs/development/development-guide.md)

## Limites da v1.0

A v1.0 não prevê aplicativo mobile nativo, execução distribuída em múltiplas instâncias do backend, dependência de serviços externos obrigatórios, automações externas de e-mail/WhatsApp ou suporte oficial a ambientes legados.

## Próximo Passo

Após aprovação desta consolidação documental no GitHub, a implementação deve começar pela Sprint 01 definida em [Sprint Plan](docs/planning/sprint-plan.md), com acompanhamento pelo futuro `docs/planning/current-state.md`.
