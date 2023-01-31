using System;
using GRBusiness.ConfigModel;
using GRBusiness.dropdownService;
using GRBusiness.GoodsReceive;
using GRBusiness.ScanReceiveService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static GRBusiness.GoodsReceive.SearchScanReceiveViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/ScanReceive")]
    public class ScanReceiveController : Controller
    {

        [HttpPost("scanDN")]
        public IActionResult scanDN([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new SearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
                var result = service.scanDN(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("scanGR")]
        public IActionResult scanGR([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new SearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
                var result = service.scanGR(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("scanUPC")]
        public IActionResult scanUPC([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new SearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
                var result = service.scanUPC(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("scanUPCUnpack")]
        public IActionResult scanUPCUnpack([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new SearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
                var result = service.scanUPCUnpack(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

       [HttpPost("dropdownDocumentType")]
        public IActionResult dropdownDocumentType([FromBody]JObject body)
        {
            try
            {
                var service = new dropdownService();
                var Models = new DocumentTypeViewModel();
                Models = JsonConvert.DeserializeObject<DocumentTypeViewModel>(body.ToString());
                var result = service.dropdownDocumentType(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("saveReceive")]
        public IActionResult saveReceive([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new ResultSearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<ResultSearchScanReceiveViewModel>(body.ToString());
                var result = service.saveReceive(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("filterGRItem")]
        public IActionResult filterGRItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new SearchScanReceiveViewModel();
                Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
                var result = service.filterGRItem(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #region deleteItem
        [HttpPost("deleteItem")]
        public IActionResult deleteItem([FromBody]JObject body)
        {
            try
            {
                var service = new ScanReceiveService();
                var Models = new GoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveItemViewModel>(body.ToString());
                var result = service.deleteItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion



        //[HttpPost("dropdownOwner")]
        //public IActionResult dropdownOwner([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new dropdownService();
        //        var Models = new OwnerViewModelV2();
        //        Models = JsonConvert.DeserializeObject<OwnerViewModelV2>(body.ToString());
        //        var result = service.dropdownOwner(Models);
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message);
        //    }
        //}



        //[HttpPost("scanReceivePlangGR")]
        //public IActionResult scanReceivePlangGR([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new ScanReceiveService();
        //        var Models = new SearchScanReceiveViewModel();
        //        Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
        //        var result = service.scanReceivePlangGR(Models);
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("scanReceiveGR")]
        //public IActionResult scanReceiveGR([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new ScanReceiveService();
        //        var Models = new SearchScanReceiveViewModel();
        //        Models = JsonConvert.DeserializeObject<SearchScanReceiveViewModel>(body.ToString());
        //        var result = service.scanReceiveGR(Models);
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex.Message);
        //    }
        //}

        //[HttpPost("saveTagscanReceive")]
        //public IActionResult SaveTagscanReceive([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new ScanReceiveService();
        //        var Models = new ScanReceiveViewModel();
        //        Models = JsonConvert.DeserializeObject<ScanReceiveViewModel>(body.ToString());
        //        var result = service.SaveTagscanReceive(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        //[HttpPost("scanReceiveProduct")]
        //public IActionResult ScanReceiveProduct([FromBody]JObject body)
        //{
        //    try
        //    {
        //        var service = new ScanReceiveService();
        //        var Models = new ScanReceiveViewModel();
        //        Models = JsonConvert.DeserializeObject<ScanReceiveViewModel>(body.ToString());
        //        var result = service.ScanReceiveProduct(Models);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}
