namespace APIClientesCarne.DTO;

public class SolicitudDTO
{
    //NO ES NECESARIO HACER UN SCAFFOLDING CON TAL DE QUE EL DTO TENGA LOS MISMOS ATRIBUTOS QUE LA BD
    public int IdSolicitud { get; set; }

    public int IdUsuarioCliente { get; set; }

    public DateTime? FechaAdmitida { get; set; }

    public DateTime? FechaAprobada { get; set; }

    public string? EstadoSolicitud { get; set; }
    
    //ESTOS SON LOS NUEVOS ATRIBUTOS CON LOS MISMOS NOMBRES DE LA BD
    public string NombreEst { get; set; }
    
    public string TipoOperacion { get; set; }
    
    public string? Direccion { get; set; }
    
    public string? Coordenadas { get; set; }    
}