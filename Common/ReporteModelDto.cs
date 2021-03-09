using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ReporteModelDto
    {
        public ReporteDto[] resourceLookup { get; set; }
        public int Status { get; set; }
        public string Error { get; set; }
    }
}
