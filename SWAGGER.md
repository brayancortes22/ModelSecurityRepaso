# Swagger / OpenAPI Documentation

## ¿Qué es Swagger?

Swagger es una herramienta que genera documentación interactiva de APIs REST. Permite probar los endpoints directamente desde el navegador sin necesidad de herramientas externas como Postman.

## Acceder a Swagger

### En Desarrollo
- **URL**: `https://localhost:7089/swagger` (el puerto puede variar según tu configuración)
- **Alternativamente**: `https://localhost:7089/swagger/index.html`

### En Producción
Swagger está deshabilitado en producción por razones de seguridad. Solo está disponible cuando `app.Environment.IsDevelopment()` retorna `true`.

## Características de Swagger

### 1. Exploración de Endpoints
- Todos los controladores y sus métodos aparecen organizados
- Se muestra automáticamente la ruta, el método HTTP (GET, POST, PUT, DELETE) y la descripción

### 2. Autenticación JWT
- Click en el botón **"Authorize"** (esquina superior derecha)
- Pegá tu token JWT en el formato: `Bearer eyJhbGc...`
- Todos los endpoints que requieran autenticación usarán automáticamente este token

### 3. Probar Endpoints
1. Selecciona un endpoint
2. Click en **"Try it out"**
3. Rellena los parámetros necesarios
4. Click en **"Execute"**
5. Verás la respuesta, headers y status code

### 4. Esquemas de Modelos
Al final de la página están todos los modelos de datos (DTOs, Requests, Responses) con sus propiedades y tipos.

## Endpoints Disponibles

### AuthController (`/api/auth`)
- **POST /api/auth/login** - Autentica un usuario y retorna tokens
- **POST /api/auth/refresh** - Renueva un token JWT expirado
- **POST /api/auth/revoke** - Revoca un refresh token

### UserController (`/api/user`)
- **GET /api/user** - Lista todos los usuarios
- **GET /api/user/{id}** - Obtiene un usuario por ID
- **POST /api/user** - Crea un nuevo usuario (requiere rol Admin)
- **PUT /api/user/{id}** - Actualiza un usuario (requiere rol Admin)
- **DELETE /api/user/{id}** - Elimina un usuario (requiere rol Admin)

### RoleController (`/api/role`)
- **GET /api/role** - Lista todos los roles
- **GET /api/role/{id}** - Obtiene un rol por ID
- **POST /api/role** - Crea un nuevo rol (requiere rol Admin)
- **PUT /api/role/{id}** - Actualiza un rol (requiere rol Admin)
- **DELETE /api/role/{id}** - Elimina un rol (requiere rol Admin)

### FormController (`/api/form`)
- **GET /api/form** - Lista todos los formularios
- **GET /api/form/{id}** - Obtiene un formulario por ID
- **POST /api/form** - Crea un nuevo formulario (requiere rol Admin)
- **PUT /api/form/{id}** - Actualiza un formulario (requiere rol Admin)
- **DELETE /api/form/{id}** - Elimina un formulario (requiere rol Admin)

### PermissionController (`/api/permission`)
- **GET /api/permission** - Lista todos los permisos
- **GET /api/permission/{id}** - Obtiene un permiso por ID
- **POST /api/permission** - Crea un nuevo permiso (requiere rol Admin)
- **PUT /api/permission/{id}** - Actualiza un permiso (requiere rol Admin)
- **DELETE /api/permission/{id}** - Elimina un permiso (requiere rol Admin)

## Ejemplo de Flujo de Uso

### 1. Login
```json
POST /api/auth/login
{
  "email": "user@example.com",
  "password": "123456"
}

Respuesta:
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-encoded-random-token",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

### 2. Autorizar en Swagger
- Copia el valor de `accessToken`
- Click en **"Authorize"**
- Pega: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- Click en **"Authorize"**

### 3. Usar Endpoints Protegidos
Todos los endpoints que requieren autenticación usarán automáticamente el token

### 4. Refrescar Token (cuando expire)
```json
POST /api/auth/refresh
{
  "refreshToken": "base64-encoded-random-token"
}
```

## Códigos de Respuesta HTTP

- **200 OK** - Solicitud exitosa
- **201 Created** - Recurso creado exitosamente
- **204 No Content** - Solicitud exitosa sin contenido (DELETE)
- **400 Bad Request** - Solicitud inválida o parámetros faltantes
- **401 Unauthorized** - No autenticado o token expirado
- **403 Forbidden** - Autenticado pero sin permisos para acceder
- **404 Not Found** - Recurso no encontrado
- **500 Internal Server Error** - Error del servidor

## Configuración en Program.cs

La configuración de Swagger está en `Web/Program.cs`:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    // Información de la API
    options.SwaggerDoc("v1", new OpenApiInfo { ... });
    
    // Configuración de autenticación JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { ... });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { ... });
});

// En el pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { ... });
}
```

## Solución de Problemas

### No veo los endpoints
- Verifica que el proyecto está corriendo: `dotnet run`
- Asegúrate que estás en `https://localhost:7089/swagger`
- Recarga la página

### Autenticación no funciona
- Verifica que el token es válido (no expirado)
- Usa el formato correcto: `Bearer {token}`
- Sin el prefijo `Bearer` no funcionará

### Algunos endpoints no aparecen
- Verifica que los controladores tienen el atributo `[ApiController]`
- Verifica que los métodos tienen atributos `[HttpGet]`, `[HttpPost]`, etc.

## Próximos Pasos

- Documentar todos los controladores con comentarios XML
- Agregar ejemplos en Swagger
- Habilitar descarga de especificación OpenAPI (swagger.json)
