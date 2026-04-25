using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity; // ← agrega este using

namespace PlataformaCreditos.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required]
    public string UsuarioId { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Los ingresos deben ser mayores a 0")]
    public decimal IngresosMensuales { get; set; }

    public bool Activo { get; set; } = true;

    // Navegación
    public IdentityUser? Usuario { get; set; } // ← cambia ApplicationUser por IdentityUser
    public ICollection<SolicitudCredito> Solicitudes { get; set; } = new List<SolicitudCredito>();
}