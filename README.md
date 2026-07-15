UserControl
🔑 Servidor de Autenticación Centralizado en .NET Core. Emite cookies de autenticación cifradas (Data Protection) y tokens para habilitar Single Sign-On (SSO) entre múltiples aplicaciones ligadas de forma segura.

2. Archivo README.md para el proyecto UserControl
# 🔑 UserControl (Buscadme)

API REST central de identidad, credenciales y gestión de usuarios del ecosistema **Buscadme**. Expone autenticación (JWT), registro, recuperación/cambio de contraseña, confirmación de cuenta y gestión de roles por sistema ("Programa"). 

Es consumida directamente por la aplicación cliente de Single Sign-On [`Accounts`](#repositorio-relacionado) y actúa como el motor transaccional de identidad que respalda a todos los demás sistemas de Buscadme (Eventos, Naturopatía, etc.).

---

## 🛠️ Arquitectura de Seguridad y SSO

Este servicio proporciona los bloques fundamentales para el mecanismo de Single Sign-On (SSO) del ecosistema:

* **Emisión de Tokens (JWT Bearer):** Genera y firma los tokens de acceso tras una validación exitosa de credenciales, permitiendo el consumo seguro de endpoints protegidos.
* **Soporte para Cookie Sharing:** Provee la información de identidad y los Claims que el cliente `Accounts` encapsulará en la cookie compartida de dominio `.buscadme.org`.
* **Seguridad de Datos:** Diseñado para integrarse con **ASP.NET Core Data Protection**, lo que permite que el cifrado y descifrado de la información de sesión de los usuarios sea consistente a lo largo de todas las aplicaciones cliente ligadas.

---

## Estructura de la Solución (Clean Architecture)

Organizado por capas independientes para desacoplar la lógica de dominio de la infraestructura:

```text
UserControl/
├── UserControl.Api/              # Controllers, Startup, punto de entrada (ASP.NET Core Web API)
├── UserControl.Application/      # Casos de uso / servicios de aplicación
├── UserControl.Core/             # Entidades de dominio, interfaces, DTOs
├── UserControl.Infrastructure/   # EF Core (MySQL), repositorios, Mongo (posts), validadores
├── UserControl.UnitTests/        # Pruebas unitarias
└── UserControl.IntegrationTests/ # Pruebas de integración
ASP.NET Core Web API (.NET 8; los proyectos de test aún apuntan a netcoreapp3.1).Entity Framework Core + MySQL (Pomelo) como base de datos principal.Autenticación JWT Bearer.AutoMapper, FluentValidation.Swagger / OpenAPI (disponible en la raíz / en desarrollo).Repositorio Mongo para un módulo de publicaciones (Post / Commentary), heredado de la plantilla base ("SocialMedia") del que se partió — no está conectado al flujo de cuentas actual.Modelo de dominio (resumen)User / Security: Credenciales, hashes de contraseña y estado de la cuenta.Program: Cada sistema satélite del ecosistema Buscadme (Eventos, Naturopatía, etc.).ProgramXUser / RoleXProgram: Qué rol/permisos tiene un usuario dentro de cada sistema específico (permite que un mismo usuario tenga distintos roles en distintos programas).Historical: Auditoría e histórico de cambios de cuenta.Post / Commentary: Módulo social heredado, sin relación con el dominio de cuentas.Requisitos previos.NET SDK 8.0MySQL Server accesible localmente (ver cadena de conexión en appsettings.json).⚠️ Antes de subir este repositorio a GitHub (Público)UserControl.Api/appsettings.json tiene credenciales reales committeadas: usuario y contraseña root de MySQL, la SecretKey de firma de los JWT, y la contraseña de la cuenta de correo infobuscadme@gmail.com usada para el envío de emails. Si el repositorio se sube público, estas credenciales quedarán expuestas de inmediato.Antes de hacer git push:Rota las credenciales: Cambia la contraseña root de tu MySQL local, genera una nueva SecretKey para firmar tus JWT, y genera un App Password en tu cuenta de Gmail.Saca los valores reales de appsettings.json y muévelos a variables de entorno o usa dotnet user-secrets en desarrollo. Deja en el repositorio únicamente placeholders (valores genéricos de ejemplo).Asegúrate de añadir appsettings.Development.json a tu archivo .gitignore si planeas mantener allí tus credenciales locales reales.Configuración de Ejemplo (appsettings.json)Los valores reales deben venir de variables de entorno o user-secrets, no quedar registrados en el historial de git:JSON{
  "ConnectionStrings": {
    "SocialMedia": "Server=localhost;port=3306;database=UserControl;User=<usuario>;Password=<password>;AllowUserVariables=True;"
  },
  "Authentication": {
    "SecretKey": "<clave-secreta-suficientemente-larga>",
    "Issuer": "https://localhost:44358",
    "Audience": "https://localhost:44358"
  },
  "EmailSenderOptions": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": true,
    "Email": "<correo>",
    "Password": "<app-password>"
  }
}
Cómo ejecutarBashdotnet restore
dotnet ef database update --project UserControl.Infrastructure --startup-project UserControl.Api
dotnet run --project UserControl.Api
La API queda disponible en https://localhost:44358 (perfil IIS Express), con Swagger UI en la raíz (/) en modo desarrollo.

Endpoints principales

Método	Ruta	Descripción
POST	/api/User	Registrar usuario, devuelve JWT
POST	/api/User/login	Iniciar sesión por credenciales
GET	/api/User/Get/{id}	Obtener usuario (requiere JWT)
PUT	/api/User	Editar usuario (requiere JWT)
POST	/api/User/recoverpassword	Solicitar recuperación de contraseña
POST	/api/User/newpassword	Establecer nueva contraseña
POST	/api/User/changepassword	Cambiar contraseña (requiere JWT)
POST	/api/User/enableaccount | /confirmAccount	Activar cuenta con código
GET	/api/User/RequestToken/{email}	Solicitar/reenviar código de activación
POST	/api/Security	Registro a nivel de credenciales

Notas conocidas / deuda técnica
UserControl.UnitTests e UserControl.IntegrationTests siguen en netcoreapp3.1 (fuera de soporte); el resto de la solución ya está en net8.0.

Existe un archivo huérfano UserControl.Infrastructure/SocialMedia - Backup.Infrastructure.csproj, no referenciado por la solución — candidato a limpieza.

El módulo Post / Commentary no tiene relación con el dominio de cuentas actual; evaluar si se mantiene o se retira de la solución definitiva.

Repositorio relacionado
Accounts — aplicación web (MVC) que funciona como cliente SSO y consume esta API para autenticar y gestionar perfiles.

