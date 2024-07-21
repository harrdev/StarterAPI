using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StarterAPI;
using StarterAPI.Attributes;
using StarterAPI.AutoMapper;
using StarterAPI.Filters;
using StarterAPI.Middelware;
using StarterAPI.Repositories;
using StarterAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure SeriLog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure database connection
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Repositories and Services to DI Container
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        //builder.WithOrigins("https://example.com")
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add APIKey Validation Filter
builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

// Add rate limiting services
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

// Add compression and caching to API responses
builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression();

builder.Services.AddEndpointsApiExplorer();

// Add swagger options -- Custom class in Configurations folder
builder.Services.AddSwagger();

builder.Logging.AddSerilog();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Starter API v1");
    });
}
app.UseHttpsRedirection();
// Use CustomException Middelware
app.UseMiddleware<CustomExceptionMiddleware>();
// Use rate limiting middleware
app.UseIpRateLimiting();
// Use CORS
app.UseCors();
// Use compression and caching
app.UseResponseCaching();
app.UseResponseCompression();

app.UseAuthorization();
app.MapControllers();

app.Run();