# Reset Service — Service Template Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Modelos e Revisões  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`

---

## 1. Objetivo

Este documento define o comportamento funcional dos Modelos de Serviço e de seu sistema de revisões no Reset Service.

Seu escopo inclui:

- criação de modelos;
- edição;
- publicação;
- revisões;
- duplicação;
- arquivamento;
- reativação;
- exclusão de rascunhos;
- relação entre modelos e serviços.

Não são definidos aqui detalhes técnicos de armazenamento, arquitetura ou implementação.

---

## 2. Modelo de Serviço

Um Modelo de Serviço representa a identidade permanente de um procedimento reutilizável.

Exemplo:

```text
Formatação — Cliente Contrato
```

Um mesmo modelo poderá possuir diferentes revisões ao longo do tempo.

```text
Formatação — Cliente Contrato
│
├── Revisão 1
├── Revisão 2
├── Revisão 3
└── Revisão 4 ← atual
```

O modelo representa **qual procedimento é esse**.

A revisão representa **como esse procedimento estava definido em determinado momento**.

---

## 3. Modelo e Revisão

Modelo e Revisão serão entidades conceitualmente diferentes.

Um modelo:

- possui identidade permanente;
- possui nome;
- possui descrição administrativa;
- possui estado;
- mantém seu histórico de revisões.

Uma revisão:

- contém a estrutura efetiva do roteiro;
- contém etapas;
- contém passos;
- contém instruções;
- possui número sequencial;
- possui data de publicação;
- identifica o usuário que publicou.

---

## 4. Estados do Modelo

Existirão três estados:

- Rascunho;
- Ativo;
- Arquivado.

---

## 5. Rascunho

Um novo modelo começa como **Rascunho**.

Enquanto estiver nesse estado, poderá ser construído e alterado livremente.

Um modelo em Rascunho:

- pode ser editado;
- pode receber etapas;
- pode receber passos;
- pode ser reorganizado;
- pode ser excluído quando nunca publicado nem utilizado;
- não pode originar serviços.

---

## 6. Ativo

Um modelo torna-se **Ativo** após sua primeira publicação.

Um modelo Ativo:

- possui pelo menos uma revisão publicada;
- possui uma revisão atual;
- pode originar novos serviços;
- pode receber novas alterações através de um rascunho separado;
- pode ser arquivado.

---

## 7. Arquivado

Um modelo Arquivado permanece armazenado para fins históricos, mas deixa de estar disponível para criação normal de novos serviços.

Um modelo Arquivado:

- permanece consultável;
- preserva todas as revisões;
- permanece relacionado aos serviços já criados;
- não pode originar novos serviços;
- pode ser reativado.

Arquivamento não representa exclusão.

---

## 8. Criação de Modelo

O fluxo inicial será:

```text
Criar modelo
     ↓
Rascunho
     ↓
Criar etapas e passos
     ↓
Revisar
     ↓
Publicar
     ↓
Revisão 1
     ↓
Ativo
```

---

## 9. Validação para Publicação

Antes da primeira publicação, o modelo deverá possuir uma estrutura válida.

Deverão ser verificados pelo menos:

- nome válido;
- pelo menos uma etapa;
- nenhum título obrigatório vazio;
- nenhuma etapa vazia;
- pelo menos um passo válido.

Modelos incompletos permanecerão como Rascunho.

---

## 10. Publicação

A publicação será uma ação explícita.

```text
[ Publicar modelo ]
```

Conteúdo em edição não será automaticamente considerado oficial.

A publicação representa a decisão de disponibilizar aquele procedimento para uso operacional.

---

## 11. Primeira Revisão

A primeira publicação gera automaticamente:

```text
Revisão 1
```

O modelo passa de:

```text
Rascunho → Ativo
```

O número da revisão não será informado manualmente pelo usuário.

---

## 12. Revisão Atual

Um modelo Ativo possuirá uma revisão publicada considerada atual.

Exemplo:

```text
Modelo:
Formatação — Cliente Contrato

Status:
Ativo

Revisão atual:
4
```

Novos serviços utilizarão automaticamente essa revisão.

---

