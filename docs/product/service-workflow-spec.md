# Reset Service — Service Workflow Specification

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Especificação Funcional do Roteiro de Serviço  
**Versão do documento:** 1.0  
**Status:** Aprovado  
**Referência:** `product-spec.md`

---

## 1. Objetivo

Este documento define o comportamento funcional do roteiro de execução do Reset Service.

Seu escopo compreende:

- etapas;
- passos;
- estados;
- progresso;
- observações;
- navegação;
- personalização;
- conclusão do roteiro.

Este documento não define arquitetura, banco de dados, tecnologias ou detalhes de implementação.

---

## 2. Estrutura do roteiro

Um roteiro será composto por uma sequência ordenada de etapas.

Cada etapa será composta por uma sequência ordenada de passos.

```text
Roteiro
│
├── Etapa 1
│   ├── Passo 1
│   ├── Passo 2
│   └── Passo 3
│
├── Etapa 2
│   ├── Passo 1
│   └── Passo 2
│
└── Etapa N
```

Durante a execução de um serviço, cada etapa será apresentada conceitualmente como uma página ou folha do roteiro.

---

## 3. Etapa

Uma etapa representa um agrupamento lógico de ações relacionadas.

Exemplos:

- Preparação;
- Backup;
- Instalação do sistema;
- Configuração;
- Aplicativos;
- Validação.

## 3.1. Propriedades funcionais

Uma etapa possuirá:

- título;
- descrição ou instrução opcional;
- posição dentro do roteiro;
- conjunto ordenado de passos;
- observações;
- progresso calculado;
- estado calculado.

O título será obrigatório.

A descrição será opcional.

---

## 4. Estado da etapa

O estado da etapa não será selecionado manualmente pelo usuário.

Ele será calculado automaticamente a partir dos passos existentes.

Os estados serão:

- Não iniciada;
- Em andamento;
- Concluída;
- Não aplicável.

---

## 4.1. Não iniciada

Uma etapa será considerada **Não iniciada** quando possuir passos aplicáveis e nenhum deles tiver sido concluído.

Exemplo:

```text
○ Configuração

☐ Instalar drivers
☐ Executar atualizações
☐ Instalar aplicativos
```

---

## 4.2. Em andamento

Uma etapa será considerada **Em andamento** quando:

- possuir pelo menos um passo concluído; e
- ainda possuir pelo menos um passo aplicável pendente.

Exemplo:

```text
● Configuração

☑ Instalar drivers
☐ Executar atualizações
☐ Instalar aplicativos
```

---

## 4.3. Concluída

Uma etapa será considerada **Concluída** quando todos os seus passos aplicáveis estiverem concluídos.

Passos classificados como Não aplicáveis não impedirão sua conclusão.

---

## 4.4. Não aplicável

Uma etapa será considerada **Não aplicável** quando todos os seus passos estiverem marcados como Não aplicáveis.

Esse estado deverá ser visualmente diferente de uma etapa concluída.

---

## 5. Passo

O passo será a menor unidade operacional do roteiro.

Ele representa uma ação que deve ser executada, verificada ou deliberadamente classificada como não aplicável.

Exemplo:

```text
☐ Instalar Google Chrome
```

Um passo poderá também apresentar instrução complementar:

```text
Instalar Google Chrome

Instalar a versão atual disponível e
configurá-la como navegador padrão.
```

---

## 6. Propriedades funcionais do passo

Um passo possuirá:

- título;
- descrição ou instrução opcional;
- posição dentro da etapa;
- estado operacional;
- observações.

O título será obrigatório.

A descrição será opcional.

---

## 7. Estados do passo

Existirão três estados:

- Pendente;
- Concluído;
- Não aplicável.

A versão 1.0 não utilizará estados adicionais como:

- falhou;
- ignorado;
- parcial;
- bloqueado;
- crítico;
- recomendado.

Situações excepcionais poderão ser explicadas através de observações.

---

## 7.1. Pendente

Representa uma ação aplicável ao serviço que ainda precisa ser executada ou verificada.

```text
☐ Instalar aplicativo
```

---

## 7.2. Concluído

Representa uma ação que foi executada ou verificada.

```text
☑ Instalar aplicativo
```

---

## 7.3. Não aplicável

Representa uma ação existente no roteiro, mas que não é necessária para aquele serviço específico.

