# Reset Service — Document Generation Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Relatórios e Documentos PDF  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`, `service-template-spec.md`, `service-data-spec.md`

---

## 1. Objetivo

Este documento define o comportamento funcional dos documentos gerados pelo Reset Service.

Seu escopo inclui:

- Registro Interno de Serviço;
- Relatório de Serviço destinado ao cliente;
- origem das informações;
- conteúdo permitido;
- separação entre informações internas e externas;
- geração;
- regeneração;
- pré-visualização;
- histórico documental;
- comportamento após reabertura;
- identidade institucional da Technolife.

Não são definidos neste documento bibliotecas, mecanismos técnicos de PDF, armazenamento físico ou arquitetura.

---

## 2. Documentos oficiais

A versão 1.0 possuirá dois documentos principais:

```text
Serviço
│
├── Registro Interno de Serviço
└── Relatório de Serviço
```

### Registro Interno de Serviço

Documento destinado ao controle e histórico interno da Technolife.

### Relatório de Serviço

Documento destinado à apresentação do trabalho realizado ao cliente.

---

## 3. Fonte única de dados

Os documentos deverão ser derivados das informações já registradas no serviço.

O usuário não deverá preencher novamente dados já existentes apenas para gerar um documento.

```text
Serviço
├── identificação
├── cliente
├── equipamento
├── roteiro
├── observações
├── histórico
└── conclusão
       │
       ├── Registro Interno
       └── Relatório do Cliente
```

O serviço armazenado no Reset Service será a fonte de verdade.

---

## 4. Identificação dos documentos

Os títulos padrão serão:

**REGISTRO INTERNO DE SERVIÇO**

e:

**RELATÓRIO DE SERVIÇO**

Ambos deverão utilizar o ID do serviço como principal identificação documental.

Exemplo:

```text
RS-2026-00142
```

Não será criado um segundo número de controle independente para cada PDF.

---

## 5. Relação com a conclusão

Os documentos finais estarão relacionados a uma conclusão do serviço.

Um serviço poderá possuir mais de uma conclusão histórica.

```text
RS-2026-00142
│
├── Conclusão 1
│   ├── Registro Interno
│   └── Relatório de Serviço
│
└── Conclusão 2
    ├── Registro Interno
    └── Relatório de Serviço
```

---

## 6. Fotografia histórica

Cada conclusão deverá representar uma fotografia lógica do serviço naquele momento.

Essa fotografia deverá preservar as informações necessárias para que os documentos referentes àquela conclusão possam continuar representando fielmente seu estado histórico.

Poderão fazer parte dessa fotografia:

- dados do serviço;
- dados do cliente;
- dados do equipamento;
- roteiro executado;
- estados dos passos;
- observações;
- responsável;
- datas;
- identidade institucional aplicável;
- configurações documentais relevantes.

A estratégia técnica será definida posteriormente.

---

## 7. Reabertura

A reabertura de um serviço não deverá alterar nem apagar a conclusão anterior.

```text
Conclusão 1
    ↓
Reabertura
    ↓
Nova execução
    ↓
Conclusão 2
```

Os documentos referentes à Conclusão 1 permanecerão historicamente identificáveis.

---

## 8. Regeneração

Regenerar um documento não representa uma nova conclusão.

Se nenhum dado histórico daquela conclusão mudou:

```text
Gerar novamente
≠
Nova conclusão
```

A regeneração deverá produzir o mesmo conteúdo lógico correspondente à conclusão selecionada.

---

## 9. Identidade institucional

Os documentos deverão utilizar informações cadastradas da Technolife.

Poderão incluir:

- logo;
- nome da empresa;
- nome empresarial;
- CNPJ;
- telefone;
- e-mail;
- site;
- endereço.

Essas informações serão administradas centralmente.

---

## 10. Cabeçalho

O cabeçalho deverá utilizar layout padronizado.

Poderá conter:

```text
[ LOGO ]

TECHNOLIFE

REGISTRO INTERNO DE SERVIÇO

Serviço: RS-2026-00142
Data: 13/08/2026
```

ou:

```text
[ LOGO ]

TECHNOLIFE

RELATÓRIO DE SERVIÇO

Serviço: RS-2026-00142
Data: 13/08/2026
```

O design definitivo será definido posteriormente.

---

## 11. Configuração do cabeçalho

O administrador poderá controlar quais informações institucionais são exibidas.

Exemplo:

```text
Cabeçalho

☑ Logo
☑ Nome
☑ Telefone
☑ E-mail
☐ CNPJ
```

A configuração utilizará opções controladas.

