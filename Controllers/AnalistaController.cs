using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Data;
using PlataformaCreditos.Models;

namespace PlataformaCreditos.Controllers;

[Authorize(Roles = "Analista")]
public class AnalistaController : Controller
{
    private readonly ApplicationDbContext _context;

    public AnalistaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Analista
    public async Task<IActionResult> Index()
    {
        var pendientes = await _context.SolicitudesCredito
            .Include(s => s.Cliente)
            .ThenInclude(c => c!.Usuario)
            .Where(s => s.Estado == EstadoSolicitud.Pendiente)
            .OrderBy(s => s.FechaSolicitud)
            .ToListAsync();

        return View(pendientes);
    }

    // POST: /Analista/Aprobar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(int id)
    {
        var solicitud = await _context.SolicitudesCredito
            .Include(s => s.Cliente)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (solicitud == null)
            return NotFound();

        if (solicitud.Estado != EstadoSolicitud.Pendiente)
        {
            TempData["Error"] = "Solo se pueden aprobar solicitudes en estado Pendiente.";
            return RedirectToAction(nameof(Index));
        }

        // Validación: monto no puede exceder 5 veces los ingresos
        if (solicitud.MontoSolicitado > solicitud.Cliente!.IngresosMensuales * 5)
        {
            TempData["Error"] = $"No se puede aprobar: el monto (S/ {solicitud.MontoSolicitado:N2}) excede 5 veces los ingresos del cliente (máx: S/ {solicitud.Cliente.IngresosMensuales * 5:N2}).";
            return RedirectToAction(nameof(Index));
        }

        solicitud.Estado = EstadoSolicitud.Aprobado;
        await _context.SaveChangesAsync();

        TempData["Exito"] = $"Solicitud #{id} aprobada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Analista/Rechazar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(int id, string motivoRechazo)
    {
        if (string.IsNullOrWhiteSpace(motivoRechazo))
        {
            TempData["Error"] = "El motivo de rechazo es obligatorio.";
            return RedirectToAction(nameof(Index));
        }

        var solicitud = await _context.SolicitudesCredito
            .FirstOrDefaultAsync(s => s.Id == id);

        if (solicitud == null)
            return NotFound();

        if (solicitud.Estado != EstadoSolicitud.Pendiente)
        {
            TempData["Error"] = "Solo se pueden rechazar solicitudes en estado Pendiente.";
            return RedirectToAction(nameof(Index));
        }

        solicitud.Estado = EstadoSolicitud.Rechazado;
        solicitud.MotivoRechazo = motivoRechazo;
        await _context.SaveChangesAsync();

        TempData["Exito"] = $"Solicitud #{id} rechazada.";
        return RedirectToAction(nameof(Index));
    }
}