using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using CooperativaApi.Data;
using CooperativaApi.Middleware;
using CooperativaApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Servicios
// -------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

// Password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// JWT Authentication
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new Exception("JWT key missing"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
        // Esto le indica a ASP.NET qué claim debe usar como rol
        RoleClaimType = ClaimTypes.Role
    };
    // DEBUG de errores de token
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine($"Auth failed: {ctx.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine("Token validado correctamente");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();

var app = builder.Build();

// -------------------------
// Middleware
// -------------------------
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// -------------------------
// Seed de datos modular
// -------------------------
await SeedDataAsync(app);

// -------------------------
// Endpoint temporal para verificar seed
// -------------------------
app.MapGet("/api/test-seed", async (AppDbContext db) =>
{
    var profesiones = await db.Profesiones.ToListAsync();
    var usuarios = await db.Users.Select(u => new { u.Nombre, u.Email, u.Rol }).ToListAsync();
    var socios = await db.Socios.ToListAsync();

    return Results.Ok(new { Profesiones = profesiones, Usuarios = usuarios, Socios = socios });
});

app.Run();


// -------------------------
// Función para seed
// -------------------------
static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    // Aplicar migraciones
    await db.Database.MigrateAsync();

    try
    {
        // Profesiones
        if (!db.Profesiones.Any())
        {
            db.Profesiones.AddRange(
                new Profesion { Nombre = "Ingeniero" },
                new Profesion { Nombre = "Medico" },
                new Profesion { Nombre = "Docente" },
                new Profesion { Nombre = "Carpintero" }
            );
            await db.SaveChangesAsync();
        }

        // Usuarios
        if (!db.Users.Any())
        {
            var admin = new User
            {
                Nombre = "Admin",
                Email = "admin@coop.com",
                Rol = "Admin"
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
            db.Users.Add(admin);

            var socioUser = new User
            {
                Nombre = "SocioPrueba",
                Email = "socio@coop.com",
                Rol = "Socio"
            };
            socioUser.PasswordHash = hasher.HashPassword(socioUser, "Socio123!");
            db.Users.Add(socioUser);

            await db.SaveChangesAsync();

            // Socio vinculado al usuario
            if (!db.Socios.Any())
            {
                var profesion = db.Profesiones.First();
                db.Socios.Add(new Socio
                {
                    NombreCompleto = "Juan Perez",
                    CI = "1234567",
                    Direccion = "Calle Falsa 123",
                    Telefono = "098112233",
                    ProfesionId = profesion.Id,
                    UserId = socioUser.Id
                });
                await db.SaveChangesAsync();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error durante el seed: {ex.Message}");
        throw;
    }
}