Não haverá editor livre de layout semelhante a um editor de documentos.

---

## 12. Rodapé

O rodapé poderá conter:

- nome da Technolife;
- telefone;
- e-mail;
- site;
- endereço;
- CNPJ;
- numeração de páginas.

Exemplo:

```text
Technolife • contato@empresa.com • (XX) XXXX-XXXX
Página 2 de 4
```

---

## 13. Elementos automáticos

Algumas informações serão inseridas automaticamente.

Entre elas:

- ID do serviço;
- tipo do documento;
- data relacionada ao serviço;
- numeração de páginas;
- número da conclusão, quando necessário.

---

## 14. Data de conclusão e geração

Data de conclusão e data de geração são conceitos diferentes.

Exemplo:

```text
Serviço concluído em:
13/08/2026

Documento gerado em:
14/08/2026
```

A data de conclusão representa o fato operacional.

A data de geração representa apenas quando a representação documental foi produzida.

---

## 15. Registro Interno

O Registro Interno deverá possuir nível de detalhe suficiente para reconstruir o serviço executado.

Sua estrutura poderá incluir:

1. Identificação;
2. Cliente;
3. Equipamento;
4. Origem do roteiro;
5. Execução;
6. Observações;
7. Histórico relevante;
8. Conclusão.

---

## 16. Identificação no Registro Interno

Poderá conter:

- ID;
- título;
- status;
- criado por;
- responsável;
- data de criação;
- data de início;
- data de conclusão;
- número da conclusão;
- indicação de roteiro personalizado.

---

## 17. Cliente no Registro Interno

Poderão ser apresentados, quando disponíveis:

- nome;
- empresa;
- telefone;
- e-mail;
- referência.

Campos sem conteúdo deverão ser omitidos.

---

## 18. Equipamento no Registro Interno

Poderão ser apresentados, quando disponíveis:

- descrição;
- fabricante;
- modelo;
- número de série;
- patrimônio;
- hostname;
- sistema operacional;
- observação do equipamento.

---

## 19. Origem do roteiro

O Registro Interno deverá poder identificar:

- modelo de origem;
- nome histórico do modelo;
- revisão;
- indicação de personalização.

Exemplo:

```text
Modelo:
Formatação — Cliente Contrato

Revisão:
4

Roteiro personalizado:
Sim
```

---

## 20. Execução do roteiro

O Registro Interno poderá apresentar todas as etapas e seus passos.

Exemplo:

```text
ETAPA 1 — PREPARAÇÃO

✓ Identificar equipamento
✓ Confirmar backup
— Configurar impressora

ETAPA 2 — INSTALAÇÃO

✓ Preparar disco
✓ Instalar sistema
✓ Instalar drivers
```

---

## 21. Estados no Registro Interno

Em uma conclusão normal, os estados esperados serão:

- Concluído;
- Não aplicável.

O documento deverá diferenciá-los visualmente.

Exemplo:

```text
✓ Concluído
— Não aplicável
```

---

## 22. Observações internas

O Registro Interno poderá apresentar:

- observações internas;
- observações destinadas ao cliente.

As duas categorias deverão permanecer identificáveis.

Informações internas representam parte válida do registro técnico.

---

## 23. Histórico relevante

O Registro Interno poderá apresentar um resumo de eventos importantes, como:

- criação;
- início;
- espera;
- retomada;
- conclusão;
- reabertura;
- nova conclusão;
- cancelamento.

Eventos poderão apresentar:

- data/hora;
- usuário;
- motivo, quando aplicável.

Não será necessário imprimir todo evento de interface.

---

## 24. Relatório de Serviço

O Relatório de Serviço deverá apresentar o resultado do trabalho de maneira profissional e apropriada ao cliente.

Sua estrutura poderá conter:

1. Identificação;
2. Cliente;
3. Equipamento;
4. Serviço realizado;
5. Procedimentos realizados;
6. Observações;
7. Recomendações;
8. Conclusão.

Se determinada seção não possuir conteúdo relevante, ela poderá ser omitida.

---

## 25. Conteúdo técnico externo

O relatório deverá apresentar informações técnicas suficientes para demonstrar o serviço, mas sem reproduzir necessariamente todo o nível de detalhe utilizado internamente.

A linguagem deverá ser compreensível e profissional.

---

## 26. Procedimentos realizados

Etapas e passos concluídos poderão ser apresentados de forma organizada.

Exemplo:

```text
PROCEDIMENTOS REALIZADOS

Preparação
✓ Backup verificado
✓ Equipamento identificado

Sistema
✓ Sistema operacional instalado
✓ Drivers atualizados
✓ Atualizações aplicadas

Validação
✓ Testes finais realizados
```

