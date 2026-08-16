# Reset Service — Non-Functional Requirements

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Requisitos Não Funcionais  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`, `service-template-spec.md`, `service-data-spec.md`, `document-generation-spec.md`, `user-access-spec.md`, `ux-navigation-spec.md`

---

## 1. Objetivo

Este documento define as características de qualidade, operação e ambiente esperadas do Reset Service.

Abrange:

- funcionamento sem internet;
- operação em rede local;
- centralização;
- desempenho;
- concorrência;
- integridade;
- recuperação de falhas;
- compatibilidade;
- volumes esperados;
- atualização;
- backup;
- observabilidade;
- segurança transversal;
- acessibilidade;
- manutenibilidade.

Não são definidas aqui tecnologias, frameworks, bancos de dados ou mecanismos específicos de implementação.

---

## 2. Prioridades não funcionais

As prioridades do produto serão:

```text
Confiabilidade
      ↓
Simplicidade
      ↓
Rapidez
      ↓
Baixo consumo
      ↓
Facilidade de manutenção
```

A solução deverá ser proporcional ao porte e à finalidade do Reset Service.

---

## 3. Independência da internet

O Reset Service deverá funcionar integralmente sem acesso à internet.

Isso inclui:

- autenticação;
- Dashboard;
- serviços;
- modelos;
- execução de roteiros;
- histórico;
- pesquisa;
- usuários;
- configurações;
- geração de documentos;
- funções administrativas locais.

Nenhuma função essencial poderá depender em tempo de execução de:

- APIs externas;
- autenticação em nuvem;
- CDN;
- geração de PDF remota;
- fontes externas;
- banco de dados externo;
- serviços hospedados na internet.

---

## 4. Rede local

O ambiente operacional será a rede interna da Technolife.

Conceitualmente:

```text
Computadores dos usuários
          │
          │ LAN
          ▼
     Reset Service
     ambiente central
          │
          ▼
         Dados
```

A indisponibilidade da internet não deverá afetar esse fluxo.

---

## 5. Centralização

A implantação e os dados deverão ser centralizados sempre que possível.

Preferência operacional:

```text
Uma implantação
      ↓
