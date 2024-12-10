using APIClientesCarne.Context;
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
        var users = _context.Admins.ToList();
        
        return Ok(users);
    }
    
    
}