## 13. Rascunhos não Originam Serviços

Somente revisões publicadas poderão originar serviços.

```text
Rascunho             ✕
Revisão publicada    ✓
```

Alterações ainda não publicadas nunca deverão chegar silenciosamente à operação.

---

## 14. Edição de Modelo Ativo

Uma revisão publicada nunca deverá ser modificada diretamente.

Ao editar um modelo Ativo, o sistema criará conceitualmente um **Rascunho da próxima revisão** baseado na revisão atual.

```text
Revisão 4 publicada
        ↓
Criar cópia de trabalho
        ↓
Alterações não publicadas
```

---

## 15. Modelo com Alterações Pendentes

Enquanto existir um rascunho:

```text
Modelo
│
├── Revisão 4     ← publicada e operacional
│
└── Rascunho      ← alterações em preparação
```

A revisão publicada continua sendo a versão oficial.

Novos serviços continuarão utilizando a Revisão 4.

---

## 16. Nova Publicação

Quando as alterações forem finalizadas:

```text
[ Publicar nova revisão ]
```

o rascunho será transformado na próxima revisão sequencial.

Exemplo:

```text
Revisão atual: 4
       ↓
Publicar alterações
       ↓
Revisão atual: 5
```

A Revisão 4 continua armazenada.

---

## 17. Imutabilidade

Revisões publicadas serão imutáveis.

Depois de publicada uma Revisão 4:

- não poderá ser alterada;
- não poderá ser sobrescrita;
- não poderá ser silenciosamente corrigida.

Uma correção deverá resultar em uma nova revisão.

```text
Revisão 4
   ↓
Nova edição
   ↓
Revisão 5
```

---

## 18. Numeração

As revisões utilizarão numeração sequencial simples.

```text
Revisão 1
Revisão 2
Revisão 3
...
```

Não será utilizado Semantic Versioning para modelos de serviço.

---

## 19. Alterações que Geram Nova Revisão

Alterações que modifiquem o procedimento deverão gerar uma nova revisão.

Incluem:

- adicionar etapa;
- remover etapa;
- editar conteúdo de etapa;
- reordenar etapas;
- adicionar passo;
- remover passo;
- editar passo;
- alterar instruções;
- reordenar passos.

---

## 20. Alterações Administrativas

Algumas mudanças pertencem à administração do modelo e não necessariamente ao conteúdo do procedimento.

Exemplos:

- alterar nome do modelo;
- alterar descrição administrativa;
- arquivar;
- reativar.

Essas ações não deverão, por si só, gerar nova revisão do roteiro.

---

## 21. Identidade Permanente

Renomear um modelo não cria outro modelo.

Exemplo:

```text
Antes:
Formatação — Contrato

Depois:
Formatação — Cliente Contrato
```

A identidade interna do modelo permanece a mesma.

---

## 22. Preservação Histórica nos Serviços

Um serviço deverá preservar as informações relevantes existentes no momento em que foi criado.

Isso inclui pelo menos:

- identidade do modelo de origem;
- nome utilizado naquele momento;
- número da revisão utilizada.

Dessa forma, renomear um modelo futuramente não deverá alterar a apresentação histórica de serviços anteriores.

---

## 23. Histórico de Revisões

O sistema deverá permitir consultar as revisões de um modelo.

Exemplo:

```text
Histórico de revisões

Revisão 4 — Atual
13/08/2026
Publicado por Carlos

Revisão 3
05/07/2026
Publicado por João

Revisão 2
22/05/2026
Publicado por João

Revisão 1
10/03/2026
Publicado por Carlos
```

---

## 24. Consulta de Revisão

Uma revisão antiga poderá ser aberta para consulta.

Será possível visualizar:

- etapas;
- passos;
- ordem;
- instruções;
- número;
- data de publicação;
- usuário responsável pela publicação;
- resumo de alterações, quando informado.

A revisão será somente leitura.

---

## 25. Resumo da Revisão

Durante a publicação poderá existir um campo opcional:

**Resumo das alterações**

Exemplo:

```text
Adicionada validação da ativação do Windows.
```

Esse campo não será obrigatório.

