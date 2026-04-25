using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCreditos.Data;
using PlataformaCreditos.Models;
using PlataformaCreditos.Models.ViewModels;

namespace PlataformaCreditos.Controllers;

[Authorize]
public class SolicitudesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SolicitudesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Solicitudes/MisSolicitudes
    public async Task<IActionResult> MisSolicitudes(
        string? estado, decimal? montoMin, decimal? montoMax,
        DateTime? fechaDesde, DateTime? fechaHasta)
    {
        var userId = _userManager.GetUserId(User);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);

        var vm = new SolicitudFiltroViewModel
        {
            Estado = estado, MontoMin = montoMin, MontoMax = montoMax,
            FechaDesde = fechaDesde, FechaHasta = fechaHasta
        };

        // Validación server-side: rango de fechas inválido
        if (fechaDesde.HasValue && fechaHasta.HasValue && fechaDesde > fechaHasta)
        {
            vm.ErrorFecha = "La fecha de inicio no puede ser mayor a la fecha fin.";
            return View(vm);
        }

        if (cliente == null)
        {
            vm.Solicitudes = new List<SolicitudCredito>();
            return View(vm);
        }

        var query = _context.SolicitudesCredito
            .Where(s => s.ClienteId == cliente.Id)
            .AsQueryable();

        if (!string.IsNullOrEmpty(estado) && Enum.TryParse<EstadoSolicitud>(estado, out var estadoEnum))
            query = query.Where(s => s.Estado == estadoEnum);

        if (montoMin.HasValue)
            query = query.Where(s => s.MontoSolicitado >= montoMin.Value);

        if (montoMax.HasValue)
            query = query.Where(s => s.MontoSolicitado <= montoMax.Value);

        if (fechaDesde.HasValue)
            query = query.Where(s => s.FechaSolicitud >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            query = query.Where(s => s.FechaSolicitud <= fechaHasta.Value);

        vm.Solicitudes = await query.OrderByDescending(s => s.FechaSolicitud).ToListAsync();
        return View(vm);
    }

    // GET: Solicitudes/Detalle/5
    public async Task<IActionResult> Detalle(int id)
    {
        var userId = _userManager.GetUserId(User);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UsuarioId == userId);
        if (cliente == null) return Forbid();

        var solicitud = await _context.SolicitudesCredito
            .Include(s => s.Cliente)
            .FirstOrDefaultAsync(s => s.Id == id && s.ClienteId == cliente.Id);

        if (solicitud == null) return NotFound();
        return View(solicitud);
    }
}