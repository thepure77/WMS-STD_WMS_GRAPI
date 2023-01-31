using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodsReceive;
using GRBusiness.GoodsReceiveItem;
using GRBusiness.LPN;
using GRBusiness.LPNItem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/LPNItem")]
    public class LPNItemController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public LPNItemController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("FilterTagItem")]
        public IActionResult FilterTagItem([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
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

        [HttpPost("ConfirmTagItemLocation")]
        public IActionResult ConfirmTagItemLocation([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.ConfirmTagItemLocation(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("PrintTag")]
        public IActionResult PrintTag([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                localFilePath = service.PrintTag(Models, _hostingEnvironment.ContentRootPath);
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

        [HttpPost("PrintTagA5")]
        public IActionResult PrintTagA5([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                localFilePath = service.PrintTagA5(Models, _hostingEnvironment.ContentRootPath);
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


        [HttpPost("PrintTagA3")]
        public IActionResult PrintTagA3([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                localFilePath = service.PrintTagA3(Models, _hostingEnvironment.ContentRootPath);
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


        [HttpPost("PrintTagA2")]
        public IActionResult PrintTagA2([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                localFilePath = service.PrintTagA2(Models, _hostingEnvironment.ContentRootPath);
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

        [HttpPost("DeleteTagItem")]
        public IActionResult DeleteTagItem([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.DeleteTagItem(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SugesstionLocation")]
        public IActionResult SugesstionLocation([FromBody]JObject body)
        {
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                var result = service.SugesstionLocation(Models);

                //var result = "";
                //System.Threading.Thread.Sleep(300000);
                //result = "หน่วงจริงๆ";
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ReportTagPutaway")]
        public IActionResult ReportTagPutaway([FromBody]JObject body)
        {
            string localFilePath = "";
            try
            {
                var service = new LPNItemService();
                var Models = new LPNItemViewModel();
                Models = JsonConvert.DeserializeObject<LPNItemViewModel>(body.ToString());
                localFilePath = service.ReportTagPutaway(Models, _hostingEnvironment.ContentRootPath);
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
    }
}