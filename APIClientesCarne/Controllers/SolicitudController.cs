using APIClientesCarne.Context;
using APIClientesCarne.DTO;
using APIClientesCarne.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIClientesCarne.Controllers;



//CREACION DEL CONTROLADOR PARA SOLICITUDES
[Route("api/[controller]")]
[ApiController]
public class SolicitudController:ControllerBase
{
    
    private readonly MyDbContext _context;


    public SolicitudController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetSolicitudes()
    {
        var solicitudes = _context.Solicituds.ToList();
        return Ok(solicitudes);
    }

    //ESTO ES SOLO UN EJEMPLO DE COMO HACER UN POST, HAY QUE PONERLE MAS VALIDACIONES
    //FALTAN ATRIBUTOS, ESTO SOLO FUE UNA PRUEBA
    //NO ES NECESARIO HACER UN SCAFFOLDING CON TAL DE QUE EL DTO TENGA LOS MISMOS ATRIBUTOS QUE LA BD
    [HttpPost]
    public IActionResult PostSolicitud([FromBody] SolicitudDTO solicitudDto)
    {
        
        var newSolicitud = new Solicitud
        {
            IdUsuarioCliente = solicitudDto.IdUsuarioCliente, //esto deberia de capturarlo a traves de JWT
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