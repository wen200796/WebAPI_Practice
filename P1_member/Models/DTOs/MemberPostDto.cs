namespace P1_member.Models.DTOs
{
    public class MemberPostDto:MemberBaseDto
    {
        public new DateTime fRegisterDatetime { get; set; }= DateTime.Now;
        public new string fPassword { get; set; } = "無";
         public new bool fEmailVerification { get; set; } = true;
         public new bool fGetCampaignInfo { get; set; } = true;
         public new bool fQualifiedTeacher { get; set; } = true;
         public  bool fStatus { get; set; } = true;
    }
}
