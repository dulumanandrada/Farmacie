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

    public DbSet<MedicamentCommand> MedicamentCommands { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // definire primary key compus
        modelBuilder.Entity<MedicamentCommand>()
            .HasKey(pc => new {pc.MedicamentId, pc.CommandId });


        // definire relatii cu modelele Bookmark si Article (FK)
        modelBuilder.Entity<MedicamentCommand>()
            .HasOne(pc => pc.Medicament)
            .WithMany(pc => pc.MedicamentCommands)
            .HasForeignKey(pc => pc.MedicamentId);

        modelBuilder.Entity<MedicamentCommand>()
            .HasOne(pc => pc.Command)
            .WithMany(pc => pc.MedicamentCommands)
            .HasForeignKey(pc => pc.CommandId);
    }
}

