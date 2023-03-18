using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Farmacie.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Command>? Commands { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }

    }
}

