# Reset Service — Current State and Execution Control

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Current State e Controle de Execução  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/planning/roadmap.md`, `docs/planning/backlog.md`, `docs/planning/sprint-plan.md`, `docs/development/testing-strategy.md`

---

## 1. Objetivo

Este documento define como o estado atual do desenvolvimento do Reset Service será acompanhado durante a implementação.

O arquivo operacional correspondente será:

```text
docs/planning/current-state.md
```

Ele deverá responder rapidamente:

```text
Onde estamos?
O que está sendo feito?
Qual foi o último estado aprovado?
Existe algum bloqueio?
Qual é o próximo passo?
```

---

## 2. Momento de criação

`current-state.md` não deverá ser criado vazio durante o planejamento.

Ele será criado quando a implementação efetivamente começar.

Nesse momento já existirão:

- sprint ativa;
- backlog item ativo;
- primeira tarefa técnica;
- branch;
- último commit aprovado;
- próximo passo concreto.

---

## 3. Papel do Current State

Os documentos possuem responsabilidades diferentes.

```text
product/*
architecture/*
→ definem produto e arquitetura

roadmap.md
→ define grandes fases

backlog.md
→ define capacidades rastreáveis

sprint-plan.md
→ define ciclos de execução

current-state.md
→ descreve exatamente o presente
```

O Current State não deverá duplicar os demais documentos.

---

## 4. Fonte de verdade

Durante a implementação:

```text
GitHub
→ fonte de verdade do código e commits

current-state.md
→ fonte de verdade do estado operacional
   do planejamento da execução
```

Caso haja divergência entre o que o documento afirma estar implementado e o que realmente está no repositório, o GitHub prevalece.

---

## 5. Característica do documento

O Current State deverá ser:

- curto;
- objetivo;
- atual;
- operacional;
- fácil de ler;
- descartável em partes;
- atualizado conforme o trabalho avança.

Não será um arquivo histórico acumulativo.

---

## 6. Estrutura padrão

Estrutura recomendada:

```markdown
## Reset Service — Current State

## Estado geral
- Versão alvo:
- Fase:
- Sprint:
- Status da sprint:

## Trabalho atual
- Backlog item:
- Tarefa:
- Status:
- Responsável técnico:

## Último estado aprovado
- Último commit aprovado:
- Mensagem:
- Branch:
- Working tree:
- Última verificação:

## Concluído nesta sprint
- ...

## Bloqueios
- Nenhum.

## Próximo passo
- ...

## Observações imediatas
- ...
```

---

## 7. Estado geral

Exemplo:

```text
Versão alvo:
v1.0

Fase:
5 — Criação de serviços

Sprint:
10 — Identidade e criação do serviço

Status da sprint:
Em andamento
```

Estados permitidos para sprint:

```text
Não iniciada
Em andamento
Em validação
Bloqueada
Concluída
```

Não serão criados estados adicionais sem necessidade concreta.

---

## 8. Uma tarefa principal ativa

A execução deverá manter uma tarefa principal ativa por vez.

Exemplo:

```text
Backlog:
BL-025 — Sequência RS-AAAA-NNNNN

Tarefa:
BL-025/T03 — Implementar geração transacional

Status:
Em implementação
```

Esse princípio reforça o desenvolvimento focado e reduz mudanças simultâneas difíceis de revisar.

---

## 9. Identificação das tarefas técnicas

As subtarefas serão identificadas dentro do backlog item.

Formato:

```text
BL-025/T01
BL-025/T02
BL-025/T03
```

Exemplo:

```text
BL-025/T01 — Criar ServiceNumberSequence
BL-025/T02 — Mapear persistência
BL-025/T03 — Implementar geração transacional
BL-025/T04 — Testar concorrência
```

Não será necessário criar uma segunda numeração global para tarefas técnicas.

---

## 10. Backlog parcialmente concluído

O Current State poderá apresentar:

```text
BL-025 — Em andamento

Concluído:
- T01
- T02

Atual:
- T03

Restante:
- T04
```

Isso fornece granularidade sem transformar `backlog.md` em um rastreador de tarefas.

---

## 11. Status da tarefa

Estados simples poderão ser utilizados:

```text
Pronta para implementação
Em implementação
Em validação
Bloqueada
Concluída
```

Uma tarefa somente será marcada como concluída após o estado realmente aprovado.

---

## 12. Responsável técnico

O campo identifica quem possui a ação imediata.

Exemplos:

```text
Responsável técnico: Codex
```

ou:

```text
Responsável técnico: Revisão ChatGPT
```

Esse campo não representa propriedade permanente da funcionalidade.

---

## 13. Último commit aprovado

O Current State deverá registrar o último commit considerado validado.

Exemplo:

```text
Último commit aprovado:
a41c38f

Mensagem:
feat(services): add service number sequence

Branch:
main
```

---

## 14. Significado de "aprovado"

Um commit será considerado aprovado somente após:

```text
Codex implementa
      ↓
testes executados
      ↓
commit
      ↓
push
      ↓
GitHub verificado
      ↓
resultado aceito
```

A afirmação do Codex de que o commit foi criado não é suficiente isoladamente.

---

## 15. Hash do commit

Para leitura rápida, o Current State poderá registrar hash curto.

Exemplo:

```text
a41c38f
```

O histórico completo permanece no Git.

---

## 16. Branch

A branch ativa deverá sempre ser explicitada.

Exemplo:

```text
Branch:
main
```

Caso branches curtas sejam adotadas futuramente, o Current State deverá refletir a branch real em uso.

---

## 17. Working tree

Estados recomendados:

```text
Working tree:
Clean
```

ou:

```text
Working tree:
Dirty — alterações da tarefa atual
```

Entre tarefas aprovadas, o estado esperado será:

```text
Clean
```

---

## 18. Última verificação

Registrar a data em que o estado foi efetivamente confirmado.

Exemplo:

```text
Última verificação:
2026-08-14
```

Horário não será obrigatório, salvo quando necessário para diagnóstico.

---

## 19. Concluído nesta sprint

A seção deverá mostrar somente o necessário para entender o progresso corrente.

Exemplo:

```text
## Concluído nesta sprint

- BL-025/T01 — entidade de sequência
- BL-025/T02 — configuração EF Core
```

Não deverá preservar indefinidamente toda a lista de tarefas de sprints antigas.

---

## 20. Limpeza ao mudar de sprint

Quando uma sprint terminar, informações detalhadas já registradas no Git poderão ser resumidas.

Exemplo:

```text
Sprint 02 concluída.
Próxima: Sprint 03 — Pipeline de comandos e infraestrutura.
```

O novo trabalho passa a ocupar o foco do documento.

---

## 21. Bloqueios

Quando não houver bloqueio:

```text
## Bloqueios

Nenhum.
```

Quando houver:

```text
## Bloqueios

- Teste de implantação no Windows 10 ainda indisponível.
  Impacto: BL-076 não pode ser concluído.
```

Cada bloqueio deverá indicar:

```text
problema
+
impacto
```

---

## 22. Decisões pendentes

Uma decisão ainda não tomada não será automaticamente classificada como bloqueio.

Somente será um bloqueio se impedir o próximo trabalho necessário.

Caso contrário, poderá aparecer em:

```text
Observações imediatas
```

ou permanecer no documento de planejamento apropriado.

---

## 23. Próximo passo

O Current State deverá possuir exatamente um próximo passo principal e concreto.

Bom:

```text
Criar e executar o prompt do BL-025/T03
para implementar a geração transacional
do número de serviço.
```

Evitar:

```text
Continuar desenvolvimento.
```

ou:

```text
Implementar o restante.
```

---

## 24. Observações imediatas

A seção será destinada somente a informações úteis para a execução próxima.

Exemplo:

```text
- BL-025/T04 deverá testar pelo menos
  10 criações concorrentes.

- Não iniciar BL-026 antes da aprovação
  completa de BL-025.
```

Observações que deixam de ser relevantes deverão ser removidas.

---

## 25. O que não pertence ao Current State

Não incluir:

- especificações completas;
- arquitetura detalhada;
- cópia do backlog;
- cópia do sprint plan;
- histórico completo de commits;
- changelog;
- relatórios extensos de testes;
- bugs já resolvidos;
- atas de conversa;
- decisões antigas já consolidadas.

---

## 26. Não será um changelog

Não utilizar formato de diário:

```text
08/14 — fizemos X
08/15 — corrigimos Y
08/16 — mudamos Z
```

Esse histórico pertence ao Git e ao GitHub.

O Current State descreve o estado presente.

---

## 27. Atualizações obrigatórias

O documento deverá ser atualizado quando ocorrer mudança relevante, especialmente:

1. início de sprint;
2. início de backlog item;
3. mudança da tarefa ativa;
4. entrada em validação;
5. aprovação de tarefa;
6. aprovação de commit;
7. surgimento de bloqueio;
8. resolução de bloqueio;
9. conclusão de backlog item;
10. conclusão de sprint;
11. mudança do próximo passo.

Não será necessário atualizar após cada alteração interna de código.

---

## 28. Atualização pelo Codex

Codex poderá atualizar o Current State quando a tarefa explicitamente pedir isso.

Entretanto, Codex não deverá declarar unilateralmente:

```text
Sprint concluída
```

ou:

```text
Implementação aprovada
```

antes da etapa de verificação prevista no fluxo.

---

## 29. Estados antes e depois da validação

Enquanto Codex trabalha:

```text
Status:
Em implementação
```

Após entregar relatório:

```text
Status:
Em validação
```

Depois de aprovado:

```text
Tarefa anterior:
Concluída

Próxima tarefa:
Pronta para implementação
```

---

## 30. Atualização e commits

Não será necessário criar commits independentes apenas para cada pequena atualização do Current State.

Quando apropriado, sua atualização poderá acompanhar a mudança de implementação relacionada.

A prioridade será manter o documento suficientemente atualizado sem gerar ruído desnecessário no histórico Git.

---

## 31. GitHub Issues

Na execução inicial da v1.0, não será obrigatório criar GitHub Issue para cada tarefa técnica.

A estrutura inicial será:

```text
backlog.md
+
sprint-plan.md
+
current-state.md
+
commits
+
GitHub
```

Issues poderão ser usadas futuramente quando trouxerem valor concreto, especialmente para:

- bugs;
- investigações;
- trabalhos adiados;
- assuntos que precisem permanecer abertos por mais tempo.

---

## 32. Estratégia de branches

A direção inicial permanece simples:

```text
main
```

Não será adotado Git Flow complexo por padrão.

Branches curtas poderão ser introduzidas posteriormente se trouxerem benefício claro ao fluxo.

O Current State deverá refletir sempre a branch real.

---

## 33. Estado inicial da implementação

Quando começarmos a implementação, um Current State inicial poderá assumir a forma:

```markdown
## Reset Service — Current State

## Estado geral

- Versão alvo: v1.0
- Fase: 1 — Fundação da solução
- Sprint: 01 — Estrutura da solução
- Status da sprint: Em andamento

## Trabalho atual

- Backlog: BL-001 — Estrutura inicial da solução
- Tarefa: BL-001/T01 — Criar solution e projetos iniciais
- Status: Pronta para implementação
- Responsável técnico: Codex

## Último estado aprovado

- Último commit aprovado: [commit de planejamento]
- Branch: main
- Working tree: Clean
- Última verificação: [data]

## Concluído nesta sprint

Nenhuma tarefa ainda.

## Bloqueios

Nenhum.

## Próximo passo

Executar o prompt de BL-001/T01.

## Observações imediatas

- Não implementar autenticação.
- Não criar entidades de negócio.
- Não criar UI funcional nesta tarefa.
```

---

## 34. Tamanho esperado

Regra operacional:

> Se `current-state.md` começar a ficar longo, provavelmente contém informação que pertence a outro documento.

O objetivo será mantê-lo normalmente legível em aproximadamente uma ou duas telas.

---

## 35. Critério de confiabilidade

O documento somente será útil se representar o estado real.

Portanto:

```text
Current State correto
>
Current State detalhado
```

Informação obsoleta deverá ser corrigida ou removida.

---

## 36. Fluxo operacional resumido

```text
Selecionar tarefa
      ↓
Atualizar Current State
      ↓
Codex implementa
      ↓
Status: Em validação
      ↓
Verificar GitHub
      ↓
Aprovar/corrigir
      ↓
Registrar commit aprovado
      ↓
Definir próximo passo
      ↓
Atualizar Current State
```

---

## 37. Decisão final

O `docs/planning/current-state.md` será:

- criado no início da implementação;
- curto;
- operacional;
- orientado ao presente;
- focado em uma tarefa principal;
- vinculado a sprint, backlog e subtarefa;
- vinculado ao último commit aprovado;
- explícito sobre branch e working tree;
- explícito sobre bloqueios;
- explícito sobre o próximo passo;
- atualizado nas mudanças relevantes;
- subordinado ao GitHub quanto ao estado real do código.

---

## 38. Estado da decisão

**PLANNING-020 — Current State e Controle de Execução: CONCLUÍDA E APROVADA.**

A especificação deste mecanismo está concluída. O arquivo operacional `current-state.md` será criado somente quando a implementação começar.