Vários usuários
```

Deverá ser evitada uma arquitetura que exija instalação e atualização independente da aplicação completa em cada estação de trabalho.

---

## 6. Ausência de sincronização offline por estação

A versão 1.0 não terá sincronização independente entre estações.

Se o ambiente central ou a rede local estiver indisponível, o Reset Service poderá ficar temporariamente indisponível.

Essa condição é aceitável.

Não será criado mecanismo de execução local seguida de sincronização posterior.

---

## 7. Leveza

A solução não deverá exigir infraestrutura de alto desempenho.

Deverão ser evitadas necessidades injustificadas de:

- GPU;
- grande consumo de memória;
- múltiplos servidores;
- clusters;
- orquestração distribuída;
- infraestrutura empresarial complexa.

---

## 8. Desempenho percebido

Interações comuns deverão apresentar resposta praticamente imediata.

Exemplos:

- marcar passo;
- navegar entre etapas;
- abrir menus;
- adicionar observação;
- atualizar informação simples.

Meta de experiência em condições normais:

```text
aproximadamente < 500 ms
```

Essa meta representa percepção do usuário e não uma garantia absoluta independente de infraestrutura.

---

## 9. Carregamento de telas

Telas de uso comum deverão normalmente apresentar conteúdo útil em aproximadamente até:

```text
2 segundos
```

na rede local e dentro dos volumes previstos.

Incluem:

- Dashboard;
- listagem de serviços;
- abertura de serviço;
- listagem de modelos;
- abertura de modelo.

---

## 10. Operações demoradas

Operações naturalmente mais pesadas poderão ultrapassar essas metas.

Exemplos:

- geração de PDF;
- backup;
- restauração;
- consultas históricas extensas.

Nesses casos, a interface deverá:

- informar processamento;
- permanecer responsiva;
- impedir execução duplicada acidental.

---

## 11. Multiusuário

O sistema deverá suportar múltiplos usuários trabalhando simultaneamente.

Exemplo:

```text
João   → Serviço A
Carlos → Serviço B
Marcos → Serviço C
```

Também deverá suportar mais de um usuário acessando o mesmo serviço.

---

## 12. Concorrência sobre o mesmo registro

Não será necessário oferecer colaboração em tempo real semelhante a editores de documentos colaborativos.

Porém:

> uma alteração não poderá sobrescrever silenciosamente uma alteração mais recente feita por outro usuário.

Conflitos relevantes deverão ser detectáveis e tratados.

A estratégia técnica será definida na arquitetura.

---

## 13. Atualização entre usuários

Alterações relevantes realizadas por outro usuário deverão tornar-se visíveis sem necessidade de reiniciar a aplicação.

Não é requisito que a propagação seja instantânea em milissegundos.

A arquitetura poderá escolher mecanismo proporcional ao problema.

---

## 14. Integridade

Dados confirmados deverão permanecer persistidos corretamente.

Especial atenção deverá ser dada a:

- serviços;
- estados dos passos;
- observações;
- histórico;
- revisões;
- usuários;
- conclusões;
- configurações.

Consistência deverá ter prioridade sobre otimizações prematuras.

---

## 15. Atomicidade

Operações de negócio importantes deverão ser consistentes como um todo.

Exemplo:

```text
Concluir serviço
```

não poderá resultar em apenas parte dos registros necessários sendo persistidos.

Estado, conclusão e histórico deverão permanecer coerentes.

---

## 16. Falha da estação cliente

Fechamento do navegador, reinicialização da estação ou falha local não deverá comprometer dados já confirmados no servidor.

O usuário deverá poder acessar novamente e continuar a partir do estado persistido.

---

## 17. Falha de comunicação

Caso uma alteração não seja confirmada pelo ambiente central, a interface deverá informar claramente ao usuário.

Não poderá apresentar sucesso quando a operação não foi persistida.

Exemplo:

```text
Não foi possível salvar a alteração.

