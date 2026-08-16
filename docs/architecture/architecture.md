# Reset Service — Arquitetura

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Versão:** 2.0  
**Status:** Aprovado para implementação

## 1. Visão

O Reset Service será uma aplicação web local, centralizada e simples, acessada por navegadores na rede interna da Technolife.

```text
PCs da LAN
   ↓ HTTP/HTTPS
ResetService.Web
   ↓
EF Core
   ↓
SQLite local
```

A arquitetura deve favorecer manutenção, atualização e entendimento simples do código.

## 2. Topologia

```text
                 REDE LOCAL

PC Técnico 1 ─┐
PC Técnico 2 ─┼── navegador ──► Reset Service
Administração ┘                    │
                                   ├── aplicação web
                                   ├── banco SQLite
                                   ├── uploads
                                   └── backups
```

O banco permanece no disco local da máquina host. Estações nunca acessam diretamente o arquivo SQLite.

## 3. Stack

- C#;
- .NET 10;
- ASP.NET Core;
- Razor Pages;
- HTML e CSS;
- JavaScript nativo onde trouxer ganho de UX;
- Entity Framework Core;
- SQLite;
- autenticação local por cookie/Identity;
- Kestrel;
- Windows como ambiente de hospedagem.

## 4. Modelo de aplicação

A aplicação será um monólito simples.

A estrutura preferencial é um único projeto web de produção, além dos testes:

```text
src/
└── ResetService.Web/
    ├── Data/
    ├── Models/
    ├── Services/
    ├── Pages/
    ├── wwwroot/
    └── Program.cs

tests/
└── ResetService.Tests/
```

Os projetos `ResetService.Core` e `ResetService.Infrastructure` existentes são legado da fundação anterior. Durante o pivô, seus elementos úteis poderão ser incorporados ao projeto web para reduzir assemblies e abstrações sem benefício prático.

## 5. Frontend

Não haverá SPA separada.

A interface utilizará Razor Pages e progressive enhancement com JavaScript para recursos como:

- autosave;
- feedback de salvamento;
- editor rich text;
- menus e modais;
- busca rápida;
- filtros;
- cópia de comandos;
- atualização parcial quando fizer sentido.

Nenhum recurso obrigatório dependerá de CDN ou internet.

## 6. Persistência

A versão inicial utilizará:

```text
EF Core + SQLite
```

O arquivo operacional ficará apenas na máquina host.

Transações deverão ser curtas. A aplicação não precisa de uma fila global de escrita para o novo domínio documental.

## 7. Concorrência

O sistema será multiusuário, mas não terá edição colaborativa em tempo real na primeira versão.

Entidades mutáveis relevantes, principalmente `Document`, terão um campo de versão.

Fluxo esperado:

```text
Usuário A abre Version 4
Usuário B salva e gera Version 5
Usuário A tenta salvar Version 4
        ↓
conflito detectado
        ↓
nenhuma sobrescrita silenciosa
```

A interface deverá explicar o conflito e preservar o conteúdo local sempre que possível.

## 8. Recursos removidos da arquitetura anterior

Não são requisitos centrais da nova versão:

- SignalR;
- System.Threading.Channels / Command Queue;
- serialização global de comandos;
- OperationId genérico para toda mutação;
- workflow de execução de serviços;
- snapshots de conclusão;
- geração de PDF como parte do núcleo;
- sincronização contínua de navegadores.

Esses recursos só poderão ser reintroduzidos se uma necessidade real e mensurável aparecer.

## 9. Pesquisa

A pesquisa será parte central do produto.

Inicialmente poderá usar recursos do próprio SQLite/EF Core com normalização adequada para:

- título;
- resumo;
- tags;
- categoria;
- conteúdo textual.

A implementação deve aceitar evolução posterior sem exigir um serviço externo de busca no MVP.

## 10. Editor

O editor é uma das poucas áreas em que biblioteca client-side madura é preferível a implementação própria.

A escolha concreta será feita durante a Sprint de editor e deverá suportar conteúdo técnico, incluindo:

- headings;
- listas;
- checklist;
- código;
- links;
- imagens;
- tabelas simples;
- blocos de aviso.

O formato persistido deverá permitir renderização segura e versionamento previsível.

## 11. Arquivos e imagens

Uploads serão armazenados centralmente na máquina host, fora de caminhos públicos arbitrários.

O banco guardará metadados e relacionamento com documentos.

Backup deverá considerar banco e uploads como uma unidade operacional.

## 12. Segurança

A aplicação continuará utilizando princípios básicos obrigatórios:

- autenticação;
- autorização no backend;
- antiforgery;
- validação de entrada;
- cookies seguros;
- proteção de senha;
- logs sem segredos;
- controle de acesso a arquivos.

Documentações não devem ser usadas como cofre de senhas. Credenciais sensíveis deverão permanecer em solução apropriada e, quando necessário, ser apenas referenciadas pelo documento.

## 13. Implantação

A aplicação será publicada centralmente para Windows.

A máquina poderá ser desktop, notebook ou Windows Server, desde que permaneça disponível durante o uso.

A experiência desejada permanece:

```text
instalar/configurar uma vez
        ↓
iniciar serviço/aplicação
        ↓
usuários acessam pela LAN
```

Atualizações serão feitas apenas na máquina host.

## 14. Desempenho esperado

Escala alvo inicial:

- poucos usuários simultâneos;
- centenas ou milhares de documentos;
- operações curtas;
- leitura muito mais frequente que escrita.

O sistema deve priorizar abertura rápida, pesquisa ágil e interface responsiva, sem engenharia para escala hipotética.

## 15. Regra arquitetural

Toda nova dependência ou camada deverá justificar claramente qual problema real resolve.

A preferência é:

> menos componentes, menos processos e menos abstrações, mantendo integridade dos dados e boa experiência de uso.
