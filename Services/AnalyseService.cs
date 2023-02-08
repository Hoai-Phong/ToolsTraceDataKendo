using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vietlott.DataAccess;
using Vietlott.DataAccess.Entities;
using Vietlott.Services.Settings;
using Vietlott.Services.Models;

namespace Vietlott.Services
{
    public class AnalyseService
    {
        private readonly ILogger _logger;
        private readonly ToolConfiguration _config;
        private readonly VietlottContext _context;

        public AnalyseService(ILogger<AnalyseService> logger, ToolConfiguration config, VietlottContext context)
        {
            _logger = logger;
            _config = config;
            _context = context;            
        }

        public void DoStatictiscByDay()
        {
            var groups = _context.KenoResultDatas
                                    .GroupBy(i => new { i.Hour });
            foreach (var group in groups)
            {
                Console.WriteLine($"GroupKey = Hour: {group.Key.Hour}");
                var total = group.Count();
                var max = 0.0;
                for (int i = 0; i < 80; i++)
                {
                    var count = group.Sum(g => g[i + 1]);
                    var percent = (double)count * 100 / total;
                    if (percent > 50 && total > 50)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{i + 1:00} | {percent:00.0}% | {total} samples");
                    }
                    if (percent > max && total > 50)
                        max = percent;
                }
                if (total > 50)
                    Console.WriteLine($"Max percent: {max:00.0}%, {total} samples");
            }
        }

        public void DoStatictiscByWeekDay()
        {
            var groups = _context.KenoResultDatas
                                    .GroupBy(i => new { i.WeekDay, i.Hour, i.Minute });
            foreach (var group in groups)
            {
                Console.WriteLine($"GroupKey = WeekDay:{group.Key.WeekDay} - Hour: {group.Key.Hour} - Minute: {group.Key.Minute}");
                var total = group.Count();
                var max = 0.0;
                for (int i = 0; i < 80; i++)
                {
                    var count = group.Sum(g => g[i + 1]);
                    var percent = (double)count * 100 / total;
                    if (percent > 50 && total > 50)
                    {
                        Console.WriteLine($"{i + 1:00} | {percent:00.0}% | {total} samples");
                    }
                    if (percent > max && total > 50)
                        max = percent;
                }
                if (total > 50)
                    Console.WriteLine($"Max percent: {max:00.0}%, {total} samples");
            }
        }

        public void DoStatictiscByDayKy()
        {
            var groups = _context.KenoResultDatas
                                   .GroupBy(i => new { i.DayKy });
            foreach (var group in groups)
            {
                Console.WriteLine($"GroupKey = DayKy: {group.Key.DayKy}");
                var total = group.Count();
                var max = 0.0;
                for (int i = 0; i < 80; i++)
                {
                    var count = group.Sum(g => g[i + 1]);
                    var percent = (double)count * 100 / total;
                    if (percent > 50)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine($"{i + 1:00} | {percent:00.0}% | {total} samples");
                    }
                    if (percent > max && total > 50)
                        max = percent;
                }
                if (total > 50)
                    Console.WriteLine($"Max percent: {max:00.0}%, {total} samples");
            }
        }

        public void PrepareData()
        {
            var prevDate = DateTime.Now;
            int dayKy = 1;
            foreach (var keno in _context.KenoResults.OrderBy(i => i.Ky).ToList())
            {
                if (prevDate != keno.ResultTime.Date)
                {
                    dayKy = 1;
                    prevDate = keno.ResultTime.Date;

                    int count = _context.SaveChanges();
                    Console.WriteLine($"{prevDate.ToString("yyyyMMdd_HHMM")} {count} records.");
                }
                if (_context.KenoResultDatas.Any(i => i.Ky == keno.Ky))
                {
                    var data = CreateKenoAnalysisData(keno, dayKy);
                    _context.KenoResultDatas.Add(data);
                }
                dayKy++;
            }
            _context.SaveChanges();
        }

        public void ShowDistribution()
        {            
            // Analyse distribution
            int[] arr = BuildDistribution();
            // Show result
            int total = arr.Sum();
            int nKy = total / 20;
            Console.WriteLine($"Number of Ky: {nKy}");

            int len = arr.Length;
            for(int i = 0; i < len; i++)
            {
                var value = i + 1;
                var count = arr[i];
                var percent = ((double)count * 100) / nKy;
                Console.WriteLine($"{value:00}: {count} ({percent:00.0}%)");
            }

        }

        public void FindMissingKy()
        {
            var kyList = _context.KenoResults.Select(i => i.Ky).OrderBy(i => i).ToArray();
            int prev = 0;
            foreach(var ky in kyList)
            {
                for(int i = prev + 1; i < ky; i++)
                {
                    Console.WriteLine("Missing ky {0}", i);
                }

                prev = ky;
            }
        }

        #region Support functions
        private int[] BuildDistribution()
        {
            var allResults = _context.KenoResults.Select(i => i.Result)
                                                .AsEnumerable()
                                                .SelectMany(i => i.Split(" ", StringSplitOptions.None))
                                                .Select(i => int.Parse(i))
                                                .GroupBy(i => i)
                                                .OrderBy(i => i.Key)
                                                .Select(i => i.Count())
                                                .ToArray();
            return allResults;
        }
        private KenoResultData CreateKenoAnalysisData(KenoResult keno, int dayky)
        {
            var data = new KenoResultData()
            {
                Ky = keno.Ky,
                Year = keno.ResultTime.Year,
                Month = keno.ResultTime.Month,
                Day = keno.ResultTime.Day,
                Hour = keno.ResultTime.Hour,
                Minute = keno.ResultTime.Minute,                
                WeekDay = (int)keno.ResultTime.DayOfWeek,
                DayKy = dayky,
            };

            var list = keno.Result.Split(' ')
                            .Select(i => int.Parse(i))
                            .ToList();
            foreach (var item in list)
                data[item] = 1;

            return data;
        }

        #endregion
    }
}
