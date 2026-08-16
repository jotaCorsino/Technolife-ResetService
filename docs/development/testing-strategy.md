# Reset Service — Estratégia de Testes

**Projeto:** Reset Service  
**Versão:** Documentation Edition  
**Status:** vigente

## 1. Objetivo

A estratégia de testes deve proteger principalmente:

- integridade da documentação;
- persistência SQLite;
- histórico de versões;
- prevenção de sobrescrita silenciosa;
- autenticação e autorização quando implementadas;
- pesquisa e organização;
- lixeira e restauração;
- backup e recuperação;
- fluxos críticos de leitura e edição;
- instalação e uso pela LAN.

Não existe mais requisito de testar Command Queue, execução de serviços por etapas ou sincronização SignalR como parte do núcleo.

## 2. Princípio

Testes fazem parte da tarefa que introduz o comportamento.

```text
Implementar
↓
Testar
↓
Corrigir
↓
Validar
↓
Concluir
```

Não criar testes apenas para aumentar quantidade. Priorizar comportamento, integridade e riscos reais.

## 3. Níveis

### Unitários

Usar quando houver regra isolada relevante, por exemplo:

- validação de tipo/status;
- regras de restauração;
- geração de slug se existir regra própria;
- comportamento de domínio que não dependa do banco.

### Integração

São prioritários para:

- EF Core;
- SQLite;
- migrations;
- constraints;
- relacionamentos;
- soft delete;
- versionamento;
- concorrência otimista;
- endpoints/páginas mutáveis;
- autenticação e autorização.

Quando possível, usar SQLite real em vez de `EF Core InMemory` para validar comportamento relacional.

### End-to-end

Adicionar seletivamente para fluxos que justificam o custo:

```text
login
→ pesquisar documento
→ abrir
```

```text
criar documento
→ editar
→ salvar
→ reabrir
```

```text
mover para lixeira
→ restaurar
```

### Operacionais

Antes de releases internas relevantes, validar:

- execução em Windows;
- acesso por outro computador da LAN;
- Chrome e Edge;
- backup;
- restauração;
- atualização da aplicação.

## 4. Persistência

Testes do banco devem validar progressivamente:

- criação do schema em banco vazio;
- constraints obrigatórias;
- exclusão em cascata/restrita conforme modelo aprovado;
- consultas de documentos ativos ignorando lixeira por padrão;
- relacionamentos de categoria e tags;
- criação de versões;
- migrations futuras preservando dados representativos.

## 5. Concorrência documental

Cenário mínimo esperado quando o controle de versão for implementado:

```text
A abre Document Version 5
B abre Document Version 5

A salva
→ Version 6

B tenta salvar conteúdo baseado na Version 5
→ operação não sobrescreve silenciosamente A
→ conflito é informado
```

Não é necessário testar edição colaborativa em tempo real porque ela não faz parte do produto atual.

## 6. Histórico

Quando o histórico estiver implementado, testar:

```text
criar documento
→ editar
→ gerar nova versão
→ visualizar versão anterior
→ restaurar versão anterior
```

A restauração deve criar novo estado atual sem apagar o histórico posterior.

## 7. Lixeira

Validar:

- documento removido deixa de aparecer nas consultas normais;
- documento permanece recuperável;
- restauração recupera seus metadados e conteúdo;
- exclusão definitiva, quando existir, respeita autorização e regras definidas.

## 8. Pesquisa

Quando implementada, cobrir comportamento representativo:

- título;
- resumo;
- categoria;
- tags;
- conteúdo quando suportado;
- termos sem diferença relevante de caixa;
- comportamento definido para acentos e termos parciais.

Evitar criar um mecanismo de busca mais sofisticado apenas para satisfazer testes não exigidos pelo produto.

## 9. Segurança

Quando autenticação estiver presente, validar no backend:

- página protegida exige autenticação;
- usuário desativado não mantém acesso indevido;
- ação administrativa exige papel adequado;
- antiforgery em operações mutáveis quando aplicável;
- entrada do editor é tratada de forma segura na renderização;
- uploads respeitam tipo, tamanho e local de armazenamento aprovados.

A UI esconder um botão não conta como autorização.

## 10. Backup e restauração

Cenário mínimo futuro:

```text
Estado A
↓
backup
↓
alterações geram Estado B
↓
restore
↓
Estado A recuperado e validado
```

O teste deve considerar banco e arquivos anexados quando anexos entrarem no produto.

## 11. UI/UX

Automação não substitui inspeção visual.

Revisar manualmente telas representativas em pelo menos:

- 1366×768;
- 1920×1080;
- Chrome;
- Edge.

Estados que merecem validação explícita:

- carregamento;
- lista vazia;
- busca sem resultado;
- erro de salvamento;
- salvamento concluído;
- conflito de edição;
- item na lixeira;
- confirmação de ação irreversível.

## 12. Comandos base

Codex deve executar, quando aplicável:

```text
dotnet restore ResetService.slnx
dotnet build ResetService.slnx -c Release
dotnet test ResetService.slnx -c Release
```

Também deve executar o teste específico da tarefa quando existir.

## 13. Falhas

Não remover ou enfraquecer teste apenas porque falhou.

Primeiro classificar:

- bug real;
- teste incorreto;
- requisito alterado;
- ambiente indisponível;
- teste legado do produto antigo.

Testes legados ligados exclusivamente ao produto antigo devem ser removidos ou substituídos de forma explícita quando forem encontrados, não ignorados silenciosamente.

## 14. Critério de tarefa concluída

Uma tarefa técnica só deve ser considerada concluída quando:

- comportamento solicitado foi implementado;
- testes proporcionais foram executados;
- build relevante está verde;
- não há warnings novos evitáveis;
- mudanças não relacionadas não foram incluídas;
- resultado de validação foi relatado.
