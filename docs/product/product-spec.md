# Reset Service — Product Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Produto  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Produto-alvo:** Reset Service v1.0

---

## 1. Objetivo deste documento

Este documento define a visão, o propósito, o escopo funcional e os limites da versão 1.0 do Reset Service.

Ele representa a referência principal sobre **o que o produto deve fazer**.

Decisões relacionadas a arquitetura, tecnologias, banco de dados, implantação, segurança técnica, estrutura de código e implementação serão documentadas separadamente em etapas posteriores do planejamento.

O código deverá atender às regras e comportamentos definidos neste documento, e não o contrário.

---

## 2. Visão do produto

O **Reset Service** é um sistema interno da Technolife destinado à criação, execução, acompanhamento e registro de roteiros padronizados para serviços técnicos.

O sistema deverá permitir que procedimentos recorrentes sejam definidos através de modelos reutilizáveis compostos por etapas e passos.

Ao iniciar um novo serviço, um modelo poderá ser utilizado como base para gerar um roteiro independente que será executado passo a passo pelo técnico.

Durante a execução será possível acompanhar o progresso, registrar observações, personalizar o roteiro quando necessário e manter o histórico do trabalho realizado.

Ao final, o sistema deverá gerar documentação padronizada tanto para controle interno da Technolife quanto para apresentação ao cliente.

---

## 3. Problema que o produto resolve

Procedimentos técnicos executados sem uma ferramenta padronizada podem depender excessivamente da memória do técnico, de documentos estáticos, anotações externas ou checklists não centralizados.

Isso pode causar:

- diferenças na execução de um mesmo tipo de serviço;
- etapas esquecidas;
- dificuldade para acompanhar o andamento;
- ausência de histórico estruturado;
- perda de informações importantes;
- dificuldade de comprovar o serviço realizado;
- retrabalho na preparação de relatórios para clientes.

O Reset Service deverá transformar esses procedimentos em roteiros digitais estruturados, rastreáveis e reutilizáveis.

---

## 4. Propósito

O Reset Service deverá:

- padronizar procedimentos técnicos;
- orientar o técnico durante a execução;
- permitir flexibilidade quando um serviço exigir particularidades;
- acompanhar visualmente o progresso;
- registrar informações relevantes durante a execução;
- manter histórico dos serviços realizados;
- gerar documentação profissional;
- servir como ferramenta de prestação de contas ao cliente.

---

## 5. Escopo inicial de utilização

A utilização inicial do Reset Service será direcionada aos procedimentos de preparação e formatação de computadores executados pela Technolife.

Os primeiros modelos previstos incluem, entre outros:

- formatação para cliente por contrato;
- formatação para cliente convencional.

Apesar disso, a estrutura funcional do produto não deverá ser limitada especificamente à formatação de computadores.

O conceito central do sistema será baseado em **roteiros de serviços técnicos**, possibilitando a criação futura de outros tipos de procedimentos sem necessidade de alterar o princípio fundamental do produto.

---

## 6. Conceitos fundamentais

A estrutura conceitual principal do sistema será:

**Modelo → Serviço → Etapa → Passo**

### 6.1. Modelo

Um modelo representa um roteiro reutilizável para determinado tipo de serviço.

O modelo contém uma sequência organizada de etapas e passos.

### 6.2. Serviço

Um serviço representa uma execução real de um roteiro.

Ele poderá ser criado utilizando um modelo existente como origem.

Após sua criação, o roteiro pertence exclusivamente ao serviço e poderá ser personalizado independentemente do modelo original.

### 6.3. Etapa

Uma etapa representa uma seção lógica do roteiro.

As etapas possuem ordem definida e agrupam passos relacionados.

Na experiência principal de execução, cada etapa será representada visualmente como uma página ou folha do roteiro.

### 6.4. Passo

Um passo representa uma ação que deve ser verificada ou executada durante uma etapa.

