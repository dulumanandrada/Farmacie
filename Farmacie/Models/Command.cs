using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farmacie.Models
{
    public class Command
    {
        //id = primary key
        [Key]
        public int Id { get; set; }

        //cine a facut comanda
        //foreign key cu User
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; } 

        //numele pe care se face comanda
        //foreign key cu Patient
        [Required(ErrorMessage ="Numele pacientului este obligatoriu!")]
        public string? PatientName { get; set; }

        //public int? PatientId { get; set; }
        //public virtual Patient? Patient { get; set; }

        //diagnostic pentru reteta
        public string? Diagnostic { get; set; }
    }
}

