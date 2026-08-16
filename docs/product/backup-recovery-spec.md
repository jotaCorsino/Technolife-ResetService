# Reset Service — Backup and Recovery Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação de Backup, Restauração e Recuperação Operacional  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referências:** `product-spec.md`, `non-functional-requirements.md`, demais especificações funcionais do produto.

---

## 1. Objetivo

Este documento define o comportamento funcional das capacidades de:

- backup;
- backup automático;
- backup manual;
- retenção;
- validação;
- restauração;
- recuperação após falhas;
- proteção antes de atualizações;
- importação e exportação de backups.

A versão 1.0 possuirá suporte a backup e restauração, porém o uso dessa capacidade será opcional.

---

## 2. Princípio central

O Reset Service deverá disponibilizar mecanismos de proteção e recuperação dos dados sem obrigar a Technolife a utilizá-los.

A decisão operacional será:

> A capacidade de backup faz parte do produto, mas sua utilização é opcional e controlada pelo Administrador.

O sistema deverá continuar funcionando normalmente mesmo que o backup automático esteja desativado.

---

## 3. Conteúdo protegido

Um backup completo deverá conter tudo que for necessário para reconstruir o estado persistente do Reset Service.

Isso inclui, conforme aplicável:

- serviços;
- roteiros copiados para serviços;
- estados dos passos;
- observações;
- histórico;
- conclusões;
- modelos;
- revisões;
- usuários;
- dados necessários à autenticação;
- configurações;
- informações institucionais;
- configurações documentais;
- snapshots históricos necessários;
- arquivos persistentes essenciais.

A composição física será definida na arquitetura.

---

## 4. Consistência

Cada backup deverá representar uma fotografia consistente do sistema.

Não será aceitável produzir estados parcialmente relacionados, por exemplo:

```text
Serviço = Concluído
Conclusão = ausente
Histórico = incompleto
```

A arquitetura deverá garantir consistência durante a criação do backup.

---

## 5. Tipos de backup

Existirão conceitualmente:

```text
Backup automático
Backup manual
Backup pré-atualização
Backup pré-restauração
```

Todos deverão ser recuperáveis através do mesmo mecanismo lógico de restauração, quando compatíveis.

---

## 6. Backup automático

O Administrador poderá ativar ou desativar o backup automático.

Exemplo:

```text
Configurações
→ Sistema
→ Backup

Backup automático
[ Ativado / Desativado ]
```

---

## 7. Backup automático desativado

Quando estiver Desativado:

- nenhum backup periódico será executado;
- o funcionamento normal do Reset Service não será bloqueado;
- serviços e demais funções continuarão disponíveis;
- não haverá alertas recorrentes de backup atrasado;
- falha de destino de backup não afetará a operação normal.

A interface deverá apenas deixar o estado claro.

Exemplo:

```text
Backup automático
Desativado

O Reset Service está operando sem backups automáticos.
```

---

## 8. Backup automático ativado

Quando Ativado, o Administrador poderá configurar pelo menos:

- horário;
- destino;
- retenção.

A frequência padrão da versão 1.0 será diária.

Como os componentes hospedados existem somente enquanto `ResetService.exe` estiver em execução, o agendamento não acordará nem iniciará remotamente a aplicação.

Se o horário ocorrer enquanto a aplicação estiver fechada, a próxima inicialização poderá executar **no máximo um backup automático pendente**, desde que a função continue habilitada e o destino esteja disponível. O sistema não criará uma execução retroativa para cada dia perdido.

---

## 9. Horário

O horário será configurável.

Valor inicial sugerido:

```text
02:00
```

A Technolife poderá alterá-lo conforme o horário real de funcionamento do servidor.

Não será necessário disponibilizar agendador avançado ou expressão de cron.

---

## 10. Retenção

Quando o backup automático estiver ativo, a configuração inicial proposta será:

```text
30 backups automáticos
```

Ao ultrapassar a retenção configurada, os backups automáticos mais antigos poderão ser removidos.

---

## 11. Backups especiais

Backups:

- manuais;
- pré-atualização;
- pré-restauração;

não deverão ser removidos automaticamente pela retenção dos backups periódicos.

