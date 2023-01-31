using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodIssue;
using GRBusiness.GoodsReceive;
using GRBusiness.LPNItem;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.Transfer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/RePrintTag")]
    [ApiController]
    public class RePrintTagController : ControllerBase
    {
        [HttpPost("FilterTagItem")]
        public IActionResult FilterTagItem([FromBody]JObject body)
        {
            try
            {
                var service = new RePrintTagService();
                var Models = new SearchGRModel();
                Models = JsonConvert.DeserializeObject<SearchGRModel>(body.ToString());
                var result = service.FilterTagItem(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }
}
