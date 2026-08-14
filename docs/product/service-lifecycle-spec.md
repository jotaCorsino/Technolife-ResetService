# Reset Service — Service Lifecycle Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação do Ciclo de Vida do Serviço  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`

---

## 1. Objetivo

Este documento define o ciclo de vida de um serviço dentro do Reset Service.

Seu escopo inclui:

- criação;
- início;
- espera;
- retomada;
- conclusão;
- cancelamento;
- reabertura;
- proteção de registros;
- histórico de transições;
- relação entre status e progresso.

Este documento não define tecnologias, persistência, banco de dados ou implementação.

---

## 2. Estados oficiais

A versão 1.0 utilizará cinco estados de serviço:

- Rascunho;
- Em andamento;
- Aguardando;
- Concluído;
- Cancelado.

Não serão criados estados adicionais sem necessidade funcional comprovada.

---

## 3. Fluxo principal

```text
Rascunho
   │
   │ iniciar
   ▼
Em andamento ◄───────────────┐
   │                         │
   ├── aguardar ─► Aguardando│
   │                  │      │
   │                  └──────┘
   │                   retomar
   │
   ├── concluir ─► Concluído
   │                  │
   │                  └── reabrir ──┐
   │                                │
   └── cancelar ─► Cancelado        │
                      │             │
                      └── reabrir ──┘
```

---

## 4. Rascunho

Todo serviço deverá nascer no estado **Rascunho**.

Nesse momento, o serviço já deverá possuir:

- ID único;
- data de criação;
- usuário responsável pela criação;
- modelo de origem;
- revisão do modelo;
- cópia independente do roteiro;
- informações de identificação preenchidas, quando disponíveis.

O serviço ainda não é considerado iniciado operacionalmente.

---

## 4.1. Ações permitidas em Rascunho

Será permitido:

- editar informações do serviço;
- preencher ou alterar dados do cliente;
- preencher ou alterar dados do equipamento;
- definir responsável;
- personalizar o roteiro;
- adicionar etapas;
- remover etapas;
- reordenar etapas;
- adicionar passos;
- remover passos;
- reordenar passos;
- adicionar informações iniciais;
- iniciar o serviço;
- cancelar o serviço.

---

## 4.2. Execução em Rascunho

O checklist operacional deverá permanecer inativo enquanto o serviço estiver em Rascunho.

A distinção será:

```text
Rascunho
   ↓
Preparação do serviço

Em andamento
   ↓
Execução do serviço
```

A marcação operacional dos passos começa somente após a ação explícita de início.

---

## 5. Início do serviço

A ação **Iniciar serviço** realizará a transição:

```text
Rascunho → Em andamento
```

O sistema deverá registrar:

- data e hora;
- usuário que realizou a ação.

Após o início:

- checklist fica ativo;
- progresso passa a representar execução operacional;
- evento de início é registrado no histórico.

---

## 6. Em andamento

**Em andamento** é o principal estado de execução.

Nesse estado será permitido:

- executar passos;
- marcar passo como Concluído;
- marcar passo como Não aplicável;
- retornar passo para Pendente;
- adicionar observações;
- editar observações;
- personalizar roteiro;
- alterar informações permitidas do serviço;
- navegar livremente entre etapas;
- alterar responsável quando permitido;
- colocar o serviço em espera;
- cancelar;
- concluir quando todos os requisitos forem atendidos.

---

## 7. Aguardando

O estado **Aguardando** representa uma interrupção temporária da execução.

Exemplos:

- aguardando senha;
- aguardando licença;
- aguardando retorno do cliente;
- aguardando peça;
- aguardando equipamento;
- aguardando informação necessária.

A transição será:

```text
Em andamento → Aguardando
```

---

## 7.1. Motivo da espera

Colocar um serviço em espera deverá exigir um motivo textual.

Exemplo:

```text
Colocar serviço em espera

Motivo:
Aguardando credenciais do cliente.
```

Não será necessário criar categorias complexas de espera na versão 1.0.

---

## 7.2. Comportamento durante espera

Enquanto estiver Aguardando, o serviço continuará consultável.

