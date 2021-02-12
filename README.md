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
Descrever as camadas e responsabilidades de cada uma. Descrever os padrões adotados SOLID, DI, UnitOfWork








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