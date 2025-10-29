using Microsoft.EntityFrameworkCore;
using Entity.Context;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities.Security;
using Business.Implements;
using Business.Interface;
using Data.Implements;

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
            options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
            break;
        case "Postgres":
            options.UseNpgsql(connStr);
            break;
        case "SqlServer":
            options.UseSqlServer(connStr);
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
builder.Services.AddScoped<RoleData>();
builder.Services.AddScoped<FormData>();
builder.Services.AddScoped<PermissionData>();
builder.Services.AddScoped<RefreshTokenData>();

// ========================================
// Inyección de Dependencias - Business Layer
// ========================================
builder.Services.AddScoped<IUserBusiness, UserBusiness>();
builder.Services.AddScoped<IRoleBusiness, RoleBusiness>();
builder.Services.AddScoped<IFormBusiness, FormBusiness>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();

// ========================================
// AutoMapper
// ========================================
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container.
builder.Services.AddControllers();
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
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ========================================
// Middleware de Autenticación y Autorización
// ========================================
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
