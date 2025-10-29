using Microsoft.EntityFrameworkCore;
using ModelSecurityRepaso.Entity.Context;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ModelSecurityRepaso.Business.Interface.Security;
using ModelSecurityRepaso.Business.Implements.Security;
using ModelSecurityRepaso.Business.Implements;
using ModelSecurityRepaso.Business.Interface;
using ModelSecurityRepaso.Data.Implements;
using ModelSecurityRepaso.Data.Interface;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ModelSecurityRepaso.Web.Data;

// Cargar variables de entorno desde el archivo .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Selecciona el proveedor de base de datos desde la configuración
var dbProvider = builder.Configuration["DbProvider"];
string connStr = dbProvider switch
{
    "MySql" => builder.Configuration.GetConnectionString("MySqlConnection"),
    "Postgres" => builder.Configuration.GetConnectionString("PostgresConnection"),
    "SqlServer" => builder.Configuration.GetConnectionString("SqlServerConnection"),
    _ => throw new Exception("Proveedor de base de datos no soportado")
};

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "MySql":
            options.UseMySql(connStr, ServerVersion.AutoDetect(connStr), 
                b => b.MigrationsAssembly("Web"));
            break;
        case "Postgres":
            options.UseNpgsql(connStr, 
                b => b.MigrationsAssembly("Web"));
            break;
        case "SqlServer":
            options.UseSqlServer(connStr, 
                b => b.MigrationsAssembly("Web"));
            break;
        default:
            throw new Exception("Proveedor de base de datos no soportado");
    }
});

// ========================================
// Configuración JWT
// ========================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey no configurada");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// ========================================
// Inyección de Dependencias - JWT Service
// ========================================
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ========================================
// Inyección de Dependencias - Data Layer
// ========================================
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<RoleData>();
builder.Services.AddScoped<IRoleData, RoleData>();
builder.Services.AddScoped<FormData>();
builder.Services.AddScoped<IFormData, FormData>();
builder.Services.AddScoped<PermissionData>();
builder.Services.AddScoped<IPermissionData, PermissionData>();
builder.Services.AddScoped<RefreshTokenData>();
builder.Services.AddScoped<IRefreshTokenData, RefreshTokenData>();
builder.Services.AddScoped<FormModuleData>();
builder.Services.AddScoped<IFormModuleData, FormModuleData>();
builder.Services.AddScoped<ModuleData>();
builder.Services.AddScoped<IModuleData, ModuleData>();
builder.Services.AddScoped<PersonData>();
builder.Services.AddScoped<IPersonData, PersonData>();
builder.Services.AddScoped<RoleFormPermissionData>();
builder.Services.AddScoped<IRoleFormPermissionData, RoleFormPermissionData>();
builder.Services.AddScoped<UserRoleData>();
builder.Services.AddScoped<IUserRoleData, UserRoleData>();

// ========================================
// Inyección de Dependencias - Business Layer
// ========================================
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IRoleBusiness, RoleBusiness>();
builder.Services.AddScoped<IFormBusiness, FormBusiness>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();
builder.Services.AddScoped<IRefreshTokenBusiness, RefreshTokenBusiness>();
builder.Services.AddScoped<IFormModuleBusiness, FormModuleBusiness>();
builder.Services.AddScoped<IModuleBusiness, ModuleBusiness>();
builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();
builder.Services.AddScoped<IRoleFormPermissionBusiness, RoleFormPermissionBusiness>();
builder.Services.AddScoped<IUserRoleBusiness, UserRoleBusiness>();

// ========================================
// AutoMapper
// ========================================
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container.
builder.Services.AddControllers();

// ========================================
// Swagger/OpenAPI Configuration
// ========================================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Model Security API",
        Version = "v1",
        Description = "API para gestión de seguridad con autenticación JWT y control de roles",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "dev@example.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT"
        }
    });

    // Configurar autenticación JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa un token JWT válido con el prefijo 'Bearer'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            new string[] { }
        }
    });

    // Incluir comentarios XML si existen
    var xmlFilename = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Model Security API v1");
        options.RoutePrefix = "swagger"; // URL: https://localhost:xxxx/swagger
        options.DefaultModelsExpandDepth(2);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ========================================
// Inicializar Base de Datos con Datos de Prueba
// ========================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DatabaseSeeder.Initialize(dbContext);
}

// ========================================
// Middleware de Autenticación y Autorización
// ========================================
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
