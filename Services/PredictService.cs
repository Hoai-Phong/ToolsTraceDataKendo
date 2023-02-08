using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vietlott.DataAccess;
using Vietlott.Services.Settings;

namespace Vietlott.Services
{
    public class PredictService
    {
        private readonly ILogger _logger;
        private readonly ToolConfiguration _config;
        private readonly VietlottContext _context;

        public PredictService(ILogger<PredictService> logger, ToolConfiguration config, VietlottContext context)
        {
            _logger = logger;
            _config = config;
            _context = context;
        }

        #region PredictRandomByDate
        public void PredictRandomByResultDate()
        {
            int nRec = 1000;
            int nRan = 1000;
            var samples = _context.KenoResults.OrderByDescending(i => i.Ky)
                                            .Take(nRec)
                                            .ToArray();
            double maxAcc = 0;
            for (int baseSeed = 1; baseSeed < 1000000000; baseSeed++)
            {
                var predicts = samples.Select(i => PredictRandomByResultDate(i.ResultTime, baseSeed, nRan))
                                    .ToArray();
                int[] count = new int[nRan];
                for (int i = 0; i < nRec; i++)
                {
                    var sample = samples[i].Result;
                    var predict = predicts[i];
                    for (int p = 0; p < nRan; p++)
                    {
                        if (sample.Contains(predict[p]))
                            count[p]++;
                    }
                }
                //Console.WriteLine("Predict random by result time");
                var accMax = (double)count.Max() * 100 / nRec;
                var accMin = (double)count.Min() * 100 / nRec;
                if (accMax > 50)
                {
                    Console.WriteLine();
                    Console.WriteLine($"BaseSeed: {baseSeed} | Max Accurate: {accMax:00.0}% | Min Accurate: {accMin:00.0}%");
                }
                else if(accMax > maxAcc)
                {
                    maxAcc = accMax;
                    Console.Write("\r                                                                                                      ");
                    Console.Write($"\r BaseSeed: {baseSeed} | Max Accurate: {accMax:00.0}% | Min Accurate: {accMin:00.0}%");
                }
                //for (int p = 0; p < nRan; p++)
                //{
                //    var accurate = (double)count[p] * 100 / nRec;
                //    Console.WriteLine($"Random: {p + 1:00} | Accurate: {accurate:00.0}%");
                //}
            }
        }

        public string[] PredictRandomByResultDate(DateTime datetime, int ratio, int n)
        {            
            var seed = datetime.GetHashCode() + ratio;
            Random rand = new Random(seed);
            var predicts = new string[n];
            for (int i = 0; i < n; i++)
            {
                predicts[i] = (rand.Next(80) + 1).ToString("00");
            }
            return predicts;
        }

        public string[] PredictRandomByResultDate(DateTime datetime, int n)
        {
            var seed = datetime.GetHashCode();
            Random rand = new Random(seed);
            var predicts = new string[n];
            for (int i = 0; i < n; i++)
            {
                predicts[i] = (rand.Next(80) + 1).ToString("00");
            }
            return predicts;
        }

        #endregion

        #region PredictRandomByDate
        public void PredictRandomByDate()
        {
            // Calculate accurate of algorithm
            var fromDate = DateTime.Today.AddDays(-10);
            var toDate = DateTime.Today.AddDays(-1);
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                var resultList = _context.KenoResults.Where(i => i.ResultTime > date && i.ResultTime < date.AddDays(1))
                                                .OrderBy(i => i.ResultTime)
                                                .Select(i => i.Result)
                                                .ToList();

                int nMatch = 0;
                for (int i = 0; i < resultList.Count; i++)
                {
                    var predict = PredictRandomByDate(date, i + 1).ToString("00");
                    if (resultList[0].Contains(predict))
                        nMatch++;
                }
                var accurate = (double)nMatch * 100 / resultList.Count;
                // Show result
                Console.WriteLine("Predict random by date");
                Console.WriteLine($"  Date: {date.ToShortDateString()}");
                Console.WriteLine($"  Number of results: {resultList.Count}");
                Console.WriteLine($"  Number of matched: {nMatch}");
                Console.WriteLine($"  Accurate: {accurate:00.0}%");
            }
        }

        public int PredictRandomByDate(DateTime date, int nkyInDate)
        {
            var hash = date.Date.GetHashCode();
            Random rand = new Random(hash);
            var predict = 0;
            for (int i = 0; i < nkyInDate * 80; i++)
            {
                predict = rand.Next(80) + 1;
            }
            return predict;
        }

        #endregion
    }
}
