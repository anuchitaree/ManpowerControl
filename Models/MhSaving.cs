using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manhour_services.Models
{
    public class MhSaving
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key,Column(Order =0)]
        public long ID { get; set; }
        [Required, Column(Order = 1)]
        public string? ActivityID { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public double MhSavingPlan { get; set; }

        [Required]
        public double MhSavingActual { get; set; }

    }
}
