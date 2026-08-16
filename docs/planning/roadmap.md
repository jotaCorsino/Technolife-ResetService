# Reset Service — Roadmap

**Versão alvo:** v1.0 Documentation Edition

## Direção

Construir uma base interna de conhecimento técnico simples, rápida e segura, acessível pela LAN e orientada ao trabalho diário da Technolife.

O roadmap prioriza uma primeira versão utilizável cedo. Funcionalidades adicionais entram apenas após necessidade observada no uso real.

## Sprint 00 — Pivot

Objetivo: remover o direcionamento do produto antigo e consolidar a nova fundação.

Entregas:

- redefinir README e destino do produto;
- simplificar arquitetura;
- substituir modelo de dados;
- substituir backlog e sprint plan;
- marcar especificações antigas de Service como legado;
- confirmar o primeiro corte do MVP.

## Sprint 01 — Document Core

Objetivo: primeiro fluxo persistente de documentação.

Entregas:

- simplificar a solution se aprovado durante implementação;
- `Document`;
- `Category`;
- `Tag`;
- `DocumentTag`;
- `DocumentVersion`;
- DbContext e configurações EF Core;
- primeira migration real;
- listagem de documentos;
- criação;
- leitura;
- edição;
- soft delete básico.

Critério de saída:

> um documento criado em um navegador pode ser salvo no SQLite e consultado por outro navegador na LAN.

## Sprint 02 — Reading & Editor UX

Objetivo: tornar a aplicação confortável para produzir e consumir documentação técnica.

Entregas:

- layout principal;
- sidebar e topbar;
- página de leitura;
- editor rich text;
- headings;
- listas;
- listas numeradas;
- links;
- blocos de código;
- feedback de salvamento;
- estados vazios e erros principais;
- responsividade para desktop/notebook.

## Sprint 03 — Organization & Search

Objetivo: encontrar conhecimento rapidamente.

Entregas:

- pesquisa global;
- busca por título, resumo e conteúdo;
- categorias e subcategorias;
- tags;
- filtros por tipo/categoria/tag;
- tipos de documentação;
- página de categorias;
- ordenação simples.

## Sprint 04 — Safety & History

Objetivo: proteger documentação contra perda e sobrescrita.

Entregas:

- histórico de versões;
- restauração de versão;
- lixeira e restauração;
- concorrência otimista;
- autosave;
- tratamento de perda de conexão durante edição;
- backup inicial de banco + uploads.

## Sprint 05 — Productivity & Internal Release

Objetivo: disponibilizar uma versão confortável para uso diário.

Entregas:

- duplicação de documentos;
- favoritos;
- recentes;
- templates de documento;
- checklist;
- blocos de aviso/dica/observação;
- botão copiar em blocos de comando;
- autenticação e perfis finais do MVP;
- instalação na máquina host;
- endereço interno na LAN;
- validação com múltiplos computadores.

Critério de saída:

> equipe consegue começar a alimentar e usar a base de conhecimento em tarefas reais.

## Pós-MVP

Somente após uso interno, avaliar:

- melhoria de full-text search;
- anexos adicionais;
- impressão/exportação;
- templates avançados;
- avisos de edição simultânea;
- atalhos como Ctrl+K;
- auditoria administrativa ampliada;
- documentação específica por cliente, caso exista necessidade clara.

## Explicitamente fora do roadmap inicial

- workflow de execução de serviços;
- SignalR como infraestrutura central;
- Command Queue;
- CRM;
- ticketing;
- inventário;
- financeiro;
- aplicativo mobile nativo;
- integrações externas obrigatórias;
- IA;
- analytics avançado.
