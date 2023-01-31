using System;
using DataAccess;
using GRBusiness;
using GRBusiness.GoodsReceive;
using GRBusiness.GoodsReceiveItem;
using GRBusiness.LPN;
using GRBusiness.LPNItem;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlanGRAPI.Controllers
{
    [Route("api/FilterTable")]
    [ApiController]
    public class FilterTableController : ControllerBase
    {


        #region im_GoodsReceive
        [HttpPost("im_GoodsReceive")]
        public IActionResult im_PlanGoodsReceive([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_GoodsReceive(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region im_GoodsReceiveItem
        [HttpPost("im_GoodsReceiveItem")]
        public IActionResult im_GoodsReceiveItem([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveItemService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_GoodsReceiveItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region im_GoodsReceiveItemLocation
        [HttpPost("im_GoodsReceiveItemLocation")]
        public IActionResult im_GoodsReceiveItemLocation([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveItemService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.im_GoodsReceiveItemLocation(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region wm_Tag
        [HttpPost("wm_Tag")]
        public IActionResult wm_Tag([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.wm_Tag(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region wm_TagItem
        [HttpPost("wm_TagItem")]
        public IActionResult wm_TagItem([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.wm_TagItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region View_RPT_PrintOutTag
        [HttpPost("View_RPT_PrintOutTag")]
        public IActionResult View_RPT_PrintOutTag([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
                var Models = new DocumentViewModel();
                Models = JsonConvert.DeserializeObject<DocumentViewModel>(body.ToString());
                var result = service.View_RPT_PrintOutTag(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
    }
}