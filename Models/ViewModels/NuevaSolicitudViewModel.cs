using System.ComponentModel.DataAnnotations;

namespace PlataformaCreditos.Models.ViewModels;

public class NuevaSolicitudViewModel
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    [Display(Name = "Monto Solicitado")]
    public decimal MontoSolicitado { get; set; }

    public string? Mensaje { get; set; }
    public bool Exito { get; set; }
}