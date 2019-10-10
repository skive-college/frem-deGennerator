using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiveKomunefremødeGennerator.Model
{
    public class DagsRegistrering
    {
        public DateTime Dato { get; set; }
        public String ElevNavn { get; set; }
        public int NormTimer { get; set; }
        public double RealTid { get; set; }
        public double Sygdom { get; set; }
        public double UlovligFraværd { get; set; }
        public double LovligFraværd { get; set; }
    }
}
