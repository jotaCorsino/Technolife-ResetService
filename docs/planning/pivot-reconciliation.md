# Reset Service — Reconciliação do Pivô

Este documento registra quais decisões do planejamento anterior continuam válidas após a mudança do produto para uma base interna de conhecimento técnico.

Seu objetivo é impedir dois erros:

1. carregar para a nova aplicação complexidade específica do antigo fluxo de serviços;
2. descartar requisitos transversais de operação, segurança, implantação e UX que continuam importantes.

## 1. Regra de interpretação

O pivô substitui o **domínio funcional central**, não toda a fundação do projeto.

```text
ANTIGO DOMÍNIO
Service / Template / Stage / Step / execução
→ descontinuado

REQUISITOS TRANSVERSAIS
LAN / Windows / segurança / backup / UX / desempenho
→ continuam válidos, adaptados ao novo produto
```

Em caso de conflito, esta reconciliação, `current-state.md`, `product-destination.md`, a arquitetura nova e o backlog novo prevalecem.

## 2. Requisitos preservados

### 2.1 Aplicação web centralizada

Continua obrigatório:

- uma única instalação central;
- acesso pelos computadores da empresa através do navegador;
- nenhuma instalação da aplicação nas estações clientes;
- dados centralizados;
- funcionamento normal sem dependência da internet.

### 2.2 Ambiente da máquina hospedeira

A máquina hospedeira poderá ser:

- desktop;
- notebook;
- Windows Server.

Windows Server não é obrigatório.

A direção continua sendo Windows x64, com validação real de Windows 10 e Windows 11 conforme compatibilidade do runtime adotado.

Notebook como host é permitido, desde que energia/suspensão/hibernação não interrompam o serviço durante o período operacional.

### 2.3 Clientes e navegadores

Prioridade oficial:

- Windows 10 + Chrome/Edge moderno;
- Windows 11 + Chrome/Edge moderno.

Windows 7/8/8.1 podem funcionar em regime de melhor esforço se houver navegador compatível.

Internet Explorer não é alvo.

Não sacrificar a plataforma moderna para browsers obsoletos, mas evitar JavaScript experimental e dependências modernas desnecessárias.

### 2.4 Rede e endereço

O uso cotidiano deve preferir um nome estável, por exemplo:

```text
https://resetservice/
```

Preferir DNS interno ou hostname estável em vez de exigir IP/porta memorizados.

Quando possível, usar HTTPS confiável na LAN e porta padrão.

### 2.5 Execução do host

A aplicação deve poder funcionar como Windows Service e iniciar sem login interativo.

A inicialização automática continua desejável para que reiniciar o host não exija abrir manualmente o programa.

### 2.6 Instalação física e dados

Preservar o princípio:

```text
Program Files
→ binários substituíveis

ProgramData
→ dados persistentes
```

Dados, banco, uploads, backups, logs, configuração e chaves não devem depender do diretório dos binários.

Atualizar a aplicação não pode destruir o estado persistente.

### 2.7 Atualizações

Atualizações continuam centralizadas:

```text
atualizar host
→ todos os navegadores passam a usar a nova versão
```

A atualização deve poder ocorrer offline, sem dependência de GitHub ou serviço cloud em produção.

Migrations devem ser controladas e preservar os dados existentes.

### 2.8 Escala esperada

Planejamento proporcional continua aproximadamente em:

- 1 a 20 usuários cadastrados;
- 1 a 10 usuários simultâneos;
- centenas ou milhares de documentos ao longo do tempo.

Não projetar cluster, alta disponibilidade ou escalabilidade horizontal para a v1.0.

### 2.9 Desempenho

A aplicação deve parecer rápida na LAN.

Metas de experiência continuam úteis como referência:

- interações comuns: aproximadamente abaixo de 500 ms em condições normais;
- conteúdo útil de telas comuns: aproximadamente até 2 segundos.

São metas de UX, não SLA rígido.

### 2.10 Multiusuário e concorrência

Continua obrigatório permitir vários usuários simultâneos.

Não é necessário Google-Docs-like real time.

Regras preservadas:

- nenhuma alteração recente pode ser sobrescrita silenciosamente;
- conflitos relevantes devem ser detectados;
- alterações de outros usuários devem tornar-se visíveis sem reiniciar a aplicação;
- propagação instantânea em milissegundos não é requisito.

Na nova aplicação, a direção principal é concorrência otimista por versão do documento. Polling leve ou atualização sob demanda pode ser usado se trouxer valor; SignalR não é requisito central.

### 2.11 Falhas de comunicação e conteúdo não salvo

A UI não deve apresentar sucesso antes da persistência confirmada.

Se a conexão com o servidor for perdida durante edição, a interface deve deixar isso claro e reduzir o risco de perda de conteúdo ainda não confirmado, usando preservação local temporária quando aplicável.

### 2.12 Autenticação e autoria

Continua válido:

- sem acesso anônimo às funções da aplicação;
- autenticação local, sem dependência obrigatória de Microsoft/Google/AD/cloud;
- contas individuais, evitando credenciais genéricas compartilhadas;
- usuários possuem identidade permanente mesmo se nome/login mudar;
- desativar usuário preserva autoria histórica;
- backend é autoridade de identidade e permissão.

