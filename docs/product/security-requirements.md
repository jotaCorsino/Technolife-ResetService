# Reset Service — Security Requirements

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Requisitos de Segurança e Proteção de Dados  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `user-access-spec.md`, `document-generation-spec.md`, `backup-recovery-spec.md`, `non-functional-requirements.md`

---

## 1. Objetivo

Este documento define os requisitos funcionais e não funcionais de segurança do Reset Service.

Seu escopo inclui:

- autenticação;
- autorização;
- credenciais;
- sessões;
- tentativas de login;
- validação de entrada;
- proteção de dados;
- segurança dos documentos;
- backups;
- uploads;
- logs;
- segredos;
- concorrência;
- integridade das regras de negócio.

Os mecanismos técnicos específicos serão definidos posteriormente durante a arquitetura.

---

## 2. Princípio central

O fato de o Reset Service operar em uma rede interna não elimina a necessidade de segurança.

A segurança deverá utilizar múltiplas camadas:

```text
Rede interna
     ↓
Autenticação
     ↓
Sessão
     ↓
Autorização
     ↓
Validação
     ↓
Persistência
     ↓
Logs e histórico
```

---

## 3. Acesso anônimo

Funcionalidades de negócio não poderão ser acessadas anonimamente.

Sem autenticação, somente deverão estar disponíveis:

- login;
- configuração inicial do primeiro Administrador quando ainda não existirem usuários;
- recursos estritamente necessários a essas telas.

Serviços, modelos, usuários, configurações, documentos e operações administrativas exigirão autenticação válida.

---

## 4. Autorização no servidor

Permissões deverão ser aplicadas no backend.

Ocultar uma ação no frontend não será considerado mecanismo suficiente de segurança.

Exemplo:

```text
Técnico
→ botão "Reabrir" não aparece

Tentativa direta de executar reabertura
→ servidor rejeita
```

As regras definidas em `user-access-spec.md` deverão ser aplicadas de forma consistente.

---

## 5. Menor privilégio

Cada perfil deverá possuir somente as capacidades necessárias às suas responsabilidades.

Técnico não necessita de permissões para:

- administrar usuários;
- restaurar backups;
- publicar modelos;
- alterar configurações institucionais.

Essas operações permanecem restritas ao Administrador.

---

## 6. Credenciais

Senhas não poderão ser armazenadas de forma recuperável ou legível.

Nem Administradores poderão consultar a senha atual de outro usuário.

O sistema deverá armazenar somente representação adequada para autenticação.

A tecnologia específica será definida posteriormente.

---

## 7. Senhas em dados auxiliares

Senhas jamais poderão ser registradas em:

- logs;
- histórico;
- URLs;
- PDFs;
- relatórios;
- mensagens de erro;
- arquivos exportados de configuração.

---

## 8. Política funcional de senha

A política inicial será proporcional ao contexto do produto.

Requisitos:

- mínimo de 8 caracteres;
- senha vazia proibida;
- nome de acesso não poderá ser utilizado diretamente como senha;
- frases-senha serão permitidas;
- senhas longas deverão ser aceitas;
- não haverá obrigatoriedade artificial de combinação específica de maiúsculas, minúsculas, números e símbolos.

Exemplo aceitável:

```text
cafe-com-leite-2026
```

O limite máximo técnico será definido na implementação.

---

## 9. Primeiro Administrador

Não haverá senha administrativa padrão ou conta pré-configurada com credenciais conhecidas.

Na primeira configuração, o usuário deverá criar a credencial do primeiro Administrador.

Não haverá:

```text
admin / admin
```

ou equivalente.

---

## 10. Alteração da própria senha

O fluxo normal deverá exigir:

- senha atual;
- nova senha;
- confirmação da nova senha.

A senha atual deverá ser validada antes da alteração.

---

## 11. Redefinição administrativa

Administradores poderão redefinir credenciais de outros usuários.

Não poderão visualizar a senha existente.

A nova credencial definida administrativamente deverá ser tratada como temporária e exigir alteração no próximo login.

---

## 12. Sessões

