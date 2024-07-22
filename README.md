# StarterAPI

Welcome to the StarterAPI! This API allows you to manage a collection of items, including creating, reading, updating, and deleting item records. The API is built with ASP.NET Core 8.0 and includes features such as rate limiting, CORS configuration, API key validation, logging with Serilog, response caching, response compression, AutoMapper and more.

`This is currently set up with EntityFrameworkCore installed.`

## Table of Contents

- [Getting Started](#getting-started)
- [Configuration](#configuration)
  - [Database Connection String](#database-connection-string)
  - [CORS Configuration](#cors-configuration)
  - [Logging Configuration](#logging-configuration)
  - [Rate LimitingnConfiguration](#rate-limiting)
  - [API Key](#api-key)
  - [Response Caching](#response-caching)
  - [Response Compression](#response-compression)
- [Endpoints](#endpoints)
- [Swagger](#swagger)
- [Deployment](#deployment)

## Getting Started

To get started with the APIStarterProj, clone the repository and navigate to the project directory:

1. Create DB in SSMS for migrations/update to run in.
2. Add connection string to appsettings.json file.
3. Add ApiKey value in appsettings.json file.
4  Create migrations.  Update database.
5. Build and run project to test with Swagger (append "/swagger" to end of window url when project starts in browser).
6. Controllers currently have APIKEY validation.  In Swagger, click on "Authorize" and enter ApiKey value that you set in appsettings.json file.

## Configuration

The application settings can be configured in the appsettings.json file. Here are the key configuration options:

### Database Connection String

The database connection string is configured in the `ConnectionStrings` section of the `appsettings.json` file. Update this string to point to your database server and database:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\sqlexpress;Database=<DbNameHere>;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```
### CORS Configuration

CORS (Cross-Origin Resource Sharing) settings can be configured in the `Program.cs` file. By default in this app, the API allows any origin. To restrict it to specific origins, update the `WithOrigins` method with specific domains or replace `builder.WithOrigins()` with `builder.AllowAnyOrigin()`:
```
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://example.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
```
### Logging Configuration

Logging is configured using Serilog. The log file path and other settings can be updated in the `Program.cs` file. By default, logs are written to the `Logs` directory with daily rolling files. Currently setup to create a new log file every day for 7 days, removing the first one every day after 7:
```
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();
```
In the codebase, inject the logger and use where appropriate (typically the controllers):
```
private readonly ILogger<StudentController> _logger;

public StudentController (Ilogger<StudentController> logger) {
  _logger = logger;
}

<some code>
_logger.LogError(exception, "There was a problem...");
_logger.LogInformation("Fetching all students");
```
### Rate Limiting Configuration

Rate limiting is configured in the `IpRateLimiting` section of the `appsettings.json` file. You can adjust the limits and periods as needed:

```{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 10
      },
      {
        "Endpoint": "*",
        "Period": "1h",
        "Limit": 200
      }
    ]
  }
```
### API Key

The API key is required to access the endpoints. It is configured in the `appsettings.json` file:

Example:
```
{
  "ApiKey": "a1b2c3"
}
```
Ensure you set this key and use it in the request headers as `X-API-KEY`.

## Response Caching

Response caching is used to improve the performance of the API by storing the results of requests and reusing them for subsequent requests. This reduces the load on the server and decreases the response time for clients.

### What is Response Caching Doing?

The response caching setup in the project is configured to cache responses for a specified duration. When a request is made to the API, the response is stored in a cache. If the same request is made again within the caching duration, the cached response is returned instead of processing the request again. This can significantly improve the performance and scalability of the API.  'Note: If the results from the GET calls are updated frequenty, consider removing the cache or reducing the time on it'.

### Where is Response Caching Configured?

Response caching is configured in the `Program.cs` file. Here is the relevant configuration:

```
builder.Services.AddResponseCaching();
```
The above line adds the response caching services to the dependency injection container.
```
app.UseResponseCaching();
```
The above line enables response caching middleware in the request processing pipeline.

### Attributes on the Routes
To enable response caching for specific routes, you use the [ResponseCache] attribute. This attribute can be added to your controller actions to specify the caching behavior.

Here are the parameters you can use with the [ResponseCache] attribute:

- `Duration`: The time, in seconds, for which the response should be cached.
- `Location`: Specifies where the data from a particular URL must be cached. Possible values are `Any`, `Client`, and `None`.
- `VaryByQueryKeys`: Specifies the query string parameters to vary the cache. This means that different cached responses are stored based on the query string values.

### Example
Here is an example of how to use the [ResponseCache] attribute on your routes:
```
[HttpGet]
[ResponseCache(Duration = 60)] // Cache response for 60 seconds
public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
{
    // Implementation
}

[HttpGet("{id}")]
[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "id" })] // Cache response for 60 seconds, vary by 'id' query parameter
public async Task<ActionResult<Student>> GetStudent(int id)
{
    // Implementation
}
```

## Response Compression

Response compression reduces the size of the responses sent from your API, decreasing bandwidth usage and improving load times for clients.

### Configuration in `Program.cs`

Response compression is configured in the `Program.cs` file:

```
builder.Services.AddResponseCompression();
app.UseResponseCompression();
```
- `builder.Services.AddResponseCompression()`: Adds response compression services.
- `app.UseResponseCompression()`: Enables response compression middleware.

### What is Response Compression doing?
When enabled, the server compresses the response data using algorithms like Gzip or Brotli before sending it to the client. Most modern clients (browsers and HTTP clients) automatically handle compressed responses, so no special client-side handling is needed.

### Client-Side Handling
Modern clients automatically include the Accept-Encoding header, indicating supported compression algorithms. They decompress responses as needed.


## Endpoints

Here are some of the key endpoints available in the APIStarterProj:

- `GET /api/student` - Retrieves a list of all students.
- `GET /api/student/{id}` - Retrieves a specific student by ID.
- `POST /api/student` - Creates a new student.
- `PUT /api/student/{id}` - Updates an existing student by ID.
- `DELETE /api/student/{id}` - Deletes an student by ID.

For a full list of endpoints and to test them interactively, use the Swagger UI available in development mode at `localhost:<port>/swagger`.

## Swagger
### Using Swagger

Swagger is available in the development environment for testing the API endpoints. It is configured in Program.cs file, using Configurations/SwaggerConfiguration.cs file..

### Accessing Swagger

To access Swagger, navigate to the following URL in your browser while the application is running:
```
http://localhost:<port>/swagger
```

Replace `<port>` with the port number your application is running on.

### Authorizing with API Key

To test the endpoints that require an API key, use the Authorize button in the Swagger UI:

1. Open the Swagger UI in your browser.
2. Click the `Authorize` button at the top right corner of the page.
3. In the popup modal, enter your API key in the `Value` field with the prefix `X-API-KEY` (e.g., `X-API-KEY: a1b2c3`).
4. Click the `Authorize` button in the modal to apply the API key to all requests.
5. Close the modal.

Now, all requests made through the Swagger UI will include the API key in the headers.

### Testing Endpoints

You can test the endpoints directly in the Swagger UI:

1. Select an endpoint you want to test from the list.
2. Click on the endpoint to expand its details.
3. Fill in any required parameters.
4. Click the `Try it out` button to send a request to the API.
5. View the response returned by the API.

Using Swagger, you can easily explore and interact with the API.


## Deployment
Enter deployment instructions here.  Example, cleaning appsettings.json file or any other configuration secrets.