Exemplo:

```text
— Configurar impressora
```

Uma observação poderá explicar o motivo:

> Cliente não utiliza impressora nesta estação.

---

## 8. Obrigatoriedade dos passos

A versão 1.0 não utilizará categorias como:

- obrigatório;
- opcional;
- recomendado;
- importante;
- crítico.

A regra será:

> Todo passo aplicável deve ser concluído.

Se um passo não for necessário para determinado serviço, deverá ser explicitamente marcado como **Não aplicável**.

Assim:

```text
Aplicável
   ↓
Pendente
   ↓
Concluído
```

ou:

```text
Não necessário
      ↓
Não aplicável
```

---

## 9. Alteração de estado

Enquanto o serviço estiver aberto para execução, o usuário poderá corrigir livremente o estado de um passo.

Transições permitidas incluem:

```text
Pendente ↔ Concluído

Pendente ↔ Não aplicável

Concluído ↔ Não aplicável
```

Marcar um passo por engano não deverá exigir qualquer processo especial de correção.

---

## 10. Bloqueio após conclusão

Quando o serviço estiver no estado **Concluído**, os passos e demais informações operacionais deverão ficar protegidos contra alterações comuns.

Para modificar o roteiro novamente será necessário reabrir explicitamente o serviço.

```text
Concluído
    ↓
Reabrir serviço
    ↓
Em andamento
```

---

## 11. Cálculo de progresso

O progresso será calculado com base nos passos aplicáveis.

Passos Não aplicáveis serão excluídos do cálculo.

A fórmula conceitual será:

```text
                Passos concluídos
Progresso = ─────────────────────────
             Total de passos aplicáveis
```

---

## 11.1. Exemplo

Um serviço possui:

```text
10 passos existentes

8 concluídos
1 pendente
1 não aplicável
```

O total aplicável é:

```text
9
```

Portanto:

```text
8 / 9 = 88,9%
```

O passo Não aplicável não reduz o progresso.

---

## 12. Peso dos passos

Todos os passos terão o mesmo peso no cálculo de progresso da versão 1.0.

Não existirão pesos personalizados.

Por exemplo, um passo não poderá valer duas ou três vezes mais que outro.

A inclusão de pesos somente deverá ser considerada futuramente caso exista uma necessidade operacional comprovada.

---

## 13. Progresso geral do serviço

O progresso geral não será calculado através da média do progresso das etapas.

Ele será calculado utilizando todos os passos aplicáveis existentes no roteiro.

Isso evita distorções quando etapas possuem quantidades muito diferentes de passos.

Exemplo:

```text
Etapa A
2 passos

Etapa B
18 passos
```

As duas etapas não deverão representar automaticamente 50% do serviço cada.

---

## 14. Progresso da etapa

O progresso de uma etapa utilizará a mesma regra do progresso geral, mas considerando apenas seus próprios passos.

Exemplo:

```text
ETAPA: Aplicativos

☑ Chrome
☑ Office
☐ Adobe Reader
— Teams
```

Temos:

```text
3 passos aplicáveis
2 concluídos
```

Portanto:

```text
66,7%
```

---

## 15. Etapa totalmente não aplicável

Quando todos os passos de uma etapa forem Não aplicáveis, a etapa deverá apresentar o estado:

**Não aplicável**

Não deverá ser apresentada simplesmente como 100% concluída.

Isso permite distinguir:

```text
Tudo foi executado
```

de:

```text
Nada desta etapa era necessário
```

---

## 16. Etapas vazias

Durante a edição de um modelo, o sistema poderá permitir temporariamente uma etapa sem passos.

Exemplo:

```text
Nova etapa
0 passos
```

Essa condição será permitida apenas durante a construção ou edição do modelo.

Um modelo disponível para utilização deverá possuir pelo menos:

- uma etapa;
- um passo válido.

Etapas vazias não deverão fazer parte de um roteiro operacional iniciado.

---

## 17. Observações

Observações serão registros associados ao roteiro.

Elas poderão existir em três níveis:

```text
Serviço
│
├── Observações do serviço
│
├── Etapa
│   ├── Observações da etapa
│   │
│   └── Passo
│       └── Observações do passo
```

---

## 18. Observação do serviço

Representa informação relacionada ao serviço como um todo.

Exemplo:

> Equipamento recebido sem fonte de alimentação.

---

## 19. Observação da etapa

Representa informação relacionada a um conjunto de atividades.

Exemplo:

> Configuração parcialmente executada enquanto aguardava credenciais do cliente.

---

## 20. Observação do passo

Representa informação relacionada diretamente a uma ação.

Exemplo:

> Licença informada inicialmente pelo cliente não foi aceita.

---

## 21. Observações como registros individuais

As observações não deverão funcionar apenas como um único campo de texto continuamente sobrescrito.

Cada observação deverá representar um registro individual.

Exemplo:

```text
Microsoft Office

João • 09:42
Licença fornecida inicialmente não foi aceita.

João • 10:15
Cliente forneceu nova licença e a ativação
foi concluída.
```

---

## 22. Propriedades de uma observação

Uma observação deverá possuir conceitualmente:

- texto;
- autor;
- data e hora;
- nível de associação;
- visibilidade.

O nível poderá ser:

- serviço;
- etapa;
- passo.

---

## 23. Visibilidade de observações

Existirão duas classificações:

- Interna;
- Cliente.

---

## 23.1. Interna

Informação destinada exclusivamente ao uso interno da Technolife.

Poderá aparecer:

- na interface interna;
- no histórico;
- no registro interno em PDF.

Não poderá aparecer no relatório destinado ao cliente.

---

## 23.2. Cliente

Informação adequada para compartilhamento externo.

Poderá aparecer:

- na interface;
- no registro interno;
- no relatório do cliente.

Fluxo conceitual:

```text
Observação interna
       └──────────────→ Registro interno

Observação cliente
       ├──────────────→ Registro interno
       └──────────────→ Relatório cliente
```

---

## 24. Edição de observações

Enquanto o serviço estiver aberto, uma observação poderá ser:

- criada;
- editada;
- removida.

Após a conclusão do serviço, deverá ser protegida juntamente com o restante do roteiro.

A versão 1.0 não exigirá histórico completo das diferentes versões de texto de cada observação.

---

## 25. Reordenação de etapas

Etapas poderão ser reorganizadas durante:

- edição de modelos;
- personalização de serviços abertos.

Conceitualmente:

```text
≡ Preparação
≡ Backup
≡ Formatação
≡ Configuração
≡ Validação
```

Alterar a ordem dentro de um serviço deverá caracterizar personalização daquele roteiro.

A alteração de posição não deverá modificar o progresso.

---

## 26. Reordenação de passos

Os passos também poderão ser reorganizados dentro de sua etapa.

Exemplo:

```text
≡ Instalar drivers
≡ Executar atualizações
≡ Instalar Office
≡ Configurar impressoras
```

A reordenação deverá alterar apenas a sequência de apresentação e execução sugerida.

---

## 27. Adição de etapas e passos durante um serviço

Enquanto o serviço estiver aberto, será possível adicionar novas etapas e novos passos.

Um novo passo deverá iniciar no estado:

**Pendente**

Assim que for adicionado, ele passará a participar imediatamente do cálculo de progresso.

Exemplo:

Antes:

```text
10 / 10 = 100%
```

Novo passo adicionado:

```text
10 / 11 = 90,9%
```

Esse comportamento é esperado porque existe agora uma nova ação pendente.

---

## 28. Remoção durante o serviço

Será possível remover etapas e passos durante a personalização do roteiro.

A remoção de um elemento sem execução ou informações associadas poderá utilizar um fluxo simples.

Elementos que já possuam:

- estado Concluído;
- estado Não aplicável;
- observações;
- informações operacionais relevantes;

deverão exigir confirmação antes de serem removidos.

A remoção deverá caracterizar personalização do roteiro.

---

## 29. Edição durante o serviço

Enquanto o serviço estiver aberto, será possível editar:

- título de etapa;
- descrição de etapa;
- título de passo;
- instrução de passo.

Essas alterações:

- pertencem somente ao serviço;
- não alteram o modelo de origem;
- caracterizam personalização.

Após a conclusão, novas alterações exigirão reabertura.

---

## 30. Navegação entre etapas

A navegação será livre.

O usuário poderá utilizar:

- etapa anterior;
- próxima etapa;
- seleção direta através da navegação superior.

Exemplo:

```text
●━━━━●━━━━◉━━━━○━━━━○
1    2    3    4    5
```

O usuário poderá selecionar qualquer etapa disponível.

Não será obrigatório concluir uma etapa antes de consultar ou executar outra.

---

## 31. Pendências e navegação

A existência de passos pendentes não deverá bloquear a navegação.

O sistema poderá comunicar claramente:

> Esta etapa ainda possui 2 passos pendentes.

Mas deverá permitir que o técnico avance ou retorne quando necessário.

O Reset Service é um sistema de roteiro e controle, e não um fluxo sequencial rígido.

---

## 32. Conclusão de uma etapa

Quando o último passo aplicável de uma etapa for concluído, a etapa deverá mudar automaticamente para o estado **Concluída**.

A interface não deverá avançar automaticamente para a próxima página.

Em vez disso, deverá apresentar claramente a conclusão e destacar a próxima ação.

Exemplo:

```text
✓ Etapa concluída

[ Próxima etapa → ]
```

Isso mantém o controle da navegação com o usuário e permite registrar observações antes de sair da página.

---

## 33. Regra de conclusão do serviço

Um serviço somente poderá ser concluído normalmente quando não houver nenhum passo no estado **Pendente**.

São aceitos:

```text
Concluído
Não aplicável
```

Não é aceito:

```text
Pendente
```

Assim:

```text
Pendente       → impede conclusão
Concluído      → permite conclusão
Não aplicável  → permite conclusão
```

---

## 34. Significado de 100%

A indicação de 100% deverá significar que não existem ações aplicáveis pendentes.

Um serviço somente poderá atingir 100% quando todos os seus passos estiverem:

- concluídos; ou
- classificados como não aplicáveis.

Essa regra garante que a barra de progresso tenha significado operacional real.

---

## 35. Relação com modelos

Em um modelo:

- etapas definem a estrutura;
- passos definem o procedimento;
- não existem estados operacionais de execução.

Quando o modelo origina um serviço:

```text
MODELO

Etapa
└── Passo

      ↓ cópia

SERVIÇO

Etapa
└── Passo
    ├── estado
    └── observações
```

Estados e registros de execução pertencem ao serviço.

---

## 36. Relação com personalização

Qualquer alteração estrutural realizada no roteiro de um serviço deverá permanecer isolada naquele serviço.

São consideradas personalizações:

- adicionar etapa;
- remover etapa;
- editar etapa;
- reordenar etapa;
- adicionar passo;
- remover passo;
- editar passo;
- reordenar passo.

Marcar checklists e adicionar observações não são considerados alterações estruturais do modelo.

---

## 37. Resumo funcional

A estrutura conceitual final será:

```text
ETAPA
│
├── título
├── descrição
├── ordem
├── estado calculado
├── progresso calculado
├── observações
│
└── PASSOS
    │
    ├── título
    ├── instrução
    ├── ordem
    ├── estado
    │   ├── pendente
    │   ├── concluído
    │   └── não aplicável
    │
    └── observações
        ├── texto
        ├── autor
        ├── data/hora
        └── visibilidade
            ├── interna
            └── cliente
```

---

## 38. Regras fundamentais

As seguintes regras são consideradas fundamentais para o Reset Service:

1. Etapas não possuem conclusão manual.
2. O estado da etapa é derivado automaticamente dos passos.
3. Todo passo aplicável deve ser concluído.
4. Passos não necessários devem ser marcados como Não aplicáveis.
5. Passos Não aplicáveis não participam do cálculo de progresso.
6. Todos os passos possuem o mesmo peso na versão 1.0.
7. O progresso geral é calculado a partir dos passos e não da média das etapas.
8. O usuário pode navegar livremente entre as etapas.
9. A conclusão de uma etapa não causa navegação automática.
10. Um serviço não pode ser concluído enquanto possuir passos pendentes.
11. Alterações estruturais no roteiro de um serviço não modificam seu modelo de origem.
12. Serviços concluídos precisam ser reabertos antes de qualquer nova alteração operacional.

---

## 39. Estado da decisão

**PLANNING-002 — Modelo funcional de Etapas e Passos: CONCLUÍDA E APROVADA.**

Este documento formaliza o comportamento do núcleo operacional do Reset Service e deverá servir como referência para as especificações funcionais, UX, arquitetura, testes e implementação posteriores.