# Reset Service — UX Navigation Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Navegação e Experiência Principal  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`, `service-template-spec.md`, `service-data-spec.md`, `document-generation-spec.md`, `user-access-spec.md`

---

## 1. Objetivo

Este documento define a estrutura funcional de navegação e a experiência principal do Reset Service.

Seu escopo inclui:

- navegação global;
- Dashboard;
- listagem de serviços;
- criação de serviço;
- execução do roteiro;
- visualização de detalhes;
- histórico;
- documentos;
- modelos;
- configurações;
- estados visuais;
- princípios gerais de interação.

Este documento não define framework, biblioteca visual, CSS, componentes técnicos ou implementação.

---

## 2. Princípio de experiência

O Reset Service deverá funcionar prioritariamente como um roteiro operacional.

A interface deverá comunicar de forma simples:

- onde o usuário está;
- o que precisa ser feito;
- o que já foi concluído;
- o que ainda está pendente;
- qual é a próxima ação relevante.

O sistema não deverá transmitir a sensação de um ERP administrativo complexo.

---

## 3. Navegação principal

A navegação global deverá possuir somente quatro áreas principais:

```text
Dashboard
Serviços
Modelos
Configurações
```

Outras funcionalidades deverão ser organizadas dentro dessas áreas.

---

## 4. Estrutura geral

```text
RESET SERVICE
│
├── Dashboard
│
├── Serviços
│   ├── Todos
│   ├── Rascunhos
│   ├── Em andamento
│   ├── Aguardando
│   ├── Concluídos
│   └── Cancelados
│
├── Modelos
│   ├── Ativos
│   ├── Rascunhos
│   └── Arquivados
│
└── Configurações
    ├── Empresa
    ├── Documentos
    ├── Usuários
    └── Sistema
```

A disponibilidade das áreas e ações deverá respeitar as permissões do usuário autenticado.

---

## 5. Navegação global em desktop

A versão 1.0 terá desktop e notebook como ambientes prioritários.

A navegação poderá utilizar uma barra lateral persistente contendo:

- identidade do Reset Service;
- Dashboard;
- Serviços;
- Modelos;
- Configurações, quando permitidas;
- usuário autenticado;
- perfil;
- ação de saída.

Exemplo conceitual:

```text
┌──────────────────┬────────────────────────────────────────────┐
│ RESET SERVICE    │                                            │
│                  │                                            │
│ Dashboard        │                  Conteúdo                  │
│ Serviços         │                                            │
│ Modelos          │                                            │
│ Configurações    │                                            │
│                  │                                            │
│ João da Silva    │                                            │
│ Técnico          │                                            │
│ Sair             │                                            │
└──────────────────┴────────────────────────────────────────────┘
```

---

## 6. Dashboard

O Dashboard deverá ser a primeira tela após autenticação.

Sua função principal será apresentar a situação operacional atual.

Deverá permitir identificar rapidamente:

- quantidade de serviços Em andamento;
- quantidade de serviços Aguardando;
- serviços concluídos recentemente;
- serviços ativos;
- progresso;
- etapa atual;
- responsável;
- acesso direto ao serviço.

O Dashboard não deverá priorizar gráficos ou análises complexas.

---

## 7. Ação Novo Serviço

A criação de serviço será uma das principais ações da aplicação.

O Dashboard e a área de Serviços deverão possuir acesso evidente a:

```text
+ Novo serviço
```

O usuário não deverá precisar navegar por vários menus para iniciar um serviço.

---

## 8. Representação de serviços

Serviços ativos poderão ser apresentados em itens com aparência de cards compactos.

Cada item deverá comunicar, quando disponível:

- ID;
- título;
- cliente ou empresa;
- equipamento;
- estado;
- responsável;
- progresso;
- etapa atual;
- última atualização.

O item deverá fornecer acesso direto ao serviço.

---

## 9. Área de Serviços

A tela Serviços deverá centralizar a consulta operacional e histórica.

Estrutura conceitual:

```text
SERVIÇOS

[ Buscar por ID, cliente, equipamento... ]

[ Todos ] [ Rascunhos ] [ Em andamento ]
[ Aguardando ] [ Concluídos ] [ Cancelados ]

[ Filtros ]                         [ + Novo serviço ]

---------------------------------------------------

Lista de serviços
```

---

## 10. Pesquisa

A pesquisa deverá permanecer facilmente acessível.

Deverá permitir localizar serviços pelas informações definidas em `service-data-spec.md`.

Pesquisa textual e filtros estruturados serão mecanismos distintos.

---

## 11. Filtros

Filtros poderão incluir:

- estado;
- responsável;
- modelo;
- período.

