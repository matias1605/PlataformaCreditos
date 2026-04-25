namespace PlataformaCreditos.Models.ViewModels;
using PlataformaCreditos.Models;

public class SolicitudFiltroViewModel
{
    public List<SolicitudCredito> Solicitudes { get; set; } = new();
    public string? Estado { get; set; }
    public decimal? MontoMin { get; set; }
    public decimal? MontoMax { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public string? ErrorFecha { get; set; }
}