using P1_member.Models.DTOs;

namespace P1_member.Repository
{
    public interface IMemberRepository
    {
        Task<MemberBaseDto> GetById(int id);    
         Task<IEnumerable<MemberBaseDto>> GetAll(MemberParameterDto? parameter=null);
         Task<int> Add(MemberPostDto memberDto);
        Task<bool> Update(int id, MemberBaseDto memberDto);
        Task<bool> Delete(int id);
    }
}
