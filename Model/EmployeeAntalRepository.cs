using System;
using System.Collections.Generic;


namespace AZ_project.Model
{
    public class EmployeeAntalRepository : IEmployeeAntalRepository
    {
        public List<EmployeeAntal> GetAll()
        {
            List<EmployeeAntal> EmployeeAntalList = new List<EmployeeAntal>();

            //First: hard-coded test implementation:
            EmployeeAntalList = GetHardCodedList();

            //Second: EF - DB First implementation:


            //Third: CSV - implmentation:


            return EmployeeAntalList;
        }

        private List<EmployeeAntal> GetHardCodedList ()
        {
            List<EmployeeAntal> Hardcoded = new List<EmployeeAntal>();

            Hardcoded.Add(
                new EmployeeAntal()
                    { MA_nr = "1",
                        Køn = "Kvinde",
                        Alder = 46,
                        Arbejdstid = "Deltid",
                        Virksomhedsområde = "Koncern HR",
                        Fuldtid = 0.84m,
                        År = 2016 }
            );

            Hardcoded.Add(
                new EmployeeAntal()
                    { MA_nr = "2",
                        Køn = "Kvinde",
                        Alder = 34,
                        Arbejdstid = "Heltid",
                        Virksomhedsområde = "Koncern HR",
                        Fuldtid = 1,
                        År = 2016 }
            );

            Hardcoded.Add(
                new EmployeeAntal()
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