Seu objetivo é facilitar a compreensão histórica das mudanças.

---

## 26. Um Único Rascunho

Cada modelo poderá possuir no máximo um rascunho de alterações simultâneo.

Não existirão múltiplas linhas paralelas de edição na versão 1.0.

```text
1 revisão publicada atual
+
0 ou 1 rascunho
```

---

## 27. Descartar Alterações

Um rascunho de alterações poderá ser descartado.

Essa ação:

- deverá exigir confirmação;
- removerá apenas as alterações não publicadas;
- não modificará nenhuma revisão publicada.

---

## 28. Duplicação

O sistema deverá permitir duplicar um modelo.

A duplicação criará outro modelo independente.

Exemplo:

```text
Formatação — Cliente Contrato
        ↓ duplicar
Formatação — Cliente Avulso
```

---

## 29. Estado da Duplicação

O novo modelo criado pela duplicação começará como:

```text
Rascunho
```

Ele deverá ser revisado e publicado antes de poder originar serviços.

---

## 30. Revisões da Duplicação

A sequência de revisões não é herdada.

Exemplo:

```text
Modelo original
Revisão 8

      ↓ duplicar

Novo modelo
Rascunho

      ↓ publicar

Revisão 1
```

Cada modelo possui seu próprio histórico.

---

## 31. Origem da Duplicação

Por padrão, a duplicação utilizará a revisão publicada atual do modelo.

Alterações ainda não publicadas não deverão ser incluídas silenciosamente na duplicação.

---

## 32. Arquivamento

Um modelo Ativo poderá ser arquivado.

```text
Ativo → Arquivado
```

Após o arquivamento:

- não será oferecido normalmente para novos serviços;
- continuará consultável;
- continuará ligado aos serviços anteriores;
- continuará preservando revisões.

---

## 33. Arquivar não é Excluir

Modelos já publicados deverão permanecer armazenados.

```text
Arquivar ✓

Excluir permanentemente ✕
```

Isso preserva a rastreabilidade dos serviços criados a partir deles.

---

## 34. Reativação

Um modelo Arquivado poderá ser reativado.

```text
Arquivado → Ativo
```

Sua última revisão publicada volta a ser utilizada como revisão atual.

A reativação não gera uma nova revisão automaticamente.

---

## 35. Edição de Modelo Arquivado

Modelos Arquivados permanecerão em modo de consulta.

Para editar novamente será necessário:

```text
Arquivado
   ↓
Reativar
   ↓
Ativo
   ↓
Editar
```

---

## 36. Arquivamento com Rascunho

Caso exista um rascunho de alterações quando um modelo for arquivado, o sistema deverá alertar o usuário.

O rascunho deverá ser preservado.

Se o modelo for reativado futuramente, as alterações não publicadas poderão continuar disponíveis.

---

## 37. Exclusão de Modelo

A exclusão será permitida somente para modelos que:

- nunca tenham sido publicados; e
- nunca tenham originado qualquer serviço.

Na prática, a exclusão será destinada principalmente a rascunhos criados por engano.

Após a primeira publicação, o mecanismo correto será arquivar.

---

## 38. Revisões não são Excluídas

Uma revisão publicada não poderá ser excluída.

Ela representa parte permanente da história do procedimento.

Isso vale mesmo quando nenhuma execução tenha utilizado especificamente aquela revisão.

---

## 39. Publicação Incorreta

Caso uma revisão seja publicada com erro, ela não será apagada nem editada.

A correção deverá ocorrer através de nova revisão.

```text
Revisão 6 com erro
       ↓
Editar
       ↓
Publicar
       ↓
Revisão 7 corrigida
```

---

## 40. Recuperação de Conteúdo Antigo

Não existirá alteração retroativa de revisão.

Caso o conteúdo de uma revisão antiga precise ser recuperado, ele deverá servir de base para uma nova revisão.

Exemplo:

```text
Revisão atual: 7

Usar conteúdo da Revisão 4 como referência
                 ↓
Nova publicação
                 ↓
Revisão 8
```

A sequência histórica permanece linear.

---

## 41. Uso na Criação de Serviços

