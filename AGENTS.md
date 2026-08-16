# Reset Service — Instruções para agentes de código

Este repositório passou por um pivô de produto em agosto de 2026.

## Contexto obrigatório

O Reset Service NÃO é mais um sistema de execução de serviços técnicos passo a passo.

O produto atual é uma aplicação web interna da Technolife para criação, organização, consulta, edição e preservação de documentação técnica usada em assistência técnica, redes, servidores, firewall, DNS, e-mail, hospedagem e service desk.

Antes de alterar código, leia nesta ordem:

1. `docs/planning/current-state.md`
2. `docs/product/product-destination.md`
3. `docs/architecture/architecture.md`
4. `docs/architecture/data-model.md`
5. `docs/planning/backlog.md`
6. `docs/planning/sprint-plan.md`
7. `docs/development/development-guide.md`
8. `docs/development/testing-strategy.md`

## Fonte de verdade

O planejamento novo prevalece sobre documentos antigos que ainda mencionem:

- `Service` como entidade principal;
- `ServiceTemplate` / `TemplateRevision`;
- execução por `Stage` / `Step`;
- progresso de serviço;
- SignalR como requisito central;
- Command Queue / `System.Threading.Channels`;
- geração de PDF como requisito do MVP.

Não implemente essas funcionalidades apenas porque aparecem em documentação legada.

## Arquitetura aprovada

Preservar como padrão:

- C# / .NET 10;
- ASP.NET Core;
- Razor Pages;
- HTML, CSS e JavaScript;
- EF Core;
- SQLite local na máquina hospedeira;
- aplicação web centralizada acessada pela LAN;
- autenticação local quando a sprint correspondente for iniciada.

A direção é um monólito simples e fácil de manter.

Não introduzir sem decisão explícita:

- React, Angular ou Vue;
- SPA separada;
- microsserviços;
- Redis;
- RabbitMQ;
- MediatR;
- AutoMapper;
- CQRS framework;
- repository genérico;
- Unit of Work customizado;
- SignalR;
- Command Queue;
- infraestrutura distribuída.

## Núcleo de domínio atual

As primeiras entidades do produto são:

- `Document`;
- `Category`;
- `Tag`;
- `DocumentTag`;
- `DocumentVersion`.

Recursos posteriores incluem usuários, favoritos, recentes, anexos, lixeira, histórico, autosave, busca e backup.

## Regra de escopo

Trabalhe em uma tarefa de backlog por vez.

Faça tudo que for necessário para concluir a tarefa atual, mas não antecipe conscientemente a próxima sprint.

Se uma decisão alterar arquitetura, modelo de dados relevante, segurança, autenticação, UX principal ou deployment, pare e reporte a decisão necessária em vez de assumir silenciosamente.

## Git local

Antes de modificar arquivos:

```text
git branch --show-current
git status
```

Não descarte, sobrescreva nem incorpore silenciosamente alterações locais não relacionadas.

Quando o pivô ainda não estiver na branch local de trabalho, sincronize o repositório antes de implementar. Não continue usando planejamento antigo por estar presente no checkout local.

## Validação

Para mudanças .NET, execute no mínimo quando aplicável:

```text
dotnet restore ResetService.slnx
dotnet build ResetService.slnx -c Release
dotnet test ResetService.slnx -c Release
```

Acrescente testes específicos da tarefa quando necessários.

Não remova ou enfraqueça testes apenas para obter build verde.

## UI e UX

A simplicidade arquitetural não autoriza UI descuidada.

A interface deve priorizar:

- legibilidade;
- busca rápida;
- navegação previsível;
- estados vazios úteis;
- erros compreensíveis;
- feedback de salvamento;
- ações destrutivas recuperáveis quando possível;
- boa hierarquia visual;
- desktop/notebook como ambiente principal.

Evite aparência de ERP administrativo complexo.

## Relatório final esperado

Ao concluir uma tarefa, informe de forma objetiva:

- tarefa executada;
- arquivos alterados;
- decisões locais relevantes;
- testes/comandos executados e resultado;
- branch e estado do working tree;
- qualquer bloqueio ou decisão pendente.
