using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManpowerControl.Models
{
    public class WorkSheetName
    {
        public  string ? FactoryID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Result {  get; set; }
    }

    public enum MonthName
    {
        Jan,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }
}
