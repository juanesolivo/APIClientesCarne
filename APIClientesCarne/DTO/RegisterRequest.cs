﻿namespace APIClientesCarne.DTO;
//DTO USADO PARA REALIZAR EL REGISTRO DE UN USUARIO (CLIENTE)
public class RegisterRequest
{
    public int IdUsuario { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Rol { get; set; }
    public string Email { get; set; }
    public string Direccion { get; set; }
    public string Nombre { get; set; }
    public string? Apellidos { get; set; }
    public string? Telefono { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    
    public string Rnc { get; set; }  // Nuevo campo
    
    public string Estado { get; set; } // Nuevo campo
    
}