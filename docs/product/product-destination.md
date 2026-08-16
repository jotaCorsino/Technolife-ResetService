# Reset Service — Destino do Produto

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Versão:** 2.0  
**Status:** Aprovado para implementação

## 1. Objetivo

O Reset Service será uma aplicação web interna para criação, organização, consulta e manutenção de documentação técnica usada pela equipe da Technolife.

Sua função principal é reduzir o tempo necessário para encontrar, compreender e executar corretamente tarefas recorrentes de assistência técnica, infraestrutura, redes e service desk.

## 2. Escopo operacional

A base deverá comportar conteúdos como:

- formatação e preparação de computadores;
- configuração de Windows e softwares;
- troubleshooting de estações e redes;
- switches, roteadores, Wi-Fi e VPN;
- firewalls, NAT e políticas;
- Windows Server, Linux, Active Directory, arquivos e backup;
- DNS;
- e-mail e Microsoft 365;
- hospedagem, SSL, FTP e migrações;
- impressoras;
- rotinas de service desk;
- padrões internos e referências técnicas.

## 3. Experiência principal

```text
Abrir navegador
      ↓
Pesquisar ou navegar
      ↓
Abrir documentação
      ↓
Ler procedimento / copiar comando / seguir checklist
      ↓
Executar tarefa
```

Para registrar conhecimento:

```text
Nova documentação
      ↓
Escolher tipo e categoria
      ↓
Escrever ou duplicar conteúdo existente
      ↓
Salvar
      ↓
Conteúdo disponível para a equipe
```

## 4. Tipos de documento

A primeira versão utilizará:

- Procedimento;
- Troubleshooting;
- Configuração;
- Checklist;
- Referência;
- Documento livre.

Templates poderão sugerir estruturas adequadas para cada tipo sem impedir edição livre.

## 5. Organização

Documentos serão localizados por:

- pesquisa global;
- categorias e subcategorias;
- tags;
- tipo;
- favoritos;
- recentes.

A pesquisa deverá ser uma função de primeira classe e não depender de o usuário conhecer previamente a estrutura de categorias.

## 6. Conteúdo técnico

O editor deverá tratar bem:

- títulos e subtítulos;
- listas e listas numeradas;
- checklists;
- links;
- imagens;
- tabelas simples;
- blocos de código e comandos;
- avisos, observações e dicas.

O modo de leitura deverá priorizar legibilidade e permitir copiar comandos com facilidade.

## 7. Segurança contra perda de informação

A aplicação deverá oferecer progressivamente:

- salvamento confiável;
- autosave;
- histórico de versões;
- restauração sem apagar versões posteriores;
- lixeira;
- backup;
- conflito de edição detectado por versão.

Excluir normalmente significa mover para a lixeira. Exclusão definitiva será administrativa.

## 8. Usuários

A primeira versão terá dois perfis simples:

```text
User
Administrator
```

Usuários comuns poderão consultar e manter documentação conforme regras definidas no produto. Administradores terão acesso a usuários, categorias, lixeira e configurações.

Permissões mais granulares ficam fora do MVP até existir necessidade real.

## 9. Hospedagem e acesso

O sistema será centralizado em uma única máquina Windows compatível, que poderá ser desktop, notebook ou Windows Server.

```text
Máquina host
├── Reset Service
├── SQLite
├── uploads
└── backups
```

As estações acessarão somente pelo navegador na LAN. Não haverá instalação local do sistema nos computadores dos usuários.

## 10. Independência da internet

Operações normais deverão funcionar sem internet:

- login;
- pesquisa;
- leitura;
- criação e edição;
- categorias e tags;
- histórico;
- lixeira;
- backup e restauração.

Nenhum asset obrigatório dependerá de CDN.

## 11. Multiusuário

Vários usuários poderão acessar a aplicação simultaneamente.

Não haverá edição colaborativa estilo Google Docs na primeira versão.

A proteção principal contra sobrescrita será concorrência otimista por versão do documento. Avisos de edição simultânea poderão ser adicionados sem introduzir infraestrutura distribuída.

## 12. UX e UI

Design é requisito funcional do produto.

A interface deverá priorizar:

- leitura confortável;
- hierarquia visual clara;
- pesquisa evidente;
- navegação previsível;
- feedback de salvamento;
- estados vazios úteis;
- mensagens de erro acionáveis;
- ações destrutivas reversíveis;
- baixa carga cognitiva;
- bom uso em desktops e notebooks comuns.

A aplicação não deverá ter aparência ou comportamento de ERP complexo.

## 13. Limites da primeira versão

Ficam fora inicialmente:

- execução operacional de serviços por etapas;
- CRM;
- inventário;
- ticketing;
- financeiro;
- chat;
- comentários complexos;
- workflow de aprovação;
- SignalR como requisito central;
- Command Queue;
- edição colaborativa em tempo real;
- aplicativo mobile nativo;
- integrações externas obrigatórias;
- API pública;
- IA;
- assinatura digital;
- analytics avançado.

## 14. Critério de sucesso

A primeira versão interna será considerada útil quando um funcionário conseguir:

```text
acessar pela LAN
↓
fazer login
↓
pesquisar conhecimento
↓
abrir e ler uma documentação
↓
criar ou duplicar uma documentação
↓
editar e organizar
↓
salvar sem perda
↓
outro usuário consultar o conteúdo
↓
recuperar conteúdo excluído ou versão anterior
```

## 15. Estratégia de evolução

O produto será desenvolvido em incrementos pequenos.

A prioridade será colocar uma versão funcional em uso interno cedo e usar o atrito observado no trabalho real para decidir as próximas melhorias.

Nova funcionalidade deverá responder positivamente a pelo menos uma destas perguntas:

- ajuda a encontrar conhecimento?
- ajuda a criar ou manter conhecimento?
- ajuda a compreender ou executar uma tarefa técnica?
- ajuda a proteger informação importante?

Caso contrário, não pertence ao MVP.
