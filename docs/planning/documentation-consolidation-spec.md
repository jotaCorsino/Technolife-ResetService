# Reset Service — README and Documentation Consolidation

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** README Inicial e Consolidação Documental  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`, `docs/planning/*`, `docs/development/*`

---

## 1. Objetivo

Este documento define:

1. o conteúdo esperado do README inicial do repositório;
2. o inventário documental necessário antes da implementação;
3. as normalizações conhecidas entre decisões antigas e posteriores;
4. os limites da revisão documental;
5. os requisitos da futura tarefa única de consolidação a ser executada pelo Codex.

Essa consolidação será a última atividade documental relevante antes do início da Sprint 01.

---

## 2. Papel do README

O arquivo:

```text
README.md
```

será a porta de entrada do repositório Reset Service.

Ele deverá permitir compreender rapidamente:

- o que é o produto;
- qual problema resolve;
- qual seu estado atual;
- qual sua arquitetura;
- qual sua stack principal;
- como a documentação está organizada;
- onde estão roadmap e backlog;
- como será conduzido o desenvolvimento.

O README não substituirá as especificações detalhadas.

---

## 3. Descrição inicial

O README deverá apresentar o Reset Service como:

> Aplicação web interna da Technolife para criação, execução, acompanhamento e documentação de procedimentos técnicos estruturados.

Também deverá deixar claro que a solução utiliza:

```text
Instalação central
+
LAN
+
Navegadores
```

e não exige instalação individual nas estações dos usuários.

---

## 4. Identidade do produto

O README poderá apresentar de maneira discreta a direção de experiência:

> **Roteiro. Foco. Progresso.**

Essa frase representa a identidade operacional e de UX do produto.

---

## 5. Funcionamento geral

O README deverá resumir o fluxo principal:

```text
Modelo
↓
Revisão publicada
↓
Serviço
↓
Roteiro independente
↓
Execução
↓
Conclusão
↓
Documentos
↓
Histórico
```

Sem reproduzir integralmente `product-spec.md`.

---

## 6. Estado inicial do projeto

Antes do início do código, o README deverá informar:

```text
Status:
Planejamento da v1.0 concluído.

Implementação:
Ainda não iniciada.

Próxima etapa:
Sprint 01 — Estrutura da solução.
```

Depois que a implementação começar, essa seção deverá ser atualizada.

---

## 7. Current State após início da implementação

Quando existir:

```text
docs/planning/current-state.md
```

o README deverá indicar esse arquivo como referência para o estado operacional atual.

O README não deverá tentar substituir o Current State.

---

## 8. Arquitetura resumida

O README deverá apresentar apenas a visão geral:

```text
Browsers
   ↓
HTTPS / LAN
   ↓
ASP.NET Core / Razor Pages
   ↓
Application / Domain
   ↓
EF Core
   ↓
SQLite
```

Também deverá resumir:

```text
Command Queue
→ ordena gravações

Optimistic Concurrency
→ impede sobrescrita silenciosa

SignalR
→ sincroniza navegadores
```

Detalhes permanecem em `docs/architecture/architecture.md`.

---

## 9. Stack principal

O README poderá listar:

- C#;
- .NET 10;
- ASP.NET Core 10;
- Razor Pages;
- EF Core 10;
- SQLite;
- SignalR;
- ASP.NET Core Identity;
- `System.Threading.Channels`;
- PDFsharp/MigraDoc;
- xUnit;
- Playwright .NET quando aplicável.

Implantação prevista:

```text
Windows x64
Windows Service
self-contained
LAN
HTTPS
```

---

## 10. Máquina hospedeira

O README não deverá tratar Windows Server como obrigatório.

Formulação correta:

> O Reset Service será hospedado centralmente em uma máquina Windows x64 compatível com a plataforma adotada.

Essa máquina poderá ser:

- desktop;
- notebook;
- Windows Server.

---

## 11. Clientes

A estratégia principal de clientes será:

- Windows 10;
- Windows 11;
- Chrome suportado;
- Edge suportado.

Windows antigos serão classificados como:

> **compatibilidade legada / melhor esforço**

e não como suporte oficial.

---

## 12. Internet

O README deverá informar que o funcionamento operacional normal não depende de internet.

A aplicação deverá operar através da LAN.

Atualizações poderão ser realizadas por pacote offline.

---

## 13. Estrutura do repositório

Antes da implementação:

```text
ResetService/
├── docs/
└── README.md
```

Depois da criação da solution, a estrutura deverá evoluir para algo como:

```text
ResetService/
├── src/
├── tests/
├── docs/
└── README.md
```

Diretórios não deverão ser criados vazios apenas para antecipar essa estrutura.

---

## 14. Índice documental — Produto

O README deverá apontar para:

```text
docs/product/
├── product-spec.md
├── service-workflow-spec.md
├── service-lifecycle-spec.md
├── service-template-spec.md
├── service-data-spec.md
├── document-generation-spec.md
├── user-access-spec.md
├── ux-navigation-spec.md
├── non-functional-requirements.md
├── backup-recovery-spec.md
├── security-requirements.md
└── product-destination.md
```

---

## 15. Índice documental — Arquitetura

```text
docs/architecture/
├── architecture.md
├── data-model.md
├── security.md
└── deployment-operations.md
```

---

## 16. Índice documental — Planejamento

```text
docs/planning/
├── roadmap.md
├── backlog.md
├── sprint-plan.md
├── current-state-spec.md
└── documentation-consolidation-spec.md
```

Depois do início da implementação:

```text
docs/planning/current-state.md
```

será adicionado.

---

## 17. Índice documental — Desenvolvimento

```text
docs/development/
├── testing-strategy.md
└── development-guide.md
```

---

## 18. Guia do usuário

O futuro:

```text
docs/user-guide.md
```

não deverá ser criado antes da interface real estar suficientemente estável.

O manual deverá refletir o produto implementado, não uma interface imaginada.

---

## 19. Documentação administrativa futura

Um manual operacional ou administrativo completo também deverá ser criado próximo da preparação da v1.0.

Ele deverá se basear na instalação, atualização, backup e recuperação realmente implementados.

---

## 20. Inventário esperado antes da implementação

O conjunto documental esperado será:

```text
README.md

docs/
├── product/
│   ├── product-spec.md
│   ├── service-workflow-spec.md
│   ├── service-lifecycle-spec.md
│   ├── service-template-spec.md
│   ├── service-data-spec.md
│   ├── document-generation-spec.md
│   ├── user-access-spec.md
│   ├── ux-navigation-spec.md
│   ├── non-functional-requirements.md
│   ├── backup-recovery-spec.md
│   ├── security-requirements.md
│   └── product-destination.md
│
├── architecture/
│   ├── architecture.md
│   ├── data-model.md
│   ├── security.md
│   └── deployment-operations.md
│
├── planning/
│   ├── roadmap.md
│   ├── backlog.md
│   ├── sprint-plan.md
│   ├── current-state-spec.md
│   └── documentation-consolidation-spec.md
│
└── development/
    ├── testing-strategy.md
    └── development-guide.md
```

`current-state.md` será criado somente quando começar a implementação.

---

## 21. Revisão de consistência

Antes do commit documental consolidado, os documentos deverão ser revisados em conjunto.

Objetivos:

- localizar inconsistências;
- localizar terminologia antiga;
- localizar referências quebradas;
- localizar caminhos incorretos;
- localizar duplicações conflitantes;
- alinhar decisões antigas às posteriores aprovadas.

---

## 22. Regra de precedência

Quando uma decisão antiga tiver sido explicitamente substituída posteriormente:

> **A decisão mais recente e aprovada prevalece.**

Quando existir conflito genuinamente ambíguo, Codex não deverá escolher arbitrariamente.

O conflito deverá ser reportado.

---

## 23. Normalização — Backup

Formulações antigas não deverão afirmar que backup automático é obrigatório.

Regra final:

```text
Capacidade de backup
→ obrigatória no produto

Backup manual
→ disponível

Backup automático
→ disponível

Uso do backup automático
→ opcional
```

`non-functional-requirements.md` deverá refletir essa decisão.

---

## 24. Normalização — Máquina hospedeira

Não utilizar como requisito:

```text
Windows Server obrigatório
```

Regra:

```text
Máquina hospedeira Windows compatível

Pode ser:
- desktop
- notebook
- Windows Server
```

---

## 25. Normalização — Windows

Evitar afirmar que somente Windows 11/Windows Server podem hospedar a aplicação.

A documentação deverá distinguir:

- compatibilidade da plataforma .NET;
- validação interna do Reset Service;
- máquina hospedeira;
- máquina cliente;
- compatibilidade legada.

---

## 26. Normalização — Clientes antigos

Windows 7, 8 e 8.1 não deverão aparecer como plataformas oficialmente suportadas.

Utilizar:

> compatibilidade legada / melhor esforço.

---

## 27. Normalização — Multiusuário

A decisão final será sempre representada por:

```text
Command Queue
+
Optimistic Concurrency
+
SignalR
```

SignalR não deverá ser descrito isoladamente como solução para concorrência.

---

## 28. Responsabilidades da estratégia multiusuário

```text
Command Queue
→ ordenação das gravações

Version / Optimistic Concurrency
→ proteção contra estado obsoleto

SignalR
→ propagação das alterações confirmadas
```

---

## 29. Normalização — Confirmação das gravações

Regra:

```text
Entrou na fila
≠ confirmado
```

Somente:

```text
COMMIT concluído
= operação confirmada
```

Depois:

```text
COMMIT
↓
SignalR
```

---

## 30. Normalização — Migrations

A documentação não deverá recomendar migrations indiscriminadas no startup da produção.

A decisão correta será:

```text
Release
↓
processo de atualização
↓
migration explícita
↓
validação
↓
nova versão
```

EF Core Migration Bundle será a direção preferencial quando aplicável.

---

## 31. Normalização — Primeiro Administrador

O bootstrap deverá permanecer:

```text
acesso local na máquina hospedeira
↓
criação do primeiro Administrator
↓
bootstrap desativado
```

Não haverá cadastro público inicial disponível normalmente através da LAN.

---

## 32. Normalização — Terminologia de usuários

Termos técnicos:

```text
Administrator
Technician
```

Não alternar entre diferentes nomes técnicos para os mesmos roles.

A interface poderá posteriormente apresentar traduções adequadas ao usuário.

---

## 33. Normalização — Estados dos serviços

Termos técnicos oficiais:

```text
Draft
In Progress
Waiting
Completed
Cancelled
```

---

## 34. Normalização — Estados dos passos

Termos técnicos oficiais:

```text
Pending
Completed
Not Applicable
```

Não introduzir estados adicionais durante a revisão documental.

---

## 35. Terminologia de produto

Na documentação voltada ao conceito do produto, utilizar preferencialmente:

```text
Roteiro
Etapa
Passo
```

No código futuro poderão existir nomes técnicos como:

```text
ServiceRoute
ServiceStage
ServiceStep
```

quando apropriado.

---

## 36. Referências entre documentos

A revisão deverá verificar se referências apontam para nomes e caminhos reais.

Não deixar referências obsoletas para arquivos renomeados ou inexistentes.

---

## 37. Markdown

Os documentos deverão ter Markdown simples e consistente.

Verificar:

- um título principal por arquivo;
- hierarquia de headings;
- listas;
- tabelas;
- code fences;
- links;
- caminhos;
- espaçamento;
- ausência de marcação quebrada.

---

## 38. IDs dos writing blocks

Atributos utilizados durante a conversa, como `id="..."`, não fazem parte do conteúdo Markdown dos documentos finais.

Se tiverem sido copiados para os arquivos, deverão ser removidos.

---

## 39. Limites da consolidação

A revisão documental não autoriza Codex a:

- alterar requisitos aprovados;
- redesenhar arquitetura;
- trocar tecnologias;
- modificar permissões;
- alterar workflow;
- eliminar funcionalidades;
- criar funcionalidades;
- reestruturar backlog por preferência;
- alterar sprints arbitrariamente.

O trabalho será documental e editorial.

---

## 40. README como índice

O README deverá destacar links para os documentos principais:

```text
Destino do produto
→ docs/product/product-destination.md

Arquitetura
→ docs/architecture/architecture.md

Roadmap
→ docs/planning/roadmap.md

Backlog
→ docs/planning/backlog.md

Sprints
→ docs/planning/sprint-plan.md

Qualidade
→ docs/development/testing-strategy.md

Processo de desenvolvimento
→ docs/development/development-guide.md
```

---

## 41. Comandos de desenvolvimento

Antes de a solution existir, o README não deverá apresentar comandos como se fossem operacionais.

Por exemplo, não afirmar prematuramente que:

```text
dotnet run
```

já inicia o produto.

As instruções reais de build e execução deverão ser adicionadas depois da Sprint 01 com comandos efetivamente testados.

---

## 42. Licença

A consolidação documental não deverá adicionar automaticamente licença open source.

Licenciamento será decisão separada quando necessário.

---

## 43. Badges

Não adicionar badges decorativos antes que existam mecanismos reais correspondentes.

Exemplos não necessários agora:

- build;
- coverage;
- NuGet;
- licença;
- release.

---

## 44. CI/CD

A consolidação documental não deverá criar:

- GitHub Actions;
- pipelines;
- workflows;
- publicação automática.

CI/CD será tratado quando existir código compilável e necessidade concreta.

---

## 45. Estado final esperado do README

Antes do início da implementação, o README deverá deixar claro:

```text
Planejamento da v1.0:
Concluído

Implementação:
Não iniciada

Próxima etapa:
Sprint 01 — Estrutura da solução
```

---

## 46. Tarefa única de consolidação

Depois de todos os documentos estarem salvos localmente, será preparado um único prompt para Codex.

Objetivo:

```text
Consolidar toda a documentação
antes da implementação.
```

---

## 47. Escopo esperado da tarefa Codex

Codex deverá:

1. verificar branch e working tree;
2. inventariar os documentos locais;
3. comparar o inventário real com este documento;
4. preservar `product-spec.md` já existente;
5. criar ou revisar `README.md`;
6. organizar arquivos nos diretórios aprovados;
7. revisar Markdown;
8. corrigir referências e caminhos;
9. remover resíduos de writing blocks;
10. normalizar inconsistências conhecidas;
11. detectar conflitos não resolvíveis automaticamente;
12. não escrever código;
13. revisar o diff;
14. realizar verificações documentais razoáveis;
15. criar commit documental coerente;
16. fazer push para `main`;
17. terminar com working tree limpa;
18. apresentar relatório verificável.

---

## 48. Fora de escopo da tarefa Codex

Não criar:

- `.sln`;
- projetos .NET;
- `src/`;
- `tests/`;
- packages;
- código C#;
- migrations;
- banco SQLite;
- CI;
- instalador;
- infraestrutura vazia.

---

## 49. Tratamento de conflitos

Se Codex encontrar conflito documental que não possa ser resolvido pelas normalizações explícitas deste documento:

```text
não escolher silenciosamente
```

Deverá:

- identificar arquivos;
- citar os trechos conflitantes;
- explicar o conflito;
- não alterar essa decisão;
- reportar para revisão.

---

## 50. Commit esperado

A mensagem poderá seguir padrão semelhante a:

```text
docs: consolidate project planning
```

ou outra mensagem documental equivalente e coerente.

Não será necessário criar commits separados para cada documento.

---

## 51. Estado final esperado da tarefa

Depois da consolidação:

```text
Branch:
main

Documentação:
organizada

README:
presente

Planning docs:
versionados

Push:
concluído

Working tree:
Clean
```

---

## 52. Verificação posterior

Depois do relatório do Codex:

```text
Codex
↓
commit/push
↓
ChatGPT verifica GitHub
↓
estrutura
↓
conteúdo
↓
normalizações
↓
aprovação
```

O relatório isolado não será suficiente para fechar a etapa.

---

## 53. Transição para implementação

Depois que a consolidação documental for aprovada no GitHub:

```text
Criar current-state.md
↓
Sprint 01
↓
BL-001
↓
BL-001/T01
↓
primeiro prompt de código
```

---

## 54. Estado da decisão

**PLANNING-023 — README Inicial e Revisão do Inventário Documental: CONCLUÍDA E APROVADA.**

O planejamento documental necessário para preparar o repositório antes da implementação está concluído.
