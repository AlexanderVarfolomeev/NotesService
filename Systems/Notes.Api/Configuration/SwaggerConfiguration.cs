using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Notes.Common;
using Notes.Settings.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Notes.Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddAppSwagger(this IServiceCollection services, IApiSettings settings)
    {
        if (!settings.General.SwaggerVisible)
            return services;

        services.AddOptions<SwaggerGenOptions>()
            .Configure<IApiVersionDescriptionProvider>((options, provider) =>
            {
                foreach (var avd in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(avd.GroupName, new OpenApiInfo
                    {
                        Version = avd.GroupName,
                        Title = $"NotesService"
                    });
                }
            });

        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();

            options.UseInlineDefinitionsForEnums();

            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            options.OperationFilter<SwaggerDefaultValues>();


            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{settings.IdentityServer.Url}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.NotesApiScope, "NotesScope"},
                        }
                    },
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri($"{settings.IdentityServer.Url}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.NotesApiScope, "NotesScope"},
                        }
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new List<string>()
                    }
                });
        });

        return services;
    }

    public static WebApplication UseAppSwagger(this WebApplication app)
    {
        var settings = app.Services.GetService<IApiSettings>();
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        if (!settings.General.SwaggerVisible)
            return app;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "api/{documentname}/api.yaml";
        });

        app.UseSwaggerUI(
            options =>
            {
                options.RoutePrefix = "api";
                provider.ApiVersionDescriptions.ToList().ForEach(
                    description => options.SwaggerEndpoint($"/api/{description.GroupName}/api.yaml", description.GroupName.ToUpperInvariant())
                );

                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(-1);
                options.OAuthAppName("Notes API");

                options.OAuthClientId(settings.IdentityServer.ClientId);
                options.OAuthClientSecret(settings.IdentityServer.ClientSecret);
            }
        );
        return app;
    }

    private class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
                return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                parameter.Description ??= description.ModelMetadata?.Description;

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());

                parameter.Required |= description.IsRequired;
            }
        }
    }
}