# Reset Service — Service Data Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Identificação e Dados do Serviço  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`, `service-template-spec.md`

---

## 1. Objetivo

Este documento define as informações pertencentes a um serviço no Reset Service, incluindo:

- identificação;
- dados do cliente;
- dados do equipamento;
- dados operacionais;
- origem do roteiro;
- obrigatoriedade dos campos;
- regras de edição;
- preservação histórica;
- informações pesquisáveis.

A definição de banco de dados, tipos técnicos, índices ou implementação será realizada posteriormente.

---

## 2. Princípio de simplicidade

A criação de um serviço deverá exigir o mínimo de preenchimento manual possível.

A regra funcional será:

> O Reset Service exige somente as informações necessárias para identificar o serviço e seu roteiro. Dados de cliente e equipamento são opcionais.

A ausência dessas informações não deverá impedir a criação ou execução de um serviço.

---

## 3. Grupos de informações

Os dados serão organizados conceitualmente em:

```text
SERVIÇO
│
├── Identificação
├── Cliente
├── Equipamento
├── Operação
└── Origem do roteiro
```

Essa divisão deverá orientar tanto a apresentação quanto a futura modelagem dos dados.

---

## 4. Identificador do serviço

Todo serviço deverá possuir um identificador:

- único;
- gerado automaticamente;
- permanente;
- não editável;
- não reutilizável.

O formato funcional será:

```text
RS-AAAA-NNNNN
```

Onde:

- `RS` identifica o Reset Service;
- `AAAA` representa o ano de criação;
- `NNNNN` representa a sequência anual.

Exemplo:

```text
RS-2026-00142
```

---

## 5. Sequência anual

A sequência numérica poderá reiniciar a cada novo ano.

Exemplo:

```text
RS-2026-00831
RS-2026-00832

RS-2027-00001
RS-2027-00002
```

Como o ano faz parte do identificador, os códigos permanecem únicos.

A capacidade prevista de cinco dígitos permite até 99.999 identificadores por ano.

---

## 6. Momento da geração do ID

O identificador deverá ser criado imediatamente quando o serviço for registrado.

```text
Criar serviço
      ↓
Gerar ID
      ↓
Rascunho
```

O ID existe antes do início operacional do serviço.

---

## 7. IDs cancelados

Um identificador já criado nunca poderá ser reutilizado.

Exemplo:

```text
RS-2026-00142 → Cancelado
RS-2026-00143 → Próximo serviço
```

O cancelamento não deverá causar reaproveitamento da sequência anterior.

---

## 8. Título

Todo serviço deverá possuir um título.

Ao criar o serviço a partir de um modelo, o título inicial deverá utilizar automaticamente o nome do modelo.

Exemplo:

```text
Modelo:
Formatação — Cliente Contrato

Título inicial:
Formatação — Cliente Contrato
```

O título poderá posteriormente ser personalizado.

---

## 9. Personalização do título

Alterar o título do serviço não altera:

- modelo de origem;
- revisão de origem;
- nome do modelo;
- histórico do modelo.

Exemplo:

```text
Título:
Formatação e preparação — Notebook Financeiro
```

O título representa a identificação humana daquela execução específica.

---

## 10. Identificação básica

Todo serviço deverá possuir:

| Informação | Obrigatoriedade | Origem |
|---|---|---|
| ID | Obrigatório | Sistema |
| Título | Obrigatório | Modelo / usuário |
| Status | Obrigatório | Sistema |
| Criado em | Obrigatório | Sistema |
| Criado por | Obrigatório | Sistema |
| Responsável | Opcional | Usuário |
| Modelo de origem | Obrigatório | Sistema |
| Revisão de origem | Obrigatório | Sistema |

---

## 11. Dados do cliente

O serviço poderá armazenar:

- nome;
- empresa;
- telefone;
- e-mail;
- referência do cliente.

Todos serão opcionais.

---

## 12. Nome e empresa

Nome e empresa serão campos independentes.

Isso permite representar diferentes cenários.

### Pessoa física

```text
Nome:
João da Silva

Empresa:
—
```

### Empresa com contato

```text
Nome:
Maria Souza

Empresa:
Empresa ABC Ltda.
```

### Empresa sem contato individual

```text
Nome:
—

Empresa:
Empresa ABC Ltda.
```

