using ManpowerControl.Models;
using ManpowerControl.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManpowerControl.Data
{
    public class ManpowerContext : DbContext

    {
        // public DbSet<RelationalActivity> RelationalActivity { get; set; } = null!;
        public DbSet<Activity> Activity { get; set; } = null!;
        public DbSet<MhSaving> MhSaving { get; set; } = null!;
        public DbSet<StepProgress> StepProgress { get; set; } = null!; 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            Params.DbConnnectionString = "User ID =postgres;Server=localhost;Port=5432;Database=manpowerControl;Username=postgres;Password=postgres;Integrated Security=true;Pooling=true;";

            optionsBuilder.UseNpgsql(Params.DbConnnectionString);


        }

    }
}
