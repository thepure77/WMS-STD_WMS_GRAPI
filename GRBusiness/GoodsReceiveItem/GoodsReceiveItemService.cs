using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness.GoodsReceive;
using GRBusiness.PlanGoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GRBusiness.GoodsReceiveItem
{
    public class GoodsReceiveItemService
    {
        private GRDbContext db;

        public GoodsReceiveItemService()
        {
            db = new GRDbContext();
        }

        public GoodsReceiveItemService(GRDbContext db)
        {
            this.db = db;
        }

        public List<GoodsReceiveItemPOContrack> getPoContrackByGoodReceiveId(string id)
        {
            try
            {
                var result = new List<GoodsReceiveItemPOContrack>();

                using (var context = new GRDbContext())
                {
                    Guid GoodsReceiveIndex = new Guid(id);

                    var queryResult = db.Po_subcontact.Where(c => c.GoodsReceive_Index == GoodsReceiveIndex && c.Status_Id != -1).OrderBy(o =>o.Product_Id).ToList();

                    foreach (var data in queryResult)
                    {
                        var item = new GoodsReceiveItemPOContrack();
                        item.Po_Index = data.Po_Index;
                        item.GoodsReceive_Index = data.GoodsReceive_Index;
                        item.GoodsReceive_No = data.GoodsReceive_No;
                        item.GoodsIssueItem_Index = data.GoodsIssueItem_Index;
                        item.GoodsIssue_Index = data.GoodsIssue_Index;
                        item.GoodsIssue_No = data.GoodsIssue_No;
                        item.Qty = data.Qty;
                        item.Ratio = data.Ratio;
                        item.TotalQty = data.TotalQty;
                        item.Product_Id = data.Product_Id;
                        item.Product_Index = data.Product_Index;
                        item.Product_Name = data.Product_Name;
                        item.ProductConversion_Index = data.ProductConversion_Index;
                        item.ProductConversion_Id = data.ProductConversion_Id;
                        item.ProductConversion_Name = data.ProductConversion_Name;
                        item.ERP_Location = data.ERP_Location;
                  

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

        public List<GoodsReceiveItemCheckViewModel> GetByGoodReceiveId(string id)
        {
            try
            {
                var result = new List<GoodsReceiveItemCheckViewModel>();

                using (var context = new GRDbContext())
                {

                    //string pstring = "";
                    //pstring += " and GoodsReceive_Index = '" + id + "'";
                    //pstring += " and Document_Status != -1 ";
                    //var strwhere = new SqlParameter("@strwhere", pstring);

                    //var queryResult = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere", strwhere).ToList();
                    Guid GoodsReceiveIndex = new Guid(id);

                    var queryResult = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == GoodsReceiveIndex && c.Document_Status != -1).OrderBy(o => o.Product_Id).ToList();

                    foreach (var data in queryResult)
                    {
                        var item = new GoodsReceiveItemCheckViewModel();
                        var planGR = data.Ref_Document_Index.ToString();
                        item.goodsReceive_Index = data.GoodsReceive_Index;
                        item.goodsReceiveItem_Index = data.GoodsReceiveItem_Index;
                        item.LineNum = data.LineNum;
                        item.product_Index = data.Product_Index;
                        item.product_Id = data.Product_Id;
                        item.product_Name = data.Product_Name;
                        item.product_SecondName = data.Product_SecondName;
                        item.product_ThirdName = data.Product_ThirdName;
                        item.product_Lot = data.Product_Lot;
                        item.itemStatus_Index = data.ItemStatus_Index;
                        item.itemStatus_Id = data.ItemStatus_Id;
                        item.itemStatus_Name = data.ItemStatus_Name;
                        item.qty = string.Format(String.Format("{0:N2}", data.Qty));
                        item.ratio = data.Ratio;
                        item.totalQty = data.TotalQty;
                        item.productConversion_Index = data.ProductConversion_Index;
                        item.productConversion_Id = data.ProductConversion_Id;
                        item.productConversion_Name = data.ProductConversion_Name;
                        item.ref_Document_No = data.Ref_Document_No;
                        item.mfg_Date = data.MFG_Date.toString();
                        item.exp_Date = data.EXP_Date.toString();

                        item.unitWeight = data.UnitWeight;
                        item.weight = data.Weight;
                        item.weight_Index = data.Weight_Index;
                        item.weight_Id = data.Weight_Id;
                        item.weight_Name = data.Weight_Name;
                        item.netWeight = data.NetWeight;

                        item.unitGrsWeight = data.UnitGrsWeight;
                        item.grsWeight = data.GrsWeight;
                        item.grsWeight_Index = data.GrsWeight_Index;
                        item.grsWeight_Id = data.GrsWeight_Id;
                        item.grsWeight_Name = data.GrsWeight_Name;

                        item.unitWidth = data.UnitWidth;
                        item.width = data.Width;
                        item.width_Index = data.Width_Index;
                        item.width_Id = data.Width_Id;
                        item.width_Name = data.Width_Name;

                        item.unitLength = data.UnitLength;
                        item.length = data.Length;
                        item.length_Index = data.Length_Index;
                        item.length_Id = data.Length_Id;
                        item.length_Name = data.Length_Name;

                        item.unitHeight = data.UnitHeight;
                        item.height = data.Height;
                        item.height_Index = data.Height_Index;
                        item.height_Id = data.Height_Id;
                        item.height_Name = data.Height_Name;

                        item.volume = data.Volume / data.Qty;
                        item.unitVolume = item.volume / data.Qty;


                        item.unitPrice = data.UnitPrice;
                        item.price = data.Price;
                        item.totalPrice = data.TotalQty;

                        item.currency_Index = data.Currency_Index;
                        item.currency_Id = data.Currency_Id;
                        item.currency_Name = data.Currency_Name;

                        item.ref_Code1 = data.Ref_Code1;
                        item.ref_Code2 = data.Ref_Code2;
                        item.ref_Code3 = data.Ref_Code3;
                        item.ref_Code4 = data.Ref_Code4;
                        item.ref_Code5 = data.Ref_Code5;

                        item.invoice_No = data.Invoice_No;
                        item.declaration_No = data.Declaration_No;
                        item.hS_Code = data.HS_Code;
                        item.conutry_of_Origin = data.Conutry_of_Origin;

                        item.tax1 = data.Tax1;
                        item.tax1_Currency_Index = data.Tax1_Currency_Index;
                        item.tax1_Currency_Id = data.Tax1_Currency_Id;
                        item.tax1_Currency_Name = data.Tax1_Currency_Name;

                        item.tax2 = data.Tax2;
                        item.tax2_Currency_Index = data.Tax2_Currency_Index;
                        item.tax2_Currency_Id = data.Tax2_Currency_Id;
                        item.tax2_Currency_Name = data.Tax2_Currency_Name;

                        item.tax3 = data.Tax1;
                        item.tax3_Currency_Index = data.Tax3_Currency_Index;
                        item.tax3_Currency_Id = data.Tax3_Currency_Id;
                        item.tax3_Currency_Name = data.Tax3_Currency_Name;

                        item.tax4 = data.Tax4;
                        item.tax4_Currency_Index = data.Tax4_Currency_Index;
                        item.tax4_Currency_Id = data.Tax4_Currency_Id;
                        item.tax4_Currency_Name = data.Tax4_Currency_Name;

                        item.tax5 = data.Tax5;
                        item.tax5_Currency_Index = data.Tax5_Currency_Index;
                        item.tax5_Currency_Id = data.Tax5_Currency_Id;
                        item.tax5_Currency_Name = data.Tax5_Currency_Name;

                        item.ref_Document_Index = data.Ref_Document_Index;
                        item.ref_DocumentItem_Index = data.Ref_DocumentItem_Index;
                        item.ref_Document_No = data.Ref_Document_No;
                        item.ref_Document_LineNum = data.Ref_Document_LineNum;
                        item.documentRef_No1 = data.DocumentRef_No1;
                        item.documentRef_No2 = data.DocumentRef_No2;
                        item.documentRef_No3 = data.DocumentRef_No3;
                        item.documentRef_No4 = data.DocumentRef_No4;
                        item.documentRef_No5 = data.DocumentRef_No5;
                        item.document_Status = data.Document_Status;
                        item.goodsReceive_Remark = data.GoodsReceive_Remark;
                        item.erp_Location = data.ERP_Location;
                        item.udf_1 = data.UDF_1;
                        item.udf_2 = data.UDF_2;
                        item.udf_3 = data.UDF_3;
                        item.udf_4 = data.UDF_4;
                        item.udf_5 = data.UDF_5;


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


        public List<GoodsReceiveItemViewModel> getId(GoodsReceiveItemViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    string pstring = "";
                    pstring += " and GoodsReceive_Index = '" + data.goodsReceive_Index + "'";
                    pstring += " and Product_Index = '" + data.product_Index + "'";
                    pstring += " and Document_Status != -1 ";
                    var strwhere = new SqlParameter("@strwhere", pstring);

                    var queryResult = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItemPopup @strwhere", strwhere).ToList();

                    var result = new List<GoodsReceiveItemViewModel>();
                    foreach (var item in queryResult)
                    {
                        var resultItem = new GoodsReceiveItemViewModel();
                        resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                        resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        resultItem.product_Name = item.Product_Name;
                        resultItem.product_SecondName = item.Product_SecondName;
                        resultItem.productConversion_Id = item.ProductConversion_Id;
                        resultItem.productConversion_Index = item.ProductConversion_Index;
                        resultItem.productConversion_Name = item.ProductConversion_Name;
                        resultItem.ratio = item.Ratio;
                        resultItem.qty = item.Qty;
                        resultItem.weight = item.Weight;
                        resultItem.volume = item.Volume;
                        resultItem.totalQty = item.TotalQty;
                        resultItem.itemStatus_Index = item.ItemStatus_Index;
                        resultItem.itemStatus_Name = item.ItemStatus_Name;
                        resultItem.itemStatus_Id = item.ItemStatus_Id;
                        resultItem.create_Date = item.Create_Date.toString();
                        resultItem.create_By = item.Create_By;
                        resultItem.update_Date = item.Update_Date.toString();
                        resultItem.update_By = item.Update_By;
                        resultItem.cancel_Date = item.Cancel_Date.toString();
                        resultItem.cancel_By = item.Cancel_By;
                        resultItem.lineNum = item.LineNum;
                        result.Add(resultItem);
                    }

                    if (queryResult.Count == 0)
                    {
                        pstring += " and GoodsReceive_Index = '" + data.goodsReceive_Index + "'";
                        pstring += " and Product_Index = '" + data.product_Index + "'";
                        pstring += " and Document_Status != -1 ";
                        strwhere = new SqlParameter("@strwhere", pstring);
                        var itemList = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItemByScan @strwhere", strwhere).ToList();

                        foreach (var item in itemList)
                        {
                            var resultItem = new GoodsReceiveItemViewModel();
                            resultItem.product_Name = item.Product_Name;
                            resultItem.productConversion_Name = item.ProductConversion_Name;
                            resultItem.product_SecondName = item.Product_SecondName;
                            resultItem.ratio = item.Ratio;
                            resultItem.qty = item.Qty;
                            resultItem.totalQty = item.TotalQty;
                            resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                            resultItem.productConversion_Name = item.ProductConversion_Name;
                            resultItem.create_Date = item.Create_Date.toString();
                            resultItem.create_By = item.Create_By;
                            resultItem.update_Date = item.Update_Date.toString();
                            resultItem.update_By = item.Update_By;
                            resultItem.cancel_Date = item.Cancel_Date.toString();
                            resultItem.cancel_By = item.Cancel_By;
                            resultItem.lineNum = item.LineNum;
                            result.Add(resultItem);
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PopupPlanGoodsReceiveViewModel> GetPlanGoodReceivePopup(PopupPlanGoodsReceiveViewModel model)
        {
            //if (String.IsNullOrEmpty(id)) throw new NullReferenceException();

            try
            {
                var result = new List<PopupPlanGoodsReceiveViewModel>();

                using (var context = new GRDbContext())
                {
                    var checkGR = db.View_CheckPlanGR.Find(model.planGoodsReceive_Index);

                    if (checkGR == null)
                    {


                        var filterModel = new List<PopupPlanGoodsReceiveViewModel>();



                        //if (!string.IsNullOrEmpty(model.planGoodsReceive_Index.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                        //{
                        //    filterModel.planGoodsReceive_Index = model.planGoodsReceive_Index;

                        //}
                        //if (!string.IsNullOrEmpty(model.owner_Index.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                        //{
                        //    filterModel.owner_Index = model.owner_Index;

                        //}
                        result = utils.SendDataApi<List<PopupPlanGoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("getPlanGRIfilter"), model.sJson());


                    }
                    else
                    {
                        //var query = db.View_PlanGoodsReceiveItem.AsQueryable();

                        var filterModel = new List<PopupPlanGoodsReceiveViewModel>();
                        result = utils.SendDataApi<List<PopupPlanGoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("getPlanGRIPendingfilter"), model.sJson());


                        
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PlanGoodsReceiveItemViewModel> CheckCopyItem(GoodsReceiveItemViewModel model)
        {
            //if (String.IsNullOrEmpty(id)) throw new NullReferenceException();

            try
            {
                var pgiModel = new PlanGoodsReceiveItemViewModel();
                pgiModel.planGoodsReceive_Index = model.ref_Document_Index;
                pgiModel.planGoodsReceiveItem_Index = model.ref_DocumentItem_Index;

                List<PlanGoodsReceiveItemViewModel> result = new List<PlanGoodsReceiveItemViewModel>();

                using (var context = new GRDbContext())
                {
                    var planGoodsReceive_Index = model.ref_Document_Index;

                    result = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("getPlanGRI"), pgiModel.sJson());
                    //}
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region im_GoodsReceiveItem
        public List<GoodsReceiveItemViewModel> im_GoodsReceiveItem(DocumentViewModel model)
        {
            try
            {
                var query = db.IM_GoodsReceiveItem.AsQueryable();




                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.GoodsReceive_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.GoodsReceiveItem_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Document_Status));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().ref_document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.ref_document_Index).Contains(c.Ref_Document_Index));
                }



                var queryresult = query.ToList();

                var result = new List<GoodsReceiveItemViewModel>();

                foreach (var item in queryresult)
                {
                    var resultItem = new GoodsReceiveItemViewModel();
                    resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.lineNum = item.LineNum;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = item.Qty;
                    resultItem.qtyPlan = item.QtyPlan;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.pallet_Index = item.Pallet_Index;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.mFG_Date = item.MFG_Date.toString();
                    resultItem.eXP_Date = item.EXP_Date.toString();
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.weight = item.Weight;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.ref_Process_Index = item.Ref_Process_Index;
                    resultItem.ref_Document_No = item.Ref_Document_No;
                    resultItem.ref_Document_Index = item.Ref_Document_Index;
                    resultItem.ref_Document_LineNum = item.Ref_Document_LineNum;
                    resultItem.ref_DocumentItem_Index = item.Ref_DocumentItem_Index;
                    resultItem.goodsReceive_Remark = item.GoodsReceive_Remark;
                    resultItem.goodsReceive_DockDoor = item.GoodsReceive_DockDoor;
                    resultItem.create_Date = item.Create_Date.toString();
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date.toString();
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date.toString();
                    resultItem.cancel_By = item.Cancel_By;

                    result.Add(resultItem);
                }


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region im_GoodsReceiveItemLocation
        public List<GoodsReceiveItemLocationViewModel> im_GoodsReceiveItemLocation(DocumentViewModel model)
        {
            try
            {
                var query = db.IM_GoodsReceiveItemLocation.AsQueryable();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.GoodsReceive_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                   query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.GoodsReceiveItem_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().documentItemLocation_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItemLocation_Index).Contains(c.GoodsReceiveItemLocation_Index));
                }



                var queryresult = query.ToList();

                var result = new List<GoodsReceiveItemLocationViewModel>();

                foreach (var item in queryresult)
                {
                    var resultItem = new GoodsReceiveItemLocationViewModel();
                    resultItem.goodsReceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;
                    resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.tagItem_Index = item.TagItem_Index;
                    resultItem.tag_Index = item.Tag_Index;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;              
                    resultItem.mfg_Date = item.MFG_Date;
                    resultItem.exp_Date = item.EXP_Date;
                    resultItem.unitWeight = item.UnitWeight;
                    resultItem.weight = item.Weight;
                    resultItem.unitWidth = item.UnitWidth;
                    resultItem.unitLength = item.UnitLength;
                    resultItem.unitHeight = item.UnitHeight;
                    resultItem.unitVolume = item.UnitVolume;
                    resultItem.volume = item.Volume;
                    resultItem.unitPrice = item.UnitPrice;
                    resultItem.price = item.Price;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.location_Index = item.Location_Index;
                    resultItem.location_Id = item.Location_Id;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.qty = item.Qty;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.udf_1 = item.UDF_1;
                    resultItem.udf_2 = item.UDF_2;
                    resultItem.udf_3 = item.UDF_3;
                    resultItem.udf_4 = item.UDF_4;
                    resultItem.udf_5 = item.UDF_5;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date;
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.putaway_Status = item.Putaway_Status;
                    resultItem.putaway_By = item.Putaway_By;
                    resultItem.putaway_Date = item.Putaway_Date;
                    resultItem.suggest_Location_Index = item.Suggest_Location_Index;
                    resultItem.invoice_No = item.Invoice_No;


                    result.Add(resultItem);
                }


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



    }
}
