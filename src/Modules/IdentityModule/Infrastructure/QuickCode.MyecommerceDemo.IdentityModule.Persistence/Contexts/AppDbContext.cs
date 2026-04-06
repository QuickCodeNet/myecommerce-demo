using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : 
    IdentityDbContext<ApiUser>(options)
{
}


public class ApiUser : IdentityUser
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public string? PermissionGroupName { get; set; }
}