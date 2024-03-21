using ManpowerControl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManpowerControl.Models
{
    public class GroupActivity
    {
        public int ID { get; set; }
        // public string? FactoryID { get; set; }
        // public int UpdateYear { get; set; }
        // public int UpdateMonth { get; set; }
        public Activity? Activity { get; set; }
        public List<MhSaving>? MhSaving { get; set; }
        public List<StepProgress>? StepProgresses { get; set; }

    }
}
