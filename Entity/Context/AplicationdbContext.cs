using ModelSecurityRepaso.Entity.Model;
using ModelSecurityRepaso.Entity.Model.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using ModuleModel = ModelSecurityRepaso.Entity.Model.Module;

namespace ModelSecurityRepaso.Entity.Context
{
    /// <summary>
    /// Representa el contexto de la base de datos de la aplicación, proporcionando configuraciones y métodos
    /// para la gestión de entidades y consultas personalizadas con Dapper.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Configuración de la aplicación.
        /// por ejemplo, para obtener cadenas de conexión.
        /// </summary>
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor del contexto de la base de datos.
        /// </summary>
        /// <param name="options">Opciones de configuración para el contexto de base de datos.</param>
        /// <param name="configuration">Instancia de IConfiguration para acceder a la configuración de la aplicación.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Conjuntos de entidades que representan tablas en la base de datos.
        /// </summary>
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormModule> FormModules { get; set; }
        public DbSet<ModuleModel> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RoleFormPermission> RoleFormPermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // Propiedades alias para acceso singular (compatibilidad con código existente)
        public DbSet<User> User => Users;
        public DbSet<Role> Role => Roles;
        public DbSet<UserRole> UserRole => UserRoles;
        public DbSet<RefreshToken> RefreshToken => RefreshTokens;

        /// <summary>
        /// Configura los modelos de la base de datos con todas las relaciones.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== RELACIÓN: Person (1) ---> (1) User ==========
            modelBuilder.Entity<Person>()
                .HasOne(p => p.User)
                .WithOne(u => u.Person)
                .HasForeignKey<User>(u => u.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: User (1) ---> (N) UserRole ==========
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Role (1) ---> (N) UserRole ==========
            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Form (1) ---> (N) FormModule ==========
            modelBuilder.Entity<Form>()
                .HasMany(f => f.FormModules)
                .WithOne(fm => fm.Form)
                .HasForeignKey(fm => fm.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Module (1) ---> (N) FormModule ==========
            modelBuilder.Entity<ModuleModel>()
                .HasMany(m => m.FormModules)
                .WithOne(fm => fm.Module)
                .HasForeignKey(fm => fm.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Role (1) ---> (N) RoleFormPermission ==========
            modelBuilder.Entity<Role>()
                .HasMany(r => r.RoleFormPermissions)
                .WithOne(rfp => rfp.Role)
                .HasForeignKey(rfp => rfp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Form (1) ---> (N) RoleFormPermission ==========
            modelBuilder.Entity<Form>()
                .HasMany(f => f.RoleFormPermissions)
                .WithOne(rfp => rfp.Form)
                .HasForeignKey(rfp => rfp.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: Permission (1) ---> (N) RoleFormPermission ==========
            modelBuilder.Entity<Permission>()
                .HasMany(p => p.RoleFormPermissions)
                .WithOne(rfp => rfp.Permission)
                .HasForeignKey(rfp => rfp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== RELACIÓN: User (1) ---> (N) RefreshToken ==========
            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== CONFIGURACIÓN DE ÍNDICES ÚNICOS ==========
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.NumberDocument)
                .IsUnique();

            // ========== CONFIGURACIÓN DE CONSTRAINTS ==========
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Form>()
                .Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<ModuleModel>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Permission>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// solo para desarrollo no para produccion.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Otras configuraciones adicionales pueden ir aquí
            // por ejemplo optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            // que sirve para conectar la base de datos con la cadena de conexion.
            // pero en este caso se esta manejando desde el startup.cs o program.cs que seria lo recomendado.
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Establece la precisión predeterminada para las propiedades de tipo decimal en todas las entidades.
            // por ejemplo si tengo un precio en decimal se guarda con 18 digitos y 2 decimales.
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        /// <summary>
        /// Guarda los cambios en la base de datos, asegurando la auditoría antes de persistir los datos.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guarda los cambios en la base de datos de manera asíncrona, asegurando la auditoría antes de la persistencia.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indica si se deben aceptar todos los cambios en caso de éxito.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación.</param>
        /// <returns>Número de filas afectadas de forma asíncrona.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// Registra automáticamente CreatedAt, ModifiedAt e IsDeleted.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries<BaseModel>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Deleted:
                        // Usar delete lógico en lugar de físico
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.ModifiedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}