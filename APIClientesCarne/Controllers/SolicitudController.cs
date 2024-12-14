using APIClientesCarne.Context;
using APIClientesCarne.DTO;
using APIClientesCarne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APIClientesCarne.Controllers;

//Creacion de un controlador para las solicitudes   
[Route("api/[controller]")]
[ApiController]
public class SolicitudController : ControllerBase
{
    private readonly MyDbContext _context;

    public SolicitudController(MyDbContext context)
    {
        _context = context;
    }

    // Endpoint protegido con JWT
    [Authorize]
    [HttpGet]
    public IActionResult GetSolicitudes()
    {
        // Obtener el email del usuario autenticado
        var userEmail = User.Identity.Name;
        // Obtener el usuario autenticado de la base de datos
        var user = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
        {
            return Unauthorized("Usuario no encontrado");
        }

        // Filtrar las solicitudes por el ID del usuario autenticado
        var solicitudes = _context.Solicituds
            .Where(s => s.IdUsuarioCliente == user.IdUsuario)
            .ToList();

        return Ok(solicitudes);
    }

    // Endpoint protegido con JWT
    [Authorize]
    [HttpPost]
    public IActionResult PostSolicitud([FromBody] SolicitudDTO solicitudDto)
    {
        // Obtener el email del usuario autenticado
        var userEmail = User.Identity.Name;
        // Obtener el usuario autenticado de la base de datos
        var user = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
        {
            return Unauthorized("Usuario no encontrado");
        }

        var newSolicitud = new Solicitud
        {
            IdUsuarioCliente = user.IdUsuario,
            Coordenadas = solicitudDto.Coordenadas,
            Direccion = solicitudDto.Direccion,
            TipoOperacion = solicitudDto.TipoOperacion,
            NombreEst = solicitudDto.NombreEst,
            FechaAdmitida = DateTime.Now,
            EstadoSolicitud = "En espera",
            FechaAprobada = null,
        };

        _context.Solicituds.Add(newSolicitud);
        _context.SaveChanges();

        return Ok(newSolicitud);
    }
}