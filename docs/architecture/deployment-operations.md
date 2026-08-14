# Reset Service — Deployment and Operations

**Projeto:** Reset Service  
**Empresa:** Technolife  
**Documento:** Implantação, Instalação, Atualização e Operação  
**Versão:** 1.0  
**Status:** Aprovado  
**Referências:** `docs/product/*`, `docs/architecture/architecture.md`, `docs/architecture/data-model.md`, `docs/architecture/security.md`

---

## 1. Objetivo

Este documento define como o Reset Service deverá ser:

- instalado;
- hospedado;
- disponibilizado na rede;
- iniciado;
- atualizado;
- diagnosticado;
- desinstalado;
- recuperado.

A implantação deverá permanecer simples e compatível com a realidade operacional da Technolife.

---

## 2. Princípio central

O Reset Service será instalado uma única vez em uma máquina central da rede.

Os usuários não instalarão o programa em suas estações.

Fluxo:

```text
Máquina hospedeira
       ↓
Reset Service
       ↓
Rede local
       ↓
Navegadores
```

A experiência do usuário deverá ser:

```text
Abrir navegador
       ↓
https://resetservice/
       ↓
Login
       ↓
Utilizar o sistema
```

---

## 3. Máquina hospedeira

O equipamento responsável por executar o Reset Service será chamado de:

> **Máquina hospedeira**

Não será necessário utilizar literalmente um Windows Server.

Poderão ser utilizados:

- computador desktop;
- notebook;
- Windows Server;
- outro computador Windows x64 compatível com a versão adotada do .NET.

---

## 4. Windows Server não é obrigatório

A arquitetura não dependerá de recursos exclusivos de Windows Server.

Um cenário perfeitamente válido será:

```text
Notebook ou desktop Technolife
          ↓
Windows
          ↓
Technolife Reset Service
          ↓
LAN
          ↓
demais computadores
```

---

## 5. Compatibilidade da máquina hospedeira

O suporte oficial deverá acompanhar a compatibilidade real do runtime .NET adotado.

Direção para a versão 1.0:

```text
Windows 11 x64
→ suportado

Windows 10 x64 compatível
com o .NET utilizado
→ suportado

Windows Server compatível
→ suportado
```

Windows 10 comum deverá fazer parte dos testes reais de implantação antes da liberação definitiva.

A documentação do Reset Service deverá distinguir:

- compatibilidade validada pela Technolife;
- suporte oficial da plataforma/runtime.

---

## 6. Equipamento não dedicado

A máquina hospedeira não precisa necessariamente ser dedicada exclusivamente ao Reset Service.

Entretanto, deverá possuir recursos suficientes e permanecer operacional durante o período em que o sistema precisar estar disponível.

---

## 7. Notebook como hospedeiro

Notebook será permitido.

Entretanto, configurações de energia deverão evitar indisponibilidade involuntária.

Quando a máquina estiver destinada a hospedar o Reset Service:

```text
Suspensão automática
→ desativada quando necessário

Hibernação automática
→ evitada durante operação

Fechar tampa
→ não deverá suspender a máquina
  quando isso interromper o serviço
```

O guia de implantação deverá explicar essa necessidade.

---

## 8. Indisponibilidade da máquina

Se a máquina hospedeira:

- for desligada;
- suspender;
- perder a rede;
- reiniciar;
- apresentar falha;

o Reset Service ficará temporariamente indisponível.

Isso é consequência aceitável da arquitetura centralizada.

---

## 9. Estações clientes

Os computadores dos usuários não executarão:

- .NET;
- EF Core;
- SQLite;
- serviço do Windows;
- aplicação instalada do Reset Service.

Eles executarão apenas o navegador.

---

## 10. Compatibilidade dos clientes

A compatibilidade da estação será determinada principalmente pelo navegador, e não pela versão do Windows usada no servidor.

Suporte oficial inicial:

```text
Windows 10
+
Chrome ou Edge moderno

Windows 11
+
Chrome ou Edge moderno
```

---

## 11. Windows antigos como clientes

Sistemas antigos poderão ser utilizados em regime de compatibilidade de melhor esforço.

Exemplos:

```text
Windows 7
Windows 8
Windows 8.1
```

