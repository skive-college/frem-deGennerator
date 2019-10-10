using SkiveKomunefremødeGennerator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiveKomunefremødeGennerator.Helpers
{
    public class DB
    {
        

        public static List<Student> GetStudents()
        {
            List<Student> s = new List<Student>();

            try
            {
                string queryString = "SELECT ID, Name FROM Students";
                using (SqlConnection cnn = new SqlConnection(aspplanconnectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, cnn);
                    cnn.Open();
                    using (SqlDataReader oReader = cmd.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            Student st = new Student
                            {
                                ID = int.Parse(oReader["ID"].ToString()),
                                Name = oReader["Name"].ToString()
                            };
                            s.Add(st);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return s;
        }

        public static List<DagsRegistrering> getDagsReg(Student s, DateTime? from, DateTime? to)
        {
            List<DagsRegistrering> retur = new List<DagsRegistrering>();

            try
            {
                string queryString = "SET DATEFORMAT dmy; SELECT * from presents where StudentID = @ID AND @FromDate <= Date AND Date <= @ToDate";
                using (SqlConnection cnn = new SqlConnection(aspplanconnectionString))
                {
                    SqlCommand cmd = new SqlCommand(queryString, cnn);
                    cnn.Open();
                    cmd.Parameters.AddWithValue("@ID", s.ID.ToString());
                    cmd.Parameters.AddWithValue("@FromDate", from.ToString());
                    cmd.Parameters.AddWithValue("@ToDate", to.ToString());
                    using (SqlDataReader oReader = cmd.ExecuteReader())
                    {

                        while (oReader.Read())
                        {
                            string[] typeNavne = new string[4];
                            typeNavne[0] = getTypeName(int.Parse(oReader["Model1"].ToString()));
                            typeNavne[1] = getTypeName(int.Parse(oReader["Model2"].ToString()));
                            typeNavne[2] = getTypeName(int.Parse(oReader["Model3"].ToString()));
                            typeNavne[3] = getTypeName(int.Parse(oReader["Model4"].ToString()));
                            //realtid = 0, Sygdom = 1, UFravær = 2, Lfravær = 3
                            double[] counts = CalcTime(typeNavne);
                            DagsRegistrering dr = new DagsRegistrering
                            {
                                ElevNavn = s.Name,
                                Dato = DateTime.Parse(oReader["Date"].ToString()),
                                NormTimer = 5,
                                RealTid = counts[0],
                                Sygdom = counts[1],
                                UlovligFraværd = counts[2],
                                LovligFraværd = counts[3]

                            };
                            retur.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retur;
        }

        public static string getTypeName(int ID)
        {
            string retur = "";
            string queryString = "SELECT * FROM RegistrationTypes where ID = @ID ";
            using (SqlConnection cnn = new SqlConnection(aspplanconnectionString))
            {
                SqlCommand cmd = new SqlCommand(queryString, cnn);
                cnn.Open();
                cmd.Parameters.AddWithValue("@ID", ID.ToString());

                using (SqlDataReader oReader = cmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        retur = oReader["TypeName"].ToString();
                    }

                }
            }
            return retur;
        }
        private static double[] CalcTime(String[] typeNavne)
        {
            //realtid = 0, Sygdom = 1, UFravær = 2, Lfravær = 3
            double[] counts = new double[4];
            for(int i = 0; i < typeNavne.Length; i++)
            {
                switch(typeNavne[i])
                {
                    case "Fri":
                        counts[3]+=1.25;
                        break;
                    case "Syg":
                        counts[1] += 1.25;
                        break;
                    case "Udeblevet":
                    case "Ikke set":
                        counts[2] += 1.25;
                        break;
                    case "Aktiv":
                    case "Inaktiv":
                    case "vfu":
                    case "Forsent":
                        counts[0] += 1.25;
                        break;

                }
            }
            return counts;
        }
        

        
        private static string aspplanconnectionString = "Data Source=planner.aspitweb.dk;Initial Catalog=AspitPlanner;User id=SA;Password=is10lAlle;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private static string testdbconnectionString = "Data Source=planner.aspitweb.dk;Initial Catalog=TestDB;User id=SA;Password=is10lAlle;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
       
        
    }
}
