using System;
using System.ComponentModel.DataAnnotations;

namespace Farmacie.Models
{
    public class Medicament
    {
        //id = primary key
        [Key]
        public int Id { get; set; }

        //denumire
        [Required(ErrorMessage = "Numele medicamentului este obligatoriu!")]
        public string Name { get; set; }

        //gramaj
        public double Weight { get; set; }

        //forma pastile
        public string? Form { get; set; }

        //lot
        public string Lot { get; set; }

        //data expirare
        //[DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime ExpDate { get; set; }

        //nr bucati stoc
        public int Quantity { get; set; }

        public virtual ICollection<MedicamentCommand>? MedicamentCommands { get; set; }
    }
}

