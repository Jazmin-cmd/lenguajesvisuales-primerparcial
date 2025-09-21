<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Cooperativa API - Examen</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f5f7fa;
            color: #333;
            line-height: 1.6;
            margin: 0;
            padding: 0;
        }
        header {
            background-color: #0078d7;
            color: #fff;
            padding: 2rem;
            text-align: center;
        }
        header h1 {
            margin: 0;
            font-size: 2rem;
        }
        main {
            max-width: 900px;
            margin: 2rem auto;
            padding: 0 2rem;
        }
        section {
            background-color: #fff;
            padding: 1.5rem 2rem;
            margin-bottom: 2rem;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }
        h2 {
            color: #0078d7;
            margin-top: 0;
        }
        pre {
            background-color: #f0f0f0;
            padding: 1rem;
            border-radius: 6px;
            overflow-x: auto;
        }
        code {
            font-family: Consolas, monospace;
            color: #d6336c;
        }
        ul {
            padding-left: 1.5rem;
        }
        footer {
            text-align: center;
            padding: 1rem;
            color: #777;
            font-size: 0.9rem;
        }
    </style>
</head>
<body>

<header>
    <h1>Cooperativa API - Examen</h1>
    <p>Desarrollo de API REST en C# con ASP.NET Core 8 Web API</p>
</header>

<main>
    <section>
        <h2>Descripción del Proyecto</h2>
        <p>
            Este proyecto implementa una <strong>API REST</strong> para la gestión de una cooperativa. Permite manejar usuarios, socios, préstamos y aportaciones. 
            Incluye autenticación JWT, roles de usuario (Admin y Socio), validaciones de entrada y operaciones CRUD.
        </p>
    </section>

    <section>
        <h2>Instalación y Ejecución</h2>
        <ul>
            <li>Clonar el repositorio:
                <pre><code>git clone https://github.com/Jazmin-cmd/lenguajesvisuales-primerparcial.git</code></pre>
            </li>
            <li>Entrar a la carpeta del proyecto:
                <pre><code>cd CooperativaApi</code></pre>
            </li>
            <li>Restaurar paquetes NuGet:
                <pre><code>dotnet restore</code></pre>
            </li>
            <li>Ejecutar la API:
                <pre><code>dotnet run</code></pre>
            </li>
            <li>Probar los endpoints con Postman o cualquier cliente HTTP.</li>
        </ul>
    </section>

    <section>
        <h2>Datos de Prueba</h2>
        <p>Usuarios para login:</p>
        <ul>
            <li><strong>Admin</strong>: 
                <pre><code>Email: admin@cooperativa.com
Password: Admin123!</code></pre>
            </li>
            <li><strong>Socio</strong>: 
                <pre><code>Email: socio1@cooperativa.com
Password: Socio123!</code></pre>
            </li>
        </ul>
    </section>

    <section>
        <h2>Notas Técnicas</h2>
        <ul>
            <li>Autenticación mediante JWT.</li>
            <li>Roles de usuario controlados con <code>[Authorize(Roles="...")]</code>.</li>
            <li>Validaciones de datos en DTOs con <code>[Required]</code> y <code>[Range]</code>.</li>
            <li>Manejo de excepciones centralizado con <code>ErrorHandlingMiddleware</code>.</li>
            <li>Endpoints paginados y filtrables.</li>
        </ul>
    </section>
</main>

<footer>
    <p>&copy; 2025 Cooperativa API - Examen de C# ASP.NET Core</p>
</footer>

</body>
</html>
