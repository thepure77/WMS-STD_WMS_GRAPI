using Comone.Utils;
using DataAccess;
using GRBusiness.PlanGoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static GRBusiness.PlanGoodsReceive.PlanGoodsReceiveDocViewModel;
using static GRBusiness.PlanGoodsReceive.PlanGoodsReceivePopupViewModel;
using static GRBusiness.PlanGoodsReceive.ReturnReceiveViewModel;
using static GRBusiness.PlanGoodsReceive.SearchDetailModel;
using static PlanGRBusiness.ViewModels.PlanGoodsReceivePopupViewModels;

namespace GRBusiness
{
    public class PlanGoodsReceiveService
    {
        public List<PlanGoodsReceiveDocViewModel> Filter()
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    //var queryResult = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive").ToList();
                    string pstring = "";


                    pstring += " and PlanGoodsReceive_Date >= Cast(Getdate() as Date) Order By Create_Date DESC ";



                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var PageNumber = new SqlParameter("@PageNumber", 1);
                    var RowspPage = new SqlParameter("@RowspPage", 30);
                    var queryResultTotal = context.View_PlanGrProcessStatus.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();
                    var count = queryResultTotal.Count();

                    //.Where(c => c.Create_Date >= DateTime.Today).OrderByDescending(o => o.Create_Date).ThenBy(a => a.PlanGoodsReceive_Date)
                    //var queryResult = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive").ToList();
                    var result = new List<PlanGoodsReceiveDocViewModel>();
                    foreach (var item in queryResultTotal)
                    {
                        var resultItem = new PlanGoodsReceiveDocViewModel();


                        resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                        resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                        resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                        resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Due_Date.toString();
                        resultItem.VendorIndex = item.Vendor_Index;
                        resultItem.VendorId = item.Vendor_Id;
                        resultItem.VendorName = item.Vendor_Name;
                        resultItem.OwnerIndex = item.Owner_Index;
                        resultItem.OwnerId = item.Owner_Id;
                        resultItem.OwnerName = item.Owner_Name;
                        resultItem.UDF1 = item.UDF_1;
                        resultItem.DocumentTypeIndex = item.DocumentType_Index;
                        resultItem.DocumentTypeId = item.DocumentType_Id;
                        resultItem.DocumentTypeName = item.DocumentType_Name;
                        resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                        resultItem.DocumentStatus = item.Document_Status;
                        resultItem.ProcessStatusName = item.ProcessStatus_Name;
                        resultItem.WarehouseIndex = item.Warehouse_Index;
                        resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                        resultItem.WarehouseId = item.Warehouse_Id;
                        resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                        resultItem.WarehouseName = item.Warehouse_Name;
                        resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                        resultItem.DocumentRemark = item.Document_Remark;
                        resultItem.Create_Date = item.Create_Date.toString();
                        resultItem.Create_By = item.Create_By;
                        resultItem.Update_Date = item.Update_Date.toString();
                        resultItem.Update_By = item.Update_By;
                        resultItem.Cancel_Date = item.Cancel_Date.toString();
                        resultItem.Cancel_By = item.Cancel_By;
                        resultItem.count = count;
                        resultItem.Item_Document_Remark = item.Item_Document_Remark;
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

        public actionResultPlanGRPopupViewModel FilterPopup(PlanGoodsReceivePopupViewModel model)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var actionResultPlanGRPopup = new actionResultPlanGRPopupViewModel();
                    var result = new List<PlanGoodsReceivePopupViewModel>();
                    //GRFilter page
                    if (model.chk == "1")
                    {
                        string pstring = "And Document_Status = 1";
                        pstring += " Order By PlanGoodsReceive_Date Desc ";


                        if (model.CurrentPage == 0)
                        {
                            model.CurrentPage = 1;
                        }
                        if (model.PerPage == 0)
                        {
                            model.PerPage = 30;
                        }
                        var strwhere = new SqlParameter("@strwhere", pstring);
                        var PageNumber = new SqlParameter("@PageNumber", 1);
                        var RowspPage = new SqlParameter("@RowspPage", 10000);

                        var queryResultTotal = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                        var strwhere1 = new SqlParameter("@strwhere", pstring);
                        var PageNumber1 = new SqlParameter("@PageNumber", model.CurrentPage);
                        var RowspPage1 = new SqlParameter("@RowspPage", model.PerPage);
                        var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                        //var queryResult = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive").Where(c => c.Document_Status == 1).OrderByDescending(o => o.Create_Date).ThenBy(a => a.PlanGoodsReceive_Date).ToList();
                        foreach (var item in query)
                        {
                            var resultItem = new PlanGoodsReceivePopupViewModel();

                            //var PlanDate = resultItem.PlanGoodsReceiveDate.toDate();
                            //var PlanDueDate = resultItem.PlanGoodsReceiveDueDate.toDate();


                            resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                            resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                            resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.VendorIndex = item.Vendor_Index;
                            resultItem.VendorId = item.Vendor_Id;
                            resultItem.VendorName = item.Vendor_Name;
                            resultItem.OwnerIndex = item.Owner_Index;
                            resultItem.OwnerId = item.Owner_Id;
                            resultItem.OwnerName = item.Owner_Name;
                            resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                            resultItem.DocumentStatus = item.Document_Status;
                            resultItem.WarehouseIndex = item.Warehouse_Index;
                            resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                            resultItem.WarehouseId = item.Warehouse_Id;
                            resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                            resultItem.WarehouseName = item.Warehouse_Name;
                            resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                            resultItem.DocumentRemark = item.Document_Remark;
                            resultItem.Create_Date = item.Create_Date.toString();
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_Date = item.Update_Date.toString();
                            resultItem.Update_By = item.Update_By;
                            resultItem.Cancel_Date = item.Cancel_Date.toString();
                            resultItem.Cancel_By = item.Cancel_By;

                            result.Add(resultItem);
                        }

                        var count = queryResultTotal.Count;
                        actionResultPlanGRPopup = new actionResultPlanGRPopupViewModel();
                        actionResultPlanGRPopup.itemsPlanGR = result.ToList();
                        actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };
                    }
                    //Create Gr page
                    else if (model.chk == "2")
                    {
                        string pstring = " And Document_Status = 1 ";
                        //pstring += " Order By PlanGoodsReceive_Date Desc ";


                        if (model.CurrentPage == 0)
                        {
                            model.CurrentPage = 1;
                        }
                        if (model.PerPage == 0)
                        {
                            model.PerPage = 10;
                        }
                        var strwhere = new SqlParameter("@strwhere", pstring);
                        var PageNumber = new SqlParameter("@PageNumber", 1);
                        var RowspPage = new SqlParameter("@RowspPage", 10000);

                        var queryResultTotal = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive_Popup @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                        var strwhere1 = new SqlParameter("@strwhere", pstring);
                        var PageNumber1 = new SqlParameter("@PageNumber", model.CurrentPage);
                        var RowspPage1 = new SqlParameter("@RowspPage", model.PerPage);
                        var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive_Popup @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                        foreach (var item in query)
                        {
                            var resultItem = new PlanGoodsReceivePopupViewModel();

                            //var PlanDate = resultItem.PlanGoodsReceiveDate.toDate();
                            //var PlanDueDate = resultItem.PlanGoodsReceiveDueDate.toDate();


                            resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                            resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                            resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.VendorIndex = item.Vendor_Index;
                            resultItem.VendorId = item.Vendor_Id;
                            resultItem.VendorName = item.Vendor_Name;
                            resultItem.OwnerIndex = item.Owner_Index;
                            resultItem.OwnerId = item.Owner_Id;
                            resultItem.OwnerName = item.Owner_Name;
                            resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                            resultItem.DocumentStatus = item.Document_Status;
                            resultItem.WarehouseIndex = item.Warehouse_Index;
                            resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                            resultItem.WarehouseId = item.Warehouse_Id;
                            resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                            resultItem.WarehouseName = item.Warehouse_Name;
                            resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                            resultItem.DocumentRemark = item.Document_Remark;
                            resultItem.Create_Date = item.Create_Date.toString();
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_Date = item.Update_Date.toString();
                            resultItem.Update_By = item.Update_By;
                            resultItem.Cancel_Date = item.Cancel_Date.toString();
                            resultItem.Cancel_By = item.Cancel_By;
                            resultItem.DocumentTypeIndex = item.DocumentType_Index;

                            result.Add(resultItem);
                        }
                        var count = queryResultTotal.Count;
                        actionResultPlanGRPopup = new actionResultPlanGRPopupViewModel();
                        actionResultPlanGRPopup.itemsPlanGR = result.ToList();
                        actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };
                    }
                    else
                    {
                        string pstring = "And Document_Status = 1";
                        pstring += " Order By PlanGoodsReceive_Date Desc ";


                        if (model.CurrentPage == 0)
                        {
                            model.CurrentPage = 1;
                        }
                        if (model.PerPage == 0)
                        {
                            model.PerPage = 10;
                        }
                        var strwhere = new SqlParameter("@strwhere", pstring);
                        var PageNumber = new SqlParameter("@PageNumber", 1);
                        var RowspPage = new SqlParameter("@RowspPage", 10000);

                        var queryResultTotal = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                        var strwhere1 = new SqlParameter("@strwhere", pstring);
                        var PageNumber1 = new SqlParameter("@PageNumber", model.CurrentPage);
                        var RowspPage1 = new SqlParameter("@RowspPage", model.PerPage);
                        var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                        //var queryResult = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive").Where(c => c.Document_Status == 1).OrderByDescending(o => o.Create_Date).ThenBy(a => a.PlanGoodsReceive_Date).ToList();
                        foreach (var item in query)
                        {
                            var resultItem = new PlanGoodsReceivePopupViewModel();

                            //var PlanDate = resultItem.PlanGoodsReceiveDate.toDate();
                            //var PlanDueDate = resultItem.PlanGoodsReceiveDueDate.toDate();


                            resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                            resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                            resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.VendorIndex = item.Vendor_Index;
                            resultItem.VendorId = item.Vendor_Id;
                            resultItem.VendorName = item.Vendor_Name;
                            resultItem.OwnerIndex = item.Owner_Index;
                            resultItem.OwnerId = item.Owner_Id;
                            resultItem.OwnerName = item.Owner_Name;
                            resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                            resultItem.DocumentStatus = item.Document_Status;
                            resultItem.WarehouseIndex = item.Warehouse_Index;
                            resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                            resultItem.WarehouseId = item.Warehouse_Id;
                            resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                            resultItem.WarehouseName = item.Warehouse_Name;
                            resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                            resultItem.DocumentRemark = item.Document_Remark;
                            resultItem.Create_Date = item.Create_Date.toString();
                            resultItem.Create_By = item.Create_By;
                            resultItem.Update_Date = item.Update_Date.toString();
                            resultItem.Update_By = item.Update_By;
                            resultItem.Cancel_Date = item.Cancel_Date.toString();
                            resultItem.Cancel_By = item.Cancel_By;

                            result.Add(resultItem);
                        }

                        var count = queryResultTotal.Count;
                        actionResultPlanGRPopup = new actionResultPlanGRPopupViewModel();
                        actionResultPlanGRPopup.itemsPlanGR = result.ToList();
                        actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };
                    }

                    return actionResultPlanGRPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PlanGoodsReceiveDocViewModel getId(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            try
            {
                using (var context = new GRDbContext())
                {
                    string pstring = " and PlanGoodsReceive_Index = '" + id + "'";

                    var queryResult = context.View_PlanGrProcessStatus.FromSql("sp_GetPlanGoodsReceive @strwhere = {0}", pstring).FirstOrDefault();

                    var resultItem = new PlanGoodsReceiveDocViewModel();


                    resultItem.PlanGoodsReceiveIndex = queryResult.PlanGoodsReceive_Index;
                    resultItem.PlanGoodsReceiveNo = queryResult.PlanGoodsReceive_No;
                    resultItem.DocumentTypeIndex = queryResult.DocumentType_Index;
                    resultItem.DocumentTypeName = queryResult.DocumentType_Name;
                    resultItem.DocumentTypeId = queryResult.DocumentType_Id;
                    resultItem.VendorIndex = queryResult.Vendor_Index;
                    resultItem.VendorId = queryResult.Vendor_Id;
                    resultItem.VendorName = queryResult.Vendor_Name;
                    resultItem.OwnerIndex = queryResult.Owner_Index;
                    resultItem.OwnerId = queryResult.Owner_Id;
                    resultItem.OwnerName = queryResult.Owner_Name;
                    resultItem.UDF1 = queryResult.UDF_1;
                    resultItem.DocumentRefNo1 = queryResult.DocumentRef_No1;
                    resultItem.DocumentRefNo2 = queryResult.DocumentRef_No2;
                    resultItem.DocumentRefNo3 = queryResult.DocumentRef_No3;
                    resultItem.DocumentRefNo4 = queryResult.DocumentRef_No4;
                    resultItem.DocumentRefNo5 = queryResult.DocumentRef_No5;
                    resultItem.DocumentStatus = queryResult.Document_Status;
                    resultItem.WarehouseIndex = queryResult.Warehouse_Index;
                    resultItem.WarehouseIndexTo = queryResult.Warehouse_Index_To;
                    resultItem.WarehouseId = queryResult.Warehouse_Id;
                    resultItem.WarehouseIdTo = queryResult.Warehouse_Id_To;
                    resultItem.WarehouseName = queryResult.Warehouse_Name;
                    resultItem.WarehouseNameTo = queryResult.Warehouse_Name_To;
                    resultItem.DocumentRemark = queryResult.Document_Remark;
                    resultItem.ProcessStatusName = queryResult.ProcessStatus_Name;
                    resultItem.PlanGoodsReceiveDate = queryResult.PlanGoodsReceive_Date.toString();
                    resultItem.PlanGoodsReceiveDueDate = queryResult.PlanGoodsReceive_Due_Date.toString();
                    resultItem.Create_Date = queryResult.Create_Date.toString();
                    resultItem.Create_By = queryResult.Create_By;
                    resultItem.Update_Date = queryResult.Update_Date.toString();
                    resultItem.Update_By = queryResult.Update_By;
                    resultItem.Cancel_Date = queryResult.Cancel_Date.toString();
                    resultItem.Cancel_By = queryResult.Cancel_By;
                    resultItem.UserAssign = queryResult.UserAssign;

                    return resultItem;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean getDelete(PlanGoodsReceiveDocViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    string pstring = "";

                    if (data.PlanGoodsReceiveIndex.ToString() != "00000000-0000-0000-0000-000000000000" && data.PlanGoodsReceiveIndex.ToString() != null)
                    {
                        pstring = " and PlanGoodsReceive_Index ='" + data.PlanGoodsReceiveIndex + "'";
                    }
                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var checkGR = context.IM_PlanGoodsReceive.FromSql("sp_GetCheckGR @strwhere", strwhere).Any();

                    if (!checkGR)
                    {
                        var PlanGoodsReceive_Index = new SqlParameter("PlanGoodsReceive_Index", data.PlanGoodsReceiveIndex);
                        var Cancel_By = new SqlParameter("Cancel_By", data.Cancel_By);
                        var rowsAffected = context.Database.ExecuteSqlCommand("sp_Delete_im_PlanGoodsReceive @PlanGoodsReceive_Index,@Cancel_By", PlanGoodsReceive_Index, Cancel_By);
                        return true;
                    }
                }

                return false;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean confirmStatus(PlanGoodsReceiveDocViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    var PlanGoodsReceive_Index = new SqlParameter("PlanGoodsReceive_Index", data.PlanGoodsReceiveIndex);
                    var Update_By = new SqlParameter("Update_By", data.Update_By);
                    var rowsAffected = context.Database.ExecuteSqlCommand("sp_Confirm_im_PlanGoodsReceive @PlanGoodsReceive_Index,@Update_By", PlanGoodsReceive_Index, Update_By);
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public Boolean SaveChanges(PlanGoodsReceiveDocViewModel data)
        //{
        //    try
        //    {
        //        using (var context = new GRDbContext())
        //        {
        //            var result = "";
        //            var strwhere = new SqlParameter("@strwhere", " and PlanGoodsReceive_Index = '" + data.PlanGoodsReceiveIndex + "'");
        //            var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere", strwhere).FirstOrDefault();
        //            if (query == null)
        //            {

        //                if (data.PlanGoodsReceiveIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                {
        //                    data.PlanGoodsReceiveIndex = Guid.NewGuid();
        //                }
        //                if (data.PlanGoodsReceiveNo == null)
        //                {
        //                    var DocumentType_Index = new SqlParameter("@DocumentType_Index", data.DocumentTypeIndex.ToString());

        //                    var DocDate = new SqlParameter("@DocDate", data.PlanGoodsReceiveDate.toDate());

        //                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                    resultParameter.Size = 2000; // some meaningfull value
        //                    resultParameter.Direction = ParameterDirection.Output;
        //                    context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                    //var result = resultParameter.Value;
        //                    data.PlanGoodsReceiveNo = resultParameter.Value.ToString();
        //                }

        //                //----Set Header------
        //                var itemHeader = new PlanGoodsReceiveViewModel();

        //                var document_status = 0;

        //                itemHeader.planGoodsReceive_Index = data.PlanGoodsReceiveIndex;
        //                itemHeader.owner_Index = data.OwnerIndex;
        //                itemHeader.owner_Id = data.OwnerId;
        //                itemHeader.owner_Name = data.OwnerName;
        //                itemHeader.vendor_Index = data.VendorIndex;
        //                itemHeader.vendor_Id = data.VendorId;
        //                itemHeader.vendor_Name = data.VendorName;
        //                itemHeader.documentType_Index = data.DocumentTypeIndex;
        //                itemHeader.documentType_Id = data.DocumentTypeId;
        //                itemHeader.documentType_Name = data.DocumentTypeName;
        //                itemHeader.planGoodsReceive_No = data.PlanGoodsReceiveNo;
        //                itemHeader.planGoodsReceive_Date = data.PlanGoodsReceiveDate.toDate();
        //                itemHeader.planGoodsReceive_Due_Date = data.PlanGoodsReceiveDueDate.toDate();
        //                itemHeader.documentRef_No1 = data.DocumentRefNo1;
        //                itemHeader.documentRef_No2 = data.DocumentRefNo2;
        //                itemHeader.documentRef_No3 = data.DocumentRefNo3;
        //                itemHeader.documentRef_No4 = data.DocumentRefNo4;
        //                itemHeader.documentRef_No5 = data.DocumentRefNo5;
        //                itemHeader.document_Status = document_status;
        //                itemHeader.udf_1 = data.UDF1;
        //                itemHeader.udf_2 = data.UDF2;
        //                itemHeader.udf_3 = data.UDF3;
        //                itemHeader.udf_4 = data.UDF4;
        //                itemHeader.udf_5 = data.UDF5;
        //                itemHeader.documentPriority_Status = data.DocumentPriorityStatus;
        //                itemHeader.document_Remark = data.DocumentRemark;
        //                itemHeader.create_By = data.Create_By;
        //                //itemHeader.Create_Date = DateTime.Now.Date;
        //                //itemHeader.Create_Date = DateTime.Now;
        //                itemHeader.update_By = data.Update_By;
        //                //itemHeader.Update_Date = DateTime.Now;
        //                itemHeader.cancel_By = data.Cancel_By;
        //                //itemHeader.Cancel_Date = DateTime.Now;
        //                itemHeader.warehouse_Index = data.WarehouseIndex;
        //                itemHeader.warehouse_Id = data.WarehouseId;
        //                itemHeader.warehouse_Name = data.WarehouseName;
        //                itemHeader.warehouse_Index_To = data.WarehouseIndexTo;
        //                itemHeader.warehouse_Id_To = data.WarehouseIdTo;
        //                itemHeader.warehouse_Name_To = data.WarehouseNameTo;

        //                //----Set Detail-----
        //                var itemDetail = new List<PlanGoodsReceiveItemViewModel>();
        //                int addNumber = 0;
        //                foreach (var item in data.listPlanGoodsReceiveItemViewModel)
        //                {
        //                    //Get ItemStatus
        //                    var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
        //                    var ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
        //                    var ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
        //                    var ColumnName4 = new SqlParameter("@ColumnName4", "''");
        //                    var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //                    var TableName = new SqlParameter("@TableName", "ms_ItemStatus");
        //                    var Where = new SqlParameter("@Where", "");
        //                    var DataItemStatus = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();


        //                    addNumber++;
        //                    var resultItem = new PlanGoodsReceiveItemViewModel();
        //                    // Gen Index for line item
        //                    if (item.PlanGoodsReceiveItemIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                    {
        //                        item.PlanGoodsReceiveItemIndex = Guid.NewGuid();
        //                    }

        //                    // Index From Header
        //                    resultItem.PlanGoodsReceiveIndex = data.PlanGoodsReceiveIndex;
        //                    resultItem.PlanGoodsReceiveItemIndex = item.PlanGoodsReceiveItemIndex;

        //                    if (item.LineNum == null)
        //                    {
        //                        resultItem.LineNum = addNumber.ToString();
        //                    }
        //                    else
        //                    {
        //                        resultItem.LineNum = item.LineNum;
        //                    }

        //                    resultItem.ProductIndex = item.ProductIndex;
        //                    resultItem.ProductId = item.ProductId;
        //                    resultItem.ProductName = item.ProductName;
        //                    resultItem.ProductSecondName = item.ProductSecondName;
        //                    resultItem.ProductThirdName = item.ProductThirdName;
        //                    if (item.ProductLot != null)
        //                    {
        //                        resultItem.ProductLot = item.ProductLot;
        //                    }
        //                    else
        //                    {
        //                        resultItem.ProductLot = "";
        //                    }
        //                    if (DataItemStatus.dataincolumn1 != null && DataItemStatus.dataincolumn1 != "")
        //                    {
        //                        resultItem.ItemStatusIndex = new Guid(DataItemStatus.dataincolumn1);
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatusIndex = System.Guid.Empty;
        //                    }
        //                    if (DataItemStatus.dataincolumn2 != null && DataItemStatus.dataincolumn2 != "")
        //                    {
        //                        resultItem.ItemStatusId = DataItemStatus.dataincolumn2;
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatusId = "";
        //                    }
        //                    if (DataItemStatus.dataincolumn3 != null && DataItemStatus.dataincolumn3 != "")
        //                    {
        //                        resultItem.ItemStatusName = DataItemStatus.dataincolumn3;
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatusName = "";
        //                    }
        //                    resultItem.Qty = item.Qty;
        //                    resultItem.Ratio = item.Ratio;
        //                    if (item.Ratio != 0)
        //                    {
        //                        var totalqty = item.Qty * item.Ratio;
        //                        item.TotalQty = totalqty;
        //                    }
        //                    resultItem.TotalQty = item.TotalQty;
        //                    resultItem.ProductConversionIndex = item.ProductConversionIndex;
        //                    resultItem.ProductConversionId = item.ProductConversionId;
        //                    resultItem.ProductConversionName = item.ProductConversionName;
        //                    resultItem.UDF1 = item.UDF1;
        //                    resultItem.MFGDate = item.MFGDate;
        //                    resultItem.EXPDate = item.EXPDate;
        //                    resultItem.UnitWeight = item.UnitWeight;
        //                    if (item.UnitWeight != null)
        //                    {
        //                        var totalWeight = item.Qty * item.UnitWeight;

        //                        item.Weight = totalWeight;
        //                    }
        //                    resultItem.Weight = item.Weight;
        //                    resultItem.UnitWidth = item.UnitWidth;
        //                    resultItem.UnitLength = item.UnitLength;
        //                    resultItem.UnitHeight = item.UnitHeight;
        //                    resultItem.UnitVolume = item.UnitVolume;
        //                    if (item.UnitVolume != null)
        //                    {
        //                        var totalVolume = item.Qty * item.UnitVolume;
        //                        item.Volume = totalVolume;
        //                    }
        //                    resultItem.Volume = item.Volume;
        //                    resultItem.UnitPrice = item.UnitPrice;
        //                    resultItem.Price = item.Price;
        //                    resultItem.DocumentRefNo1 = item.DocumentRefNo1;
        //                    resultItem.DocumentRefNo2 = item.DocumentRefNo2;
        //                    resultItem.DocumentRefNo3 = item.DocumentRefNo3;
        //                    resultItem.DocumentRefNo4 = item.DocumentRefNo4;
        //                    resultItem.DocumentRefNo5 = item.DocumentRefNo5;
        //                    resultItem.DocumentStatus = document_status;
        //                    resultItem.DocumentRemark = item.DocumentRemark;
        //                    resultItem.UDF1 = item.UDF1;
        //                    resultItem.UDF2 = item.UDF2;
        //                    resultItem.UDF3 = item.UDF3;
        //                    resultItem.UDF4 = item.UDF4;
        //                    resultItem.UDF5 = item.UDF5;
        //                    resultItem.Create_By = data.Create_By;
        //                    //resultItem.Create_Date = item.Create_Date;
        //                    resultItem.Update_By = data.Update_By;
        //                    //resultItem.Update_Date = item.Update_Date;
        //                    resultItem.Cancel_By = data.Cancel_By;
        //                    //resultItem.Cancel_Date = item.Cancel_Date;


        //                    itemDetail.Add(resultItem);




        //                }

        //                var itemHeaderlist = new List<PlanGoodsReceiveViewModel>();
        //                itemHeaderlist.Add(itemHeader);


        //                //-- SAVE STORE PROC ----//

        //                DataTable dtHeader = CreateDataTable(itemHeaderlist);
        //                DataTable dtDetail = CreateDataTable(itemDetail);


        //                //var PlanGoodsReceive = new SqlParameter("PlanGoodsReceive", dtHeader);
        //                //PlanGoodsReceive.SqlDbType = SqlDbType.Structured;
        //                //var PlanGoodsReceiveItem = new SqlParameter("PlanGoodsReceiveItem", dtDetail);
        //                //PlanGoodsReceiveItem.SqlDbType = SqlDbType.Structured;
        //                //var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_im_PlanGoodsReceive2 @PlanGoodsReceive,@PlanGoodsReceiveItem", PlanGoodsReceive, PlanGoodsReceiveItem);

        //                var PlanGoodsReceive = new SqlParameter("PlanGoodsReceive", SqlDbType.Structured);
        //                PlanGoodsReceive.TypeName = "[dbo].[im_PlanGoodsReceiveData]";
        //                PlanGoodsReceive.Value = dtHeader;


        //                var PlanGoodsReceiveItem = new SqlParameter("PlanGoodsReceiveItem", SqlDbType.Structured);
        //                PlanGoodsReceiveItem.TypeName = "[dbo].[im_PlanGoodsReceiveItemData]";
        //                PlanGoodsReceiveItem.Value = dtDetail;

        //                var commandText = "EXEC sp_Save_im_PlanGoodsReceive @PlanGoodsReceive,@PlanGoodsReceiveItem";
        //                var rowsAffected = context.Database.ExecuteSqlCommand(commandText, PlanGoodsReceive, PlanGoodsReceiveItem);

        //                //---------------------------//

        //                result = rowsAffected.ToString();
        //            }
        //            else
        //            {
        //                if (query.Document_Status != 0)
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    if (data.PlanGoodsReceiveIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                    {
        //                        data.PlanGoodsReceiveIndex = Guid.NewGuid();
        //                    }
        //                    if (data.PlanGoodsReceiveNo == null)
        //                    {
        //                        var DocumentType_Index = new SqlParameter("@DocumentType_Index", data.DocumentTypeIndex.ToString());

        //                        var DocDate = new SqlParameter("@DocDate", data.PlanGoodsReceiveDate.toDate());

        //                        var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                        resultParameter.Size = 2000; // some meaningfull value
        //                        resultParameter.Direction = ParameterDirection.Output;
        //                        context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
        //                        //var result = resultParameter.Value;
        //                        data.PlanGoodsReceiveNo = resultParameter.Value.ToString();
        //                    }

        //                    //----Set Header------
        //                    var itemHeader = new PlanGoodsReceiveViewModel();

        //                    var document_status = 0;

        //                    itemHeader.planGoodsReceive_Index = data.PlanGoodsReceiveIndex;
        //                    itemHeader.owner_Index = data.OwnerIndex;
        //                    itemHeader.owner_Id = data.OwnerId;
        //                    itemHeader.owner_Name = data.OwnerName;
        //                    itemHeader.vendor_Index = data.VendorIndex;
        //                    itemHeader.vendor_Id = data.VendorId;
        //                    itemHeader.vendor_Name = data.VendorName;
        //                    itemHeader.documentType_Index = data.DocumentTypeIndex;
        //                    itemHeader.documentType_Id = data.DocumentTypeId;
        //                    itemHeader.documentType_Name = data.DocumentTypeName;
        //                    itemHeader.planGoodsReceive_No = data.PlanGoodsReceiveNo;
        //                    itemHeader.planGoodsReceive_Date = data.PlanGoodsReceiveDate.toDate();
        //                    itemHeader.planGoodsReceive_Due_Date = data.PlanGoodsReceiveDueDate.toDate();
        //                    itemHeader.documentRef_No1 = data.DocumentRefNo1;
        //                    itemHeader.documentRef_No2 = data.DocumentRefNo2;
        //                    itemHeader.documentRef_No3 = data.DocumentRefNo3;
        //                    itemHeader.documentRef_No4 = data.DocumentRefNo4;
        //                    itemHeader.documentRef_No5 = data.DocumentRefNo5;
        //                    itemHeader.document_Status = document_status;
        //                    itemHeader.udf_1 = data.UDF1;
        //                    itemHeader.udf_2 = data.UDF2;
        //                    itemHeader.udf_3 = data.UDF3;
        //                    itemHeader.udf_4 = data.UDF4;
        //                    itemHeader.udf_5 = data.UDF5;
        //                    itemHeader.documentPriority_Status = data.DocumentPriorityStatus;
        //                    itemHeader.document_Remark = data.DocumentRemark;
        //                    itemHeader.create_By = data.Create_By;
        //                    //itemHeader.Create_Date = DateTime.Now.Date;
        //                    //itemHeader.Create_Date = DateTime.Now;
        //                    itemHeader.update_By = data.Update_By;
        //                    //itemHeader.Update_Date = DateTime.Now;
        //                    itemHeader.cancel_By = data.Cancel_By;
        //                    //itemHeader.Cancel_Date = DateTime.Now;
        //                    itemHeader.warehouse_Index = data.WarehouseIndex;
        //                    itemHeader.warehouse_Id = data.WarehouseId;
        //                    itemHeader.warehouse_Name = data.WarehouseName;
        //                    itemHeader.warehouse_Index_To = data.WarehouseIndexTo;
        //                    itemHeader.warehouse_Id_To = data.WarehouseIdTo;
        //                    itemHeader.warehouse_Name_To = data.WarehouseNameTo;

        //                    //----Set Detail-----
        //                    var itemDetail = new List<PlanGoodsReceiveItemViewModel>();
        //                    int addNumber = 0;
        //                    foreach (var item in data.listPlanGoodsReceiveItemViewModel)
        //                    {
        //                        addNumber++;
        //                        var resultItem = new PlanGoodsReceiveItemViewModel();
        //                        // Gen Index for line item
        //                        if (item.PlanGoodsReceiveItemIndex.ToString() == "00000000-0000-0000-0000-000000000000")
        //                        {
        //                            item.PlanGoodsReceiveItemIndex = Guid.NewGuid();
        //                        }

        //                        // Index From Header
        //                        resultItem.PlanGoodsReceiveIndex = data.PlanGoodsReceiveIndex;
        //                        resultItem.PlanGoodsReceiveItemIndex = item.PlanGoodsReceiveItemIndex;

        //                        if (item.LineNum == null)
        //                        {
        //                            resultItem.LineNum = addNumber.ToString();
        //                        }
        //                        else
        //                        {
        //                            resultItem.LineNum = item.LineNum;
        //                        }

        //                        resultItem.ProductIndex = item.ProductIndex;
        //                        resultItem.ProductId = item.ProductId;
        //                        resultItem.ProductName = item.ProductName;
        //                        resultItem.ProductSecondName = item.ProductSecondName;
        //                        resultItem.ProductThirdName = item.ProductThirdName;
        //                        if (item.ProductLot != null)
        //                        {
        //                            resultItem.ProductLot = item.ProductLot;
        //                        }
        //                        else
        //                        {
        //                            resultItem.ProductLot = "";
        //                        }
        //                        resultItem.ItemStatusIndex = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4");
        //                        resultItem.ItemStatusId = "1";
        //                        resultItem.ItemStatusName = "Good";
        //                        resultItem.Qty = item.Qty;
        //                        resultItem.Ratio = item.Ratio;
        //                        if (item.Ratio != 0)
        //                        {
        //                            var totalqty = item.Qty * item.Ratio;
        //                            item.TotalQty = totalqty;
        //                        }
        //                        resultItem.TotalQty = item.TotalQty;
        //                        resultItem.ProductConversionIndex = item.ProductConversionIndex;
        //                        resultItem.ProductConversionId = item.ProductConversionId;
        //                        resultItem.ProductConversionName = item.ProductConversionName;
        //                        resultItem.UDF1 = item.UDF1;
        //                        resultItem.MFGDate = item.MFGDate;
        //                        resultItem.EXPDate = item.EXPDate;
        //                        resultItem.UnitWeight = item.UnitWeight;
        //                        if (item.UnitWeight != null)
        //                        {
        //                            var totalWeight = item.Qty * item.UnitWeight;

        //                            item.Weight = totalWeight;
        //                        }
        //                        resultItem.Weight = item.Weight;
        //                        resultItem.UnitWidth = item.UnitWidth;
        //                        resultItem.UnitLength = item.UnitLength;
        //                        resultItem.UnitHeight = item.UnitHeight;
        //                        resultItem.UnitVolume = item.UnitVolume;
        //                        if (item.UnitVolume != null)
        //                        {
        //                            var totalVolume = item.Qty * item.UnitVolume;
        //                            item.Volume = totalVolume;
        //                        }
        //                        resultItem.Volume = item.Volume;
        //                        resultItem.UnitPrice = item.UnitPrice;
        //                        resultItem.Price = item.Price;
        //                        resultItem.DocumentRefNo1 = item.DocumentRefNo1;
        //                        resultItem.DocumentRefNo2 = item.DocumentRefNo2;
        //                        resultItem.DocumentRefNo3 = item.DocumentRefNo3;
        //                        resultItem.DocumentRefNo4 = item.DocumentRefNo4;
        //                        resultItem.DocumentRefNo5 = item.DocumentRefNo5;
        //                        resultItem.DocumentStatus = document_status;
        //                        resultItem.DocumentRemark = item.DocumentRemark;
        //                        resultItem.UDF1 = item.UDF1;
        //                        resultItem.UDF2 = item.UDF2;
        //                        resultItem.UDF3 = item.UDF3;
        //                        resultItem.UDF4 = item.UDF4;
        //                        resultItem.UDF5 = item.UDF5;
        //                        resultItem.Create_By = data.Create_By;
        //                        resultItem.Create_Date = item.Create_Date;
        //                        resultItem.Update_By = data.Update_By;
        //                        resultItem.Update_Date = item.Update_Date;
        //                        resultItem.Cancel_By = data.Cancel_By;
        //                        resultItem.Cancel_Date = item.Cancel_Date;


        //                        itemDetail.Add(resultItem);




        //                    }

        //                    var itemHeaderlist = new List<PlanGoodsReceiveViewModel>();
        //                    itemHeaderlist.Add(itemHeader);


        //                    //-- SAVE STORE PROC ----//

        //                    DataTable dtHeader = CreateDataTable(itemHeaderlist);
        //                    DataTable dtDetail = CreateDataTable(itemDetail);


        //                    //var PlanGoodsReceive = new SqlParameter("PlanGoodsReceive", dtHeader);
        //                    //PlanGoodsReceive.SqlDbType = SqlDbType.Structured;
        //                    //var PlanGoodsReceiveItem = new SqlParameter("PlanGoodsReceiveItem", dtDetail);
        //                    //PlanGoodsReceiveItem.SqlDbType = SqlDbType.Structured;
        //                    //var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_im_PlanGoodsReceive2 @PlanGoodsReceive,@PlanGoodsReceiveItem", PlanGoodsReceive, PlanGoodsReceiveItem);

        //                    var PlanGoodsReceive = new SqlParameter("PlanGoodsReceive", SqlDbType.Structured);
        //                    PlanGoodsReceive.TypeName = "[dbo].[im_PlanGoodsReceiveData]";
        //                    PlanGoodsReceive.Value = dtHeader;


        //                    var PlanGoodsReceiveItem = new SqlParameter("PlanGoodsReceiveItem", SqlDbType.Structured);
        //                    PlanGoodsReceiveItem.TypeName = "[dbo].[im_PlanGoodsReceiveItemData]";
        //                    PlanGoodsReceiveItem.Value = dtDetail;

        //                    var commandText = "EXEC sp_Save_im_PlanGoodsReceive @PlanGoodsReceive,@PlanGoodsReceiveItem";
        //                    var rowsAffected = context.Database.ExecuteSqlCommand(commandText, PlanGoodsReceive, PlanGoodsReceiveItem);

        //                    //---------------------------//

        //                    result = rowsAffected.ToString();
        //                    //Clear UserAssign
        //                    String SqlUpdatePlanGoodsReceive = " Update im_PlanGoodsReceive set " +
        //                                       " UserAssign =  '' " +
        //                                       " where Convert(Varchar(200),PlanGoodsReceive_Index) ='" + data.PlanGoodsReceiveIndex + "'";
        //                    var row = context.Database.ExecuteSqlCommand(SqlUpdatePlanGoodsReceive);

        //                }
        //            }
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));

            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public actionResultPlanGRPopupViewModel search(PlanGoodsReceivePopupViewModel data)
        {
            try
            {

                using (var context = new GRDbContext())
                {
                    var result = new List<PlanGoodsReceivePopupViewModel>();
                    var actionResultPlanGRPopup = new actionResultPlanGRPopupViewModel();
                    //PlanGR popup GR Page
                    if (data.chk == "1")
                    {
                        string pwhereFilter = "";

                        if (data.PlanGoodsReceiveNo != "" && data.PlanGoodsReceiveNo != null)
                        {
                            pwhereFilter = " And PlanGoodsReceive_No like N'%" + data.PlanGoodsReceiveNo + "%'";
                        }
                        else if (data.VendorName != "" && data.VendorName != null)
                        {
                            pwhereFilter = " And Vendor_Name like N'%" + data.VendorName + "%'";
                        }
                        else if (data.PlanGoodsReceiveDate != "" && data.PlanGoodsReceiveDate != null)
                        {
                            pwhereFilter = " And CAST(PlanGoodsReceive_Date as Date) = N'" + data.PlanGoodsReceiveDate + "'";
                        }
                        else if (data.PlanGoodsReceiveDueDate != "" && data.PlanGoodsReceiveDueDate != null)
                        {
                            pwhereFilter = " And CAST(PlanGoodsReceive_Due_Date as Date) = N'" + data.PlanGoodsReceiveDueDate + "'";
                        }
                        pwhereFilter += " And Document_Status <> -1 ";
                        pwhereFilter += " Order By PlanGoodsReceive_Date Desc ";


                        var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                        var PageNumber = new SqlParameter("@PageNumber", 1);
                        var RowspPage = new SqlParameter("@RowspPage", 10000);

                        var queryResultTotal = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                        var strwhere1 = new SqlParameter("@strwhere", pwhereFilter);
                        var PageNumber1 = new SqlParameter("@PageNumber", data.CurrentPage);
                        var RowspPage1 = new SqlParameter("@RowspPage", data.PerPage);
                        var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                        foreach (var item in query)
                        {
                            var resultItem = new PlanGoodsReceivePopupViewModel();
                            resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                            resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                            resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Due_Date.toString();
                            resultItem.VendorIndex = item.Vendor_Index;
                            resultItem.VendorId = item.Vendor_Id;
                            resultItem.VendorName = item.Vendor_Name;
                            resultItem.OwnerIndex = item.Owner_Index;
                            resultItem.OwnerId = item.Owner_Id;
                            resultItem.OwnerName = item.Owner_Name;
                            resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                            resultItem.DocumentStatus = item.Document_Status;
                            resultItem.WarehouseIndex = item.Warehouse_Index;
                            resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                            resultItem.WarehouseId = item.Warehouse_Id;
                            resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                            resultItem.WarehouseName = item.Warehouse_Name;
                            resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                            resultItem.DocumentTypeIndex = item.DocumentType_Index;
                            resultItem.DocumentRemark = item.Document_Remark;
                            result.Add(resultItem);
                        }
                        var count = queryResultTotal.Count;
                        actionResultPlanGRPopup.itemsPlanGR = result.ToList();
                        actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };
                    }
                    //PlanGR popup GRCreate Page
                    if (data.chk == "2")
                    {
                        string pwhereFilter = "";

                        if (data.PlanGoodsReceiveNo != "" && data.PlanGoodsReceiveNo != null)
                        {
                            pwhereFilter = " And PlanGoodsReceive_No like N'%" + data.PlanGoodsReceiveNo + "%'";
                        }
                        else if (data.VendorName != "" && data.VendorName != null)
                        {
                            pwhereFilter = " And Vendor_Name like N'%" + data.VendorName + "%'";
                        }
                        else if (data.PlanGoodsReceiveDate != "" && data.PlanGoodsReceiveDate != null)
                        {
                            pwhereFilter = " And CAST(PlanGoodsReceive_Date as Date) = N'" + data.PlanGoodsReceiveDate + "'";
                        }
                        else if (data.PlanGoodsReceiveDueDate != "" && data.PlanGoodsReceiveDueDate != null)
                        {
                            pwhereFilter = " And CAST(PlanGoodsReceive_Due_Date as Date) = N'" + data.PlanGoodsReceiveDueDate + "'";
                        }

                        if (data.DocumentTypeIndex.ToString() != "00000000-0000-0000-0000-000000000000" && data.DocumentTypeIndex != null)
                        {
                            pwhereFilter += " and DocumentType_Index in (select DocumentType_Index from sy_DocumentTypeRef where DocumentType_Index_To = '" + data.DocumentTypeIndex + "')";
                        }
                        pwhereFilter += " and Owner_Index = '" + data.OwnerIndex + "'";
                        pwhereFilter += " And Document_Status = 1 ";


                        var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                        var PageNumber = new SqlParameter("@PageNumber", 1);
                        var RowspPage = new SqlParameter("@RowspPage", 10000);

                        var queryResultTotal = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive_Popup @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                        var strwhere1 = new SqlParameter("@strwhere", pwhereFilter);
                        var PageNumber1 = new SqlParameter("@PageNumber", data.CurrentPage);
                        var RowspPage1 = new SqlParameter("@RowspPage", data.PerPage);
                        var query = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive_Popup @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();


                        foreach (var item in query)
                        {
                            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index_To)");
                            var ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id_To");
                            var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Name_To");
                            var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                            var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            var TableName = new SqlParameter("@TableName", "sy_DocumentTypeRef");
                            var Where = new SqlParameter("@Where", "Where DocumentType_Index = '" + item.DocumentType_Index + "'");
                            var DataDocumentTypeRef = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                            var resultItem = new PlanGoodsReceivePopupViewModel();
                            resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                            resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                            resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                            resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Due_Date.toString();
                            resultItem.VendorIndex = item.Vendor_Index;
                            resultItem.VendorId = item.Vendor_Id;
                            resultItem.VendorName = item.Vendor_Name;
                            resultItem.OwnerIndex = item.Owner_Index;
                            resultItem.OwnerId = item.Owner_Id;
                            resultItem.OwnerName = item.Owner_Name;
                            resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                            resultItem.DocumentStatus = item.Document_Status;
                            resultItem.WarehouseIndex = item.Warehouse_Index;
                            resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                            resultItem.WarehouseId = item.Warehouse_Id;
                            resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                            resultItem.WarehouseName = item.Warehouse_Name;
                            resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                            resultItem.DocumentTypeIndex = item.DocumentType_Index;
                            resultItem.DocumentTypeId = item.DocumentType_Id;
                            resultItem.DocumentTypeName = item.DocumentType_Name;
                            resultItem.DocumentRemark = item.Document_Remark;
                            resultItem.GrDocumentTypeIndex = new Guid(DataDocumentTypeRef.dataincolumn1);
                            resultItem.GrDocumentTypeId = DataDocumentTypeRef.dataincolumn2;
                            resultItem.GrDocumentTypeName = DataDocumentTypeRef.dataincolumn3;
                            result.Add(resultItem);
                        }
                        var count = queryResultTotal.Count;
                        actionResultPlanGRPopup.itemsPlanGR = result.ToList();
                        actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };
                    }

                    return actionResultPlanGRPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public actionResultPlanGRViewModel planGRsearch(SearchDetailModel model)
        {
            try
            {

                using (var context = new GRDbContext())
                {
                    string pstring = "";


                    //if (model.PlanGoodsReceiveDate == "" || model.PlanGoodsReceiveDate == null)
                    //{
                    //    pstring += " and PlanGoodsReceive_Date >= Cast(Getdate() as Date) order by PlanGoodsReceive_Date DESC ";
                    //}
                    if (model.PlanGoodsReceiveNo != null && model.PlanGoodsReceiveNo != "")
                    {
                        pstring += " and PlanGoodsReceive_No like'%" + model.PlanGoodsReceiveNo + "%'";
                    }
                    if (model.OwnerIndex.ToString() != "")
                    {
                        pstring += " and Owner_Index ='" + model.OwnerIndex + "'";
                    }
                    else if (model.OwnerName != "" && model.OwnerName != null)
                    {
                        pstring += " and Owner_Name like '%" + model.OwnerName + "%'";
                    }
                    if (model.VendorIndex.ToString() != "")
                    {
                        pstring += " and Vendor_Index ='" + model.VendorIndex + "'";
                    }
                    else if (model.VendorName != "" && model.VendorName != null)
                    {
                        pstring += " and Vender_Name like '%" + model.VendorName + "%'";
                    }
                    if (model.WarehouseIndex.ToString() != "")
                    {
                        pstring += " and Warehouse_Index ='" + model.WarehouseIndex + "'";
                    }
                    else if (model.WarehouseName != "" && model.WarehouseName != null)
                    {
                        pstring += " and Warehouse_Name like '%" + model.WarehouseName + "%'";
                    }
                    if (model.WarehouseIndexTo.ToString() != "")
                    {
                        pstring += " and Warehouse_Index_To ='" + model.WarehouseIndexTo + "'";
                    }
                    else if (model.WarehouseNameTo != "" && model.WarehouseNameTo != null)
                    {
                        pstring += " and Warehouse_Name_To like '%" + model.WarehouseNameTo + "%'";
                    }
                    if (model.DocumentStatus.ToString() != "")
                    {
                        pstring += " and Document_Status ='" + model.DocumentStatus + "'";
                    }
                    else if (model.ProcessStatusName != null && model.ProcessStatusName != "")
                    {
                        pstring += " and ProcessStatus_Name like '%" + model.ProcessStatusName + "%'";
                    }
                    if (model.DocumentTypeIndex.ToString() != "")
                    {
                        pstring += " and DocumentType_Index ='" + model.DocumentTypeIndex + "'";
                    }
                    else if (model.DocumentTypeName != "" && model.DocumentTypeName != null)
                    {
                        pstring += " and DocumentType_Name like '%" + model.DocumentTypeName + "%'";
                    }
                    if ((model.PlanGoodsReceiveDate != "" && model.PlanGoodsReceiveDate != null) && (model.PlanGoodsReceiveDateTo != "" && model.PlanGoodsReceiveDateTo != null))
                    {
                        pstring += " and CAST(PlanGoodsReceive_Date as Date) >= '" + model.PlanGoodsReceiveDate + "'" + " and CAST(PlanGoodsReceive_Date as Date) <= '" + model.PlanGoodsReceiveDateTo + "'";
                    }
                    else if (model.PlanGoodsReceiveDate != "" && model.PlanGoodsReceiveDate != null)
                    {
                        pstring += " and CAST(PlanGoodsReceive_Date as Date) >='" + model.PlanGoodsReceiveDate + "'";
                    }
                    else if (model.PlanGoodsReceiveDateTo != "" && model.PlanGoodsReceiveDateTo != null)
                    {
                        pstring += " and CAST(PlanGoodsReceive_Date as Date) <='" + model.PlanGoodsReceiveDateTo + "'";
                    }
                    if ((model.PlanGoodsReceiveDueDate != null && model.PlanGoodsReceiveDueDate != "") && (model.PlanGoodsReceiveDueDateTo != null && model.PlanGoodsReceiveDueDateTo != ""))
                    {
                        pstring += " and CAST(PlanGoodsReceive_Due_Date as Date) >= '" + model.PlanGoodsReceiveDueDate + "'" + " and CAST(PlanGoodsReceive_Due_Date as Date) <= '" + model.PlanGoodsReceiveDueDateTo + "'";
                    }
                    else if (model.PlanGoodsReceiveDueDate != null && model.PlanGoodsReceiveDueDate != "")
                    {
                        pstring += " and CAST(PlanGoodsReceive_Due_Date as Date) >='" + model.PlanGoodsReceiveDueDate + "'";
                    }
                    else if (model.PlanGoodsReceiveDueDateTo != null && model.PlanGoodsReceiveDueDateTo != "")
                    {
                        pstring += " and CAST(PlanGoodsReceive_Due_Date as Date) <='" + model.PlanGoodsReceiveDueDateTo + "'";
                    }
                    model.Orderby = model.Orderby == "" || model.Orderby == null ? model.Orderby = "ASC" : model.Orderby;
                    if ((model.ColumnName != "" && model.ColumnName != null) && (model.Orderby != "" && model.Orderby != null))
                    {
                        pstring += " Order By " + model.ColumnName + " " + model.Orderby + " ";
                    }
                    else
                    {
                        pstring += " Order By Create_Date DESC ";
                    }

                    if (model.CurrentPage == 0)
                    {
                        model.CurrentPage = 1;
                    }
                    if (model.PerPage == 0)
                    {
                        model.PerPage = 30;
                    }
                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var PageNumber = new SqlParameter("@PageNumber", 1);
                    var RowspPage = new SqlParameter("@RowspPage", 10000);

                    var queryResultTotal = context.View_PlanGrProcessStatus.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                    var strwhere1 = new SqlParameter("@strwhere", pstring);
                    var PageNumber1 = new SqlParameter("@PageNumber", model.CurrentPage);
                    var RowspPage1 = new SqlParameter("@RowspPage", model.PerPage);
                    var query = context.View_PlanGrProcessStatus.FromSql("sp_GetPlanGoodsReceiveByPagination @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                    var result = new List<SearchDetailModel>();
                    foreach (var item in query)
                    {

                        var resultItem = new SearchDetailModel();
                        resultItem.PlanGoodsReceiveIndex = item.PlanGoodsReceive_Index;
                        resultItem.OwnerIndex = item.Owner_Index;
                        resultItem.PlanGoodsReceiveNo = item.PlanGoodsReceive_No;
                        resultItem.OwnerName = item.Owner_Name;
                        resultItem.PlanGoodsReceiveDate = item.PlanGoodsReceive_Date.toString();
                        resultItem.PlanGoodsReceiveDueDate = item.PlanGoodsReceive_Due_Date.toString();
                        resultItem.DocumentTypeIndex = item.DocumentType_Index;
                        resultItem.DocumentTypeName = item.DocumentType_Name;
                        resultItem.VendorName = item.Vendor_Name;
                        resultItem.VendorIndex = item.Vendor_Index;
                        resultItem.WarehouseIndex = item.Warehouse_Index;
                        resultItem.WarehouseName = item.Warehouse_Name;
                        resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                        resultItem.DocumentStatus = item.Document_Status;
                        resultItem.DocumentRemark = item.Document_Remark;
                        resultItem.ProcessStatusName = item.ProcessStatus_Name;
                        resultItem.Item_Document_Remark = item.Item_Document_Remark;
                        result.Add(resultItem);
                    }

                    var count = queryResultTotal.Count;
                    var actionResultPlanGR = new actionResultPlanGRViewModel();
                    actionResultPlanGR.itemsPlanGR = result.ToList();
                    actionResultPlanGR.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };

                    return actionResultPlanGR;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean closeDocument(CloseDocumentViewModel model)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    var docIndex = string.Join(",", model.id);

                    var result = false;
                    string pstring = " and PlanGoodsReceive_Index in (" + string.Join(",", model.id) + ")";
                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var queryResult1 = context.View_CheckCloseDocuments.FromSql("sp_GetConfirmDoc @strwhere", strwhere).ToList();

                    String SqlUpdatePlanReceive = " UPDATE im_PlanGoodsReceive set " +
                                               " Document_Status =  2 " +
                                               " where PlanGoodsReceive_Index in (" + docIndex + ")";

                    var row = context.Database.ExecuteSqlCommand(SqlUpdatePlanReceive);

                    if (row > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                    return result;
                }
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String updateUserAssign(PlanGoodsReceiveDocViewModel item)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new PlanGoodsReceiveDocViewModel();

                    var contextM = new GRDbContext();

                    String SqlUpdatePlanReceive = " Update im_PlanGoodsReceive set " +
                                               " UserAssign =  N'" + item.UserAssign + "'" +
                                               " where Convert(Varchar(200),PlanGoodsReceive_Index) ='" + item.PlanGoodsReceiveIndex + "'";
                    var row = context.Database.ExecuteSqlCommand(SqlUpdatePlanReceive);


                    var strwhere = new SqlParameter("@strwhere", " and PlanGoodsReceive_Index = '" + item.PlanGoodsReceiveIndex + "'");
                    var CheckUser = contextM.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive").FirstOrDefault();

                    return CheckUser.UserAssign.ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String deleteUserAssign(PlanGoodsReceiveDocViewModel item)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new PlanGoodsReceiveDocViewModel();

                    var contextM = new GRDbContext();
                    if (!string.IsNullOrEmpty(item.PlanGoodsReceiveIndex.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                    {
                        String SqlUpdateGoodsReceive = " Update im_PlanGoodsReceive set " +
                                           " UserAssign =  '' " +
                                           " where Convert(Varchar(200),PlanGoodsReceive_Index) ='" + item.PlanGoodsReceiveIndex + "'";
                        var row = context.Database.ExecuteSqlCommand(SqlUpdateGoodsReceive);
                    }
                    else
                    {
                        return "";
                    }


                    var strwhere = new SqlParameter("@strwhere", " and PlanGoodsReceive_Index = '" + item.PlanGoodsReceiveIndex + "'");
                    var CheckUser = contextM.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere", strwhere).FirstOrDefault();

                    return CheckUser.UserAssign.ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> CheckDocumentStatus(CheckDocumentStatusViewModel model)
        {
            try
            {
                var res = new List<string>();

                using (var context = new GRDbContext())
                {
                    var docNo = string.Join(",", model.GoodsReceiveNoList);

                    var strwhere = new SqlParameter("@strwhere", " and PlanGoodsReceive_No in (" + docNo + ")");
                    var queryResult = context.View_CheckDocumentStatus.FromSql("sp_GetViewCheckDocumentStatus @strwhere ", strwhere)
                        .GroupBy(g => new { g.GRDocument_Status, g.PlanGRDocument_Status, g.PlanGoodsReceive_No })
                        .Select(c => new { c.Key.GRDocument_Status, c.Key.PlanGRDocument_Status, c.Key.PlanGoodsReceive_No }).ToList();

                    //ดูว่า Plan GR มี GR หรือยัง
                    if (queryResult.Count > 0)
                    {


                        var isValidateStatus = queryResult.Any(c => c.GRDocument_Status != 3 && c.GRDocument_Status != -99);

                        //ถ้าไม่มี GR Status เป็น 3 1ใบ และไม่ใช่ 3 1ใบ ปิดไม่ได้
                        if (isValidateStatus)
                        {

                            var data = queryResult.Where(c => c.GRDocument_Status != 3 && c.GRDocument_Status != -1).ToList();
                            if (data.Count > 0)
                            {
                                var strwhere1 = new SqlParameter("@strwhere", " and Ref_Document_No in (" + docNo + ")");
                                var chkGRItem = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere ", strwhere1).ToList();

                                var Gritem = chkGRItem.Any(c => c.Document_Status == -1);
                                if (Gritem)
                                {
                                    return res;
                                }
                                else
                                {
                                    foreach (var d in data)
                                    {

                                        if (d.PlanGRDocument_Status == 3)
                                        {
                                            res.Add(d.PlanGoodsReceive_No + " ทำการรับครบถ้วนแล้ว ไม่สามารถ Close เอกสารได้ ");
                                        }
                                        else
                                        {
                                            res.Add(d.PlanGoodsReceive_No + " มีการผูกเอกสารไปแล้ว และยังทำการรับสินค้าไม่เสร็จสิ้น ");
                                        }
                                    }
                                }


                            }
                            else if (data.Count == 0)
                            {
                                var data1 = queryResult.Where(c => c.PlanGRDocument_Status == 2)
                                    .GroupBy(g => new { g.PlanGRDocument_Status, g.PlanGoodsReceive_No })
                                    .Select(c => new { c.Key.PlanGRDocument_Status, c.Key.PlanGoodsReceive_No }).ToList();
                                foreach (var d in data1)
                                {
                                    res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                                }

                            }

                            return res;
                        }
                        //ถ้ามีแล้ว Status ต้องเป็น 3 ถึงจะปิดได้
                        else
                        {
                            isValidateStatus = queryResult.Any(c => c.PlanGRDocument_Status != 0);

                            if (isValidateStatus)
                            {
                                var data = queryResult.ToList();
                                foreach (var d in data)
                                {
                                    switch (d.PlanGRDocument_Status)
                                    {
                                        case 2:
                                            res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                                            break;
                                        case 3:
                                            res.Add(d.PlanGoodsReceive_No + " ทำการรับครบถ้วนแล้ว ไม่สามารถ Close เอกสารได้ ");
                                            break;
                                        case -1:
                                            res.Add(d.PlanGoodsReceive_No + " ทำการ Delete ไปแล้ว ไม่สามารถ Close เอกสารได้ ");
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                return res;
                            }

                            return new List<string>();
                        }
                    }

                    else if (queryResult.Count == 0)
                    {
                        var queryResult1 = context.IM_PlanGoodsReceive.FromSql("sp_GetPlanGoodsReceive @strwhere ", strwhere).ToList();

                        var data = queryResult1.ToList();
                        foreach (var d in data)
                        {
                            switch (d.Document_Status)
                            {
                                //case 1:
                                //    res.Add(d.PlanGoodsReceive_No + " เอกสารมีการ Confirm แล้ว ไม่สามารถ Close เอกสารได้ ");
                                //    break;
                                case 2:
                                    res.Add(d.PlanGoodsReceive_No + " ทำการ Close เอกสารเรียบร้อยแล้ว ไม่สามารถ Close เอกสารได้อีก ");
                                    break;
                                default:
                                    break;
                            }
                        }


                        return res;
                    }

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public actionResultPlanGIPopupViewModel PlanGoodsIssuePopup(ReturnReceiveViewModel data)
        {
            try
            {

                using (var context = new GRDbContext())
                {
                    string pwhereFilter = "";
                    var actionResultPlanGIPopup = new actionResultPlanGIPopupViewModel();

                    var result = new List<ReturnReceiveViewModel>();



                    if (data.PlanGoodsIssueNo != "" && data.PlanGoodsIssueNo != null)
                    {
                        pwhereFilter = " And PlanGoodsIssue_No like N'%" + data.PlanGoodsIssueNo + "%'";
                    }
                    else
                    {
                        //pwhereFilter = " and CAST(PlanGoodsIssue_Due_Date as Date) = '" + DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + "'";
                        //pwhereFilter += " and CAST(PlanGoodsIssue_Due_Date as Date) >= '" + DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + "'" + " and CAST(PlanGoodsIssue_Due_Date as Date) <= '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
                        pwhereFilter += "";
                    }

                    if (data.OwnerName != "" && data.OwnerName != null)
                    {
                        pwhereFilter += " And Owner_Name like N'%" + data.OwnerName + "%'";
                    }
                    else
                    {
                        pwhereFilter += "";
                    }

                    //if (data.DocumentTypeName == "Return from customer")
                    //{
                    //    pwhereFilter += " AND PlanGoodsIssue_Index in (select a.Ref_Document_Index from im_TruckLoadItem a inner join im_TruckLoad b on a.TruckLoad_Index = b.TruckLoad_Index where b.Document_Status = 2)";
                    //}
                    var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                    var query = context.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere", strwhere).ToList();

                    var perpages = data.PerPage == 0 ? query.ToList() : query.Skip((data.CurrentPage - 1) * data.PerPage).Take(data.PerPage).ToList();


                    //var strwhere = new SqlParameter("@strwhere", pwhereFilter);
                    //var PageNumber = new SqlParameter("@PageNumber", 1);
                    //var RowspPage = new SqlParameter("@RowspPage", 1000);

                    //var queryResultTotal = context.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere , @PageNumber , @RowspPage ", strwhere, PageNumber, RowspPage).ToList();

                    //var strwhere1 = new SqlParameter("@strwhere", pwhereFilter);
                    //var PageNumber1 = new SqlParameter("@PageNumber", data.CurrentPage);
                    //var RowspPage1 = new SqlParameter("@RowspPage", data.PerPage);
                    //var query = context.View_ReturnReceive.FromSql("sp_GetPlanGoodsIssueByPaginationPopup @strwhere , @PageNumber , @RowspPage ", strwhere1, PageNumber1, RowspPage1).ToList();

                    foreach (var item in perpages)
                    {
                        var resultItem = new ReturnReceiveViewModel();

                        resultItem.PlanGoodsIssueIndex = item.PlanGoodsIssue_Index;
                        resultItem.PlanGoodsIssueNo = item.PlanGoodsIssue_No;
                        resultItem.PlanGoodsIssueDate = item.PlanGoodsIssue_Date.toString();
                        resultItem.PlanGoodsIssueDueDate = item.PlanGoodsIssue_Due_Date.toString();
                        resultItem.OwnerIndex = item.Owner_Index;
                        resultItem.OwnerId = item.Owner_Id;
                        resultItem.OwnerName = item.Owner_Name;
                        resultItem.DocumentRefNo1 = item.DocumentRef_No1;
                        resultItem.DocumentStatus = item.Document_Status;
                        resultItem.WarehouseIndex = item.Warehouse_Index;
                        resultItem.WarehouseIndexTo = item.Warehouse_Index_To;
                        resultItem.WarehouseId = item.Warehouse_Id;
                        resultItem.WarehouseIdTo = item.Warehouse_Id_To;
                        resultItem.WarehouseName = item.Warehouse_Name;
                        resultItem.WarehouseNameTo = item.Warehouse_Name_To;
                        resultItem.CreateDate = item.Create_Date.toString();
                        resultItem.CreateBy = item.Create_By;
                        resultItem.UpdateDate = item.Update_Date.toString();
                        resultItem.UpdateBy = item.Update_By;
                        resultItem.CancelDate = item.Cancel_Date.toString();
                        resultItem.CancelBy = item.Cancel_By;
                        resultItem.RefPlanGoodsIssueNo = item.Ref_PlanGoodsIssue_No;
                        result.Add(resultItem);
                    }
                    var count = query.Count;
                    actionResultPlanGIPopup = new actionResultPlanGIPopupViewModel();
                    actionResultPlanGIPopup.itemsPlanGI = result.ToList();
                    actionResultPlanGIPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };




                    return actionResultPlanGIPopup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
