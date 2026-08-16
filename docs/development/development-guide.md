# Reset Service — Development Guide

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Guia de Desenvolvimento e Regras de Trabalho com Codex  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`, `docs/planning/roadmap.md`, `docs/planning/backlog.md`, `docs/planning/sprint-plan.md`, `docs/planning/current-state-spec.md`, `docs/development/testing-strategy.md`

---

## 1. Objetivo

Este documento define como o planejamento aprovado do Reset Service será transformado em código.

O processo deverá favorecer:

- escopo controlado;
- mudanças pequenas;
- testes junto da implementação;
- revisão simples;
- histórico Git compreensível;
- segurança;
- consistência arquitetural;
- recuperação rápida do contexto.

O princípio central será:

```text
Uma tarefa pequena
      ↓
Um objetivo claro
      ↓
Implementação
      ↓
Testes
      ↓
Commit
      ↓
Push
      ↓
Verificação
      ↓
Próxima tarefa
```

---

## 2. Responsabilidades

A divisão de responsabilidades será:

| Papel | Responsabilidade |
|---|---|
| ChatGPT | Planejamento, arquitetura, decomposição e revisão |
| Codex | Implementação técnica |
| Usuário | Aprovação das decisões e condução do fluxo |
| GitHub | Fonte de verdade do código e commits |
| `current-state.md` futuro | Estado operacional da execução após início da implementação |

Codex poderá realizar escolhas locais de implementação, mas não redefinir requisitos ou decisões arquiteturais aprovadas por iniciativa própria.

---

## 3. Hierarquia da execução

A implementação seguirá:

```text
Roadmap
   ↓
Backlog
   ↓
Sprint
   ↓
Tarefa técnica
   ↓
Prompt focado
   ↓
Codex
```

A tarefa técnica será normalmente a unidade utilizada em cada ciclo de implementação.

---

## 4. Identificação das tarefas

As tarefas técnicas serão identificadas dentro do backlog item.

Formato:

```text
BL-XXX/TYY
```

Exemplo:

```text
BL-025/T01 — Criar ServiceNumberSequence
BL-025/T02 — Configurar persistência
BL-025/T03 — Implementar geração transacional
BL-025/T04 — Testar concorrência
```

---

## 5. Uma tarefa principal por vez

A direção inicial será trabalhar com apenas uma tarefa técnica principal ativa.

Fluxo:

```text
T01
↓
Implementação
↓
Validação
↓
Aprovação
↓
T02
```

Não será padrão manter várias implementações dependentes simultaneamente.

---

## 6. Tamanho das tarefas

Uma tarefa deve ser pequena o suficiente para:

- possuir um objetivo principal;
- gerar diff compreensível;
- permitir revisão;
- possuir critérios objetivos;
- possuir testes proporcionais;
- evitar múltiplas novas decisões arquiteturais.

---

## 7. Quando subdividir

Uma tarefa deverá ser dividida se estiver acumulando simultaneamente responsabilidades como:

```text
modelo
+
migration
+
regra complexa
+
backend HTTP
+
frontend
+
SignalR
+
E2E
```

Um backlog item não precisa resultar em um único commit.

---

## 8. Estrutura dos prompts

Prompts de implementação deverão normalmente conter:

```text
Contexto
Objetivo
Documentos relevantes
Escopo obrigatório
Fora de escopo
Critérios de aceite
Testes
Git
Relatório final
```

A estrutura deverá ser adaptada à tarefa, sem adicionar seções desnecessárias.

---

## 9. Contexto mínimo

O prompt deverá fornecer somente o contexto necessário.

Exemplo:

```text
Sprint 02.
BL-003 — Persistência EF Core + SQLite.
A solution e os projetos-base já existem.
```

Não será necessário repetir integralmente todas as especificações do produto.

---

## 10. Consulta aos documentos

Quando necessário, Codex deverá consultar os documentos existentes no próprio repositório.

Exemplo:

```text
Leia antes de implementar:

