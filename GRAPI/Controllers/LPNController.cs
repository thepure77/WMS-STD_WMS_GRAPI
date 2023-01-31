using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodsReceive;
using GRBusiness.GoodsReceiveItem;
using GRBusiness.LPN;
using GRBusiness.LPNItem;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/LPN")]
    public class LPNController : Controller
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    try
        //    {
        //        var service = new LPNService();

        //        var result = service.Filter();

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public IActionResult Get(Guid id)
        //{
        //    try
        //    {
        //        var service = new GoodsReceiveService();

        //        var result = service.getId(id);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        //POST api/<controller>

        [HttpPost("CheckType_Po_SubContrack")]
        public IActionResult CheckType_Po_SubContrack([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveViewModel>(body.ToString());
                var result = service.CheckType_Po_SubContrack(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateTagHeader")]
        public IActionResult CreateTagHeader([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveViewModel>(body.ToString());
                var result = service.CreateTagHeader(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateTagItems")]
        public IActionResult CreateTagItems([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.CreateTagItems(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("CreateTagItemsV2")]
        public IActionResult CreateTagItemsV2([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.CreateTagItemsV2(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateTagItemsV3")]
        public IActionResult CreateTagItemsV3([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.CreateTagItemsV3(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateTagItemsOneToMany")]
        public IActionResult CreateTagItemsOneToMany([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.CreateTagItemsOneToMany(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("filterGR")]
        public IActionResult filterGR([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new SearchGRModel();
                Models = JsonConvert.DeserializeObject<SearchGRModel>(body.ToString());
                var result = service.FilterGR(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetByGoodReceiveId/{id}")]
        public IActionResult GetByGoodReceiveId(string id)
        {
            try
            {
               var service = new LPNService();
                var result = service.GetByGoodReceiveId(id);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        #region autoTag
        [HttpPost("autoTag")]
        public IActionResult autoTag([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
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
        #endregion

        [HttpPost("createPerTagItems")]
        public IActionResult createPerTagItems([FromBody]JObject body)
        {
            try
            {
                var service = new LPNService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.createPerTagItems(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
