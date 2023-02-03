using System;
using System.Collections.Generic;

namespace vietlott;

public partial class GuessResultKeno
{
    public int Id { get; set; }

    public int? LiveKy { get; set; }

    public string? GuessResult { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? Times { get; set; }

    public string? NumberSequence { get; set; }
}