A conexão com o servidor foi perdida.
```

---

## 18. Conteúdo não confirmado

Campos com conteúdo potencialmente maior, como observações, deverão receber tratamento adequado para reduzir perda acidental.

A solução poderá futuramente utilizar:

- salvamento explícito;
- preservação temporária;
- confirmação antes de abandonar conteúdo.

A implementação será definida posteriormente.

---

## 19. Unicidade dos IDs

O identificador:

```text
RS-AAAA-NNNNN
```

deverá permanecer único mesmo quando mais de um usuário criar serviços simultaneamente.

A garantia não poderá depender exclusivamente da interface.

---

## 20. Sequência das revisões

Publicações simultâneas não poderão gerar números de revisão duplicados dentro do mesmo modelo.

A persistência deverá assegurar uma sequência coerente.

---

## 21. Usuários esperados

Para planejamento da versão 1.0:

- aproximadamente 1 a 20 usuários cadastrados;
- aproximadamente 1 a 10 usuários simultâneos.

Esses números orientam proporcionalidade arquitetural e não deverão ser tratados como limitações rígidas.

---

## 22. Volume de serviços

O sistema deverá permanecer adequado para histórico de longo prazo.

Uma ordem de grandeza como:

```text
50.000 serviços
```

deverá ser considerada tecnicamente normal.

A aplicação não deverá depender de limpeza frequente do histórico para continuar utilizável.

---

## 23. Roteiros

O sistema deverá suportar confortavelmente roteiros com:

- dezenas de etapas;
- centenas de passos.

Como referência, até aproximadamente 50 etapas em um serviço não deverá representar situação tecnicamente excepcional.

---

## 24. Observações e histórico

A quantidade de observações e eventos deverá poder crescer ao longo da vida do serviço.

Caso necessário, técnicas como paginação ou carregamento progressivo poderão ser utilizadas sem alterar as regras de negócio.

---

## 25. Característica do armazenamento

Os dados principais são predominantemente estruturados e leves:

- textos;
- estados;
- datas;
- referências;
- configurações.

Como anexos não fazem parte do escopo atual, não se espera crescimento elevado de armazenamento binário na versão 1.0.

---

## 26. Ambiente Windows

Windows será o ambiente oficial prioritário para implantação da versão 1.0.

Não existe requisito de homologação de servidor Linux nesta versão.

A escolha tecnológica poderá ser multiplataforma desde que isso não acrescente complexidade desnecessária.

---

## 27. Clientes

Os computadores clientes deverão conseguir utilizar o produto sem instalação operacional complexa.

Caso a arquitetura escolhida seja web, deverão ser priorizados navegadores modernos do Windows.

---

## 28. Navegadores

Caso a aplicação seja web, deverão ser oficialmente considerados:

- Microsoft Edge moderno;
- Google Chrome moderno.

Não haverá requisito de compatibilidade com Internet Explorer ou navegadores legados.

---

## 29. Resolução

Desktop e notebook serão os ambientes prioritários.

A aplicação deverá continuar utilizável em resoluções comuns a partir de aproximadamente:

```text
1366 × 768
```

sem depender de monitores grandes ou de resolução Full HD.

---

## 30. Dispositivos móveis

Smartphones e tablets não serão plataforma oficial da versão 1.0.

Responsividade básica é desejável, mas não deverá aumentar significativamente a complexidade do projeto.

---

## 31. Disponibilidade

Não será exigida infraestrutura de alta disponibilidade.

Interrupções poderão ocorrer durante:

- desligamento do servidor;
- aplicação fechada na máquina hospedeira;
- manutenção;
- indisponibilidade da LAN;
- atualização controlada.

Enquanto `ResetService.exe` não estiver em execução, não haverá processo residente, consumo contínuo de CPU/RAM ou URL disponível. O objetivo será abertura, operação e encerramento simples e previsível.

---

## 32. Inicialização do ambiente

Após uma reinicialização normal do computador ou servidor responsável, o sistema permanecerá parado até que um operador execute `ResetService.exe` na máquina hospedeira.

Essa ação iniciará no mesmo processo o Kestrel, o SQLite e os componentes hospedados internos e abrirá o navegador padrão. Não haverá requisito de inicialização automática com o Windows.

---

## 33. Manutenibilidade

A arquitetura deverá privilegiar poucos componentes e operação compreensível.

Deverão ser evitados sem necessidade:

- microsserviços;
- filas distribuídas;
- múltiplos bancos;
- clusters;
- infraestrutura de orquestração;
- componentes que exijam administração especializada contínua.

---

## 34. Atualização

Sempre que possível, a atualização do Reset Service deverá ser centralizada.

Objetivo:

```text
Atualizar ambiente central
          ↓
