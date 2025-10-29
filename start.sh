#!/bin/bash

# Script para iniciar la aplicación ModelSecurityRepaso

echo "🚀 Iniciando ModelSecurityRepaso..."
echo ""
echo "📝 Requisitos previos:"
echo "  ✓ .NET 9.0 SDK instalado"
echo "  ✓ MySQL ejecutándose en localhost:3306"
echo "  ✓ Base de datos 'ModelSecurityRepaso' creada"
echo ""

# Cambiar a directorio Web
cd Web

echo "🔨 Compilando el proyecto..."
dotnet build

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Compilación exitosa"
    echo ""
    echo "🌐 Iniciando servidor..."
    echo ""
    echo "📚 Swagger estará disponible en:"
    echo "   👉 https://localhost:7089/swagger"
    echo ""
    echo "🔐 Endpoints disponibles:"
    echo "   - POST /api/auth/login (sin autenticación)"
    echo "   - POST /api/auth/refresh (sin autenticación)"
    echo "   - POST /api/auth/revoke (sin autenticación)"
    echo "   - GET/POST/PUT/DELETE /api/user (requiere autenticación)"
    echo "   - GET/POST/PUT/DELETE /api/role (requiere autenticación)"
    echo "   - GET/POST/PUT/DELETE /api/form (requiere autenticación)"
    echo "   - GET/POST/PUT/DELETE /api/permission (requiere autenticación)"
    echo ""
    echo "⚠️  Presiona Ctrl+C para detener el servidor"
    echo ""
    
    dotnet run
else
    echo ""
    echo "❌ Error en la compilación"
    exit 1
fi