Os passos funcionam principalmente como itens de checklist e são utilizados para acompanhamento do progresso do serviço.

---

## 7. Dashboard

O sistema deverá possuir uma tela inicial destinada à visão operacional dos serviços.

O dashboard deverá permitir identificar rapidamente:

- serviços em andamento;
- serviços aguardando;
- serviços concluídos recentemente;
- quantidade de serviços por estado;
- progresso dos serviços ativos;
- etapa atual dos serviços;
- serviços que necessitam de atenção;
- acesso rápido aos serviços;
- criação de um novo serviço.

O dashboard deverá priorizar informação operacional e não análises complexas.

Sua principal função será responder:

> Quais serviços existem e qual é a situação atual de cada um?

---

## 8. Modelos de serviço

O sistema deverá permitir o gerenciamento de modelos de serviço.

Deverá ser possível:

- criar modelo;
- editar modelo;
- visualizar modelo;
- duplicar modelo;
- arquivar ou desativar modelo;
- utilizar modelo para criar um serviço;
- definir nome;
- definir descrição;
- criar etapas;
- editar etapas;
- remover etapas;
- reordenar etapas;
- criar passos;
- editar passos;
- remover passos;
- reordenar passos.

A estrutura básica será:

```text
Modelo
├── Etapa 1
│   ├── Passo
│   ├── Passo
│   └── Passo
├── Etapa 2
│   ├── Passo
│   └── Passo
└── Etapa N
```

---

## 9. Independência entre modelo e serviço

Esta é uma regra fundamental do produto.

Quando um serviço for criado a partir de um modelo, o sistema deverá gerar uma cópia independente do roteiro.

Alterações posteriores no modelo original não poderão modificar serviços já existentes.

Da mesma forma, alterações realizadas no roteiro de um serviço não poderão alterar seu modelo de origem.

Fluxo conceitual:

```text
Modelo
   ↓
Cópia no momento da criação
   ↓
Roteiro independente do serviço
```

---

## 10. Revisão de modelos

Os modelos deverão possuir um mecanismo simples de identificação de revisão.

O objetivo é permitir rastrear qual versão do procedimento foi utilizada na criação de determinado serviço.

Exemplo:

```text
Modelo: Formatação Contrato
Revisão: 4
```

Um serviço deverá manter registro de:

- modelo de origem;
- revisão utilizada no momento da criação.

O sistema de revisão deverá permanecer simples e não será tratado como uma plataforma complexa de versionamento de documentos.

---

## 11. Criação de serviços

O sistema deverá permitir a criação de novos serviços a partir de modelos existentes.

O fluxo conceitual será:

```text
Novo serviço
      ↓
Selecionar modelo
      ↓
Preencher informações
      ↓
Criar serviço
      ↓
Gerar roteiro independente
```

Cada serviço deverá possuir um identificador único gerado automaticamente pelo sistema.

O usuário não deverá escolher manualmente nem reutilizar esse identificador.

O formato final do identificador será definido posteriormente.

---

## 12. Informações do serviço

Cada serviço possuirá um conjunto de informações de identificação.

### 12.1. Informações gerais

Poderão incluir:

- ID do serviço;
- título;
- modelo de origem;
- revisão do modelo;
- data de criação;
- responsável;
- situação atual;
- observações gerais.

### 12.2. Cliente

Os dados poderão incluir:

- nome;
- empresa;
- telefone;
- e-mail;
- identificação adicional.

Esses campos não deverão ser obrigatórios por padrão.

### 12.3. Equipamento

Os dados poderão incluir:

- fabricante;
- modelo;
- número de série;
- patrimônio;
- hostname;
- sistema operacional;
- observações.

Esses campos também poderão ser opcionais.

O Reset Service não terá como objetivo funcionar como um CRM completo ou sistema completo de inventário.

---

## 13. Estados do serviço

Um serviço poderá assumir os seguintes estados principais:

### Rascunho

