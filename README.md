# Reset Service

Aplicação web interna da Technolife para criação, execução, acompanhamento e documentação de procedimentos técnicos estruturados.

Roteiro. Foco. Progresso.

## Estado Atual

- Planejamento da v1.0: concluído.
- Implementação: em andamento.
- Sprint atual: Sprint 03 — Pipeline de comandos e infraestrutura.

O acompanhamento operacional está em [Current State](docs/planning/current-state.md).

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
- Publicação self-contained.
- Execução sob demanda por `ResetService.exe` na máquina hospedeira.
- Abertura automática do navegador padrão no host após a inicialização.
- Acesso pela LAN.
- HTTPS.
- Nenhuma instalação nas estações clientes.
- Somente uma instância ativa na máquina hospedeira.
- Encerramento planejado com bloqueio de novos comandos e drenagem da fila.
- Processo completamente encerrado quando a aplicação for fechada.
- Funcionamento normal sem dependência da internet.

Windows Server não é obrigatório.

Os clientes usam o navegador ou um atalho para a URL e dependem de a aplicação já estar aberta no host. Executar o binário por compartilhamento de rede a partir de uma estação cliente não é um modo suportado de uso.

## Clientes

Suporte principal planejado:

- Windows 10.
- Windows 11.
- Chrome suportado.
- Edge suportado.

Ambientes Windows antigos serão tratados como compatibilidade legada / melhor esforço.

## Estrutura do Repositório

O repositório contém a documentação, a solution e os projetos iniciais de produção e testes:

```text
README.md
docs/
src/
tests/
ResetService.slnx
```

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

O próximo passo operacional é mantido em [Current State](docs/planning/current-state.md).
