using Microsoft.OpenApi.Models;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Swagger Description/Info
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Starter API",
                Description = "Starter API Template",
                Version = "v1"
            });

            // Add API key definition
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the endpoints.",
                In = ParameterLocation.Header,
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            // Add a global security requirement to require API Key on all routes
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        Scheme = "ApiKeyScheme",
                        Name = "X-API-KEY",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}