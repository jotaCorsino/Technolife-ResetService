# Reset Service — Master Backlog v1.0

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Backlog Mestre da v1.0  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`, `docs/development/testing-strategy.md`, `docs/planning/roadmap.md`

---

## 1. Objetivo

Este documento transforma o roadmap aprovado em itens de backlog rastreáveis.

O backlog será a ponte entre:

```text
Roadmap
   ↓
Backlog
   ↓
Sprints
   ↓
Tarefas
   ↓
Prompts para Codex
   ↓
Implementação
```

Os itens deste documento ainda não representam prompts individuais de implementação.

---

## 2. Estrutura dos itens

Cada item possui:

- ID;
- fase;
- título;
- objetivo;
- dependências;
- critérios de aceite;
- prioridade.

Prioridades utilizadas:

### Crítica

Relacionada a:

- integridade;
- segurança;
- persistência;
- arquitetura;
- pré-requisito técnico;
- risco de perda de dados.

### Alta

Funcionalidade essencial da v1.0.

### Normal

Funcionalidade necessária ao produto completo, mas que não bloqueia imediatamente o núcleo anterior.

Todos os itens deste documento fazem parte da v1.0.

---

## 3. Fase 1 — Fundação da solução

## BL-001 — Estrutura inicial da solução

**Prioridade:** Crítica  
**Dependências:** nenhuma.

### Objetivo

Criar a estrutura técnica inicial aprovada.

```text
src/
├── ResetService.Web/
├── ResetService.Core/
└── ResetService.Infrastructure/