docs/architecture/architecture.md
docs/architecture/data-model.md
docs/development/testing-strategy.md
```

Os documentos aprovados prevalecem sobre preferências pessoais de implementação.

---

## 11. Objetivo explícito

Todo prompt deverá possuir objetivo inequívoco.

Exemplo:

```text
Objetivo:
configurar o DbContext principal e o provider
SQLite sem criar entidades de negócio.
```

---

## 12. Escopo obrigatório

O prompt deverá dizer exatamente o que precisa ser implementado.

Exemplo:

```text
Implemente:

- EF Core SQLite;
- ResetServiceDbContext;
- configuração da conexão;
- registro no DI;
- teste de inicialização.
```

---

## 13. Fora de escopo

Sempre que houver risco de implementação antecipada, o prompt deverá explicitar limites.

Exemplo:

```text
Não implemente:

- Identity;
- Service;
- modelos;
- SignalR funcional;
- backup;
- UI de negócio.
```

---

## 14. Regra de ouro do escopo

A regra será:

> Fazer tudo que é necessário para concluir a tarefa e nada que pertença conscientemente à próxima tarefa.

Essa regra prevalece durante todo o desenvolvimento.

---

## 15. Escolhas locais

Codex poderá decidir autonomamente escolhas pequenas, técnicas, reversíveis e sem impacto arquitetural.

Exemplos:

- nome de método privado;
- organização interna de fixture;
- helper pequeno;
- estrutura local de teste;
- refatoração mínima necessária.

---

## 16. Decisões que não podem ser silenciosas

Codex não deverá decidir sozinho mudanças que alterem:

- arquitetura;
- modelo de dados relevante;
- comportamento de produto;
- segurança;
- autenticação;
- persistência principal;
- dependências estruturais;
- protocolo;
- UX importante;
- estratégia de deployment.

Essas mudanças exigem retorno ao planejamento.

---

## 17. Tecnologias aprovadas

Decisões estruturais existentes deverão ser preservadas.

Exemplos:

```text
Razor Pages
SQLite
EF Core
SignalR
System.Threading.Channels
ASP.NET Core Identity
PDFsharp/MigraDoc
Executável Windows self-contained sob demanda
```

Nenhuma delas deverá ser substituída implicitamente.

---

## 18. Evitar overengineering

Codex deverá preferir a solução mais simples que satisfaça os requisitos.

Não adicionar automaticamente:

```text
MediatR
AutoMapper
CQRS framework
Repository genérico
Unit of Work customizado
Redis
RabbitMQ
Event bus externo
abstrações antecipadas
```

Esses componentes somente serão introduzidos se surgir necessidade concreta e aprovada.

---

## 19. Novas dependências

Uma nova biblioteca de produção deverá ter justificativa clara.

Antes de adicioná-la deverá ser considerado se:

```text
.NET
+
ASP.NET Core
+
bibliotecas já aprovadas
```

resolvem adequadamente o problema.

Dependências não deverão ser adicionadas apenas para economizar poucas linhas.

---

## 20. Banco e migrations

Mudanças persistentes deverão seguir o fluxo de EF Core aprovado.

Quando aplicável:

```text
modelo
↓
configuração
↓
migration
↓
teste
↓
validação
```

Não alterar manualmente o schema de produção fora do processo controlado.

---

## 21. Migrations em produção

A aplicação não deverá executar migrations indiscriminadamente a cada startup em produção.

Migrations de release serão controladas pelo processo de atualização.

---

## 22. Segurança dentro das features

Segurança não será tratada como etapa posterior.

Uma operação mutável deverá considerar conforme aplicável:

```text
Authentication
↓
Antiforgery
↓
Authorization
↓
Input validation
↓
Command Queue
↓
Domain validation
↓
Transaction
↓
COMMIT
↓
SignalR
```

---

## 23. Frontend não é autoridade

Dados enviados pelo navegador serão considerados não confiáveis.

O backend deverá determinar por conta própria:

- usuário autenticado;
- ator;
- perfil;
- permissões;
- estado atual;
- versão atual quando necessário;
- validade da transição.

---

## 24. Autoria

Não confiar em campos como:

```text
ActorUserId
ActorName
Role
```

enviados pelo cliente como autoridade.

O ator real será obtido da sessão autenticada.

---

## 25. Testes fazem parte da implementação

Testes correspondentes deverão acompanhar a feature sempre que possível.

Evitar como padrão:

```text
commit 1 — funcionalidade
commit 2 — testes futuramente
```

quando ambos pertencem naturalmente à mesma tarefa.

---

## 26. Tipos de testes esperados

| Tipo de alteração | Teste esperado |
|---|---|
| Regra de domínio | Unitário |
| EF Core/schema | Integração SQLite |
| HTTP/autorização | Integração ASP.NET Core |
| Concorrência | Integração concorrente |
| SignalR | Integração/multiusuário |
| Fluxo crítico de UI | E2E quando aplicável |
| PDF | Conteúdo e revisão adequada |
| Backup/restore | Ciclo completo |

---

## 27. Verificação base

Quando aplicável, Codex deverá executar:

```text
dotnet build
dotnet test
```

além dos testes específicos necessários.

---

## 28. Testes não executados

Se algum teste necessário não puder ser executado, isso deverá aparecer claramente no relatório final.

Não utilizar frases genéricas como:

```text
parece funcionar
```

no lugar da validação prevista.

---

## 29. Teste falhando

Um teste não deverá ser removido ou enfraquecido automaticamente apenas porque começou a falhar.

Primeiro deverá ser determinado se existe:

- bug;
- teste incorreto;
- teste obsoleto;
- requisito alterado;
- problema de ambiente.

Alterar teste que representa requisito aprovado exige justificativa.

---

## 30. Warnings

Código novo não deverá introduzir warnings evitáveis no build principal.

Warnings externos inevitáveis poderão ser avaliados caso a caso.

---

## 31. Estilo de código

A implementação deverá seguir:

- C# idiomático;
- `.editorconfig`;
- nullable reference types;
- async quando apropriado;
- cancellation quando relevante;
- nomes claros;
- métodos focados;
- abstrações proporcionais;
- código legível.

---

## 32. Comentários

Comentários devem principalmente explicar:

> por que algo existe

quando isso não for evidente pelo código.

Evitar comentários que apenas traduzem literalmente a instrução executada.

---

## 33. Alterações não relacionadas

Codex não deverá modificar código não relacionado apenas por considerar possível melhorar sua aparência.

Regra:

```text
não necessário para a tarefa
→ não alterar
```

salvo correção mínima indispensável.

---

## 34. Bugs encontrados durante a implementação

Se um bug:

- bloqueia a tarefa;
- é pequeno;
- possui correção segura e local;

Codex poderá corrigi-lo e relatar.

Se for independente, grande ou exigir nova decisão, deverá ser reportado separadamente.

---

## 35. Verificação Git antes de alterar

Antes de implementar:

```text
git branch --show-current
git status
```

ou equivalente.

Codex deverá conhecer:

- branch;
- alterações existentes;
- estado da working tree.

---

## 36. Alterações inesperadas existentes

Se houver arquivos modificados não relacionados à tarefa, Codex não deverá:

- descartá-los;
- sobrescrevê-los;
- resetá-los;
- incorporá-los silenciosamente.

O problema deverá ser relatado.

---

## 37. Working tree

O objetivo de cada ciclo concluído será:

```text
Working tree: Clean
```

Não deverão permanecer artefatos esquecidos.

---

## 38. Arquivos não versionáveis

Evitar commits acidentais de:

```text
bin/
obj/
logs/
SQLite local operacional
bancos temporários
backups
segredos
certificados privados
arquivos temporários
```

`.gitignore` deverá ser mantido adequadamente.

---

## 39. Commits

Cada commit deverá representar mudança coerente.

Mensagens preferenciais:

```text
feat(services): add service number sequence
fix(auth): reject inactive users
test(services): cover concurrent number generation
docs(planning): update development state
chore(build): configure release settings
```

---

## 40. Mensagens inadequadas

Evitar:

```text
update stuff
changes
fix
final
final2
misc
```

---

## 41. Quantidade de commits

Não será obrigatório um commit exato por tarefa.

Uma tarefa pequena normalmente poderá produzir um commit.

Uma tarefa maior poderá produzir mais de um commit coerente.

Evitar:

- dezenas de microcommits sem propósito;
- commit enorme contendo várias features.

---

## 42. Histórico publicado

Depois de enviado ao `main`, o histórico não deverá ser reescrito apenas por estética.

Correções posteriores deverão preferencialmente gerar novo commit.

---

## 43. Push

Depois da implementação aprovada localmente pelo próprio conjunto de testes previsto:

```text
commit
↓
push
```

O relatório deverá indicar destino.

Exemplo:

```text
Branch: main
Push: origin/main
```

---

## 44. Relatório final do Codex

Formato esperado:

```text
Tarefa:
BL-XXX/TYY — Nome

