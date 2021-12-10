using biblioteca.Auth;
using biblioteca.Auth.Interfaces;
using biblioteca.Configuracao;
using biblioteca.Context;
using biblioteca.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace biblioteca
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //CORS
            services.AddCors();
           
            //CONFIGURAÇÕES
            var sessao = Configuration.GetSection("JwtConfiguracoes");
            services.Configure<JwtConfiguracoes>(sessao);

            //SERVIÇOS
            services.AddScoped<IJwtAuthGerenciador, JwtAuthGerenciador>();

            //AUTENTICAÇÃO
            services.AddConfiguracaoAuth(Configuration);

            //CONTROLLER
            services.AddControllers();

            //SWAGGER - DOCUMENTAÇÃO API
            services.AdicionarConfiguracaoSwagger();

            //CONTEXTO
            services.AddDbContext<BibliotecaDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //CORS
            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());

            app.UseConfiguracaoSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
