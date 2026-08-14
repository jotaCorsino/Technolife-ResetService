# Reset Service — Sprint Plan

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Plano de Sprints  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/planning/roadmap.md`, `docs/planning/backlog.md`, `docs/development/testing-strategy.md`, `docs/product/*`, `docs/architecture/*`

---

## 1. Objetivo

Este documento organiza os itens do backlog mestre da v1.0 em ciclos progressivos de desenvolvimento.

As sprints são orientadas a resultado.

Não representam obrigatoriamente períodos fixos de calendário.

Uma sprint será concluída quando seu objetivo estiver:

- implementado;
- testado;
- revisado;
- integrado;
- documentado quando necessário;
- commitado e enviado ao repositório.

---

## 2. Relação entre planejamento e implementação

A hierarquia será:

```text
Roadmap
   ↓
Backlog
   ↓
Sprint
   ↓
Tarefa técnica
   ↓
Prompt focado para Codex
   ↓
Implementação
   ↓
Testes
   ↓
Commit / Push
   ↓
Verificação
```

Uma sprint inteira não deverá ser enviada ao Codex como um único prompt.

---

## 3. Quantidade de sprints

A v1.0 será inicialmente organizada em:

```text
26 sprints
```

distribuindo os 78 itens do backlog mestre.

O número poderá ser refinado durante a execução quando houver justificativa técnica, sem alterar os requisitos do produto.

---

## 4. Sprint 01 — Estrutura da solução

**Backlog:** BL-001, BL-002

## Objetivo

Criar:

- solution;
- projetos;
- referências;
- configuração por ambiente.

Resultado esperado:

```text
ResetService.sln
+
Web
+
Core
+
Infrastructure
+
configuração-base
```

## Gate

- build Release aprovado;
- arquitetura de referências respeitada;
- Development/Test/Production preparados;
- nenhuma feature de negócio antecipada.

---

## 5. Sprint 02 — Persistência e concorrência-base

**Backlog:** BL-003, BL-004

## Objetivo

Implementar:

- EF Core;
- SQLite;
- DbContext;
- migration inicial;
- padrão de concorrência otimista;
- tokens `Version`.

## Gate

- banco inicial criado por migration;
- persistência testada;
- conflito baseado em versão antiga detectável;
- nenhuma sobrescrita silenciosa.

---

## 6. Sprint 03 — Pipeline de comandos e infraestrutura

**Backlog:** BL-005, BL-006

## Objetivo

Implementar:

- `System.Threading.Channels`;
- command processing;
- scopes;
- logging;
- tratamento global de erros;
- health checks;
- SignalR básico.

Resultado:

```text
Request
   ↓
Command Queue
   ↓
Execution
   ↓
Result
```

## Gate

- fila funcionando;
- health check funcionando;
- erros inesperados registrados;
- infraestrutura SignalR inicial disponível.

---

## 7. Sprint 04 — Identity e primeiro acesso

**Backlog:** BL-007, BL-008, BL-009

## Objetivo

Implementar:

- ASP.NET Core Identity;
- roles;
- bootstrap local;
- primeiro Administrador;
- login;
- logout;
- sessão.

Resultado:

```text
Instalação nova
      ↓
Primeiro Admin
      ↓
Login
      ↓
Aplicação protegida
```

## Gate

Acesso anônimo às funcionalidades de negócio deve estar bloqueado.

---

## 8. Sprint 05 — Hardening da autenticação

**Backlog:** BL-010, BL-011, BL-012

## Objetivo

Implementar:

- política de senha;
- lockout;
- credencial temporária;
- policies;
- antiforgery;
- rate limiting;
- Data Protection;
- security headers;
- proteção inicial do SignalR.

## Gate

Suíte de segurança-base aprovada.

Depois desta sprint, toda feature nova deverá respeitar autenticação e autorização desde sua criação.

---

## 9. Sprint 06 — Empresa e documentos-base

**Backlog:** BL-013, BL-014, BL-015

## Objetivo

Implementar:

- dados da Technolife;
- logo;
- StoredAsset;
- configurações documentais iniciais.

Resultado:

Identidade institucional pronta para ser utilizada posteriormente pelos documentos.

---

## 10. Sprint 07 — Usuários e configurações administrativas

**Backlog:** BL-016, BL-017, BL-018

## Objetivo

Implementar:

- administração de usuários;
- perfil pessoal;
- alteração de senha;
- ativação/desativação;
- alteração de perfil;
- proteção do último Administrador;
- configurações tipadas do sistema.

## Gate

Administração básica completa.

---

## 11. Marco A — Fundação operacional

Após a Sprint 07:

```text
Aplicação
+
Banco
+
Segurança
+
Usuários
+
Configuração institucional
```

---

## 12. Sprint 08 — Construção de modelos

**Backlog:** BL-019, BL-020, BL-021

## Objetivo

Implementar:

- modelo Draft;
- etapas;
- passos;
- editor inicial;
- ordenação;
- validação estrutural.

Resultado:

Um procedimento pode ser construído completamente antes da publicação.

---

## 13. Sprint 09 — Publicação e revisões

**Backlog:** BL-022, BL-023, BL-024

## Objetivo

Implementar:

- publicação;
- Revision 1+;
- próxima revisão Draft;
- duplicação;
- arquivamento;
- reativação;
- descarte de Draft;
- histórico de revisões.

## Gate

Revisões publicadas comprovadamente imutáveis.

---

## 14. Sprint 10 — Identidade e criação do serviço

**Backlog:** BL-025, BL-026, BL-027

## Objetivo

Implementar:

```text
Modelo publicado
       ↓
Novo serviço
       ↓
RS-AAAA-NNNNN
       ↓
Cópia independente do roteiro
```

## Gate

- geração transacional;
- nenhuma duplicação sob concorrência;
- roteiro copiado não depende mais da revisão para execução.

---

## 15. Sprint 11 — Dados e listagem dos serviços

**Backlog:** BL-028, BL-029, BL-030

## Objetivo

Implementar:

- cliente;
- equipamento;
- responsável;
- origem histórica;
- listagem;
- abertura;
- busca inicial.

Resultado:

Serviços podem ser criados, identificados, localizados e reabertos pela interface.

---

## 16. Sprint 12 — Execução do roteiro

**Backlog:** BL-031, BL-032, BL-033, BL-034

## Objetivo

Implementar:

- tela operacional;
- páginas/etapas;
- navegação;
- Pending;
- Completed;
- Not Applicable;
- progresso;
- estado calculado.

Resultado:

```text
Serviço
  ↓
Etapas
  ↓
Passos
  ↓
Execução
```

funcionando operacionalmente.

---

## 17. Sprint 13 — Ciclo de vida completo

**Backlog:** BL-035, BL-036, BL-037, BL-038

## Objetivo

Implementar:

- iniciar;
- aguardar;
- retomar;
- cancelar;
- revisão final;
- concluir.

## Gate

Todas as transições inválidas conhecidas devem ser rejeitadas pelo backend.

---

## 18. Sprint 14 — Observações

**Backlog:** BL-039, BL-040, BL-041

## Objetivo

Implementar:

- observação de serviço;
- observação de etapa;
- observação de passo;
- Internal;
- Client;
- Information;
- Recommendation;
- edição;
- remoção controlada.

Resultado:

O serviço passa a registrar exceções e contexto além do checklist.

---

## 19. Sprint 15 — Personalização e histórico

**Backlog:** BL-042, BL-043, BL-044

## Objetivo

Implementar:

- personalização estrutural;
- adicionar/remover/mover;
- `IsRouteCustomized`;
- ServiceEvent;
- timeline;
- motivos;
- autoria.

---

## 20. Marco B — Núcleo operacional

Após a Sprint 15:

```text
Modelo
   ↓
Serviço
   ↓
Roteiro
   ↓
Execução
   ↓
Observações
   ↓
Personalização
   ↓
Conclusão lógica
```

---

## 21. Sprint 16 — Tempo real

**Backlog:** BL-045, BL-046, BL-047

## Objetivo

Implementar:

- grupos SignalR;
- checklist em tempo real;
- observações;
- status;
- responsável;
- detalhes;
- roteiro.

Cenário obrigatório:

```text
Browser A altera
      ↓
COMMIT
      ↓
SignalR
      ↓
Browser B atualiza
sem F5
```

---

## 22. Sprint 17 — Concorrência multiusuário robusta

**Backlog:** BL-048, BL-049, BL-050

## Objetivo

Implementar e validar:

- conflitos por `Version`;
- reconexão;
- resincronização;
- idempotência;
- carga concorrente.

## Gate obrigatório

Múltiplos usuários deverão conseguir trabalhar no mesmo serviço sem:

- perda de alteração confirmada;
- sobrescrita silenciosa;
- duplicação indevida;
- estado inconsistente.

---

## 23. Sprint 18 — Conclusões históricas

**Backlog:** BL-051, BL-052

## Objetivo

Implementar:

- `ServiceConclusion`;
- c01/c02/etc.;
- snapshot imutável;
- schema version;
- hash de integridade.

Resultado:

```text
Conclusão
   ↓
Snapshot histórico imutável
```

---

## 24. Sprint 19 — Documentos

**Backlog:** BL-053, BL-054, BL-055, BL-056

## Objetivo

Implementar:

- Registro Interno;
- Relatório de Serviço;
- PDFsharp/MigraDoc;
- prévia;
- regeneração;
- documentos por ciclo de conclusão.

## Gate crítico

```text
Internal
→ nunca aparece no Relatório de Serviço
```

---

## 25. Sprint 20 — Dashboard, busca e histórico operacional

**Backlog:** BL-057, BL-058, BL-059, BL-060

## Objetivo

Implementar:

- Dashboard;
- busca;
- filtros;
- histórico de serviços.

Resultado:

Navegação operacional completa sobre serviços atuais e históricos.

---

## 26. Sprint 21 — Consolidação de UX

**Backlog:** BL-061, BL-062

## Objetivo

Refinar:

- loading;
- erros;
- estados vazios;
- confirmações;
- 1366×768;
- Chrome;
- Edge;
- teclado;
- foco;
- acessibilidade básica.

---

## 27. Marco C — Produto operacional multiusuário

Após a Sprint 21:

```text
Núcleo operacional
+
Tempo real
+
Concorrência
+
Documentos
+
Dashboard
+
Pesquisa
+
UX consolidada
```

---

## 28. Sprint 22 — Backup operacional

**Backlog:** BL-063, BL-064, BL-065

## Objetivo

Implementar:

- configuração;
- backup manual;
- backup automático opcional.

Cenários obrigatórios:

```text
AutomaticBackup = false
→ produto funciona normalmente
```

e:

```text
AutomaticBackup = true
→ rotina configurada funciona
```

---

## 29. Sprint 23 — Recuperação

**Backlog:** BL-066, BL-067, BL-068

## Objetivo

Implementar:

- manifesto;
- catálogo;
- retenção;
- exportação;
- importação;
- restauração integral;
- maintenance mode;
- invalidação de sessões.

## Gate obrigatório

```text
Estado A
↓
Backup
↓
Estado B
↓
Restore
↓
Estado A recuperado
```

---

## 30. Sprint 24 — Distribuição Windows

**Backlog:** BL-069, BL-070, BL-071, BL-072

## Objetivo

Implementar:

- publicação `win-x64`;
- self-contained;
- instalação;
- Windows Service;
- ACL;
- firewall;
- HTTPS;
- hostname/DNS;
- startup automático;
- recuperação do serviço.

Resultado esperado:

```text
Instalar em máquina compatível
        ↓
Reiniciar
        ↓
https://resetservice/
        ↓
Sistema disponível
```

---

## 31. Sprint 25 — Atualização e manutenção

**Backlog:** BL-073, BL-074, BL-075

## Objetivo

Implementar:

- processo de atualização;
- maintenance mode;
- queue drain;
- migration bundle;
- health check;
- rollback de binários;
- desinstalação;
- reinstalação segura.

## Gate

Atualização de versão anterior representativa deverá ocorrer sem perda de dados.

---

## 32. Sprint 26 — Validação final e release v1.0

**Backlog:** BL-076, BL-077, BL-078

## Objetivo

Esta sprint não deverá introduzir novas funcionalidades planejadas.

Ela servirá para validar o produto completo.

Inclui:

- Windows 10 alvo;
- Windows 11;
- Chrome;
- Edge;
- desktop;
- notebook;
- acesso LAN;
- SignalR;
- multiusuário;
- concorrência;
- serviço grande;
- volume representativo;
- SQLite;
- segurança;
- documentos;
- backup/restauração;
- instalação;
- atualização;
- documentação final.

Se SQLite não atender aos testes representativos, deverá ser formalmente reavaliado antes da liberação.

---

## 33. Marco D — Reset Service v1.0

Após a Sprint 26:

```text
Reset Service v1.0
```

deverá estar:

- instalável;
- operacional;
- multiusuário;
- protegido;
- documentado;
- recuperável;
- atualizável;
- validado nos ambientes oficiais.

---

## 34. Regras de execução das sprints

1. Uma sprint define resultados, não um único prompt de implementação.
2. Cada backlog item deverá ser quebrado em tarefas menores antes de implementação.
3. Cada tarefa receberá um prompt específico para Codex.
4. Uma tarefa deve produzir mudança pequena, revisável e testável.
5. Testes aplicáveis serão executados antes da tarefa ser considerada pronta.
6. Correções encontradas durante revisão pertencem à tarefa atual.
7. Não avançar conscientemente com working tree suja da tarefa anterior.
8. Não antecipar funcionalidade de sprint futura sem necessidade técnica explícita.
9. Refatorações significativas deverão ter objetivo identificado.
10. Testes acompanham as funcionalidades na própria sprint.
11. Documentação afetada deve permanecer consistente.
12. O GitHub continuará sendo a fonte de verdade da implementação.

---

## 35. Fluxo de uma tarefa

```text
Selecionar tarefa
      ↓
Definir escopo
      ↓
Criar prompt para Codex
      ↓
Implementar
      ↓
Executar testes
      ↓
Codex apresenta relatório
      ↓
Commit
      ↓
Push
      ↓
Verificação
      ↓
Aprovar ou corrigir
      ↓
Próxima tarefa
```

---

## 36. Current State

Durante implementação deverá existir:

```text
docs/planning/current-state.md
```

Esse documento deverá responder rapidamente:

- qual sprint está ativa;
- qual backlog item está ativo;
- qual tarefa está ativa;
- último commit aprovado;
- o que está concluído;
- o que está bloqueado;
- qual é o próximo passo.

Ele deverá permanecer curto e operacional.

---

## 37. Critério de conclusão da sprint

Uma sprint será considerada concluída somente quando:

```text
Itens planejados concluídos
+
Testes aprovados
+
Correções concluídas
+
Documentação consistente
+
Commits enviados
+
GitHub verificado
+
Working tree limpa
```

Código escrito sem validação não significa sprint concluída.

---

## 38. Resumo das sprints

| Sprint | Foco |
|---|---|
| 01 | Estrutura da solução |
| 02 | Persistência e concorrência |
| 03 | Fila e infraestrutura |
| 04 | Identity e primeiro acesso |
| 05 | Segurança-base |
| 06 | Empresa e documentos-base |
| 07 | Usuários e configurações |
| 08 | Construção de modelos |
| 09 | Revisões |
| 10 | Criação de serviços |
| 11 | Dados dos serviços |
| 12 | Execução |
| 13 | Ciclo de vida |
| 14 | Observações |
| 15 | Personalização e histórico |
| 16 | Tempo real |
| 17 | Concorrência multiusuário |
| 18 | Conclusões |
| 19 | PDFs |
| 20 | Dashboard e pesquisa |
| 21 | UX |
| 22 | Backup |
| 23 | Restauração |
| 24 | Distribuição Windows |
| 25 | Atualização |
| 26 | Validação da v1.0 |

---

## 39. Estado da decisão

**PLANNING-019 — Plano de Sprints: CONCLUÍDA E APROVADA.**

O desenvolvimento da v1.0 será organizado inicialmente em **26 sprints progressivas**, mantendo tarefas e prompts de implementação menores do que o escopo de cada sprint.