---

## 13. Referência do cliente

O campo **Referência do cliente** será textual e opcional.

Poderá armazenar informações como:

- código interno;
- contrato;
- departamento;
- unidade;
- referência em outro sistema;
- outra identificação pertinente.

Não serão criados campos especializados para cada uma dessas possibilidades na versão 1.0.

---

## 14. Ausência de CRM

A versão 1.0 não terá como objetivo manter um cadastro central completo de clientes.

Os dados deverão pertencer diretamente ao contexto histórico do serviço.

```text
Serviço
└── Dados do cliente
```

O Reset Service não será tratado como CRM.

---

## 15. Preservação dos dados do cliente

Os dados armazenados em um serviço representam as informações utilizadas naquela execução.

Alterações futuras nos dados reais do cliente não deverão modificar automaticamente serviços anteriores.

Essa regra protege:

- histórico;
- PDFs;
- rastreabilidade.

---

## 16. Dados do equipamento

O serviço poderá armazenar:

- descrição;
- fabricante;
- modelo;
- número de série;
- patrimônio;
- hostname;
- sistema operacional;
- observação do equipamento.

Todos serão opcionais.

---

## 17. Descrição do equipamento

O campo Descrição fornecerá identificação rápida e humana.

Exemplos:

```text
Notebook do financeiro
```

```text
Desktop da recepção
```

Não será necessário possuir fabricante ou modelo para utilizar esse campo.

---

## 18. Fabricante e modelo

Fabricante e modelo deverão permanecer separados.

Exemplo:

```text
Fabricante:
Dell

Modelo:
Latitude 5420
```

Essa separação facilita leitura, pesquisa e documentação.

---

## 19. Número de série

O número de série será opcional e deverá ser pesquisável.

Exemplo:

```text
8HK29P3
```

---

## 20. Patrimônio

O patrimônio será opcional e deverá ser pesquisável.

Exemplo:

```text
TI-00452
```

---

## 21. Hostname

O hostname será opcional e deverá ser pesquisável.

Exemplo:

```text
PC-FIN-03
```

---

## 22. Sistema operacional

O sistema operacional será inicialmente um campo textual.

Exemplo:

```text
Windows 11 Pro
```

A versão 1.0 não exigirá catálogo estruturado de sistemas operacionais e edições.

---

## 23. Observação do equipamento

A observação do equipamento destina-se a características ou condições relacionadas diretamente à máquina.

Exemplo:

> Equipamento recebido com trinca próxima à dobradiça.

Essa informação é diferente das observações operacionais do serviço.

---

## 24. Níveis diferentes de informação

As informações deverão permanecer semanticamente separadas.

### Equipamento

> Carcaça apresenta risco na tampa superior.

### Serviço

> Cliente solicitou prioridade para entrega.

### Etapa

> Backup demorou devido à quantidade de arquivos.

### Passo

> Office ativado utilizando licença fornecida pelo cliente.

---

## 25. Dados operacionais

O serviço deverá possuir informações operacionais como:

- status;
- criado por;
- responsável;
- data de criação;
- data de início;
- data da conclusão vigente;
- progresso;
- indicação de roteiro personalizado.

Parte dessas informações será automaticamente gerada ou calculada pelo sistema.

---

## 26. Responsável

O responsável representa o principal usuário associado operacionalmente ao serviço.

Seu preenchimento será opcional na criação.

Um serviço poderá existir inicialmente sem responsável definido.

---

## 27. Criado por e Responsável

Esses conceitos serão diferentes.

Exemplo:

```text
Criado por:
Carlos

Responsável:
João
```

`Criado por` representa um fato histórico e não deverá ser editável.

`Responsável` representa atribuição operacional e poderá mudar conforme as regras do ciclo de vida.

---

## 28. Datas automáticas

Datas operacionais deverão ser registradas automaticamente pelo sistema.

O usuário não deverá digitar manualmente:

- data de criação;
- data de início;
- data de conclusão.

Isso reduz inconsistências nos registros.

---

## 29. Apresentação de data e hora

A interface deverá utilizar apresentação compatível com o contexto brasileiro.

Exemplos:

```text
13/08/2026
```

```text
13/08/2026 às 09:34
```

A representação técnica será definida na arquitetura.

---

