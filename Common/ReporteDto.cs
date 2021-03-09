using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ReporteDto
    {
        public int version { get; set; }
        public string creationDate {get; set;}
        public string updateDate {get; set;}
        public string label {get; set;}
        public string description {get; set;}
        public string uri {get; set;}
        public string resourceType {get; set;}
    }
}
