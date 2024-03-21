using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManpowerControl.Models
{
    public class RelationalActivity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long ID { get; set; }

        [Required]
        public string? FactoryID { get; set; }

        [Required]
        public int UpdateYear { get; set; }
        [Required]
        public int UpdateMonth { get; set; }
        [Required]
        public string? ActivityID { get; set; }
        [Required]
        public string? MhSavingID { get; set; }
        [Required]
        public string? StepProgressesID { get; set; }
    }
}