## 30. Origem do roteiro

Todo serviço deverá preservar informações sobre o roteiro que originou sua criação.

Isso inclui:

- identidade do modelo;
- nome do modelo no momento da criação;
- revisão utilizada.

Essas informações serão imutáveis.

---

## 31. Preservação do nome histórico

Caso um modelo seja renomeado após a criação do serviço, o serviço deverá preservar o nome utilizado na época.

Exemplo:

```text
Serviço histórico:

Modelo:
Formatação — Contrato

Revisão:
3
```

Mesmo que o modelo atual passe a se chamar:

```text
Formatação — Cliente Contrato
```

---

## 32. Roteiro personalizado

O serviço deverá possuir uma indicação:

```text
Roteiro personalizado:
Sim / Não
```

O valor inicial será:

```text
Não
```

---

## 33. Alterações que caracterizam personalização

São consideradas alterações estruturais:

- adicionar etapa;
- remover etapa;
- editar etapa;
- reordenar etapa;
- adicionar passo;
- remover passo;
- editar passo;
- reordenar passo.

Após qualquer uma dessas ações:

```text
Roteiro personalizado = Sim
```

---

## 34. Execução não caracteriza personalização

As seguintes ações não modificam estruturalmente o roteiro:

- concluir passo;
- retornar passo para Pendente;
- marcar Não aplicável;
- adicionar observação;
- editar observação.

Essas ações representam execução e não deverão alterar o indicador de personalização.

---

## 35. Criação mínima

Para criar um serviço, o usuário deverá obrigatoriamente selecionar um modelo válido.

As demais informações obrigatórias serão geradas ou derivadas automaticamente.

```text
Selecionar modelo
       ↓
ID                  → Sistema
Título              → Modelo
Status              → Rascunho
Criado em           → Sistema
Criado por          → Usuário autenticado
Modelo de origem    → Modelo selecionado
Revisão             → Revisão atual
```

O usuário poderá criar um serviço sem preencher dados de cliente ou equipamento.

---

## 36. Informações opcionais na criação

Durante a criação poderão ser informados:

- título personalizado;
- responsável;
- nome do cliente;
- empresa;
- telefone;
- e-mail;
- referência;
- descrição do equipamento;
- fabricante;
- modelo;
- número de série;
- patrimônio;
- hostname;
- sistema operacional;
- observações pertinentes.

Nenhuma dessas informações deverá impedir a criação quando ausente.

---

## 37. Edição em Rascunho

Enquanto o serviço estiver em Rascunho, será permitido editar praticamente todos os dados informados manualmente.

Não poderão ser alterados:

- ID;
- criado em;
- criado por;
- identidade do modelo de origem;
- nome histórico do modelo de origem;
- revisão utilizada.

---

## 38. Edição Em andamento

Enquanto Em andamento, será possível corrigir ou complementar:

- título;
- responsável;
- dados do cliente;
- dados de contato;
- referência;
- dados do equipamento;
- observações permitidas.

Essa possibilidade existe porque novas informações podem surgir durante a execução.

---

## 39. Edição em Aguardando

O estado Aguardando bloqueia a execução do roteiro, mas não a manutenção de informações cadastrais.

Será permitido editar:

- título;
- responsável;
- cliente;
- empresa;
- telefone;
- e-mail;
- referência;
- informações do equipamento.

Não será permitido executar ou modificar estruturalmente o roteiro enquanto o serviço permanecer Aguardando.

---

## 40. Concluído e Cancelado

Serviços Concluídos e Cancelados deverão ter seus dados protegidos.

Para modificar qualquer informação histórica será necessário reabrir explicitamente o serviço.

Essa regra também vale para pequenas correções cadastrais.

---

## 41. Correções após fechamento

Mesmo correções simples não deverão ocorrer silenciosamente após a conclusão.

Exemplo:

```text
Del → Dell
```

Para realizar a correção:

```text
Concluído
    ↓
Reabrir
    ↓
Corrigir
    ↓
Concluir novamente
```

Isso preserva consistência entre registro, histórico e documentos emitidos.

---

## 42. Pesquisa

A pesquisa textual de serviços deverá considerar pelo menos:

- ID;
- título;
- nome do cliente;
- empresa;
- referência do cliente;
- descrição do equipamento;
- fabricante;
- modelo;
- número de série;
- patrimônio;
- hostname.