A nomenclatura final dos perfis da Documentation Edition deve ser consolidada antes da implementação de autorização. Não criar uma matriz complexa de ACL na v1.0.

### 2.13 Primeiro administrador

Preservar o conceito de bootstrap da primeira conta administrativa em instalação nova.

A configuração inicial não deve deixar uma instalação sem administrador ativo válido.

O fluxo técnico definitivo será revisado na sprint de autenticação.

### 2.14 Segurança web

Permanecem como direção:

- ASP.NET Core Identity quando autenticação for implementada;
- password hashing do framework;
- cookies `HttpOnly` e `Secure` em produção;
- antiforgery para operações mutáveis;
- autorização real no backend;
- validação de entrada;
- HTTPS oficial na LAN;
- ausência de CORS aberto por conveniência;
- logs sem segredos;
- princípio de menor privilégio para arquivos e serviço Windows.

Regras específicas ligadas a SignalR/Command Queue deixam de valer.

### 2.15 Backup e restauração

Backup continua requisito essencial, não luxo arquitetural.

Preservar:

- backup manual;
- possibilidade de backup automático configurável;
- retenção simples;
- restauração testável;
- possibilidade de manter cópia fora do disco físico primário;
- banco e arquivos anexados incluídos quando aplicável.

Backup no mesmo disco não deve ser tratado como proteção suficiente contra falha física.

### 2.16 Logs e diagnóstico

Manter logs técnicos suficientes para diagnosticar:

- inicialização;
- falhas de persistência;
- erros inesperados;
- backup/restauração;
- atualização;
- versão da aplicação.

Log técnico e histórico funcional dos documentos são conceitos diferentes.

### 2.17 Desktop e resolução

Desktop/notebook continuam sendo o alvo principal.

A aplicação deve permanecer confortável a partir de aproximadamente 1366×768 e aproveitar adequadamente 1920×1080.

Responsividade básica é desejável, mas smartphone não é plataforma principal da v1.0.

### 2.18 Qualidade de UI/UX

O investimento em UI/UX permanece deliberadamente alto apesar da simplificação técnica.

Preservar princípios:

- não parecer ERP complexo;
- navegação previsível;
- ação principal evidente;
- pesquisa facilmente acessível;
- filtros sem poluir a tela;
- listas compactas e legíveis;
- conteúdo central com largura confortável;
- estados de loading, vazio, erro, conflito e conexão projetados;
- feedback visual imediato;
- ações destrutivas reversíveis sempre que possível.

## 3. Requisitos adaptados ao novo domínio

### Pesquisa

Antes buscava serviços; agora deve localizar conhecimento técnico por título, resumo, conteúdo, categoria, tags e tipo.

### Histórico

Antes registrava ciclo de vida de serviços; agora protege versões de documentos, autoria e restaurações.

### Templates

Antes eram modelos executáveis de serviço; agora são estruturas opcionais para acelerar criação de Procedimento, Troubleshooting, Configuração, Checklist e outros documentos.

### Multiusuário

Antes vários técnicos executavam o mesmo roteiro; agora vários usuários consultam e eventualmente editam a mesma base de conhecimento.

### Dashboard/Home

Antes mostrava situação operacional de serviços; agora deve priorizar busca, favoritos, recentes, categorias e acesso rápido à documentação.

### Documentos

Antes PDFs eram saída do serviço; agora o documento técnico é o próprio conteúdo central do produto. PDF/exportação pode ser recurso posterior, não o núcleo.

## 4. Requisitos removidos de propósito

Não fazem mais parte do produto atual salvo nova decisão explícita:

- `Service` como entidade central;
- número `RS-AAAA-NNNNN`;
- clientes/equipamentos dentro de um Service;
- `ServiceTemplate` e revisões publicadas obrigatórias;
- cópia de roteiro para cada Service;
- `Stage` / `Step` operacionais;
- Pending / Completed / NotApplicable de execução;
- progresso de serviço;
- estados Draft/InProgress/Waiting/Completed/Cancelled de Service;
- conclusão/reabertura de Service;
- observações internas/cliente de Service;
- snapshots de conclusão;
- PDFs de Registro Interno e Relatório de Serviço;
- Command Queue global;
- `System.Threading.Channels` como infraestrutura do produto;
- SignalR como requisito de sincronização central;
- OperationId generalizado para comandos do antigo fluxo.

## 5. Novo núcleo funcional

O produto atual deve concentrar esforço em:

- criação e leitura de documentação técnica;
- editor adequado a conteúdo técnico;
- tipos de documento e templates;
- categorias/subcategorias e tags;
- busca global;
- duplicação;
- favoritos e recentes;
- histórico de versões;
- autosave/preservação de rascunho;
- lixeira e restauração;
- concorrência otimista;
- anexos/imagens quando implementados;
- backup;
- ótima experiência visual e de uso.

## 6. Regra para Codex

Codex deve distinguir:

```text
LEGADO DE DOMÍNIO
→ não implementar

REQUISITO TRANSVERSAL PRESERVADO
→ manter como restrição do produto

NOVO NÚCLEO DOCUMENTAL
→ implementar conforme backlog vigente
```

Ao encontrar uma decisão antiga que não esteja classificada aqui e que altere arquitetura, segurança, implantação, modelo de dados ou UX principal, não assumir silenciosamente. Registrar a dúvida como decisão necessária.
