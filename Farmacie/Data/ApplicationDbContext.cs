using Farmacie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Farmacie.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Patient> Patients { set; get; }
    public DbSet<Medicament> Medicaments { set; get; }
    public DbSet<Command> Commands { get; set; }
}

