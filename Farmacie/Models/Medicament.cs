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
        public string Name { get; set; }

        //gramaj
        public double Weight { get; set; }

        //forma pastile
        public string? Form { get; set; }

        //lot
        public string Lot { get; set; }

        //data expirare
        public DateTime ExpDate { get; set; }

        //nr bucati stoc
        public int Quantity { get; set; }
    }
}

