# Mock_Project_Team3

## Getting started

To get the application up and running, there are two strategies:

- Running the application on the local network (needs manual database setup)
- Running the application in a container (doesn't need manual setup)
- Already deployed application: can be access through [https://famsproject.ddns.net/login](https://famsproject.ddns.net/login) with credentials `superadmin@gmail.com`/`123456`

### API documentation

- Postman: [link](https://documenter.getpostman.com/view/25687326/2sA2rCV2oy)

### Setting up

The mail settings and cloud settings need to be updated accordingly

```json
"MailSettings": {
  "Server": "smtp.gmail.com",
  "Port": "<PORT-NUMBER>",
  "SenderName": "<SENDER-NAME>",
  "SenderEmail": "<EMAIL>",
  "UserName": "<USERNAME>",
  "PassWord": "<YOUR-MAIL-PASSWORD>"
},
"CloundSettings": {
  "CloundName": "<CLOUD-NAME>",
  "CloundKey": "<CLOUD-KEY>",
  "CloundSecret": "<YOUR-SECRET>"
}
```

### Running on the local network

Update the connection string in `Mock_Project_Net03\Mock_Project_Net03\appsettings.json` to the PostgreSQL data source

> Server=localhost;Database=DBMockProject;Port=5432;User Id=postgres;Password=123456

After that, navigate to `Mock_Project_Net03\Mock_Project_Net03`, and then run

> dotnet run

### Running on the container

Even simpler, navigate to `Mock_Project_Net03\Mock_Project_Net03`, and then run

> docker-compose up

After that, navigate to `http://localhost:8888/swagger/index.html`

to see the OpenAPI documentation

## Migrations

To apply the migration to a data source, first setup the database in the `appsettings.json`

Then navigate to `Mock_Project_Net03\Mock_Project_Net03`, and then run

> dotnet-ef database update

## Basic workflow

To create a feature/use case, extend a service in `Mock_Project_Net03\Mock_Project_Net03\Services`, or if there isn't one, create a new class

```csharp
public class TestService
{
  private readonly IRepository<TestRepository, int> _testRepo;
  public TestService(IRepository<TestRepository, int> testRepo)
  {
    _testRepo = testRepo;
  }
}
```

Whenever a repository related to an entity is needed, just add one as a `readonly` field and inject it via the constructor

After writing the needed functionality, create a controller action in `Mock_Project_Net03\Mock_Project_Net03\Controllers`, and use the service, again via constructor injection:

```csharp
[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
  private ClassService _classService;

  public ClassController(ClassService classService)
  {
    _classService = classService;
  }

  [Authorize]
  [HttpGet("{id}")]
  public async Task<ActionResult<GetClassByIdResponse>> GetClassById(int id)
  {
    // do something with _classService here
    return Ok();
  }
}
```

### Mapping

Mapping definition can be put in `Mock_Project_Net03\Mock_Project_Net03\Mapper\ApplicationMapper.cs`, for example:

```csharp
CreateMap<Enrollment, EnrollmentRequest>().ReverseMap();
```

### Service registrations

Whenever a new `Service` class is created, it needs to be registered in `Mock_Project_Net03\Mock_Project_Net03\Extensions\ServicesExtensions.cs`, for instance:

```csharp
services.AddScoped<SemesterService>();
```

### Miscellaneous

Requests/Responses definitions should go in `Mock_Project_Net03\Mock_Project_Net03\Common\Payloads`, respectively

Validators are defined using `FluentValidation`, they are defined in `Mock_Project_Net03\Mock_Project_Net03\Validation`

DTOs are defined in `Mock_Project_Net03\Mock_Project_Net03\Dtos`

Mostly, other things don't need to be altered or modified further