Serviço criado, porém ainda não iniciado.

### Em andamento

Serviço cuja execução está ativa.

### Aguardando

Serviço temporariamente interrompido devido a alguma dependência, informação ou condição externa.

### Concluído

Serviço encerrado após sua execução.

### Cancelado

Serviço interrompido definitivamente sem conclusão normal.

Mudanças relevantes de estado deverão ocorrer de maneira explícita.

---

## 14. Roteiro de execução

O roteiro de execução será a experiência principal do Reset Service.

Cada etapa será apresentada como uma página ou folha individual do roteiro.

A interface deverá permitir que o usuário concentre sua atenção na etapa atual enquanto mantém visível sua posição dentro do serviço.

Conceitualmente:

```text
Serviço

●━━━━●━━━━◉━━━━○━━━━○
1    2    3    4    5

┌───────────────────────────┐
│ ETAPA 3 DE 5              │
│                           │
│ Configuração              │
│                           │
│ ☑ Drivers                 │
│ ☑ Atualizações            │
│ ☐ Aplicativos             │
│ ☐ Impressoras             │
│                           │
│ Observações               │
│ ...                       │
└───────────────────────────┘

← Anterior          Próxima →
```

---

## 15. Navegação entre etapas

O usuário deverá conseguir navegar entre as etapas utilizando:

- ação para etapa anterior;
- ação para próxima etapa;
- seleção direta de uma etapa através da navegação superior.

A navegação não deverá ser bloqueada apenas porque a etapa atual ainda possui passos pendentes.

O sistema poderá informar pendências, mas deverá permitir que o técnico consulte ou execute outras etapas quando necessário.

---

## 16. Passos do roteiro

Cada passo poderá possuir:

- título;
- descrição ou instrução;
- estado;
- observação;
- posição dentro da etapa.

Os estados inicialmente previstos são:

- pendente;
- concluído;
- não aplicável.

Um passo marcado como **não aplicável** representa uma ação que faz parte do roteiro, mas não é necessária naquele serviço específico.

Passos não aplicáveis não deverão prejudicar o cálculo do progresso.

As regras detalhadas de comportamento dos passos serão definidas posteriormente.

---

## 17. Observações

O sistema deverá permitir o registro de observações em diferentes níveis.

### 17.1. Observação do passo

Relacionada diretamente a uma ação específica.

### 17.2. Observação da etapa

Relacionada ao conjunto de atividades daquela etapa.

### 17.3. Observação do serviço

Relacionada ao serviço como um todo.

O sistema deverá permitir distinguir informações de uso exclusivamente interno de informações destinadas ao cliente.

Essa classificação será utilizada posteriormente na geração dos documentos.

---

## 18. Personalização do roteiro

Após a criação de um serviço, seu roteiro poderá ser personalizado.

O usuário poderá:

- adicionar etapas;
- editar etapas;
- remover etapas;
- reordenar etapas;
- adicionar passos;
- editar passos;
- remover passos;
- reordenar passos;
- adicionar observações.

Essas alterações serão exclusivas daquele serviço.

O sistema deverá registrar que o roteiro foi personalizado em relação ao modelo utilizado como origem.

---

## 19. Progresso

O sistema deverá calcular automaticamente o progresso do serviço a partir dos passos aplicáveis.

O progresso deverá ser atualizado imediatamente quando um passo tiver seu estado alterado.

Exemplo conceitual:

```text
18 passos aplicáveis
12 passos concluídos

Progresso: 66,7%
```

O progresso deverá possuir representação visual clara.

Também deverá ser possível identificar o progresso das etapas.

---

## 20. Revisão e conclusão do serviço

Antes da conclusão definitiva, o sistema deverá apresentar uma revisão do serviço.

Essa revisão deverá permitir identificar:

- etapas concluídas;
- etapas com pendências;
- quantidade de passos concluídos;
- passos aplicáveis ainda pendentes;
- observações relevantes.

