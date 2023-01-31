using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BomBusiness;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRBusiness.LPN;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlanGRBusiness.ViewModels;
using static PlanGRBusiness.ViewModels.PlanGoodsReceivePopupViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/GoodsReceive")]
    public class GoodsReceivePopupController : Controller
    {
        [HttpPost("PlanGRfilterPopup")]
        public IActionResult PlanGRfilterPopup([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new PlanGoodsReceivePopupViewModels();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceivePopupViewModels>(body.ToString());
                var result = service.PlanGRfilterPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ProductFilterPopup")]
        public IActionResult ProductPopupFilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ProductViewModel();
                Models = JsonConvert.DeserializeObject<ProductViewModel>(body.ToString());
                var result = service.ProductFilterPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ProductLineitemPopup")]
        public IActionResult ProductLineitemPopup([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new PlanGoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveItemViewModel>(body.ToString());
                var result = service.ProductLineitemPopup(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region popupGoodsReceivefilter
        [HttpPost("popupGoodsReceivefilter")]
        public IActionResult popupGoodsIssuefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new SearchGRModel();
                Models = JsonConvert.DeserializeObject<SearchGRModel>(body.ToString());
                var result = service.popupGoodsReceivefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region popupBomIfilter
        [HttpPost("popupBomIfilter")]
        public IActionResult popupBomIfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new BomViewModel();
                Models = JsonConvert.DeserializeObject<BomViewModel>(body.ToString());
                var result = service.popupBomIfilter(Models);
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

