using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness;
using GRBusiness.GoodsReceive;
using Microsoft.AspNetCore.Mvc;

namespace GRAPI.Controllers
{
    [Route("api/Json")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        // GET api/values
        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            try
            {
                if (id == "PlanGoodsReceiveService")
                {
                    var Models = new PlanGoodsReceiveService();

                    return Ok(Models);
                   
                }
                else if (id == "GoodsReceiveDocViewModel")
                {
                    var Models = new GoodsReceiveDocViewModel();
                    Models.goodsReceive_Index = Guid.NewGuid();
                    var Item = new GoodsReceiveItemViewModel();
                    Item.goodsReceiveItem_Index = Guid.NewGuid();
                    var itemDetail = new List<GoodsReceiveItemViewModel>();
                  
                    itemDetail.Add(Item);
                    Models.listGoodsReceiveItemViewModels = itemDetail;
                    return Ok(Models);

                }
                else if (id == "GoodsReceiveTagItemViewModel")
                {
                    var Models = new GoodsReceiveTagItemViewModel();

                    return Ok(Models);

                }
                else if (id == "CheckReceiveQtyViewModel")
                {
                    var Models = new CheckReceiveQtyViewModel();

                    return Ok(Models);

                }
                else if (id == "GoodsReceiveConfirmViewModel")
                {
                    var Models = new GoodsReceiveConfirmViewModel();

                    return Ok(Models);

                }
                else
                {
                    return Ok();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
