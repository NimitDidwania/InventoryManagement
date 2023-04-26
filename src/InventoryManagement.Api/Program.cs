using System.Reflection;
using FluentValidation.AspNetCore;
using InventoryManagement.Api.Middlewares;
using InventoryManagement.Api.Services.Implementations;
using InventoryManagement.Api.Services.Interfaces;
using InventoryManagement.Common.Entities;
using InventoryManagement.Common.ExceptionHandling;
using InventoryManagement.Common.Logger;
using InventoryManagement.Common.Repositories.Implementations;
using InventoryManagement.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<ProjectContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("mssql"), b => b.MigrationsAssembly("InventoryManagement.Api")));
builder.Services.AddScoped<IInventoryServices, InventoryServices>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ICustomLogger,FileLogger>();
builder.Services.AddControllers()
    .AddFluentValidation(c =>
    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config => {
            config.SwaggerDoc("v1", new OpenApiInfo() { Title = "InventoryManagement.Api", Version = "v1",
            Description = "This API is a solution for a School Uniform Shop. The API is protected and only authorized users can access. There are two types of users- ADMIN and CONSUMER with different accessibilities." });
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            config.IncludeXmlComments(xmlPath);
        });
        

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<TokenValidation>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
