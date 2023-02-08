using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vietlott.DataAccess.Entities
{
    public partial class KenoResult
    {
        public override string ToString()
        {
            return $"[{Ky}] [{ResultTime.ToString("yyyy/MM/dd HH:mm")}] {Result}";
        }
    }
}