Os filtros adicionais deverão permanecer recolhidos quando não estiverem sendo utilizados, evitando excesso de controles na tela.

---

## 12. Visualização da lista

A versão 1.0 não precisará oferecer múltiplos modos de exibição como grade e tabela.

Será utilizada uma única visualização responsiva em formato de lista-card, adequada para leitura rápida.

---

## 13. Criação de Serviço

A criação será dividida conceitualmente em duas etapas simples.

---

## 14. Seleção de modelo

Primeiro o usuário deverá escolher um modelo Ativo.

Cada modelo poderá apresentar:

- nome;
- revisão atual;
- quantidade de etapas;
- quantidade de passos.

Somente conteúdo publicado poderá ser utilizado.

---

## 15. Informações básicas

Depois da escolha do modelo, poderão ser preenchidas informações opcionais como:

- título;
- responsável;
- cliente;
- empresa;
- telefone;
- e-mail;
- referência;
- equipamento;
- demais dados previstos em `service-data-spec.md`.

A ausência de dados opcionais não deverá impedir a criação.

---

## 16. Criação rápida

Como o modelo é a única escolha manual indispensável, o usuário poderá realizar:

```text
Novo serviço
      ↓
Escolher modelo
      ↓
Criar serviço
```

sem precisar preencher um formulário extenso.

---

## 17. Destino após criação

Depois de criado, o serviço deverá ser aberto imediatamente.

O usuário não será redirecionado novamente para a listagem.

O novo serviço estará em Rascunho.

---

## 18. Tela do Serviço

A tela do serviço deverá manter três áreas conceituais principais:

```text
Cabeçalho do serviço

Navegação / progresso

Conteúdo atual
```

O cabeçalho deverá manter o contexto do registro mesmo quando o usuário navegar internamente por suas diferentes áreas.

---

## 19. Cabeçalho do Serviço

Deverá apresentar, quando relevante:

- ID;
- título;
- estado;
- progresso;
- cliente/empresa;
- equipamento;
- responsável;
- ações do ciclo de vida.

Exemplo:

```text
RS-2026-00142            EM ANDAMENTO        72%

Formatação — Cliente Contrato
Empresa ABC • Dell Latitude 5420

Responsável: João
```

---

## 20. Navegação interna

A tela do serviço terá quatro áreas principais:

```text
Roteiro
Detalhes
Histórico
Documentos
```

---

## 21. Roteiro

Será a área principal da execução.

Ao abrir um serviço, o Roteiro deverá ser a visualização padrão.

Isso vale especialmente para serviços:

- Rascunho;
- Em andamento;
- Aguardando.

Serviços Concluídos e Cancelados poderão manter a mesma estrutura em modo somente leitura.

---

## 22. Metáfora da folha

Cada Etapa será apresentada como uma página ou folha individual.

A metáfora deverá ser visual, mas não literal.

Devem ser evitados elementos decorativos exagerados como:

- espirais;
- texturas de papel;
- linhas de caderno;
- efeitos 3D intensos.

O objetivo é comunicar:

> uma página, uma etapa, um conjunto de ações.

---

## 23. Área central da etapa

Mesmo em telas largas, a etapa deverá possuir largura moderada para melhorar:

- leitura;
- foco;
- hierarquia;
- execução.

O roteiro não deverá ocupar indiscriminadamente toda a largura disponível.

---

## 24. Navegação das etapas

As etapas deverão possuir uma navegação superior que também comunique progresso.

Exemplo:

```text
✓━━━━✓━━━━●━━━━○━━━━○
1    2    3    4    5
```

A navegação permitirá seleção direta de uma etapa.

---

## 25. Roteiros longos

A navegação deverá continuar utilizável quando houver muitas etapas.

O design poderá utilizar mecanismos como:

- rolagem horizontal;
- paginação visual;
- janela de etapas próximas;
- controles anterior/próximo.

A implementação não deverá presumir um limite visual pequeno de etapas.

---

## 26. Identificação da etapa

A página deverá apresentar claramente:

- posição atual;
- total de etapas;
- título;
- descrição, quando existente.

Exemplo:

```text
ETAPA 3 DE 8

Configuração do Sistema
```

---

## 27. Passos

Os passos deverão funcionar prioritariamente como checklist.

Exemplo:

```text
☐ Instalar Microsoft Office
```

Após conclusão:

```text
☑ Instalar Microsoft Office
```

A alteração deverá fornecer feedback visual imediato.

---

## 28. Instruções do passo

Passos poderão apresentar descrição ou instruções.

Textos curtos poderão permanecer visíveis.

Textos longos poderão utilizar apresentação expansível para preservar a limpeza da página.

