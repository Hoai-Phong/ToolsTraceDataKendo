using System;
using System.Collections.Generic;

namespace Vietlott.DataAccess.Entities;

public partial class KenoResult
{
    public int Ky { get; set; }

    public DateTime ResultTime { get; set; }

    public string Result { get; set; } = null!;

    public override string ToString()
    {
        return $"[{Ky}] [{ResultTime.ToString("yyyy/MM/dd HH:mm")}] {Result}";
    }

}
