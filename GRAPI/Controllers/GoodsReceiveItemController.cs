using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodsReceive;
using GRBusiness.GoodsReceiveItem;
using GRBusiness.PlanGoodsReceive;
using GRBusiness.PlanGoodsReceiveItem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GRAPI.Controllers
{
    [Route("api/GoodsReceiveItem")]
    [ApiController]
    public class GoodsReceiveItemController : ControllerBase
    {
        [HttpGet("GetByGoodReceiveId/{id}")]
        public IActionResult GetByGoodReceiveId(string id)
        {
            try
            {
                GoodsReceiveItemService service = new GoodsReceiveItemService();
                var result = service.GetByGoodReceiveId(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpGet("getPoContrackByGoodReceiveId/{id}")]
        public IActionResult getPoContrackByGoodReceiveId(string id)
        {
            try
            {
                GoodsReceiveItemService service = new GoodsReceiveItemService();
                var result = service.getPoContrackByGoodReceiveId(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("getId")]
        public IActionResult Get([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveItemService();
                var Models = new GoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveItemViewModel>(body.ToString());

                var result = service.getId(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GetPlanGoodReceivePopup")]
        public IActionResult GetPlanGoodReceivePopup([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveItemService();
                var Models = new PopupPlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<PopupPlanGoodsReceiveViewModel>(body.ToString());

                var result = service.GetPlanGoodReceivePopup(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CheckCopyItem")]
        public IActionResult CheckCopyItem([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveItemService();
                var Models = new GoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveItemViewModel>(body.ToString());

                var result = service.CheckCopyItem(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //[HttpPost("checkGR")]
        //public IActionResult checkGR([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new GoodsReceiveItemService();
        //        var Models = new GoodsReceiveItemViewModel();
        //        Models = JsonConvert.DeserializeObject<GoodsReceiveItemViewModel>(body.ToString());

        //        var result = service.checkGR(Models);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        

        //[HttpPost("CheckPlanGR")]
        //public IActionResult CheckPlanGR([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new GoodsReceiveItemService();
        //        var Models = new ItemListViewModel();
        //        Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());

        //        var result = service.CheckPlanGR(Models.key);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}