tests/
...
```

### Critérios de aceite

- solution compila em Release;
- projetos possuem referências coerentes com a arquitetura;
- Web não concentra regras de domínio;
- Core não depende da infraestrutura;
- estrutura permite crescimento controlado.

---

## BL-002 — Configuração e ambientes

**Prioridade:** Crítica  
**Dependências:** BL-001.

### Objetivo

Preparar configuração para:

- Development;
- Test;
- Production.

### Critérios de aceite

- aplicação inicia com configurações específicas do ambiente;
- segredos de produção não fazem parte do repositório;
- configuração local pode ser reproduzida;
- comportamento de produção não depende de ferramentas de desenvolvimento.

---

## BL-003 — Persistência EF Core + SQLite

**Prioridade:** Crítica  
**Dependências:** BL-001.

### Objetivo

Preparar infraestrutura de persistência.

### Escopo

- EF Core;
- SQLite;
- DbContext;
- migrations;
- criação inicial do banco;
- configuração do arquivo operacional.

### Critérios de aceite

- banco novo pode ser criado através de migration;
- aplicação consegue acessar SQLite;
- erro de banco é diagnosticável;
- migrations podem ser executadas de forma controlada.

---

## BL-004 — Infraestrutura de concorrência

**Prioridade:** Crítica  
**Dependências:** BL-003.

### Objetivo

Implementar padrão de concorrência otimista utilizando `Version`.

### Critérios de aceite

- entidade mutável pode detectar versão desatualizada;
- alteração antiga não sobrescreve silenciosamente alteração recente;
- conflito pode ser traduzido para resposta funcional compreensível.

---

## BL-005 — Command Queue

**Prioridade:** Crítica  
**Dependências:** BL-001.

### Objetivo

Criar infraestrutura central de processamento de comandos mutáveis.

### Tecnologia prevista

```text
System.Threading.Channels
```

### Critérios de aceite

- comandos são processados de forma controlada;
- cada comando possui seu próprio escopo;
- resultado só é confirmado depois da execução;
- fila pode parar de aceitar novos comandos;
- fila pode ser drenada antes de manutenção.

---

## BL-006 — Infraestrutura transversal

**Prioridade:** Alta  
**Dependências:** BL-001.

### Objetivo

Criar recursos compartilhados necessários ao restante da aplicação.

### Escopo

- logging;
- tratamento global de erros;
- health check;
- SignalR básico.

### Critérios de aceite

- erro inesperado é registrado;
- usuário não recebe stack trace;
- health check responde corretamente;
- infraestrutura SignalR pode aceitar conexão quando autenticação estiver disponível.

---

## 4. Fase 2 — Identidade e segurança

## BL-007 — ASP.NET Core Identity

**Prioridade:** Crítica  
**Dependências:** BL-003.

### Objetivo

Integrar ASP.NET Core Identity com o modelo da aplicação.

### Critérios de aceite

- `ApplicationUser` persistido;
- roles `Administrator` e `Technician` disponíveis;
- senhas armazenadas de forma protegida;
- não existe cadastro público.

---

## BL-008 — Bootstrap do primeiro Administrador

**Prioridade:** Crítica  
**Dependências:** BL-007.

### Objetivo

Permitir a inicialização segura da primeira instalação.

### Critérios de aceite

- primeiro Administrador pode ser criado localmente;
- fluxo não fica exposto normalmente à LAN;
- após criação do primeiro Admin, bootstrap normal deixa de ser acessível.

---

## BL-009 — Login, logout e sessão

**Prioridade:** Crítica  
**Dependências:** BL-007.

### Objetivo

Implementar autenticação por cookie.

### Critérios de aceite

- login funciona;
- logout invalida sessão;
- acesso anônimo ao negócio é rejeitado;
- cookie usa configurações aprovadas;
- sessão expira conforme política.

---

## BL-010 — Segurança de credenciais

**Prioridade:** Crítica  
**Dependências:** BL-007.

### Objetivo

Implementar políticas de credencial.

### Critérios de aceite

- mínimo de 8 caracteres;
- regras artificiais de composição não são exigidas;
- cinco falhas consecutivas provocam lockout;
- credencial temporária exige alteração;
- Administrador não consegue consultar senha atual.

---

## BL-011 — Policies e autorização

**Prioridade:** Crítica  
**Dependências:** BL-007.

### Objetivo

Centralizar regras técnicas de autorização.

### Critérios de aceite

- ações administrativas possuem policies apropriadas;
- frontend não é a única barreira;
- requisição manual sem permissão é rejeitada;
- Técnico e Administrador recebem somente suas capacidades aprovadas.

---

## BL-012 — Proteções web

**Prioridade:** Crítica  
**Dependências:** BL-009.

### Objetivo

Aplicar proteções HTTP e de sessão.

### Escopo

- antiforgery;
- rate limiting do login;
- Data Protection;
- security headers;
- segurança inicial do SignalR.

### Critérios de aceite

- operação mutável sem antiforgery válido é rejeitada quando aplicável;
- login possui proteção contra rajadas;
- tokens e segredos não aparecem nos logs;
- configuração permanece compatível com HTTPS.

---

## 5. Fase 3 — Administração e configuração

## BL-013 — Perfil institucional da Technolife

**Prioridade:** Alta  
**Dependências:** BL-009.

### Objetivo

Implementar `CompanySettings`.

### Critérios de aceite

Administrador pode manter:

- nome;
- razão social;
- CNPJ;
- telefone;
- e-mail;
- site;
- endereço.

---

## BL-014 — Gestão da logo institucional

**Prioridade:** Alta  
**Dependências:** BL-013.

### Objetivo

Implementar upload seguro da identidade visual.

### Critérios de aceite

- PNG/JPEG suportados;
- limite de tamanho aplicado;
- conteúdo validado;
- nome físico controlado pela aplicação;
- substituição não destrói asset histórico necessário.

---

## BL-015 — Configurações documentais

**Prioridade:** Alta  
**Dependências:** BL-013.

### Objetivo

Separar configuração documental da identidade institucional.

### Critérios de aceite

Admin consegue configurar:

- logo no documento;
- dados institucionais exibidos;
- cabeçalho/rodapé previsto;
- texto padrão de conclusão.

---

## BL-016 — Gestão de usuários

**Prioridade:** Alta  
**Dependências:** BL-007, BL-011.

### Objetivo

Implementar administração completa dos usuários locais.

### Critérios de aceite

Admin pode:

- criar;
- editar;
- redefinir senha;
- alterar perfil;
- desativar;
- reativar.

Também:

- usuário inativo não autentica;
- último Administrador ativo não pode ser removido funcionalmente.

---

## BL-017 — Conta do usuário

**Prioridade:** Alta  
**Dependências:** BL-009.

### Objetivo

Implementar funcionalidades pessoais da conta autenticada.

### Critérios de aceite

- nome do usuário aparece na interface;
- perfil é visível;
- usuário pode alterar a própria senha;
- logout é explicitamente acessível.

---

## BL-018 — Configurações de sistema

**Prioridade:** Normal  
**Dependências:** BL-013.

### Objetivo

Criar infraestrutura para configurações tipadas do sistema.

### Critérios de aceite

- configurações são persistidas;
- somente Administrador altera;
- não é criada estrutura key/value genérica indiscriminada.

---

## 6. Fase 4 — Modelos e revisões

## BL-019 — Modelos Draft

**Prioridade:** Alta  
**Dependências:** BL-011.

### Objetivo

Criar e administrar modelos ainda não publicados.

### Critérios de aceite

- Admin cria modelo;
- título obrigatório;
- descrição administrativa opcional;
- modelo nunca publicado pode ser excluído quando permitido.

---

## BL-020 — Editor de etapas

**Prioridade:** Alta  
**Dependências:** BL-019.

### Objetivo

Implementar estrutura das etapas.

### Critérios de aceite

- adicionar;
- editar;
- remover;
- ordenar;
- persistir posição;
- validar título.

---

## BL-021 — Editor de passos

**Prioridade:** Alta  
**Dependências:** BL-020.

### Objetivo

Implementar passos dentro das etapas.

### Critérios de aceite

- adicionar;
- editar;
- remover;
- ordenar;
- preservar instruções opcionais;
- impedir publicação de estrutura inválida.

---

## BL-022 — Publicação e revisões

**Prioridade:** Crítica  
**Dependências:** BL-020, BL-021.

### Objetivo

Publicar um modelo válido.

### Critérios de aceite

- validação completa antes da publicação;
- número automático da revisão;
- RevisionNumber único;
- revisão publicada imutável;
- modelo passa para Active.

---

## BL-023 — Próxima revisão em Draft

**Prioridade:** Alta  
**Dependências:** BL-022.

### Objetivo

Permitir evolução de modelo publicado sem alterar a versão vigente.

### Critérios de aceite

- no máximo um Draft por modelo;
- revisão publicada continua sendo utilizada pelos serviços novos;
- publicação do Draft cria próxima revisão.

---

## BL-024 — Ciclo administrativo dos modelos

**Prioridade:** Alta  
**Dependências:** BL-022.

### Objetivo

Completar o ciclo de vida dos modelos.

### Escopo

- duplicar;
- arquivar;
- reativar;
- descartar Draft;
- consultar revisões.

### Critérios de aceite

- revisão publicada nunca é removida;
- duplicação cria modelo independente;
- primeira publicação da cópia é Revision 1;
- arquivados deixam de ser oferecidos para novos serviços.

---

## 7. Fase 5 — Criação de serviços

## BL-025 — Sequência RS-AAAA-NNNNN

**Prioridade:** Crítica  
**Dependências:** BL-003, BL-005.

### Objetivo

Gerar números únicos de serviço.

### Critérios de aceite

- geração transacional;
- sequência anual;
- nenhuma colisão concorrente;
- número cancelado nunca reutilizado.

---

## BL-026 — Novo Serviço

**Prioridade:** Alta  
**Dependências:** BL-022, BL-025.

### Objetivo

Criar serviço a partir de modelo ativo.

### Critérios de aceite

- modelo é o único dado manual obrigatório;
- serviço recebe ID imediatamente;
- título é inicializado;
- status começa Draft.

---

## BL-027 — Cópia independente do roteiro

**Prioridade:** Crítica  
**Dependências:** BL-026.

### Objetivo

Criar roteiro operacional próprio do serviço.

### Critérios de aceite

- etapas/passos são copiados;
- alteração futura do modelo não altera serviço existente;
- IDs de origem são preservados quando aplicáveis.

---

## BL-028 — Dados de identificação

**Prioridade:** Alta  
**Dependências:** BL-026.

### Objetivo

Implementar dados de cliente, equipamento e operação.

### Critérios de aceite

- campos opcionais não bloqueiam criação;
- título e responsável são editáveis conforme estado;
- campos seguem `service-data-spec.md`.

---

## BL-029 — Origem histórica

**Prioridade:** Alta  
**Dependências:** BL-027.

### Objetivo

Preservar origem do serviço.

### Critérios de aceite

São preservados:

- modelo;
- nome histórico do modelo;
- revisão;
- criador;
- criação.

Renomear o modelo não altera serviços anteriores.

---

## BL-030 — Listagem inicial de serviços

**Prioridade:** Alta  
**Dependências:** BL-026.

### Objetivo

Criar entrada operacional da área Serviços.

### Critérios de aceite

- serviços aparecem após criação;
- status é visível;
- serviço pode ser aberto;
- busca inicial por ID/título funciona.

---

## 8. Fase 6 — Execução

## BL-031 — Tela operacional do serviço

**Prioridade:** Alta  
**Dependências:** BL-027.

### Objetivo

Criar a estrutura principal da execução.

### Critérios de aceite

Cabeçalho apresenta contexto como:

- ID;
- título;
- status;
- responsável;
- progresso.

Tabs principais estão disponíveis conforme UX aprovada.

---

## BL-032 — Navegação por etapas

**Prioridade:** Alta  
**Dependências:** BL-031.

### Objetivo

Implementar metáfora de páginas/etapas.

### Critérios de aceite

- navegação livre;
- próxima/anterior;
- progresso segmentado;
- nenhuma conclusão automática ao mudar de etapa.

---

## BL-033 — Estados dos passos

**Prioridade:** Crítica  
**Dependências:** BL-031, BL-005.

### Objetivo

Implementar:

```text
Pending
Completed
NotApplicable
```

### Critérios de aceite

- estados persistidos;
- respeitam status do serviço;
- podem ser alterados enquanto permitido;
- passam pela fila de comandos.

---

## BL-034 — Progresso e estado calculado

**Prioridade:** Crítica  
**Dependências:** BL-033.

### Objetivo

Calcular automaticamente o andamento.

### Critérios de aceite

- N/A excluído do denominador;
- estado da etapa calculado;
- progresso geral usa todos os passos aplicáveis;
- etapa completamente N/A é apresentada corretamente.

---

## BL-035 — Início do serviço

**Prioridade:** Alta  
**Dependências:** BL-031.

### Objetivo

Implementar Draft → InProgress.

### Critérios de aceite

- checklist bloqueado em Draft;
- início registra ator/data;
- evento criado;
- execução é habilitada.

---

## BL-036 — Espera e retomada

**Prioridade:** Alta  
**Dependências:** BL-035.

### Objetivo

Implementar Waiting e Resume.

### Critérios de aceite

- motivo obrigatório;
- execução bloqueada em Waiting;
- dados cadastrais permitidos continuam editáveis;
- retomada registra evento.

---

## BL-037 — Cancelamento

**Prioridade:** Alta  
**Dependências:** BL-035.

### Objetivo

Cancelar serviços com preservação histórica.

### Critérios de aceite

- motivo obrigatório;
- status persistido;
- evento registrado;
- registro não é apagado.

---

## BL-038 — Revisão e conclusão

**Prioridade:** Crítica  
**Dependências:** BL-033, BL-034.

### Objetivo

Concluir corretamente um serviço.

### Critérios de aceite

- zero Pending obrigatório;
- revisão final disponível;
- backend rejeita conclusão inválida;
- conclusão registra ator/data.

---

## 9. Fase 7 — Observações, personalização e histórico

## BL-039 — Observações do serviço

**Prioridade:** Alta  
**Dependências:** BL-031.

### Objetivo

Adicionar notas cronológicas gerais.

### Critérios de aceite

- texto;
- autor;
- data;
- visibilidade;
- ordenação cronológica.

---

## BL-040 — Observações de etapa e passo

**Prioridade:** Alta  
**Dependências:** BL-039.

### Objetivo

Adicionar observações contextuais.

### Critérios de aceite

- associação correta;
- Internal/Client;
- Client exige Information ou Recommendation.

---

## BL-041 — Edição e remoção de observações

**Prioridade:** Alta  
**Dependências:** BL-039.

### Objetivo

Aplicar regras de alteração das notas.

### Critérios de aceite

- serviço aberto permite edição conforme regras;
- serviço protegido bloqueia alterações;
- remoção utiliza rastreabilidade adequada.

---

## BL-042 — Personalização estrutural do roteiro

**Prioridade:** Alta  
**Dependências:** BL-031.

### Objetivo

Permitir alteração do roteiro copiado.

### Escopo

- etapas;
- passos;
- títulos;
- instruções;
- posição;
- adição;
- remoção.

### Critérios de aceite

- alteração afeta somente o serviço;
- regras de confirmação respeitadas;
- modelo original permanece intacto.

---

## BL-043 — IsRouteCustomized

**Prioridade:** Alta  
**Dependências:** BL-042.

### Objetivo

Registrar que o roteiro foi estruturalmente personalizado.

### Critérios de aceite

- inicia false;
- primeira mudança estrutural torna true;
- não volta automaticamente para false.

---

## BL-044 — Histórico funcional

**Prioridade:** Alta  
**Dependências:** BL-035, BL-036, BL-037.

### Objetivo

Implementar timeline relevante.

### Critérios de aceite

Eventos importantes exibem:

- tipo;
- data;
- ator;
- motivo quando aplicável.

Não registrar indiscriminadamente todo checkbox.

---

## 10. Fase 8 — Multiusuário e tempo real

## BL-045 — Grupos SignalR por serviço

**Prioridade:** Alta  
**Dependências:** BL-006, BL-031.

### Objetivo

Direcionar eventos somente aos clientes interessados.

### Critérios de aceite

- conexões entram/saem do grupo correto;
- grupos não substituem autorização;
- serviço A não recebe eventos desnecessários do B.

---

## BL-046 — Sincronização dos passos

**Prioridade:** Crítica  
**Dependências:** BL-033, BL-045.

### Objetivo

Atualizar checklists automaticamente entre usuários.

### Critérios de aceite

- alteração confirmada chega aos outros clientes;
- nenhuma atualização antes do COMMIT;
- não exige F5.

---

## BL-047 — Sincronização das informações do serviço

**Prioridade:** Alta  
**Dependências:** BL-045.

### Objetivo

Propagar alterações relevantes adicionais.

### Escopo

- responsável;
- status;
- observações;
- detalhes;
- roteiro.

### Critérios de aceite

Clientes interessados recebem automaticamente estado atualizado.

---

## BL-048 — Conflitos de versão

**Prioridade:** Crítica  
**Dependências:** BL-004, BL-005.

### Objetivo

Impedir sobrescrita silenciosa.

### Critérios de aceite

- versão desatualizada detectada;
- operação incompatível rejeitada;
- cliente recebe estado atual e mensagem funcional.

---

## BL-049 — Reconexão e resincronização

**Prioridade:** Crítica  
**Dependências:** BL-045.

### Objetivo

Recuperar corretamente perda de conexão SignalR.

### Critérios de aceite

Após reconexão:

- autenticação revalidada;
- grupo reassociado;
- estado atual recarregado;
- cliente não presume ter recebido eventos perdidos.

---

## BL-050 — Idempotência e carga concorrente

**Prioridade:** Crítica  
**Dependências:** BL-005, BL-048.

### Objetivo

Proteger operações críticas e comprovar comportamento multiusuário.

### Critérios de aceite

- retries não duplicam efeitos indevidos;
- criação concorrente não duplica IDs;
- operações confirmadas não são perdidas;
- testes simultâneos aprovados.

---

## 11. Fase 9 — Conclusões e documentos

## BL-051 — ServiceConclusion

**Prioridade:** Crítica  
**Dependências:** BL-038.

### Objetivo

Criar registros históricos de conclusão.

### Critérios de aceite

- c01, c02 etc.;
- unicidade por serviço;
- conclusão anterior preservada após reabertura.

---

## BL-052 — Snapshot histórico

**Prioridade:** Crítica  
**Dependências:** BL-051.

### Objetivo

Criar snapshot JSON imutável e versionado.

### Critérios de aceite

- contém dados necessários à documentação;
- possui schema version;
- não muda após criação;
- pode possuir hash de integridade.

---

## BL-053 — Registro Interno de Serviço

**Prioridade:** Alta  
**Dependências:** BL-052, BL-015.

### Objetivo

Gerar documento técnico da Technolife.

### Critérios de aceite

Pode representar corretamente:

- serviço;
- cliente;
- equipamento;
- roteiro;
- N/A;
- notas internas;
- notas de cliente;
- conclusão;
- informações institucionais.

---

## BL-054 — Relatório de Serviço

**Prioridade:** Crítica  
**Dependências:** BL-052.

### Objetivo

Gerar documento externo seguro.

### Critérios de aceite

- Internal jamais entra no dataset;
- conteúdo Client aparece corretamente;
- N/A tratado conforme especificação;
- documento deriva do snapshot.

---

## BL-055 — Prévia e regeneração

**Prioridade:** Alta  
**Dependências:** BL-053, BL-054.

### Objetivo

Permitir revisar e regenerar documentos.

### Critérios de aceite

- prévia respeita mesmas regras do PDF;
- c01 sempre utiliza snapshot c01;
- regeneração não cria nova conclusão.

---

## BL-056 — Documentos por ciclo

**Prioridade:** Alta  
**Dependências:** BL-055.

### Objetivo

Organizar documentos históricos.

### Critérios de aceite

- conclusões separadas;
- nomes de arquivo consistentes;
- cancelado pode gerar Registro Interno quando previsto.

---

## 12. Fase 10 — Dashboard, pesquisa e UX

## BL-057 — Dashboard operacional

**Prioridade:** Alta  
**Dependências:** BL-030, BL-034.

### Objetivo

Criar visão inicial operacional.

### Critérios de aceite

Exibe adequadamente:

- Em andamento;
- Aguardando;
- concluídos recentes;
- progresso;
- etapa;
- responsável;
- acesso rápido.

---

## BL-058 — Busca textual

**Prioridade:** Alta  
**Dependências:** BL-030.

### Objetivo

Pesquisar serviços pelos campos aprovados.

### Critérios de aceite

Busca considera quando aplicável:

- ID;
- título;
- cliente;
- empresa;
- referência;
- equipamento;
- serial;
- patrimônio;
- hostname.

---

## BL-059 — Filtros

**Prioridade:** Alta  
**Dependências:** BL-058.

### Objetivo

Adicionar filtros estruturados.

### Critérios de aceite

Filtros incluem:

- status;
- responsável;
- modelo;
- período.

Combinações funcionam de forma previsível.

---

## BL-060 — Histórico de serviços

**Prioridade:** Alta  
**Dependências:** BL-056.

### Objetivo

Consultar serviços históricos.

### Critérios de aceite

Serviço concluído/cancelado permite consultar:

- detalhes;
- roteiro;
- histórico;
- documentos.

---

## BL-061 — Refinamento visual e estados da UI

**Prioridade:** Normal  
**Dependências:** funcionalidades anteriores.

### Objetivo

Padronizar comportamento visual.

### Critérios de aceite

Telas importantes possuem estados adequados de:

- carregamento;
- sucesso;
- erro;
- vazio;
- confirmação.

---

## BL-062 — Compatibilidade e acessibilidade

**Prioridade:** Normal  
**Dependências:** BL-061.

### Objetivo

Validar requisitos da experiência.

### Critérios de aceite

- 1366×768 utilizável;
- teclado funciona nos fluxos principais;
- foco visível;
- labels adequados;
- status não depende somente de cor;
- Chrome/Edge oficiais aprovados.

---

## 13. Fase 11 — Backup e recuperação

## BL-063 — Configuração de backup

**Prioridade:** Alta  
**Dependências:** BL-018.

### Objetivo

Permitir configurar a rotina automática opcional.

### Critérios de aceite

Admin controla:

- ativado/desativado;
- horário;
- destino;
- retenção.

Desativar não impede o produto de funcionar.

---

## BL-064 — Backup manual

**Prioridade:** Alta  
**Dependências:** BL-003.

### Objetivo

Criar backup consistente sob demanda.

### Critérios de aceite

- Admin cria backup;
- estado é consistente;
- validação executada;
- SQLite não precisa ser manipulado manualmente.

---

## BL-065 — Backup automático

**Prioridade:** Alta  
**Dependências:** BL-063, BL-064.

### Objetivo

Executar backup periódico quando habilitado.

### Critérios de aceite

- execução diária;
- horário configurável;
- nenhuma execução quando desabilitado;
- falha registrada e informada ao Admin.

---

## BL-066 — Manifesto, catálogo e retenção

**Prioridade:** Alta  
**Dependências:** BL-064.

### Objetivo

Gerenciar metadados e histórico dos backups.

### Critérios de aceite

Cada backup possui:

- tipo;
- data;
- versão;
- tamanho;
- resultado;
- validação.

Backups manuais e especiais não são apagados pela retenção automática normal.

---

## BL-067 — Importação e exportação

**Prioridade:** Alta  
**Dependências:** BL-066.

### Objetivo

Permitir transporte controlado dos backups.

### Critérios de aceite

- backup pode ser exportado;
- pacote pode ser importado;
- importação valida;
- importação não restaura imediatamente.

---

## BL-068 — Restauração completa

**Prioridade:** Crítica  
**Dependências:** BL-064, BL-067.

### Objetivo

Restaurar integralmente o estado.

### Critérios de aceite

- somente Admin;
- backup validado antes;
- confirmação forte;
- pré-restauração quando possível;
- maintenance mode;
- nenhuma mesclagem parcial;
- sessões antigas invalidadas.

---

## 14. Fase 12 — Implantação e release

## BL-069 — Publicação Windows self-contained

**Prioridade:** Crítica.

### Objetivo

Gerar artefato de produção para Windows x64.

### Critérios de aceite

- Release;
- self-contained;
- executa em máquina compatível sem instalação manual prévia do runtime.

---

## BL-070 — Instalação da máquina hospedeira

**Prioridade:** Crítica  
**Dependências:** BL-069.

### Objetivo

Preparar desktop, notebook ou Windows Server compatível.

### Critérios de aceite

Instalação configura:

- aplicação;
- ProgramData;
- ACLs;
- serviço;
- diretórios necessários.

---

## BL-071 — Rede, DNS, HTTPS e firewall

**Prioridade:** Crítica  
**Dependências:** BL-070.

### Objetivo

Disponibilizar o produto de maneira simples na LAN.

### Critérios de aceite

- acesso por navegador;
- URL amigável quando configurada;
- HTTPS;
- certificado confiável;
- firewall restrito ao necessário;
- cliente não precisa conhecer porta/banco.

---

## BL-072 — Startup e recuperação do serviço

**Prioridade:** Alta  
**Dependências:** BL-070.

### Objetivo

Garantir inicialização previsível.

### Critérios de aceite

- inicia automaticamente após reboot;
- não depende de login do Windows;
- recuperação controlada após falhas transitórias.

---

## BL-073 — Processo de atualização

**Prioridade:** Crítica  
**Dependências:** BL-069.

### Objetivo

Atualizar centralmente o produto.

### Critérios de aceite

Processo inclui:

- validação;
- manutenção;
- bloqueio de novos comandos;
- drenagem;
- parada;
- instalação;
- migration quando aplicável;
- health check.

---

## BL-074 — Migration e rollback

**Prioridade:** Crítica  
**Dependências:** BL-073.

### Objetivo

Garantir evolução segura da persistência.

### Critérios de aceite

- migrations são controladas;
- migration bundle quando adotado;
- binários anteriores podem ser preservados;
- downgrade automático de banco não ocorre;
- procedimento de recuperação é conhecido.

---

## BL-075 — Desinstalação e reinstalação seguras

**Prioridade:** Alta  
**Dependências:** BL-070.

### Objetivo

Evitar perda acidental durante manutenção do produto.

### Critérios de aceite

- desinstalação preserva dados por padrão;
- reinstalação detecta dados existentes;
- nenhuma sobrescrita silenciosa.

---

## BL-076 — Validação Windows 10/11 e navegadores

**Prioridade:** Crítica  
**Dependências:** BL-071.

### Objetivo

Confirmar matriz oficial de compatibilidade.

### Critérios de aceite

- Windows 10 alvo testado;
- Windows 11 testado;
- Chrome testado;
- Edge testado;
- ambientes legados classificados como melhor esforço.

---

## BL-077 — Testes finais de escala e concorrência

**Prioridade:** Crítica  
**Dependências:** BL-050, BL-062.

### Objetivo

Validar arquitetura sob carga representativa.

### Critérios de aceite

- múltiplos clientes simultâneos;
- nenhum comando confirmado perdido;
- nenhuma sobrescrita silenciosa;
- desempenho aceitável;
- SQLite formalmente aprovado ou reavaliado com base nos resultados.

---

## BL-078 — Preparação final da v1.0

**Prioridade:** Crítica  
**Dependências:** backlog v1.0 completo.

### Objetivo

Executar fechamento da release.

### Critérios de aceite

- nenhum bug bloqueador conhecido;
- segurança revisada;
- documentos revisados;
- instalação aprovada;
- backup/restauração aprovados;
- build final reproduzível;
- documentação consistente com o produto entregue.

---

## 15. Resumo

| Fase | Backlog | Quantidade |
|---|---|---:|
| Fundação | BL-001–006 | 6 |
| Segurança | BL-007–012 | 6 |
| Administração | BL-013–018 | 6 |
| Modelos | BL-019–024 | 6 |
| Criação de serviços | BL-025–030 | 6 |
| Execução | BL-031–038 | 8 |
| Observações/histórico | BL-039–044 | 6 |
| Multiusuário | BL-045–050 | 6 |
| Documentos | BL-051–056 | 6 |
| Dashboard/UX | BL-057–062 | 6 |
| Backup | BL-063–068 | 6 |
| Implantação/release | BL-069–078 | 10 |
| **Total** |  | **78** |

---

## 16. Dependência macro

```text
Fundação
   ↓