Implementado:
- ...
- ...

Testes:
- dotnet build: PASS
- dotnet test: PASS (...)

Arquivos principais:
- ...
- ...

Commit:
abc1234 — feat(...): ...

Push:
origin/main

Working tree:
Clean

Observações:
- Nenhuma.
```

---

## 45. Relatório em caso de bloqueio

Se Codex não conseguir concluir:

```text
Status:
Bloqueado

Concluído:
- ...

Problema:
- ...

Testes:
- ...

Git:
- ...

Working tree:
- ...
```

A falha deverá ser apresentada de forma explícita.

---

## 46. Implementação quebrada

Por padrão, Codex não deverá criar um commit final apresentando uma tarefa incompleta como concluída.

Se não conseguir cumprir os critérios:

```text
corrigir
ou
reportar bloqueio
```

---

## 47. Escopo incompatível com o repositório

Se o prompt pressupuser algo inexistente, Codex deverá informar:

```text
Esperado:
...

Encontrado:
...

Dependência faltante:
...
```

Não deverá improvisar uma grande arquitetura adicional para contornar a inconsistência.

---

## 48. Relatório não significa aprovação

Mesmo depois de:

```text
Tests: PASS
Push: OK
```

o fluxo continua:

```text
Relatório Codex
      ↓
Verificação GitHub
      ↓
