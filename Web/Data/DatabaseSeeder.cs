using ModelSecurityRepaso.Entity.Context;
using ModelSecurityRepaso.Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace ModelSecurityRepaso.Web.Data
{
    /// <summary>
    /// Clase para inicializar datos de prueba en la base de datos.
    /// Se ejecuta automáticamente cuando la aplicación inicia.
    /// </summary>
    public class DatabaseSeeder
    {
        /// <summary>
        /// Inicializa la base de datos con datos de prueba.
        /// </summary>
        public static void Initialize(ApplicationDbContext context)
        {
            // Asegúrate de que la BD esté creada
            context.Database.Migrate();

            // Si ya hay datos, no hagas nada
            if (context.Persons.Any())
            {
                return;
            }

            try
            {
                // 1. Crear Person (Persona)
                var person = new Person
                {
                    FirstName = "Admin",
                    LastName = "User",
                    SecondName = null,
                    LastSecondName = null,
                    TypeDocument = "CC",
                    NumberDocument = 123456789,
                    Phone = "3001234567",
                    Address = "Calle Principal 123",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Persons.Add(person);
                context.SaveChanges();

                // 2. Crear User (Usuario)
                var user = new User
                {
                    Name = "Admin User",
                    Email = "admin@example.com",
                    Password = "admin123",
                    PersonId = person.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Users.Add(user);
                context.SaveChanges();

                // 3. Crear Role (Rol)
                var adminRole = new Role
                {
                    Name = "Admin",
                    Description = "Administrador del sistema",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Roles.Add(adminRole);
                context.SaveChanges();

                var userRole = new Role
                {
                    Name = "User",
                    Description = "Usuario estándar",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Roles.Add(userRole);
                context.SaveChanges();

                // 4. Crear relación User-Role
                var userRoleRel = new UserRole
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.UserRoles.Add(userRoleRel);
                context.SaveChanges();

                // 5. Crear Permisos
                var permissions = new[]
                {
                    new Permission
                    {
                        Name = "Create",
                        Description = "Permiso para crear",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Permission
                    {
                        Name = "Read",
                        Description = "Permiso para leer",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Permission
                    {
                        Name = "Update",
                        Description = "Permiso para actualizar",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Permission
                    {
                        Name = "Delete",
                        Description = "Permiso para eliminar",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                context.Permissions.AddRange(permissions);
                context.SaveChanges();

                // 6. Crear Módulos
                var modules = new[]
                {
                    new Module
                    {
                        Name = "User Management",
                        Description = "Módulo de gestión de usuarios",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new Module
                    {
                        Name = "Security",
                        Description = "Módulo de seguridad",
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                context.Modules.AddRange(modules);
                context.SaveChanges();

                // 7. Crear Formularios
                var form = new Form
                {
                    Name = "Users Form",
                    Description = "Formulario de gestión de usuarios",
                    Url = "/users",
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.Forms.Add(form);
                context.SaveChanges();

                // 8. Asignar módulos a formularios
                var formModule = new FormModule
                {
                    Name = "Users Module",
                    ModuleId = modules[0].Id,
                    FormId = form.Id,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                context.FormModules.Add(formModule);
                context.SaveChanges();

                // 9. Asignar permisos a formularios por rol
                var roleFormPermissions = new[]
                {
                    new RoleFormPermission
                    {
                        RoleId = adminRole.Id,
                        FormId = form.Id,
                        PermissionId = permissions[0].Id, // Create
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoleFormPermission
                    {
                        RoleId = adminRole.Id,
                        FormId = form.Id,
                        PermissionId = permissions[1].Id, // Read
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoleFormPermission
                    {
                        RoleId = adminRole.Id,
                        FormId = form.Id,
                        PermissionId = permissions[2].Id, // Update
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    },
                    new RoleFormPermission
                    {
                        RoleId = adminRole.Id,
                        FormId = form.Id,
                        PermissionId = permissions[3].Id, // Delete
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow,
                        IsDeleted = false
                    }
                };
                context.RoleFormPermissions.AddRange(roleFormPermissions);
                context.SaveChanges();

                Console.WriteLine("✅ Base de datos inicializada con datos de prueba correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al inicializar la base de datos: {ex.Message}");
                throw;
            }
        }
    }
}
