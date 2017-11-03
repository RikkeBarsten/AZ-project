
namespace AZ_project.Model
{
    public class EmployeeAntalDTO
    {
        public string MA_nr { get; set; }   
        public string Køn { get; set; }
        public int? Alder { get; set; }
        public string Arbejdstid { get; set; }
        public string Virksomhedsområde { get; set; }
        public string Overenskomst { get; set; }
        
        public decimal? Fuldtid { get; set; }
        public int År { get; set; }
    }
}

