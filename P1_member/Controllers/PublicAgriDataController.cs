using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using P1_member.Models.DTOs;

namespace P1_member.Controllers
{
    [Route("api/PublicAgriData/[action]")]
    [ApiController]
    public class PublicAgriDataController : ControllerBase
    {
        private readonly string _base_url = "https://data.moa.gov.tw/api/v1/";
        [HttpGet]
        public async Task<IActionResult>GetPorkTrans(string TransDate)
        {
            string request_url = _base_url + "PorkTransType/?TransDate=" + TransDate;
            var request = new HttpRequestMessage(HttpMethod.Get, request_url);
            using (var client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    var response_text = response.Content.ReadAsStringAsync().Result;
                    JObject response_body = JObject.Parse(response_text);
                    JArray data = (JArray)response_body["Data"];
                    if (data == null || data.Count() ==0) 
                    { 
                        return NotFound();
                    }
                    IEnumerable<PorkTransDto> porkTransEnumerable = data.Select(item => JsonConvert.DeserializeObject<PorkTransDto>(item.ToString()));

                    //var response_datas = JsonConvert.DeserializeObject<IEnumerable<PorkTransDto>>(response_text);
                    // return Ok(response_datas);
                    //double sumTransNum_AvgPrice = porkTransEnumerable.Sum(p => p.TransNum_AvgPrice);
                    //return Content(data.Count().ToString());
                    //return Content(sumTransNum_AvgPrice.ToString());
                    PorkTransDto dailyAverage = new PorkTransDto
                    {
                        TransNum_AvgPrice = porkTransEnumerable.Average(p => p.TransNum_AvgPrice),
                        SpecPig_AvgPrice = porkTransEnumerable.Average(p => p.SpecPig_AvgPrice),
                        AvgPrice_95in_115in = porkTransEnumerable.Average(p => p.AvgPrice_95in_115in),
                        AvgPrice_75in_95 = porkTransEnumerable.Average(p => p.AvgPrice_75in_95),
                        AvgPrice_115up = porkTransEnumerable.Average(p => p.AvgPrice_115up),
                        AvgPrice_75low = porkTransEnumerable.Average(p => p.AvgPrice_75low),
                        OutPigs_AvgPrice = porkTransEnumerable.Average(p => p.OutPigs_AvgPrice),
                        OtherPigs_AvgPrice = porkTransEnumerable.Average(p => p.OtherPigs_AvgPrice),
                        FreezerPigs_AvgPrice = porkTransEnumerable.Average(p => p.FreezerPigs_AvgPrice),
                        TotalTrans_ExcludeFreezer_AvgPrice = porkTransEnumerable.Average(p => p.TotalTrans_ExcludeFreezer_AvgPrice)
                    };
                    //return Ok(porkTransEnumerable);
                    return Ok(dailyAverage);
                }
                return NotFound();
            }
        }
    }
}
