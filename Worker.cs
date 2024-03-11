using manhour_services.Data;
using manhour_services.Models;
using manhour_services.Modules;
using ManpowerControl.Models;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using OfficeOpenXml;
using System.Linq;
using System.Text.Json;
using System.Xml;

namespace ManpowerControl
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Initialize();
            return base.StartAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var path = @"d:\project\excel-python\DNKI_Dec'23.xlsx";

                var filename = Filename(path);
                var factoryActivities = new List<Activity>();

                var package = new ExcelPackage(path);

                var worksheet = package.Workbook.Worksheets["TIE&PE"]; // 49 to 4999 step 2
                if (worksheet == null)
                {
                    // no work sheet
                }



                for (int r = 49; r < 52; r += 2)
                {
                    var factoryActivity = new Activity();
                    factoryActivity.FactoryID = filename.FactoryID;
                    factoryActivity.UpdateYear = filename.Year;
                    factoryActivity.UpdateMonth = filename.Month;

                    var activityID = Guid.NewGuid().ToString();

                    var activityName = new ActivityName();
                    var activityDetail = worksheet?.Cells[r, 3].Value;
                    if (activityDetail == null)
                    {
                        continue;
                    }
                    else
                    {
                        activityName.ActivityDetail = worksheet?.Cells[r, 3].Value.ToString();
                    }
                    activityName.FactoryID = factoryActivity.FactoryID;
                    var linename = worksheet?.Cells[r, 4].Value;
                    activityName.LineName = linename == null ? "" : worksheet?.Cells[r, 4].Value.ToString();

                    var productModel = worksheet?.Cells[r, 5].Value;
                    activityName.ProductModel = productModel == null ? "" : worksheet?.Cells[r, 5].Value.ToString();

                    var pic = worksheet?.Cells[r, 6].Value;
                    activityName.Pic = pic == null ? "" : worksheet?.Cells[r, 6].Value.ToString();

                    var automationCategory = worksheet?.Cells[r, 7].Value;
                    activityName.AutomationCategory = automationCategory == null ? "" : worksheet?.Cells[r, 7].Value.ToString();

                    var feasibility = worksheet?.Cells[r, 8].Value;
                    activityName.Feasibility = feasibility == null ? "" : worksheet?.Cells[r, 8].Value.ToString();

                    var status = worksheet?.Cells[r, 9].Value;
                    activityName.Status = status == null ? "" : worksheet?.Cells[r, 9].Value.ToString();

                    var categoryReasonIssue = worksheet?.Cells[r, 10].Value;
                    activityName.CategoryReasonIssue = categoryReasonIssue == null ? "" : worksheet?.Cells[r, 10].Value.ToString();

                    var category = worksheet?.Cells[r, 11].Value;
                    activityName.Category = category == null ? "" : worksheet?.Cells[r, 11].Value.ToString();

                    var subCategoryDetail = worksheet?.Cells[r, 12].Value;
                    activityName.SubCategoryDetail = subCategoryDetail == null ? "" : worksheet?.Cells[r, 12].Value.ToString();

                    activityName.ActivityID = activityID;


                    var mhSaving = new List<MhSaving>();
                    int[] month = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 1, 2, 3 };
                    for (int c = 0; c < 12; c++)
                    {
                        var _plan = worksheet?.Cells[r, c + 14].Value;
                        var plan = _plan == null ? 0 : Convert.ToDouble(worksheet?.Cells[r, c + 14].Value.ToString());
                        var _actual = worksheet?.Cells[r + 1, c + 14].Value;
                        var actual = _actual == null ? 0 : Convert.ToDouble(worksheet?.Cells[r + 1, c + 14].Value.ToString());
                        mhSaving.Add(new MhSaving
                        {
                            ActivityID = activityID,
                            Month = month[c],
                            Year = filename.Year,
                            MhSavingPlan = plan,
                            MhSavingActual = actual,
                        }); ;
                    }

                    var stepProgresses = new List<StepProgress>();
                    for (int c = 0; c < 12; c++)
                    {
                        var startCol = 31;
                        var _plan = worksheet?.Cells[r, c + startCol].Value;
                        var plan = _plan == null ? 0 : Convert.ToInt32(worksheet?.Cells[r, c + startCol].Value.ToString());
                        var _actual = worksheet?.Cells[r + 1, c + startCol].Value;
                        var actual = _actual == null ? 0 : Convert.ToInt32(worksheet?.Cells[r + 1, c + startCol].Value.ToString());
                        stepProgresses.Add(new StepProgress
                        {
                            ActivityID = activityID,
                            Month = month[c],
                            Year = filename.Year,
                            StepProgressPlan = plan,
                            StepProgressActual = actual,
                        }); ;
                    }
                    factoryActivity.ActivityName = activityName;
                    factoryActivity.MhSavings = mhSaving;
                    factoryActivity.StepProgresses = stepProgresses;

                    factoryActivities.Add(factoryActivity);





                }
                Console.WriteLine(JsonSerializer.Serialize(factoryActivities));

                //using(var db = new ManpowerContext())
                //{
                //    if (db.Database.CanConnect())
                //    {
                //        _logger.LogInformation("The database is connected.");
                //    }

                //    db.ActivityNames.AddRange(activityName);

                //}

                await Task.Delay(10000, stoppingToken);
            }
        }


        private WorkSheetName Filename(string filename)
        {

            var monthNames = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var file = Path.GetFileName(filename);
            var packing = file.Split('.')[0];
            var factorydate = packing.Split('_');
            if (factorydate.Length != 2)
            {
                return new WorkSheetName { FactoryID = null, Month = 0, Year = 0, Result = false };
            }

            var yearmonth = factorydate[1].Split('\'');
            if (yearmonth.Length != 2)
            {
                return new WorkSheetName { FactoryID = factorydate[0], Month = 0, Year = 0, Result = false };
            }
            if (!monthNames.Contains(yearmonth[0]))
            {
                return new WorkSheetName { FactoryID = factorydate[0], Month = 0, Year = Convert.ToInt32(yearmonth[1]), Result = false };
            }
            var index = monthNames.IndexOf(yearmonth[0]);
            return
                new WorkSheetName { FactoryID = factorydate[0], Month = index, Year = Convert.ToInt32(yearmonth[1]), Result = true };

        }
        private void Initialize()
        {
            Params.DbConnnectionString = _configuration.GetValue<string>("PostgresConnectionString");


        }
    }
}
