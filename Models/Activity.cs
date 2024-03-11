using manhour_services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManpowerControl.Models
{
    public class Activity
    {
        public int ID { get; set; }
        public string? FactoryID { get; set; }
        public int UpdateYear { get; set; }
        public int UpdateMonth { get; set; }
        public ActivityName? ActivityName { get; set; }
        public List<MhSaving>? MhSavings { get; set; }
        public List<StepProgress>? StepProgresses { get; set; }

    }
}
