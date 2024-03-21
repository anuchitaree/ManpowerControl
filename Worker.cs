using System;
using ManpowerControl.Data;
using ManpowerControl.Modules;
using ManpowerControl.Models;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using OfficeOpenXml;
using System.Linq;
using System.Text.Json;
using System.Xml;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

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
                var listGroupActivity = new List<GroupActivity>();

                var package = new ExcelPackage(path);

                var worksheet = package.Workbook.Worksheets["TIE&PE"]; // 49 to 4999 step 2
                if (worksheet == null)
                {
                    // no work sheet
                }

                var filedate = new Filedate();

                for (int r = 49; r < 5000; r += 2)
                {
                    var groupActivity = new GroupActivity();
                    filedate.FactoryID = filename.FactoryID;
                    filedate.UpdateYear = filename.Year;
                    filedate.UpdateMonth = filename.Month;

                    var activityID = Guid.NewGuid().ToString();

                    var activity = new Activity();
                    activity.ActivityID = activityID;
                    activity.FactoryID = filename.FactoryID;
                    activity.UpdateYear = filename.Year;
                    activity.UpdateMonth = filename.Month;

                    var activityDetail = worksheet?.Cells[r, 3].Value;
                    if (activityDetail == null)
                    {
                        continue;
                    }
                    else
                    {
                        activity.ActivityDetail = worksheet?.Cells[r, 3].Value.ToString();
                    }
                    // activity.FactoryID = factoryActivity.FactoryID;
                    var linename = worksheet?.Cells[r, 4].Value;
                    activity.LineName = linename == null ? "" : worksheet?.Cells[r, 4].Value.ToString();

                    var productModel = worksheet?.Cells[r, 5].Value;
                    activity.ProductModel = productModel == null ? "" : worksheet?.Cells[r, 5].Value.ToString();

                    var pic = worksheet?.Cells[r, 6].Value;
                    activity.Pic = pic == null ? "" : worksheet?.Cells[r, 6].Value.ToString();

                    var automationCategory = worksheet?.Cells[r, 7].Value;
                    activity.AutomationCategory = automationCategory == null ? "" : worksheet?.Cells[r, 7].Value.ToString();

                    var feasibility = worksheet?.Cells[r, 8].Value;
                    activity.Feasibility = feasibility == null ? "" : worksheet?.Cells[r, 8].Value.ToString();

                    var status = worksheet?.Cells[r, 9].Value;
                    activity.Status = status == null ? "" : worksheet?.Cells[r, 9].Value.ToString();

                    var categoryReasonIssue = worksheet?.Cells[r, 10].Value;
                    activity.CategoryReasonIssue = categoryReasonIssue == null ? "" : worksheet?.Cells[r, 10].Value.ToString();

                    var category = worksheet?.Cells[r, 11].Value;
                    activity.Category = category == null ? "" : worksheet?.Cells[r, 11].Value.ToString();

                    var subCategoryDetail = worksheet?.Cells[r, 12].Value;
                    activity.SubCategoryDetail = subCategoryDetail == null ? "" : worksheet?.Cells[r, 12].Value.ToString();



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
                            Order = c,
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
                            Order = c,
                            Month = month[c],
                            Year = filename.Year,
                            StepProgressPlan = plan,
                            StepProgressActual = actual,
                        }); ;
                    }
                    groupActivity.Activity = activity;
                    groupActivity.MhSaving = mhSaving;
                    groupActivity.StepProgresses = stepProgresses;

                    listGroupActivity.Add(groupActivity);
                }

                var _activity = new List<Activity>();
                var _mhSaving = new List<MhSaving>();
                var _stepProgress = new List<StepProgress>();
                foreach (var act in listGroupActivity)
                {
                    if (act.Activity != null)
                    {
                        _activity.Add(act.Activity);
                    }
                    if (act.MhSaving?.Count() > 0)
                    {
                        _mhSaving.AddRange(act.MhSaving);
                    }
                    if (act.StepProgresses?.Count() > 0)
                    {
                        _stepProgress.AddRange(act.StepProgresses);
                    }

                }


                using (var db = new ManpowerContext())
                {
                    if (db.Database.CanConnect())
                    {
                        _logger.LogInformation("The database is connected.");
                    }


                    var olddata = db.Activity
                    .Where(x => x.UpdateYear == filedate.UpdateYear && x.UpdateMonth == filedate.UpdateMonth && x.FactoryID == filedate.FactoryID).ToList();
                    if (olddata.Count() > 0)
                    {
                        db.Activity.RemoveRange(olddata);
                        db.SaveChanges();
                    }

                    await InsertActivity(_activity);
                    await InsertMhSaving(_mhSaving);
                    await InsertStepProgress(_stepProgress);






                }
                //  new List<GroupActivity>();
                // Console.WriteLine(JsonSerializer.Serialize(_activity));
                Console.WriteLine("------Completed--------");
                // Console.WriteLine(JsonSerializer.Serialize(_mhSaving));
                // Console.WriteLine();

                await Task.Delay(100000, stoppingToken);
            }
        }

        private async Task<bool> InsertActivity(List<Activity> data)
        {
            try
            {
                using (var db = new ManpowerContext())
                {
                    await db.Activity.AddRangeAsync(data);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> InsertMhSaving(List<MhSaving> data)
        {
            try
            {
                using (var db = new ManpowerContext())
                {
                    await db.MhSaving.AddRangeAsync(data);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> InsertStepProgress(List<StepProgress> data)
        {
            try
            {
                using (var db = new ManpowerContext())
                {
                    await db.StepProgress.AddRangeAsync(data);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
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