Depois da autenticação, cada usuário possuirá uma sessão.

A sessão deverá:

- identificar o usuário;
- refletir suas permissões;
- não permitir adulteração pelo cliente;
- poder ser encerrada;
- possuir validade limitada;
- poder ser invalidada administrativamente quando necessário.

---

## 13. Logout

A ação Sair deverá invalidar a sessão.

Navegar para páginas anteriores no browser não poderá restaurar autorização válida para operações protegidas.

---

## 14. Expiração por inatividade

As sessões deverão possuir expiração por inatividade.

Valor funcional inicial proposto:

```text
8 horas
```

A configuração poderá ser administrável dentro de limites seguros.

Uso ativo não deverá provocar encerramento inesperado da sessão.

---

## 15. Alteração de perfil

Mudanças de perfil deverão passar a valer sem permitir que sessões antigas mantenham indefinidamente permissões anteriores.

A implementação deverá atualizar ou invalidar essas autorizações de forma previsível.

---

## 16. Desativação de usuário

Um usuário desativado não poderá continuar utilizando indefinidamente uma sessão já existente.

A desativação deverá retirar sua autorização de forma rápida e previsível.

---

## 17. Restauração

Após restauração de backup, todas as sessões existentes deverão ser invalidadas.

Nova autenticação será necessária.

---

## 18. Tentativas de login

O sistema deverá limitar tentativas repetidas de autenticação.

A política funcional inicial será:

```text
5 falhas consecutivas
        ↓
bloqueio temporário
```

A duração técnica do bloqueio será definida posteriormente.

---

## 19. Mensagem de autenticação inválida

A interface deverá evitar revelar se determinado nome de usuário existe.

Mensagem preferencial:

```text
Usuário ou senha inválidos.
```

---

## 20. Último acesso

A administração poderá exibir a data/hora do último acesso conhecido do usuário.

Essa informação será administrativa e não fará parte do histórico operacional de serviços.

---

## 21. Transporte na rede

A arquitetura deverá avaliar formalmente a proteção da comunicação entre navegador/cliente e servidor.

O projeto não deverá assumir automaticamente que comunicação desprotegida é aceitável apenas porque ocorre em LAN.

A decisão técnica será tomada na arquitetura.

---

## 22. Exposição

A versão 1.0 não terá requisito de exposição pública à internet.

A aplicação deverá ser configurada para o ambiente interno necessário.

A documentação deverá desencorajar publicação pública não planejada.

---

## 23. Entrada não confiável

Todo dado recebido do cliente deverá ser tratado como não confiável.

Isso inclui:

- nomes;
- títulos;
- observações;
- cliente;
- equipamento;
- filtros;
- parâmetros;
- configurações;
- arquivos enviados;
- caminhos solicitados.

---

## 24. Validação no backend

Regras importantes deverão ser verificadas novamente pelo backend.

A validação visual do formulário é apenas uma conveniência.

Exemplo:

```text
Frontend → impede campo obrigatório vazio
Backend  → também impede
```

---

## 25. Proteção contra injeção

A implementação deverá proteger contra formas de injeção compatíveis com as tecnologias adotadas.

Áreas relevantes incluem:

- banco de dados;
- comandos de sistema;
- caminhos de arquivos;
- templates;
- geração documental.

---

## 26. Proteção contra XSS

Conteúdo inserido por usuários não poderá transformar-se em código executável no navegador.

Textos como observações deverão ser apresentados de forma segura.

---

## 27. Observações como texto simples

Na versão 1.0:

> Observações e campos textuais comuns serão texto simples.

Não haverá editor HTML ou WYSIWYG para observações.

Essa decisão simplifica:

- segurança;
- apresentação;
- geração de PDF;
- consistência.

---

## 28. Requisições forjadas

Caso a arquitetura utilize sessões em navegador, operações que modificam dados deverão possuir proteção adequada contra requisições forjadas.

O mecanismo dependerá da tecnologia escolhida.

---

## 29. Duplicação acidental de operações

Operações importantes deverão ser protegidas contra execução duplicada acidental.

Exemplos:

