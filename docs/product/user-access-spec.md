# Reset Service — User Access Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação Funcional de Usuários, Autenticação e Permissões  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `service-workflow-spec.md`, `service-lifecycle-spec.md`, `service-template-spec.md`, `service-data-spec.md`, `document-generation-spec.md`

---

## 1. Objetivo

Este documento define o comportamento funcional relacionado a:

- usuários;
- autenticação;
- perfis;
- permissões;
- autoria;
- responsabilidade;
- ativação e desativação de contas;
- alteração e redefinição de senha;
- configuração inicial do primeiro Administrador.

Este documento não define mecanismos técnicos de autenticação, armazenamento de senhas, sessões, criptografia, cookies, tokens ou protocolos de segurança.

Esses assuntos serão especificados posteriormente na arquitetura e na documentação técnica de segurança.

---

## 2. Princípio de acesso

Todo uso do Reset Service deverá ocorrer através de usuário autenticado.

Não haverá acesso anônimo às funcionalidades da aplicação.

A autenticação permite que o sistema identifique quem está utilizando a aplicação e associe autoria às ações relevantes.

---

## 3. Autenticação local

A versão 1.0 possuirá seu próprio cadastro local de usuários.

O funcionamento não dependerá de:

- conta Microsoft;
- Active Directory;
- Google;
- autenticação em nuvem;
- conexão com a internet;
- serviço externo de identidade.

O mecanismo técnico será definido posteriormente.

---

## 4. Perfis

Existirão exatamente dois perfis funcionais:

- Administrador;
- Técnico.

A versão 1.0 não possuirá perfis adicionais como:

- Supervisor;
- Gerente;
- Auditor;
- Atendente;
- Somente leitura.

Também não haverá permissões customizadas individualmente por usuário.

---

## 5. Técnico

O perfil Técnico representa o usuário operacional responsável principalmente pela execução e acompanhamento dos serviços.

O Técnico poderá:

- acessar o Dashboard;
- visualizar todos os serviços;
- pesquisar serviços;
- utilizar filtros;
- criar serviços;
- utilizar modelos publicados;
- visualizar modelos;
- preencher e alterar informações permitidas do serviço;
- executar roteiros;
- alterar estados dos passos;
- registrar observações;
- personalizar roteiros de serviços;
- alterar responsável em serviços abertos;
- colocar serviços em espera;
- retomar serviços;
- concluir serviços;
- cancelar serviços;
- consultar histórico;
- consultar serviços concluídos;
- consultar serviços cancelados;
- visualizar documentos;
- gerar documentos quando permitido;
- alterar sua própria senha.

---

## 6. Restrições do Técnico

O Técnico não poderá:

- criar usuários;
- editar usuários;
- desativar usuários;
- reativar usuários;
- alterar perfis;
- redefinir credenciais de outros usuários;
- criar Modelos de Serviço;
- editar Modelos de Serviço;
- publicar revisões;
- descartar rascunhos de modelos;
- arquivar modelos;
- reativar modelos;
- alterar dados institucionais da Technolife;
- alterar configurações documentais;
- reabrir serviços Concluídos;
- reabrir serviços Cancelados;
- executar operações administrativas de backup ou restauração.

---

## 7. Administrador

O perfil Administrador possui todas as capacidades operacionais do Técnico e, adicionalmente, as capacidades administrativas do sistema.

Conceitualmente:

```text
Administrador
=
Técnico
+
Administração
```

O Administrador poderá trabalhar normalmente na execução dos serviços.

---

## 8. Capacidades administrativas

O Administrador poderá:

- criar usuários;
- editar usuários;
- desativar usuários;
- reativar usuários;
- alterar perfis;
- redefinir senhas;
- criar modelos;
- editar modelos;
- publicar revisões;
- descartar rascunhos de modelos;
- duplicar modelos;
- arquivar modelos;
- reativar modelos;
- configurar informações institucionais;
- configurar documentos;
- reabrir serviços;
- executar funções administrativas de backup e restauração quando implementadas.

---

## 9. Estrutura funcional do usuário

Um usuário possuirá conceitualmente:

```text
USUÁRIO
│
├── Nome
├── Nome de acesso
├── Perfil
└── Status
```

As credenciais de autenticação também pertencem à conta, mas não serão tratadas como informações normalmente consultáveis.

---

## 10. Nome

Todo usuário deverá possuir um nome para identificação humana.

Exemplo:

```text
João da Silva
```

Esse nome poderá ser utilizado em:

