using manhour_services.Models;
using manhour_services.Modules;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manhour_services.Data
{
    public class ManpowerContext : DbContext

    {
        public DbSet<ActivityName> ActivityNames { get; set; } = null!;
        public DbSet<MhSaving> MhSavings { get; set; } = null!;
        public DbSet<StepProgress> StepProgresses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            Params.DbConnnectionString = "User ID =postgres;Server=localhost;Port=5432;Database=manpowerControl;Username=postgres;Password=postgres;Integrated Security=true;Pooling=true;";

            optionsBuilder.UseNpgsql(Params.DbConnnectionString);


        }

    }
}