Será permitido:

- visualizar roteiro;
- visualizar informações;
- consultar observações;
- consultar histórico;
- registrar informação administrativa compatível com a espera;
- retomar o serviço;
- cancelar o serviço.

A execução operacional deverá permanecer bloqueada.

Não será permitido:

- marcar ou desmarcar passos;
- alterar estrutura do roteiro;
- executar novas etapas;
- concluir o serviço.

---

## 8. Retomada

A ação **Retomar serviço** realizará:

```text
Aguardando → Em andamento
```

O sistema deverá registrar:

- data e hora;
- usuário responsável.

O serviço volta imediatamente a permitir execução operacional.

---

## 9. Conclusão

A conclusão somente poderá ocorrer a partir do estado:

**Em andamento**

Transições diretas para Concluído a partir de Rascunho, Aguardando ou Cancelado não serão permitidas.

---

## 9.1. Condições para conclusão

Antes de permitir a conclusão, o sistema deverá verificar se existem passos no estado:

**Pendente**

Se houver qualquer passo pendente, a conclusão deverá ser bloqueada.

Serão aceitos apenas passos:

- Concluídos;
- Não aplicáveis.

---

## 10. Revisão final

Antes da confirmação da conclusão, o sistema deverá apresentar uma revisão.

Ela deverá exibir pelo menos:

- ID do serviço;
- cliente;
- equipamento;
- responsável;
- etapas;
- estado de cada etapa;
- quantidade de passos;
- quantidade de concluídos;
- quantidade de Não aplicáveis;
- quantidade de pendentes;
- observações existentes;
- indicação de roteiro personalizado.

Exemplo:

```text
REVISÃO DO SERVIÇO

#2026-0041
Empresa ABC

✓ Preparação
✓ Backup
✓ Instalação
— Impressoras
✓ Validação

32 concluídos
3 não aplicáveis
0 pendentes

[ Voltar ao roteiro ]

[ Concluir serviço ]
```

---

## 11. Efeitos da conclusão

Ao confirmar:

```text
Em andamento → Concluído
```

o sistema deverá registrar:

- data e hora;
- usuário responsável pela conclusão;
- estado final do roteiro;
- progresso final.

O serviço passa a ser protegido contra alterações operacionais comuns.

---

## 12. Serviço concluído

Em um serviço Concluído será permitido:

- visualizar informações;
- consultar roteiro;
- consultar observações;
- consultar histórico;
- gerar documentos;
- regenerar documentos;
- reabrir quando permitido.

Não será permitido normalmente:

- alterar checklist;
- editar roteiro;
- adicionar etapas;
- remover etapas;
- adicionar passos;
- remover passos;
- adicionar observações operacionais;
- alterar informações do serviço.

Para novas alterações será necessária reabertura explícita.

---

## 13. Documentos e conclusão

A conclusão do serviço e a geração de documentos serão processos separados.

```text
Concluir serviço
      ↓
Serviço Concluído
      ↓
Documentos disponíveis
```

Gerar ou regenerar PDF não deverá alterar o status do serviço.

---

## 14. Cancelamento

O cancelamento representa o encerramento do serviço sem conclusão normal.

Exemplos:

- desistência do cliente;
- abertura incorreta;
- impossibilidade de execução;
- retirada do equipamento;
- substituição por outro serviço.

---

## 14.1. Transições para Cancelado

Serão permitidas:

```text
Rascunho → Cancelado

Em andamento → Cancelado

Aguardando → Cancelado
```

Um serviço Concluído não poderá ser cancelado diretamente.

Caso necessário:

```text
Concluído
   ↓
Reabrir
   ↓
Em andamento
   ↓
Cancelar
   ↓
Cancelado
```

---

## 15. Motivo do cancelamento

O cancelamento deverá exigir um motivo.

Exemplo:

```text
Cancelar serviço?

Motivo:
Cliente solicitou o cancelamento do procedimento.
```

O motivo será:

- registrado no histórico;
- tratado como informação interna;
- elegível para o registro interno.