desde que possuam navegador capaz de executar a interface.

Não haverá garantia oficial de funcionamento completo nesses ambientes.

---

## 12. Compatibilidade legada

Para clientes antigos, poderão funcionar:

- login;
- navegação;
- consultas;
- formulários;
- execução básica.

Recursos mais sensíveis à versão do navegador poderão apresentar limitações, especialmente:

- SignalR;
- atualização em tempo real;
- recursos modernos de JavaScript;
- comportamento visual.

---

## 13. Não sacrificar a plataforma moderna

A aplicação não será limitada artificialmente para manter compatibilidade com navegadores obsoletos.

Não será requisito:

```text
Internet Explorer
```

nem browsers que impeçam o funcionamento adequado da arquitetura aprovada.

---

## 14. Diretriz do frontend

Ao mesmo tempo, o frontend deverá evitar dependências modernas desnecessárias.

Preferência:

```text
HTML simples
CSS estável
JavaScript nativo
APIs amplamente suportadas
```

Não serão utilizados recursos experimentais sem necessidade concreta.

---

## 15. Aviso de navegador antigo

Quando possível, a aplicação poderá detectar ambiente incompatível e mostrar aviso.

Exemplo:

```text
Seu navegador é antigo.

Alguns recursos do Reset Service,
incluindo atualização em tempo real,
podem não funcionar corretamente.

Recomendamos Google Chrome
ou Microsoft Edge atualizado.
```

O acesso não precisa ser bloqueado quando a funcionalidade básica ainda for possível.

---

## 16. Acesso pela LAN

Qualquer computador autorizado conectado à rede interna deverá poder acessar o sistema através de navegador.

Não será necessário cadastrar previamente cada computador no Reset Service.

---

## 17. Endereço oficial

Endereço preferencial:

```text
https://resetservice/
```

A porta HTTPS padrão deverá ser utilizada sempre que possível:

```text
443
```

O usuário não deverá precisar memorizar IP ou porta.

---

## 18. DNS interno

Quando a infraestrutura permitir, deverá existir resolução de nome interna.

Exemplo:

```text
resetservice
      ↓
IP da máquina hospedeira
```

DNS interno será preferível a alterações manuais em cada estação.

---

## 19. Arquivo hosts

Editar manualmente:

```text
C:\Windows\System32\drivers\etc\hosts
```

em todas as estações não fará parte da estratégia oficial de implantação.

Poderá existir apenas como solução excepcional de ambiente muito simples.

---

## 20. Fallback de endereço

Quando não houver DNS interno administrável, poderá ser utilizado o hostname da máquina.

Exemplo:

```text
https://PC-RESETSERVICE/
```

O endereço utilizado deverá permanecer estável.

---

## 21. Endereço IP

Não será requisito configurar manualmente IP fixo diretamente no Windows.

Preferencialmente poderá existir:

```text
Reserva DHCP
```

para garantir endereço estável à máquina hospedeira.

---

## 22. HTTPS

A implantação oficial utilizará HTTPS.

O certificado deverá corresponder ao nome pelo qual os usuários acessam a aplicação.

Exemplo:

```text
URL
https://resetservice/

Certificado
resetservice
```

---

## 23. Confiança do certificado

Os computadores clientes deverão confiar no certificado ou na autoridade que o emitiu.

Possibilidades:

### Ambiente corporativo

```text
CA interna
      ↓
certificado do Reset Service
      ↓
clientes já confiam
```

### Rede simples

```text
CA/certificado interno
      ↓
confiança instalada nos clientes
      ↓
navegação sem aviso
```

---

## 24. Certificado de desenvolvimento

Certificados de desenvolvimento do .NET não serão utilizados como certificado oficial de produção.

---

## 25. Experiência de instalação

A instalação deverá ser guiada.

Conceitualmente:

```text
Executar instalador
      ↓
Verificar ambiente
      ↓
Instalar aplicação
      ↓
Criar diretórios
      ↓
Configurar permissões
      ↓
Configurar HTTPS
      ↓
Criar firewall
      ↓
Criar Windows Service
      ↓
Inicializar banco
      ↓
Iniciar aplicação
      ↓
Verificar funcionamento
```

