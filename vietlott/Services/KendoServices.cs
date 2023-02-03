using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Globalization;
using System.Text.Json;
using vietlott.Constants;
using vietlott.Response;
using vietlott.Settings;
namespace vietlott.Services
{
    internal class KendoServices
    {
        private readonly ILogger<KendoServices> _logger;
        private readonly IToolConfiguration _config;
        private readonly VietlottContext _context;
        private RestClient _httpRestClient;
        private static readonly Random getrandom = new Random();
        private List<int> GuessResult_Kendo = new List<int>();
        public IEnumerable<ResultKendoReponse>? resultKendoReponse { get; set; }
        public KendoServices(IToolConfiguration configurationRoot, ILogger<KendoServices> logger, VietlottContext context)
        {
            _logger = logger;
            _config = configurationRoot;
            _context = context;
            _httpRestClient = new RestClient("https://www.minhchinh.com/livekqxs/xstt/js/KN.js?key=c56a1c4145e05bd2a6b2838b381147");
            _httpRestClient.AddDefaultHeader("Accept", "*/*");
            _httpRestClient.AddDefaultHeader("User-Agent", "request");
        }
        public void Run()
        {
            Console.WriteLine(CommonConst.Message_Start);
            _logger.LogInformation(CommonConst.Message_Start);
            using (var progress = new ProgressBarServices())
            {
                for (int i = 0; i <= 100; i++)
                {
                    progress.Report((double)i / 100);
                    Thread.Sleep(20);
                }
                ResultKendo();
            }
        }

        public void ResultKendo()
        {
            try
            {
                var request = new RestRequest();
                request.Method = Method.Get;
                var response = _httpRestClient.Execute(request);
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content!.Replace("xsdt[8]=", "");
                    var resultKendoReponse = JsonConvert.DeserializeObject<ResultKendoReponse>(data!);
                    ResultKeno model = new ResultKeno();
                    model.Result = response.Content.ToString();
                    model.LiveDate = resultKendoReponse!.live_date!;
                    model.LiveKy = resultKendoReponse.live_ky;
                    model.NextKy = resultKendoReponse.next_ky;
                    model.DateResult = resultKendoReponse.lastResult!.kq!.ToString()!;
                    model.CreateDate = DateTime.Now;
                    using (var context = new VietlottContext())
                    {
                        var isCheckData = context.ResultKenos.Where(n => n.LiveKy == model.LiveKy).FirstOrDefault();
                        DateTime dtNow = DateTime.Now;
                        var dtStr = resultKendoReponse.next_date;
                        DateTime dtNext_live = DateTime.Parse(dtStr!);
                        if (dtNow > dtNext_live && isCheckData == null)
                        {
                            Console.WriteLine(" \n Added Successfully ResultKenos");
                            context.Entry(model).State = EntityState.Added;
                            context.SaveChanges();
                            Random_Kendo(model.NextKy);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex.ToString());
            }
        }
        public async Task SetInterval(Action action, TimeSpan timeout)
        {
            Console.WriteLine("Trace ResultKeno.");
            await Task.Delay(timeout).ConfigureAwait(false);
            action();
            await SetInterval(action, timeout);
        }
        public void Random_Kendo(int nextKy)
        {
            for (int i = 0; i < 20; i++)
            {
                int number = GetRandomNumber(1, 80);
                GuessResult_Kendo.Add(number);

            }
            GuessResult_Kendo.Sort();
            GuessResultKeno model = new GuessResultKeno();
            model.LiveKy = nextKy;
            model.Times = 0;
            model.GuessResult = string.Join(",", GuessResult_Kendo.ToArray());
            model.CreateDate = DateTime.Now;
            using (var context = new VietlottContext())
            {
                var isCheckData = context.GuessResultKenos.Where(n => n.LiveKy == nextKy).FirstOrDefault();
                if (isCheckData == null)
                {
                    Console.WriteLine("Added Successfully GuessResultKenos ");
                    context.Entry(model).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
        }
        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom)
            {
                return getrandom.Next(min, max);
            }
        }
    }

}