- histórico;
- identificação de responsável;
- autoria de eventos;
- registros internos;
- interface.

O nome será obrigatório.

---

## 11. Nome de acesso

Cada usuário deverá possuir um nome de acesso único.

Exemplos:

```text
joao
carlos
marcos
```

Não será obrigatório utilizar endereço de e-mail como identificador de login.

---

## 12. Unicidade

Dois usuários não poderão possuir simultaneamente o mesmo nome de acesso.

O nome de acesso deverá identificar uma única conta.

---

## 13. Identidade permanente da conta

A conta deverá possuir identidade própria independente de alterações posteriores no nome ou no nome de acesso.

Exemplo:

```text
joao.s
↓
joao.silva
```

continua representando a mesma conta e o mesmo histórico.

A implementação dessa identidade será definida posteriormente.

---

## 14. Status do usuário

Existirão dois estados:

- Ativo;
- Inativo.

---

## 15. Usuário Ativo

Um usuário Ativo poderá autenticar-se e realizar as operações permitidas por seu perfil.

---

## 16. Usuário Inativo

Um usuário Inativo não poderá acessar o Reset Service.

Sua conta deverá continuar armazenada para preservação histórica.

---

## 17. Usuários não são excluídos normalmente

Contas utilizadas operacionalmente não deverão ser removidas do sistema.

O mecanismo normal será:

```text
Ativo → Inativo
```

e não:

```text
Usuário → Excluído
```

Essa regra preserva a autoria de registros antigos.

---

## 18. Preservação histórica

A desativação de um usuário não poderá modificar registros anteriores.

Exemplo:

```text
RS-2026-00142
Concluído por João da Silva
```

deverá continuar identificando João mesmo depois de sua conta ser desativada.

---

## 19. Conta individual

O princípio operacional será:

> Uma pessoa deve utilizar sua própria conta.

Deverão ser evitadas contas genéricas compartilhadas para uso cotidiano.

Exemplos que devem ser evitados:

```text
tecnico
informatica
equipe
admin-geral
```

A identificação individual é necessária para que o histórico possua significado.

---

## 20. Visibilidade dos serviços

Todos os usuários autenticados poderão visualizar todos os serviços.

A versão 1.0 não restringirá serviços somente ao usuário responsável.

Isso facilita:

- colaboração;
- continuidade;
- substituição de técnicos;
- acompanhamento pela equipe.

---

## 21. Responsável e permissão

Ser responsável por um serviço não confere exclusividade de acesso.

Exemplo:

```text
Responsável:
João
```

Carlos poderá trabalhar naquele serviço se possuir as permissões operacionais necessárias.

A autoria real das ações deverá ser registrada.

---

## 22. Responsável e autor

São conceitos diferentes.

### Responsável

Representa o principal usuário operacional associado ao serviço.

### Autor

Representa quem efetivamente realizou determinada ação.

Exemplo:

```text
Responsável:
João
```

Histórico:

```text
Criado por Carlos
Iniciado por João
Retomado por Marcos
Concluído por João
```

---

## 23. Alteração de responsável

Administrador e Técnico poderão alterar o responsável enquanto o serviço estiver:

- Rascunho;
- Em andamento;
- Aguardando.

A alteração deverá ser registrada no histórico.

Exemplo:

```text
Responsável alterado

João → Carlos

Alteração realizada por Marcos.
```

---

## 24. Usuário inativo como responsável

Ao desativar um usuário que possui serviços abertos sob sua responsabilidade, o Administrador deverá ser informado.

Exemplo:

```text
João da Silva possui 3 serviços abertos.

RS-2026-00141
RS-2026-00144
RS-2026-00151
```

A aplicação poderá permitir a desativação, mas deverá tornar clara a necessidade de reatribuição.

---

## 25. Novas atribuições

Usuários Inativos não deverão aparecer normalmente entre as opções disponíveis para novas atribuições de responsabilidade.

Eles continuarão visíveis nos registros históricos existentes.

---

## 26. Criação de usuário

Somente Administradores poderão criar usuários.

O cadastro deverá solicitar pelo menos:

- nome;
- nome de acesso;
- perfil;
- credencial inicial.

O novo usuário começará no estado Ativo.

---

## 27. Primeiro Administrador

Na primeira utilização do Reset Service, quando ainda não existirem usuários, deverá existir um fluxo específico de configuração inicial.

Exemplo:

```text
Configuração inicial
        ↓
Criar primeiro Administrador
```

Serão necessários pelo menos:

- nome;
- nome de acesso;
- senha.

A conta será criada como:

```text
Perfil: Administrador
Status: Ativo
```

---

## 28. Existência obrigatória de Administrador

O sistema deverá possuir sempre pelo menos um Administrador Ativo.

Não deverá ser possível deixar a aplicação sem qualquer conta administrativa válida.

---

## 29. Último Administrador

O sistema deverá impedir:

- desativação do último Administrador Ativo;
- alteração do perfil do último Administrador Ativo para Técnico.

Antes disso, outro Administrador deverá ser criado ou promovido.

---

## 30. Alteração de perfil

Administradores poderão alterar:

```text
Técnico → Administrador
```

e:

```text
Administrador → Técnico
```

desde que a regra de pelo menos um Administrador Ativo seja respeitada.

Mudanças de perfil deverão possuir autoria registrada quando relevante.

---

## 31. Edição do usuário

Administradores poderão editar informações permitidas da conta.

Exemplos:

- nome;
- nome de acesso;
- perfil.

Alterar essas informações não deverá criar uma nova conta nem romper seu histórico.

---

## 32. Desativação

Somente Administradores poderão desativar usuários.

A operação deverá utilizar confirmação.

O usuário deverá ser informado de que:

- a conta perderá acesso;
- o histórico será preservado;
- serviços existentes não serão apagados.

---

## 33. Reativação

Um usuário Inativo poderá ser reativado por Administrador.

```text
Inativo → Ativo
```

A conta deverá preservar:

- identidade;
- histórico;
- vínculos anteriores;
- perfil, salvo alteração administrativa.

---

## 34. Alteração da própria senha

Todo usuário autenticado poderá alterar sua própria senha.

O fluxo normal deverá solicitar a credencial atual.

Requisitos técnicos de senha serão especificados posteriormente.

---

## 35. Redefinição administrativa

Um Administrador poderá redefinir a credencial de outro usuário quando necessário.

O Administrador não deverá visualizar a senha atual do usuário.

A ação representa substituição ou redefinição da credencial.

---

## 36. Credencial temporária

Uma redefinição administrativa poderá utilizar uma senha temporária.

Nesse caso, o usuário poderá ser obrigado a definir uma nova senha em seu próximo acesso.

A implementação exata será especificada na documentação de segurança.

---

## 37. Identidade do usuário na interface

A aplicação deverá deixar claro qual usuário está autenticado.

Exemplo:

```text
João da Silva
Técnico
```

Isso reduz o risco de ações serem registradas na conta de outra pessoa por engano.

---

## 38. Encerramento de sessão

Todo usuário autenticado deverá possuir uma ação explícita para sair.

```text
[ Sair ]
```

O comportamento técnico da sessão será definido posteriormente.

---

## 39. Autoria operacional

Ações operacionais relevantes deverão registrar:

- usuário;
- data/hora;
- ação.

Exemplos:

- criação do serviço;
- início;
- espera;
- retomada;
- conclusão;
- cancelamento;
- reabertura;
- alteração de responsável.

---

## 40. Autoria administrativa

Ações administrativas relevantes também deverão registrar autoria quando necessário.

Exemplos:

- publicação de revisão;
- arquivamento de modelo;
- reabertura de serviço;
- desativação de usuário;
- alteração institucional relevante.

Não será necessário registrar cada clique ou mudança puramente visual da aplicação.

---

## 41. Reabertura de serviço

Somente Administradores poderão reabrir serviços Concluídos ou Cancelados.

```text
Técnico → não permitido

Administrador → permitido
```

A exigência de motivo continua definida pela especificação do ciclo de vida.

---

## 42. Cancelamento

Técnicos e Administradores poderão cancelar serviços nos estados permitidos.

O motivo continuará obrigatório.

O cancelamento será considerado parte da operação normal e não exigirá perfil administrativo.

---

## 43. Conclusão

Técnicos e Administradores poderão concluir serviços normalmente.

A autoria da conclusão será registrada.

---

## 44. Modelos

Técnicos poderão:

- visualizar modelos publicados;
- utilizar modelos publicados na criação de serviços.

Somente Administradores poderão:

- criar;
- editar;
- publicar;
- duplicar;
- arquivar;
- reativar;
- descartar alterações.

---

## 45. Configurações institucionais

Somente Administradores poderão modificar:

- dados da Technolife;
- logo;
- cabeçalho;
- rodapé;
- textos institucionais;
- configurações de documentos.

Técnicos utilizarão as configurações existentes sem poder alterá-las.

