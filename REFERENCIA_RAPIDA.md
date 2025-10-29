# 🎯 Guía de Referencia Rápida

Una hoja de trucos para desarrolladores que ya conocen el proyecto.

---

## 📦 Crear Nueva Entidad (10 Pasos Resumidos)

```
1. Modelo         → Entity/Model/TuEntidad.cs (hereda BaseModel)
2. DTO            → Entity/Dto/TuEntidadDto.cs
3. DbContext      → Entity/Context/ApplicationDbContext.cs (OnModelCreating)
4. Repository     → Data/Implements/TuEntidadData.cs : BaseData<TuEntidad>
5. Business       → Business/Implements/TuEntidadBusiness.cs : BaseBusiness<TuEntidad>
6. Interface      → Business/Interface/ITuEntidadBusiness.cs : IBaseBusiness<TuEntidad>
7. Mapper         → Utilities/Mappers/Profiles/TuEntidadProfile.cs
8. Controller     → Web/Controllers/TuEntidadController.cs : BaseController<TuEntidad>
9. DI Config      → Web/Program.cs (services.AddScoped)
10. Migración     → dotnet ef migrations add + dotnet ef database update
```

---

## 🔗 Inyección de Dependencias (DI)

```csharp
// En Program.cs
services.AddScoped<ITuEntidadBusiness, TuEntidadBusiness>();
services.AddScoped<TuEntidadData>();
services.AddAutoMapper(typeof(TuEntidadProfile));
```

---

## 🎨 Estructura de Modelo

```csharp
public class MiEntidad : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public int ForeignKeyId { get; set; }
    
    // Navegación
    public virtual OtraEntidad? Relacionada { get; set; }
}

// Hereda automáticamente:
// - int Id (PK)
// - DateTime CreatedAt (auditoría)
// - DateTime ModifiedAt (auditoría)
// - bool IsDeleted (delete lógico)
```

---

## 💾 Operaciones CRUD

```csharp
// GET
var entity = await _business.GetByIdAsync(id);

// POST (CREATE)
var newEntity = await _business.AddAsync(entity);

// PUT (UPDATE)
var updated = await _business.UpdateAsync(entity);

// PATCH (PARTIAL UPDATE)
await _business.PatchAsync(id, new { Name = "Nuevo Nombre" });

// DELETE (LOGICAL)
await _business.DeleteLogicalAsync(id);

// DELETE (PHYSICAL)
await _business.DeleteAsync(id);
```

---

## 🔐 Excepciones por Capa

```csharp
// Capa Data
throw new DataException("Error conectando a BD");

// Capa Business
throw new BusinessException("Email ya existe");

// Capa Controller
throw new ControllerException("No autorizado", 403);
```

---

## 🔄 AutoMapper

```csharp
// En Profile
CreateMap<Entity, Dto>()
    .ForMember(d => d.Property, opt => opt.MapFrom(s => s.Source))
    .ReverseMap();

// En Controller
var dto = _mapper.Map<Dto>(entity);
var entity = _mapper.Map<Entity>(dto);
```

---

## 🔀 Transacciones Atómicas

```csharp
await _business.ExecuteInTransactionAsync(async () =>
{
    var entity1 = await _business1.AddAsync(item1);
    var entity2 = await _business2.AddAsync(item2);
    // Si falla algo, todo se revierte
});

var result = await _business.ExecuteInTransactionAsync(async () =>
{
    // Operaciones...
    return resultValue;
});
```

---

## 📝 Validaciones en Business

```csharp
public async Task ValidateAsync(Entity entity)
{
    if (string.IsNullOrEmpty(entity.Name))
        throw new BusinessException("Nombre requerido");
    
    if (entity.Name.Length < 3)
        throw new BusinessException("Mínimo 3 caracteres");
    
    // Validar unicidad
    var existing = await _data.GetAsync(e => e.Name == entity.Name);
    if (existing != null && existing.Id != entity.Id)
        throw new BusinessException("Nombre duplicado");
}
```

---

## 🌐 Endpoints Automáticos del BaseController

```
GET    /api/{entity}/{id}           → Obtener por ID
POST   /api/{entity}                → Crear
PUT    /api/{entity}/{id}           → Actualizar completo
PATCH  /api/{entity}/{id}           → Actualizar parcial
DELETE /api/{entity}/{id}           → Eliminar lógico
```

---

## 🎯 Consultas Personalizadas en Data

```csharp
public async Task<IEnumerable<Entity>> SearchAsync(string term)
{
    return await _dbSet
        .Where(e => !e.IsDeleted)
        .Where(e => e.Name.Contains(term) || e.Description.Contains(term))
        .OrderBy(e => e.Name)
        .ToListAsync();
}

// Con Include
public async Task<Entity?> GetWithRelatedAsync(int id)
{
    return await _dbSet
        .Include(e => e.RelatedEntity)
        .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
}

// Paginación
public async Task<(IEnumerable<Entity>, int)> GetPagedAsync(int page, int size)
{
    var total = await _dbSet.CountAsync(e => !e.IsDeleted);
    var items = await _dbSet
        .Where(e => !e.IsDeleted)
        .OrderBy(e => e.Name)
        .Skip((page - 1) * size)
        .Take(size)
        .ToListAsync();
    
    return (items, total);
}
```

