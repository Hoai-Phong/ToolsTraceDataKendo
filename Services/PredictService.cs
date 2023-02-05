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

        public PredictService(ILogger<PredictService> logger, ToolConfiguration config, VietlottContext context, IHttpClientFactory httpClientFactory = null)
        {
            _logger = logger;
            _config = config;
            _context = context;
        }

        #region PredictRandomByDate
        public void PredictRandomByDate()
        {
            // Calculate accurate of algorithm
            var date = DateTime.Today.AddDays(-1);
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

        public int PredictRandomByDate(DateTime date, int nkyInDate)
        {
            var hash = date.Date.GetHashCode();
            Random rand = new Random(hash);
            var predict = 0;
            for(int i = 0; i < nkyInDate; i++)
            {
                predict = rand.Next(80) + 1;
            }
            return predict;
        }

        #endregion
    }
}
