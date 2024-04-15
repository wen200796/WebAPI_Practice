namespace P1_member.Models.DTOs
{
    public class PorkTransDto
    {
        public  double TransNum_AvgPrice { get; set; }

        public  double SpecPig_AvgPrice { get; set; }
        
        public  double AvgPrice_95in_115in { get; set; }
        
        public  double AvgPrice_75in_95 { get; set; }
        
        public  double AvgPrice_115up { get; set; }
        
        public  double AvgPrice_75low { get; set; }
        
        public  double OutPigs_AvgPrice { get; set; }
        
        public  double OtherPigs_AvgPrice { get; set; }
        
        public  double FreezerPigs_AvgPrice { get; set; }
        
        public  double TotalTrans_ExcludeFreezer_AvgPrice { get; set; }

    }
}
