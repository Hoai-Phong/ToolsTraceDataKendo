using System;
using System.Collections.Generic;

namespace vietlott;

public partial class ResultKeno
{
    public int NextKy { get; set; }

    public int LiveKy { get; set; }

    public string LiveDate { get; set; } = null!;

    public string Result { get; set; } = null!;

    public string DateResult { get; set; } = null!;

    public DateTime? CreateDate { get; set; }

    public int Id { get; set; }
}