Sua remoção poderá ocorrer posteriormente através de ação administrativa explícita.

---

## 12. Backup manual

O backup manual continuará disponível independentemente de o backup automático estar ativado.

Exemplo:

```text
[ Criar backup agora ]
```

Isso permite que a Technolife utilize a capacidade somente quando considerar necessário.

---

## 13. Uso comum do backup manual

Poderá ser utilizado antes de:

- atualização;
- manutenção;
- alteração importante;
- migração;
- teste de restauração;
- outra intervenção administrativa.

---

## 14. Destino

O destino deverá ser configurável quando necessário.

A solução deverá permitir utilizar, dependendo da infraestrutura:

- armazenamento local;
- outra unidade física;
- pasta de rede;
- outro destino acessível pelo ambiente central.

A implementação específica dependerá da arquitetura escolhida.

---

## 15. Armazenamento no mesmo disco

O Reset Service não deverá impedir o uso de backup armazenado no mesmo disco do sistema.

Porém, quando isso for identificável, poderá informar de forma discreta:

> O backup está armazenado no mesmo dispositivo do sistema e pode não proteger contra falha física desse dispositivo.

O aviso não impedirá a operação.

---

## 16. Metadados

Cada backup deverá possuir informações suficientes para identificação.

Pelo menos:

- data/hora;
- tipo;
- versão do Reset Service;
- resultado;
- tamanho aproximado, quando disponível;
- estado de validação.

Exemplo:

```text
13/08/2026 02:00
Automático
Reset Service 1.0.2
145 MB
Válido
```

---

## 17. Histórico de backups

O Administrador deverá poder consultar os backups conhecidos pelo sistema.

Exemplo:

```text
BACKUPS

13/08/2026 02:00
Automático
Válido

12/08/2026 15:42
Manual
Criado por Carlos
Válido
```

---

## 18. Validação

Após a criação, o sistema deverá realizar verificações básicas para determinar se o pacote aparenta estar utilizável.

Fluxo conceitual:

```text
Criar
  ↓
Finalizar
  ↓
Validar
  ↓
Disponibilizar
```

O mecanismo técnico será definido na arquitetura.

---

## 19. Falhas

Um backup incompleto ou inválido não deverá ser apresentado como concluído com sucesso.

Estados possíveis poderão incluir:

- Válido;
- Falhou;
- Inválido.

Falhas deverão ser registradas para diagnóstico.

---

## 20. Alertas de falha

Quando o backup automático estiver Ativado e uma execução falhar, o Administrador deverá receber uma indicação clara.

Exemplo:

```text
Backup automático falhou.

Último backup válido:
12/08/2026 às 02:00
```

Técnicos não precisam receber alertas administrativos desse tipo.

---

## 21. Atualizações

Antes de atualizações que possam modificar os dados persistidos, o Reset Service deverá recomendar fortemente a criação de um backup.

Backup não será requisito obrigatório para prosseguir.

---

## 22. Prosseguir sem backup

Caso não exista backup recente, o Administrador poderá receber confirmação:

```text
Continuar sem backup?

Se a atualização apresentar problemas,
pode não existir um ponto de recuperação.
```

O Administrador poderá optar por continuar.

---

## 23. Backup pré-atualização

Quando escolhido ou quando o processo de atualização suportar sua criação automática, um backup pré-atualização deverá ser identificado separadamente.

Exemplo conceitual:

```text
Tipo:
Pré-atualização

Versão:
1.0.3

Data:
13/08/2026 10:30
```

---

## 24. Restauração

A restauração será uma operação exclusiva do Administrador.

Será considerada uma operação de alto impacto.

---

## 25. Restauração integral

A versão 1.0 suportará somente restauração completa.

Não haverá restauração individual de:

- serviço;
- modelo;
- usuário;
- observação;
- revisão.

---

## 26. Ausência de merge

A versão 1.0 também não tentará mesclar:

```text
estado atual
+
parte de um backup antigo
```

Restaurar significa retornar o estado persistente do sistema ao ponto representado pelo backup escolhido.

---

## 27. Consequência temporal

Se o Administrador restaurar:

```text
Backup:
10/08/2026 02:00
```

