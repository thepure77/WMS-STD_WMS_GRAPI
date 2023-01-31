using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRBusiness.LPN;
using GRBusiness.PlanGoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/GoodsReceive")]
    public class GoodsReceiveAutoCompleteController : Controller
    {
        [HttpPost("PlanGRfilter")]
        public IActionResult PlanGRfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.PlanGRfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Ownerfilter")]
        public IActionResult Ownerfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.Ownerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoGI_SUB")]
        public IActionResult autoGI_SUB([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGI_SUB(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoPlanGI_SUB")]
        public IActionResult autoPlanGI_SUB([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoPlanGI_SUB(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Warehousefilter")]
        public IActionResult Warehousefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.Warehousefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Statusfilter")]
        public IActionResult Statusfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.Statusfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DocumentTypefilter")]
        public IActionResult DocumentTypefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.DocumentTypefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoSKU")]
        public IActionResult autoSKU([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoSKU(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoProduct")]
        public IActionResult autoProduct([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoProduct(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("VahicleTypefilter")]
        public IActionResult VahicleTypefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.VahicleTypefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("AutoFilterGoodsReceive")]
        public IActionResult AutoFilterGoodsReceive([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.AutoFilterGoodsReceive(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DockDoorfilter")]
        public IActionResult DockDoorfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.DockDoorfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ContainerTypefilter")]
        public IActionResult ContainerTypefilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.ContainerTypefilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autobasicSuggestion")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autobasicSuggestion(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoVenderfilter")]
        public IActionResult Venderfilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.Venderfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoWHOwnerfilter")]
        public IActionResult autoSoldTofilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoWHOwnerfilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoInvoice")]
        public IActionResult autoInvoice([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoInvoice(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoProductLot")]
        public IActionResult autoProductLot([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoProductLot(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoDocumentRef")]
        public IActionResult autoDocumentRef([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoDocumentRef(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoTag")]
        public IActionResult autoTag([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoTag(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("autoGR")]
        public IActionResult autoGR([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGr(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region autoLocationFilter
        [HttpPost("autoLocationFilter")]
        public IActionResult autoLocationFilter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoLocationFilter(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Task_GR_Update
        [HttpPost("Task_GR_Update")]
        public IActionResult Task_GR_Update([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.Task_GR_Update(Models);
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


