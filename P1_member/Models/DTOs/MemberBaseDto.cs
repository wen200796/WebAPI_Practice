using System.ComponentModel.DataAnnotations;

namespace P1_member.Models.DTOs
{
    public abstract class MemberBaseDto
    {
        public int fMemberId { get; set; }
        public DateTime fRegisterDatetime { get; set; }
        [Required(ErrorMessage ="姓名必填")]
        public string? fRealName { get; set; }
        [Required]
        public string? fShowName { get; set; }
        [Required(ErrorMessage = "信箱必填")]
        public string? fEmail { get; set; }
        public string? fPhone { get; set; }
        [Required]
        public string? fPassword { get; set; }
        [Required]
        public bool fEmailVerification { get; set; }
        [Required]
        public bool fGetCampaignInfo { get; set; }
        [Required]
        public bool fQualifiedTeacher { get; set; }
        public bool? fGender { get; set; }
        public ICollection<MemberWishFieldDto>? memberWishFields { get; set; }
    }
}
