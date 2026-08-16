# Reset Service — Current State

## Estado geral

- Versão alvo: v1.0 Documentation Edition
- Fase: 0 — Pivô de produto e simplificação
- Sprint: 00 — Pivot
- Status da sprint: Em andamento

## Decisão de produto

Em agosto de 2026, o escopo anterior orientado a criação e execução de serviços técnicos passo a passo foi encerrado antes da implementação do domínio.

O produto passa a ser uma base interna de conhecimento técnico para a Technolife.

Objetivo:

> criar, organizar, encontrar, ler, editar e preservar documentação técnica que agilize tarefas de assistência técnica, redes, servidores, firewall, DNS, e-mail, hospedagem e service desk.

## Fundação técnica preservada

- .NET 10;
- ASP.NET Core;
- Razor Pages;
- EF Core;
- SQLite;
- acesso pela LAN;
- hospedagem centralizada em Windows.

## Complexidade removida do núcleo

- fluxo operacional de Service;
- ServiceTemplate / TemplateRevision;
- execução por Stage / Step;
- SignalR como requisito central;
- Command Queue / System.Threading.Channels;
- sincronização contínua de navegadores;
- geração de PDF como requisito do MVP.

## Código existente

A implementação está em estágio inicial.

Já existe:

- solution .NET;
- projeto Web Razor Pages;
- projeto Core praticamente vazio;
- projeto Infrastructure com persistência-base;
- DbContext base;
- configuração EF Core + SQLite;
- migration baseline vazia.

Não existem tabelas do domínio antigo no banco.

## Trabalho atual

Sprint 00 deve consolidar:

- README;
- destino do produto;
- arquitetura;
- modelo de dados;
- roadmap;
- backlog;
- plano de sprints.

## Próximo passo técnico

Após a Sprint 00:

```text
simplificar estrutura da solution
↓
criar entidades Document, Category, Tag e DocumentVersion
↓
configurar DbContext
↓
criar primeira migration real
↓
implementar listagem / criação / leitura / edição de documentos
```

## Regra de implementação

Não implementar funcionalidade do produto antigo apenas porque já estava documentada.

Toda nova peça deve justificar sua existência pelo novo objetivo documental.
