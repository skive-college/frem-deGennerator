﻿using SkiveKomunefremødeGennerator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace SkiveKomunefremødeGennerator.Helpers
{
    public class SkiveKomuneGenerator
    {
        
        public static string Createworddocument(List<DagsRegistrering> regs ,string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Templates\");

            string baseFolderPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string templateFilePath = System.IO.Path.Combine(path, fileName +".xlsx");
            MSExcel.Application ExcelApp = new MSExcel.Application();
            MSExcel.Workbook xlWorkBook;
            MSExcel.Worksheet xlWorkSheet;
            object missing = System.Reflection.Missing.Value;

            if (File.Exists((string)templateFilePath) && regs.Count != 0)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode AccessMode = Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange;
                    object readOnly = false;
                    object isvisible = false;
                    ExcelApp.Visible = false;

                    xlWorkBook = ExcelApp.Workbooks.Open(templateFilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    xlWorkBook.Activate();


                    xlWorkSheet = xlWorkBook.Worksheets[1];

                    xlWorkSheet.Cells[4, 4] = regs[0].ElevNavn;
                    //  object AddToMru, object TextCodepage, object TextVisualLayout, object Local

                    int row = setStartRow(regs[0].Dato);
                    int Month = regs[0].Dato.Month;
                    for (int i = 0; i < regs.Count; i++)
                    {
                        xlWorkSheet.Cells[row, 2] = regs[i].Dato;
                        xlWorkSheet.Cells[row, 3] = regs[i].NormTimer;
                        xlWorkSheet.Cells[row, 4] = regs[i].RealTid;
                        xlWorkSheet.Cells[row, 5] = regs[i].Sygdom;
                        xlWorkSheet.Cells[row, 6] = regs[i].UlovligFraværd;
                        xlWorkSheet.Cells[row, 7] = regs[i].LovligFraværd;
                        int tmp = row;
                        row = calcNextRow(row, regs[i].Dato);
                        if (regs[i].Dato.AddDays(1).Month == Month && row - 4 == tmp)
                        {
                            xlWorkSheet.Cells[tmp + 1, 2] = regs[i].Dato.AddDays(1);
                            xlWorkSheet.Cells[tmp + 1, 3] = 0;
                            xlWorkSheet.Cells[tmp + 1, 8] = "pædagogisk dag";
                        }
                        else if (i + 1 < regs.Count)
                        {
                            int FerieDage = calcFrerie(regs[i].Dato, regs[i + 1].Dato);
                            if (FerieDage > 0)
                            {
                                row = tmp + 1;
                            }
                            for (int j = 1; j < FerieDage; j++)
                            {
                                if (regs[i].Dato.AddDays(j).DayOfWeek != DayOfWeek.Saturday && regs[i].Dato.AddDays(j).DayOfWeek != DayOfWeek.Sunday)
                                {
                                    xlWorkSheet.Cells[row, 2] = regs[i].Dato.AddDays(j);
                                    xlWorkSheet.Cells[row, 3] = 0;
                                    xlWorkSheet.Cells[row, 8] = "Ferie";
                                    
                                }
                                row += 1;
                            }
                        }
                    }
                    string newfileName = "fraværdSkiveKommune" + regs[0].ElevNavn +"-" + DateTime.Now.ToString().Replace(":","-") + ".xlsx";
                    object saveas = System.IO.Path.Combine(baseFolderPath, newfileName);
                    xlWorkBook.SaveAs(saveas, missing, missing, missing
                        , missing, missing, AccessMode, missing
                        , missing, missing,  missing, missing);

                    xlWorkBook.Close();
                    ExcelApp.Quit();
                    return saveas.ToString();
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                return "";
            }
           
        }
        private static int calcFrerie(DateTime dato1, DateTime dato2)
        {
            if (dato1.DayOfWeek == DayOfWeek.Friday && dato1.AddDays(3) < dato2)
            {
                return dato2.Day - dato1.Day;
            }
            return 0;
        }
        public static void OpenExcel(string filename)
        {
            
            if (File.Exists((string)filename))
            {
                try
                {
                    MSExcel.Application xlapp;
                    MSExcel.Workbook xlworkbook;
                    xlapp = new MSExcel.Application();

                    xlworkbook = xlapp.Workbooks.Open(filename, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    xlapp.Visible = true;
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }

        

        private static int calcNextRow(int row, DateTime dato)
        {
            

            if(dato.DayOfWeek == DayOfWeek.Friday)
            {
                row += 3;
            }
            else if(dato.DayOfWeek == DayOfWeek.Thursday && Util.getWeek(dato) % 2 == 0)
            {
                row += 4;
            }
            else
            {
                row += 1;
            }

            return row;
        }

        private static int setStartRow(DateTime day)
        {
            int retur = 9;

            switch(day.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    retur += 0;
                    break;
                case System.DayOfWeek.Tuesday:
                    retur += 1;
                    break;
                case System.DayOfWeek.Wednesday:
                    retur += 2;
                    break;
                case System.DayOfWeek.Thursday:
                    retur += 3;
                    break;
                case System.DayOfWeek.Friday:
                    retur += 4;
                    break;
            }
            return retur;
        }

    }
}