registros posteriores a esse momento poderão deixar de fazer parte do estado ativo.

A interface deverá explicar essa consequência claramente.

---

## 28. Confirmação forte

A restauração deverá exigir uma confirmação explícita e mais forte do que uma ação comum.

Exemplo:

```text
Restaurar backup de 10/08/2026 às 02:00?

O estado atual do Reset Service será substituído
pelo conteúdo deste backup.
```

---

## 29. Validação antes da restauração

O backup escolhido deverá ser verificado antes da substituição do estado atual.

Ordem conceitual:

```text
Selecionar
   ↓
Validar
   ↓
Confirmar
   ↓
Restaurar
```

---

## 30. Backup pré-restauração

Sempre que possível, antes da restauração o sistema deverá oferecer ou realizar uma fotografia do estado atual.

Ela será identificada como:

```text
Backup pré-restauração
```

Esse mecanismo aumenta a possibilidade de recuperação caso tenha sido escolhido um ponto incorreto.

---

## 31. Restauração sem backup prévio

Assim como nas atualizações, a ausência de possibilidade de criar um backup pré-restauração não deverá necessariamente impedir a operação.

O Administrador deverá ser informado do risco e decidir se deseja prosseguir.

---

## 32. Modo de manutenção

Durante a substituição dos dados, o sistema deverá impedir alterações concorrentes.

Conceitualmente, entrará temporariamente em:

```text
Modo de manutenção
```

A implementação será definida posteriormente.

---

## 33. Sessões

Após uma restauração concluída, sessões existentes deverão ser encerradas.

Os usuários deverão autenticar-se novamente.

Isso evita manter sessões relacionadas a um estado anterior de usuários, permissões ou dados.

---

## 34. Falha durante restauração

A restauração deverá ser projetada para evitar estado parcialmente recuperado.

A operação deverá:

- concluir integralmente; ou
- deixar o ambiente em condição conhecida e recuperável.

---

## 35. Recuperação após falha total

O backup deverá servir também para recuperação de perda completa do ambiente central.

Fluxo esperado:

```text
Falha do servidor
       ↓
Preparar novo ambiente
       ↓
Instalar versão compatível
       ↓
Importar backup
       ↓
Restaurar
       ↓
Retomar operação
```

Esse procedimento deverá ser documentado no guia administrativo futuro.

---

## 36. Independência da instalação original

Um backup não poderá depender exclusivamente da instalação específica que o criou.

Ele deverá poder ser utilizado por outra instalação compatível do Reset Service.

---

## 37. Compatibilidade de versão

Todo backup deverá identificar a versão do produto que o criou.

Antes da restauração, o Reset Service deverá verificar se o pacote é compatível.

Regras técnicas de compatibilidade e migração serão definidas posteriormente.

---

## 38. Exportação

O Administrador poderá obter uma cópia portátil de um backup quando a arquitetura permitir.

Conceitualmente:

```text
[ Exportar backup ]
```

Isso poderá ser usado para transferi-lo para armazenamento controlado externo.

---

## 39. Importação

Também deverá existir capacidade de importar um pacote compatível para avaliação e possível restauração.

Fluxo:

```text
Importar arquivo
      ↓
Validar
      ↓
Mostrar informações
      ↓
Escolher se deseja restaurar
```

Importar não significa restaurar imediatamente.

---

## 40. Segurança dos backups

Um backup poderá conter:

- informações de clientes;
- equipamentos;
- observações internas;
- usuários;
- histórico;
- credenciais protegidas;
- configurações.

Portanto, deverá ser tratado como conteúdo sensível.

A proteção técnica será definida na especificação de segurança.

---

## 41. PDFs e arquivos históricos

Como PDFs podem ser regenerados, eles não são necessariamente a fonte principal de recuperação.

Entretanto, tudo que for indispensável para reproduzir corretamente conclusões históricas deverá estar protegido, incluindo quando necessário:

- snapshots;
- logos históricas;
- configurações documentais históricas;
- arquivos que não possam ser reconstruídos.

---

## 42. Espaço utilizado

A área administrativa poderá informar:

- quantidade de backups;
- espaço aproximado utilizado.

