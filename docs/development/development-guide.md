# Reset Service — Guia de Desenvolvimento e Trabalho com Codex

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Versão:** Documentation Edition  
**Status:** vigente

## 1. Objetivo

Este documento define como transformar o planejamento atual em código mantendo o projeto simples, revisável e fácil de manter.

O fluxo padrão é:

```text
Current State
    ↓
Sprint
    ↓
Backlog item
    ↓
Tarefa pequena
    ↓
Codex implementa
    ↓
Testes
    ↓
Revisão
    ↓
Próxima tarefa
```

## 2. Papéis

- ChatGPT: produto, arquitetura, decomposição, revisão e definição de próximos passos.
- Codex: implementação técnica no checkout local.
- Usuário: aprova mudanças de direção e conduz o fluxo.
- GitHub: fonte de verdade compartilhada do código e planejamento.
- `docs/planning/current-state.md`: estado operacional atual do projeto.

Codex pode decidir detalhes técnicos locais e reversíveis, mas não deve redefinir requisitos, arquitetura ou UX principal por iniciativa própria.

## 3. Leitura obrigatória antes de implementar

O arquivo raiz `AGENTS.md` é a entrada principal para agentes.

Para cada nova tarefa, Codex deve consultar pelo menos:

```text
AGENTS.md
docs/planning/current-state.md
docs/planning/backlog.md
docs/planning/sprint-plan.md
```

Quando a tarefa envolver arquitetura ou dados, consultar também:

```text
docs/architecture/architecture.md
docs/architecture/data-model.md
```

Quando envolver comportamento de produto ou UI/UX, consultar:

```text
docs/product/product-destination.md
```

## 4. Regra de transição do produto antigo

A documentação anterior continha conceitos como:

```text
Service
ServiceTemplate
TemplateRevision
Stage / Step
Command Queue
SignalR
PDF operacional
```

Esses conceitos não fazem mais parte do núcleo atual.

Se Codex encontrar referência antiga em documento ainda não revisado, deve seguir o novo `AGENTS.md`, `current-state.md`, arquitetura e backlog.

Não reintroduzir arquitetura antiga por compatibilidade com planejamento obsoleto.

## 5. Escopo de uma tarefa

Uma tarefa deve:

- possuir um objetivo principal;
- gerar diff compreensível;
- ter critérios de aceite verificáveis;
- incluir testes proporcionais;
- evitar implementação antecipada de backlog futuro.

Regra:

> Implementar tudo que a tarefa atual exige e nada que pertença conscientemente à próxima tarefa.

## 6. Decisões locais permitidas

Codex pode decidir autonomamente detalhes como:

- nomes de métodos privados;
- pequenas funções auxiliares;
- organização interna de testes;
- refatoração local necessária;
- detalhes de implementação que não alterem contratos relevantes.

## 7. Decisões que devem ser reportadas

Não alterar silenciosamente:

- stack principal;
- divisão estrutural da aplicação;
- modelo de dados relevante;
- autenticação e autorização;
- estratégia de persistência;
- requisitos de segurança;
- experiência principal de navegação;
- formato persistido do conteúdo documental;
- estratégia de instalação ou atualização;
- nova dependência estrutural.

Se uma tarefa exigir uma dessas mudanças, relatar o ponto antes de expandir o escopo.

## 8. Stack vigente

Direção técnica atual:

```text
C# / .NET 10
ASP.NET Core
Razor Pages
HTML / CSS / JavaScript
EF Core
SQLite
Windows como host
LAN como ambiente principal
```

Não adicionar por padrão:

```text
React / Angular / Vue
SPA separada
MediatR
AutoMapper
CQRS framework
Repository genérico
Unit of Work customizado
Redis
RabbitMQ
SignalR
Command Queue
microsserviços
```

Uma biblioteca nova deve resolver um problema real e justificar seu custo de manutenção.

## 9. Persistência e migrations

Mudanças persistentes seguem:

```text
modelo
↓
configuração EF Core
↓
migration
↓
teste de integração SQLite
↓
validação
```

O banco operacional fica local na máquina hospedeira e nunca é aberto diretamente pelas estações clientes.

Migrations de produção não devem ser aplicadas de maneira indiscriminada a cada startup sem decisão explícita de deployment.

## 10. Concorrência

Não existe mais requisito de serializar todas as alterações por fila.

Para edição documental concorrente, a direção é:

```text
versão carregada
+
versão atual
↓
comparação
↓
rejeitar sobrescrita silenciosa quando houver conflito real
```

Não implementar colaboração em tempo real estilo Google Docs.

## 11. Segurança

O navegador nunca é autoridade sobre identidade, permissões ou autoria.

Quando autenticação estiver implementada, o servidor deve derivar o usuário autenticado da sessão e validar entradas e permissões no backend.

Não armazenar senhas de clientes ou credenciais operacionais em documentação como prática incentivada pelo produto. Referências a cofres externos podem ser documentadas.

## 12. UI e UX

O projeto é simples tecnicamente, mas deve manter alta qualidade de uso.

Toda feature de interface deve considerar, quando aplicável:

- carregamento;
- estado vazio;
- sucesso;
- erro;
- perda de conexão;
- conflito de edição;
- confirmação apenas quando necessária;
- ação reversível para exclusões não definitivas;
- feedback de salvamento;
- teclado e foco;
- legibilidade em 1366×768 e 1920×1080.

O conteúdo técnico deve ter prioridade visual sobre a interface.

## 13. Git local

Antes de modificar:

```text
git branch --show-current
git status
```

Se houver mudanças locais não relacionadas:

- não descartar;
- não resetar;
- não sobrescrever;
- não incluir silenciosamente no commit.

Se o checkout local ainda contiver o planejamento antigo, sincronizar com a branch/base aprovada antes de implementar.

## 14. Commits

Commits devem representar mudanças coerentes.

Exemplos:

```text
feat(documents): add document entity
feat(categories): add category hierarchy
feat(documents): add document version history
fix(documents): reject stale document update
test(documents): cover document persistence
docs(planning): update current state
```

Evitar commits genéricos como `changes`, `update`, `final` ou `misc`.

## 15. Validação base

Quando aplicável, executar:

```text
dotnet restore ResetService.slnx
dotnet build ResetService.slnx -c Release
dotnet test ResetService.slnx -c Release
```

Adicionar testes específicos da tarefa.

Se um comando necessário não puder ser executado, explicar exatamente por quê no relatório final.

## 16. Relatório final do Codex

Formato esperado:

```text
Tarefa:
BL-XXX — nome

Implementado:
- ...

Arquivos principais:
- ...

Validação:
- comando → resultado

Decisões locais:
- ...

Git:
- branch
- status

Pendências/bloqueios:
- nenhum / descrição
```

## 17. Próxima fase

Após o pivô ser sincronizado no checkout local, a implementação deve seguir o backlog vigente. A primeira fase técnica é o núcleo documental, começando pela estrutura mínima necessária para `Document`, `Category`, `Tag`, `DocumentTag` e `DocumentVersion`.
