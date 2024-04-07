using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P1_member.Models.DTOs;
using P1_member.Repository;
using System.Net;

namespace P1_member.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        public MemberController(IMemberRepository memberRepository) 
        {
        _memberRepository = memberRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]MemberParameterDto? parameter)
        {
            try
            {
            var result = await _memberRepository.GetAll(parameter);
            if (result == null || result.ToList().Count==0) { return NotFound(); }
                return Ok(result);
            }catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"發生錯誤：{ex.Message}");

            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id == 0) { return BadRequest("ID為0"); }
            var result = await _memberRepository.GetById(id);
            if(result == null) { return NotFound("找不到此ID會員"); }
            return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"發生錯誤：{ex.Message}");

            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]MemberPostDto member)
        {
            try
            {
              int memberId = await _memberRepository.Add(member);
                if (memberId != 0)
                {
                    member.fMemberId = memberId;
                    return CreatedAtAction(nameof(GetById), new {id =  memberId }, member);
                }
                else
                {
                    return BadRequest("新增失敗");
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"發生錯誤：{ex.Message}");

            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MemberPostDto member)
        {
            try
            {
                if(id != member.fMemberId || id == 0)
                {
                    return BadRequest("ID有誤或不一致");
                }
                var toUpdateMember = await _memberRepository.GetById(id);
                if (toUpdateMember != null)
                {
                    var result = await _memberRepository.Update(id, member);
                    if (result)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Ok("更新但未影響任何資源");
                    }
                }
                else
                {
                    return NotFound("此會員不存在");
                }
            
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"發生錯誤：{ex.Message}");

            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                if (id == 0) { return BadRequest("ID為0"); }
                var result = await _memberRepository.Delete(id);
                if (result) 
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("刪除請求失敗或無該會員");
                }
               
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"發生錯誤：{ex.Message}");

            }

        }
    }
}
