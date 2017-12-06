

namespace AZ_project.Model.DB
{
    public class Analyse_Fuldtid
    {
       
        public string MA_nr { get; set; }
        public string Køn { get; set; }
        public double? Alder { get; set; }
        
        public string Virksomhedsområde { get; set; }

        public string Overenskomst { get; set; }
        
        public double? Fuldtid { get; set; }
        public int År { get; set; }
    }
}