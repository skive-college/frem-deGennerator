using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkiveKomunefremødeGennerator.Helpers
{
    public class Util
    {
        public static int getWeek(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string convertWeekDay(DateTime time)
        {
            string retur = "";

            switch(time.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    retur = "Mandag";
                    break;
                case DayOfWeek.Tuesday:
                    retur = "Tirsdag";
                    break;
                case DayOfWeek.Wednesday:
                    retur = "Onsdag";
                    break;
                case DayOfWeek.Thursday:
                    retur = "Torsdag";
                    break;
                case DayOfWeek.Friday:
                    retur = "Fredag";
                    break;
            }

            return retur;
        }

        public static string ConvertPeriode(DateTime dato)
        {
            string retur = "";

            retur = getMonth(dato.Month) + " - " + dato.Year;

            return retur;
        }

        private static string getMonth(int month)
        {
            string retur = "";
            switch(month)
            {
                case 1:
                    retur = "jan";
                    break;
                case 2:
                    retur = "feb";
                    break;
                case 3:
                    retur = "mar";
                    break;
                case 4:
                    retur = "apr";
                    break;
                case 5:
                    retur = "maj";
                    break;
                case 6:
                    retur = "jun";
                    break;
                case 7:
                    retur = "jul";
                    break;
                case 8:
                    retur = "aug";
                    break;
                case 9:
                    retur = "sep";
                    break;
                case 10:
                    retur = "okt";
                    break;
                case 11:
                    retur = "nov";
                    break;
                case 12:
                    retur = "dec";
                    break;
            }
            return retur;
        }
    }
}