---

## 29. Ações secundárias do passo

Ações menos frequentes poderão ser concentradas em um menu discreto.

Exemplo:

```text
☐ Instalar Microsoft Office                         ⋯
```

O menu poderá disponibilizar, conforme permissões e estado:

- adicionar observação;
- marcar como Não aplicável;
- editar;
- mover;
- remover.

---

## 30. Não Aplicável

A marcação de Não aplicável não precisará competir visualmente com o checkbox principal.

Poderá ser uma ação secundária.

Após aplicada, deverá ser claramente identificável.

Exemplo:

```text
— Configurar impressora
  Não aplicável
```

---

## 31. Observações

Observações poderão ser adicionadas ao:

- serviço;
- etapa;
- passo.

A interface deverá manter clara a associação de cada observação.

---

## 32. Criação de observação

Ao criar uma observação, o usuário deverá indicar sua visibilidade.

Possibilidades:

- Interna;
- Cliente.

Quando for destinada ao cliente, poderá também ser:

- Informação;
- Recomendação.

---

## 33. Visualização de observações

Observações existentes deverão ser exibidas de maneira compacta, contendo:

- autor;
- data/hora;
- visibilidade;
- conteúdo.

A apresentação não deverá transformar a página do roteiro em um sistema de chat.

---

## 34. Observações de etapa

A parte inferior da folha poderá possuir seção própria:

```text
Observações da etapa

+ Adicionar observação
```

---

## 35. Detalhes

A área Detalhes deverá concentrar as informações cadastrais e operacionais.

Poderá ser dividida em:

- Identificação;
- Cliente;
- Equipamento;
- Operação;
- Origem.

Quando o estado permitir, deverá existir ação explícita para edição.

---

## 36. Histórico

O Histórico deverá utilizar apresentação cronológica simples.

Exemplo:

```text
13/08/2026 08:31
Serviço criado por Carlos

13/08/2026 08:44
Serviço iniciado por João

13/08/2026 10:02
Serviço colocado em espera por João
Motivo: aguardando licença.
```

O foco será em acontecimentos relevantes, e não em cada interação com a interface.

---

## 37. Documentos

A área Documentos deverá organizar os PDFs por conclusão.

Exemplo:

```text
Conclusão 2 — Atual

Registro Interno
[ Visualizar ] [ Gerar PDF ]

Relatório de Serviço
[ Visualizar ] [ Gerar PDF ]

-----------------------------

Conclusão 1

Registro Interno
Relatório de Serviço
```

Conclusões anteriores deverão permanecer acessíveis.

---

## 38. Ações do ciclo de vida

Ações como:

- iniciar;
- aguardar;
- retomar;
- concluir;
- cancelar;
- reabrir;

deverão permanecer visualmente separadas das ações de checklist.

Elas poderão ser apresentadas no cabeçalho do serviço.

---

## 39. Estado Rascunho

No estado Rascunho:

- roteiro estará visível;
- checklist estará bloqueado;
- personalização estará disponível;
- ação Iniciar serviço deverá receber destaque.

A interface poderá comunicar:

> Inicie o serviço para começar a executar o roteiro.

---

## 40. Estado Em andamento

Nesse estado, as ações operacionais deverão ficar disponíveis.

Ações principais poderão incluir:

```text
[ Aguardar ] [ Concluir ]
```

Cancelamento deverá permanecer como ação secundária.

---

## 41. Estado Aguardando

A interface deverá deixar clara a pausa operacional e seu motivo.

Exemplo:

```text
AGUARDANDO

Aguardando licença do cliente.
```

A principal ação será:

```text
[ Retomar serviço ]
```

O checklist deverá permanecer visível, porém bloqueado.

---

## 42. Estado Concluído

O serviço será apresentado em modo somente leitura.

Deverá exibir:

- status;
- data/hora da conclusão;
- roteiro final;
- documentos.

Administradores poderão possuir acesso à ação Reabrir.

---

## 43. Estado Cancelado

Deverá ser claramente identificado.

O motivo do cancelamento deverá ficar acessível.

O roteiro será apresentado em modo somente leitura.

Administradores poderão reabrir o serviço.

---

## 44. Personalização

A customização estrutural não deverá competir com o fluxo normal de execução.

Ações como:

- adicionar etapa;
- reorganizar roteiro;
- adicionar passo;
- editar etapa;

deverão ser acessadas explicitamente.

A execução normal deve continuar visualmente prioritária.

---

## 45. Intenção de edição

Alterações estruturais devem exigir intenção clara.

O usuário não deverá entrar acidentalmente em edição estrutural enquanto tenta apenas executar checklists.

