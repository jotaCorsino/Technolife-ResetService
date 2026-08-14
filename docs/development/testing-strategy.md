# Reset Service — Testing Strategy and Quality Criteria

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Estratégia de Testes e Critérios de Qualidade  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/*`

---

## 1. Objetivo

Este documento define como o Reset Service será validado durante desenvolvimento, releases e implantação.

A estratégia deverá garantir principalmente:

- correção das regras de negócio;
- integridade dos dados;
- segurança;
- concorrência;
- uso multiusuário;
- atualização em tempo real;
- persistência SQLite;
- geração documental;
- backup e restauração;
- migrations;
- instalação e atualização;
- compatibilidade dos ambientes suportados.

---

## 2. Princípio

Testes fazem parte da implementação de cada funcionalidade.

Não serão tratados somente como uma etapa final do projeto.

A regra será:

```text
Implementar
   ↓
Validar
   ↓
Testar
   ↓
Corrigir
   ↓
Considerar concluído
```

---

## 3. Níveis de teste

Serão utilizados quatro níveis principais.

| Nível | Finalidade |
|---|---|
| Unitário | Regras de negócio isoladas |
| Integração | Banco, backend, autenticação, fila e infraestrutura |
| End-to-end | Fluxos completos pelo navegador |
| Operacional | Instalação, Windows, rede, backup e atualização |

Cada nível será utilizado somente onde trouxer valor real.

---

## 4. Estrutura de testes

Estrutura inicial possível:

```text
tests/
├── ResetService.UnitTests/
├── ResetService.IntegrationTests/
└── ResetService.EndToEndTests/
```

Os projetos serão criados quando houver código correspondente a testar.

Não será criada estrutura vazia apenas por antecipação.

---

## 5. Framework de testes

A direção inicial será utilizar:

```text
xUnit
```

para testes automatizados .NET.

Outras ferramentas somente serão adicionadas quando houver necessidade concreta.

---

## 6. Testes unitários

Os testes unitários terão foco principal em regras do `ResetService.Core`.

Áreas prioritárias:

- workflow;
- estados;
- progressos;
- conclusão;
- espera;
- cancelamento;
- reabertura;
- modelos;
- revisões;
- personalização;
- permissões funcionais;
- snapshots;
- regras documentais.

---

## 7. Exemplo de regra

```text
Service:
InProgress

Steps:
Completed
NotApplicable
Pending

Action:
CompleteService

Expected:
Rejected
```

Outro exemplo:

```text
Steps:
Completed
NotApplicable
Completed

Applicable = 2
Completed = 2

Progress = 100%
```

---

## 8. Testes de pouco valor

Não será objetivo testar indiscriminadamente:

- getters triviais;
- setters simples;
- propriedades sem comportamento;
- código gerado;
- detalhes internos sem importância funcional.

O foco será comportamento.

---

## 9. Persistência real

Como o banco de produção será SQLite, testes de persistência deverão utilizar SQLite real.

Não será utilizado `EF Core InMemory` como substituto principal para comportamento relacional.

---

## 10. SQLite em memória

Para testes rápidos e isolados poderá ser utilizado:

```text
SQLite in-memory
```

mantendo a conexão aberta pelo ciclo de vida necessário do teste.

---

## 11. SQLite em arquivo

Cenários que dependem de características reais de arquivo deverão utilizar banco temporário físico.

Exemplos:

- WAL;
- backup;
- restauração;
- concorrência;
- locking;
- migrations;
- filesystem;
- recuperação.

---

## 12. Constraints

Testes deverão confirmar restrições importantes.

Exemplos:

```text
ServiceNumber UNIQUE

(ServiceYear, SequenceNumber) UNIQUE

(TemplateId, RevisionNumber) UNIQUE

(ServiceId, ConclusionNumber) UNIQUE
```

As regras não dependerão apenas do código da aplicação.

---

## 13. Integração do backend

Testes de integração deverão executar o pipeline real do ASP.NET Core quando apropriado.

Direção:

```text
WebApplicationFactory<Program>
```

Fluxos poderão validar:

```text
HTTP
 ↓
Authentication
 ↓
Antiforgery
 ↓
Authorization
 ↓
Command Queue
 ↓
Domain
 ↓
EF Core
 ↓
SQLite
```

---

## 14. Autorização

Testes deverão comprovar que permissões são realmente aplicadas pelo backend.

Exemplo:

```text
Technician
→ request direto para ReopenService

Expected:
Rejected
```

Mesmo que a UI esconda o botão.

---

## 15. Concorrência

Concorrência será uma área obrigatória de testes.

Precisaremos comprovar que operações simultâneas não causam:

- sobrescrita silenciosa;
- corrupção;
- duplicação;
- estado parcial;
- perda de comando confirmado.

---

## 16. Token de versão

Cenário obrigatório:

```text
A lê Version 20
B lê Version 20

A altera
→ Version 21

B tenta salvar
baseado em Version 20

Expected:
conflito detectado
```

A alteração de A não poderá ser silenciosamente substituída.

---

## 17. Sequência concorrente

Cenário obrigatório:

```text
10 requisições simultâneas
→ criar serviço
```

Resultado esperado:

```text
10 serviços
10 ServiceNumbers distintos
nenhuma colisão
nenhum registro parcial
```

---

## 18. Fila de comandos

Testes deverão validar:

- ordenação;
- processamento;
- falha;
- cancelamento administrativo;
- rejeição de estado inválido;
- drenagem antes de manutenção.

Um comando não será considerado salvo apenas porque entrou na fila.

---

## 19. Confirmação

Resultado de sucesso somente ocorrerá depois de:

```text
COMMIT
```

Testes deverão garantir essa propriedade.

---

## 20. Idempotência

Operações críticas deverão ser testadas contra repetição acidental.

Exemplos:

- criar serviço;
- concluir;
- cancelar;
- publicar revisão;
- restaurar.

Quando houver `OperationId`, o comportamento correspondente deverá ser validado.

---

## 21. Multiusuário

A suíte deverá possuir cenários onde dois ou mais usuários trabalham simultaneamente.

Exemplo:

```text
Usuário A
→ altera Passo 5

Usuário B
→ altera Passo 8

Expected:
ambas alterações preservadas
```

---

## 22. SignalR

Alterações confirmadas deverão chegar aos demais clientes interessados.

Cenário:

```text
Browser A
→ marca passo

COMMIT
→ SignalR

Browser B
→ interface atualiza sem F5
```

---

## 23. Reconexão SignalR

Cenário:

```text
Browser B desconecta
       ↓
A realiza alterações
       ↓
B reconecta
       ↓
B resincroniza dados
```

O requisito é recuperar o estado verdadeiro do servidor.

Não será necessário reproduzir todos os eventos perdidos.

---

## 24. End-to-end

Fluxos críticos no navegador poderão utilizar:

```text
Playwright .NET
```

Os testes end-to-end serão seletivos.

Não será objetivo automatizar cada variação visual existente.

---

## 25. Fluxos E2E prioritários

Exemplo operacional:

```text
Login
→ Novo serviço
→ Escolher modelo
→ Criar
→ Iniciar
→ Executar passos
→ Adicionar observação
→ Concluir
→ Visualizar documento
```

---

## 26. Fluxo administrativo

```text
Login Administrador
→ Criar modelo
→ Construir roteiro
→ Publicar
→ Criar serviço
→ Confirmar revisão utilizada
```

---

## 27. E2E multiusuário

Deverá existir pelo menos um cenário automatizado ou controlado com dois contextos independentes de navegador.

Objetivo:

```text
Browser A
+
Browser B
+
mesmo serviço
```

e comprovar atualização em tempo real e ausência de perda silenciosa.

---

## 28. Segurança

Os requisitos de segurança terão cobertura específica.

Áreas prioritárias:

- autenticação obrigatória;
- roles;
- policies;
- lockout;
- credencial temporária;
- desativação;
- antiforgery;
- acesso direto a endpoints;
- arquivos privados;
- uploads;
- separação interno/cliente.

---

## 29. Lockout

Cenário:

```text
5 credenciais inválidas consecutivas
        ↓
conta temporariamente bloqueada
```

Depois do período previsto, a autenticação deverá voltar a ser possível.

---

## 30. Antiforgery

Operação mutável autenticada sem token válido deverá ser rejeitada quando antiforgery for exigido.

---

## 31. Relatório do cliente

Será obrigatório testar que conteúdo Interno nunca seja enviado ao relatório externo.

Exemplo:

```text
Observation:
Internal

Text:
"informação interna"

Generate:
Client Report

Expected:
texto ausente
```

A validação deverá ocorrer também no conjunto de dados fornecido ao gerador, não apenas na aparência final.

---

## 32. PDFs

Testes automáticos deverão verificar:

- geração bem-sucedida;
- documento correspondente ao tipo correto;
- snapshot correto;
- conclusão correta;
- conteúdo esperado;
- exclusão de conteúdo proibido;
- regeneração histórica.

---

## 33. Revisão visual de PDFs

Também haverá inspeção visual em casos representativos.

Exemplos:

- documento curto;
- multipágina;
- texto longo;
- com logo;
- sem logo;
- campos opcionais ausentes;
- muitas etapas;
- observações extensas.

Não será utilizada comparação pixel a pixel como estratégia principal.

---

## 34. Conclusões históricas

Cenário obrigatório:

```text
Concluir c01
↓
Reabrir
↓
alterar serviço
↓
Concluir c02
```

Depois:

```text
Gerar c01
→ conteúdo histórico original

Gerar c02
→ conteúdo atualizado
```

---

## 35. Backup

Backup deverá possuir teste de ciclo completo.

```text
Estado A
↓
Backup
↓
Estado B
↓
Restore
↓
Estado A
```

---

## 36. Conteúdo restaurado

A restauração deverá validar pelo menos:

- serviços;
- roteiros;
- modelos;
- revisões;
- usuários;
- configurações;
- conclusões;
- observações;
- assets essenciais.

---

## 37. Casos de falha de backup

Testes deverão cobrir quando aplicável:

- pacote inválido;
- pacote corrompido;
- versão incompatível;
- destino inacessível;
- falta de espaço;
- falha de criação;
- importação sem restauração.

---

## 38. Sessões após restauração

Depois de restauração:

```text
sessões existentes
→ inválidas
```

Esse comportamento deverá ser testado.

---

## 39. Migrations

Toda migration deverá passar por testes antes de ser incluída em release.

Cenário 1:

```text
Banco vazio
→ migration
→ schema correto
```

Cenário 2:

```text
Banco versão anterior
+
dados representativos
→ migration
→ dados preservados
→ nova aplicação funcional
```

---

## 40. Dados de migration

Testes de migration deverão usar dados sintéticos representativos.

Não será utilizado banco real de clientes como conjunto comum de teste.

---

## 41. Atualização

O fluxo de atualização deverá ser validado.

Inclui:

- validação do pacote;
- maintenance mode;
- bloqueio de novos comandos;
- drenagem da fila;
- parada do serviço;
- migration;
- atualização dos binários;
- inicialização;
- health check.

---

## 42. Rollback

Quando uma release alterar apenas binários, rollback simples deverá ser testado quando aplicável.

Quando houver migration incompatível, o processo de recuperação deverá ser testado em conjunto com backup/restauração.

---

## 43. Instalação

A automação de testes não substitui instalação real em Windows.

Antes da v1.0 será obrigatório validar instalação em ambientes representativos.

---

## 44. Windows 11

Deverá existir teste completo em máquina Windows 11 compatível.

Fluxo:

```text
Instalar
↓
reiniciar
↓
sem login interativo
↓
Windows Service inicia
↓
outro computador acessa
↓
login funciona
```

---

## 45. Windows 10

Deverá existir teste real em Windows 10 escolhido como alvo de compatibilidade.

Esse teste determinará quais versões/edições poderão ser declaradas oficialmente compatíveis pelo Reset Service.

---

## 46. Notebook hospedeiro

Quando notebook fizer parte dos testes de implantação, deverá ser validado o comportamento das configurações de energia necessárias para manter o serviço disponível.

---

## 47. Navegadores oficiais

Matriz principal:

| Sistema cliente | Chrome | Edge |
|---|---:|---:|
| Windows 11 | Obrigatório | Obrigatório |
| Windows 10 | Obrigatório | Obrigatório |
| Windows antigo | Melhor esforço | Melhor esforço |

---

## 48. Navegadores antigos

Compatibilidade legada poderá ser testada seletivamente.

Problemas exclusivos de browsers obsoletos não impedirão uma release se os ambientes oficialmente suportados funcionarem corretamente.

---

## 49. Resolução

A interface deverá ser validada pelo menos em:

```text
1366 × 768
```

além de resoluções desktop mais amplas.

---

## 50. Acessibilidade básica

Verificações deverão incluir:

- navegação por teclado;
- foco visível;
- labels;
- contraste;
- estados não dependentes apenas de cor.

Não será exigida certificação formal de acessibilidade para v1.0.

---

## 51. Volume

Testes deverão considerar a escala de projeto:

```text
~50.000 serviços

1–10 usuários simultâneos

serviço grande:
~50 etapas
centenas de passos
```

Não precisamos executar esses volumes em toda execução de testes rápidos.

Haverá cenários específicos de desempenho/carga.

---

## 52. Desempenho

Metas orientativas:

```text
operações comuns
≈ abaixo de 500 ms

telas comuns
≈ conteúdo útil em até 2 segundos
```

no ambiente esperado de LAN e hardware compatível.

Esses números serão usados para identificar regressões reais, não como limiar artificial absoluto em cada teste automatizado.

---

## 53. Carga simultânea

Antes da v1.0 deverá existir cenário de uso simultâneo representativo com vários clientes realizando operações.

Objetivos:

- medir contenção SQLite;
- validar fila;
- observar latência;
- confirmar SignalR;
- verificar perda de comandos;
- avaliar experiência real.

---

## 54. Critério de reavaliação do SQLite

Se os testes representativos demonstrarem contenção incompatível com os requisitos operacionais, a decisão de SQLite deverá ser reavaliada antes da implantação definitiva.

A decisão será baseada em evidência, não suposição.

---

## 55. Dados de teste

Testes utilizarão dados sintéticos.

Exemplo:

```text
Empresa Exemplo Ltda.
Notebook TEST-001
SN-TEST-00001
```

Dados reais da operação não serão usados como fixture comum.

---

## 56. Cobertura

Não haverá uma meta artificial obrigatória de cobertura percentual.

Exemplo não adotado:

```text
Coverage obrigatória = 90%
```

Cobertura servirá para localizar áreas negligenciadas.

O objetivo principal será cobertura de comportamento crítico.

---

## 57. Áreas obrigatoriamente bem testadas

Terão forte cobertura:

- workflow;
- lifecycle;
- modelos/revisões;
- permissões;
- autenticação;
- conclusão;
- snapshots;
- documentos;
- concorrência;
- fila;
- sequência de IDs;
- migrations;
- backup/restauração.

---

## 58. Qualidade da implementação

Uma funcionalidade não será considerada concluída apenas porque funciona manualmente uma vez.

Critério:

```text
Regra correta
+
persistência correta
+
segurança correta
+
testes aplicáveis
+
integração correta
=
funcionalidade pronta
```

---

## 59. Definition of Done da feature

Uma funcionalidade será considerada concluída quando, conforme aplicável:

1. comportamento estiver de acordo com as especificações;
2. código compilar sem erros;
3. testes unitários relevantes existirem e passarem;
4. testes de integração relevantes passarem;
5. persistência for validada em SQLite;
6. autorização for validada;
7. antiforgery e segurança forem considerados;
8. concorrência for considerada;
9. SignalR estiver sincronizando quando necessário;
10. erros e estados de processamento forem tratados;
11. não houver regressão conhecida relevante;
12. documentação afetada estiver atualizada.

---

## 60. Antes de commit de implementação

O conjunto de testes aplicável deverá passar localmente.

Direção:

```text
dotnet test
```

Além de outros testes específicos necessários à feature.

---

## 61. Definition of Done da release

Uma release deverá adicionalmente possuir:

- build de Release aprovado;
- migrations validadas;
- instalação/atualização testada;
- fluxo multiusuário validado;
- segurança crítica verificada;
- backup/restauração validado quando afetado;
- documentos validados quando afetados;
- navegadores oficiais verificados;
- ausência de defeitos críticos conhecidos.

---

## 62. Defeitos bloqueadores

Uma release não poderá ser aprovada com defeitos conhecidos capazes de provocar:

- corrupção de dados;
- perda de dados confirmados;
- IDs duplicados;
- quebra de autenticação;
- quebra de autorização;
- vazamento de notas internas;
- conclusão inválida;
- snapshot histórico incorreto;
- restauração insegura;
- migration destrutiva inesperada.

---

## 63. Teste final multiusuário da v1.0

Antes da liberação definitiva será obrigatório testar com múltiplos navegadores reais.

Cenário:

```text
Usuários simultâneos
       ↓
mesmo serviço
       ↓
ações diferentes
       ↓
fila
       ↓
SQLite
       ↓
SignalR
       ↓
todos sincronizados
```

Nenhuma alteração confirmada poderá desaparecer ou ser sobrescrita silenciosamente.

---

## 64. Filosofia de qualidade

O projeto seguirá a regra:

```text
"Funcionou na minha máquina"
não é critério de aceite.
```

O critério será:

```text
comportamento especificado
+
testado
+
integrado
+
validado operacionalmente
=
pronto
```

---

## 65. Estado da decisão

**PLANNING-016 — Estratégia de Testes e Critérios de Qualidade: CONCLUÍDA E APROVADA.**

Este documento deverá orientar desenvolvimento, revisão de código, commits, releases e implantação.