O usuário deverá ser informado claramente sobre pendências antes de concluir o serviço.

Após a confirmação, o serviço assumirá o estado **Concluído**.

---

## 21. Proteção de serviços concluídos

Serviços concluídos deverão ser protegidos contra alterações comuns.

O objetivo é preservar a integridade do registro histórico e dos documentos gerados.

Alterações posteriores deverão exigir uma ação explícita de reabertura.

---

## 22. Reabertura

O sistema deverá permitir a reabertura de um serviço concluído quando houver necessidade operacional.

Essa ação deverá:

- ser explícita;
- ser registrada no histórico;
- identificar quando ocorreu;
- identificar o usuário responsável.

A reabertura não poderá ocorrer silenciosamente.

---

## 23. Histórico do serviço

O sistema deverá manter um histórico simples dos eventos relevantes do serviço.

Exemplos:

```text
08:32 Serviço criado
08:34 Serviço iniciado
09:14 Etapa concluída
10:22 Serviço colocado em espera
11:05 Serviço retomado
12:42 Serviço concluído
```

Eventos relevantes poderão incluir:

- criação;
- início;
- alteração de estado;
- personalização relevante;
- conclusão;
- reabertura;
- cancelamento.

O objetivo não é registrar indiscriminadamente cada clique da interface, mas manter rastreabilidade operacional suficiente.

---

## 24. Serviços concluídos

O sistema deverá possuir uma área de histórico de serviços.

O usuário deverá conseguir:

- visualizar serviços concluídos;
- pesquisar serviços;
- filtrar serviços;
- abrir um serviço concluído;
- consultar seu roteiro;
- consultar observações;
- acessar documentos relacionados;
- regenerar documentos quando permitido.

Um serviço concluído deverá continuar armazenado como registro estruturado dentro do Reset Service.

O PDF não será a única fonte histórica do serviço.

---

## 25. Pesquisa e filtros

O sistema deverá permitir localizar serviços através de informações relevantes.

Os critérios poderão incluir:

- ID;
- cliente;
- empresa;
- equipamento;
- número de série;
- patrimônio;
- estado;
- período.

Não está prevista para a versão 1.0 uma ferramenta avançada de pesquisa textual global.

---

## 26. Registro interno em PDF

O Reset Service deverá gerar um documento padronizado destinado ao controle interno da Technolife.

Esse documento poderá conter:

- identidade da Technolife;
- ID do serviço;
- datas;
- cliente;
- equipamento;
- responsável;
- modelo utilizado;
- revisão utilizada;
- etapas;
- passos;
- situação dos passos;
- observações internas;
- observações técnicas;
- conclusão.

Esse documento funcionará como registro técnico do serviço executado.

---

## 27. Relatório do cliente em PDF

O sistema deverá gerar um documento separado destinado ao cliente.

Esse relatório deverá possuir apresentação profissional e conteúdo apropriado ao contexto externo.

Poderá conter:

- logo da Technolife;
- nome da empresa;
- ID do serviço;
- data;
- cliente;
- equipamento;
- descrição do serviço;
- procedimentos executados;
- observações destinadas ao cliente;
- recomendações;
- conclusão;
- informações de contato da Technolife.

Informações classificadas como exclusivamente internas não poderão aparecer nesse documento.

---

## 28. Fonte única de informações

Os relatórios deverão ser derivados das informações registradas durante a execução do serviço.

O usuário não deverá precisar preencher novamente as mesmas informações para gerar documentos.

Fluxo:

```text
Execução do serviço
        ↓
Dados estruturados
        ↓
 ┌──────┴─────────┐
 ↓                ↓
Registro      Relatório
interno       do cliente
```

O serviço armazenado no sistema será a fonte de verdade.

---

## 29. Configurações da empresa

O sistema deverá possuir uma área de configuração das informações institucionais da Technolife.

Poderão ser cadastrados:

- nome;
- nome empresarial;
- logo;
- CNPJ;
- telefone;
- e-mail;
- site;
- endereço;
- outras informações institucionais necessárias.

Essas informações deverão poder ser reutilizadas automaticamente nos documentos gerados.

---

## 30. Configuração dos documentos

A configuração dos documentos será conceitualmente separada dos dados cadastrais da empresa.

O sistema deverá permitir definir informações utilizadas em:

- cabeçalho;
- rodapé;
- registro interno;
- relatório do cliente.

A versão 1.0 utilizará layouts profissionais padronizados e configuráveis.

Não faz parte do escopo criar um editor gráfico completo de documentos semelhante a um editor de texto ou ferramenta de design.

---

## 31. Usuários e autenticação

A versão 1.0 deverá possuir autenticação local.

Inicialmente serão considerados dois perfis principais.

### 31.1. Administrador

Poderá:

- administrar usuários;
- configurar informações da empresa;
- configurar documentos;
- criar e editar modelos;
- utilizar modelos;
- criar serviços;
- executar serviços;
- consultar histórico;
- reabrir serviços.

### 31.2. Técnico

Poderá:

- visualizar serviços;
- criar serviços;
- executar serviços;
- personalizar roteiros;
- concluir serviços;
- consultar histórico.

A estrutura deverá permanecer simples e evitar um sistema excessivamente granular de permissões na versão 1.0.

---

## 32. Identificação de responsáveis

Quando aplicável, o sistema deverá registrar usuários responsáveis por ações relevantes.

Exemplos:

- criado por;
- responsável pelo serviço;
- concluído por;
- reaberto por;
- cancelado por.

Essa informação poderá ser utilizada na rastreabilidade e nos documentos internos.

---

## 33. Backup e restauração

Os dados armazenados pelo Reset Service deverão ser recuperáveis.

A versão 1.0 deverá possuir uma estratégia definida para:

- realização de backup;
- preservação dos dados;
- restauração em caso de falha.

Os detalhes técnicos serão definidos durante o planejamento de arquitetura.

---

## 34. Segurança operacional

O sistema deverá reduzir o risco de ações destrutivas acidentais.

Ações relevantes deverão utilizar confirmação apropriada quando necessário.

Exemplos:

- cancelar serviço;
- reabrir serviço;
- remover informações importantes;
- arquivar modelo.

Sempre que adequado, o sistema deverá preferir arquivamento ou desativação a exclusões permanentes.

---

## 35. Experiência de uso

A experiência de utilização deverá priorizar:

- interface moderna;
- simplicidade;
- boa hierarquia visual;
- poucos cliques;
- navegação previsível;
- feedback imediato;
- progresso visível;
- foco na atividade atual;
- legibilidade;
- baixa carga visual;
- uso cotidiano rápido.

O fluxo operacional principal deverá permanecer simples:

```text
Abrir serviço
      ↓
Executar passo
      ↓
Marcar resultado
      ↓
Registrar observação se necessário
      ↓
Avançar
```

---

## 36. Estrutura funcional da versão 1.0

```text
RESET SERVICE
│
├── Dashboard
│
├── Serviços
│   ├── Novo
│   ├── Rascunhos
│   ├── Em andamento
│   ├── Aguardando
│   ├── Concluídos
│   └── Cancelados
│
├── Modelos
│   ├── Criar
│   ├── Editar
│   ├── Visualizar
│   ├── Duplicar
│   ├── Arquivar
│   └── Usar
│
├── Execução
│   ├── Etapas
│   ├── Passos
│   ├── Checklist
│   ├── Observações
│   ├── Progresso
│   └── Personalização
│
├── Documentos
│   ├── Registro interno
│   └── Relatório do cliente
│
├── Histórico
│
└── Configurações
    ├── Empresa
    ├── Documentos
    └── Usuários
```

---

## 37. Fora do escopo da versão 1.0

