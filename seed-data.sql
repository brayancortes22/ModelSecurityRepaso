-- Insertar datos de prueba en ModelSecurity

-- 1. Insertar un Person (Persona)
INSERT INTO Persons (Id, FirstName, LastName, TypeDocument, NumberDocument, Phone, Address, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 'Admin', 'User', 'CC', 123456789, '3001234567', 'Calle Principal 123', NOW(), NOW(), 0);

-- 2. Insertar un User (Usuario)
INSERT INTO Users (Id, Name, Email, Password, PersonId, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 'admin', 'admin@example.com', 'admin123', 1, NOW(), NOW(), 0);

-- 3. Insertar un Role (Rol)
INSERT INTO Roles (Id, Name, Description, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 'Admin', 'Administrador del sistema', NOW(), NOW(), 0);

-- 4. Crear relación User-Role
INSERT INTO UserRoles (Id, UserId, RoleId, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 1, 1, NOW(), NOW(), 0);

-- 5. Insertar Permisos
INSERT INTO Permissions (Id, Name, Description, CreatedAt, UpdatedAt, IsDeleted)
VALUES 
(1, 'Create', 'Permiso para crear', NOW(), NOW(), 0),
(2, 'Read', 'Permiso para leer', NOW(), NOW(), 0),
(3, 'Update', 'Permiso para actualizar', NOW(), NOW(), 0),
(4, 'Delete', 'Permiso para eliminar', NOW(), NOW(), 0);

-- 6. Insertar Formularios
INSERT INTO Forms (Id, Name, Description, Url, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 'Users Form', 'Formulario de gestión de usuarios', '/users', NOW(), NOW(), 0);

-- 7. Insertar Módulos
INSERT INTO Modules (Id, Name, Description, CreatedAt, UpdatedAt, IsDeleted)
VALUES 
(1, 'User Management', 'Módulo de gestión de usuarios', NOW(), NOW(), 0),
(2, 'Security', 'Módulo de seguridad', NOW(), NOW(), 0);

-- 8. Asignar permisos a formularios por rol
INSERT INTO RoleFormPermissions (Id, RoleId, FormId, PermissionId, CreatedAt, UpdatedAt, IsDeleted)
VALUES 
(1, 1, 1, 1, NOW(), NOW(), 0),
(2, 1, 1, 2, NOW(), NOW(), 0),
(3, 1, 1, 3, NOW(), NOW(), 0),
(4, 1, 1, 4, NOW(), NOW(), 0);

-- 9. Asignar módulos a formularios
INSERT INTO FormModules (Id, ModuleId, FormId, CreatedAt, UpdatedAt, IsDeleted)
VALUES (1, 1, 1, NOW(), NOW(), 0);

SELECT 'Datos de prueba insertados correctamente' AS Resultado;
