using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using MasterDataBusiness.CargoType;
using MasterDataBusiness.ContainerType;
using MasterDataBusiness.CostCenter;
using MasterDataBusiness.Currency;
using MasterDataBusiness.DockDoor;
using MasterDataBusiness.DocumentPriority;
using MasterDataBusiness.ShipmentType;
using MasterDataBusiness.VehicleType;
using MasterDataBusiness.ViewModels;
using MasterDataBusiness.Volume;
using MasterDataBusiness.Weight;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransferBusiness.Transfer;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GRAPI.Controllers
{
    [Route("api/DropdownGoodsReceive")]
    public class GoodsReceiveDropDownController : Controller
    {
        [HttpPost("dropdownDocumentType")]
        public IActionResult dropdownDocumentType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
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

        [HttpPost("dropdownProcessStatus")]
        public IActionResult dropdownProcessStatus([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ProcessStatusViewModel();
                Models = JsonConvert.DeserializeObject<ProcessStatusViewModel>(body.ToString());
                var result = service.dropdownProcessStatus(Models);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region dropdownStorageLoc
        [HttpPost("dropdownStorageLoc")]
        public IActionResult dropdownStorageLoc([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new StorageLocViewModel();
                Models = JsonConvert.DeserializeObject<StorageLocViewModel>(body.ToString());
                var result = service.dropdownStorageLoc(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpPost("dropdownProductconversion")]
        public IActionResult dropdownProductconversion([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ProductConversionViewModelDoc();
                Models = JsonConvert.DeserializeObject<ProductConversionViewModelDoc>(body.ToString());
                var result = service.dropdownProductconversion(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        

        #region dropdownItemStatus
        [HttpPost("dropdownItemStatus")]
        public IActionResult dropdownItemStatus([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ItemStatusDocViewModel();
                Models = JsonConvert.DeserializeObject<ItemStatusDocViewModel>(body.ToString());
                var result = service.dropdownItemStatus(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownUser
        [HttpPost("dropdownUser")]
        public IActionResult dropdownUser([FromBody]JObject body)
        {
            try
            {
                var service = new AssignService();
                var Models = new UserViewModel();
                Models = JsonConvert.DeserializeObject<UserViewModel>(body.ToString());
                var result = service.dropdownUser(Models);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region dropdownCostCenter
        [HttpPost("dropdownCostCenter")]
        public IActionResult dropdownCostCenter([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new CostCenterViewModel();
                Models = JsonConvert.DeserializeObject<CostCenterViewModel>(body.ToString());
                var result = service.dropdownCostCenter(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownWeight
        [HttpPost("dropdownWeight")]
        public IActionResult dropdownWeight([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new WeightViewModel();
                Models = JsonConvert.DeserializeObject<WeightViewModel>(body.ToString());
                var result = service.dropdownWeight(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownCurrency
        [HttpPost("dropdownCurrency")]
        public IActionResult dropdownCurrency([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new CurrencyViewModel();
                Models = JsonConvert.DeserializeObject<CurrencyViewModel>(body.ToString());
                var result = service.dropdownCurrency(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion


        #region dropdownVolume
        [HttpPost("dropdownVolume")]
        public IActionResult dropdownVolume([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new VolumeViewModel();
                Models = JsonConvert.DeserializeObject<VolumeViewModel>(body.ToString());
                var result = service.dropdownVolume(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownDocumentPriority
        [HttpPost("dropdownDocumentPriority")]
        public IActionResult dropdownDocumentPriority([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new DocumentPriorityViewModel();
                Models = JsonConvert.DeserializeObject<DocumentPriorityViewModel>(body.ToString());
                var result = service.dropdownDocumentPriority(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region DropdownWarehouse
        [HttpPost("dropdownWarehouse")]
        public IActionResult DropdownWarehouse([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new WarehouseViewModel();
                Models = JsonConvert.DeserializeObject<WarehouseViewModel>(body.ToString());
                var result = service.dropdownWarehouse(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownShipmentType
        [HttpPost("dropdownShipmentType")]
        public IActionResult dropdownShipmentType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ShipmentTypeViewModel();
                Models = JsonConvert.DeserializeObject<ShipmentTypeViewModel>(body.ToString());
                var result = service.dropdownShipmentType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownCargoType
        [HttpPost("dropdownCargoType")]
        public IActionResult dropdownCargoType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new CargoTypeViewModel();
                Models = JsonConvert.DeserializeObject<CargoTypeViewModel>(body.ToString());
                var result = service.dropdownCargoType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownVehicleType
        [HttpPost("dropdownVehicleType")]
        public IActionResult dropdownVehicleType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new VehicleTypeViewModel();
                Models = JsonConvert.DeserializeObject<VehicleTypeViewModel>(body.ToString());
                var result = service.dropdownVehicleType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownContainerType
        [HttpPost("dropdownContainerType")]
        public IActionResult dropdownContainerType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new ContainerTypeViewModelV2();
                Models = JsonConvert.DeserializeObject<ContainerTypeViewModelV2>(body.ToString());
                var result = service.dropdownContainerType(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownDockDoor
        [HttpPost("dropdownDockDoor")]
        public IActionResult dropdownDockDoor([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new DockDoorViewModelV2();
                Models = JsonConvert.DeserializeObject<DockDoorViewModelV2>(body.ToString());
                var result = service.dropdownDockDoor(Models);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
        #endregion

        #region dropdownUnloadingType
        [HttpPost("dropdownUnloadingType")]
        public IActionResult dropdownUnloadingType([FromBody]JObject body)
        {
            try
            {
                var service = new GoodsReceiveService();
                var Models = new UnloadingTypeViewModel();
                Models = JsonConvert.DeserializeObject<UnloadingTypeViewModel>(body.ToString());
                var result = service.dropdownUnloadingType(Models);
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
