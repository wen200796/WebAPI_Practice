namespace P1_member.Models.DTOs
{
    public class MemberParameterDto
    {
        public string? KeyWord {  get; set; }
        public DateTime? Start {  get; set; }
        public DateTime? End { get; set; }
        public bool ? IsTeacher { get; set; }
    }
}