---

## 27. Não aplicáveis no relatório externo

Passos classificados como Não aplicáveis não deverão aparecer normalmente no relatório do cliente.

Essas informações permanecem registradas internamente.

A omissão evita poluir o documento com procedimentos que não fizeram parte do atendimento real.

---

## 28. Segurança das observações

Somente observações classificadas como destinadas ao cliente poderão fazer parte do Relatório de Serviço.

```text
Interna → proibida no relatório externo

Cliente → permitida
```

Essa separação deverá ser garantida funcionalmente.

Não poderá depender apenas de ocultação visual.

---

## 29. Tipos de informação destinada ao cliente

Uma observação externa poderá possuir um tipo simples:

- Informação;
- Recomendação.

Exemplo:

```text
OBSERVAÇÕES

Foi identificada degradação no dispositivo de armazenamento.

RECOMENDAÇÕES

Recomenda-se a substituição preventiva do SSD.
```

Essa classificação não altera a estrutura principal de observações definida anteriormente.

---

## 30. Observações internas

Observações classificadas como Internas não precisam possuir classificação adicional de Informação ou Recomendação.

Seu contexto já é exclusivamente operacional.

---

## 31. Conclusão textual

O Relatório de Serviço poderá possuir uma conclusão padrão.

Exemplo:

> Serviço concluído conforme os procedimentos descritos neste relatório.

O administrador poderá futuramente configurar um texto padrão institucional.

O usuário não deverá ser obrigado a redigir manualmente uma conclusão em todo serviço.

---

## 32. Configurações documentais

A área de configuração poderá incluir opções controladas para:

- título do documento;
- informações do cabeçalho;
- informações do rodapé;
- texto padrão de conclusão;
- informações institucionais exibidas.

Não será criado um editor visual completo de templates na versão 1.0.

---

## 33. Pré-visualização

O sistema deverá permitir visualizar os documentos antes da geração definitiva.

Especialmente para o Relatório do Cliente, a prévia deverá permitir conferir:

- cliente;
- equipamento;
- procedimentos;
- observações externas;
- recomendações;
- dados institucionais.

---

## 34. Geração

Em um serviço concluído, a área documental poderá disponibilizar ações como:

```text
Registro Interno
[ Visualizar ] [ Gerar PDF ]

Relatório de Serviço
[ Visualizar ] [ Gerar PDF ]
```

A forma exata da interface será definida posteriormente.

---

## 35. Geração sob demanda

Concluir um serviço deverá tornar seus documentos disponíveis para geração.

Não será obrigatório criar imediatamente arquivos físicos de PDF.

```text
Concluir serviço
      ↓
Preservar conclusão
      ↓
Documentos disponíveis
      ↓
Gerar quando necessário
```

---

## 36. PDF como representação

O PDF não será considerado a fonte de verdade do serviço.

```text
Dados estruturados do Reset Service
               ↓
             PDF
```

Caso um arquivo seja perdido, deverá ser possível regenerá-lo a partir dos dados preservados da conclusão correspondente.

---

## 37. Nome dos arquivos

O padrão proposto será:

### Registro Interno

```text
RS-2026-00142-c01-registro-interno.pdf
```

### Relatório de Serviço

```text
RS-2026-00142-c01-relatorio-servico.pdf
```

Uma segunda conclusão resultaria em:

```text
RS-2026-00142-c02-registro-interno.pdf
RS-2026-00142-c02-relatorio-servico.pdf
```

---

## 38. Segurança dos nomes de arquivos

Nome do cliente, empresa e equipamento não serão obrigatoriamente utilizados nos nomes físicos dos arquivos.

Isso evita:

- nomes excessivamente longos;
- caracteres inválidos;
- exposição desnecessária de dados;
- inconsistências de nomenclatura.

O ID do serviço será suficiente para identificação.

---

## 39. Serviços cancelados

Um serviço Cancelado poderá gerar Registro Interno.

O documento deverá indicar claramente:

```text
STATUS: CANCELADO
```

e poderá conter:

- motivo do cancelamento;
- progresso existente;
- passos concluídos;
- passos pendentes;
- passos Não aplicáveis;
- observações;
- histórico.

---

## 40. Relatório externo de serviço cancelado

A versão 1.0 não gerará automaticamente um Relatório de Serviço convencional para cliente quando o serviço estiver Cancelado.

Esse tipo de documento poderá ser avaliado futuramente caso surja necessidade operacional.

---

## 41. Campos vazios