---

## 46. Modelos

A área Modelos deverá permitir consultar:

- Ativos;
- Rascunhos;
- Arquivados.

Cada modelo poderá apresentar:

- nome;
- estado;
- revisão atual;
- quantidade de etapas;
- quantidade de passos.

---

## 47. Ações em Modelos

Técnicos poderão:

- visualizar;
- utilizar modelos Ativos.

Administradores poderão adicionalmente:

- criar;
- editar;
- publicar;
- duplicar;
- arquivar;
- reativar;
- consultar revisões.

A interface deverá ocultar ações não permitidas pelo perfil.

---

## 48. Editor de Modelo

O editor deverá reutilizar a linguagem visual do roteiro.

Se uma etapa será uma folha durante a execução, sua edição deverá representar essa mesma unidade.

Exemplo:

```text
EDITAR MODELO

1   2   3   4   5

┌──────────────────────────────┐
│ ETAPA 3                     │
│ Configuração                │
│                             │
│ ≡ Drivers                   │
│ ≡ Windows Update            │
│ ≡ Office                    │
│                             │
│ + Adicionar passo           │
└──────────────────────────────┘

+ Adicionar etapa
```

---

## 49. Alterações não publicadas

Quando existir rascunho de uma nova revisão, a interface deverá informar claramente.

Exemplo:

```text
Revisão atual: 5

● Alterações não publicadas

[ Descartar ] [ Publicar nova revisão ]
```

---

## 50. Revisões do Modelo

O histórico de revisões deverá permitir identificar:

- número;
- revisão atual;
- data;
- autor da publicação;
- resumo das alterações;
- ação de visualização.

Revisões antigas serão apresentadas em modo somente leitura.

---

## 51. Configurações

Configurações serão organizadas em categorias:

```text
Empresa
Documentos
Usuários
Sistema
```

A interface poderá utilizar navegação interna por abas ou menu secundário.

---

## 52. Empresa

A área Empresa deverá reunir informações como:

- logo;
- nome;
- nome empresarial;
- CNPJ;
- telefone;
- e-mail;
- site;
- endereço.

---

## 53. Documentos

A área de documentos deverá permitir configuração controlada de:

- cabeçalho;
- rodapé;
- informações institucionais;
- texto padrão de conclusão;
- outros elementos previstos em `document-generation-spec.md`.

Uma pré-visualização deverá ser disponibilizada quando útil.

---

## 54. Usuários

A área Usuários será exclusiva do Administrador.

Deverá permitir:

- listar usuários;
- criar;
- editar;
- redefinir senha;
- desativar;
- reativar.

Status e perfil deverão ser claramente visíveis.

---

## 55. Minha Conta

Todo usuário deverá possuir acesso à própria conta através da área de perfil.

Deverá ser possível visualizar:

- nome;
- nome de acesso;
- perfil;

e realizar:

- alteração da própria senha;
- encerramento da sessão.

---

## 56. Estados vazios

A interface deverá possuir estados vazios úteis.

Exemplo:

```text
Nenhum serviço em andamento.

[ + Novo serviço ]
```

Para Administradores:

```text
Nenhum modelo disponível.

[ + Criar modelo ]
```

Estados vazios devem orientar a próxima ação possível.

---

## 57. Feedback de sucesso

Ações simples deverão gerar feedback breve e não intrusivo.

Exemplos:

```text
✓ Serviço criado
```

```text
✓ Observação adicionada
```

```text
✓ Revisão publicada
```

---

## 58. Confirmações

Modais de confirmação deverão ser reservados para ações relevantes.

Exemplos:

- cancelar serviço;
- reabrir serviço;
- descartar rascunho;
- arquivar modelo;
- desativar usuário.

Ações triviais não deverão exigir confirmações repetitivas.

---

## 59. Mensagens de erro

As mensagens deverão:

- explicar o problema;
- utilizar linguagem clara;
- indicar, quando possível, como resolver.

Exemplo adequado:

```text
Não foi possível concluir o serviço.

Ainda existem 2 passos pendentes.
```

Erros técnicos genéricos não devem ser apresentados como única explicação ao usuário.

---

## 60. Responsividade

Desktop e notebook são prioridade.

A aplicação deverá permanecer funcional em diferentes resoluções usuais de computadores da empresa.

A versão 1.0 não terá como objetivo oferecer uma experiência mobile completa.

---

## 61. Navegação por teclado

O produto deverá respeitar comportamentos básicos de teclado e acessibilidade.

São esperados pelo menos:

- Tab;
- Enter;
- acionamento de checkbox por teclado;
- foco visível.

Atalhos avançados não são requisito inicial.