Não será incluído automaticamente no relatório do cliente.

---

## 16. Persistência de serviços cancelados

Serviços cancelados deverão permanecer armazenados.

Eles não serão removidos do histórico.

Exemplo:

```text
#0041  Concluído
#0042  Cancelado
#0043  Em andamento
```

O identificador `#0042` jamais poderá ser reutilizado.

---

## 17. Exclusão

A operação normal da versão 1.0 não permitirá exclusão de serviços.

Depois que um ID é criado, o registro deverá permanecer no sistema.

Um serviço criado incorretamente deverá ser cancelado, e não apagado.

Essa regra existe para preservar:

- sequência de identificadores;
- histórico;
- rastreabilidade;
- integridade dos registros.

Uma eventual ferramenta administrativa excepcional de exclusão não faz parte do fluxo normal da versão 1.0.

---

## 18. Reabertura

Serviços Concluídos e Cancelados poderão ser reabertos quando necessário.

A ação deverá ser explícita.

```text
Concluído → Em andamento

Cancelado → Em andamento
```

A reabertura nunca levará o serviço novamente para Rascunho.

---

## 19. Motivo da reabertura

A reabertura deverá exigir um motivo.

O sistema deverá registrar:

- estado anterior;
- data e hora;
- usuário responsável;
- motivo.

Exemplo:

```text
Reabrir serviço?

Motivo:
Cliente retornou para finalizar uma configuração.
```

---

## 20. Progresso após reabertura

Estado e progresso são conceitos independentes.

Um serviço reaberto poderá estar:

```text
Status: Em andamento
Progresso: 100%
```

Isso é válido quando nenhum novo passo pendente foi criado.

Caso um novo passo seja adicionado:

```text
10 concluídos / 11 aplicáveis
```

o progresso será recalculado automaticamente.

---

## 21. Histórico de conclusões

Uma reabertura não deverá apagar uma conclusão anterior.

Exemplo:

```text
Conclusão 1
    ↓
Reabertura
    ↓
Conclusão 2
```

As duas conclusões deverão permanecer identificáveis historicamente.

---

## 22. Documentos após reabertura

Documentos gerados em uma conclusão anterior representam aquela conclusão histórica.

Se o serviço for reaberto e posteriormente concluído novamente, deverá existir uma nova geração documental relacionada à nova conclusão.

Conceitualmente:

```text
Conclusão 1
├── Registro interno
└── Relatório cliente

Reabertura

Conclusão 2
├── Registro interno atualizado
└── Relatório cliente atualizado
```

Documentos históricos não deverão ser silenciosamente sobrescritos de forma que a conclusão anterior deixe de ser rastreável.

A estratégia de armazenamento será definida posteriormente.

---

## 23. Histórico de transições

Mudanças relevantes de estado deverão gerar registros históricos.

Exemplo:

```text
09:02 Serviço criado por João

09:08 Serviço iniciado por João

10:34 Serviço colocado em espera por João
Motivo: aguardando licença.

11:22 Serviço retomado por João

13:14 Serviço concluído por João

15/08 09:10 Serviço reaberto por Carlos
Motivo: instalação adicional solicitada.

15/08 10:02 Serviço concluído por Carlos
```

---

## 24. Responsabilidade

O sistema deverá distinguir:

**Responsável pelo serviço**

de:

**Usuário que executou determinada ação**

Exemplo:

```text
Responsável atual:
João
```

Carlos poderá eventualmente realizar uma ação administrativa ou operacional permitida.

Nesse caso, o histórico deverá registrar Carlos como autor da ação sem necessariamente alterar o responsável pelo serviço.

---

## 25. Alteração de responsável

O responsável poderá ser alterado enquanto o serviço estiver:

- Rascunho;
- Em andamento;
- Aguardando.

A alteração deverá gerar evento no histórico.

Serviços Concluídos ou Cancelados deverão ser reabertos antes de alterações operacionais de responsabilidade.

---

## 26. Datas relevantes

O serviço deverá possuir conceitualmente pelo menos:

- data de criação;
- data de início;
- data da conclusão vigente, quando aplicável.

