# Reset Service — Development Roadmap

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Roadmap de Desenvolvimento  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`, `docs/development/testing-strategy.md`

---

## 1. Objetivo

Este documento organiza a construção completa do Reset Service v1.0 em fases sequenciais.

O roadmap não representa ainda:

- sprint;
- tarefa de implementação;
- prompt para Codex;
- cronograma com datas.

Seu objetivo é definir **a ordem lógica de construção do produto**.

---

## 2. Princípio

A implementação deverá seguir uma sequência em que cada fase prepare corretamente a próxima.

Prioridade:

```text
Fundação
↓
Segurança
↓
Dados e administração
↓
Modelos
↓
Serviços
↓
Execução
↓
Multiusuário
↓
Documentos
↓
Experiência consolidada
↓
Recuperação
↓
Distribuição
```

Telas não deverão ser construídas isoladamente antes das regras, persistência e segurança necessárias.

---

## 3. Fase 1 — Fundação da solução

## Objetivo

Criar a base técnica do Reset Service.

## Principais entregas

- solução .NET 10;
- projetos Web, Core e Infrastructure;
- estrutura inicial de testes;
- configuração por ambiente;
- SQLite;
- EF Core;
- migrations iniciais;
- logging;
- tratamento de erros;
- tokens de concorrência;
- fila de comandos;
- SignalR básico;
- health checks;
- convenções iniciais de desenvolvimento;
- build e testes funcionando.

## Resultado esperado

```text
Aplicação inicia
↓
SQLite funciona
↓
fila funciona
↓
SignalR conecta
↓
health check responde
↓
testes executam
```

---

## 4. Fase 2 — Identidade, autenticação e segurança-base

## Objetivo

Estabelecer a fronteira de segurança antes das funcionalidades operacionais.

## Principais entregas

- ASP.NET Core Identity;
- ApplicationUser;
- roles Administrator e Technician;
- criação local do primeiro Administrador;
- login;
- logout;
- alteração de senha;
- redefinição administrativa;
- credencial temporária;
- lockout;
- rate limiting;
- cookie seguro;
- Security Stamp;
- antiforgery;
- policies;
- Data Protection;
- headers iniciais de segurança.

## Resultado esperado

```text
Instalação nova
↓
Primeiro Administrador
↓
Login
↓
Sessão protegida
↓
Autorização funcionando
```

---

## 5. Fase 3 — Configuração institucional e administração

## Objetivo

Preparar o ambiente administrativo da Technolife.

## Principais entregas

- dados da empresa;
- logo;
- configurações documentais iniciais;
- configurações do sistema;
- criação de usuários;
- alteração de usuários;
- ativação e desativação;
- mudança de perfil;
- proteção do último Administrador;
- visualização administrativa básica.

## Resultado esperado

Ambiente institucional configurado e usuários aptos a operar.

---

## 6. Fase 4 — Modelos de serviço e revisões

## Objetivo

Construir a origem reutilizável dos roteiros.

## Principais entregas

- listagem de modelos;
- criação;
- Draft;
- Active;
- Archived;
- etapas;
- passos;
- instruções;
- ordenação;
- validação;
- publicação;
- numeração de revisões;
- Draft da próxima revisão;
- descarte de Draft;
- duplicação;
- arquivamento;
- reativação;
- histórico de revisões;
- permissões de acesso;
- editor visual de roteiro.

## Resultado esperado

```text
Administrador
↓
cria modelo
↓
monta roteiro
↓
publica
↓
modelo fica disponível
```

---

## 7. Fase 5 — Criação e identificação de serviços

## Objetivo

Transformar uma revisão publicada em uma instância operacional independente.

## Principais entregas

- geração `RS-AAAA-NNNNN`;
- sequência anual;
- criação transacional;
- seleção de modelo;
- cópia independente do roteiro;
- título;
- cliente;
- equipamento;
- responsável;
- origem;
- estado Draft;
- evento de criação;
- pesquisa inicial;
- restrições de unicidade.

## Resultado esperado

```text
Modelo publicado
↓
Novo serviço
↓
RS-AAAA-NNNNN
↓
roteiro independente
```

---

## 8. Fase 6 — Execução do serviço

## Objetivo

Implementar o núcleo operacional do produto.

## Principais entregas

- tela principal do serviço;
- estrutura visual de páginas/etapas;
- navegação entre etapas;
- Pending;
- Completed;
- Not Applicable;
- progresso;
- estado calculado das etapas;
- iniciar;
- aguardar;
- retomar;
- cancelar;
- alterar responsável;
- validação do ciclo de vida;
- revisão antes da conclusão;
- conclusão lógica;
- bloqueios conforme estado.

## Resultado esperado

Serviço completo executável do início à conclusão.

---

## 9. Fase 7 — Observações, personalização e histórico

## Objetivo

Completar o registro operacional do trabalho realizado.

## Principais entregas

- observação de serviço;
- observação de etapa;
- observação de passo;
- Internal;
- Client;
- Information;
- Recommendation;
- edição permitida;
- remoção controlada;
- soft delete;
- adicionar etapas;
- adicionar passos;
- editar roteiro copiado;
- reordenar;
- remover elementos;
- `IsRouteCustomized`;
- confirmações;
- ServiceEvent;
- timeline;
- motivos;
- autoria.

## Resultado esperado

O serviço representa o trabalho executado, suas exceções e seu histórico.

---

## 10. Fase 8 — Multiusuário e tempo real

## Objetivo

Permitir uso simultâneo seguro do mesmo serviço.

## Principais entregas

- grupos SignalR por contexto;
- atualização automática de passos;
- observações em tempo real;
- status;
- responsável;
- detalhes;
- roteiro;
- reconexão;
- resincronização;
- concorrência otimista;
- mensagens de conflito;
- aplicação completa da fila de comandos;
- idempotência de operações críticas;
- testes simultâneos.

## Resultado esperado

```text
Usuário A altera
↓
fila
↓
COMMIT
↓
SignalR
↓
Usuário B atualiza automaticamente
```

Sem F5 e sem sobrescrita silenciosa.

---

## 11. Fase 9 — Conclusões e documentos

## Objetivo

Produzir documentação histórica confiável.

## Principais entregas

- ServiceConclusion;
- c01, c02 etc.;
- snapshot imutável;
- versão do snapshot;
- hash;
- Registro Interno de Serviço;
- Relatório de Serviço;
- PDFsharp;
- MigraDoc;
- identidade institucional;
- dados do cliente;
- equipamento;
- roteiro;
- observações;
- filtragem interno/cliente;
- prévia;
- regeneração;
- conclusões históricas;
- documento interno de serviço cancelado.

## Resultado esperado

```text
Conclusão
↓
snapshot histórico
↓
Registro Interno
+
Relatório de Serviço
```

---

## 12. Fase 10 — Dashboard, pesquisa e UX consolidada

## Objetivo

Transformar as funcionalidades existentes na experiência final planejada.

## Principais entregas

- Dashboard;
- serviços ativos;
- aguardando;
- recentes;
- progresso;
- etapa atual;
- pesquisa;
- filtros;
- histórico de serviços;
- estados vazios;
- navegação definitiva;
- feedback de processamento;
- mensagens de erro;
- teclado;
- foco;
- acessibilidade básica;
- responsividade em 1366×768;
- refinamento visual.

## Resultado esperado

Produto operacional coeso, navegável e consistente.

---

## 13. Fase 11 — Backup, restauração e recuperação

## Objetivo

Completar a proteção operacional dos dados.

## Principais entregas

- backup automático opcional;
- ativar/desativar;
- horário;
- destino;
- retenção;
- backup manual;
- manifesto;
- validação;
- catálogo;
- exportação;
- importação;
- restauração integral;
- backup pré-restauração;
- modo de manutenção;
- invalidação de sessões;
- tratamento de falhas;
- armazenamento;
- recuperação em nova instalação compatível.

## Resultado esperado

O produto pode ser protegido e recuperado sem administração manual do banco.

---

## 14. Fase 12 — Distribuição, atualização e preparação da v1.0

## Objetivo

Transformar o software desenvolvido em produto distribuível e operável.

## Principais entregas

- publicação `win-x64`;
- self-contained;
- distribuição por pasta local;
- preparação única da máquina hospedeira;
- execução sob demanda por `ResetService.exe`;
- abertura do navegador padrão no host;
- bloqueio de segunda instância no mesmo host;
- encerramento planejado com drenagem da fila;
- término completo do processo;
- ACLs;
- firewall;
- HTTPS;
- certificado;
- hostname/DNS;
- suporte a desktop/notebook hospedeiro;
- updater;
- modo de manutenção;
- drenagem da fila;
- migrations controladas;
- migration bundle;
- health check;
- rollback de binários;
- remoção segura dos binários;
- recuperação completa;
- Windows 10;
- Windows 11;
- Chrome;
- Edge;
- carga;
- multiusuário;
- revisão final de segurança;
- revisão documental;
- documentação final.

## Resultado esperado

```text
Distribuir
↓
configurar
↓
executar sob demanda
↓
acessar pela LAN
↓
operar
↓
encerrar completamente
↓
atualizar
↓
recuperar
```

sem ambiente de desenvolvimento.

---

## 15. Ordem oficial

```text
01 Fundação
      ↓
