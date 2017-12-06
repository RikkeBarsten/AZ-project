using System;
using System.Collections.Generic;
using System.Linq;
using AZ_project.Model.DB;
using AutoMapper;
using System.Text;

namespace AZ_project.Model
{
    public class EmployeeAntalRepository : IEmployeeAntalRepository
    {
        //Refactor - use Dependency Injection - the repository needs a constructor taking an
        //instance of AZContext as argument....-    
        AZContext db = new AZContext();
        public string GetAll()
        {
            

            //First: hard-coded test implementation (needs string refactoring):
            //EmployeeAntalList = GetHardCodedList();

            //Second: EF - DB First implementation:
            
            var employees = from e in db.Analyse_Fuldtid
                            select e;


            // https://www.exceptionnotfound.net/entity-framework-and-wcf-mapping-entities-to-dtos-with-automapper/
            // Mapper and EmployeeAntalDTO wont be used, as I decided to pass csv-strings from model to pages - these are easier to work with front-end (d3)
         
            /* List<EmployeeAntalDTO> EmployeeAntalList = new List<EmployeeAntalDTO>();
            
            Mapper.Initialize(cfg => cfg.CreateMap<Analyse_Antal, EmployeeAntalDTO>());

            var EmployeeList = Mapper.Map<IEnumerable<Analyse_Antal>, IEnumerable<EmployeeAntalDTO>>(employees);

            EmployeeAntalList = EmployeeList.ToList(); */
            
            StringBuilder CsvAllSB = new StringBuilder();

            //Create headlines
            CsvAllSB.AppendLine("Ma_nr;Køn;Alder;Virksomhedsområde;Overenskomst;Fuldtid;År");

            //Create line for each MA-nr in list:
            foreach (var ma in employees)
            {
                CsvAllSB.AppendLine(
                    ma.MA_nr + ";" +
                    ma.Køn + ";" +
                    ma.Alder.ToString() + ";" +
                    ma.Virksomhedsområde + ";" +
                    ma.Overenskomst + ";" +
                    ma.Fuldtid.ToString() + ";" +
                    ma.År.ToString()
                );
            }

                        
            //Third: CSV - implmentation:


            return CsvAllSB.ToString();
        }

        public string GetFuldtid()
            {
                
                var employeesFuldtid = db.Analyse_Fuldtid.Select (e => new {
                    Year = e.År,
                    Virksomhedsområde = e.Virksomhedsområde,
                    Gender  = e.Køn,
                    Fuldtid = e.Fuldtid
                })
                .GroupBy(l => new { l.Year, l.Virksomhedsområde, l.Gender})
                .Select (g => new {
                    Year = g.Key.Year,
                    Virksomhedsområde = g.Key.Virksomhedsområde,
                    Gender = g.Key.Gender,
                    Fuldtid = g.Sum(e => Math.Round(Convert.ToDecimal(e.Fuldtid),2))
                });


                StringBuilder CsvFuldtidSB = new StringBuilder();

                //Create headlines
                CsvFuldtidSB.AppendLine("Year;Virk;Gender;Fuldtid");

                //Create line for each MA-nr in list:
                foreach (var line in employeesFuldtid)
                {
                    CsvFuldtidSB.AppendLine(
                        line.Year.ToString() + ";" +
                        line.Virksomhedsområde + ";" +
                        line.Gender + ";" +
                        line.Fuldtid.ToString()
                       
                    );
                }
                
                
                
                return CsvFuldtidSB.ToString();
            }

        
        public string GetAntal() {
            


            StringBuilder CsvFuldtidDeltidSB = new StringBuilder();


            return CsvFuldtidDeltidSB.ToString();
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

