# Reset Service — Instruções para agentes de código

Este repositório passou por um pivô de produto em agosto de 2026.

## Contexto obrigatório

O Reset Service NÃO é mais um sistema de execução de serviços técnicos passo a passo.

O produto atual é uma aplicação web interna da Technolife para criação, organização, consulta, edição e preservação de documentação técnica usada em assistência técnica, redes, servidores, firewall, DNS, e-mail, hospedagem e service desk.

O pivô alterou o domínio funcional, mas NÃO anulou automaticamente requisitos transversais já aprovados de rede, Windows, segurança, backup, desempenho, compatibilidade e UX.

Antes de alterar código, leia nesta ordem:

1. `docs/planning/current-state.md`
2. `docs/planning/pivot-reconciliation.md`
3. `docs/product/product-destination.md`
4. `docs/architecture/architecture.md`
5. `docs/architecture/data-model.md`
6. `docs/planning/backlog.md`
7. `docs/planning/sprint-plan.md`
8. `docs/development/development-guide.md`
9. `docs/development/testing-strategy.md`

## Regra de interpretação do legado

Não classifique toda a documentação antiga como descartada.

Use três classes:

```text
LEGADO DE DOMÍNIO
Service / Template / Stage / Step / execução
→ não implementar

REQUISITO TRANSVERSAL PRESERVADO
LAN / Windows / segurança / backup / desempenho / UX
→ continua restringindo o produto

NOVO NÚCLEO DOCUMENTAL
Document / Category / Tag / Version / busca / editor
→ implementar conforme backlog vigente
```

`docs/planning/pivot-reconciliation.md` contém a classificação detalhada.

## Requisitos transversais que não podem ser esquecidos

Preservar, salvo decisão posterior explícita:

- aplicação web centralizada;
- nenhuma instalação nas estações clientes;
- funcionamento normal sem internet;
- acesso pela LAN;
- host podendo ser desktop, notebook ou Windows Server;
- Windows 10/11 x64 como alvos de validação do host conforme runtime;
- Chrome/Edge modernos como navegadores principais;
- Windows antigos como clientes em melhor esforço quando o browser permitir;
- endereço interno estável, preferencialmente por DNS/hostname;
- HTTPS oficial na LAN quando implantado;
- Windows Service e inicialização sem login interativo como direção de deploy;
- separação entre binários substituíveis e dados persistentes;
- atualização centralizada e possível offline;
- aproximadamente 1–20 usuários cadastrados e 1–10 simultâneos como escala de referência;
- UX rápida na LAN;
- nenhum overwrite silencioso em edição concorrente;
- feedback claro de falha de comunicação/salvamento;
- autenticação local e contas individuais quando a sprint correspondente começar;
- backend como autoridade de identidade/permissão;
- backup e restauração como requisitos essenciais;
- possibilidade de cópia de backup fora do disco primário;
- logs técnicos para diagnóstico;
- desktop/notebook e 1366×768 como base mínima de UX;
- alta qualidade de UI/UX apesar da simplicidade técnica.

## Fonte de verdade do domínio atual

Não implementar como parte do produto atual apenas por aparecer em documentos antigos:

- `Service` como entidade principal;
- `ServiceTemplate` / `TemplateRevision`;
- execução por `Stage` / `Step`;
- estados/progresso do antigo Service;
- snapshots/conclusões do antigo Service;
- PDFs operacionais como requisito do MVP;
- SignalR como requisito central;
- Command Queue / `System.Threading.Channels`;
- serialização global de gravações.

## Arquitetura aprovada

Preservar como padrão:

- C# / .NET 10;
- ASP.NET Core;
- Razor Pages;
- HTML, CSS e JavaScript;
- EF Core;
- SQLite local na máquina hospedeira;
- aplicação web centralizada acessada pela LAN.

A direção é um monólito simples e fácil de manter.

Não introduzir sem decisão explícita:

- React, Angular ou Vue;
- SPA separada;
- microsserviços;
- Redis;
- RabbitMQ;
- MediatR;
- AutoMapper;
- CQRS framework;
- repository genérico;
- Unit of Work customizado;
- SignalR;
- Command Queue;
- infraestrutura distribuída.

## Núcleo de domínio atual

As primeiras entidades do produto são:

- `Document`;
- `Category`;
- `Tag`;
- `DocumentTag`;
- `DocumentVersion`.

Recursos posteriores incluem usuários, favoritos, recentes, anexos, lixeira, histórico, autosave, busca e backup.

## Regra de escopo

Trabalhe em uma tarefa de backlog por vez.

Faça tudo que for necessário para concluir a tarefa atual, mas não antecipe conscientemente a próxima sprint.

Se uma decisão alterar arquitetura, modelo de dados relevante, segurança, autenticação, UX principal ou deployment, pare e reporte a decisão necessária em vez de assumir silenciosamente.

## Git local

Antes de modificar arquivos:

```text
git branch --show-current
git status
```

Não descarte, sobrescreva nem incorpore silenciosamente alterações locais não relacionadas.

Quando o pivô ainda não estiver na branch local de trabalho, sincronize o repositório antes de implementar. Não continue usando planejamento antigo por estar presente no checkout local.

## Validação

Para mudanças .NET, execute no mínimo quando aplicável:

```text
dotnet restore ResetService.slnx
dotnet build ResetService.slnx -c Release
dotnet test ResetService.slnx -c Release
```

Acrescente testes específicos da tarefa quando necessários.

Não remova ou enfraqueça testes apenas para obter build verde.

## UI e UX

A simplicidade arquitetural não autoriza UI descuidada.

A interface deve priorizar:

- legibilidade;
- busca rápida;
- navegação previsível;
- estados vazios úteis;
- erros compreensíveis;
- feedback de salvamento;
- proteção de conteúdo não confirmado quando possível;
- ações destrutivas recuperáveis quando possível;
- boa hierarquia visual;
- desktop/notebook como ambiente principal;
- uso confortável a partir de aproximadamente 1366×768;
- comportamento coerente em Chrome e Edge.

Evite aparência de ERP administrativo complexo.

## Relatório final esperado

Ao concluir uma tarefa, informe de forma objetiva:

- tarefa executada;
- arquivos alterados;
- decisões locais relevantes;
- testes/comandos executados e resultado;
- branch e estado do working tree;
- qualquer bloqueio ou decisão pendente.