---

## 26. Tecnologia do instalador

O mecanismo físico do instalador não está definido nesta etapa.

Poderá futuramente ser:

- `.exe`;
- MSI;
- outra tecnologia adequada para Windows.

O requisito é a experiência e o comportamento, não o formato específico.

---

## 27. Publicação

A aplicação deverá ser publicada inicialmente como:

```text
win-x64
self-contained
Release
```

O runtime necessário deverá acompanhar a aplicação quando apropriado.

---

## 28. Estrutura física

Aplicação:

```text
C:\Program Files\Technolife\ResetService\
```

Dados:

```text
C:\ProgramData\Technolife\ResetService\
```

---

## 29. Estrutura persistente

Proposta:

```text
C:\ProgramData\Technolife\ResetService\
│
├── data\
├── assets\
├── backups\
├── keys\
├── logs\
├── config\
└── maintenance\
```

---

## 30. Separação aplicação/dados

Regra:

```text
Program Files
→ aplicação substituível

ProgramData
→ estado persistente
```

Atualizar os binários não poderá destruir os dados.

---

## 31. Windows Service

A aplicação executará como Serviço do Windows.

Nome técnico:

```text
TechnolifeResetService
```

Nome exibido:

```text
Technolife Reset Service
```

---

## 32. Inicialização automática

Configuração preferencial:

```text
Automatic (Delayed Start)
```

Após a inicialização do Windows, o sistema deverá subir sem depender de login interativo.

---

## 33. Usuário do Windows não precisa estar conectado

Não será necessário:

```text
fazer login no Windows
↓
abrir ResetService.exe
```

O Serviço do Windows deverá executar independentemente de sessão de usuário.

---

## 34. Recuperação automática

O serviço poderá possuir política de recuperação.

Inicialmente:

```text
Primeira falha
→ reiniciar após aproximadamente 30 segundos

Segunda falha
→ reiniciar após aproximadamente 60 segundos

Falhas persistentes
→ evitar loop infinito
```

Falhas permanentes deverão permanecer diagnosticáveis.

---

## 35. Firewall

A instalação deverá criar somente a regra de rede necessária.

Conceitualmente:

```text
TCP
443
Inbound
rede local autorizada
```

Não haverá porta pública para SQLite ou outro serviço de banco.

---

## 36. SignalR

SignalR utilizará a mesma conexão HTTPS da aplicação.

Não exigirá porta externa própria.

---

## 37. Primeira inicialização

Após uma instalação nova:

```text
Reset Service inicia
       ↓
não existem usuários
       ↓
modo Initial Setup
       ↓
criação local do primeiro Admin
       ↓
bootstrap encerrado
       ↓
operação normal
```

---

## 38. Configuração inicial

A configuração inicial deverá permanecer simples.

Fluxo sugerido:

```text
1. Criar primeiro Administrador

2. Informações da Technolife

3. Configuração documental básica

4. Apresentar configuração de backup

5. Sistema pronto
```

Modelos poderão ser criados posteriormente.

---

## 39. Versão da aplicação

A aplicação deverá identificar sua versão instalada.

Exemplo:

```text
Reset Service
1.0.0
```

A área administrativa deverá permitir consultar essa informação.

---

## 40. Atualização offline

A atualização não dependerá de internet.

Fluxo:

```text
Obter pacote
      ↓
copiar para máquina hospedeira
      ↓
executar atualização
```

Não haverá requisito de:

- download automático;
- conexão com GitHub;
- serviço de update em nuvem.

---

## 41. Pacote de atualização

Poderá conceitualmente conter:

```text
ResetService-update\
│
├── application\
├── migration\
├── manifest
└── updater
```

---

## 42. Manifesto

O pacote poderá identificar:

- versão de destino;
- versões compatíveis de origem;
- versão/schema;
- arquitetura;
- checksums;
- existência de migration.

---

## 43. Processo de atualização

Fluxo:

```text
Validar pacote
      ↓
Verificar versão
      ↓
Verificar armazenamento
      ↓
Informar impacto
      ↓
Recomendar backup
      ↓
Modo de manutenção
      ↓
Bloquear novos comandos
      ↓
Drenar fila
      ↓
Parar serviço
      ↓
Preservar binários anteriores
      ↓
Executar migration
      ↓
Instalar aplicação
      ↓
Iniciar serviço
      ↓
Health check
```