- criar dois serviços por duplo clique;
- concluir duas vezes;
- publicar duas revisões;
- executar restauração repetida.

Frontend e backend deverão colaborar para evitar esses estados.

---

## 30. Identificadores não representam autorização

Descobrir um identificador interno ou URL não concede permissão.

Toda operação deverá verificar:

- autenticação;
- autorização;
- estado atual;
- regras de negócio.

---

## 31. Integridade das regras de negócio

O backend deverá impedir estados inválidos mesmo quando uma requisição for construída manualmente.

Exemplos que deverão ser rejeitados:

- concluir serviço com passos Pendentes;
- publicar modelo inválido;
- excluir revisão publicada;
- Técnico reabrir serviço;
- desativar o último Administrador;
- gerar relatório externo contendo observações internas.

---

## 32. Dados internos e documentos externos

A separação entre conteúdo Interno e Cliente representa uma fronteira de segurança.

Observações internas deverão ser excluídas funcionalmente da fonte utilizada pelo Relatório de Serviço.

Não será suficiente apenas escondê-las visualmente.

---

## 33. Pré-visualização e PDF

Pré-visualização e documento final deverão utilizar as mesmas regras de seleção de conteúdo externo.

Não poderá existir diferença de segurança entre os dois mecanismos.

---

## 34. Exportações

Operações de exportação deverão respeitar permissões.

Especial atenção será dada a:

- PDFs;
- backups.

O usuário deverá saber qual conteúdo está sendo retirado da aplicação.

---

## 35. Backups

Backups serão considerados conteúdo sensível.

Podem conter:

- clientes;
- equipamentos;
- observações;
- usuários;
- histórico;
- configurações;
- credenciais protegidas.

Somente Administradores poderão gerenciá-los dentro do sistema.

---

## 36. Disponibilização de backups

Backups não deverão ser colocados diretamente em diretório público do servidor web nem expostos através de URLs previsíveis sem autorização.

---

## 37. Criptografia própria do backup

Criptografia específica do pacote de backup não será requisito funcional obrigatório da versão 1.0.

Sua conveniência será avaliada na arquitetura.

O requisito obrigatório será proteção adequada através do ambiente, permissões e mecanismos do sistema.

---

## 38. Upload da logo

Arquivos de logo deverão ser tratados como entrada não confiável.

A aplicação deverá:

- aceitar apenas formatos suportados;
- validar o conteúdo;
- limitar tamanho;
- controlar o local de armazenamento.

Apenas extensão do arquivo não será considerada validação suficiente.

---

## 39. Arquivos e caminhos

Usuários não deverão controlar livremente caminhos internos do servidor.

A aplicação decidirá onde armazenar:

- logos;
- temporários;
- documentos;
- backups.

---

## 40. Arquivos temporários

Arquivos temporários deverão possuir ciclo de vida controlado.

Não deverão permanecer expostos ou acumulados indefinidamente.

---

## 41. PDFs

Documentos gerados não deverão expor acidentalmente:

- caminhos do servidor;
- stack traces;
- nomes internos de banco;
- configuração técnica;
- observações internas em relatório externo.

---

## 42. Logs técnicos

Logs deverão possuir informação suficiente para diagnóstico sem registrar dados sensíveis desnecessários.

Deverão evitar:

- senhas;
- tokens;
- identificadores de sessão reutilizáveis;
- conteúdo integral de backups;
- observações completas sem necessidade;
- dados pessoais desnecessários.

---

## 43. Identificadores nos logs

Quando necessário, será preferível registrar identificadores técnicos.

Exemplo:

```text
serviceId
userId
operation
timestamp
```

em vez de repetir dados completos do cliente.

---

## 44. Stack traces

Stack traces poderão existir em logs técnicos.

Não deverão ser exibidos diretamente aos usuários finais.

---

## 45. Segredos técnicos

Segredos necessários ao funcionamento do sistema não poderão ser:

- hardcoded no código;
- commitados no GitHub;
- apresentados pela interface;
- registrados em logs.

A forma de configuração será decidida na arquitetura.

