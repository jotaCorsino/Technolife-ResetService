# Reset Service — Plano de Sprints

**Versão alvo:** v1.0 Documentation Edition

## Sprint 00 — Pivot

Objetivo: encerrar formalmente o direcionamento antigo e preparar o repositório para a base de conhecimento.

Entregas:

- README atualizado;
- destino do produto atualizado;
- arquitetura simplificada;
- modelo de dados documental;
- current state atualizado;
- roadmap e backlog substituídos;
- sprint plan substituído;
- revisão posterior das especificações legadas.

Critério de saída:

> qualquer pessoa que abrir a documentação principal do repositório entende que o produto é uma base interna de conhecimento técnico e não um sistema de execução de serviços.

## Sprint 01 — Document Core

Backlog principal:

- KB-002;
- KB-003;
- KB-004;
- KB-005;
- KB-006;
- KB-007;
- KB-008;
- KB-009.

Sequência recomendada:

```text
simplificar solution
↓
entidades documentais
↓
DbContext
↓
migration
↓
listagem
↓
criação
↓
leitura
↓
edição
↓
lixeira
```

Critério de saída:

> um usuário consegue criar, editar, ler e restaurar uma documentação persistida no SQLite.

## Sprint 02 — Reading & Editor UX

Backlog principal:

- KB-010;
- KB-011;
- fundação visual da Home;
- layout global;
- estados de loading, vazio, sucesso e erro.

Critério de saída:

> documentação técnica pode ser produzida e lida confortavelmente, incluindo listas e comandos.

## Sprint 03 — Organization & Search

Backlog principal:

- KB-013;
- KB-014;
- KB-015;
- KB-016;
- KB-017;
- KB-028.

Critério de saída:

> um técnico consegue chegar rapidamente ao documento correto sem precisar conhecer previamente sua localização.

## Sprint 04 — Safety & History

Backlog principal:

- KB-018;
- KB-019;
- KB-020;
- KB-021;
- KB-022;
- KB-023.

Critério de saída:

> edição incorreta, exclusão acidental e conflito entre usuários possuem caminhos seguros de recuperação.

## Sprint 05 — Productivity & Internal Release

Backlog principal:

- KB-012;
- KB-024;
- KB-025;
- KB-026;
- KB-027;
- KB-030;
- KB-031;
- KB-032;
- KB-033.

Critério de saída:

> a aplicação está instalada na LAN e a equipe pode começar a registrar e consultar conhecimento real no trabalho diário.

## Regra de execução

Cada sprint deve produzir software demonstrável e não apenas infraestrutura.

Quando houver escolha entre uma abstração futura e uma funcionalidade que melhora a criação, busca, leitura ou proteção de documentação, priorizar a funcionalidade operacional.

## Qualidade

UI e UX não serão adiadas integralmente para o final.

Desde a Sprint 01, toda funcionalidade deve tratar pelo menos:

- estado normal;
- carregamento quando aplicável;
- vazio;
- validação;
- erro;
- sucesso/feedback da ação;
- ação destrutiva com recuperação quando possível.

A Sprint 05 fecha inconsistências e polimento, mas não transforma uma interface provisória ruim em produto final.
