# Reset Service — Backlog

**Versão alvo:** v1.0 Documentation Edition

Este backlog substitui o backlog do produto antigo orientado a execução de serviços.

## Prioridade P0 — Fundação

### KB-001 — Consolidar pivô do produto

- [x] redefinir README;
- [x] redefinir destino do produto;
- [x] simplificar arquitetura;
- [x] substituir modelo de dados;
- [x] atualizar current state;
- [x] atualizar roadmap;
- [x] atualizar backlog;
- [ ] atualizar sprint plan;
- [ ] revisar referências a especificações legadas.

### KB-002 — Simplificar estrutura da solução

Objetivo: reduzir a fundação para o menor número de projetos/camadas que continue organizado.

Critérios:

- avaliar incorporação de `Core` e `Infrastructure` no `ResetService.Web`;
- preservar configuração SQLite já validada;
- manter testes separados;
- build Release deve permanecer limpo.

### KB-003 — Modelo documental inicial

Implementar:

- `Document`;
- `Category`;
- `Tag`;
- `DocumentTag`;
- `DocumentVersion`.

Critérios:

- constraints e limites definidos;
- relacionamento de categoria;
- relacionamento N:N de tags;
- `Document.Version` preparado para concorrência;
- soft delete preparado.

### KB-004 — Primeira migration real

- configurar DbContext;
- criar migration do domínio documental;
- validar criação de banco novo;
- validar atualização controlada;
- habilitar foreign keys;
- preservar estratégia SQLite local.

## Prioridade P0 — Fluxo principal

### KB-005 — Listar documentações

- título;
- tipo;
- categoria;
- última atualização;
- acesso à leitura;
- estado vazio útil.

### KB-006 — Criar documentação

Campos mínimos:

- título;
- tipo;
- categoria;
- resumo opcional;
- conteúdo inicial.

### KB-007 — Ler documentação

- layout focado em leitura;
- metadados discretos;
- conteúdo central;
- ações principais visíveis.

### KB-008 — Editar documentação

- formulário/editor;
- validação;
- feedback de salvamento;
- versão enviada ao backend;
- nenhuma sobrescrita silenciosa.

### KB-009 — Lixeira básica

- mover para lixeira;
- ocultar de navegação normal;
- listar na lixeira;
- restaurar.

## Prioridade P1 — Editor e conteúdo técnico

### KB-010 — Editor rich text

Suportar inicialmente:

- headings;
- parágrafos;
- negrito/itálico;
- listas;
- listas numeradas;
- links;
- código.

### KB-011 — Blocos técnicos

- código/comando;
- checklist;
- aviso;
- observação;
- dica;
- botão copiar comando.

### KB-012 — Imagens e anexos

- upload controlado;
- armazenamento em filesystem;
- metadados no banco;
- validação de tamanho/tipo;
- inclusão no backup.

## Prioridade P1 — Organização

### KB-013 — Categorias

- CRUD administrativo;
- subcategorias;
- ordenação;
- impedir árvore excessivamente profunda quando possível.

### KB-014 — Tags

- criar/adicionar/remover;
- normalização;
- evitar duplicatas equivalentes.

### KB-015 — Tipos de documentação

- Procedimento;
- Troubleshooting;
- Configuração;
- Checklist;
- Referência;
- Livre.

### KB-016 — Pesquisa global

Pesquisar por:

- título;
- resumo;
- conteúdo;
- categoria;
- tags;
- tipo.

A busca deve ignorar diferenças simples de capitalização e, quando possível, acentuação.

### KB-017 — Filtros e ordenação

- tipo;
- categoria;
- tag;
- atualizado recentemente;
- título A–Z.

## Prioridade P1 — Proteção documental

### KB-018 — Histórico de versões

- criar versões relevantes;
- listar versões;
- visualizar versão anterior;
- registrar autor e data.

### KB-019 — Restaurar versão

Restaurar cria nova versão atual e nunca apaga histórico posterior.

### KB-020 — Concorrência otimista

- comparar `Version`;
- rejeitar estado obsoleto;
- mensagem clara de conflito;
- preservar conteúdo local na interface.

### KB-021 — Autosave

- debounce após edição;
- estados `Salvando`, `Salvo`, `Falha ao salvar`;
- não gravar a cada tecla;
- fallback manual por Ctrl+S quando aplicável.

### KB-022 — Perda de conexão

- informar indisponibilidade;
- preservar conteúdo ainda não enviado no navegador;
- permitir nova tentativa;
- evitar falsa confirmação de salvamento.

### KB-023 — Backup inicial

Backup consistente de:

```text
database.db
uploads/
```

- manual;
- validação básica;
- restauração testada.

## Prioridade P2 — Produtividade

### KB-024 — Duplicar documentação

Copiar:

- título com indicação de cópia;
- conteúdo;
- tipo;
- categoria;
- tags.

Não copiar histórico.

### KB-025 — Favoritos

Favorito por usuário.

### KB-026 — Recentes

Últimos documentos visualizados por usuário.

### KB-027 — Templates

Templates iniciais:

- Procedimento;
- Troubleshooting;
- Configuração;
- Checklist;
- documento em branco.

### KB-028 — Home orientada a conhecimento

- pesquisa em destaque;
- favoritos;
- recentes;
- categorias;
- atualizados recentemente quando útil.

### KB-029 — Busca rápida Ctrl+K

Avaliar depois da busca convencional estar madura.

## Prioridade P1 — Usuários e implantação

### KB-030 — Autenticação local

- ASP.NET Core Identity;
- login/logout;
- senha protegida;
- cookie seguro;
- usuário ativo/inativo.

### KB-031 — Perfis

```text
User
Administrator
```

Sem matriz granular no MVP.

### KB-032 — Administração mínima

- usuários;
- categorias;
- tags;
- lixeira;
- backup;
- configurações essenciais.

### KB-033 — Implantação LAN

- publicação Windows;
- endereço acessível na rede;
- Chrome/Edge;
- nenhuma instalação nas estações;
- teste com pelo menos dois computadores.

## Fora do backlog v1.0

- Service / Stage / Step;
- execução de roteiros;
- SignalR obrigatório;
- Command Queue;
- edição colaborativa em tempo real;
- CRM;
- tickets;
- inventário;
- dashboard analítico;
- aplicativo mobile;
- IA;
- API pública.
