using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.GoodIssue;
using GRBusiness.GoodsReceive;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.Transfer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/AssignJob")]
    [ApiController]
    public class AssignJobController : ControllerBase
    {
        #region Assign
        [HttpPost("assign")]
        public IActionResult autobasicSuggestion([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new AssignJobViewModel();
                Models = JsonConvert.DeserializeObject<AssignJobViewModel>(body.ToString());
                var result = service.assign(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskfilter
        [HttpPost("taskfilter")]
        public IActionResult taskfilter([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.taskfilter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region confirmTask
        [HttpPost("confirmTask")]
        public IActionResult confirmTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.confirmTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region taskPopup
        [HttpPost("taskPopup")]
        public IActionResult taskPopup([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new View_PreviewTaskGRViewModel();
                Models = JsonConvert.DeserializeObject<View_PreviewTaskGRViewModel>(body.ToString());
                var result = service.taskPopup(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region autoGoodTaskGRNo
        [HttpPost("autoGoodTaskGRNo")]
        public IActionResult autoGoodIssueNo([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodTaskGRNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region autoGoodsReceiveNo
        [HttpPost("autoGoodsReceiveNo")]
        public IActionResult autoGoodsReceiveNo([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new ItemListViewModel();
                Models = JsonConvert.DeserializeObject<ItemListViewModel>(body.ToString());
                var result = service.autoGoodsReceiveNo(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion


        #region CheckTagTask
        [HttpPost("CheckTagTask")]
        public IActionResult CheckTagTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.CheckTagTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region CheckTaskSuccess
        [HttpPost("CheckTaskSuccess")]
        public IActionResult CheckTaskSuccess([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.CheckTaskSuccess(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region deleteTask
        [HttpPost("deleteTask")]
        public IActionResult deleteTask([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new TaskfilterViewModel();
                Models = JsonConvert.DeserializeObject<TaskfilterViewModel>(body.ToString());
                var result = service.deleteTask(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