Usuários passam a utilizar nova versão
```

A atualização estação por estação deverá ser evitada.

---

## 35. Segurança da atualização

Atualizações não poderão comprometer:

- serviços existentes;
- modelos;
- revisões;
- usuários;
- histórico;
- documentos históricos;
- configurações.

Mudanças de estrutura de dados deverão possuir processo controlado de migração.

---

## 36. Compatibilidade histórica

Evoluções futuras do produto deverão manter os registros anteriores consultáveis.

A estratégia de atualização não poderá exigir recriação periódica da base de dados.

---

## 37. Backup

Backup é requisito obrigatório da versão 1.0.

Deverá abranger todos os elementos essenciais para reconstrução do ambiente, incluindo pelo menos:

- dados da aplicação;
- usuários;
- modelos;
- revisões;
- serviços;
- histórico;
- conclusões;
- configurações;
- informações institucionais;
- arquivos essenciais quando aplicável.

---

## 38. Backup automático

A versão 1.0 deverá permitir uma política automatizada de backup.

O backup manual deverá estar disponível, e o backup automático deverá poder ser ativado pelo Administrador quando desejado.

O Administrador poderá manter o backup automático desativado.

O Reset Service deverá continuar funcionando normalmente com backup automático desativado.

A periodicidade e retenção serão definidas em especificação própria.

Se o horário configurado ocorrer enquanto a aplicação estiver fechada, a próxima inicialização poderá executar no máximo um backup automático pendente. Não serão gerados backups retroativos para cada dia perdido.

---

## 39. Destino do backup

A solução deverá permitir armazenar backups fora do armazenamento físico primário da aplicação.

Uma cópia existente somente no mesmo disco não será considerada proteção adequada contra falha física.

---

## 40. Restauração

Deverá existir procedimento documentado e testável de restauração.

O processo deverá poder recuperar o Reset Service de maneira controlada.

Backup sem possibilidade confiável de restauração não será considerado suficiente.

---

## 41. Validação de backup

Quando tecnicamente viável, o sistema deverá validar elementos básicos do backup antes de tratá-lo como utilizável.

O mecanismo específico será definido posteriormente.

---

## 42. Logs técnicos

O sistema deverá registrar informações suficientes para diagnóstico técnico.

Podem incluir:

- erros inesperados;
- falhas de persistência;
- falhas de geração documental;
- erros de inicialização;
- operações administrativas relevantes;
- versão da aplicação.

---

## 43. Log técnico e histórico funcional

Esses conceitos deverão permanecer separados.

### Histórico funcional

Registra fatos do negócio.

Exemplo:

```text
Serviço concluído por João.
```

### Log técnico

Registra fatos de operação do software.

Exemplo:

```text
Falha de persistência ao processar operação.
```

---

## 44. Tratamento de erros

Detalhes técnicos como stack traces não deverão ser apresentados aos usuários finais.

A interface deverá mostrar mensagens compreensíveis.

Detalhes suficientes para diagnóstico deverão permanecer nos logs.

---

## 45. Segurança

Mesmo operando somente na rede interna, o Reset Service deverá possuir segurança adequada.

Princípios já obrigatórios:

- autenticação;
- autorização;
- proteção de credenciais;
- validação de entradas;
- proteção contra alterações não autorizadas;
- separação de informações internas e externas;
- autoria de ações relevantes;
- exposição limitada à rede necessária.

A definição técnica será realizada separadamente.

---

## 46. Dados pessoais e técnicos

O sistema poderá armazenar:

- nomes;
- telefones;
- e-mails;
- empresas;
- dados de equipamentos;
- observações técnicas.

Esses dados não deverão ser expostos desnecessariamente fora do ambiente autorizado.

---

## 47. Data e hora

A apresentação deverá ser adequada ao contexto brasileiro.

Eventos deverão manter ordenação temporal consistente.

Diferenças de timezone não poderão produzir horários incorretos em:

- histórico;
- conclusões;
- relatórios;
- logs funcionais.

---

## 48. Precisão

Os eventos deverão possuir precisão suficiente para reconstruir sua sequência.

A interface poderá normalmente apresentar:

```text
13/08/2026 às 09:42
```

mesmo que internamente exista precisão adicional.

---

## 49. Acessibilidade

A aplicação deverá respeitar boas práticas básicas, incluindo:

- contraste adequado;
- foco visível;
- navegação por teclado;
- labels apropriados;
- estados não identificados apenas por cor;
- áreas clicáveis adequadas.

---

## 50. Consistência visual

Elementos equivalentes deverão utilizar comportamento equivalente em toda a aplicação.

A implementação deverá favorecer um conjunto pequeno e consistente de componentes e padrões.

---

## 51. Dependências

Toda dependência relevante deverá resolver necessidade concreta.

Deverão ser evitadas bibliotecas ou serviços adicionados apenas por conveniência superficial quando aumentarem:

- instalação;
- manutenção;
- tamanho;
- vulnerabilidades;
- complexidade operacional.

---

## 52. Runtime offline

As dependências necessárias ao funcionamento normal deverão estar disponíveis localmente.

O sistema não deverá buscar recursos obrigatórios na internet durante a operação.

---

## 53. Isolamento de falhas do cliente

Falhas em uma estação cliente não deverão afetar o ambiente central nem as demais estações.

Uma falha de navegador não deverá comprometer o banco ou interromper o trabalho dos outros usuários.

---

## 54. Proporcionalidade arquitetural

O projeto deverá possuir capacidade suficiente para a operação prevista, mas evitar dimensionamento injustificado.

O Reset Service não precisa de arquitetura destinada a milhões de usuários.

A regra será:

> escolher a solução mais simples que satisfaça corretamente os requisitos presentes e ofereça evolução razoável.

---

## 55. Metas consolidadas

| Área | Requisito |
|---|---|
| Internet | Não necessária |
| Rede | LAN interna |
| Dados | Centralizados |
| Sincronização offline por estação | Não |
| Plataforma prioritária | Windows |
| Usuários cadastrados | ~1–20 |
| Usuários simultâneos | ~1–10 |
| Histórico esperado | Dezenas de milhares de serviços |
| Referência de escala | ~50.000 serviços |
| Interação comum | Aproximadamente < 500 ms percebidos |
| Tela comum | Aproximadamente até 2 s |
| Multiusuário | Obrigatório |
| Tratamento de concorrência | Obrigatório |
| Edge/Chrome | Prioritários se arquitetura web |
| Desktop/notebook | Plataforma oficial |
| Mobile | Não oficial |
| Backup | Obrigatório |
| Backup automático | Disponível e opcional |
| Restauração documentada | Obrigatória |
| Atualização | Preferencialmente centralizada |
| Dependência externa em runtime | Não permitida para funções essenciais |
| Alta disponibilidade | Não requerida |

---

## 56. Regras Fundamentais

1. O Reset Service deve funcionar sem internet.
2. A operação ocorrerá na rede local.
3. Dados serão centralizados.
4. Não haverá sincronização offline por estação na versão 1.0.
5. A solução deverá ser leve.
6. A arquitetura deverá ser simples de manter.
7. Interações comuns deverão parecer imediatas.
8. Telas comuns deverão normalmente apresentar conteúdo em aproximadamente até 2 segundos.
9. Uso multiusuário é obrigatório.
10. Alterações concorrentes não podem provocar sobrescrita silenciosa.
11. IDs devem permanecer únicos em operações simultâneas.
12. Revisões devem manter sequência consistente.
13. Dados confirmados deverão sobreviver a falhas das estações.
14. Falhas de comunicação deverão ser informadas.
15. O histórico deverá suportar dezenas de milhares de serviços.
16. Windows será o ambiente prioritário.
17. Desktop e notebook serão plataformas oficiais.
18. Mobile não será requisito oficial.
19. Atualização deverá ser preferencialmente centralizada.
20. Atualizações deverão preservar dados históricos.
21. Backup é obrigatório.
22. Backup automático será suportado.
23. Um horário de backup perdido enquanto a aplicação estiver fechada poderá gerar no máximo uma execução pendente na próxima inicialização.
24. Backups devem poder ser armazenados fora do disco principal.
25. Restauração deverá possuir procedimento documentado e testável.
26. Logs técnicos deverão existir.
27. Histórico funcional e log técnico serão conceitos diferentes.
28. Erros técnicos não deverão ser expostos diretamente aos usuários.
29. A aplicação continuará exigindo segurança adequada em rede interna.
30. Datas e horários deverão permanecer consistentes.
31. Acessibilidade básica deverá ser respeitada.
32. Dependências obrigatórias de runtime deverão funcionar offline.
33. A arquitetura deverá evitar overengineering e permanecer proporcional à operação da Technolife.

---

## 57. Estado da Decisão

**PLANNING-009 — Requisitos Não Funcionais: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência obrigatória para decisões futuras de arquitetura, implantação, persistência, concorrência, segurança, backup, testes e manutenção.