Outros acontecimentos deverão permanecer registrados através do histórico de eventos.

A representação técnica dessas datas será definida posteriormente durante a arquitetura.

---

## 27. Status e progresso

O status representa a situação operacional.

O progresso representa a execução dos passos.

São conceitos independentes.

| Estado | Progresso possível |
|---|---:|
| Rascunho | 0% operacional |
| Em andamento | 0–100% |
| Aguardando | 0–100% |
| Concluído | 100% |
| Cancelado | 0–100% |

Um serviço Em andamento poderá possuir 100% enquanto aguarda a confirmação formal de conclusão.

Um serviço Aguardando também poderá possuir 100% se todos os passos já foram executados, mas existir alguma dependência externa.

---

## 28. Transições permitidas

As únicas transições válidas serão:

```text
Rascunho
├── Em andamento
└── Cancelado

Em andamento
├── Aguardando
├── Concluído
└── Cancelado

Aguardando
├── Em andamento
└── Cancelado

Concluído
└── Em andamento
    por reabertura

Cancelado
└── Em andamento
    por reabertura
```

Transições não listadas deverão ser rejeitadas.

---

## 29. Ações por estado

| Ação | Rascunho | Em andamento | Aguardando | Concluído | Cancelado |
|---|---|---|---|---|---|
| Editar dados | Sim | Sim | Limitado | Não | Não |
| Personalizar roteiro | Sim | Sim | Não | Não | Não |
| Executar checklist | Não | Sim | Não | Não | Não |
| Adicionar observação operacional | Inicial | Sim | Limitado | Não | Não |
| Colocar em espera | Não | Sim | — | Não | Não |
| Retomar | Não | — | Sim | Não | Não |
| Concluir | Não | Sim | Não | — | Não |
| Cancelar | Sim | Sim | Sim | Não | — |
| Reabrir | Não | Não | Não | Sim | Sim |
| Gerar documentos finais | Não | Não | Não | Sim | Conforme necessidade |

A implementação deverá detalhar posteriormente quais campos específicos permanecem editáveis durante o estado Aguardando.

---

## 30. Confirmações

Ações com impacto significativo deverão utilizar confirmação apropriada.

Especialmente:

- concluir;
- cancelar;
- reabrir.

Colocar em espera não deverá exigir uma segunda confirmação depois que o usuário já informou o motivo.

Retomar e iniciar deverão privilegiar fluidez operacional.

---

## 31. Integridade histórica

Uma transição nunca deverá apagar acontecimentos anteriores.

Exemplo:

```text
Criado
↓
Iniciado
↓
Concluído
↓
Reaberto
↓
Concluído novamente
```

Não poderá ser reduzido posteriormente para:

```text
Criado
↓
Concluído
```

A linha do tempo deverá preservar a história real do serviço.

---

## 32. Regras fundamentais

1. Todo serviço nasce como Rascunho.
2. Todo serviço recebe seu ID antes de iniciar a execução.
3. IDs nunca podem ser reutilizados.
4. Checklist somente pode ser executado em Em andamento.
5. Aguardando representa interrupção real da execução.
6. Colocar em espera exige motivo.
7. Conclusão normal somente ocorre a partir de Em andamento.
8. Nenhum passo Pendente é permitido na conclusão.
9. Serviços Concluídos ficam protegidos.
10. Cancelamento exige motivo.
11. Serviços Cancelados permanecem armazenados.
12. Serviços não são excluídos no fluxo normal.
13. Reabertura exige motivo.
14. Concluídos e Cancelados reabrem como Em andamento.
15. Estado e progresso são independentes.
16. Toda transição relevante gera histórico.
17. Reabertura não elimina conclusões anteriores.
18. Documentos históricos devem permanecer vinculados à respectiva conclusão.

---

## 33. Estado da decisão

**PLANNING-003 — Ciclo de vida do Serviço: CONCLUÍDA E APROVADA.**

Este documento formaliza o ciclo operacional de um serviço no Reset Service e deverá ser considerado em especificações futuras de UX, segurança, arquitetura, persistência e testes.