using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodsReceive;
using GRBusiness.GoodsReceiveImage;
using GRBusiness.GoodsReceiveItem;
using GRBusiness.LPN;
using GRBusiness.LPNItem;
using GRBusiness.Upload;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/Upload")]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost("UploadImg")]
        public IActionResult UploadImg([FromBody]JObject body)
        {
            try
            {
                var service = new UploadService();
                var Models = new GoodsReceiveImageViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveImageViewModel>(body.ToString());
                var result = service.UploadImg(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("findImg")]
        public IActionResult findImg([FromBody]JObject body)
        {
            try
            {
                var service = new UploadService();
                var Models = new GoodsReceiveImageViewModel();
                Models = JsonConvert.DeserializeObject<GoodsReceiveImageViewModel>(body.ToString());
                var result = service.findImg(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
