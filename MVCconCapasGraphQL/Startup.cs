using System;
using System.Text;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MVCconCapasGraphQL.Queries;
using MVCconCapasGraphQL.Schema;
using MVCconCapasGraphQL.DataAccess;
using MVCconCapasGraphQL.DataAccess.Contracts;
using MVCconCapasGraphQL.Types.Users;
using MVCconCapasGraphQL.Types;
using MVCconCapasGraphQL.DataBase.Connections;
using MVCconCapasGraphQL.Types.Site;
using MVCconCapasGraphQL.DataAccess.Repositories;
using MVCconCapasGraphQL.DataAccess.Repositories.Contracts;

namespace MVCconCapasGraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            services.AddAuthorization();

            services.Configure<DbOptions>(Configuration.GetSection("ConnectionStrings"));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISiteRepository, SiteRepository>();

            services.AddSingleton<UserType>();
            services.AddSingleton(typeof(GeneralResponseType<>)); // genérico abierto

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<GraphQLQuery>();
            services.AddSingleton<GraphQLMutation>();
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<GraphQLSchema>();
            services.AddSingleton<ISchema>(sp => sp.GetRequiredService<GraphQLSchema>());

            services.AddSingleton<NavigationItemType>();
            services.AddSingleton<BeerModelType>();
            services.AddSingleton<BeerImageModelType>();
            services.AddSingleton<HomeContentModelType>();
            services.AddSingleton<FeaturedBeerModelType>();
            services.AddSingleton<FooterItemModelType>();
            services.AddSingleton<BannerModelType>();
            services.AddSingleton<SeoModelType>();
            services.AddSingleton<StaticPageModelType>();

            // JWT
            var secret = Configuration["Jwt:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireRole("admin"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGraphiQl("/graphiql", "/graphql");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
