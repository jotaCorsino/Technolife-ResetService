# Reset Service

Aplicação web interna da Technolife para criação, organização, consulta e manutenção de documentação técnica usada no trabalho diário da equipe.

A ferramenta funciona como uma base de conhecimento operacional para assistência técnica, redes, servidores, firewall, DNS, e-mail, hospedagem e service desk.

## Objetivo

Reduzir o tempo necessário para encontrar e executar corretamente tarefas técnicas recorrentes.

Exemplos de conteúdo:

- formatação e preparação padrão de computadores;
- troubleshooting de estações e redes;
- configuração de firewalls, VPN, DNS e e-mail;
- procedimentos de servidores e compartilhamentos;
- migração de hospedagem;
- checklists de implantação e manutenção;
- referências e comandos técnicos.

## Princípios do produto

- simples antes de complexo;
- rápido para encontrar, ler e editar;
- boa UI e UX como parte do produto, não acabamento posterior;
- proteção contra perda de documentação;
- manutenção e atualização simples;
- funcionamento local, sem dependência obrigatória de internet;
- nenhuma instalação nas estações clientes.

## Experiência principal

```text
Abrir navegador
      ↓
Acessar o endereço interno
      ↓
Pesquisar ou navegar
      ↓
Abrir documentação
      ↓
Executar tarefa técnica
```

Para registrar conhecimento:

```text
Nova documentação
      ↓
Escolher tipo / categoria
      ↓
Escrever ou adaptar conteúdo
      ↓
Salvar
      ↓
Conhecimento disponível para a equipe
```

## Tipos de documentação

A primeira versão deverá suportar:

- Procedimento;
- Troubleshooting;
- Configuração;
- Checklist;
- Referência;
- Documento livre.

## Organização

Os documentos serão organizados por:

- categorias e subcategorias;
- tags;
- tipo;
- pesquisa textual.

Recursos de produtividade previstos incluem favoritos, recentes, duplicação e templates.

## Proteção da informação

A aplicação deverá possuir, de forma progressiva:

- salvamento seguro;
- histórico de versões;
- restauração de versões;
- lixeira;
- backup;
- controle de concorrência otimista para evitar sobrescrita silenciosa.

Ações destrutivas devem ser reversíveis sempre que possível.

## Arquitetura resumida

```text
Browsers na LAN
      ↓
ASP.NET Core / Razor Pages
      ↓
EF Core
      ↓
SQLite local
```

A versão atual não utilizará como requisitos centrais:

- Command Queue;
- SignalR para sincronização global;
- microsserviços;
- frontend SPA separado;
- infraestrutura distribuída.

## Stack principal

- C# / .NET 10;
- ASP.NET Core / Razor Pages;
- HTML, CSS e JavaScript;
- Entity Framework Core;
- SQLite;
- autenticação local;
- execução centralizada em Windows.

## Implantação

O sistema será instalado em uma única máquina Windows da empresa, que poderá ser desktop, notebook ou Windows Server.

Os demais computadores acessarão pela rede local usando Chrome ou Edge, por um endereço interno estável, por exemplo:

```text
https://resetservice/
```

## Estado atual

O projeto passou por um pivô de produto em agosto de 2026.

A concepção anterior, orientada à criação e execução de serviços técnicos passo a passo, foi descontinuada antes da implementação do domínio.

A fundação já criada — ASP.NET Core, Razor Pages, EF Core e SQLite — será reaproveitada.

O trabalho atual está concentrado em consolidar o novo escopo e iniciar o núcleo documental.

## Documentação principal

- [Destino do produto](docs/product/product-destination.md)
- [Arquitetura](docs/architecture/architecture.md)
- [Modelo de dados](docs/architecture/data-model.md)
- [Estado atual](docs/planning/current-state.md)
- [Roadmap](docs/planning/roadmap.md)
- [Backlog](docs/planning/backlog.md)
- [Plano de sprints](docs/planning/sprint-plan.md)

> Especificações antigas centradas em `Service`, `ServiceTemplate`, execução de roteiro, Command Queue e SignalR são consideradas legado de planejamento e não devem orientar nova implementação enquanto não forem removidas ou arquivadas.
