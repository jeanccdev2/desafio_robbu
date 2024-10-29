using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Robbu.Desafio.Jean.API.Persistence;
using Robbu.Desafio.Jean.API.Persistence.DbContexts;
using Robbu.Desafio.Jean.API.Persistence.Repositories;
using Robbu.Desafio.Jean.API.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Robbu.Desafio.Jean.API.Settings
{
    public static class ApiServicesExtensions
    {
        public static WebApplicationBuilder AddApiDependencies(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddEndpointsApiExplorer()
                .AddMemoryCache()
                .AddControllers();

            builder
                .AddApplicationDbContext()
                .AddDependenciesInjections()
                .AddMediator()
                .AddCorsPolicy()
                .AddAspNetIdentity()
                .AddSwagger();

            builder.AddAuthentication();

            return builder;
        }

        private static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<Program>();
            });

            return builder;
        }

        private static WebApplicationBuilder AddApplicationDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }

        private static WebApplicationBuilder AddDependenciesInjections(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            return builder;
        }

        private static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.WithOrigins("*")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return builder;
        }

        private static WebApplicationBuilder AddAspNetIdentity(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                .AddDefaultTokenProviders();

            builder.Services
                .AddDbContext<AspNetIdentityDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }

        private static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Robbu.Desafio.Jean.API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Digite seu token JWT aqui."
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
            });

            return builder;
        }

        private static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}