Informações opcionais sem conteúdo deverão ser omitidas.

Evitar:

```text
Telefone:
E-mail:
Patrimônio:
Hostname:
```

Preferir somente informações existentes.

Isso melhora legibilidade e aparência profissional.

---

## 42. Paginação

Os documentos poderão utilizar múltiplas páginas livremente.

A legibilidade tem prioridade sobre a tentativa de concentrar todo o conteúdo em uma única página.

A paginação deverá ser automática.

---

## 43. Agrupamento de conteúdo

Sempre que razoável, etapas e seções deverão permanecer visualmente agrupadas.

O mecanismo de geração deverá evitar quebras de página ruins quando houver espaço suficiente para manter um bloco unido.

Não será uma regra absoluta para conteúdos grandes.

---

## 44. Logo da Technolife

O administrador poderá cadastrar a logo institucional.

Caso nenhuma logo esteja disponível:

- o documento continuará válido;
- o nome da Technolife deverá ser suficiente para identificação institucional.

A validação técnica dos arquivos de imagem será definida posteriormente.

---

## 45. Aparência

Os documentos deverão possuir linguagem visual:

- moderna;
- limpa;
- profissional;
- consistente;
- legível;
- sem excesso de elementos decorativos.

O Relatório de Serviço deverá ser apropriado para entrega direta ao cliente.

---

## 46. Alteração das configurações institucionais

Configurações futuras da Technolife não deverão modificar silenciosamente documentos históricos.

Exemplo:

```text
Conclusão em 2026
Telefone: X

Technolife altera telefone em 2027
Telefone atual: Y
```

A conclusão de 2026 deverá continuar relacionada ao contexto institucional preservado daquele momento.

---

## 47. Identidade visual histórica

O mesmo princípio poderá ser aplicado à logo e demais elementos relevantes.

Se a identidade institucional mudar, conclusões anteriores poderão continuar utilizando a configuração existente quando foram encerradas.

Essa regra protege a estabilidade dos registros históricos.

---

## 48. Correção de informações

Os PDFs não serão editados diretamente como fonte de correção.

Fluxo correto:

```text
Identificar informação incorreta
          ↓
Corrigir no Reset Service
          ↓
Gerar novamente
```

Quando a correção exigir alteração de um serviço Concluído, deverão ser respeitadas as regras de reabertura.

---

## 49. Conteúdo por documento

## Registro Interno

Poderá incluir:

- identificação completa;
- cliente;
- equipamento;
- responsável;
- modelo e revisão;
- indicação de personalização;
- etapas;
- todos os passos;
- Não aplicáveis;
- observações internas;
- observações externas;
- histórico relevante;
- conclusão.

## Relatório de Serviço

Poderá incluir:

- identidade da Technolife;
- ID;
- cliente;
- equipamento;
- serviço realizado;
- procedimentos concluídos;
- observações externas;
- recomendações;
- conclusão.

---

## 50. Regras Fundamentais

1. Existirão dois documentos principais.
2. O Registro Interno é destinado à Technolife.
3. O Relatório de Serviço é destinado ao cliente.
4. Ambos utilizam os dados estruturados do serviço.
5. Não haverá preenchimento duplicado.
6. O ID do serviço identifica os documentos.
7. Cada conclusão possui fotografia histórica própria.
8. Reabertura não sobrescreve conclusão anterior.
9. Regeneração não cria uma nova conclusão.
10. Registro Interno pode conter informações internas e externas.
11. Relatório de Serviço nunca contém informações classificadas como Internas.
12. Não aplicáveis são preservados internamente e normalmente omitidos externamente.
13. Observações externas podem ser Informação ou Recomendação.
14. Campos vazios devem ser omitidos.
15. Cabeçalho e rodapé utilizam dados institucionais configurados.
16. Os layouts são padronizados, e não livremente editáveis.
17. Pré-visualização deverá estar disponível.
18. A conclusão não exige geração física imediata.
19. Os PDFs poderão ser regenerados.
20. Serviços Cancelados poderão gerar Registro Interno.
21. Serviços Cancelados não gerarão Relatório de Serviço convencional na versão 1.0.
22. PDF é representação, não fonte de verdade.
23. Configurações institucionais relevantes devem ser preservadas historicamente por conclusão.
24. Correções devem ocorrer no Reset Service, nunca diretamente no PDF.

---

## 51. Estado da Decisão

**PLANNING-006 — Relatórios e Documentos PDF: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência para futuras decisões de UX, arquitetura, armazenamento, segurança, testes e implementação relacionadas à geração documental.