---

## 🗄️ Configuración DbContext

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Entity>(entity =>
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        // Relación 1:N
        entity.HasMany(e => e.Children)
            .WithOne(c => c.Parent)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    });
}
```

---

## 🛠️ Configurar Multi-DB

**.env:**
```env
DbProvider=MySql
ConnectionStrings__MySql=Server=localhost;Database=db;Uid=user;Pwd=pass;
ConnectionStrings__PostgreSQL=Host=localhost;Database=db;Username=user;Password=pass;
```

**Program.cs:**
```csharp
var provider = Environment.GetEnvironmentVariable("DbProvider") ?? "MySql";
var connStr = Environment.GetEnvironmentVariable($"ConnectionStrings__{provider}");

switch (provider)
{
    case "PostgreSQL":
        options.UseNpgsql(connStr);
        break;
    default:
        options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
        break;
}
```

---

## ⚠️ Manejo de Errores en Controller

```csharp
try
{
    var entity = await _business.AddAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
}
catch (BusinessException ex)
{
    return BadRequest(new { error = ex.Message });
}
catch (Exception ex)
{
    return StatusCode(500, new { error = "Error interno" });
}
```

---

## 📊 Auditoría Automática

La auditoría se activa automáticamente en `SaveChangesAsync()`:

```csharp
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    EnsureAudit();
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
                break;
            case EntityState.Modified:
                entry.Entity.ModifiedAt = DateTime.UtcNow;
                break;
        }
    }
}
```

---

## 🔍 Linq Queries Comunes

```csharp
// WHERE + OR
var results = await _dbSet
    .Where(e => e.Name.Contains(term) || e.Description.Contains(term))
    .ToListAsync();

// INCLUDE + WHERE
var entity = await _dbSet
    .Include(e => e.Children)
    .FirstOrDefaultAsync(e => e.Id == id);

// ORDER BY + DISTINCT
var unique = await _dbSet
    .Where(e => !e.IsDeleted)
    .GroupBy(e => e.Name)
    .Select(g => g.First())
    .OrderBy(e => e.Name)
    .ToListAsync();

// COUNT + SUM
var count = await _dbSet.CountAsync(e => !e.IsDeleted);
var total = await _dbSet.SumAsync(e => e.Amount);

// ANY + ALL
var hasInactive = await _dbSet.AnyAsync(e => e.IsDeleted);
var allValid = await _dbSet.AllAsync(e => e.Name != null);
```

---

## 🚀 Comandos EF Core

```bash
# Crear migración
dotnet ef migrations add NombreMigracion

# Listar migraciones
dotnet ef migrations list

# Aplicar cambios a BD
dotnet ef database update

# Revertir última migración
dotnet ef migrations remove

# Script SQL
dotnet ef migrations script

# Update desde migración específica
dotnet ef database update NombreMigracion
```

---

## 📋 Tabla de Patrones

| Patrón | Ubicación | Uso |
|--------|-----------|-----|
| Repository | Data/Implements | CRUD + Queries |
| Service | Business/Implements | Lógica + Validaciones |
| DTO | Entity/Dto | Transferencia HTTP |
| Mapper | Utilities/Mappers | Transformación |
| Exception | Utilities/Exception | Manejo errores |

---

## 🎓 Principios SOLID - Checklist

- [ ] **S:** ¿Cada clase tiene 1 responsabilidad?
- [ ] **O:** ¿Es extensible sin modificar base?
- [ ] **L:** ¿Los subtipos sustituyen supertipo?
- [ ] **I:** ¿Las interfaces son pequeñas?
- [ ] **D:** ¿Usas DI para dependencias?

---

## 🐛 Troubleshooting Rápido

```
Error: DbContext not configured
→ Verifica Program.cs builder.Services.AddDbContext()

Error: Column not mapped
→ Agrega DbSet<T> en ApplicationDbContext

Error: Migration conflict
→ dotnet ef migrations remove + rehacer

Error: Foreign key null
→ Verifica que realmente insertaste el padre

Error: Timeout
→ Agrega .CommandTimeout(60) en queries largas
```

---

## ✅ Before You Commit

- [ ] Código comentado con XML docs
- [ ] Validaciones en Business, no en Controller
- [ ] DTOs, no entidades directas
- [ ] Inyección de dependencias
- [ ] Manejo de excepciones
- [ ] Migraciones incluidas
- [ ] Sin cambios en BaseData/BaseBusiness
- [ ] Tests unitarios (si aplica)

---

**Última actualización:** 29 de octubre, 2025
