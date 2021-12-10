using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace biblioteca.Configuracao
{
    public static class SwaggerConfiguracao
    {
        #region Métodos
        public static void AdicionarConfiguracaoSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "Biblioteca",
                    Description = "API responsável por gerenciar um sistema gerenciamento de biblioteca.",
                    Contact = new OpenApiContact
                    {
                        Name = "Gessy Sousa",
                        Email = "gessy@email.com"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autorização JWT via header (requisição) utilizando o scheme Bearer. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string [] { }
                    }
                });
            });
        }
        #endregion

        public static void UseConfiguracaoSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "v1");
            });
        }
    }
}
