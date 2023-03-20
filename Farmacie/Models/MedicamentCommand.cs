using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farmacie.Models
{
    public class MedicamentCommand
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Key]
        //public int MedicamentCommandId { get; set; }

        public int? MedicamentId { get; set; }
        public int? CommandId { get; set; }

        public string? MedicamentName { get; set; }
        public int? QuantityWanted { get; set; }
        //-------------------------------------------------
        public virtual Medicament? Medicament { get; set; }
        public virtual Command? Command { get; set; }
    }
}

