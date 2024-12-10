using System;
using System.Collections.Generic;

namespace APIClientesCarne.Models;

public partial class RolePermiso
{
    public int IdRol { get; set; }

    public string Role { get; set; } = null!;

    public string? TableName { get; set; }

    public bool? CanCreate { get; set; }

    public bool? CanUpdate { get; set; }

    public bool? CanDelete { get; set; }

    public bool? CanRead { get; set; }
    
}
