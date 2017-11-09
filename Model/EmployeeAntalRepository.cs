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
        public string GetAll()
        {
            

            //First: hard-coded test implementation:
            //EmployeeAntalList = GetHardCodedList();

            //Second: EF - DB First implementation:
            AZContext db = new AZContext();
            var employees = from e in db.Analyse_Antal
                            select e;


            // https://www.exceptionnotfound.net/entity-framework-and-wcf-mapping-entities-to-dtos-with-automapper/
            // Mapper and EmployeeAntalDTO wont be used, as I decided to pass csv-strings from model to pages - these are easier to work with front-end (d3)
         
            /* List<EmployeeAntalDTO> EmployeeAntalList = new List<EmployeeAntalDTO>();
            
            Mapper.Initialize(cfg => cfg.CreateMap<Analyse_Antal, EmployeeAntalDTO>());

            var EmployeeList = Mapper.Map<IEnumerable<Analyse_Antal>, IEnumerable<EmployeeAntalDTO>>(employees);

            EmployeeAntalList = EmployeeList.ToList(); */
            
            StringBuilder CsvSB = new StringBuilder();

            //Create headlines
            CsvSB.AppendLine("Ma_nr, Køn, Alder, Arbejdstid, Virksomhedsområde, Overenskomst, Fuldtid, År");

            //Create line for each MA-nr in list:
            foreach (var ma in employees)
            {
                CsvSB.AppendLine(
                    ma.MA_nr + "," +
                    ma.Køn + "," +
                    ma.Alder.ToString() + "," +
                    ma.Arbejdstid + "," +
                    ma.Virksomhedsområde + "," +
                    ma.Overenskomst + "," +
                    ma.Fuldtid.ToString() + "," +
                    ma.År.ToString()
                );
            }
                        
                        

            
            //Third: CSV - implmentation:


            return CsvSB.ToString();
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