02 Identidade e segurança
      ↓
03 Administração e configurações
      ↓
04 Modelos e revisões
      ↓
05 Criação de serviços
      ↓
06 Execução
      ↓
07 Observações, personalização e histórico
      ↓
08 Multiusuário e tempo real
      ↓
09 Conclusões e documentos
      ↓
10 Dashboard, pesquisa e UX
      ↓
11 Backup e recuperação
      ↓
12 Implantação e release
      ↓
Reset Service v1.0
```

---

## 16. Capacidades transversais

Algumas capacidades atravessam várias fases.

## Testes

Começam na Fase 1 e evoluem durante todo o desenvolvimento.

## Segurança

Tem sua fundação na Fase 2 e será aplicada a todas as fases posteriores.

## SignalR

Infraestrutura começa na Fase 1 e amadurece funcionalmente na Fase 8.

## Documentos

Configuração institucional começa na Fase 3 e geração completa ocorre na Fase 9.

## Backup

A arquitetura é considerada desde a fundação e a funcionalidade completa é entregue na Fase 11.

---

## 17. Marcos internos

## Marco A — Fundação operacional

Após Fase 3:

```text
Aplicação
+
persistência
+
segurança
+
usuários
+
configuração institucional
```

---

## Marco B — Núcleo operacional

Após Fase 7:

```text
Modelo
↓
Serviço
↓
Roteiro
↓
Execução
↓
Conclusão lógica
```

---

## Marco C — Produto operacional multiusuário

Após Fase 10:

```text
núcleo operacional
+
tempo real
+
documentos
+
dashboard
+
pesquisa
+
UX consolidada
```

---

## Marco D — Reset Service v1.0 distribuível

Após Fase 12:

```text
produto completo
+
backup
+
distribuição
+
atualização
+
recuperação
+
validação final
```

---

## 18. Fora da v1.0

Continuam excluídos:

- financeiro;
- faturamento;
- estoque;
- CRM completo;
- inventário completo;
- portal de cliente;
- WhatsApp;
- envio automático de e-mail;
- Microsoft 365/AD;
- autenticação externa;
- dependência de nuvem;
- aplicativo mobile;
- IA;
- assinatura digital;
- anexos genéricos;
- automação de implantação de sistemas operacionais;
- editor avançado de PDF;
- microsserviços;
- múltiplas instâncias do backend.

---

## 19. Relação com o backlog

O roadmap define **ordem e grandes objetivos**.

O backlog deverá transformar cada fase em itens menores com:

```text
ID
objetivo
escopo
dependências
critérios de aceite
testes esperados
prioridade
fase
```

Posteriormente:

```text
Roadmap
   ↓
Backlog
   ↓
Sprint
   ↓
Feature / tarefa
   ↓
Prompt específico
   ↓
Codex
   ↓
Implementação
   ↓
Testes
   ↓
Commit
```

---

## 20. Estado da decisão

**PLANNING-017 — Roadmap de Desenvolvimento: CONCLUÍDA E APROVADA.**

O roadmap passa a definir a ordem oficial de construção do Reset Service v1.0.