---

## 46. Repositório GitHub

O repositório não deverá conter:

- credenciais reais;
- banco de produção;
- backups reais;
- dados reais de clientes;
- chaves privadas;
- configurações com segredos.

---

## 47. Desenvolvimento

Ambientes de desenvolvimento e testes deverão preferencialmente utilizar dados fictícios ou sintéticos.

Dados reais de produção não deverão ser copiados como conjunto normal de desenvolvimento.

---

## 48. Autoria

A autoria de operações será determinada pela sessão autenticada.

O navegador não poderá informar livremente quem foi o autor de uma operação.

---

## 49. Campos controlados pelo sistema

Informações como:

- ID;
- criado por;
- datas do sistema;
- autor da publicação;
- autor da conclusão;

serão determinadas no ambiente central.

Não serão confiadas aos valores enviados pelo cliente.

---

## 50. Fonte temporal

Eventos funcionais relevantes deverão utilizar uma fonte temporal central consistente.

O relógio do navegador não será considerado fonte confiável para autoria temporal dos registros.

---

## 51. Concorrência e segurança

Operações deverão ser validadas também contra o estado atual do registro.

Exemplo:

```text
Usuário abriu serviço Em andamento

Outro usuário concluiu o serviço

Tela antiga tenta modificar um passo
        ↓
Servidor rejeita operação incompatível
```

---

## 52. Conflitos

Quando uma operação falhar devido a alteração concorrente, a interface deverá informar a situação em linguagem compreensível.

Exemplo:

```text
Este serviço foi alterado por outro usuário.

Os dados serão atualizados antes de continuar.
```

---

## 53. Auditoria proporcional

Não haverá registro de cada clique da aplicação.

Serão preservados eventos relevantes para:

- autoria;
- segurança;
- administração;
- reconstrução da história operacional.

A auditoria deverá ser útil sem gerar complexidade excessiva.

---

## 54. Confirmações

Operações de alto impacto continuarão exigindo confirmação explícita.

Exemplos:

- restaurar backup;
- cancelar serviço;
- reabrir;
- arquivar modelo;
- desativar usuário;
- descartar alterações.

Confirmações complementam, mas não substituem, autorização.

---

## 55. Proteção por preservação histórica

Algumas decisões já tomadas também protegem contra perda acidental:

```text
Usuário    → desativar, não excluir
Modelo     → arquivar, não excluir após publicação
Revisão    → preservar
Serviço    → cancelar
Conclusão  → preservar historicamente
```

---

## 56. Superusuário

Não haverá:

- superusuário oculto;
- senha mestre;
- conta invisível;
- bypass permanente de autorização.

Administração ocorrerá através de contas normais com perfil Administrador.

---

## 57. Recuperação administrativa

A regra de sempre existir ao menos um Administrador Ativo reduz o risco de perda de administração.

Cenários extremos de recuperação poderão possuir procedimento técnico documentado no futuro, sem backdoor permanente embutido.

---

## 58. Dependências

Bibliotecas e componentes deverão ser avaliados também considerando:

- manutenção ativa;
- histórico de segurança;
- necessidade;
- possibilidade de atualização;
- funcionamento offline;
- compatibilidade com a arquitetura.

Dependências abandonadas deverão ser evitadas.

---

## 59. Atualizações de segurança

A solução deverá permitir atualizar dependências ao longo do ciclo de vida do produto.

A manutenção de segurança faz parte da evolução normal do Reset Service.

---

## 60. Configuração segura por padrão

Quando houver mais de uma configuração possível, a instalação inicial deverá preferir opções razoavelmente seguras sem prejudicar a operação.

---

## 61. Acesso negado

Quando um usuário autenticado tentar uma ação não permitida, a interface deverá informar adequadamente.

Exemplo:

```text
Você não possui permissão para reabrir este serviço.
```

---

## 62. Registro de violações relevantes

Tentativas relevantes de operações não autorizadas poderão ser registradas tecnicamente para diagnóstico.

Não será necessário registrar toda navegação normal.

---

## 63. Responsabilidades externas

