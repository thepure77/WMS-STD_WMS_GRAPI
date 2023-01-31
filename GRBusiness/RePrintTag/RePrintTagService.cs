using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GRBusiness.LPNItem
{
    public class RePrintTagService
    {
        private GRDbContext db;

        public RePrintTagService()
        {
            db = new GRDbContext();
        }

        public RePrintTagService(GRDbContext db)
        {
            this.db = db;
        }

        public List<LPNItemViewModel> FilterTagItem(SearchGRModel data)
        {
            try
            {
                var result = new List<LPNItemViewModel>();

                var queryResult = db.wm_TagItem.AsQueryable();

                if (!string.IsNullOrEmpty(data.goodsReceive_No))
                {
                    var query = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No == data.goodsReceive_No).FirstOrDefault();

                    queryResult = queryResult.Where(c => c.GoodsReceive_Index == query.GoodsReceive_Index && c.Tag_Status != -1 && (c.Process_Index == new Guid("58400298-4347-488C-AF71-76B78A44232D") || c.Process_Index == new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA"))).OrderBy(o => o.Tag_No);
                }

                if (!string.IsNullOrEmpty(data.tag_No))
                {
                    queryResult = queryResult.Where(c => c.Tag_No == data.tag_No && c.Tag_Status != -1);
                }

                if (data.product_Index != new Guid("00000000-0000-0000-0000-000000000000") && data.product_Index != null)
                {
                    queryResult = queryResult.Where(c => c.Product_Index == data.product_Index);
                }

                if (data.location_Index != new Guid("00000000-0000-0000-0000-000000000000") && data.location_Index != null)
                {
                    queryResult = queryResult.Where(c => c.Suggest_Location_Index == data.location_Index);
                }

                var queryResultItem = queryResult.ToList();

                var queryGIL = db.IM_GoodsReceiveItemLocation.Where(c => queryResultItem.Select(s => s.GoodsReceive_Index).Contains(c.GoodsReceive_Index)).ToList();

                var filterModel = new ProcessStatusViewModel();
                filterModel.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");

                var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());

                foreach (var items in queryResultItem)
                {
                    var model = new LPNItemViewModel();
                    model.tagItem_Index = items.TagItem_Index;
                    model.tag_Index = items.Tag_Index;
                    model.tag_No = items.Tag_No;
                    model.goodsReceive_Index = items.GoodsReceive_Index;
                    model.goodsReceiveItem_Index = items.GoodsReceiveItem_Index;
                    model.product_Index = items.Product_Index;
                    model.product_Id = items.Product_Id;
                    model.product_Name = items.Product_Name;
                    model.product_SecondName = items.Product_SecondName;
                    model.product_ThirdName = items.Product_ThirdName;
                    model.product_Lot = items.Product_Lot;
                    model.itemStatus_Index = items.ItemStatus_Index;
                    model.itemStatus_Id = items.ItemStatus_Id;
                    model.itemStatus_Name = items.ItemStatus_Name;
                    model.suggest_Location_Index = items.Suggest_Location_Index;
                    model.suggest_Location_Id = items.Suggest_Location_Id;
                    model.suggest_Location_Name = items.Suggest_Location_Name;
                    model.qty = string.Format(String.Format("{0:N0}", items.Qty));
                    model.ratio = items.Ratio;
                    model.totalQty = items.TotalQty;
                    model.productConversion_Index = items.ProductConversion_Index;
                    model.productConversion_Id = items.ProductConversion_Id;
                    model.productConversion_Name = items.ProductConversion_Name;
                    model.weight = string.Format(String.Format("{0:N3}", items.Weight));
                    model.volume = string.Format(String.Format("{0:N3}", items.Volume));
                    model.mFG_Date = items.MFG_Date;
                    model.eXP_Date = items.EXP_Date;
                    model.tagRef_No1 = items.TagRef_No1;
                    model.tagRef_No2 = items.TagRef_No2;
                    model.tagRef_No3 = items.TagRef_No3;
                    model.tagRef_No4 = items.TagRef_No4;
                    model.tagRef_No5 = items.TagRef_No5;
                    model.tag_Status = items.Tag_Status;
                    model.uDF_1 = items.UDF_1;
                    model.uDF_2 = items.UDF_2;
                    model.uDF_3 = items.UDF_3;
                    model.uDF_4 = items.UDF_4;
                    model.uDF_5 = items.UDF_5;
                    model.create_By = items.Create_By;
                    model.create_Date = items.Create_Date;
                    model.update_By = items.Update_By;
                    model.update_Date = items.Update_Date;
                    model.cancel_By = items.Cancel_By;
                    model.cancel_Date = items.Cancel_Date;

                    if (queryGIL.Count > 0)
                    {
                        var resultGIL = queryGIL.Where(c => c.GoodsReceiveItem_Index == model.goodsReceiveItem_Index && c.TagItem_Index == model.tagItem_Index).FirstOrDefault();
                        if (resultGIL != null)
                        {
                            if (resultGIL.Putaway_Status == null)
                            {
                                var Putaway_Status = 0.ToString();
                                model.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                            }
                            else
                            {
                                var Putaway_Status = resultGIL.Putaway_Status.ToString();
                                model.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                            }

                        }
                    }
                    else
                    {
                        var Putaway_Status = 0.ToString();
                        model.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                    }
                    result.Add(model);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
