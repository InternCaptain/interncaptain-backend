using API.Models;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services.AddDbContext<ApiContext>((provider, options) =>
                options.UseNpgsql(Configuration.GetConnectionString("postgres"),
                        b => b.MigrationsAssembly("API.GraphQL"))
                    .UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>())
                    .UseSnakeCaseNamingConvention());

            services
                .AddInMemorySubscriptions()
                .AddGraphQL(
                new SchemaBuilder()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
            );

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(o =>
                o.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());

            app.UseRouting();

            app.UseWebSockets();
            
            app.UseGraphQL()
                .UsePlayground()
                .UseVoyager();

        }
    }
}