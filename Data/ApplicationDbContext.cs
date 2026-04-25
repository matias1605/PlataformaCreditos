using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<SolicitudCredito> SolicitudesCredito { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Cliente>()
            .Property(c => c.IngresosMensuales)
            .HasPrecision(18, 2);

        builder.Entity<SolicitudCredito>()
            .Property(s => s.MontoSolicitado)
            .HasPrecision(18, 2);

        // Un cliente -> muchas solicitudes
        builder.Entity<Cliente>()
            .HasMany(c => c.Solicitudes)
            .WithOne(s => s.Cliente)
            .HasForeignKey(s => s.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}