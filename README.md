# Modelo de Seguridad - Arquitectura por Capas ASP.NET Core

Documentación completa del sistema de seguridad empresarial implementado con **ASP.NET Core 8**, **Entity Framework Core**, y una **arquitectura limpia por capas** que respeta principios **SOLID** y buenas prácticas de programación orientada a objetos.

---

## 📋 Tabla de Contenidos

1. [Visión General](#visión-general)
2. [Arquitectura por Capas](#arquitectura-por-capas)
3. [Principios SOLID y Buenas Prácticas](#principios-solid-y-buenas-prácticas)
4. [Modelo de Entidades (MER)](#modelo-de-entidades-mer)
5. [Estructura de Carpetas](#estructura-de-carpetas)
6. [Flujo de Trabajo](#flujo-de-trabajo)
7. [Guía: Crear una Nueva Entidad](#guía-crear-una-nueva-entidad)
8. [Guía: Métodos Personalizados](#guía-métodos-personalizados)
9. [Configuración Multi-DB](#configuración-multi-db)
10. [Ejemplos Prácticos](#ejemplos-prácticos)

---

## 🎯 Visión General

Este proyecto implementa un **sistema de seguridad RBAC (Role-Based Access Control)** con:

- ✅ Control de usuarios y roles
- ✅ Permisos granulares por formulario
- ✅ Auditoría automática de cambios
- ✅ Soporte multi-base de datos (MySQL, PostgreSQL, SQL Server)
- ✅ Validación y manejo de errores centralizado
- ✅ Arquitectura escalable y mantenible

---

## 🏗️ Arquitectura por Capas

```
ModelSecurityRepaso.sln
├── Entity/         ← Modelos y contexto de BD
├── Data/           ← Repositorios (CRUD genérico)
├── Business/       ← Lógica de negocio y validaciones
├── Utilities/      ← Excepciones, Mappers, Helpers
└── Web/            ← Controladores y endpoints API
```

### Flujo de Datos

```mermaid
graph LR
    A[Cliente HTTP] -->|Request| B[Controller]
    B -->|Valida| C[BaseBusiness]
    C -->|Persiste| D[BaseData Repository]
    D -->|ORM| E[DbContext]
    E -->|SQL| F[(Base de Datos)]
    F -->|Datos| E
    E -->|Entidades| D
    D -->|DTOs| C
    C -->|Response| B
    B -->|JSON| A
```

---

## 🎓 Principios SOLID y Buenas Prácticas

### 1. **S - Single Responsibility Principle (SRP)**
Cada clase tiene una única responsabilidad:

- `BaseData<T>`: Solo operaciones CRUD
- `BaseBusiness<T>`: Solo validaciones y lógica
- `BaseController<T>`: Solo manejo de HTTP
- `ApplicationDbContext`: Solo configuración EF

```csharp
// ✅ CORRECTO - SRP
public class UserBusiness : BaseBusiness<User>
{
    // Solo validaciones de usuario
    public async Task ValidateUserAsync(User user) { }
}

// ❌ INCORRECTO - Mezcla responsabilidades
public class UserBusiness
{
    public void ValidateUser() { }
    public void SendEmail() { }        // No pertenece aquí
    public void SaveToDatabase() { }   // Corresponde a Data
}
```

---

### 2. **O - Open/Closed Principle (OCP)**
Abierto para extensión, cerrado para modificación:

```csharp
// Clase base genérica (cerrada a modificación)
public abstract class BaseData<T> where T : BaseModel
{
    public virtual async Task<T> GetByIdAsync(int id) { }
    public virtual async Task AddAsync(T entity) { }
    // ...
}

// Extensión para casos especiales (abierta a extensión)
public class UserData : BaseData<User>
{
    // Agrega métodos específicos sin modificar la base
    public async Task<User> GetByEmailAsync(string email) { }
}
```

---

### 3. **L - Liskov Substitution Principle (LSP)**
Los subtipos pueden reemplazar al supertipo:

```csharp
// Interface
public interface IBaseBusiness<T> where T : BaseModel
{
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
}

// Implementaciones intercambiables
public class UserBusiness : BaseBusiness<User>, IBaseBusiness<User> { }
public class RoleBusiness : BaseBusiness<Role>, IBaseBusiness<Role> { }

// El controlador no importa cuál es la implementación
public class BaseController<T> where T : BaseModel
{
    private readonly IBaseBusiness<T> _business; // Funciona con cualquiera
}
```

---

### 4. **I - Interface Segregation Principle (ISP)**
Interfaces específicas, no genéricas enormes:

```csharp
// ✅ CORRECTO - Interfaces segregadas
public interface IReadable<T>
{
    Task<T> GetByIdAsync(int id);
}

public interface IWritable<T>
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
}

// ❌ INCORRECTO - Interface monolítica
public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task AddAsync(T entity);
    // ... 20 métodos más
}
```

---

### 5. **D - Dependency Injection Principle (DIP)**
Depender de abstracciones, no de concreciones:

```csharp
// En Program.cs
services.AddScoped<IBaseBusiness<User>, UserBusiness>();
services.AddScoped<IBaseData<User>, UserData>();

// En el controlador
public class UserController : BaseController<User>
{
    // Recibe interfaz, no clase concreta
    public UserController(IBaseBusiness<User> business) 
        : base(business) { }
}
```

---

### 6. **POO - Conceptos Clave**

#### **Herencia**
```csharp
// Clase base con comportamiento común
public abstract class BaseModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}

// Hereda automáticamente los propiedades
public class User : BaseModel { }
```

#### **Polimorfismo**
```csharp
// Método virtual que puede ser sobrescrito
public virtual async Task DeleteLogicalAsync(int id)
{
    var entity = await GetByIdAsync(id);
    entity.IsDeleted = true;
    await UpdateAsync(entity);
}

// En una subclase, personaliza el comportamiento
public override async Task DeleteLogicalAsync(int id)
{
    // Lógica adicional específica
    await base.DeleteLogicalAsync(id);
}
```

#### **Encapsulación**
```csharp
// Propiedades con acceso controlado
public class User : BaseModel
{
    private string _password; // Privado
    
    public string Email { get; set; } // Público con get/set
    
    public string Password
    {
        get { return _password; }
        set { _password = HashPassword(value); } // Valida al asignar
    }
}
```

---

## 📊 Modelo de Entidades (MER)

### Diagrama Conceptual

```mermaid
erDiagram
    PERSON ||--o| USER : "1:1"
    USER ||--o{ USER-ROLE : "1:N"
    ROLE ||--o{ USER-ROLE : "1:N"
    ROLE ||--o{ ROLE-FORM-PERMISSION : "1:N"
    FORM ||--o{ FORM-MODULE : "1:N"
    FORM-MODULE ||--o| MODULE : "N:1"
    FORM ||--o{ ROLE-FORM-PERMISSION : "1:N"
    PERMISSION ||--o{ ROLE-FORM-PERMISSION : "1:N"

    PERSON {
        int Id PK
        string FirstName
        string SecondName
        string LastName
        string LastSecondName
        string TypeDocument
        int NumberDocument UK
        string Phone
        string Address
    }

    USER {
        int Id PK
        string Name
        string Email UK
        string Password
        int PersonId FK
        boolean IsDeleted
    }

    USER-ROLE {
        int Id PK
        int UserId FK
        int RoleId FK
    }

    ROLE {
        int Id PK
        string Name
        string Description
    }

    FORM {
        int Id PK
        string Name
        string Description
        string Url
    }

    FORM-MODULE {
        int Id PK
        string Name
        int ModuleId FK
        int FormId FK
    }

    MODULE {
        int Id PK
        string Name
        string Description
    }

    PERMISSION {
        int Id PK
        string Name
        string Description
    }

    ROLE-FORM-PERMISSION {
        int Id PK
        int RoleId FK
        int FormId FK
        int PermissionId FK
    }
```

### Relaciones Explicadas

| Relación | Tipo | Descripción |
|----------|------|-------------|
| Person → User | 1:1 | Una persona tiene máximo un usuario |
| User → UserRole | 1:N | Un usuario puede tener múltiples roles |
| Role → UserRole | 1:N | Un rol puede estar asignado a múltiples usuarios |
| Role → RoleFormPermission | 1:N | Un rol tiene múltiples permisos en formularios |
| Form → FormModule | 1:N | Un formulario tiene múltiples módulos |
| FormModule → Module | N:1 | Múltiples módulos pueden estar en un formulario |
| Form → RoleFormPermission | 1:N | Un formulario tiene múltiples permisos por rol |
| Permission → RoleFormPermission | 1:N | Un permiso se asigna a múltiples roles/formularios |

---

## 📁 Estructura de Carpetas

```
ModelSecurityRepaso/
│
├── Entity/
│   ├── Context/
│   │   └── ApplicationDbContext.cs          ← Configuración EF Core
│   ├── Dto/
│   │   └── base/
│   │       └── BaseDto.cs                   ← DTO base
│   └── Model/
│       ├── base/
│       │   └── Base.cs                      ← Modelo base con auditoría
│       ├── Person.cs                        ← Entidad de persona
│       ├── User.cs                          ← Entidad de usuario
│       ├── Role.cs                          ← Rol de acceso
│       ├── UserRole.cs                      ← Relación usuario-rol
│       ├── Form.cs                          ← Formulario del sistema
│       ├── FormModule.cs                    ← Módulo de formulario
│       ├── Module.cs                        ← Módulo del sistema
│       ├── Permission.cs                    ← Permiso de acceso
│       └── RoleFormPermission.cs            ← Relación rol-form-permiso
│
├── Data/
│   ├── Implements/
│   │   └── base/
│   │       └── BaseData.cs                  ← CRUD genérico
│   └── Interface/
│       └── base/
│           └── IBaseData.cs                 ← Contrato del repositorio
│
├── Business/
│   ├── Implements/
│   │   └── base/
│   │       └── BaseBusiness.cs              ← Lógica y validaciones
│   └── Interface/
│       └── base/
│           └── IBaseBusiness.cs             ← Contrato de negocio
│
├── Utilities/
│   ├── Exception/
│   │   ├── BusinessException.cs             ← Errores de lógica
│   │   ├── DataException.cs                 ← Errores de datos
│   │   └── ControllerException.cs           ← Errores HTTP
│   └── Mappers/
│       └── Profiles/
│           └── UserProfile.cs               ← AutoMapper config
│
└── Web/
    ├── Controllers/
    │   ├── Implements/
    │   │   └── base/
    │   │       └── BaseController.cs         ← CRUD genérico HTTP
    │   └── Interface/
    │       └── base/
    │           └── IBaseController.cs        ← Contrato HTTP
    ├── Program.cs                            ← Configuración DI
    ├── appsettings.json                      ← Config por entorno
    └── .env                                  ← Variables de entorno
```

---

## 🔄 Flujo de Trabajo

### Ciclo de una Solicitud HTTP

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant Business
    participant Data
    participant DbContext
    participant Database

    Client->>Controller: POST /user
    activate Controller
    
    Controller->>Business: AddAsync(userDto)
    activate Business
    
    Business->>Business: ValidateAsync(user)
    alt Validación Falla
        Business-->>Controller: BusinessException
    else Validación OK
        Business->>Data: AddAsync(user)
        activate Data
        
        Data->>DbContext: Set<User>.AddAsync()
        activate DbContext
        
        DbContext->>DbContext: EnsureAudit()
        DbContext->>Database: SaveChangesAsync()
        activate Database
        
        Database-->>DbContext: OK + Id
        deactivate Database
        
        DbContext-->>Data: Entity
        deactivate DbContext
        
        Data-->>Business: Entity
        deactivate Data
        
        Business-->>Controller: UserDto
    end
    
    deactivate Business
    Controller-->>Client: 201 Created + Location
    deactivate Controller
```

### Estados y Transiciones

```mermaid
stateDiagram-v2
    [*] --> Request: Cliente envía solicitud
    Request --> Validation: Controller recibe
    Validation --> Error1: Falla validación HTTP
    Validation --> Business: Validación OK
    Business --> Error2: Falla lógica negocio
    Business --> Data: Lógica OK
    Data --> Error3: Falla persistencia
    Data --> Audit: Persistencia OK
    Audit --> Response: Auditoría registrada
    Error1 --> BadRequest: 400
    Error2 --> Conflict: 409
    Error3 --> InternalError: 500
    Response --> Success: 200/201
    BadRequest --> [*]
    Conflict --> [*]
    InternalError --> [*]
    Success --> [*]
```

---

## 🆕 Guía: Crear una Nueva Entidad

Supongamos que quieres agregar una entidad llamada **`Department`** (Departamento).

### Paso 1: Crear el Modelo en Entity

**Archivo:** `Entity/Model/Department.cs`

```csharp
using ModelSecurityRepaso.Entity.Model.base;

namespace ModelSecurityRepaso.Entity.Model
{
    /// <summary>
    /// Representa un departamento dentro de la organización.
    /// Hereda propiedades de auditoría de BaseModel.
    /// </summary>
    public class Department : BaseModel
    {
        /// <summary>
        /// Nombre único del departamento.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del propósito del departamento.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Identificador del gerente (FK a User).
        /// </summary>
        public int ManagerId { get; set; }

        /// <summary>
        /// Navegación a Usuario gerente (relación).
        /// </summary>
        public virtual User? Manager { get; set; }

        /// <summary>
        /// Colección de usuarios en el departamento.
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
```

### Paso 2: Crear DTO en Entity

**Archivo:** `Entity/Dto/DepartmentDto.cs`

```csharp
namespace ModelSecurityRepaso.Entity.Dto
{
    /// <summary>
    /// DTO para transferir datos de Department en la API.
    /// </summary>
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
    }
}
```

### Paso 3: Configurar en ApplicationDbContext

**Archivo:** `Entity/Context/ApplicationDbContext.cs`

Agrega esta línea en el método `OnModelCreating()`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // ... configuraciones existentes ...

    // Configurar Department
    modelBuilder.Entity<Department>(entity =>
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        entity.Property(e => e.Description)
            .HasMaxLength(500);

        // Relación 1:N con User
        entity.HasMany(d => d.Users)
            .WithOne(u => u.Department)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación 1:1 con Manager (User)
        entity.HasOne(d => d.Manager)
            .WithMany()
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    // Registrar DbSet
}

// Dentro de DbContext:
public DbSet<Department> Departments { get; set; }
```

### Paso 4: Crear Repositorio (BaseData)

**Archivo:** `Data/Implements/DepartmentData.cs`

```csharp
using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Data.Implements.base;

namespace ModelSecurityRepaso.Data.Implements
{
    /// <summary>
    /// Repositorio específico para operaciones con Department.
    /// Extiende BaseData genérico con métodos personalizados.
    /// </summary>
    public class DepartmentData : BaseData<Department>
    {
        /// <summary>
        /// Constructor que recibe el contexto de BD por inyección.
        /// </summary>
        public DepartmentData(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Obtiene un departamento por nombre (único).
        /// </summary>
        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(d => d.Manager)
                .Include(d => d.Users)
                .FirstOrDefaultAsync(d => d.Name == name && !d.IsDeleted);
        }

        /// <summary>
        /// Obtiene departamentos con su gerente, paginados.
        /// </summary>
        public async Task<(IEnumerable<Department> Items, int Total)> GetWithManagerAsync(
            int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Include(d => d.Manager)
                .Where(d => !d.IsDeleted)
                .OrderBy(d => d.Name);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
```

### Paso 5: Crear Lógica de Negocio (BaseBusiness)

**Archivo:** `Business/Implements/DepartmentBusiness.cs`

```csharp
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Implements.base;
using ModelSecurityRepaso.Data.Implements;
using ModelSecurityRepaso.Utilities.Exception;

namespace ModelSecurityRepaso.Business.Implements
{
    /// <summary>
    /// Lógica de negocio para operaciones con Department.
    /// Valida reglas de negocio antes de persistir.
    /// </summary>
    public class DepartmentBusiness : BaseBusiness<Department>
    {
        private readonly DepartmentData _departmentData;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public DepartmentBusiness(DepartmentData departmentData) : base(departmentData)
        {
            _departmentData = departmentData;
        }

        /// <summary>
        /// Valida que el nombre del departamento sea único.
        /// </summary>
        public async Task ValidateDepartmentAsync(Department department)
        {
            if (string.IsNullOrWhiteSpace(department.Name))
                throw new BusinessException("El nombre del departamento es requerido.");

            if (department.Name.Length < 3 || department.Name.Length > 100)
                throw new BusinessException("El nombre debe tener entre 3 y 100 caracteres.");

            // Verifica unicidad
            var existing = await _departmentData.GetByNameAsync(department.Name);
            if (existing != null && existing.Id != department.Id)
                throw new BusinessException(
                    $"Ya existe un departamento con el nombre '{department.Name}'.");

            if (department.ManagerId <= 0)
                throw new BusinessException("El departamento debe tener un gerente.");
        }

        /// <summary>
        /// Agrega un nuevo departamento con validaciones.
        /// </summary>
        public new async Task<Department> AddAsync(Department department)
        {
            await ValidateDepartmentAsync(department);
            return await base.AddAsync(department);
        }

        /// <summary>
        /// Actualiza un departamento con validaciones.
        /// </summary>
        public new async Task<Department> UpdateAsync(Department department)
        {
            await ValidateDepartmentAsync(department);
            return await base.UpdateAsync(department);
        }
    }
}
```

### Paso 6: Crear Interfaz

**Archivo:** `Business/Interface/IDepartmentBusiness.cs`

```csharp
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Business.Interface.base;

namespace ModelSecurityRepaso.Business.Interface
{
    /// <summary>
    /// Contrato para operaciones de negocio con Department.
    /// </summary>
    public interface IDepartmentBusiness : IBaseBusiness<Department>
    {
        Task ValidateDepartmentAsync(Department department);
    }
}
```

### Paso 7: Crear AutoMapper Profile

**Archivo:** `Utilities/Mappers/Profiles/DepartmentProfile.cs`

```csharp
using AutoMapper;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Dto;

namespace ModelSecurityRepaso.Utilities.Mappers.Profiles
{
    /// <summary>
    /// Mapeo entre entidad Department y su DTO.
    /// </summary>
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.ManagerName, 
                    opt => opt.MapFrom(src => src.Manager!.Name))
                .ReverseMap();
        }
    }
}
```

### Paso 8: Crear Controlador

**Archivo:** `Web/Controllers/DepartmentController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Web.Controllers.Implements.base;
using ModelSecurityRepaso.Business.Interface;

namespace ModelSecurityRepaso.Web.Controllers
{
    /// <summary>
    /// Controlador REST para operaciones CRUD de Department.
    /// Hereda endpoints genéricos de BaseController.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : BaseController<Department>
    {
        private readonly IDepartmentBusiness _departmentBusiness;

        public DepartmentController(IDepartmentBusiness departmentBusiness) 
            : base(departmentBusiness)
        {
            _departmentBusiness = departmentBusiness;
        }

        // Hereda automáticamente:
        // GET    /api/department/{id}
        // POST   /api/department
        // PUT    /api/department/{id}
        // PATCH  /api/department/{id}
        // DELETE /api/department/{id}

        // Métodos personalizados adicionales (opcional):
        // [HttpGet("by-name/{name}")]
        // public async Task<IActionResult> GetByName(string name) { }
    }
}
```

### Paso 9: Registrar en Dependency Injection (Program.cs)

```csharp
// En el método ConfigureServices():

// Registrar repositorio
services.AddScoped<DepartmentData>();

// Registrar lógica de negocio
services.AddScoped<IDepartmentBusiness, DepartmentBusiness>();

// Registrar AutoMapper profile
services.AddAutoMapper(typeof(DepartmentProfile));
```

### Paso 10: Crear Migración

```bash
cd Web
dotnet ef migrations add AddDepartment
dotnet ef database update
```

---

## ⚙️ Guía: Métodos Personalizados

### En la Capa Data (BaseData)

Cuando necesitas operaciones específicas que no son CRUD básico:

```csharp
/// <summary>
/// Obtiene usuarios con búsqueda filtrada.
/// Ejemplo de método personalizado complejo.
/// </summary>
public async Task<IEnumerable<User>> SearchActiveUsersAsync(
    string searchTerm, 
    int roleId)
{
    var query = _dbSet.AsQueryable();

    // Filtro 1: No eliminados lógicamente
    query = query.Where(u => !u.IsDeleted);

    // Filtro 2: Búsqueda por email o nombre
    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = query.Where(u => 
            u.Email.Contains(searchTerm) || 
            u.Name.Contains(searchTerm));
    }

    // Filtro 3: Por rol
    if (roleId > 0)
    {
        query = query
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId));
    }

    return await query
        .Include(u => u.Person)
        .Include(u => u.UserRoles)
        .OrderBy(u => u.Name)
        .ToListAsync();
}
```

**Ubicación:** `Data/Implements/UserData.cs`

---

### En la Capa Business (BaseBusiness)

Para lógica compleja con validaciones:

```csharp
/// <summary>
/// Asigna un rol a un usuario con validaciones complejas.
/// Verifica que el usuario y rol existan y sean activos.
/// </summary>
public async Task AssignRoleAsync(int userId, int roleId)
{
    // Validar que el usuario exista
    var user = await _userData.GetByIdAsync(userId);
    if (user == null)
        throw new BusinessException($"Usuario ID {userId} no encontrado.");

    if (user.IsDeleted)
        throw new BusinessException("No se puede asignar roles a un usuario eliminado.");

    // Validar que el rol exista
    var role = await _roleData.GetByIdAsync(roleId);
    if (role == null)
        throw new BusinessException($"Rol ID {roleId} no encontrado.");

    // Verificar que la asignación no exista ya
    var existingAssignment = await _userRoleData.GetAsync(
        ur => ur.UserId == userId && ur.RoleId == roleId);
    
    if (existingAssignment != null)
        throw new BusinessException(
            "El usuario ya tiene asignado este rol.");

    // Crear la asignación
    var userRole = new UserRole 
    { 
        UserId = userId, 
        RoleId = roleId 
    };

    await _userRoleData.AddAsync(userRole);
}
```

**Ubicación:** `Business/Implements/UserBusiness.cs`

---

### En el Controlador (BaseController)

Para lógica específica de HTTP:

```csharp
/// <summary>
/// Busca usuarios con criterios complejos.
/// Endpoint personalizado que no está en el CRUD base.
/// </summary>
[HttpGet("search")]
public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(
    [FromQuery] string? term,
    [FromQuery] int? roleId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
{
    try
    {
        var users = await _userBusiness.SearchActiveUsersAsync(term ?? "", roleId ?? 0);
        
        var paginated = users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = _mapper.Map<IEnumerable<UserDto>>(paginated);

        return Ok(new
        {
            message = "Búsqueda completada",
            data = dtos,
            total = users.Count(),
            page,
            pageSize
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = ex.Message });
    }
}
```

**Ubicación:** `Web/Controllers/UserController.cs`

---

## 🔧 Configuración Multi-DB

### Variables de Entorno (.env)

**Archivo:** `.env`

```env
# Seleccionar proveedor: MySql, PostgreSQL, SqlServer
DbProvider=MySql

# Cadena de conexión MySQL
ConnectionStrings__MySql=Server=localhost;Port=3306;Database=ModelSecurityDB;Uid=root;Pwd=password;

# Cadena de conexión PostgreSQL
ConnectionStrings__PostgreSQL=Host=localhost;Port=5432;Database=ModelSecurityDB;Username=postgres;Password=password;

# Cadena de conexión SQL Server
ConnectionStrings__SqlServer=Server=localhost;Database=ModelSecurityDB;User Id=sa;Password=password;
```

### Configuración en Program.cs

```csharp
using DotNetEnv;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();

var provider = Environment.GetEnvironmentVariable("DbProvider") ?? "MySql";
var connectionString = Environment.GetEnvironmentVariable(
    $"ConnectionStrings__{provider}") ?? "";

// Registrar DbContext según el proveedor
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (provider.ToLower())
    {
        case "postgresql":
            options.UseNpgsql(connectionString);
            break;
        case "sqlserver":
            options.UseSqlServer(connectionString);
            break;
        default: // MySql
            options.UseMySql(connectionString, 
                new MySqlServerVersion(new Version(8, 0, 0)));
            break;
    }
});

// ... resto de configuración
```

---

## 📘 Ejemplos Prácticos

### Ejemplo 1: Crear un Usuario Completo

```csharp
// En el controlador
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
{
    try
    {
        // 1. Mapear DTO a Entidad
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            PersonId = dto.PersonId
        };

        // 2. Llama a negocio (que valida)
        var createdUser = await _userBusiness.AddAsync(user);

        // 3. Asigna un rol por defecto
        await _userBusiness.AssignRoleAsync(createdUser.Id, 2); // Rol "Usuario"

        // 4. Mapea a DTO de respuesta
        var userDto = _mapper.Map<UserDto>(createdUser);

        return CreatedAtAction(nameof(GetById), 
            new { id = createdUser.Id }, userDto);
    }
    catch (BusinessException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = "Error interno" });
    }
}
```

---

### Ejemplo 2: Implementar Método de Búsqueda Personalizado

```csharp
// En UserBusiness
public async Task<List<User>> GetActiveUsersByRoleAsync(string roleName)
{
    // Obtiene usuarios activos que tengan un rol específico
    var query = _userRepository._dbSet
        .Where(u => !u.IsDeleted)
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .Where(u => u.UserRoles.Any(ur => 
            ur.Role.Name == roleName))
        .OrderBy(u => u.Name);

    return await query.ToListAsync();
}
```

---

### Ejemplo 3: Transacción Atómica Multi-Entidad

```csharp
// En Business
public async Task CreateDepartmentWithManagerAsync(
    Department department, 
    User manager)
{
    // Ejecuta transacción completa o rollback
    await ExecuteInTransactionAsync(async () =>
    {
        // 1. Validar gerente
        if (manager.IsDeleted)
            throw new BusinessException("El gerente no puede estar eliminado");

        // 2. Guardar departamento
        var createdDept = await _departmentData.AddAsync(department);

        // 3. Asignar gerente
        manager.DepartmentId = createdDept.Id;
        await _userData.UpdateAsync(manager);

        // Si algo falla, todo se revierte automáticamente
    });
}
```

---

### Ejemplo 4: Auditoría Automática

La auditoría se activa automáticamente al guardar:

```csharp
// En ApplicationDbContext
public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
{
    EnsureAudit(); // Registra automáticamente
    return await base.SaveChangesAsync(cancellationToken);
}

private void EnsureAudit()
{
    foreach (var entry in ChangeTracker.Entries<BaseModel>())
    {
        switch (entry.State)
        {
            case EntityState.Added:
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.ModifiedAt = DateTime.UtcNow;
                break;

            case EntityState.Modified:
                entry.Entity.ModifiedAt = DateTime.UtcNow;
                break;
        }
    }
}
```

---

## 🚀 Siguientes Pasos

1. **Crear migración inicial:**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

2. **Instalar paquetes requeridos:**
   ```bash
   dotnet add package DotNetEnv
   dotnet add package AutoMapper
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   ```

3. **Probar endpoints:**
   ```bash
   # Crear usuario
   curl -X POST http://localhost:5000/api/user \
     -H "Content-Type: application/json" \
     -d '{"name":"Juan","email":"juan@example.com"}'
   ```

---

## 📚 Referencias

- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Entity Framework Core](https://docs.microsoft.com/ef/)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/aspnet/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## ✍️ Notas Finales

- **Nunca** modifiques `BaseData` o `BaseBusiness` directamente; extiende con subclases.
- **Siempre** crea DTOs para exponer en API; nunca devuelvas entidades directamente.
- **Valida** en la capa Business, no en el controlador.
- **Usa inyección** de dependencias; evita `new` para servicios.
- **Documenta** con XML comments; facilita el mantenimiento.

---

**Última actualización:** 29 de octubre, 2025  
**Versión:** 1.0.0  
**Autor:** Equipo de Desarrollo