Aprovação ou correção
```

---

## 49. Resultados da revisão

Os resultados principais serão:

```text
APROVADO
CORREÇÃO NECESSÁRIA
BLOQUEADO
```

---

## 50. Aprovado

Quando aprovado:

- commit passa a ser o último estado aprovado;
- tarefa pode ser marcada como concluída;
- Current State é atualizado;
- próxima tarefa pode começar.

---

## 51. Correção necessária

Não avançar para a tarefa seguinte.

Criar um prompt pequeno de correção.

Exemplo:

```text
O commit abc1234 está correto exceto pelo
tratamento de concorrência em X.

Corrija somente esse comportamento,
adicione o teste correspondente,
execute a suíte aplicável,
commit e push.
```

---

## 52. Bloqueado

Quando houver impedimento real:

- registrar bloqueio;
- indicar impacto;
- não mascarar como conclusão;
- retornar ao planejamento ou ambiente quando necessário.

---

## 53. Current State

Durante a implementação:

```text
docs/planning/current-state.md
```

Esse arquivo operacional ainda não existe nesta consolidação documental e deverá ser criado somente quando a implementação começar.

deverá acompanhar o estado operacional.

Codex poderá atualizá-lo quando o prompt indicar.

---

## 54. Aprovação no Current State

Codex não deverá marcar unilateralmente:

```text
Aprovado
```

ou:

```text
Sprint concluída
```

antes da verificação externa.

Durante entrega, o estado apropriado será normalmente:

```text
Em validação
```

---

## 55. Documentação

Documentação deverá ser atualizada quando a implementação alterar algo que:

- desenvolvedor;
- operador;
- administrador;
- usuário;

precise conhecer.

Não atualizar documentos apenas para criar atividade.

---

## 56. Divergência entre código e documentação

Não deverá existir divergência conhecida mantida conscientemente entre implementação e documentação aprovada.

Quando surgir mudança legítima, primeiro deverá ser decidido se:

```text
código está errado
ou
documentação precisa mudar
```

---

## 57. ADRs

Não será criado um ADR para toda decisão pequena.

Decisões arquiteturais relevantes poderão atualizar diretamente os documentos existentes.

Documento novo somente será criado se houver necessidade concreta.

---

## 58. Revisão de uma entrega

A revisão deverá concentrar-se no escopo da tarefa:

- diff;
- requisitos;
- arquitetura;
- segurança;
- persistência;
- testes;
- commit;
- working tree.

Não será necessário revisar integralmente todo o projeto a cada tarefa.

---

## 59. Trabalho paralelo

A direção inicial será sequencial.

```text
T01
↓
Aprovação
↓
T02
↓
Aprovação
```

Paralelismo poderá ser adotado futuramente somente para trabalhos realmente independentes.

---

## 60. Template de prompt de implementação

Modelo de referência:

```text
RESET SERVICE — IMPLEMENTAÇÃO

