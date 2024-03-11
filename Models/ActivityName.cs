using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manhour_services.Models
{
    public class ActivityName
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long ID { get; set; }

        [Required]
        public int UpdateYear { get; set; }
        [Required]
        public int UpdateMonth { get; set; }


        [Required]
        public string? ActivityID { get; set; }
        [Required]
        public string? FactoryID { get; set; }
        [Required]
        public string? ActivityDetail { get; set; }

        public string? LineName { get; set; }
        public string? ProductModel { get; set; }
        public string? Pic { get; set; }
        public string? AutomationCategory { get; set; }
        public string? Feasibility { get; set; }
        public string? Status { get; set; }
        public string? CategoryReasonIssue { get; set; }
        public string? Category { get; set; }
        public string? SubCategoryDetail { get; set; }
       

    }
}