Não será necessário desenvolver gerenciamento avançado de armazenamento.

---

## 43. Falta de espaço

Quando a criação de um backup falhar por falta de espaço:

- backups válidos anteriores não deverão ser corrompidos;
- o erro deverá ser registrado;
- o Administrador deverá ser informado, quando o backup automático estiver habilitado.

---

## 44. Uso durante backup

Sempre que a tecnologia permitir, backups deverão poder ser criados enquanto usuários continuam trabalhando normalmente.

A execução diária não deverá exigir logout coletivo.

A arquitetura determinará o mecanismo adequado.

---

## 45. Indisponibilidade durante restauração

A restauração poderá tornar o sistema temporariamente indisponível.

Essa indisponibilidade é aceitável por se tratar de operação rara e administrativa.

---

## 46. Interface funcional

A área poderá seguir conceitualmente:

```text
CONFIGURAÇÕES > SISTEMA > BACKUP

Backup automático
[ Desativado ]

O Reset Service está operando sem backups automáticos.

[ Ativar ]

[ Criar backup agora ]

────────────────────────────

Backups disponíveis

13/08/2026 10:40
Manual
Válido
[ Detalhes ] [ Exportar ] [ Restaurar ]
```

Com automático ativo:

```text
Backup automático
[ Ativado ]

Horário
02:00

Retenção
30

Destino
[...]

Último backup válido
Hoje às 02:00

Próximo backup
Amanhã às 02:00
```

A aparência definitiva será definida posteriormente.

---

## 47. Relação com outros mecanismos de infraestrutura

A capacidade de backup do Reset Service não impede a utilização adicional de:

- backup do servidor;
- snapshots;
- ferramentas corporativas de proteção;
- cópias de infraestrutura.

Essas estratégias poderão coexistir.

---

## 48. Regras Fundamentais

1. A versão 1.0 terá capacidade de backup e restauração.
2. Utilizar backup será opcional.
3. O backup automático poderá ser ativado ou desativado pelo Administrador.
4. Desativar backup automático não limitará nenhuma função operacional.
5. Backup manual permanecerá disponível sob demanda.
6. Quando ativado, o backup automático será diário.
7. O horário será configurável.
8. Se o horário ocorrer com a aplicação fechada, a próxima inicialização poderá executar no máximo um backup automático pendente.
9. Não haverá uma execução retroativa para cada dia perdido.
10. A retenção inicial proposta será de 30 backups automáticos.
11. Backups manuais não serão removidos pela retenção automática.
12. Backups pré-atualização e pré-restauração serão tratados separadamente.
13. O destino poderá ser configurável.
14. O sistema permitirá uso mesmo que o backup esteja no mesmo armazenamento principal.
15. Backups deverão representar estado consistente.
16. Backups deverão possuir metadados.
17. Backups deverão passar por validação básica.
18. Falhas não poderão ser apresentadas como sucesso.
19. Alertas de backup serão relevantes somente quando a função automática estiver ativada.
20. Atualizações recomendarão backup, mas poderão prosseguir sem ele mediante decisão administrativa.
21. Somente Administradores poderão restaurar.
22. A restauração será integral.
23. Não haverá recuperação granular na versão 1.0.
24. Não haverá merge entre backup e estado atual.
25. O impacto temporal da restauração deverá ser explicado.
26. O backup deverá ser validado antes da restauração.
27. Sempre que possível será criado ou oferecido backup pré-restauração.
28. A restauração poderá utilizar modo de manutenção.
29. Sessões deverão ser encerradas após a restauração.
30. Backups deverão permitir recuperação em outra instalação compatível.
31. A versão de origem do backup deverá ser identificável.
32. Importação não realizará restauração automaticamente.
33. Backups poderão ser exportados quando aplicável.
34. Backups deverão ser tratados como dados sensíveis.
35. A capacidade de backup não substitui outras estratégias de proteção da infraestrutura.

---

## 49. Estado da Decisão

**PLANNING-010 — Backup, Restauração e Recuperação Operacional: CONCLUÍDA E APROVADA.**

O uso de backup é opcional. O produto fornece a capacidade e os mecanismos de recuperação, mas a Technolife decide quando e como utilizá-los.