---

## 46. Backup e restauração

Operações administrativas de backup e restauração serão restritas ao perfil Administrador.

O comportamento funcional e técnico será especificado separadamente.

---

## 47. Matriz de permissões

| Função | Técnico | Administrador |
|---|---:|---:|
| Acessar Dashboard | Sim | Sim |
| Visualizar todos os serviços | Sim | Sim |
| Criar serviço | Sim | Sim |
| Pesquisar serviços | Sim | Sim |
| Executar checklist | Sim | Sim |
| Adicionar observações | Sim | Sim |
| Personalizar roteiro do serviço | Sim | Sim |
| Alterar responsável | Sim | Sim |
| Colocar em espera | Sim | Sim |
| Retomar serviço | Sim | Sim |
| Concluir serviço | Sim | Sim |
| Cancelar serviço | Sim | Sim |
| Consultar histórico | Sim | Sim |
| Gerar PDFs | Sim | Sim |
| Visualizar modelos | Sim | Sim |
| Utilizar modelo | Sim | Sim |
| Criar modelo | Não | Sim |
| Editar modelo | Não | Sim |
| Publicar revisão | Não | Sim |
| Arquivar modelo | Não | Sim |
| Reabrir serviço | Não | Sim |
| Configurar empresa | Não | Sim |
| Configurar documentos | Não | Sim |
| Criar usuários | Não | Sim |
| Alterar usuários | Não | Sim |
| Desativar/Reativar usuários | Não | Sim |
| Redefinir senha de outro usuário | Não | Sim |
| Backup/Restauração administrativa | Não | Sim |

---

## 48. Ausência de permissões individuais

A versão 1.0 não permitirá configurar permissões específicas para cada usuário.

Exemplo não suportado:

```text
João
✓ Criar serviço
✕ Concluir serviço
✓ Editar modelo
```

As permissões serão determinadas exclusivamente pelo perfil.

---

## 49. Ausência de hierarquia entre Técnicos

Todos os Técnicos possuirão as mesmas capacidades funcionais.

Não haverá níveis ou hierarquia entre usuários do perfil Técnico.

---

## 50. Conta Inativa e autorização

Uma conta Inativa deixa de possuir autorização válida para utilizar o Reset Service.

A forma técnica de invalidar sessões existentes será definida na documentação de segurança.

---

## 51. Modelo funcional de autorização

O modelo funcional deverá permanecer simples:

```text
Usuário autenticado?
        ↓
Qual é o perfil?
        ↓
O perfil pode executar a ação?
```

A versão 1.0 não utilizará:

- grupos;
- subgrupos;
- políticas customizadas;
- permissões por serviço;
- permissões por modelo;
- escopos individuais complexos.

---

## 52. Regras Fundamentais

1. Todo uso exige autenticação.
2. A autenticação será própria e local.
3. Existirão somente os perfis Administrador e Técnico.
4. Administrador possui todas as capacidades operacionais do Técnico.
5. Todos os usuários autenticados podem visualizar todos os serviços.
6. Responsabilidade não determina exclusividade de acesso.
7. Responsabilidade e autoria são conceitos diferentes.
8. Cada pessoa deverá utilizar sua própria conta.
9. Usuários utilizados historicamente não deverão ser excluídos.
10. Contas Inativas não podem acessar o sistema.
11. A desativação preserva todo o histórico.
12. Usuários Inativos não aparecem normalmente em novas atribuições.
13. Sempre deverá existir pelo menos um Administrador Ativo.
14. O último Administrador Ativo não pode ser desativado nem rebaixado.
15. Técnicos podem criar, executar, concluir e cancelar serviços.
16. Somente Administradores podem reabrir serviços.
17. Somente Administradores administram modelos.
18. Somente Administradores administram usuários.
19. Somente Administradores alteram configurações institucionais.
20. Administradores podem redefinir credenciais, mas não visualizar senhas existentes.
21. Todo usuário pode alterar sua própria senha.
22. Ações operacionais e administrativas relevantes deverão preservar autoria.
23. Não haverá permissões individuais customizadas.
24. Não haverá hierarquia entre Técnicos.
25. O primeiro uso deverá permitir configurar o primeiro Administrador.

---

## 53. Estado da Decisão

**PLANNING-007 — Usuários, Autenticação e Permissões Funcionais: CONCLUÍDA E APROVADA.**

Este documento passa a servir como referência para futuras decisões relacionadas à UX, segurança, autenticação técnica, persistência, auditoria e testes.