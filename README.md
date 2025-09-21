# 👋 Cooperativa API

🎓 **Proyecto académico:** Desarrollo de API REST en C# con ASP.NET Core 8  
💻 **Objetivo:** Gestionar socios, usuarios, profesiones, préstamos y aportaciones de manera segura y eficiente  
🚀 **Características principales:** JWT para autenticación, roles (Admin/Socio), validaciones y CRUD completo  

---

## 📝 Descripción del proyecto

Cooperativa API es una **API REST** que permite administrar las operaciones principales de una cooperativa:

- Al **crear un socio**, automáticamente se genera un **usuario** asociado.  
- Con este usuario podemos:
  - Solicitar **préstamos**  
  - Consultar nuestras **aportaciones**  
  - Ver nuestra **profesión**  
- Los métodos **POST, PUT y DELETE** están protegidos mediante **token JWT**, que se valida en cada request:  
  - Cada request debe incluir el **header** con el token  
  - El token contiene información del usuario, incluyendo **Id y Rol**  
  - El servidor verifica que el token sea válido, no haya expirado y contenga los claims requeridos (por ejemplo, rol Admin o Socio)  

> En resumen, ningún cliente puede modificar datos sin un JWT válido, y solo ciertos roles pueden ejecutar acciones específicas.  

La API implementa las funcionalidades básicas necesarias y cumple con los requerimientos mínimos para gestionar socios, usuarios, profesiones, préstamos y aportaciones, con seguridad y manejo de roles. Sin embargo, aún puede mejorarse y ampliarse para ser más robusta y escalable.

---

## ⚙️ Tecnologías utilizadas

<p align="left">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white"/>
  <img src="https://img.shields.io/badge/ASP.NET Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
  <img src="https://img.shields.io/badge/EntityFramework-339933?style=for-the-badge&logo=entity-framework&logoColor=white"/>
  <img src="https://img.shields.io/badge/SQL Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white"/>
  <img src="https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white"/>
  <img src="https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSONwebtokens&logoColor=white"/>
</p>

---
🧪 Datos de prueba

Usuario  | Rol   | Email           | Contraseña
---------|-------|----------------|-----------
Admin    | Admin | admin@coop.com  | Admin123
Socio    | Socio | socio@coop.com  | Socio123


## 🚀 Instalación y ejecución

1. Clonar el repositorio:

```bash
git clone https://github.com/Jazmin-cmd/lenguajesvisuales-primerparcial.git
cd CooperativaApi
