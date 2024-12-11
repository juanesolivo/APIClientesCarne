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
            return Unauthorized();
        }

        if (user.Password != loginRequest.Password)
        {
            return Unauthorized();
        }
        
        return Ok(user);
    }
    
    //Endpoint de Registro
    [HttpPost("Register")]
    public IActionResult AddOrUpdate([FromBody] RegisterRequest registerRequest)
    {
        
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