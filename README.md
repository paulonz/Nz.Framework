
# Nz.Framework
Todo projeto Web começa igual, definição das camadas, responsabilidades, banco de dados, autenticação, autorização, etc. Este projeto serve como um guia, até mesmo um template para projetos Web escritos em .net.

Every web project starts the same, definition of layers, responsibilities, database, authentication, authorization, etc. This project serves as a guide, even a template for Web projects written in .NET.
### Environment
 - [.net Framework 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
 - [PostgreSQL 13](https://www.postgresql.org/)
 - [Visual Studio 2019](https://visualstudio.microsoft.com/)
### NuGet Packages
* Microsoft
  * [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)
  * [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson)
  * [Microsoft.AspNetCore.Mvc.Versioning](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning)
  * [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)
  * [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design)
  * [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational)
  * [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk)
  * [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
  * [System.Collections.Concurrent](https://www.nuget.org/packages/System.Collections.Concurrent)
  * [System.IdentityModel.Tokens.Jwt](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt)
* Community
  * [coverlet.collector](https://www.nuget.org/packages/coverlet.collector)
  * [EFCore.NamingConventions](https://www.nuget.org/packages/EFCore.NamingConventions)
  * [FluentAssertions](https://www.nuget.org/packages/FluentAssertions)
  * [MailKit](https://www.nuget.org/packages/MailKit)
  * [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)
  * [Npgsql](https://www.nuget.org/packages/Npgsql)
  * [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
  * [Swashbuckle.AspNetCore.SwaggerGen](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerGen)
  * [System.Linq.Dynamic.Core](https://www.nuget.org/packages/System.Linq.Dynamic.Core)
  * [xunit](https://www.nuget.org/packages/xunit)
  * [xunit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio)
### Quick Start
Comece configurando o acesso ao banco de dados. Essas configurações são definidas em dois pontos:
* Para cada Micro serviço criado, na variável de ambiente DATABASE_CONNECTION_STRING
* Nos testes de integração, nas classes base, dentro do método BuildEnvironmentVariables
Nas variáveis de ambiente também estão outras configurações, como do Token JWT e do serviço SMTP

Start by configuring access to the database. These settings are defined in two points:
* For each Microservice created, in the environment variable DATABASE_CONNECTION_STRING
* In integration tests, in the base classes, within the BuildEnvironmentVariables method
In the environment variables are also other configurations, such as the JWT Token and the SMTP service
## Project structure
Este projeto é composto por Helpers, Libs, Core e Api. Abaixo uma breve descrição sobre cada uma dessas camadas:

This project is composed of Helpers, Libs, Core and Api. Below is a brief description of each of these layers:
### Helpers
Algumas ações são comuns dentro de um projeto, como criptografar/descriptografar, serializar/deserializar objetos, entre outras. A camada Helpers possui uma coleção de objetos e métodos de uso comum em uma aplicação. Suas definições estão no projeto **Nz.Common.Helpers** e suas implementações padrão estão no projeto **Nz.Common.Helpers.Impl.Default**. Os helpers disponíveis são:

Some actions are common within a project, such as encrypting / decrypting, serializing / deserializing objects, among others. The Helpers layer has a collection of objects and methods commonly used in an application. Its definitions are in the **Nz.Common.Helpers** project and its default implementations are in the **Nz.Common.Helpers.Impl.Default** project. The available helpers are:
```csharp
public interface IResourceHelper
{
	string LookupResource(
		Type resourceType,
		string resourceKey);
}
```
```csharp
public interface IParserHelper
{
	string ToJson<T>(
		T model) where T : class;

	T FromJson<T>(
		string json) where T : class;

	Target To<Target, From>(From model)
		where Target : class
		where From : class;
}
```
```csharp
public interface IEnumHelpers
{
	string GetDisplay<T>(
		T source,
		System.Resources.ResourceManager resourceManager) where T : Enum;
}
```
### Libs
Outras ações ou objetos comuns em uma aplicação estão na camada Libs. Nesta camada estão implementações mais complexas, como por exemplo: envio de email, templates para mensagens, paginação de resultados, entre outros. Algumas libs possuem configurações, essas configurações são definidas pelas interfaces I{libName}Settings.
As libs disponíveis e suas implementações são:

Other common actions or objects in an application are in the Libs layer. In this layer are more complex implementations, such as: sending emails, templates for messages, paging results, among others. Some libs have settings, these settings are defined by the I {libName} Settings interfaces.
The available libs and their implementations are:
| Definição (Definition) | Implementação (Implementation) |
|--|--|
| Nz.Libs.Encryption.IEncryption | Nz.Libs.Encryption.Impl.HashAlgorithm.Encryption |
| Nz.Libs.Encryption.IEncryptionSettings | Nz.Libs.Encryption.Impl.HashAlgorithm.EncryptionSettings |
| Nz.Libs.Jwt.Settings.IJwtSettings | Nz.Libs.Jwt.Settings.Impl.Default.JwtSettings |
| Nz.Libs.EmailSender.IEmailSender | Nz.Libs.EmailSender.Impl.Smtp.EmailSender |
| Nz.Libs.EmailSender.IEmailSenderSettings | Nz.Libs.EmailSender.Impl.Smtp.EmailSenderSettings |
| Nz.Libs.MessageTemplate.IMessageTemplate | Nz.Libs.MessageTemplate.Impl.MessageResource.MessageTemplate |
|  | Nz.Libs.MessageTemplate.MessageTemplateType |
|  | Nz.Libs.RestPagination.EnablePagingAttribute |
### Core
No core fica a real lógica da aplicação, suas regras de negócio além do acesso e manipulação de dados. O core é dividido em 5 camadas:

At the core is the real logic of the application, its business rules as well as data access and manipulation. The core is divided into 5 layers:
#### Model
A camada model possui a implementação dos objetos que são armazenados no banco de dados. Existe um projeto base (**Nz.Core.Model**) que possui as definições padrão para um objeto do tipo model. Para cada micro serviço criado existe uma implementação de model isolada em outro projeto, somente com os objetos utilizados pelo micro serviço.

The model layer has the implementation of the objects that are stored in the database. There is a base project (**Nz.Core.Model**) that has the default settings for an object of type model. For each micro service created there is an isolated model implementation in another project, with only the objects used by the micro service.
#### DatabaseContext
DatabaseContext é o mapeamento dos objetos model para o banco de dados, nele é definido o banco de dados que deve ser utilizado, assim como regras específicas de índices, padrão de nomes para tabelas e colunas. Para cada micro serviço existe uma implementação de DatabaseContext, com suas configurações e o mapeamento para as models utilizadas pelo micro serviço.

DatabaseContext is the mapping of model objects to the database, it defines the database to be used, as well as specific index rules, standard names for tables and columns. For each micro service there is an implementation of DatabaseContext, with its settings and the mapping for the models used by the micro service.
#### UnitOfWork
Esta camada faz a manipulação dos dados usados pela aplicação, ela faz uso das camadas model e databaseContext. A definição de IUnitOfWork provê a leitura, escrita e alteração de dados. A implementação padrão faz uso do pacote [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational). 

This layer manipulates the data used by the application, it makes use of the model and databaseContext layers. The definition of IUnitOfWork provides for reading, writing and changing data. The standard implementation makes use of the [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational) package.
#### Business
Na business são feitas as validações de regras relacionadas ao objeto sendo manipulado. Existe um projeto base **Nz.Core.Business** com as definições de comportamento padrão para a camada, essa implementação está no projeto **Nz.Core.Business.Impl.Default**. Cada micro serviço possui sua definição e implementação dessa camada, tratando apenas objetos que fazem parte do micro serviço.

In business, validations of rules related to the object being manipulated are carried out. There is a ** Nz.Core.Business ** base project with the default behavior definitions for the layer, this implementation is in the ** Nz.Core.Business.Impl.Default ** project. Each micro service has its definition and implementation of this layer, treating only objects that are part of the micro service.
#### Service
A camada de serviço é responsável por receber os dados originários da api, fazer conversão de tipo (quando necessário) e encaminhar para a business responsável. A service também é responsável por controlar transações com o banco de dados, isso é feito utilizando o objeto databaseContext, esse contexto é iniciado na camada service e o commit da transação também é feito por ela.

The service layer is responsible for receiving data originating from the api, making type conversion (when necessary) and forwarding it to the responsible business. The service is also responsible for controlling transactions with the database, this is done using the databaseContext object, this context is initiated in the service layer and the transaction is also committed by it.
### API
A API é a porta de entrada para a lógica da aplicação. Outras aplicações (como web ou mobile) fazem suas requisições para a API, o processamento é feito e o resultado entregue. A camada API desta solução foi desenvolvida usando a abordagem Restful, desta forma, todo IO é feito com base em objetos Json. As respostas dadas pela API utilizam o http status para indicar o tipo de resultado, a lista de status http pode ser consultada [aqui](https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status).
Esta camada foi organizada em micro serviços, para saber mais sobre essa organização leia o tópico **Microservices** neste arquivo.

The API is the gateway to application logic. Other applications (such as web or mobile) make their requests for the API, processing is done and the result is delivered. The API layer of this solution was developed using the Restful approach, in this way, all IO is done based on Json objects. The responses given by the API use the http status to indicate the type of result, the http status list can be consulted [here](https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status).
This layer was organized in micro services, to learn more about this organization read the topic ** Microservices ** in this file.
## Microservices
No projeto já existem dois micro serviços: Auth e Announcement. Todo micro serviço deve fazer referência a Nz.Api, nela estão as implementações base e também definições do comportamento padrão da API e seus endpoints.
Cada microserviço possui sua implementação para API, ViewModel, Service, Bussiness e Model.
No projeto Nz.Api existem classes abstratas para simplificar a criação de endpoints do tipo CRUD, assim como para a camada Service e Business.
Exemplo de Controller CRUD para tratar Announcement:

There are already two microservices in the project: Auth and Announcement. Every microservice must make reference to Nz.Api, it contains the base implementations and also defines the standard behavior of the API and its endpoints.
Each microservice has its own implementation for API, ViewModel, Service, Bussiness and Model.
In the Nz.Api project there are abstract classes to simplify the creation of CRUD type endpoints, as well as for the Service and Business layer.
Example of CRUD Controller to handle Announcement:
```csharp
    [Authorize(Roles = nameof(Core.Model.RoleType.ManageAnnouncements))]
    public class ManageAnnouncementsController : ApiControllerCRUDBase<Core.Model.Impl.Announcement.Announcement>
    {
        public ManageAnnouncementsController(
            IManageAnnouncementsService currentService,
            ILogger<ManageAnnouncementsController> logger) :
            base(currentService, logger)
        { }
    }
```
Essa Controller possui os métodos:

This Controller has the methods:
```csharp
public async Task<IActionResult> GetAllAsync([FromQuery] string[] include, [FromQuery] string where, [FromQuery] string orderBy, [FromQuery] int? page, [FromQuery] int? pageSize);
public async Task<IActionResult> GetSingleAsync(int id, [FromQuery] string[] include);
public async Task<IActionResult> PostAsync([FromBody] T value, [FromQuery] string[] include);
public async Task<IActionResult> PutAsync(int id, [FromBody] T value, [FromQuery] string[] include);
public async Task<IActionResult> DeleteAsync(int id);
public async Task<IActionResult> UnDeleteAsync(int id, [FromQuery] string[] include);
```
## API Features
### Include
Com exceção ao método DeleteAsync, é possível incluir objetos relacionados no resultado de uma pesquisa (desde que sejam objetos do mesmo microserviço). Isso é feito utilizando o parâmetro de QueryString 'include', por exemplo:

In GET methods it is possible to include objects related to the result of a search (as long as they are objects of the same microservice). This is done using the QueryString parameter 'include', for example:
`GET /1.0/thing/123/include=detail`
```json
{
    "id": 123,
    "name": "thing name",
    "detail": {
        "id": 321,
        "name": "detail name"
    }
}
```
### OrderBy
Para os métodos GET é possível ordenar o resultado, essa ordenação pode ser ASC ou DESC. Para ordenar um resultado basta adicionar o parâmetro orderBy na querystring da requisição:

For GET methods it is possible to order the result, this order can be ASC or DESC. To order a result just add the parameter orderBy in the querystring of the request:

`GET /1.0/thing/orderBy=name DESC`
```json
{
	"items": [
		{
			"id": 2,
			"name": "second item"
		},
		{
			"id": 1,
			"name": "first item"
		}
	],
	"pagination": {
		"pageSize": 1,
		"totalResults": 2,
		"totalPages": 1,
		"page": 1,
		"previous": null,
		"next": null
	}
}
```
### Pagination
Todo método que retorne uma lista de objetos inclui as informações a respeito da paginação desse resultado. É possível passar por parâmetro na querystring da requisição qual página deve ser exibida e quantos resultados por página. Os valores informados em 'previous' e 'next' são as urls completas para a próxima página ou para a página anterior:

Every method that returns a list of objects includes information about the pagination of that result. It is possible to pass by parameter in the request querying which page should be displayed and how many results per page. The values ​​entered in 'previous' and 'next' are the complete urls for the next page or the previous page:
`GET /1.0/thing/page=2&pageSize=2`
```json
{
	"items": [
		{
			"id": 3,
			"name": "third item"
		},
		{
			"id": 4,
			"name": "fourth item"
		}
	],
	"pagination": {
		"pageSize": 2,
		"totalResults": 4,
		"totalPages": 2,
		"page": 2,
		"previous": "https://localhost/thing/page=1&pageSize=2",
		"next": null
	}
}
```
### Where
Além da paginação, os métodos GET que retornam listas suportam o parâmetro where em sua querystring. Esse parâmetro filtra os resultados da consulta. É possível utilizar qualquer operador válido para uma linq expression:
  
In addition to pagination, GET methods that return lists support the where parameter in your querystring. This parameter filters the results of the query. It is possible to use any valid operator for a linq expression
`GET /1.0/thing/where=id > 2`
```json
{
	"items": [
		{
			"id": 3,
			"name": "third item"
		},
		{
			"id": 4,
			"name": "fourth item"
		}
	],
	"pagination": {
		"pageSize": 1,
		"totalResults": 2,
		"totalPages": 1,
		"page": 1,
		"previous": null,
		"next": null
	}
}
```
### Route Versioning
Este projeto usa o pacote [Microsoft.AspNetCore.Mvc.Versioning](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning) para trabalhar com múltiplas versões de rotas. A versão padrão de todos os endpoints é 1.0:

This project uses the [Microsoft.AspNetCore.Mvc.Versioning](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Versioning) package to work with multiple versions of routes. The default version for all endpoints is 1.0:
`GET /1.0/thing/123`
```json
{
    "id": 123,
    "name": "thing name"
}
```
### Default endpoints
Todo micro serviço possui dois endpoints padrão: healthcheck e typeDescriptor. O endpoint healthcheck verifica se a aplicação está online e com conexão ao banco de dados, retornando 200 (OK) em caso de sucesso. Já o endpoint typeDescriptor é utilizado para descrever tipos, representados por enums no código do micro serviço:

Every microservice has two standard endpoints: healthcheck and typeDescriptor. The healthcheck endpoint verifies that the application is online and connected to the database, returning 200 (OK) in case of success. The typeDescriptor endpoint is used to describe types, represented by enums in the microservice code:
`GET /1.0/healthcheck`
200 OK

`GET /1.0/typeDescriptor/thingType`
```json
{
  "items": [
    {
      "id": 0,
      "description": "cool thing"
    },
    {
      "id": 1,
      "description": "bad thing"
    }
  ],
  "pagination": {
    "pageSize": 50,
    "totalResults": 2,
    "totalPages": 1,
    "page": 1,
    "previous": "",
    "next": ""
  }
}
```
### Globalization
As strings utilizadas em todo o projeto estão armazenadas em arquivos de resources. O idioma padrão é o 'en', as mensagens retornadas por cada endpoint serão de acordo com parâmetro 'accepted-language' informada no header da requisição. Caso não seja encontrado o arquivo resource com o idioma solicitado, será utilizado o 'en'.

The strings used throughout the project are stored in resource files. The default language is 'en', the messages returned by each endpoint will be according to the 'accepted-language' parameter informed in the request header. If the resource file with the requested language is not found, the 'en' will be used.
### Swagger
Este projeto usa o pacote [Swashbuckle.AspNetCore.SwaggerGen](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerGen) para gerar a documentação de cada micro serviço. A url padrão para acessar essa documentação é:

This project uses the [Swashbuckle.AspNetCore.SwaggerGen](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerGen) package to generate the documentation for each microservice. The default url to access this documentation is:
`https://localhost:{port}/index.html`
### Logger
As configurações de log são definidas na classe base 'Program'. A configuração padrão está definida para utilizar a implementação 'SimpleConsole':

The log settings are defined in the base class 'Program'. The default configuration is set to use the 'SimpleConsole' implementation:

```csharp
private Action<ILoggingBuilder> CreateLoggerSettings()
{
	return new Action<ILoggingBuilder>(logging =>
	{
		logging.ClearProviders();
		logging.AddEventLog(options =>
		{
			options.SourceName = ApplicationName;
		});
		logging.AddSimpleConsole(options =>
		{
			options.IncludeScopes = true;
			options.UseUtcTimestamp = true;
		});
		logging.AddDebug();
	});
}
```
### Defaults
**DateTime**
por padrão, as datas são armazenadas e recuperadas em UTC.
by default, dates are stored and retrieved in UTC.
**Logger** 
o provedor de log padrão é 'SimpleConsole'.
the default log provider is 'SimpleConsole'.
**Route Version** 
a versão padrão das rotas é 1.0.
the default routes version is 1.0.
**Pagination Page size** 
a quantidade de registros retornados, por padrão, é 50.
the number of records returned, by default, is 50.
**Code Analysis**
o recurso Code Analysis está habilitado em todos os projetos da solução.
the Code Analysis feature is enabled in all projects in the solution.
**Email sender** 
emails são enviados, por padrão, o protocolo SMTP.
emails are sent, by default, using the SMTP protocol.
**Source comments**
os comentários no código fonte estão escritos em Português do Brasil.
the comments in the source code are written in Brazilian Portuguese.
## Tests
Os projetos de testes são divididos de acordo com seu objetivo e grupo de recursos que serão validados. No projeto existem 4 projetos de testes, sendo:
**Nz.Tests.Components**
Coleção de testes unitários para componentes da aplicação: Helpers e Libs.
Collection of unit tests for application components: Helpers and Libs.
**Nz.Tests.Common**
Este é o projeto base para testes integrados. Esses testes são executados acessando e validando comportamento e resultados dos endpoints. Diferente dos testes unitários, os testes integrados validam cenários.
This is the base project for integrated testing. These tests are performed by accessing and validating endpoint behavior and results. Unlike unit tests, integrated tests validate scenarios.
**Nz.Tests.Auth**
Coleção de testes integrados para os endpoints disponíveis no microserviço 'Auth'.
Collection of integrations tests for the endpoints available in the 'Auth' microservice.
**Nz.Tests.Announcement**
Coleção de testes integrados para os endpoints disponíveis no microserviço 'Announcement'.
Collection of integrations tests for the endpoints available in the 'Announcement' microservice.
### Integration Tests
Os testes de integração implementados no projeto necessitam de um servidor Postgresql, para cada teste executado é criada uma base.
Antes de executar os testes de integração é preciso configurar a conexão com o servidor de banco de dados. 
Recomendamos utilizar um servidor específico para os testes, também recomendamos criar um login específico para executar a conexão como banco.
Este usuário precisa ter a permissão para criação de novas bases de dados, essas configurações estão no arquivo base em cada projeto de teste:

The integration tests implemented in the project require a Postgresql server, for each test performed a base is created.
Before running the integration tests, you need to configure the connection to the database server.
We recommend using a specific server for the tests, we also recommend creating a specific login to perform the connection as a bank.
This user must have permission to create new databases, these settings are in the base file for each test project:

```csharp
private static void BuildEnvironmentVariables()
{
	string databaseHost = "127.0.0.1";
	string databasePort = "5433";
	string databaseUser = "nz_user_auth_tests";
	string databasePassword = "xxxxx";
	string databaseName = BuildTemporaryDatabaseName();

	Environment.SetEnvironmentVariable("ASPNETCORE_URLS", Auth.Base);
	Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
	Environment.SetEnvironmentVariable("GENERAL_BASE_URI", Auth.Base);
	Environment.SetEnvironmentVariable("JWT_VALID_ISSUER", Auth.BaseNoPort);
	Environment.SetEnvironmentVariable("JWT_SIGNING_KEY", "xxxxxx");
	Environment.SetEnvironmentVariable("JWT_VALID_AUDIENCE", Auth.BaseNoPort);
	Environment.SetEnvironmentVariable("JWT_EXPIRES_IN_MINUTES", "60");
	Environment.SetEnvironmentVariable("EMAIL_SMTP_HOST", "smtp.xxxx.io");
	Environment.SetEnvironmentVariable("EMAIL_FROM_EMAIL", "api@nz.com");
	Environment.SetEnvironmentVariable("EMAIL_SMTP_USER", "xxxxxx");
	Environment.SetEnvironmentVariable("EMAIL_FROM_NAME", "NzAPI");
	Environment.SetEnvironmentVariable("EMAIL_SMTP_PASSWORD", "xxxxxx");
	Environment.SetEnvironmentVariable("EMAIL_SMTP_PORT", "465");

	Environment.SetEnvironmentVariable("EMAIL_POP3_HOST", "pop3.xxxx.io");
	Environment.SetEnvironmentVariable("EMAIL_POP3_PORT", "1100");
	Environment.SetEnvironmentVariable("EMAIL_POP3_USER", "xxxxx");
	Environment.SetEnvironmentVariable("EMAIL_POP3_PASSWORD", "xxxx");

	Environment.SetEnvironmentVariable("DATABASE_CONNECTION_STRING", $"Server={databaseHost};Port={databasePort};Database={databaseName};User Id={databaseUser};Password={databasePassword};");
}
```