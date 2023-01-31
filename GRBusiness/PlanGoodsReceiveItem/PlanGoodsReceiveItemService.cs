using Comone.Utils;
using DataAccess;
using GRBusiness.GoodsReceive;
using GRBusiness.PlanGoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GRBusiness.PlanGoodsReceiveItem
{
    public class PlanGoodsReceiveItemService
    {
        public List<ViewPlanGoodsReceiveItem> GetByPlanGoodReceiveId(string id)
        {
            if (String.IsNullOrEmpty(id)) throw new NullReferenceException();

            try
            {
                var result = new List<ViewPlanGoodsReceiveItem>();

                using (var context = new GRDbContext())
                {
                    string pstring = "";
                    pstring += " and PlanGoodsReceive_Index = '" + id + "'";
                    pstring += " and Document_Status != -1";
                    var strwhere = new SqlParameter("@strwhere", pstring);

                    var queryResult = context.View_GetPlanGoodsReceiveItem.FromSql("sp_GetViewPlanGoodsReceiveItem @strwhere", strwhere).ToList();
                    
                    foreach (var data in queryResult)
                    {
                        var item = new ViewPlanGoodsReceiveItem();
                        //var items = new PlanGoodsReceiveViewModel();

                        item.PlanGoodsReceiveIndex = data.PlanGoodsReceive_Index;
                        item.PlanGoodsReceiveItemIndex = data.PlanGoodsReceiveItem_Index;
                        //if (data.PlanGoodsReceive_Index != null)
                        //{
                        //    var strwhere1 = new SqlParameter("@strwhere", " and PlanGoodsReceive_Index = '" + data.PlanGoodsReceive_Index + "'");
                        //    var itemList = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere",strwhere1).FirstOrDefault();
                        //    item.PlanGoodsReceiveIndex = itemList.PlanGoodsReceive_Index;                            
                        //}
                        //var strwhere2 = new SqlParameter("@strwhere", " and PlanGoodsReceive_Index = '" + data.PlanGoodsReceive_Index + "'");
                        //var itemLists = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere",strwhere2).FirstOrDefault();
                        item.PlanGoodsReceiveNo = data.PlanGoodsReceive_No;
                        item.ProductIndex = data.Product_Index;
                        item.LineNum = data.LineNum;
                        item.ProductId = data.Product_Id;
                        item.ProductName = data.Product_Name;
                        item.ProductSecondName = data.Product_SecondName;
                        item.ProductThirdName = data.Product_ThirdName;
                        item.ProductLot = data.Product_Lot;
                        item.ItemStatusIndex = data.ItemStatus_Index;
                        item.ItemStatusId = data.ItemStatus_Id;
                        item.ItemStatusName = data.ItemStatus_Name;
                        item.OwnerIndex = data.Owner_Index;
                        item.Qty = data.Qty;
                        item.Ratio = data.Ratio;
                        item.TotalQty = data.TotalQty;
                        item.ProductConversionIndex = data.ProductConversion_Index;
                        item.ProductConversionId = data.ProductConversion_Id;
                        item.ProductConversionName = data.ProductConversion_Name;
                        item.MFGDate = data.MFG_Date;
                        item.EXPDate = data.EXP_Date;
                        item.Weight = data.Weight;
                        item.UnitWeight = data.UnitWeight;
                        item.UnitWidth = data.UnitWidth;
                        item.UnitLength = data.UnitLength;
                        item.UnitHeight = data.UnitHeight;
                        item.UnitVolume = data.UnitVolume;
                        item.Volume = data.Volume;                 
                        item.UnitPrice = data.UnitPrice;
                        item.Price = data.Price;
                        item.DocumentRefNo1 = data.DocumentRef_No1;
                        item.DocumentRefNo2 = data.DocumentRef_No2;
                        item.DocumentRefNo3 = data.DocumentRef_No3;
                        item.DocumentRefNo4 = data.DocumentRef_No4;
                        item.DocumentRefNo5 = data.DocumentRef_No5;
                        item.DocumentStatus = data.Document_Status;
                        item.DocumentRemark = data.DocumentItem_Remark;
                        item.DocumentTypeIndex = data.DocumentType_Index;
                        item.UDF1 = data.UDF_1;
                        item.UDF2 = data.UDF_2;
                        item.UDF3 = data.UDF_3;
                        item.UDF4 = data.UDF_4;
                        item.UDF5 = data.UDF_5;

                        result.Add(item);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public PlanGoodsReceiveItemViewModel getId(Guid id)
        //{
        //    if (id == Guid.Empty) { throw new NullReferenceException(); }

        //    try
        //    {
        //        using (var context = new GRDbContext())
        //        {
        //            string pstring = " and PlanGoodsReceiveItem_Index = '" + id + "'";

        //            var queryResult = context.IM_PlanGoodsReceiveItems.FromSql("sp_GetPlanGoodsReceiveItem @strwhere = {0}", pstring).FirstOrDefault();

        //            var item = new PlanGoodsReceiveItemViewModel();



        //            item.planGoodsReceive_Index = queryResult.PlanGoodsReceive_Index;
        //            item.planGoodsReceiveItem_Index = queryResult.PlanGoodsReceiveItem_Index;
        //            item.product_Index = queryResult.Product_Index;
        //            item.product_Id = queryResult.Product_Id;
        //            item.product_Name = queryResult.Product_Name;
        //            item.product_SecondName = queryResult.Product_SecondName;
        //            item.product_ThirdName = queryResult.Product_ThirdName;
        //            item.product_Lot = queryResult.Product_Lot;
        //            item.itemStatus_Index = queryResult.ItemStatus_Index;
        //            item.itemStatus_Id = queryResult.ItemStatus_Id;
        //            item.itemStatus_Name = queryResult.ItemStatus_Name;
        //            item.qty = queryResult.Qty;
        //            item.ratio = queryResult.Ratio;
        //            item.totalQty = queryResult.TotalQty;
        //            item.productConversion_Index = queryResult.ProductConversion_Index;
        //            item.productConversion_Id = queryResult.ProductConversion_Id;
        //            item.productConversion_Name = queryResult.ProductConversion_Name;
        //            item.mfg_Date = queryResult.MFG_Date;
        //            item.exp_Date = queryResult.EXP_Date;
        //            item.weight = queryResult.Weight;
        //            item.UnitWeight = queryResult.UnitWeight;
        //            item.UnitWidth = queryResult.UnitWidth;
        //            item.UnitLength = queryResult.UnitLength;
        //            item.UnitHeight = queryResult.UnitHeight;
        //            item.UnitVolume = queryResult.UnitVolume;
        //            item.Volume = queryResult.Volume;
        //            item.UnitPrice = queryResult.UnitPrice;
        //            item.Price = queryResult.Price;
        //            item.DocumentRefNo1 = queryResult.DocumentRef_No1;
        //            item.DocumentRefNo2 = queryResult.DocumentRef_No2;
        //            item.DocumentRefNo3 = queryResult.DocumentRef_No3;
        //            item.DocumentRefNo4 = queryResult.DocumentRef_No4;
        //            item.DocumentRefNo5 = queryResult.DocumentRef_No5;
        //            item.DocumentStatus = queryResult.Document_Status;
        //            item.DocumentRemark = queryResult.Document_Remark;
        //            item.UDF1 = queryResult.UDF_1;
        //            item.UDF2 = queryResult.UDF_2;
        //            item.UDF3 = queryResult.UDF_3;
        //            item.UDF4 = queryResult.UDF_4;
        //            item.UDF5 = queryResult.UDF_5;

        //            return item;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<GetGoodsReceiveViewModel> GetGoodsReceiveItem(Guid id)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    string pstring = " and Ref_Document_Index = '" + id + "'";
                    pstring += " and Document_Status != -1 ";

                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var queryResult = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();

                    var result = new List<GetGoodsReceiveViewModel>();

                    foreach (var item in queryResult)
                    {
                        var resultItem = new GetGoodsReceiveViewModel();                        

                        resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                        resultItem.RefDocumentNo = item.Ref_Document_No;
                        resultItem.GoodsReceiveItemIndex = item.GoodsReceiveItem_Index;
                        var strwhere1 = new SqlParameter("@strwhere", " and GoodsReceive_Index = '" + item.GoodsReceive_Index + "'");
                        var itemList = context.IM_GoodsReceive.FromSql("sp_GetGoodsReceive @strwhere", strwhere1).FirstOrDefault();
                        resultItem.GoodsReceiveNo = itemList.GoodsReceive_No;
                        resultItem.ProductName = item.Product_Name;
                        resultItem.ProductSecondName = item.Product_SecondName;
                        resultItem.ProductConversionName = item.ProductConversion_Name;
                        resultItem.TotalQty = item.TotalQty;
                        resultItem.qty = item.Qty;
                        resultItem.Weight = item.Weight;
                        resultItem.Volume = item.Volume;
                        resultItem.ProductId = item.Product_Id;
                        resultItem.ProductName = item.Product_Name;
                        resultItem.ProductConversionName = item.ProductConversion_Name;                       
                        resultItem.ItemStatusIndex = item.ItemStatus_Index;
                        resultItem.ItemStatusName = item.ItemStatus_Name;
                        resultItem.ItemStatusId = item.ItemStatus_Id;
                        resultItem.DocumentStatus = item.Document_Status;
                        resultItem.ProductConversionName = item.ProductConversion_Name;
                        resultItem.GoodsReceiveDate = itemList.GoodsReceive_Date.toString();
                        resultItem.Create_Date = item.Create_Date.GetValueOrDefault();
                        resultItem.Create_By = item.Create_By;
                        resultItem.Update_Date = item.Update_Date.GetValueOrDefault();
                        resultItem.Update_By = item.Update_By;
                        resultItem.Cancel_Date = item.Cancel_Date.GetValueOrDefault();
                        resultItem.Cancel_By = item.Cancel_By;
                        result.Add(resultItem);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RemainQtyViewModel> GetRemainQty(Guid id)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    string pstring = " and PlanGoodsReceive_Index = '" + id + "'";
                    pstring += "  and Total > 0 ";
                    var strwhere = new SqlParameter("@strwhere", pstring);

                    var queryResult = context.View_GoodsReceivePending.FromSql("sp_GetGoodsReceivePending @strwhere", strwhere).ToList();

                    var result = new List<RemainQtyViewModel>();

                    foreach (var item in queryResult)
                    {
                        var resultItem = new RemainQtyViewModel();

                        resultItem.PlanGoodsReceive_Index = item.PlanGoodsReceive_Index;
                        resultItem.PlanGoodsReceiveItem_Index = item.PlanGoodsReceiveItem_Index;
                        resultItem.ProductConversionId = item.ProductConversion_Id;
                        resultItem.ProductConversionName = item.ProductConversion_Name;
                        resultItem.ProductId = item.Product_Id;
                        resultItem.ProductName = item.Product_Name;
                        resultItem.ProductSecondName = item.Product_SecondName;
                        resultItem.Total = item.Total;
                        resultItem.Qty = item.Qty;
                        resultItem.Ratio = item.Ratio;
                        resultItem.GRTotalQty = item.GRTotalQty;
                        //resultItem.GoodsReceiveDate = item.GoodsReceive_Date.toString();
                                              
                                              
                       
                        result.Add(resultItem);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PlanGoodsIssueItemPopViewModel> GetPlanGoodsIssueItemPopup(PlanGoodsIssueItemPopViewModel model)
        {


            try
            {
                var result = new List<PlanGoodsIssueItemPopViewModel>();

                using (var context = new GRDbContext())
                {
                    var pstring = "";

                    pstring = " and PlanGoodsIssue_Index = '" + model.PlanGoodsIssueIndex + "' and Document_Status != -1 ";

                    var strwhere1 = new SqlParameter("@strwhere", pstring);
                    var queryResult = context.Get_PlanGoodsIssueItemPopup.FromSql("sp_GetPlanGoodsIssueReturnReceive @strwhere", strwhere1).ToList();


                    foreach (var data in queryResult)
                    {
                        var item = new PlanGoodsIssueItemPopViewModel();

                        var strwhere = new SqlParameter("@strwhere", " and Ref_Document_Index = '" + model.PlanGoodsIssueIndex + "' and Ref_DocumentItem_Index ='" + data.PlanGoodsIssueItem_Index + "'");
                        //var chkGIL = context.IM_GoodsIssueItemLocation.FromSql("sp_GetGoodsIssueItemLocation @strwhere", strwhere).FirstOrDefault();
                        item.PlanGoodsIssueIndex = data.PlanGoodsIssue_Index;
                        item.PlanGoodsIssueItemIndex = data.PlanGoodsIssueItem_Index;
                        item.PlanGoodsIssueNo = data.PlanGoodsIssue_No;
                        item.ProductIndex = data.Product_Index;
                        item.ProductId = data.Product_Id;
                        item.ProductName = data.Product_Name;
                        item.ProductSecondName = data.Product_SecondName;
                        item.ProductThirdName = data.Product_ThirdName;
                        item.ProductLot = data.Product_Lot;
                        item.ItemStatusIndex = data.ItemStatus_Index;
                        item.ItemStatusId = data.ItemStatus_Id;
                        item.ItemStatusName = data.ItemStatus_Name;
                        item.Qty = data.Qty;
                        item.Ratio = data.Ratio;
                        item.TotalQty = data.TotalQty;
                        item.ProductConversionIndex = data.ProductConversion_Index;
                        item.ProductConversionId = data.ProductConversion_Id;
                        item.ProductConversionName = data.ProductConversion_Name;
                        //if (data.EXP_Date != null || data.MFG_Date != null)
                        //{
                        //    item.EXPDate = data.EXP_Date;
                        //    item.MFGDate = data.MFG_Date;
                        //}
                        //else
                        //{
                        //    item.EXPDate = data.EXP_Date;
                        //    item.MFGDate = data.MFG_Date;
                        //}
                        item.EXPDate = data.EXP_Date;
                        item.MFGDate = data.MFG_Date;
                        item.Weight = data.Weight;
                        item.UnitWeight = data.UnitWeight;
                        item.UnitWidth = data.UnitWidth;
                        item.UnitLength = data.UnitLength;
                        item.UnitHeight = data.UnitHeight;
                        item.UnitVolume = data.UnitVolume;
                        item.Volume = data.Volume;
                        item.UnitPrice = data.UnitPrice;
                        item.Price = data.Price;
                        item.DocumentRefNo1 = data.DocumentRef_No1;
                        item.DocumentRefNo2 = data.DocumentRef_No2;
                        item.DocumentRefNo3 = data.DocumentRef_No3;
                        item.DocumentRefNo4 = data.DocumentRef_No4;
                        item.DocumentRefNo5 = data.DocumentRef_No5;
                        item.DocumentStatus = data.Document_Status;
                        item.DocumentRemark = data.Document_Remark;
                        item.UDF1 = data.UDF_1;
                        item.UDF2 = data.UDF_2;
                        item.UDF3 = data.UDF_3;
                        item.UDF4 = data.UDF_4;
                        item.UDF5 = data.UDF_5;

                        result.Add(item);
                    }
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
