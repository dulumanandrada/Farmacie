﻿using System;
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
        [Required(ErrorMessage ="Numele comenzii este obligatoriu!")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Pacientul este obligatoriu!")]
        public string? PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        //diagnostic pentru reteta
        public string? Diagnostic { get; set; }

        public string? Status { get; set; }

        public virtual ICollection<MedicamentCommand>? MedicamentCommands { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllStatus { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? AllPatients { get; set; }
    }
}

