# 🚀 Guía de Inicio Rápido

Esta guía te ayuda a comenzar con el proyecto **ModelSecurityRepaso** en 5 minutos.

---

## ✅ Prerrequisitos

- ✔️ .NET 8 SDK instalado
- ✔️ SQL Server, MySQL o PostgreSQL disponible
- ✔️ Git y Terminal
- ✔️ VS Code o Visual Studio Community

---

## 📦 1. Instalar Dependencias

```bash
# Navega a la carpeta Web
cd Web

# Instala paquetes NuGet requeridos
dotnet add package DotNetEnv
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package Pomelo.EntityFrameworkCore.MySql  # Si usas MySQL
dotnet add package Npgsql.EntityFrameworkCore        # Si usas PostgreSQL

# Restaura todas las dependencias
dotnet restore
```

---

## 🔧 2. Configurar Base de Datos

### Opción A: MySQL (Recomendado para desarrollo)

```bash
# Crear base de datos
mysql -u root -p
> CREATE DATABASE ModelSecurityDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
> EXIT;

# Actualizar .env
DbProvider=MySql
ConnectionStrings__MySql=Server=localhost;Port=3306;Database=ModelSecurityDB;Uid=root;Pwd=tu_password;
```

### Opción B: PostgreSQL

```bash
# Crear base de datos
psql -U postgres
> CREATE DATABASE modelsecuritydb;
> \q

# Actualizar .env
DbProvider=PostgreSQL
ConnectionStrings__PostgreSQL=Host=localhost;Port=5432;Database=modelsecuritydb;Username=postgres;Password=tu_password;
```

---

## 🗄️ 3. Aplicar Migraciones

```bash
cd Web

# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar cambios a BD
dotnet ef database update
```

---

## ▶️ 4. Ejecutar la Aplicación

```bash
cd Web
dotnet run

# La API estará disponible en:
# https://localhost:5001
# http://localhost:5000
```

---

## 🧪 5. Probar Endpoints

### Crear Usuario

```bash
curl -X POST http://localhost:5000/api/user \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "password": "SecurePassword123",
    "personId": 1
  }'
```

### Obtener Usuario por ID

```bash
curl http://localhost:5000/api/user/1
```

### Actualizar Usuario

```bash
curl -X PUT http://localhost:5000/api/user/1 \
  -H "Content-Type: application/json" \
  -d '{
    "id": 1,
    "name": "Juan Carlos",
    "email": "juan@example.com",
    "password": "NewPassword123",
    "personId": 1
  }'
```

---

## 📋 Flujo de Creación de una Nueva Entidad

Sigue estos 10 pasos (documentados en README.md):

1. **Crear Modelo** → `Entity/Model/TuEntidad.cs`
2. **Crear DTO** → `Entity/Dto/TuEntidadDto.cs`
3. **Configurar en DbContext** → `Entity/Context/ApplicationDbContext.cs`
4. **Repositorio** → `Data/Implements/TuEntidadData.cs`
5. **Lógica de Negocio** → `Business/Implements/TuEntidadBusiness.cs`
6. **Interfaz Negocio** → `Business/Interface/ITuEntidadBusiness.cs`
7. **AutoMapper Profile** → `Utilities/Mappers/Profiles/TuEntidadProfile.cs`
8. **Controlador** → `Web/Controllers/TuEntidadController.cs`
9. **Registrar en DI** → `Web/Program.cs`
10. **Migración y Update** → `dotnet ef migrations add` + `update`

---

## 🔍 Estructura Carpetas Clave

```
.
├── Entity/           ← Modelos + Context (ORM)
├── Data/             ← Repositorios (CRUD)
├── Business/         ← Lógica + Validaciones
├── Utilities/        ← Excepciones + Mappers
└── Web/              ← API + Controladores
```

---

## 🎯 Principios SOLID en Este Proyecto

| Principio | Ubicación | Ejemplo |
|-----------|-----------|---------|
| **S** - SRP | Cada carpeta tiene responsabilidad única | Data = CRUD, Business = Validaciones |
| **O** - OCP | Clases base genéricas extensibles | `BaseData<T>` → Subclases sin modificar |
| **L** - LSP | Interfaces intercambiables | `IBaseBusiness<T>` implementada por todas |
| **I** - ISP | Interfaces segregadas no monolíticas | `IReadable<T>` + `IWritable<T>` |
| **D** - DIP | Inyección de dependencias | `services.AddScoped<Interface, Implementation>` |

---

## 🐛 Troubleshooting

### Error: "DbContext no está configurado"
```csharp
// En Program.cs, asegúrate de tener:
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ...));
```

### Error: "Usuario no encontrado" (404)
```csharp
// Verifica que la migración se aplicó:
dotnet ef database update
```

### Error: "El email ya existe"
```csharp
// Esto es correcto, hay validación de unicidad.
// Intenta con otro email.
```

---

## 📚 Enlaces Útiles

- [README Completo](./README.md) - Documentación exhaustiva
- [Modelo MER](./Diagram/mer.xml) - Diagrama de base de datos
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/)

---

## 💡 Próximos Pasos

1. ✅ Entender la arquitectura (lee README.md)
2. ✅ Crear tu primera entidad (sigue guía paso a paso)
3. ✅ Implementar métodos personalizados
4. ✅ Agregar validaciones complejas
5. ✅ Usar transacciones atómicas
6. ✅ Generar reportes con Dapper

---

**¿Necesitas ayuda?** Consulta el README.md para ejemplos detallados.
