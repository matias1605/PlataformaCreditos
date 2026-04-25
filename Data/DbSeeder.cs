using Microsoft.AspNetCore.Identity;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.EnsureCreated();

        // Crear rol Analista
        if (!await roleManager.RoleExistsAsync("Analista"))
            await roleManager.CreateAsync(new IdentityRole("Analista"));

        // Crear usuario analista
        if (await userManager.FindByEmailAsync("analista@creditos.com") == null)
        {
            var analista = new IdentityUser { UserName = "analista@creditos.com", Email = "analista@creditos.com", EmailConfirmed = true };
            await userManager.CreateAsync(analista, "Analista123!");
            await userManager.AddToRoleAsync(analista, "Analista");
        }

        // Crear usuario cliente1
        IdentityUser? user1 = await userManager.FindByEmailAsync("cliente1@creditos.com");
        if (user1 == null)
        {
            user1 = new IdentityUser { UserName = "cliente1@creditos.com", Email = "cliente1@creditos.com", EmailConfirmed = true };
            await userManager.CreateAsync(user1, "Cliente123!");
        }

        // Crear usuario cliente2
        IdentityUser? user2 = await userManager.FindByEmailAsync("cliente2@creditos.com");
        if (user2 == null)
        {
            user2 = new IdentityUser { UserName = "cliente2@creditos.com", Email = "cliente2@creditos.com", EmailConfirmed = true };
            await userManager.CreateAsync(user2, "Cliente123!");
        }

        if (!context.Clientes.Any())
        {
            var c1 = new Cliente { UsuarioId = user1!.Id, IngresosMensuales = 3000, Activo = true };
            var c2 = new Cliente { UsuarioId = user2!.Id, IngresosMensuales = 5000, Activo = true };
            context.Clientes.AddRange(c1, c2);
            await context.SaveChangesAsync();

            context.SolicitudesCredito.AddRange(
                new SolicitudCredito { ClienteId = c1.Id, MontoSolicitado = 5000, Estado = EstadoSolicitud.Pendiente },
                new SolicitudCredito { ClienteId = c2.Id, MontoSolicitado = 8000, Estado = EstadoSolicitud.Aprobado }
            );
            await context.SaveChangesAsync();
        }
    }
}