Tarefa:
BL-XXX/TYY — <nome>

Contexto:
<estado mínimo necessário>

Leia:
<documentos relevantes>

Objetivo:
<objetivo principal>

Implemente:
<escopo obrigatório>

Não implemente:
<limites>

Critérios de aceite:
<condições objetivas>

Testes:
<validações necessárias>

Git:
- verifique branch e status antes de iniciar;
- não descarte alterações existentes;
- implemente somente o escopo;
- execute os testes aplicáveis;
- faça commit coerente;
- faça push;
- termine com working tree limpa.

Relatório final:
- resumo;
- testes;
- arquivos principais;
- commit;
- push;
- working tree;
- observações ou bloqueios.
```

O template deverá ser reduzido quando uma tarefa simples não exigir todas essas informações.

---

## 61. Fluxo completo

```text
Selecionar Sprint
      ↓
Selecionar Backlog Item
      ↓
Decompor próxima tarefa
      ↓
Atualizar Current State
      ↓
Criar prompt focado
      ↓
Codex verifica Git
      ↓
Codex implementa
      ↓
Codex testa
      ↓
Codex commit/push
      ↓
Codex relata
      ↓
Verificar GitHub
      ↓
      ├── Aprovado
      │      ↓
      │   Próxima tarefa
      │
      ├── Correção necessária
      │      ↓
      │   Prompt de correção
      │
      └── Bloqueado
             ↓
          Resolver impedimento
```

---

## 62. Princípios fundamentais

1. Uma tarefa técnica principal por vez.
2. Objetivo explícito.
3. Escopo pequeno.
4. Fora de escopo declarado quando necessário.
5. Documentos aprovados orientam a implementação.
6. Codex não redefine arquitetura silenciosamente.
7. Soluções simples são preferidas.
8. Dependências novas exigem justificativa.
9. Segurança acompanha cada feature.
10. Frontend não é autoridade.
11. Testes acompanham a implementação.
12. Não remover testes apenas porque falharam.
13. Não alterar código não relacionado.
14. Verificar Git antes de começar.
15. Não descartar alterações inesperadas.
16. Working tree limpa ao final de ciclos concluídos.
17. Commits coerentes.
18. Push faz parte da entrega.
19. Relatório do Codex não substitui verificação.
20. Correções são resolvidas antes da próxima tarefa.
21. GitHub é a fonte de verdade da implementação.
22. Current State representa o presente operacional.

---

## 63. Estado da decisão

**PLANNING-021 — Guia de Desenvolvimento e Regras de Trabalho com Codex: CONCLUÍDA E APROVADA.**

Este documento passa a definir o processo oficial de transformação das sprints e backlog items em implementação verificável.
