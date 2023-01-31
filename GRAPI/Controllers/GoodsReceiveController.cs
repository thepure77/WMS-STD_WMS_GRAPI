using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRBusiness.LPN;
using GRBusiness.PlanGoodsReceive;
using GRBusiness.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/GoodsReceive")]
    public class GoodsReceiveController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GoodsReceiveController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost("filter")]
        public IActionResult filter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new SearchGRModel();
                Models = JsonConvert.DeserializeObject<SearchGRModel>(body.ToString());
                var result = service.filter(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                var service = new GoodsReceiveService();

                var result = service.find(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        // POST api/<controller>
        [HttpPost("Create")]
        public IActionResult Create([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.CreateOrUpdate(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("get_Pocontrack")]
        public IActionResult po_contrack([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.po_contrack(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Delete")]
        public IActionResult Delete([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.Delete(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DeletePGR")]
        public IActionResult DeletePGR([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.DeletePGR(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("CheckGoodReceiveItem")]
        public IActionResult CheckGoodReceiveItem([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new PlanGoodsReceiveItemViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveItemViewModel>(body.ToString());


                var result = service.CheckGoodReceiveItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("CheckTAG")]
        public IActionResult PostCheckTAG([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveTagModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagModel>(body.ToString());


                var result = service.CheckTAG(Models.Tag_No);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CheckReceiveQtyV2")]
        public IActionResult PostCheckReceiveQtyV2([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new CheckReceiveQtyViewModel();
                Models = JsonConvert.DeserializeObject<CheckReceiveQtyViewModel>(body.ToString());
                var result = service.CheckReceiveQtyV2(Models.planGoodsReceive_Index, Models.planGoodsReceiveItem_Index, Models.product_Index.ToString(), Models.qty, Models.productConversion_Ratio, Models.productConversion_Index.ToString());

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SaveTag")]
        public IActionResult PostSaveTag([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.SaveTAGChanges(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("filterTag")]
        public IActionResult post([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new TagItemViewModel();
                Models = JsonConvert.DeserializeObject<TagItemViewModel>(body.ToString());
                var result = service.FilterTag(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("getDeleteScan")]
        public IActionResult getDeleteScan([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new TagItemViewModel();
                Models = JsonConvert.DeserializeObject<TagItemViewModel>(body.ToString());
                var result = service.getDeleteScan(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

       [HttpPost("GoodsReceiveConfirmUnPack")]
        public IActionResult GoodsReceiveConfirmUnPack([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveConfirmViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveConfirmViewModel>(body.ToString());
                var result = service.GoodsReceiveConfirmUnPack(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GoodsReceiveConfirmV3")]
        public IActionResult GoodsReceiveConfirmV3([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveConfirmViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveConfirmViewModel>(body.ToString());
                var result = service.GoodsReceiveConfirmV3(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GoodsReceiveConfirmV3Auto")]
        public IActionResult GoodsReceiveConfirmV3Auto([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
      
                var result = service.GoodsReceiveConfirmV3Auto();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("AutoScanReceive")]
        public IActionResult AutoScanReceive([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveTagItemViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemViewModel>(body.ToString());
                var result = service.AutoScanReceive(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Confirm")]
        public IActionResult Confirm([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.confirmStatus(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateScanLPN")]
        public IActionResult CreateScanLPN([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new LPNViewModel();
                Models = JsonConvert.DeserializeObject<LPNViewModel>(body.ToString());
                var result = service.CreateScanLPN(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost("updateUserAssign")]
        public IActionResult PostupdateUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.updateUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("updateUserAssignScanReceive")]
        public IActionResult updateUserAssignScanReceive([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.updateUserAssignScanReceive(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("checkUserAssign")]
        public IActionResult checkUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.checkUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }




        [HttpPost("deleteUserAssign")]
        public IActionResult deleteUserAssign([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.deleteUserAssign(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("updateUserAssignKey")]
        public IActionResult PostupdateUserAssignKey([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.updateUserAssignKey(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CheckPalletLocationLPN")]
        public IActionResult CheckPalletLocationLPN([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveTagItemPutawayLpnViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveTagItemPutawayLpnViewModel>(body.ToString());
                var result = service.CheckPalletLocationLPN(Models);


                //   var result = service.GetProductDetail(Models.Owner_Index.ToString(), Models.Product_Index);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ScanPlanGR")]
        public IActionResult ScanPlanGR([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new PlanGoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<PlanGoodsReceiveViewModel>(body.ToString());
                var result = service.ScanPlanGR(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("GetProductBarcode")]
        public IActionResult GetProductBarcode([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ProductBarcodeModel();
                Models = JsonConvert.DeserializeObject<ProductBarcodeModel>(body.ToString());
                var result = service.GetProductBarcodes(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("configGoodsReceiveItemLocation")]
        public IActionResult configGoodsReceiveItemLocation([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new View_GoodsReceiveViewModel();
                Models = JsonConvert.DeserializeObject<View_GoodsReceiveViewModel>(body.ToString());
                var result = service.configGoodsReceiveItemLocation(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("PrintGR")]
        public IActionResult PrintGR([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ReportGRModel();
                Models = JsonConvert.DeserializeObject<ReportGRModel>(body.ToString());
                localFilePath = service.PrintGR(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }

        [HttpPost("PrintGRPutaway")]
        public IActionResult PrintGRPutaway([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ReportGRPutawayModel();
                Models = JsonConvert.DeserializeObject<ReportGRPutawayModel>(body.ToString());
                localFilePath = service.PrintGRPutaway(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }

        [HttpPost("PrintReceipt")]
        public IActionResult PrintReceipt([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ReportReceiptViewModel();
                Models = JsonConvert.DeserializeObject<ReportReceiptViewModel>(body.ToString());
                localFilePath = service.PrintReceipt(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }

        [HttpPost("SentToSap")]
        public IActionResult SentToSap([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ListGoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<ListGoodsReceiveDocViewModel>(body.ToString());
                var result = service.SentToSap(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("SentToSapGetJson/{id}")]
        public IActionResult SentToSapGetJson(string id)
        {
            try
            {
                var service = new GoodsReceiveService();
                var result = service.SentToSapGetJson(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #region genDocumentNo
        [HttpPost("genDocumentNo")]
        public IActionResult genDocumentNo([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.genDocumentNo(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region findUser
        [HttpPost("findUser")]
        public IActionResult findUser([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new im_Signatory_logViewModel();
                Models = JsonConvert.DeserializeObject<im_Signatory_logViewModel>(body.ToString());
                var result = service.findUser(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        [HttpPost("SearchTagPutaway")]
        public IActionResult SearchTagPutaway([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new SearchTagPutawayViewModel();
                Models = JsonConvert.DeserializeObject<SearchTagPutawayViewModel>(body.ToString());
                var result = service.SearchTagPutaway(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        [HttpPost("ReportPrintOutGR")]
        public IActionResult ReportPrintOutGR([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ReportPrintOutGRViewModel();
                Models = JsonConvert.DeserializeObject<ReportPrintOutGRViewModel>(body.ToString());
                localFilePath = service.ReportPrintOutGR(Models, _hostingEnvironment.ContentRootPath);
                if (!System.IO.File.Exists(localFilePath))
                {
                    return NotFound();
                }
                return File(System.IO.File.ReadAllBytes(localFilePath), "application/octet-stream");
                //return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                System.IO.File.Delete(localFilePath);
            }
        }

        [HttpPost("updateStatusBom")]
        public IActionResult updateStatusBom([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new GoodsReceiveDocViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveDocViewModel>(body.ToString());
                var result = service.updateStatusBom(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("makeAllGr")]
        public IActionResult makeAllGr([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new SearchGRModel();
                Models = JsonConvert.DeserializeObject<SearchGRModel>(body.ToString());
                var result = service.GetAllGR(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }


    }

}

