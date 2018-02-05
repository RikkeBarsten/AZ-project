using System;
using System.Collections.Generic;
using System.Linq;
using AZ_project.Model.DB;
using System.Text;
using System.IO;

namespace AZ_project.Model
{
    public class EmployeeAntalRepository : IEmployeeAntalRepository
    {
        //Refactor - use Dependency Injection - the repository needs a constructor taking an
        //instance of AZContext as argument....-    
        AZContext db = new AZContext();
        
        public string GetFuldtid()
            {
                // Get data from EF - group and sum into aggregated datastructure
                var employeesFuldtid = db.Analyse_Fuldtid
                .Where(e => e.År == 2017)
                .Select (e => new {
                    Year = e.År,
                    Virksomhedsområde = e.Virksomhedsområde,
                    Stillingskategori = e.Stillingskategori,
                    Gender  = e.Køn,
                    Fuldtid = e.Fuldtid
                })
                .GroupBy(l => new { l.Year, l.Virksomhedsområde, l.Stillingskategori, l.Gender})
                .Select (g => new {
                    Year = g.Key.Year,
                    Virksomhedsområde = g.Key.Virksomhedsområde,
                    Stillingskategori = g.Key.Stillingskategori,
                    Gender = g.Key.Gender,
                    Fuldtid = g.Sum(e => Math.Round(Convert.ToDecimal(e.Fuldtid),2))
                });

                //Create stringbuilder instance to handle csv-stringbuilding
                StringBuilder CsvFuldtidSB = new StringBuilder();

                //Create headlines
                CsvFuldtidSB.AppendLine("Year;Virk;Kat;Gender;Fuldtid");

                //Create line for each MA-nr in list:
                foreach (var line in employeesFuldtid)
                {
                    CsvFuldtidSB.AppendLine(
                        line.Year.ToString() + ";" +
                        line.Virksomhedsområde + ";" +
                        line.Stillingskategori + ";" +
                        line.Gender + ";" +
                        line.Fuldtid.ToString()
                    );
                }
                 
                return CsvFuldtidSB.ToString();
            }

        public string GetFuldtid(bool csv)
        {
            String fuldtidString = String.Empty;
            
            if (!csv)
            {
                fuldtidString = GetFuldtid();
            }
            else
            {
                fuldtidString = File.ReadAllText("wwwroot/data/agg_fuldtid.csv", Encoding.UTF7);
            }

            return fuldtidString;
            
        }
        
        public string GetAntal() {
            
            var employeesAntal = db.Analyse_Antal
            .Where(e => e.År == 2017)
            .Select (e => new {
                    Year = e.År,
                    Virksomhedsområde = e.Virksomhedsområde,
                    Gender  = e.Køn,
                    Stillingskategori = e.Stillingskategori,
                    Arbejdstid = e.Arbejdstid,
                    MA_nr = e.MA_nr
                })
                .GroupBy(l => new { l.Year, l.Virksomhedsområde, l.Gender, l.Stillingskategori, l.Arbejdstid})
                .Select (g => new {
                    Year = g.Key.Year,
                    Virksomhedsområde = g.Key.Virksomhedsområde,
                    Gender = g.Key.Gender,
                    Stillingskategori = g.Key.Stillingskategori,
                    Arbejdstid = g.Key.Arbejdstid,
                    Antal = g.Select(m => m.MA_nr).Distinct().Count()
                });

            StringBuilder CsvAntalSB = new StringBuilder();

            CsvAntalSB.AppendLine("Year;Virk;Gender;Kat;Arbejdstid;Antal");

                //Create line for each MA-nr in list:
                foreach (var line in employeesAntal)
                {
                    CsvAntalSB.AppendLine(
                        line.Year.ToString() + ";" +
                        line.Virksomhedsområde + ";" +
                        line.Gender + ";" +
                        line.Stillingskategori + ";" +
                        line.Arbejdstid + ";" +
                        line.Antal.ToString()
                       
                    );
                }


            return CsvAntalSB.ToString();
        }

        public string GetAntal(bool csv){

            String antalString = String.Empty;
            
            if (!csv)
            {
                antalString = GetAntal();
            }
            else
            {
                antalString = File.ReadAllText("wwwroot/data/agg_antal.csv", Encoding.UTF7);
            }

            return antalString;
        }
        
        
        
        
        
        
        
        
        
        
        
        private List<EmployeeAntalDTO> GetHardCodedList ()
        {
            List<EmployeeAntalDTO> Hardcoded = new List<EmployeeAntalDTO>();

            Hardcoded.Add(
                new EmployeeAntalDTO()
                    { MA_nr = "1",
                        Køn = "Kvinde",
                        Alder = 46,
                        Arbejdstid = "Deltid",
                        Virksomhedsområde = "Koncern HR",
                        Fuldtid = 0.84,
                        År = 2016 }
            );

            Hardcoded.Add(
                new EmployeeAntalDTO()
                    { MA_nr = "2",
                        Køn = "Kvinde",
                        Alder = 34,
                        Arbejdstid = "Heltid",
                        Virksomhedsområde = "Koncern HR",
                        Fuldtid = 1,
                        År = 2016 }
            );

            Hardcoded.Add(
                new EmployeeAntalDTO()
                    { MA_nr = "3",
                        Køn = "Mand",
                        Alder = 52,
                        Arbejdstid = "Heltid",
                        Virksomhedsområde = "Koncern HR",
                        Fuldtid = 1,
                        År = 2016 }
            );

            return Hardcoded;
        }
    }
}

