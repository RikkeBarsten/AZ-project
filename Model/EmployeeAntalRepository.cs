using System;
using System.Collections.Generic;
using System.Linq;
using AZ_project.Model.DB;
using AutoMapper;


namespace AZ_project.Model
{
    public class EmployeeAntalRepository : IEmployeeAntalRepository
    {
        public List<EmployeeAntalDTO> GetAll()
        {
            List<EmployeeAntalDTO> EmployeeAntalList = new List<EmployeeAntalDTO>();

            //First: hard-coded test implementation:
            //EmployeeAntalList = GetHardCodedList();

            //Second: EF - DB First implementation:
            AZContext db = new AZContext();
            var employees = from e in db.Analyse_Antal
                            select e;

            Mapper.Initialize(cfg => cfg.CreateMap<Analyse_Antal, EmployeeAntalDTO>());

            var EmployeeList = Mapper.Map<IEnumerable<Analyse_Antal>, IEnumerable<EmployeeAntalDTO>>(employees);

            EmployeeAntalList = EmployeeList.ToList();
            
            
            //Third: CSV - implmentation:


            return EmployeeAntalList;
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
                        Fuldtid = 0.84m,
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