---

## 44. Drenagem da fila

Antes de desligar a aplicação de forma planejada:

```text
novos comandos
→ não aceitos

comandos já aceitos
→ concluídos

fila
→ vazia
```

Somente depois ocorrerá a parada.

---

## 45. Backup antes da atualização

Backup continuará:

> **Recomendado, mas opcional.**

Quando houver alteração de schema, o risco deverá ser explicitado.

O Administrador poderá:

```text
Criar backup
Continuar sem backup
Cancelar
```

---

## 46. Migrations

Mudanças de schema não serão aplicadas silenciosamente toda vez que a aplicação iniciar.

A atualização será responsável pelas migrations necessárias.

---

## 47. Migration Bundle

EF Core Migration Bundle é a direção preferencial para execução controlada das migrations em produção.

Cada migration deverá ser:

- revisada;
- testada;
- empacotada;
- associada à release correta.

---

## 48. Versão anterior dos binários

Antes da substituição, poderá ser preservada a versão anterior da aplicação.

Exemplo:

```text
maintenance\
└── previous-app\
```

Isso não substitui backup de dados.

---

## 49. Rollback sem alteração de schema

Quando o banco não tiver sido alterado:

```text
nova aplicação falhou
       ↓
parar
       ↓
restaurar binários anteriores
       ↓
iniciar
```

poderá ser suficiente.

---

## 50. Rollback após migration

Quando a estrutura dos dados tiver sido alterada, não se assumirá que os binários antigos continuam compatíveis.

A recuperação poderá exigir:

```text
versão anterior
+
backup anterior
```

---

## 51. Sem downgrade automático

A versão 1.0 não executará migrations inversas automaticamente como estratégia genérica de rollback.

Recuperação será explícita e controlada.

---

## 52. Health check

Depois de atualização, deverão ser verificadas pelo menos:

```text
Windows Service
Aplicação HTTP
SQLite
Schema esperado
Inicialização sem erro crítico
```

Somente então o updater deverá considerar a atualização concluída.

---

## 53. Modo de manutenção

Atualização e restauração poderão colocar o sistema temporariamente em manutenção.

Os usuários deverão receber indicação adequada quando possível.

---

## 54. Reinicialização da máquina hospedeira

Fluxo esperado:

```text
Windows inicia
      ↓
Serviço inicia automaticamente
      ↓
SQLite abre
      ↓
Kestrel inicia
      ↓
https://resetservice/
disponível
```

---

## 55. Logs

Diretório:

```text
C:\ProgramData\Technolife\ResetService\logs\
```

Logs deverão possuir:

- rotação;
- retenção;
- limite de crescimento.

Não deverão consumir armazenamento indefinidamente.

---

## 56. Windows Event Log

Eventos importantes de serviço/startup poderão também ser enviados ao Windows Event Log.

O log detalhado continuará sob controle da aplicação.

---

## 57. Diagnóstico no sistema

Área:

```text
Configurações
→ Sistema
```

poderá mostrar:

```text
Versão
Sistema
Banco
Tempo real
Backup automático
Último backup válido
Armazenamento
```

A interface não será um painel técnico excessivo.

---

## 58. Dados técnicos sensíveis

A interface não deverá mostrar:

- connection strings;
- segredos;
- stack traces;
- chaves;
- SQL interno.

Esses detalhes pertencem ao diagnóstico técnico controlado.

---

## 59. Espaço disponível

Antes de operações críticas como:

- backup;
- atualização;
- restauração;

o sistema deverá verificar armazenamento quando tecnicamente possível.

Falta de espaço deverá impedir início inseguro da operação.

---

## 60. Desinstalação

Por padrão, a desinstalação deverá remover:

- aplicação;
- Windows Service;
- configurações operacionais associadas à instalação;
- regra de firewall quando apropriado.

Mas deverá preservar:

```text
dados
backups
assets
```

salvo exclusão explicitamente solicitada.

---

## 61. Dados não serão apagados silenciosamente

Nunca:

```text
Desinstalar
→ apagar todos os serviços automaticamente
```

