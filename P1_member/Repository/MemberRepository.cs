using Dapper;
using Microsoft.Data.SqlClient;
using P1_member.Models.DTOs;
using System.Data;
using System.Linq;
using System.Text;
namespace P1_member.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string? _connectString;
        public MemberRepository(IConfiguration configuration) 
        {
            _connectString = configuration.GetConnectionString("LifeShareLearn");
        }
        public async Task<IEnumerable<MemberBaseDto>> GetAll(MemberParameterDto? parameter = null)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                var memberDict = new Dictionary<int, MemberGetDto>();
                StringBuilder sql = new StringBuilder(GetSelectSQL());

                if (parameter != null)
                {
                    sql.Append(" WHERE 1 = 1");
                    if (!string.IsNullOrWhiteSpace(parameter.KeyWord))
                    {
                        sql.Append(@" AND (fRealName LIKE '%' + @KeyWord + '%'
 OR fRealName LIKE '%' + @KeyWord + '%'
 OR fShowName LIKE '%' + @KeyWord + '%'
 OR fEmail LIKE '%' + @KeyWord + '%'
 OR fPhone LIKE '%' + @KeyWord + '%'
)");
                    }
                    if(parameter.Start!= null)
                    {
                        parameter.Start = ((DateTime)parameter.Start).Date;
                        sql.Append(" AND fRegisterDatetime >= @Start");
                    }
                    if (parameter.End != null)
                    {
                        parameter.End = ((DateTime)parameter.End).Date.AddDays(1);
                        sql.Append(" AND fRegisterDatetime <= @End");
                    }                    
                    if (parameter.IsTeacher != null)
                    {
                        sql.Append(" AND fQualifiedTeacher = @IsTeacher");
                    }
                }

                var memberList =  await conn.QueryAsync<MemberGetDto, MemberWishFieldDto, MemberGetDto>(sql.ToString(), (m, mwc) => MapWishFields(m, mwc, memberDict),param:parameter, splitOn: "fMWishFieldsId");
                return memberList.Distinct();
            }
        }
        public async Task<MemberBaseDto> GetById(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                var memberDict = new Dictionary<int, MemberGetDto>();
                StringBuilder sql = new StringBuilder(GetSelectSQL());
            sql.Append(" WHERE m.fMemberId = @fMemberId");
            var parameter = new DynamicParameters();
            parameter.Add("fMemberId", id);
            var member = await conn.QueryAsync<MemberGetDto, MemberWishFieldDto, MemberGetDto>(sql.ToString(), (m, mwc) => MapWishFields(m, mwc, memberDict), param:parameter, splitOn: "fMWishFieldsId");

                if(member == null) { return null; }
                member = member.Distinct();
                if(member.Count()> 1 || member.Count() == 0) { return null; }
                return member.FirstOrDefault();
            }
        }

        private string GetSelectSQL()
        {
            return @"SELECT m.*,mwc.*,cf.fFieldName as Name FROM tMember as m 
        LEFT JOIN tMemberWishFields  as mwc
        ON m.fMemberId = mwc.fMemberId 
        LEFT JOIN tCourseFields  as cf
        ON mwc.fField_Id = cf.fField_Id";
        }

        private MemberGetDto MapWishFields(MemberGetDto m, MemberWishFieldDto mwc, Dictionary<int, MemberGetDto> memberDict)
        {
            MemberGetDto? member;
            if (!memberDict.TryGetValue(m.fMemberId, out member))
            {
                member = m;
                member.memberWishFields = new List<MemberWishFieldDto>();
                memberDict.Add(m.fMemberId, member);
            }

            if (mwc != null)
            {
                member.memberWishFields.Add(mwc);
            }

            return member;
        }

        //public async Task<IEnumerable<MemberDto>> GetAll()
        //{
        //        using (var conn = new SqlConnection(_connectString))
        //        {
        //            string sql = @"SELECT m.*,mfc.* FROM tMember as m 
        //LEFT JOIN tMemberFavCourses mfc
        //ON m.fMemberId = mfc.fMemberId ";
        //            return await conn.QueryAsync<MemberDto>(sql);
        //        }
        //    }
        public async Task<int> Add(MemberPostDto memberDto)
        {
            string sql = @"
 INSERT INTO tMember 
        (
           fRegisterDatetime,
           fRealName,
           fShowName,
           fEmail,
           fPhone,
           fPassword,
          fEmailVerification,
          fGetCampaignInfo,
          fQualifiedTeacher,
          fStatus
) 
        VALUES 
        (
           @fRegisterDatetime,
           @fRealName,
           @fShowName,
           @fEmail,
           @fPhone,
           @fPassword,
          @fEmailVerification,
          @fGetCampaignInfo,
          @fQualifiedTeacher,
          @fStatus
        );
       SELECT @@IDENTITY;
";
            using (var conn = new SqlConnection(_connectString))
            {
                int id = await conn.QueryFirstOrDefaultAsync<int>(sql, memberDto);
                return id;
            }
        }

        public async Task<bool> Delete(int id)
        {
            string sql = @"
DELETE FROM tMember
WHERE fMemberId = @Id
";
            var parameters = new DynamicParameters(new{Id= id});

            using (var conn = new SqlConnection(_connectString))
            {
                int affectRows = await conn.ExecuteAsync(sql, parameters);
                return affectRows > 0;
            }
        }





        public async Task<bool> Update(int id, MemberBaseDto memberDto)
        {
            string sql = @"
UPDATE tMember
SET 
    fRealName  = @fRealName,
           fShowName = @fShowName,
           fEmail = @fEmail,
           fPhone =@fPhone
WHERE fMemberId = @Id
";
            var parameters = new DynamicParameters(memberDto);
            parameters.Add("Id", id);
            using (var conn = new SqlConnection(_connectString))
            {
                int affectRows = await conn.ExecuteAsync(sql, parameters);
                return affectRows > 0;
            }
        }
    }
}
