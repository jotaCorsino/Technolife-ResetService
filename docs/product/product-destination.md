# Reset Service — Product Destination and Implementation Readiness

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Destino do Produto e Critérios de Prontidão para Implementação  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`, `docs/planning/*`, `docs/development/*`

---

## 1. Objetivo

Este documento define:

1. o estado final esperado do Reset Service v1.0;
2. os critérios necessários para encerrar formalmente a fase de planejamento;
3. as condições mínimas para autorizar o início da implementação.

O documento não substitui especificações detalhadas, backlog ou roadmap.

Sua função é responder:

> Quando a v1.0 estiver concluída, o que teremos construído e quais condições precisam existir antes de começarmos a programar?

---

## 2. Destino da v1.0

O Reset Service v1.0 será uma aplicação web interna da Technolife destinada à criação, execução, acompanhamento, documentação e histórico de procedimentos técnicos estruturados.

Fluxo principal:

```text
Administrador cria modelo
        ↓
publica revisão
        ↓
Técnico cria serviço
        ↓
roteiro é copiado
        ↓
serviço é executado
        ↓
observações são registradas
        ↓
progresso é acompanhado
        ↓
serviço é revisado
        ↓
conclusão é registrada
        ↓
documentos são gerados
        ↓
histórico permanece consultável
```

---

## 3. Experiência principal

O usuário deverá utilizar o produto principalmente por:

```text
Abrir navegador
      ↓
https://resetservice/
      ↓
Login
      ↓
Dashboard
      ↓
Abrir ou criar serviço
      ↓
Executar roteiro
```

Não haverá necessidade de instalar software do Reset Service nas estações clientes.

---

## 4. Hospedagem centralizada

O Reset Service será instalado em uma única máquina hospedeira Windows compatível.

A máquina poderá ser:

- desktop;
- notebook;
- Windows Server.

Windows Server não será obrigatório.

A máquina hospedeira executará:

```text
Reset Service
+
ASP.NET Core
+
SQLite
+
Windows Service
```

Os demais computadores acessarão pela rede local.

---

## 5. Independência da internet

O funcionamento operacional do produto não dependerá de conexão com a internet.

Deverão funcionar exclusivamente pela LAN:

- autenticação;
- administração;
- modelos;
- serviços;
- execução;
- observações;
- SignalR;
- multiusuário;
- pesquisa;
- PDFs;
- backup;
- restauração.

Atualizações poderão ser aplicadas por pacote offline.

---

## 6. Perfis de usuário

Existirão dois perfis funcionais:

```text
Administrator
Technician
```

Todos os usuários autenticados poderão visualizar serviços.

Permissões administrativas serão controladas pelo backend.

A identidade do responsável pelo serviço será independente do usuário que executa cada ação.

---

## 7. Modelos de serviço

Administradores poderão criar procedimentos reutilizáveis compostos por:

```text
Modelo
  ↓
Etapas
  ↓
Passos
```

Modelos publicados possuirão revisões imutáveis.

Exemplo:

```text
Preparação de Notebook

Rev 1
Rev 2
Rev 3
```

Serviços novos utilizarão apenas a revisão publicada atualmente vigente.

Serviços existentes nunca serão atualizados automaticamente quando um modelo evoluir.

---

## 8. Roteiro independente

Na criação do serviço:

```text
Modelo Rev 3
      ↓
cópia
      ↓
RS-2026-00142
```

o roteiro será copiado para a própria instância do serviço.

A partir desse momento, o serviço não dependerá do modelo para sua execução.

---

## 9. Identificação dos serviços

Cada serviço receberá identificador permanente:

```text
RS-AAAA-NNNNN
```

Exemplo:

```text
RS-2026-00142
```

O identificador:

- será criado ainda em Draft;
- será único;
- não será editável;
- nunca será reutilizado;
- utilizará sequência anual.

---

## 10. Dados do serviço

Um serviço poderá registrar informações como:

- título;
- cliente;
- empresa;
- telefone;
- e-mail;
- referência;
- equipamento;
- fabricante;
- modelo;
- serial;
- patrimônio;
- hostname;
- sistema operacional;
- responsável.

Cliente e equipamento serão dados do serviço.

A v1.0 não criará CRM ou inventário independente.

---

## 11. Execução operacional

A identidade principal da experiência será:

> **Roteiro. Foco. Progresso.**

O roteiro será apresentado como estrutura vertical organizada por etapas.

Exemplo:

```text
Etapa 1

[✓] Passo concluído
[ ] Passo pendente
[—] Não aplicável
```

Cada etapa terá identidade visual semelhante a uma página do roteiro.

---

## 12. Navegação entre etapas

A navegação será livre.

O usuário poderá acessar etapas posteriores mesmo quando etapas anteriores ainda possuírem pendências.

Concluir o último passo de uma etapa não provocará navegação automática.

A interface poderá destacar a ação:

```text
Próxima etapa
```

---

## 13. Estados dos passos

Existirão exatamente:

```text
Pending
Completed
Not Applicable
```

Não serão criados estados adicionais como:

- opcional;
- crítico;
- recomendado.

A não aplicabilidade será representada por `Not Applicable`.

---

## 14. Progresso

O progresso será calculado automaticamente:

```text
Completed
────────────
Applicable
```

Passos `Not Applicable` serão excluídos do denominador.

O progresso geral utilizará todos os passos aplicáveis do serviço.

---

## 15. Ciclo de vida do serviço

Estados oficiais:

```text
Draft
In Progress
Waiting
Completed
Cancelled
```

Transições serão validadas pelo backend.

Regras fundamentais:

- Waiting exige motivo;
- Cancelled exige motivo;
- Completed exige zero passos Pending;
- estados concluídos/protegidos impedem alterações operacionais normais.

---

## 16. Observações

Observações poderão existir nos níveis:

```text
Service
Stage
Step
```

Visibilidade:

```text
Internal
Client
```

Observações destinadas ao cliente poderão ser classificadas como:

```text
Information
Recommendation
```

---

## 17. Personalização do roteiro

Enquanto permitido pelo estado do serviço, o roteiro copiado poderá ser personalizado.

Será possível:

- adicionar etapa;
- remover etapa;
- editar etapa;
- reordenar etapas;
- adicionar passo;
- remover passo;
- editar passo;
- reordenar passos.

O modelo que originou o serviço permanecerá inalterado.

---

## 18. Uso multiusuário

Mais de um usuário poderá trabalhar simultaneamente no mesmo serviço.

A arquitetura combinará:

```text
Command Queue
+
Optimistic Concurrency
+
SignalR
```

Responsabilidades:

```text
Command Queue
→ ordenar gravações

Optimistic Concurrency
→ impedir sobrescrita silenciosa

SignalR
→ manter navegadores sincronizados
```

---

## 19. Tempo real

Depois de uma alteração persistida com sucesso:

```text
Usuário A altera
      ↓
Servidor processa
      ↓
COMMIT
      ↓
SignalR
      ↓
Usuário B atualiza
```

O navegador não deverá exigir F5 para receber alterações normais do mesmo serviço.

---

## 20. Reconexão

SignalR não será fonte de verdade.

Se um navegador perder a conexão:

```text
desconecta
↓
eventos podem ser perdidos
↓
reconecta
↓
estado atual é consultado novamente
```

O servidor e o banco continuam sendo a autoridade do estado.

---

## 21. Conclusões históricas

Cada conclusão produzirá um ciclo numerado:

```text
c01
c02
c03
```

Cada ciclo possuirá snapshot imutável.

Exemplo:

```text
c01
↓
reabertura
↓
alterações
↓
c02
```

`c01` continuará representando o estado original daquela conclusão.

---

## 22. Documentos

A v1.0 produzirá dois documentos principais.

## Registro Interno de Serviço

Documento completo para uso da Technolife.

## Relatório de Serviço

Documento destinado ao cliente.

Conteúdo `Internal` será removido estruturalmente antes da geração do relatório externo.

---

## 23. Histórico

Serviços permanecerão disponíveis para consulta após:

- conclusão;
- cancelamento;
- reabertura;
- novas conclusões.

Não haverá exclusão operacional normal de serviços históricos.

---

## 24. Dashboard

A tela inicial será orientada à operação.

Deverá priorizar:

- serviços em andamento;
- serviços aguardando;
- concluídos recentes;
- progresso;
- responsável;
- situações que exigem atenção.

Não será utilizado excesso de gráficos sem função operacional clara.

---

## 25. Administração

Administradores terão acesso às áreas administrativas necessárias, incluindo:

```text
Empresa
Documentos
Usuários
Sistema
Modelos
Backup / Restore
```

Técnicos não terão autorização para operações administrativas restritas.

---

## 26. Backup e restauração

O produto possuirá suporte nativo a:

- backup manual;
- backup automático opcional;
- validação;
- catálogo;
- retenção;
- exportação;
- importação;
- restauração integral.

Backup automático poderá permanecer desativado sem comprometer a operação do sistema.

---

## 27. Segurança

A v1.0 incluirá, conforme as especificações aprovadas:

- autenticação local;
- armazenamento seguro de senhas;
- lockout;
- cookies seguros;
- HTTPS;
- antiforgery;
- rate limiting;
- autorização no backend;
- validação de inputs;
- Data Protection;
- proteção de arquivos privados;
- exclusão estrutural de conteúdo interno dos PDFs externos;
- logs sem segredos.

---

## 28. Instalação

A experiência operacional desejada será:

```text
Instalar uma vez
      ↓
Configurar
      ↓
Windows Service
      ↓
https://resetservice/
```

Nenhuma instalação do Reset Service será necessária nas estações clientes.

---

## 29. Atualização

Atualizações serão aplicadas centralmente.

```text
Atualizar máquina hospedeira
        ↓
nova versão entra em execução
        ↓
todos os clientes passam
a utilizar a nova versão
```

Não haverá atualização individual nas estações.

---

## 30. Recuperação completa

Em caso de perda da máquina hospedeira:

```text
nova máquina Windows compatível
        ↓
instalar Reset Service
        ↓
configurar rede e HTTPS
        ↓
importar backup
        ↓
validar
        ↓
restaurar
        ↓
continuar operação
```

---

## 31. Limites da v1.0

O Reset Service não deverá evoluir durante a implementação para:

- ERP;
- CRM completo;
- financeiro;
- faturamento;
- estoque;
- inventário corporativo;
- help desk genérico;
- ticketing;
- portal público;
- plataforma cloud;
- aplicativo mobile;
- sistema de implantação automática do Windows;
- Microsoft 365/AD;
- IA;
- assinatura digital;
- editor avançado de PDF.

Esses limites fazem parte da definição do produto.

---

## 32. Critério funcional de destino atingido

O destino da v1.0 será considerado atingido quando, em uma instalação real, for possível executar integralmente:

```text
Criar primeiro Admin
↓
Configurar Technolife
↓
Criar usuários
↓
Criar modelo
↓
Publicar revisão
↓
Criar serviço
↓
Executar o roteiro
↓
Trabalhar simultaneamente
↓
Registrar observações
↓
Personalizar roteiro
↓
Aguardar e retomar
↓
Concluir
↓
Gerar documentos
↓
Consultar histórico
↓
Criar backup
↓
Restaurar
↓
Atualizar o sistema
```

sem depender do ambiente de desenvolvimento.

---

## 33. Gate de prontidão para implementação

A implementação somente deverá começar quando não houver decisão fundamental conhecida pendente que impeça a construção segura da fundação do produto.

---

## 34. Planejamento de produto necessário

Antes do código deverão estar aprovados:

- Product Specification;
- Service Workflow;
- Service Lifecycle;
- Service Templates/Revisions;
- Service Data;
- Document Generation;
- Users/Access;
- UX/Navigation;
- Non-Functional Requirements;
- Backup/Recovery;
- Security Requirements;
- Product Destination.

---

## 35. Planejamento arquitetural necessário

Antes do código deverão estar definidos:

- stack;
- arquitetura;
- persistência;
- modelo de dados;
- concorrência;
- command queue;
- SignalR;
- autenticação;
- segurança técnica;
- deployment;
- compatibilidade Windows;
- atualização;
- operação.

---

## 36. Planejamento de desenvolvimento necessário

Antes do código deverão existir:

- estratégia de testes;
- roadmap;
- backlog;
- sprint plan;
- especificação do Current State;
- guia de desenvolvimento e trabalho com Codex.

---

## 37. Inventário documental obrigatório

Antes do primeiro prompt de implementação deverão existir, no repositório:

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
│   └── current-state-spec.md
│
└── development/
    ├── testing-strategy.md
    └── development-guide.md
```

`current-state.md` será criado somente no início efetivo da implementação.

---

## 38. README inicial

Antes do primeiro prompt de código, deverá existir um `README.md`.

O README inicial deverá explicar pelo menos:

- o que é o Reset Service;
- finalidade;
- estado atual do projeto;
- arquitetura resumida;
- stack principal;
- estrutura documental;
- referências para roadmap/backlog/sprints;
- onde consultar o Current State quando a implementação começar.

O README não substituirá o futuro manual do usuário.

---

## 39. Revisão consolidada dos documentos

Antes do início da implementação, os documentos deverão ser revisados em conjunto para localizar:

- inconsistências;
- termos antigos;
- duplicações conflitantes;
- caminhos incorretos;
- referências quebradas;
- decisões posteriormente alteradas.

A revisão não deverá alterar decisões aprovadas sem nova discussão.

---

## 40. Normalizações conhecidas

A consolidação deverá corrigir formulações antigas que foram substituídas por decisões posteriores.

Exemplo conhecido:

```text
Backup automático obrigatório
```

não deverá permanecer em documento antigo se a decisão final aprovada for:

```text
Capacidade de backup obrigatória
+
uso do backup automático opcional
```

A decisão mais recente e explicitamente aprovada prevalece.

---

## 41. Tarefa documental antes do código

Depois de todos os documentos estarem salvos localmente, deverá existir uma única tarefa focada para Codex destinada a:

```text
ler documentos
↓
organizar estrutura
↓
revisar consistência
↓
normalizar termos
↓
corrigir referências
↓
criar/ajustar README
↓
não alterar decisões aprovadas
↓
commit
↓
push
```

---

## 42. Essa tarefa será exclusivamente documental

Não deverá incluir:

- solution;
- projetos .NET;
- packages;
- migrations;
- código;
- entidades;
- infraestrutura vazia.

Planejamento e implementação permanecerão separados.

---

## 43. Estado do repositório antes do código

Antes de autorizar o primeiro prompt de implementação:

```text
Branch:
main

Planning docs:
commitados

Remote:
atualizado

Working tree:
Clean
```

Não deverá existir documentação importante somente localmente.

---

## 44. Verificação do GitHub

Depois do commit documental:

```text
Codex entrega relatório
↓
GitHub é verificado
↓
estrutura é conferida
↓
documentos são conferidos
↓
commit é aprovado
```

Somente depois disso a implementação deverá começar.

---

## 45. Current State inicial

Quando o planejamento documental estiver aprovado no GitHub, será criado:

```text
docs/planning/current-state.md
```

Estado inicial esperado:

```text
Versão:
v1.0

Fase:
1 — Fundação da solução

Sprint:
01 — Estrutura da solução

Backlog:
BL-001 — Estrutura inicial da solução

Tarefa:
BL-001/T01 — Criar solution e projetos iniciais
```

---

## 46. Primeiro prompt de implementação

A primeira tarefa de código será:

```text
Sprint 01
BL-001
BL-001/T01
```

O primeiro prompt não deverá solicitar funcionalidades futuras.

Evitar:

```text
Implemente o Reset Service.
```

---

## 47. Checklist final de prontidão

Antes da autorização do primeiro código:

```text
[ ] Todas as especificações aprovadas estão salvas
[ ] Estrutura documental está organizada
[ ] Product Destination existe
[ ] Roadmap existe
[ ] Backlog BL-001–BL-078 existe
[ ] Sprint Plan existe
[ ] Testing Strategy existe
[ ] Development Guide existe
[ ] Current State Spec existe
[ ] README inicial existe
[ ] Inconsistências conhecidas foram normalizadas
[ ] Documentos foram revisados em conjunto
[ ] Documentos foram commitados
[ ] Documentos foram enviados ao GitHub
[ ] Estado no GitHub foi verificado
[ ] Working tree está limpa
[ ] Current State inicial foi criado
[ ] BL-001/T01 está definido
```

Quando todos os itens forem satisfeitos:

> **Planejamento encerrado. Implementação autorizada.**

---

## 48. Planejamento não é imutável

Encerrar o planejamento não significa impedir alterações futuras.

Significa que não existe decisão fundamental conhecida faltando que impeça o início responsável do desenvolvimento.

Se durante implementação surgir nova evidência:

```text
Evidência
↓
Revisão
↓
Decisão
↓
Documentação
↓
Implementação
```

Mudanças estruturais não deverão ocorrer silenciosamente.

---

## 49. Próxima transição

Após esta especificação, o fluxo será:

```text
1. Salvar product-destination.md

2. Preparar README inicial

3. Revisar inventário documental

4. Preparar tarefa documental única para Codex

5. Codex organiza/revisa os documentos

6. Commit e push

7. Verificar GitHub

8. Criar current-state.md

9. Decompor BL-001/T01

10. Criar primeiro prompt de implementação
```

---

## 50. Estado da decisão

**PLANNING-022 — Destino do Produto e Critérios de Prontidão para Implementação: CONCLUÍDA E APROVADA.**

O destino funcional da v1.0 está definido e existe agora um gate objetivo para encerrar a fase de planejamento e autorizar a implementação.