namespace Vietlott.Services.Models
{

    public class ResultKendoReponse
    {
        public int runtt { get; set; }
        public int newtime { get; set; }
        public int delay { get; set; }
        public int next_ky { get; set; }
        public string? next_date { get; set; }
        public int live_ky { get; set; }
        public string? live_date { get; set; }
        public lastResult? lastResult { get; set; }
        public object? ts { get; set; }
        public object? nt { get; set; }
        public object? it { get; set; }
        public object? cv { get; set; }

    }
    public class lastResult
    {
        public object? kq { get; set; }
        public int chan { get; set; }
        public int le { get; set; }
        public int lon { get; set; }
        public int nho { get; set; }
        public int total { get; set; }
        public int ky { get; set; }
        public string? date { get; set; }
    }
}
