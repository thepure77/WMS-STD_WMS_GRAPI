using Business.Library;
using Comone.Utils;
using GRBusiness.ConfigModel;
using System;
using System.Collections.Generic;

namespace GRBusiness.dropdownService
{
    public class dropdownService
    {
        public List<OwnerViewModelV2> dropdownOwner(OwnerViewModelV2 data)
        {
            try
            {
                var result = new List<OwnerViewModelV2>();

                //var filterModel = new OwnerViewModel();

                //if (!string.IsNullOrEmpty(data.OwnerIndex.ToString()))
                //{
                //    filterModel.OwnerIndex = data.OwnerIndex;
                //}
                //GetConfig
                result = utils.SendDataApi<List<OwnerViewModelV2>>(new AppSettingConfig().GetUrl("dropdownOwner"), data.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DocumentTypeViewModel> dropdownDocumentType(DocumentTypeViewModel data)
        {
            try
            {
                var result = new List<DocumentTypeViewModel>();

                var filterModel = new DocumentTypeViewModel();


                filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");


                //GetConfig
                result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