Segurança
   ↓
Administração
   ↓
Modelos
   ↓
Serviços
   ↓
Execução
   ↓
Observações e histórico
   ↓
Multiusuário
   ↓
Documentos
   ↓
Dashboard e UX
   ↓
Backup
   ↓
Implantação
   ↓
v1.0
```

Dependências individuais prevalecem sobre a ordem puramente numérica.

---

## 17. Relação com tarefas técnicas

Um item de backlog poderá futuramente ser dividido em tarefas menores.

Exemplo:

```text
BL-025 — Sequência de serviço
│
├── criar entidade de sequência
├── configurar EF Core
├── criar migration
├── implementar geração
├── implementar transação
├── adicionar constraints
├── testar concorrência
└── integrar ao Novo Serviço
```

Essas tarefas pertencem à implementação, não representam novos requisitos de produto.

---

## 18. Relação com Codex

Prompts futuros deverão ser pequenos e focados.

Não utilizar:

```text
Implemente toda a Fase 1.
```

Preferir:

```text
Implemente exclusivamente a tarefa
definida para BL-001.

Não avance para autenticação,
persistência de negócio ou UI funcional.
```

O fluxo de implementação será:

```text
Tarefa
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
   ↓
Push
   ↓
Verificação
   ↓
Próxima tarefa
```

---

## 19. Estado da decisão

**PLANNING-018 — Backlog Mestre da v1.0: CONCLUÍDA E APROVADA.**

O backlog oficial da v1.0 contém **78 itens rastreáveis, BL-001 a BL-078**.