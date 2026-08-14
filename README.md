# Reset Service

Aplicação web interna da Technolife para criação, execução, acompanhamento e documentação de procedimentos técnicos estruturados.

Roteiro. Foco. Progresso.

## Estado Atual

- Planejamento da v1.0: concluído.
- Implementação: ainda não iniciada.
- Próxima etapa: Sprint 01 — Estrutura da solução.

O arquivo operacional futuro `docs/planning/current-state.md` será criado somente quando a implementação começar.

## Funcionamento Geral

```text
Modelo
→ Revisão publicada
→ Serviço
→ Roteiro independente
→ Execução
→ Conclusão
→ Documentos
→ Histórico
```

## Arquitetura Resumida

```text
Browsers
→ HTTPS / LAN
→ ASP.NET Core / Razor Pages
→ Application / Domain
→ EF Core
→ SQLite
```

A estratégia multiusuário combina:

- Command Queue: ordena gravações persistentes.
- Optimistic Concurrency / Version: impede sobrescrita silenciosa baseada em estado obsoleto.
- SignalR: propaga alterações confirmadas aos navegadores.

## Stack Principal

- C#
- .NET 10
- ASP.NET Core 10
- Razor Pages
- EF Core 10
- SQLite
- SignalR
- ASP.NET Core Identity
- System.Threading.Channels
- PDFsharp / MigraDoc
- xUnit
- Playwright .NET quando aplicável

## Implantação

- Windows x64 compatível.
- Máquina hospedeira em desktop, notebook ou Windows Server.
- Execução como Windows Service.
- Publicação self-contained.
- Acesso pela LAN.
- HTTPS.
- Nenhuma instalação nas estações clientes.
- Funcionamento normal sem dependência da internet.

Windows Server não é obrigatório.

## Clientes

Suporte principal planejado:

- Windows 10.
- Windows 11.
- Chrome suportado.
- Edge suportado.

Ambientes Windows antigos serão tratados como compatibilidade legada / melhor esforço.

## Estrutura do Repositório

Neste momento, o repositório contém principalmente:

```text
README.md
docs/
```

Os diretórios `src/` e `tests/` serão criados durante a Sprint 01.

## Documentação Principal

- [Destino do produto](docs/product/product-destination.md)
- [Arquitetura](docs/architecture/architecture.md)
- [Modelo de dados](docs/architecture/data-model.md)
- [Segurança](docs/architecture/security.md)
- [Implantação e operações](docs/architecture/deployment-operations.md)
- [Roadmap](docs/planning/roadmap.md)
- [Backlog](docs/planning/backlog.md)
- [Plano de sprints](docs/planning/sprint-plan.md)
- [Estratégia de testes](docs/development/testing-strategy.md)
- [Guia de desenvolvimento](docs/development/development-guide.md)

## Desenvolvimento

O fluxo de implementação planejado segue:

```text
Roadmap
→ Backlog
→ Sprint
→ Tarefa técnica
→ Implementação
→ Testes
→ Verificação
```

A solution pode ser restaurada e compilada com os comandos já validados:

```text
dotnet restore ResetService.slnx
dotnet build ResetService.slnx -c Release
```

## Escopo da v1.0

A v1.0 cobre a operação interna essencial: modelos de procedimentos, serviços com roteiro independente, execução por etapas e passos, estados do serviço, histórico, documentos, usuários, segurança, operação multiusuário, backup, restauração, implantação local e atualização controlada.

Ficam fora da v1.0: aplicativo mobile nativo, execução distribuída em múltiplas instâncias do backend, dependência de serviços externos obrigatórios, automações externas de e-mail/WhatsApp, integrações fiscais ou suporte oficial a ambientes legados.

## Próximo Passo

Após aprovação da consolidação documental no GitHub, a implementação deve começar pela Sprint 01 definida em [Plano de sprints](docs/planning/sprint-plan.md).
