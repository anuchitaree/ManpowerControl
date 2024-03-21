using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManpowerControl.Models
{
    public class StepProgress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long ID { get; set; }
        [Required]
        public string? ActivityID { get; set; }

        [Required]
        public int Order { get; set; } 
        [Required]
        public int Year { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int StepProgressPlan { get; set; }
        [Required]
        public int StepProgressActual { get; set; }
    }
}