Na criação normal de um serviço, somente modelos:

- Ativos;
- com revisão publicada;

serão apresentados.

Ao selecionar um modelo, o sistema utilizará sua revisão publicada atual.

---

## 42. Revisões Antigas

Revisões antigas permanecerão disponíveis para consulta histórica.

A criação normal de serviços na versão 1.0 não permitirá selecionar arbitrariamente uma revisão antiga.

A regra será:

```text
Revisão publicada atual → novos serviços
```

---

## 43. Independência dos Serviços

Depois que um serviço é criado, sua origem permanece fixa.

Exemplo:

```text
Serviço #100
Origem: Revisão 4
```

Se o modelo posteriormente chegar à Revisão 5:

```text
Serviço #100 → continua baseado na Revisão 4
```

Isso vale independentemente do estado atual do serviço.

---

## 44. Atualização Automática de Serviço

A versão 1.0 não terá função automática para atualizar um serviço existente para a revisão mais recente.

Isso evita conflitos com:

- execução já iniciada;
- checklists;
- observações;
- personalizações;
- alterações estruturais.

Quando necessário, o roteiro do serviço poderá ser personalizado manualmente.

---

## 45. Personalização não Cria Revisão

Alterações realizadas dentro de um serviço não geram uma revisão do modelo.

Exemplo:

```text
Modelo Revisão 5
       ↓
Serviço #101
       ↓
Passo adicional no serviço
```

O modelo continua na Revisão 5.

O serviço apenas passa a ser identificado como personalizado.

---

## 46. Modelo versus Serviço

A separação funcional será:

```text
MODELO
│
├── define procedimento reutilizável
├── possui revisões
├── possui publicação
├── pode ser arquivado
└── origina serviços

SERVIÇO
│
├── utiliza uma revisão como origem
├── recebe uma cópia independente
├── possui estados operacionais
├── possui checklists
├── possui observações
└── pode ser personalizado
```

---

## 47. Fluxo Geral

```text
CRIAR MODELO
     ↓
RASCUNHO
     ↓
editar
     ↓
PUBLICAR
     ↓
REVISÃO 1
     ↓
ATIVO
     │
     ├──────► criar serviços
     │
     ├── editar
     │      ↓
     │   RASCUNHO
     │      ↓
     │   publicar
     │      ↓
     │   REVISÃO 2
     │
     └── arquivar
            ↓
        ARQUIVADO
            │
            └── reativar
                   ↓
                 ATIVO
```

---

## 48. Regras Fundamentais

1. Modelo e Revisão são conceitos diferentes.
2. Todo novo modelo começa como Rascunho.
3. Rascunhos não podem originar serviços.
4. A primeira publicação gera Revisão 1.
5. Apenas uma revisão publicada é considerada atual.
6. Revisões publicadas são imutáveis.
7. Alterações no procedimento resultam em nova revisão.
8. A edição de modelo Ativo ocorre em rascunho separado.
9. Pode existir no máximo um rascunho por modelo.
10. Serviços sempre utilizam conteúdo publicado.
11. Novos serviços utilizam normalmente a revisão atual.
12. Revisões antigas permanecem disponíveis para consulta.
13. Numeração de revisões é sequencial e automática.
14. Rascunhos podem ser descartados.
15. Duplicação cria outro modelo independente.
16. Um modelo duplicado inicia sua própria sequência em Revisão 1.
17. Arquivamento não representa exclusão.
18. Modelos Arquivados não originam novos serviços.
19. Modelos publicados não podem ser excluídos no fluxo normal.
20. Revisões publicadas não podem ser excluídas.
21. Correções de revisões publicadas resultam em nova revisão.
22. Serviços não são atualizados automaticamente quando o modelo evolui.
23. Serviços não podem selecionar normalmente revisões antigas na criação.
24. Personalizações de serviço não modificam nem criam revisões do modelo.

---

## 49. Estado da Decisão

**PLANNING-004 — Modelos de Serviço e Sistema de Revisões: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência funcional para futuras decisões relacionadas a modelos, criação de serviços, UX, persistência, histórico e testes.