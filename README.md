# Fiap Cloud Games — Notifications API

API responsável pelo envio de notificações do ecossistema Fiap Cloud Games.

Sumário
- Descrição
- Tecnologias
- Arquitetura
- Pré-requisitos
- Configuração
- Executando localmente
- Executando em Docker / Kubernetes
- Endpoints
- Eventos e Consumers
- Observabilidade
- Testes
- Contribuição

Descrição
----------
Microserviço que expõe endpoints para consulta de notificações por usuário e processa mensagens/consumidores para criação de perfil e envio de notificações reativas a eventos do domínio.

Tecnologias principais
---------------------
- .NET 7+ (Endpoints minimal API)
- MySQL (EF Core)
- Azure Service Bus (MassTransit)
- Elasticsearch (indexação e busca)
- Elastic APM (observabilidade)

Arquitetura / Organização do projeto
-----------------------------------
Estrutura principal:

- FiapCloudGames.Contracts/: contratos / eventos de integração
- FiapCloudGamesNotifications.Api/: API, consumers, endpoints, extensões
- FiapCloudGamesNotifications.Application/: regras de aplicação, serviços
- FiapCloudGamesNotifications.Domain/: entidades, repositórios, domínio
- FiapCloudGamesNotifications.Test/: testes automatizados

Pré-requisitos
--------------
- .NET SDK 7+ instalado
- MySQL (ou acesso a uma instância MySQL)
- Azure Service Bus (namespace/connection string)
- Elasticsearch (URI + índice) — opcional para funcionalidades de busca
- Variáveis de ambiente / segredos configurados (ver seção abaixo)

Configuração
-------------
O serviço usa `appsettings.json` com placeholders que utilizam variáveis de ambiente. As chaves usadas pelo projeto (exemplos):

- ConnectionStrings:MySQL → `MySQL` (ex.: `Server=...;Database=...;User=...;Password=...;`)
- Authentication:Key → chave simétrica usada para validar JWT
- Authentication:Issuer → issuer do token
- Authentication:Audience → audience do token
- AzureServiceBus:ConnectionString → string de conexão do Service Bus
- ElasticSearch:Uri → endpoint do Elasticsearch
- ElasticSearch:IndexName → nome do índice

Exemplo (Windows PowerShell):

``powershell
$env:ConnectionStrings__MySQL = "Server=localhost;Database=fiap_notifications;Uid=root;Pwd=secret;"
$env:Authentication__Key = "sua_chave_aqui"
$env:Authentication__Issuer = "issuer"
$env:Authentication__Audience = "audience"
$env:AzureServiceBus__ConnectionString = "Endpoint=sb://..."
$env:ElasticSearch__Uri = "http://localhost:9200"
```

Executando localmente
---------------------
1. Restaurar pacotes e compilar:

``powershell
dotnet restore
dotnet build
```

2. Aplicar migrations (opcional) — o `Program` aceita o argumento `migrate` para executar migrations EF Core:

``powershell
dotnet run --project FiapCloudGamesNotifications.Api -- migrate
```

3. Executar a API:

``powershell
dotnet run --project FiapCloudGamesNotifications.Api
```

Ao rodar em ambiente de desenvolvimento, o Swagger UI fica disponível (quando `ASPNETCORE_ENVIRONMENT=Development`).

Executando com Docker
---------------------
O repositório inclui um `Dockerfile` em [FiapCloudGamesNotifications.Api/Dockerfile](FiapCloudGamesNotifications.Api/Dockerfile).

Exemplo rápido para criar e rodar a imagem (ajuste as variáveis de ambiente):

``bash
docker build -t fiap/notifications:latest -f FiapCloudGamesNotifications.Api/Dockerfile .
docker run -e ConnectionStrings__MySQL="Server=db;Database=...;Uid=...;Pwd=...;" \
	-e AzureServiceBus__ConnectionString="Endpoint=sb://..." \
	-e Authentication__Key="sua_chave" \
	-p 5000:80 fiap/notifications:latest
```

Kubernetes
----------
Manifests estão em `k8s/` (deployment, service, configmap, secret, migration). Ajuste `configmap.yaml` e `secret.yaml` com as variáveis de ambiente necessárias.

Endpoints
---------
Rota pública principal exposta pela API:

- GET /users/{userId}/notifications — retorna notificações do usuário (política de autorização: `SameUserOrAdmin`).

Exemplo de chamada (cURL):

``bash
curl -H "Authorization: Bearer <JWT>" \
	https://localhost:5001/users/00000000-0000-0000-0000-000000000000/notifications
```

Política de Autorização
-----------------------
O projeto contém a policy `SameUserOrAdmin` implementada em `FiapCloudGamesNotifications.Api/Authorize`, que exige que o usuário logado seja o mesmo do recurso ou possua papel de administrador.

Eventos de Integração e Consumers
--------------------------------
Eventos/contratos encontram-se em `FiapCloudGames.Contracts/IntegrationEvents/`.
Consumers registrados (MassTransit):

- `CreateUserProfileConsumer`
- `SendNotificationOrderPlacedConsumer`
- `SendNotificationPaymentOrderProcessedConsumer`
- `SendNotificationUserLockedConsumer`
- `SendNotificationUserUnlockedConsumer`
- `SendWelcomeNotificationToNewUserConsumer`

MassTransit & Azure Service Bus
-------------------------------
A configuração de filas/consumers está em `FiapCloudGamesNotifications.Api/Extensions/MassTransitExtensions.cs`. O projeto exige a variável `AzureServiceBus:ConnectionString` para conectar ao namespace do Service Bus.

Observabilidade
---------------
Configurações do Elastic APM e Elasticsearch estão em `appsettings.json` (chaves: `ElasticApm` e `ElasticSearch`). Configure `ElasticApm:ServerUrl` e `ElasticApm:SecretToken` conforme necessário para enviar traces.

Testes
------
Para executar os testes unitários / de integração:

``powershell
dotnet test FiapCloudGamesNotifications.Test
```

Contribuição
------------
- Abra uma issue descrevendo a sugestão/bug.
- Crie uma branch com um nome descritivo.
- Faça PR para `main` incluindo testes quando aplicável.

Licença
-------
Defina aqui a licença do projeto (ex.: MIT) ou remova esta seção se não aplicável.

Contatos
--------
Para dúvidas e suporte, contate a equipe responsável pelo repositório.

---
Arquivo(s) úteis
- `FiapCloudGamesNotifications.Api/Program.cs` — configuração principal e migrations (`migrate` argument)
- `FiapCloudGamesNotifications.Api/Endpoints/NotificationsEndpoints.cs` — endpoints expostos
- `FiapCloudGamesNotifications.Api/Extensions/MassTransitExtensions.cs` — configuração de mensagens/consumers
- `k8s/` — manifests para deploy

Se quiser, gero exemplos de `docker-compose` com MySQL + Service Bus emulator + Elasticsearch ou atualizo o README com instruções mais específicas para CI/CD (Azure Pipelines). Basta pedir.