Algumas proteções dependem também do ambiente onde o produto será instalado.

Incluem:

- atualizações do Windows;
- firewall;
- permissões do sistema de arquivos;
- proteção física;
- configuração da LAN;
- mecanismos externos de backup.

O futuro guia de implantação deverá documentar essas responsabilidades.

---

## 64. Requisitos Consolidados

| Área | Requisito |
|---|---|
| Autenticação | Obrigatória |
| Acesso anônimo ao negócio | Proibido |
| Autorização | Backend |
| Perfis | Administrador / Técnico |
| Menor privilégio | Obrigatório |
| Senhas em texto legível | Proibido |
| Senhas em logs | Proibido |
| Senha mínima | 8 caracteres |
| Frases-senha | Permitidas |
| Senha mestre | Não existe |
| Sessão | Expiração e invalidação |
| Inatividade inicial | 8 horas |
| Falhas consecutivas de login | 5 antes de proteção temporária |
| Enumeração de usuário | Evitar |
| Validação backend | Obrigatória |
| Proteção contra injeção | Obrigatória |
| Proteção contra XSS | Obrigatória |
| Observações | Texto simples |
| HTML livre em observações | Não |
| Dados internos em relatório externo | Proibidos |
| Backups | Conteúdo sensível |
| Uploads | Validados e limitados |
| Segredos no Git | Proibidos |
| Dados reais em desenvolvimento | Evitar |
| Stack trace ao usuário | Proibido |
| Autoria | Sessão autenticada |
| Fonte temporal | Ambiente central |
| Superusuário oculto | Não |
| Auditoria | Proporcional |

---

## 65. Regras Fundamentais

1. Rede interna não substitui autenticação.
2. Funcionalidades de negócio exigem autenticação.
3. Autorização será aplicada no backend.
4. Interface não será considerada mecanismo suficiente de autorização.
5. Será aplicado menor privilégio.
6. Senhas não serão armazenadas de maneira legível.
7. Administradores não poderão visualizar senhas existentes.
8. Senhas não aparecerão em logs.
9. Não haverá credenciais administrativas padrão.
10. Senhas terão mínimo inicial de 8 caracteres.
11. Não haverá regras artificiais obrigatórias de composição.
12. Frases-senha e senhas longas serão permitidas.
13. Credenciais temporárias exigirão troca.
14. Sessões possuirão expiração e invalidação.
15. Desativação de conta deverá retirar autorização.
16. Após restauração, sessões serão invalidadas.
17. Tentativas consecutivas de login possuirão proteção temporária.
18. Login inválido não deverá revelar a existência da conta.
19. A aplicação não será destinada à exposição pública na internet.
20. Toda entrada será considerada não confiável.
21. Regras importantes serão validadas no backend.
22. Proteção contra injeções será obrigatória.
23. Proteção contra XSS será obrigatória.
24. Observações utilizarão texto simples.
25. Proteções contra requisições forjadas serão implementadas conforme a arquitetura.
26. Operações críticas deverão resistir à duplicação acidental.
27. Identificadores não representam autorização.
28. Conteúdo interno será estruturalmente excluído do relatório externo.
29. Prévia e PDF utilizarão a mesma política de conteúdo.
30. Backups serão tratados como dados sensíveis.
31. Uploads deverão ser validados.
32. Segredos não serão commitados.
33. Dados reais de produção não deverão ser usados normalmente no desenvolvimento.
34. Regras de negócio deverão resistir a requisições manuais inválidas.
35. Autoria será determinada pela sessão.
36. Datas relevantes serão determinadas pelo ambiente central.
37. Operações com estado desatualizado poderão ser rejeitadas.
38. Auditoria será proporcional.
39. Não haverá superusuário oculto ou senha mestre.
40. Segurança do produto deverá ser complementada pela segurança do ambiente de implantação.

---

## 66. Estado da Decisão

**PLANNING-011 — Segurança e Proteção de Dados: CONCLUÍDA E APROVADA.**

Este documento passa a ser referência obrigatória para arquitetura, implementação, revisão de código, testes e implantação.