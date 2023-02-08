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
    public class AnalyseService
    {
        private readonly ILogger _logger;
        private readonly ToolConfiguration _config;
        private readonly VietlottContext _context;

        public AnalyseService(ILogger<AnalyseService> logger, ToolConfiguration config, VietlottContext context, IHttpClientFactory httpClientFactory = null)
        {
            _logger = logger;
            _config = config;
            _context = context;            
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
        #endregion
    }
}
