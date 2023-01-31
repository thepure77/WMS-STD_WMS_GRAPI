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
    [Route("api/Matdoc_Production")]
    public class Matdoc_ProductionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Matdoc_ProductionController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
      

        [HttpPost("update_lineproduction_select")]
        public IActionResult update_lineproduction()
        {
            try
            {
                var service = new Matdoc_lineproductionService();
                var result = service.update_lineproduction_select();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("update_lineproduction_update")]
        public IActionResult update_lineproduction_update()
        {
            try
            {
                var service = new Matdoc_lineproductionService();
                var result = service.update_lineproduction_update();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    }

}