Os seguintes recursos não fazem parte do escopo planejado da versão 1.0:

- sistema financeiro;
- cobrança;
- emissão de nota fiscal;
- controle de estoque;
- inventário completo de equipamentos;
- CRM completo;
- portal para clientes;
- abertura de chamados por clientes;
- integração com WhatsApp;
- envio automático de e-mails;
- integração com Active Directory;
- integração com Microsoft 365;
- integrações externas;
- dependência de armazenamento em nuvem;
- aplicativo mobile;
- disponibilização pública pela internet;
- inteligência artificial integrada ao produto;
- assinatura digital;
- editor gráfico avançado de PDFs;
- automação da instalação ou formatação do sistema operacional.

O Reset Service deverá orientar, acompanhar, registrar e documentar o serviço.

Ele não será, na versão 1.0, uma ferramenta automática de deployment ou instalação de sistemas operacionais.

---

## 38. Premissas do produto

O planejamento da versão 1.0 considera as seguintes premissas:

1. O Reset Service será utilizado internamente pela Technolife.
2. O sistema deverá funcionar sem dependência da internet.
3. O ambiente operacional será a rede local da empresa.
4. Diferentes computadores da rede deverão conseguir acessar o sistema.
5. Os dados deverão permanecer centralizados e preservados.
6. A aplicação deverá ser leve e adequada ao uso cotidiano.
7. A arquitetura definitiva será escolhida posteriormente.
8. O produto deverá preservar histórico e rastreabilidade dos serviços.
9. A experiência de execução do roteiro será a prioridade da interface.
10. Novas funcionalidades não deverão comprometer a simplicidade da experiência principal.

---

## 39. Princípios de produto

### Roteiro

O sistema deve deixar claro o que precisa ser feito.

### Foco

O usuário deve conseguir concentrar-se na etapa atual sem perder contexto.

### Progresso

O sistema deve deixar evidente quanto do serviço já foi realizado e o que ainda falta.

### Simplicidade

Funcionalidades não devem adicionar complexidade desnecessária ao uso diário.

### Rastreabilidade

A aplicação deve preservar informações suficientes para reconstruir o que aconteceu durante um serviço.

### Flexibilidade controlada

Os modelos padronizam os procedimentos, mas serviços específicos podem ser personalizados sem modificar sua origem.

### Fonte única de verdade

Dashboard, histórico e relatórios devem derivar das mesmas informações do serviço.

### Separação de contexto

Informações internas e informações destinadas ao cliente devem permanecer claramente separadas.

---

## 40. Definição de sucesso da versão 1.0

A versão 1.0 será considerada funcionalmente completa quando a Technolife conseguir realizar o seguinte fluxo utilizando somente o Reset Service:

```text
Criar modelo
      ↓
Organizar etapas e passos
      ↓
Criar serviço através do modelo
      ↓
Registrar cliente/equipamento quando necessário
      ↓
Executar roteiro passo a passo
      ↓
Personalizar roteiro quando necessário
      ↓
Registrar observações
      ↓
Acompanhar progresso
      ↓
Revisar serviço
      ↓
Concluir
      ↓
Gerar registro interno
      ↓
Gerar relatório para cliente
      ↓
Consultar serviço posteriormente no histórico
```

Esse fluxo representa o núcleo funcional do produto.

---

## 41. Controle de alterações deste documento

Este documento deverá evoluir somente quando houver uma decisão consciente de alteração do produto.

Mudanças funcionais relevantes deverão ser refletidas aqui antes ou juntamente com sua implementação.

O documento não deverá ser alterado apenas para descrever comportamentos divergentes criados acidentalmente durante o desenvolvimento.

---

## 42. Estado da decisão

**PLANNING-001 — Escopo funcional do produto v1.0: CONCLUÍDA E APROVADA.**

Este documento formaliza as decisões estabelecidas durante essa etapa e passa a servir como referência para as próximas atividades de planejamento.