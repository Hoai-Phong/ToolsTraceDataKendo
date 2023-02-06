using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vietlott.DataAccess;
using Vietlott.DataAccess.Entities;
using Vietlott.Services.Models;
using Vietlott.Services.Settings;
using HtmlAgilityPack;
using System.Globalization;

namespace Vietlott.Services
{
    public class CollectResultService
    {
        private readonly ILogger<CollectResultService> _logger;
        private readonly ToolConfiguration _config;
        private readonly VietlottContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
      
        public CollectResultService(ILogger<CollectResultService> logger, ToolConfiguration config, VietlottContext context, IHttpClientFactory httpClientFactory = null)
        {
            _logger = logger;
            _config = config;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public void LiveResultFromMinhChinhDotCom()
        {            
            while (true)
            {
                var date = DateTime.Now;                
                var result = CollectResultFromMinhChinhDotCom(date, 1);
                int nRow = 0;
                if (result.Any())
                    nRow = InsertKenoResult(result);
                if (nRow == 0)
                {
                    Console.Write("\r");
                    Console.Write("                                                              ");
                    Console.Write("\r");
                    Console.Write($"[{date.ToString("dd-MM-yyyy HH:mm:ss")}] None....");                    
                }
                else
                {
                    Console.Write("\r");
                    Console.Write("                                                              ");
                    Console.Write("\r");
                    Console.Write($"[{date.ToString("dd-MM-yyyy HH:mm:ss")}] {nRow} records.");                    
                }
                Thread.Sleep(30000);
            }

        }



        public void CollectResultFromMinhChinhDotCom()
        {
            var date = DateTime.Today;
            int nNoResult = 0;
            while (nNoResult < 30)
            {
                Console.WriteLine($"-Date: {date.ToShortDateString()}");
                var result = CollectResultFromMinhChinhDotCom(date);
                if (result.Any())
                {
                    InsertKenoResult(result);
                    nNoResult = 0;
                }
                else
                    nNoResult++;
                date = date.AddDays(-1);
            }

        }

        public void CollectResultFromMinhChinhDotCom(DateTime fromDate, DateTime toDate)
        {
            var date = toDate;            
            while (date >= fromDate)
            {
                Console.WriteLine($"-Date: {date.ToShortDateString()}");
                var result = CollectResultFromMinhChinhDotCom(date);
                if (result.Any())
                {
                    InsertKenoResult(result);                    
                }                
                date = date.AddDays(-1);
            }
        }

        public IList<KenoResult> CollectResultFromMinhChinhDotCom(DateTime date)
        {
            var list = new List<KenoResult>();
            int page = 1;
            while (true)
            {
                Console.WriteLine($"--Page: {page}");
                var kenos = CollectResultFromMinhChinhDotCom(date, page);                
                if (kenos.Any())
                    list.AddRange(kenos);
                else
                    break;
                page++;
            }
            return list;
        }

        public IList<KenoResult> CollectResultFromMinhChinhDotCom(DateTime date, int page)
        {
            // Call http request
            var url = @"https://www.minhchinh.com/xo-so-dien-toan-keno.html";
            var data = new MinhChinhFormRequest() { Date = date, Page = page };
            var formData = new FormUrlEncodedContent(data.ToHtmlFormData());
            var httpClient = _httpClientFactory.CreateClient();
            var task1 = httpClient.PostAsync(url, formData);
            task1.Wait();
            var result = task1.Result;
            if (!result.IsSuccessStatusCode)
                return new List<KenoResult>();
            // Extract html text
            var doc = new HtmlDocument();
            doc.Load(result.Content.ReadAsStream());
            var node = doc.DocumentNode.SelectSingleNode("//*[@id=\"containerKQKeno\"]/div[2]");
            if (node == null)
                return new List<KenoResult>();
            var resultNodes = doc.DocumentNode.SelectNodes("//div[@class=\"wrapperKQKeno \"] | //div[@class=\"wrapperKQKeno odd\"]");
            var kenos = new List<KenoResult>();
            if (resultNodes == null)
                return kenos;
            foreach (var rn in resultNodes)
            {
                try
                {
                    var ky = rn.SelectSingleNode("div[@class=\"kyKQKeno\"]").InnerText.Replace("#", "");
                    var date1 = rn.SelectSingleNode("div[@class=\"timeKQ\"]/div[1]").InnerText.Trim();
                    var time = rn.SelectSingleNode("div[@class=\"timeKQ\"]/div[2]").InnerText.Trim();                    
                    var kq = string.Join(' ', rn.SelectNodes("div[@class=\"boxKQKeno\"]/div").Select(i => i.InnerText));

                    var dt = $"{date1.Replace("/", "")}{time.Replace(":", "")}";
                    var keno = new KenoResult()
                    {
                        Ky = int.Parse(ky),
                        ResultTime = DateTime.ParseExact(dt, "ddMMyyyyHHmm", CultureInfo.InvariantCulture),
                        Result = kq
                    };
                    kenos.Add(keno);
                }
                catch
                {
                    Console.WriteLine($"---Failed to parse: Date = {date.ToShortDateString()}");
                    Console.WriteLine($"---Failed to parse: Page = {page}");
                    Console.WriteLine($"---Failed to parse: {rn.InnerHtml}");
                }
            }
            return kenos;
        }

        #region Support functions
        private int InsertKenoResult(IEnumerable<KenoResult> result)
        {
            foreach(var item in result)
            {
                if (!_context.KenoResults.Any(i => i.Ky == item.Ky))
                {
                    _context.KenoResults.Add(item);
                    Console.Write("\r");
                    Console.Write("                                                              ");
                    Console.Write("\r");
                    Console.WriteLine(item.ToString());
                }
            }
            return _context.SaveChanges();
        }
        #endregion
    }
}
