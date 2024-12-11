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


    //ESTO ES SOLO UN EJEMPLO DE COMO HACER UN POST, HAY QUE PONERLE MAS VALIDACIONES
    //FALTAN ATRIBUTOS, ESTO SOLO FUE UNA PRUEBA
    //NO ES NECESARIO HACER UN SCAFFOLDING CON TAL DE QUE EL DTO TENGA LOS MISMOS ATRIBUTOS QUE LA BD
    [HttpPost]
    public IActionResult PostSolicitud([FromBody] SolicitudDTO solicitudDto)
    {
        
        var NewSolicitud = new Solicitud
        {
            IdUsuarioCliente = solicitudDto.IdUsuarioCliente,
            Coordenadas = solicitudDto.Coordenadas,
            Direccion = solicitudDto.Direccion,
            TipoOperacion = solicitudDto.TipoOperacion,
            NombreEst = solicitudDto.NombreEst,
            
            
            
            
        };
        
        
        _context.Solicituds.Add(NewSolicitud);
        _context.SaveChanges();
        
        return Ok(NewSolicitud);
        
        
        
    }
    
    
}