---

## 43. Filtros estruturados

Pesquisa textual e filtros serão conceitos diferentes.

Filtros deverão permitir futuramente critérios como:

- status;
- responsável;
- modelo;
- período.

Exemplo:

```text
Pesquisa:
Empresa ABC

Filtro:
Status = Em andamento
```

A definição detalhada da interface será realizada na especificação de UX.

---

## 44. Relação com PDFs

Os documentos gerados deverão utilizar os mesmos dados armazenados no serviço.

Não haverá um cadastro duplicado para geração de PDF.

```text
SERVIÇO
│
├── identificação
├── cliente
├── equipamento
├── operação
└── origem
        │
        ├── Registro interno
        └── Relatório do cliente
```

---

## 45. Informações internas e externas

Nem todas as informações deverão aparecer em todos os documentos.

Informações como:

- criado por;
- modelo;
- revisão;
- histórico operacional;

são principalmente internas.

Informações como:

- ID;
- cliente;
- equipamento;
- data;
- serviço realizado;

podem ser apresentadas ao cliente.

A composição definitiva dos documentos será especificada separadamente.

---

## 46. Campos personalizados

A versão 1.0 não terá ferramenta para criação arbitrária de campos personalizados.

Essa decisão evita complexidade desnecessária em:

- interface;
- validação;
- persistência;
- pesquisa;
- relatórios.

Novos campos deverão ser adicionados ao produto somente quando houver necessidade operacional recorrente.

---

## 47. Anexos

Arquivos, fotografias e anexos não fazem parte desta especificação.

Sua inclusão deverá ser avaliada separadamente caso exista necessidade operacional concreta.

Não deverão ser adicionados implicitamente à versão 1.0.

---

## 48. Estrutura Consolidada

```text
SERVIÇO
│
├── IDENTIFICAÇÃO
│   ├── ID
│   ├── Título
│   └── Status
│
├── CLIENTE
│   ├── Nome
│   ├── Empresa
│   ├── Telefone
│   ├── E-mail
│   └── Referência
│
├── EQUIPAMENTO
│   ├── Descrição
│   ├── Fabricante
│   ├── Modelo
│   ├── Número de série
│   ├── Patrimônio
│   ├── Hostname
│   ├── Sistema operacional
│   └── Observação
│
├── OPERAÇÃO
│   ├── Criado por
│   ├── Responsável
│   ├── Criado em
│   ├── Iniciado em
│   ├── Concluído em
│   ├── Progresso
│   └── Roteiro personalizado
│
└── ORIGEM
    ├── Identidade do modelo
    ├── Nome histórico do modelo
    └── Revisão
```

---

## 49. Regras Fundamentais

1. Todo serviço possui ID único e permanente.
2. O formato funcional do ID será `RS-AAAA-NNNNN`.
3. A sequência poderá reiniciar anualmente.
4. IDs nunca serão reutilizados.
5. Todo serviço possui título.
6. O título inicial será derivado do modelo.
7. Dados de cliente são opcionais.
8. Dados de equipamento são opcionais.
9. A versão 1.0 não funcionará como CRM completo.
10. A versão 1.0 não funcionará como inventário completo.
11. Dados de cliente e equipamento representam o contexto histórico do serviço.
12. Criado por e Responsável são conceitos distintos.
13. Datas operacionais são registradas automaticamente.
14. Origem do roteiro e revisão são imutáveis.
15. Alterações estruturais caracterizam roteiro personalizado.
16. Execução de checklist e observações não caracterizam personalização.
17. Dados cadastrais podem ser editados em Rascunho, Em andamento e Aguardando.
18. Serviços Concluídos ou Cancelados ficam protegidos.
19. Alterações após fechamento exigem reabertura.
20. Os principais identificadores de cliente e equipamento devem participar da pesquisa.
21. PDFs reutilizam os dados do próprio serviço.
22. Não haverá campos personalizados arbitrários na versão 1.0.

---

## 50. Estado da Decisão

**PLANNING-005 — Identificação e Dados do Serviço: CONCLUÍDA E APROVADA.**

Este documento passa a servir como referência para futuras especificações de criação de serviços, pesquisa, UX, relatórios, persistência e testes.