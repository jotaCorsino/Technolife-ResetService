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

## Regra de reconciliação

O pivô substitui o domínio funcional antigo, mas não elimina automaticamente requisitos transversais já aprovados.

A classificação oficial está em:

`docs/planning/pivot-reconciliation.md`

Devem continuar sendo considerados, entre outros:

- operação pela LAN sem dependência normal de internet;
- instalação central e nenhuma instalação nas estações clientes;
- Windows como host, inclusive desktop/notebook quando adequado;
- Chrome/Edge como navegadores principais e compatibilidade legada em melhor esforço;
- desempenho percebido rápido;
- múltiplos usuários simultâneos;
- nenhuma sobrescrita silenciosa em conflitos;
- autenticação local e autoria individual;
- backup/restauração;
- logs e diagnóstico;
- atualização centralizada/offline;
- alta qualidade de UI/UX;
- desktop/notebook e uso confortável a partir de aproximadamente 1366×768.

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
- reconciliação dos requisitos preservados;
- instruções para Codex (`AGENTS.md`);
- guia de desenvolvimento;
- estratégia de testes;
- roadmap;
- backlog;
- plano de sprints.

## Próximo passo técnico

Somente após a Sprint 00 estar reconciliada:

```text
sincronizar checkout local do Codex
↓
confirmar branch e working tree
↓
simplificar estrutura da solution apenas se continuar vantajoso
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

Também não descartar requisito transversal apenas porque nasceu durante o planejamento antigo.

Toda nova peça deve justificar sua existência pelo objetivo documental e respeitar a reconciliação aprovada.
