using APIClientesCarne.Context;
using APIClientesCarne.DTO;
using APIClientesCarne.Models;
using APIClientesCarne.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace APIClientesCarne.Controllers;




[Route("api/[controller]")]
[ApiController]
public class AuthController:ControllerBase
{
    
    
    private readonly MyDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, MyDbContext context, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _context = context;
        _logger = logger;
    }
    
    // Método para generar el token JWT
    private IActionResult GenerateToken(Claim[] claims, Usuario user, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: signIn
        );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogInformation("Login exitoso");

        return Ok(new { Token = tokenValue, User = user, Role = role });
    }



    //Esto hay que aplicarle el JWT para que solo muestre las solicitudes
    //hechas por el usuario que esta revisando
    [Authorize]
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
            return Unauthorized("Contraseña Incorrecta");
        }
        
        // Crear los claims para el token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Rol)
        };

        // Generar el token usando el método GenerateToken
        return GenerateToken(claims, user, user.Rol);
        
        
    }
    
    //Endpoint de Registro de usuario o Edicion de informacion usuario
    [HttpPost("RegisterEdit")]
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
        if (registerRequest.IdUsuario > 0)
        {
            var existingUser = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == registerRequest.IdUsuario);

            if (existingUser == null)
            {
                return NotFound($"Usuario con ID {registerRequest.IdUsuario} no encontrado.");
            }

            // Actualizar los valores
            existingUser.Username = registerRequest.Username;
            existingUser.Nombre = registerRequest.Nombre;
            existingUser.Email = registerRequest.Email;
            existingUser.Telefono = registerRequest.Telefono;
            existingUser.FechaNacimiento = registerRequest.FechaNacimiento;
            existingUser.Apellidos = registerRequest.Apellidos;
            existingUser.Direccion = registerRequest.Direccion;
            existingUser.Rnc = registerRequest.Rnc;
            
            // Actualizar contraseña si se envía una nueva
            if (!string.IsNullOrEmpty(registerRequest.Password))
            {
                existingUser.Password = PassHash.HashPassword(registerRequest.Password);
            }

            _context.SaveChanges();
            return Ok(new { Message = "Usuario actualizado exitosamente.", UserId = existingUser.IdUsuario });

        }
        else
        {
            //Si no es una edicion, realizar una creacion de usuario
            if (_context.Usuarios.Any(u => u.Email == registerRequest.Email))
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            var newUser = new Usuario()
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                Apellidos = registerRequest.Apellidos,
                Rol = "Cliente",
                Estado = "Activo",
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
    
}