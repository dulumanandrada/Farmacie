using System;
using System.ComponentModel.DataAnnotations;

namespace Farmacie.Models
{
    public class Patient
    {
        //id = primary key
        [Key]
        public string Id { get; set; }

        //nume
        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string FirstName { get; set; }

        //prenume
        [Required(ErrorMessage = "Prenumele este obligatoriu!")]
        public string LastName { get; set; }

        //CNP
        [Required(ErrorMessage = "CNP-ul este obligatoriu!")]
        [StringLength(13, ErrorMessage = "CNP-ul trebuie sa contina 13 cifre!", MinimumLength = 13)]
        public string CNP { get; set; }

        //Telefon (nu obligatoriu)
        public string? Phone { get; set; }

        //Adresa (nu obligatorie)
        public string? Address { get; set; }
    }
}