A remoção definitiva de dados deverá ser ação separada e explícita.

---

## 62. Reinstalação

Se o instalador encontrar dados existentes, não deverá sobrescrevê-los silenciosamente.

Deverá identificar o cenário como:

- atualização;
- reparo;
- reinstalação;
- recuperação.

---

## 63. Recuperação total

Fluxo:

```text
Preparar máquina Windows compatível
        ↓
Instalar Reset Service
        ↓
Configurar rede/HTTPS
        ↓
Importar backup
        ↓
Validar
        ↓
Restaurar
        ↓
Autenticar novamente
```

---

## 64. Experiência final

## Administrador

```text
instala uma vez
      ↓
configura a máquina hospedeira
      ↓
cria primeiro Admin
      ↓
mantém uma instalação central
```

## Usuários

```text
qualquer PC autorizado da LAN
      ↓
Chrome / Edge
      ↓
https://resetservice/
      ↓
login
```

---

## 65. Matriz resumida de compatibilidade

| Ambiente | Situação |
|---|---|
| Desktop Windows como host | Suportado |
| Notebook Windows como host | Suportado |
| Windows Server como host | Opcional/suportado quando compatível |
| Windows 11 x64 host | Suporte oficial |
| Windows 10 x64 host | Conforme suporte/validação da versão do .NET |
| Windows 10 cliente | Suporte oficial |
| Windows 11 cliente | Suporte oficial |
| Chrome moderno | Suporte oficial |
| Edge moderno | Suporte oficial |
| Windows 7/8/8.1 cliente | Compatibilidade legada / melhor esforço |
| Navegador legado | Sem garantia completa |
| Internet | Não necessária |
| Instalação nas estações | Não |
| Acesso por IP | Apenas contingência |
| URL amigável | Preferencial |

---

## 66. Regras Fundamentais

1. Windows Server não será obrigatório.
2. Desktop, notebook ou servidor poderão hospedar a aplicação.
3. A máquina hospedeira deverá permanecer ligada e acessível pela LAN.
4. Notebook hospedeiro deverá ser configurado para evitar suspensão operacional.
5. A versão do Windows hospedeiro deverá ser compatível com a stack implantada.
6. Windows 10 fará parte da estratégia de compatibilidade.
7. Clientes não precisam executar .NET.
8. Clientes precisam apenas de rede e navegador compatível.
9. Windows 10 e 11 com browsers modernos serão ambientes clientes oficiais.
10. Windows antigos serão compatibilidade de melhor esforço.
11. O produto não será limitado para suportar navegadores obsoletos.
12. O frontend evitará tecnologias experimentais desnecessárias.
13. O acesso normal será por endereço amigável.
14. DNS interno será preferido.
15. Edição manual de `hosts` não será estratégia oficial.
16. HTTPS será utilizado.
17. Certificado deverá ser confiável pelos clientes.
18. Porta padrão preferencial será 443.
19. A aplicação será instalada uma vez centralmente.
20. O deploy será self-contained quando apropriado.
21. Aplicação e dados permanecerão separados.
22. O Reset Service executará como Windows Service.
23. Não dependerá de usuário conectado ao Windows.
24. Startup será automático.
25. Recuperação automática limitada poderá ser configurada.
26. Firewall deverá expor somente o necessário.
27. Primeiro Administrador será criado através de bootstrap local.
28. Atualizações funcionarão offline.
29. Novos comandos serão interrompidos antes de manutenção.
30. A fila deverá ser drenada antes de shutdown planejado.
31. Backup pré-update será recomendado, não obrigatório.
32. Migrations serão controladas pelo processo de atualização.
33. Rollback de banco não será presumido seguro.
34. Health check será executado após atualização.
35. Desinstalação preservará dados por padrão.
36. Recuperação total deverá funcionar em outra máquina compatível.

---

## 67. Estado da decisão

**PLANNING-015 — Implantação, Instalação, Atualização e Operação: CONCLUÍDA E APROVADA.**

A arquitetura não depende de um servidor Windows dedicado. O Reset Service poderá funcionar em notebook, desktop ou servidor compatível, permanecendo acessível pelos navegadores dos computadores da rede.