---

## 62. Aparência

A identidade visual do Reset Service deverá ser:

- moderna;
- minimalista;
- profissional;
- limpa;
- legível;
- focada.

Deverão ser priorizados:

- espaços adequados;
- cards;
- bordas suaves;
- hierarquia tipográfica clara;
- iconografia simples;
- uso controlado de cor.

---

## 63. Elementos a evitar

A interface deverá evitar:

- excesso de gradientes;
- efeitos tridimensionais;
- animações longas;
- cores excessivas;
- telas excessivamente densas;
- aparência de sistema administrativo legado.

---

## 64. Estados e cores

Cores poderão reforçar estados, mas não deverão ser a única forma de identificação.

Estado deverá utilizar também:

- texto;
- ícone;
- símbolo;
- forma.

Isso melhora acessibilidade e compreensão.

---

## 65. Animações

Microanimações discretas poderão existir para:

- checkboxes;
- progresso;
- abertura de painéis;
- transições rápidas.

Nenhuma animação deverá atrasar a operação.

---

## 66. Progresso

Na execução, o progresso deverá permanecer constantemente identificável.

O usuário deve conseguir responder rapidamente:

- em qual etapa estou;
- quantas etapas existem;
- quanto já foi concluído;
- quanto ainda falta.

---

## 67. Densidade de informação

A tela operacional deverá apresentar prioritariamente informações necessárias à execução atual.

Informações secundárias permanecerão acessíveis através das áreas:

- Detalhes;
- Histórico;
- Documentos.

Não deverá ser apresentada toda a informação do serviço simultaneamente.

---

## 68. Fluxo principal

A experiência principal será:

```text
Login
 ↓
Dashboard
 ↓
Novo serviço
 ↓
Escolher modelo
 ↓
Informações básicas
 ↓
Criar
 ↓
Rascunho
 ↓
Iniciar
 ↓
Roteiro
 ↓
Executar etapas
 ↓
Revisão final
 ↓
Concluir
 ↓
Documentos
 ↓
Histórico
```

Esse fluxo deverá orientar decisões futuras de design.

---

## 69. Mapa de telas

```text
Login
│
├── Configuração inicial
│   └── Primeiro Administrador
│
└── Aplicação
    │
    ├── Dashboard
    │
    ├── Serviços
    │   ├── Lista
    │   ├── Novo serviço
    │   └── Serviço
    │       ├── Roteiro
    │       ├── Detalhes
    │       ├── Histórico
    │       └── Documentos
    │
    ├── Modelos
    │   ├── Lista
    │   ├── Novo modelo
    │   └── Modelo
    │       ├── Estrutura
    │       └── Revisões
    │
    ├── Configurações
    │   ├── Empresa
    │   ├── Documentos
    │   ├── Usuários
    │   └── Sistema
    │
    └── Minha conta
        └── Alterar senha
```

---

## 70. Regras Fundamentais

1. A navegação principal terá Dashboard, Serviços, Modelos e Configurações.
2. Dashboard será operacional e simples.
3. Novo Serviço será uma ação prioritária.
4. Busca deverá possuir destaque na área de Serviços.
5. A criação de serviço deverá exigir poucos passos.
6. O roteiro utilizará a metáfora visual de páginas.
7. Cada etapa corresponde visualmente a uma página.
8. A navegação das etapas também comunica progresso.
9. O usuário poderá navegar livremente entre etapas.
10. A conclusão de etapa não deverá causar avanço automático.
11. O Roteiro será a visualização padrão de um serviço.
12. A tela de serviço terá Roteiro, Detalhes, Histórico e Documentos.
13. O cabeçalho do serviço preservará contexto entre essas áreas.
14. Ações de ciclo de vida ficarão separadas das ações de checklist.
15. Personalizações estruturais deverão exigir intenção clara.
16. Modelos e serviços deverão compartilhar linguagem visual.
17. O editor de modelos deverá representar as mesmas unidades visuais utilizadas durante a execução.
18. Configurações serão organizadas por categoria.
19. Feedbacks simples deverão evitar modais.
20. Confirmações serão reservadas para ações de impacto.
21. O progresso permanecerá visível durante a execução.
22. Estados não dependerão somente de cores.
23. Desktop e notebook serão prioridade.
24. A interface deverá privilegiar rapidez, clareza e baixa densidade visual.
25. O Reset Service não deverá possuir aparência de ERP tradicional.

---

## 71. Estado da Decisão

**PLANNING-008 — Navegação, Estrutura de Telas e Experiência Principal: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência para futuras decisões de design visual, frontend, acessibilidade, arquitetura e testes de experiência.