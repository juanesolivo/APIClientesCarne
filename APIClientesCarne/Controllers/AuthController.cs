using APIClientesCarne.Context;
using APIClientesCarne.DTO;
using APIClientesCarne.Models;
using APIClientesCarne.Utils;
using Microsoft.AspNetCore.Mvc;

namespace APIClientesCarne.Controllers;




[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    
    
    private readonly MyDbContext _context;

    public AuthController(MyDbContext context)
    {
        _context = context;
    }




    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _context.Usuarios.ToList();
        
        return Ok(users);
    }

    [HttpPost("Login")]
    public IActionResult Post([FromBody] LoginRequest loginRequest)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Email == loginRequest.Email);
        if (user == null)
        {
            return Unauthorized("No se encontro el usuario");
        }

        if (!PassHash.VerifyPassword(loginRequest.Password, user.Password))
        {
            return Unauthorized("Contraseña o Email Incorrecto");
        }
        
        return Ok(user);
    }
    
    //Endpoint de Registro de usuario o Edicion de informacion usuario
    [HttpPost("Register")]
    public IActionResult AddOrUpdate([FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest == null)
        {
            return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
        }

        if (string.IsNullOrEmpty(registerRequest.Email))
        {
            return BadRequest("El correo es obligatorio.");
        }

        if (string.IsNullOrEmpty(registerRequest.Username))
        {
            return BadRequest("El nombre de usuario es obligatorio.");
        }
        
        //Verificar si es una edicion
        if (registerRequest.UserId > 0)
        {
            //Por hacer
        }
        if (_context.Usuarios.Any(u => u.Email == registerRequest.Email))
        {
            return BadRequest();
        }

        var newUser = new Usuario()
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            Apellidos = registerRequest.Apellidos,
            Rol = "Cliente",
            Direccion = registerRequest.Direccion,
            FechaNacimiento = registerRequest.FechaNacimiento,
            Nombre = registerRequest.Nombre,
            Password = PassHash.HashPassword(registerRequest.Password),
            Telefono = registerRequest.Telefono,
            FechaIngreso = DateTime.Today
        };
        _context.Usuarios.Add(newUser);
        _context.SaveChanges();
        return Ok(newUser);
    }

    
}