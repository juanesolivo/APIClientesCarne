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
    //HAY QUE HACER QUE SI LA SOLICITUD NO HA PASADO UN INSPECTOR, ESTE LA PUEDA EDITAR
    [Authorize]
    [HttpPost("SolicitudEdit")]
    public IActionResult PostOrEditSolicitud([FromBody] SolicitudDTO solicitudDto)
    {
        // Obtener el email del usuario autenticado
        var userEmail = User.Identity.Name;
        // Obtener el usuario autenticado de la base de datos
        var user = _context.Usuarios.FirstOrDefault(u => u.Email == userEmail);

        if (user == null)
        {
            return Unauthorized("Usuario no encontrado");
        }

        //verificar si es una edicion y si esta en espera
        if (solicitudDto.IdSolicitud >= 0)
        {
            var existingSolicitud = _context.Solicituds.FirstOrDefault(u => u.IdSolicitud == solicitudDto.IdSolicitud);

            if (existingSolicitud == null)
            {
                return NotFound($"Solicitud con ID {solicitudDto.IdSolicitud} no encontrado o no esta en espera.");
            }

            if (existingSolicitud.EstadoSolicitud == "En espera")
            {
                //actualizar los valores
                existingSolicitud.Direccion = solicitudDto.Direccion;
                existingSolicitud.Coordenadas = solicitudDto.Coordenadas;
                existingSolicitud.NombreEst = solicitudDto.NombreEst;
                existingSolicitud.TipoOperacion = solicitudDto.TipoOperacion;
            }
            else
            {
                return NotFound($"Solicitud con ID {solicitudDto.IdSolicitud} ya esta asignada por ende no se puede editar.");
            }
        }
        
        

        //Si no es edicion, crear la solicitud
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