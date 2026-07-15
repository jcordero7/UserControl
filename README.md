
---
### 2. Archivo README.md Genérico para **UserControl** (Backend / API de Identidad)

# 🔑 UserControl (Identity & Access Management API)

API REST centralizada de identidad, credenciales y gobernanza de usuarios diseñada para ecosistemas de aplicaciones distribuidas. Expone servicios de autenticación mediante tokens (JWT), registro de cuentas, flujos de recuperación de contraseñas, confirmación de identidad y control de acceso basado en roles por sistema o programa.

Es el backend transaccional consumido por el cliente de Single Sign-On [`Accounts`](#repositorio-relacionado) y actúa como el motor de identidad unificado para todos los servicios y microservicios de la organización.

---

## 🛠️ Arquitectura de Seguridad y SSO

Este servicio proporciona los pilares de seguridad del ecosistema:

* **Emisión de Tokens (JWT Bearer):** Genera y firma tokens de acceso de forma segura tras validar credenciales, permitiendo el consumo de recursos y APIs protegidas.
* **Soporte para Cookie Sharing:** Provee claims detallados de identidad y permisos que el cliente de autenticación (`Accounts`) encapsula en la cookie compartida del dominio raíz común.
* **Consistencia de Cifrado (Data Protection):** Diseñado para integrarse de manera homogénea con **ASP.NET Core Data Protection**, permitiendo que las llaves de seguridad se compartan y validen de manera idéntica entre múltiples plataformas clientes.

---

## Estructura de la Solución (Clean Architecture)

Organizado bajo una estructura desacoplada para garantizar que las reglas de negocio permanezcan aisladas de los detalles de infraestructura:

```text
UserControl/
├── UserControl.Api/              # Controllers, Startup, punto de entrada (ASP.NET Core Web API)
├── UserControl.Application/      # Casos de uso y servicios de aplicación
├── UserControl.Core/             # Entidades de dominio, interfaces y DTOs
├── UserControl.Infrastructure/   # EF Core (MySQL), repositorios de datos y validadores
├── UserControl.UnitTests/        # Pruebas unitarias
└── UserControl.IntegrationTests/ # Pruebas de integración distributivas
ASP.NET Core Web API desarrollado bajo .NET 8 (proyectos de pruebas en netcoreapp3.1).

Persistencia de datos principal con Entity Framework Core sobre motor MySQL (Pomelo).

Mecanismos de validación declarativa con FluentValidation y mapeo con AutoMapper.

Documentación interactiva de API con Swagger / OpenAPI expuesta en desarrollo.

Modelo de dominio (resumen)
User / Security: Gestión de credenciales, hash de seguridad y estados de cuenta.

Program: Entidad que representa a cada una de las aplicaciones satélite del ecosistema.

ProgramXUser / RoleXProgram: Matriz de permisos flexible que define qué rol tiene un mismo usuario dentro de cada sistema o programa independiente de la organización.

Historical: Auditoría completa e histórico de cambios sobre las cuentas.

Requisitos previos
.NET SDK 8.0

Instancia de base de datos MySQL (ver configuración de conexión en appsettings.json).

⚠️ Configuración de Seguridad para Despliegues
El archivo UserControl.Api/appsettings.json en este repositorio público utiliza placeholders estándar para proteger los secretos de desarrollo. Asegúrate de configurar tus valores reales utilizando variables de entorno o la herramienta de almacenamiento de secretos de .NET (dotnet user-secrets) antes de ejecutarlo en entornos locales o de producción.

No almacenes contraseñas de bases de datos, llaves privadas de JWT, ni credenciales de servidores de correo (SMTP) directamente en el control de versiones.

Configuración de Ejemplo (appsettings.json)
JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;port=3306;database=UserControl;User=<usuario>;Password=<password>;AllowUserVariables=True;"
  },
  "Authentication": {
    "SecretKey": "<tu-clave-secreta-personalizada-de-firma>",
    "Issuer": "https://localhost:44358",
    "Audience": "https://localhost:44358"
  },
  "EmailSenderOptions": {
    "Host": "smtp.dominio.com",
    "Port": 587,
    "EnableSsl": true,
    "Email": "<correo-remitente>",
    "Password": "<app-password>"
  }
}
Cómo ejecutar
Descarga las dependencias:

Bash
dotnet restore
Ejecuta las migraciones de base de datos:

Bash
dotnet ef database update --project UserControl.Infrastructure --startup-project UserControl.Api
Inicia el servidor de desarrollo:

Bash
dotnet run --project UserControl.Api
La API quedará disponible en https://localhost:44358, con Swagger UI mapeado en la raíz (/) para pruebas en modo desarrollo.

Endpoints principales
Método	Ruta	Descripción
POST	/api/User	Registro de nuevo usuario (retorna JWT)
POST	/api/User/login	Autenticación y login mediante credenciales tradicionales
GET	/api/User/Get/{id}	Recupera perfil de un usuario (requiere JWT)
PUT	/api/User	Actualización de perfil (requiere JWT)
POST	/api/User/recoverpassword	Inicio de flujo para recuperación de contraseña
POST	/api/User/newpassword	Aplicación de nueva contraseña de recuperación
POST	/api/User/changepassword	Cambio de contraseña interno de usuario (requiere JWT)
POST	/api/User/enableaccount	Activación y verificación de cuenta
Notas conocidas / deuda técnica
Los proyectos de testing (UserControl.UnitTests y IntegrationTests) apuntan a netcoreapp3.1. Se sugiere unificar toda la solución a la versión de soporte a largo plazo (net8.0).

Estructuras secundarias remanentes de plantillas iniciales están listas para ser depuradas en próximas iteraciones de refactorización.

Repositorio relacionado
Accounts — Portal web MVC de administración de cuentas y cliente del Single Sign-On (SSO).
