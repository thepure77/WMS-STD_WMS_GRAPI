using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness;
using GRBusiness.AutoNumber;
using GRBusiness.ConfigModel;
using GRBusiness.GoodIssue;
using GRBusiness.GoodsReceive;
using GRDataAccess.Models;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using static GRBusiness.GoodsReceive.TaskfilterViewModel;

namespace TransferBusiness.Transfer
{
    public class AssignService
    {
        private GRDbContext db;

        public AssignService()
        {
            db = new GRDbContext();
        }
        public AssignService(GRDbContext db)
        {
            this.db = db;
        }

        #region CreateDataTable
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

        #endregion

        #region assign
        public String assign(AssignJobViewModel data)
        {


            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {
                db.Database.SetCommandTimeout(360);
                var list_GR = data.listGoodsReceiveViewModel.Select(s => s.goodsReceive_Index).ToList();
                var CheckTag = db.View_GRTag.Where(c => list_GR.Contains(c.GoodsReceive_Index)).ToList();
                //var CheckTag = db.View_GRTag.Where(c => data.listGoodsReceiveViewModel.Select(s => s.goodsReceive_Index).Contains(c.GoodsReceive_Index)).ToList();

                foreach (var resultCheckTag in CheckTag)
                {
                    if (resultCheckTag.Tag_No == null)
                    {
                        return "เลขที่ใบรับวัสดุยังไม่ได้ทำการสร้างพาเลท";
                    }
                }

                //var GoodsReceiveItem = db.IM_GoodsReceiveItem.Where(c => data.listGoodsReceiveViewModel.Select(s => s.goodsReceive_Index).Contains(c.GoodsReceive_Index)).ToList();
                var GoodsReceiveItem = db.IM_GoodsReceiveItem.Where(c => list_GR.Contains(c.GoodsReceive_Index)).ToList();

                //var GoodsReceive = db.IM_GoodsReceive.Where(c => data.listGoodsReceiveViewModel.Select(s => s.goodsReceive_Index).Contains(c.GoodsReceive_Index)).ToList();
                var GoodsReceive = db.IM_GoodsReceive.Where(c => list_GR.Contains(c.GoodsReceive_Index)).ToList();

                #region 1 : 1

                if (data.Template == "1")
                {

                    var ViewJoin = (from GRI in GoodsReceiveItem
                                    join GR in GoodsReceive on GRI.GoodsReceive_Index equals GR.GoodsReceive_Index

                                    select new View_AssignJobViewModel
                                    {
                                        goodsReceive_Index = GR.GoodsReceive_Index,
                                        goodsReceive_No = GR.GoodsReceive_No,
                                        goodsReceiveItem_Index = GRI.GoodsReceiveItem_Index,
                                        goodsReceive_Date = GR.GoodsReceive_Date,
                                        qty = GRI.Qty,
                                        totalQty = GRI.TotalQty,

                                    }).AsQueryable();



                    var ResultGroup = ViewJoin.GroupBy(c => new { c.goodsReceive_Index, c.goodsReceive_Date })
                     .Select(group => new
                     {
                         GR = group.Key.goodsReceive_Index,
                         GRD = group.Key.goodsReceive_Date,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();

                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.GR, item.GRD, item.ResultItem, data.Create_By, data.Template);
                    }

                }

                #endregion

                #region Tag

                if (data.Template == "2")
                {

                    var listDataTag = db.wm_TagItem.Where(c => list_GR.Contains(c.GoodsReceive_Index) && c.Tag_Status != -1).ToList();
                    //var listDataTag = db.wm_TagItem.Where(c => data.listGoodsReceiveViewModel.Select(s => s.goodsReceive_Index).Contains(c.GoodsReceive_Index) && c.Tag_Status != -1).ToList();

                    var ViewJoin = (from GRI in GoodsReceiveItem
                                    join GR in GoodsReceive on GRI.GoodsReceive_Index equals GR.GoodsReceive_Index
                                    join Tag in listDataTag on GRI.GoodsReceiveItem_Index equals Tag.GoodsReceiveItem_Index

                                    select new View_AssignJobViewModel
                                    {
                                        goodsReceive_Index = GR.GoodsReceive_Index,
                                        goodsReceive_No = GR.GoodsReceive_No,
                                        goodsReceiveItem_Index = GRI.GoodsReceiveItem_Index,
                                        goodsReceive_Date = GR.GoodsReceive_Date,
                                        tag_Index = Tag.Tag_Index,
                                        qty = GRI.Qty,
                                        totalQty = GRI.TotalQty,
                                        tagItem_Index = Tag.TagItem_Index

                                    }).AsQueryable();



                    var ResultGroup = ViewJoin.GroupBy(c => new { c.tagItem_Index, c.goodsReceive_Date })
                     .Select(group => new
                     {
                         Tag = group.Key.tagItem_Index,
                         GRD = group.Key.goodsReceive_Date,
                         ResultItem = group.OrderByDescending(o => o.location_Id).ThenByDescending(o => o.product_Id).ThenByDescending(o => o.qty).ToList()
                     }).ToList();

                    foreach (var item in ResultGroup)
                    {
                        this.CreateTask(item.Tag, item.GRD, item.ResultItem, data.Create_By, data.Template);

                    }
                }

                #endregion

                #region Update Status GR 

                foreach (var ResultGoodsReceive in GoodsReceive)
                {
                    var FindGoodsReceive = db.IM_GoodsReceive.Find(ResultGoodsReceive.GoodsReceive_Index);
                    FindGoodsReceive.Document_Status = 3;
                }

                #endregion


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveGoodsReceiveTask", msglog);
                    transaction.Rollback();

                    return exy.ToString();

                }

                return "true";

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CreateTask
        public String CreateTask(Guid? Index, DateTime? GRD, List<View_AssignJobViewModel> ResultItem, String Create_By, String Tempalate)
        {
            decimal GRIQty = 0;
            decimal CountQty = 0;
            decimal QtyBreak = 500000;
            String TaskGRIndex = "";
            String TaskGRNo = "";
            String TagNo = "";

            try
            {


                if (Tempalate != "1")
                {
                    foreach (var itemResult in ResultItem)
                    {

                        GRIQty = itemResult.qty.GetValueOrDefault();


                        for (int i = (int)Math.Ceiling(GRIQty); i > 0;)
                        {
                            if (CountQty == 0)
                            {
                                #region Create Task Header


                                #region GenDoc
                                var Gen = new List<DocumentTypeViewModel>();
                                var filterModel = new DocumentTypeViewModel();
                                filterModel.process_Index = new Guid("A787EF04-3E01-48E6-B2F0-31EC995F85D0");
                                filterModel.documentType_Index = new Guid("405C0935-531D-461B-8E12-01C362CB6E81");
                                //GetConfig
                                Gen = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());
                                DataTable resultDocumentType = CreateDataTable(Gen);
                                var genDoc = new AutoNumberService();
                                DateTime DocumentDate = DateTime.Now;
                                TaskGRNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                                #endregion

                                var result = new im_TaskGR();

                                result.TaskGR_Index = Guid.NewGuid();
                                result.TaskGR_No = TaskGRNo;
                                result.Create_By = Create_By;
                                result.Create_Date = DateTime.Now;
                                result.Document_Status = 0;

                                db.im_TaskGR.Add(result);

                                TaskGRIndex = result.TaskGR_Index.ToString();
                                CountQty = QtyBreak;

                                #endregion
                            }

                            #region Create TaskItem

                            var resultItem = new im_TaskGRItem();

                            var FindGRI = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceiveItem_Index == itemResult.goodsReceiveItem_Index).FirstOrDefault();

                            var FindViewGR = db.View_GRTag.Where(c => c.TagItem_Index == Index).FirstOrDefault();


                            if (GRIQty >= CountQty)
                            {

                                resultItem.TaskGRItem_Index = Guid.NewGuid();
                                resultItem.TaskGR_Index = new Guid(TaskGRIndex);
                                resultItem.TaskGR_No = TaskGRNo;
                                resultItem.Tag_Index = FindViewGR.Tag_Index;
                                resultItem.TagItem_Index = FindViewGR.TagItem_Index;
                                resultItem.Tag_No = FindViewGR.Tag_No;
                                resultItem.Product_Index = FindGRI.Product_Index;
                                resultItem.Product_Id = FindGRI.Product_Id;
                                resultItem.Product_Name = FindGRI.Product_Name;
                                resultItem.Product_SecondName = FindGRI.Product_SecondName;
                                resultItem.Product_ThirdName = FindGRI.Product_ThirdName;
                                resultItem.Product_Lot = FindGRI.Product_Lot;
                                resultItem.ItemStatus_Index = FindGRI.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGRI.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGRI.ItemStatus_Name;
                                resultItem.Location_Index = FindViewGR.Suggest_Location_Index;
                                resultItem.Location_Id = FindViewGR.Suggest_Location_Id;
                                resultItem.Location_Name = FindViewGR.Suggest_Location_Name;
                                resultItem.Qty = FindViewGR.Qty;
                                resultItem.Ratio = FindGRI.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGRI.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGRI.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGRI.ProductConversion_Name;
                                resultItem.MFG_Date = FindGRI.MFG_Date;
                                resultItem.EXP_Date = FindGRI.EXP_Date;

                                resultItem.UnitWeight = FindGRI.UnitWeight;
                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGRI.UnitWeight ?? 0);
                                resultItem.UnitWidth = FindGRI.UnitWidth;
                                resultItem.UnitLength = FindGRI.UnitLength;
                                resultItem.UnitHeight = FindGRI.UnitHeight;
                                resultItem.UnitVolume = FindGRI.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGRI.UnitVolume;
                                resultItem.UnitPrice = FindGRI.UnitPrice;


                                resultItem.DocumentRef_No1 = FindGRI.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGRI.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGRI.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGRI.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGRI.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGRI.UDF_1;
                                resultItem.UDF_2 = FindGRI.UDF_2;
                                resultItem.UDF_3 = FindGRI.UDF_3;
                                resultItem.UDF_4 = FindGRI.UDF_2;
                                resultItem.UDF_5 = FindGRI.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                                resultItem.Ref_Document_Index = FindGRI.GoodsReceive_Index;
                                resultItem.Ref_Document_No = itemResult.goodsReceive_No;
                                resultItem.Ref_Document_LineNum = FindGRI.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGRI.GoodsReceiveItem_Index;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.ERP_Location = FindGRI.ERP_Location;

                                db.im_TaskGRItem.Add(resultItem);

                                GRIQty = GRIQty - CountQty;
                                CountQty = 0;
                                i = (int)Math.Ceiling(GRIQty);


                            }

                            else if (GRIQty < QtyBreak)
                            {

                                resultItem.TaskGRItem_Index = Guid.NewGuid();
                                resultItem.TaskGR_Index = new Guid(TaskGRIndex);
                                resultItem.TaskGR_No = TaskGRNo;
                                resultItem.Tag_Index = FindViewGR.Tag_Index;
                                resultItem.TagItem_Index = FindViewGR.TagItem_Index;
                                resultItem.Tag_No = FindViewGR.Tag_No;
                                resultItem.Product_Index = FindGRI.Product_Index;
                                resultItem.Product_Id = FindGRI.Product_Id;
                                resultItem.Product_Name = FindGRI.Product_Name;
                                resultItem.Product_SecondName = FindGRI.Product_SecondName;
                                resultItem.Product_ThirdName = FindGRI.Product_ThirdName;
                                resultItem.Product_Lot = FindGRI.Product_Lot;
                                resultItem.ItemStatus_Index = FindGRI.ItemStatus_Index;
                                resultItem.ItemStatus_Id = FindGRI.ItemStatus_Id;
                                resultItem.ItemStatus_Name = FindGRI.ItemStatus_Name;
                                resultItem.Location_Index = FindViewGR.Suggest_Location_Index;
                                resultItem.Location_Id = FindViewGR.Suggest_Location_Id;
                                resultItem.Location_Name = FindViewGR.Suggest_Location_Name;
                                resultItem.Qty = FindViewGR.Qty;
                                resultItem.Ratio = FindGRI.Ratio;
                                resultItem.TotalQty = (resultItem.Qty * resultItem.Ratio);
                                resultItem.ProductConversion_Index = FindGRI.ProductConversion_Index;
                                resultItem.ProductConversion_Id = FindGRI.ProductConversion_Id;
                                resultItem.ProductConversion_Name = FindGRI.ProductConversion_Name;
                                resultItem.MFG_Date = FindGRI.MFG_Date;
                                resultItem.EXP_Date = FindGRI.EXP_Date;

                                resultItem.UnitWeight = FindGRI.UnitWeight;
                                resultItem.Weight = (resultItem.Qty ?? 0) * (FindGRI.UnitWeight ?? 0);
                                resultItem.UnitWidth = FindGRI.UnitWidth;
                                resultItem.UnitLength = FindGRI.UnitLength;
                                resultItem.UnitHeight = FindGRI.UnitHeight;
                                resultItem.UnitVolume = FindGRI.UnitVolume;
                                resultItem.Volume = (resultItem.Qty ?? 0) * FindGRI.UnitVolume;
                                resultItem.UnitPrice = FindGRI.UnitPrice;

                                resultItem.DocumentRef_No1 = FindGRI.DocumentRef_No1;
                                resultItem.DocumentRef_No2 = FindGRI.DocumentRef_No2;
                                resultItem.DocumentRef_No3 = FindGRI.DocumentRef_No3;
                                resultItem.DocumentRef_No4 = FindGRI.DocumentRef_No4;
                                resultItem.DocumentRef_No5 = FindGRI.DocumentRef_No5;
                                resultItem.Document_Status = 0;
                                resultItem.UDF_1 = FindGRI.UDF_1;
                                resultItem.UDF_2 = FindGRI.UDF_2;
                                resultItem.UDF_3 = FindGRI.UDF_3;
                                resultItem.UDF_4 = FindGRI.UDF_2;
                                resultItem.UDF_5 = FindGRI.UDF_5;
                                resultItem.Ref_Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                                resultItem.Ref_Document_Index = FindGRI.GoodsReceive_Index;
                                resultItem.Ref_Document_No = itemResult.goodsReceive_No;
                                resultItem.Ref_Document_LineNum = FindGRI.LineNum;
                                resultItem.Ref_DocumentItem_Index = FindGRI.GoodsReceiveItem_Index;
                                resultItem.Create_By = Create_By;
                                resultItem.Create_Date = DateTime.Now;
                                resultItem.ERP_Location = FindGRI.ERP_Location;


                                db.im_TaskGRItem.Add(resultItem);

                                CountQty = CountQty - GRIQty;
                                GRIQty = 0;
                                i = (int)Math.Ceiling(GRIQty);

                            }

                            #endregion
                        }

                    }

                }

                else
                {
                    var FindGR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == Index).FirstOrDefault();

                    if (FindGR != null)
                    {

                        #region Create Task Header

                        var result = new im_TaskGR();


                        var Gen = new List<DocumentTypeViewModel>();

                        var filterModel = new DocumentTypeViewModel();


                        filterModel.process_Index = new Guid("A787EF04-3E01-48E6-B2F0-31EC995F85D0");
                        filterModel.documentType_Index = new Guid("405C0935-531D-461B-8E12-01C362CB6E81");
                        //GetConfig
                        Gen = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                        DataTable resultDocumentType = CreateDataTable(Gen);

                        var genDoc = new AutoNumberService();
                        string DocNo = "";
                        DateTime DocumentDate = (DateTime)FindGR.GoodsReceive_Date;
                        DocNo = genDoc.genAutoDocmentNumber(Gen, DocumentDate);
                        TaskGRNo = DocNo;

                        result.TaskGR_Index = Guid.NewGuid();
                        result.TaskGR_No = TaskGRNo;
                        result.Document_Status = 0;
                        result.Create_By = Create_By;
                        result.Create_Date = DateTime.Now;

                        db.im_TaskGR.Add(result);

                        #endregion

                        #region Create TaskItem

                        var FindGRI = db.View_GRTag.Where(c => c.GoodsReceive_Index == Index).ToList();

                        var TaskItem = new List<im_TaskGRItem>();


                        foreach (var listView in FindGRI)
                        {
                            var resultItem = new im_TaskGRItem();


                            resultItem.TaskGRItem_Index = Guid.NewGuid();
                            resultItem.TaskGR_Index = result.TaskGR_Index;
                            resultItem.TaskGR_No = TaskGRNo;
                            resultItem.Tag_Index = listView.Tag_Index;
                            resultItem.TagItem_Index = listView.TagItem_Index;
                            resultItem.Tag_No = listView.Tag_No;
                            resultItem.Product_Index = listView.Product_Index;
                            resultItem.Product_Id = listView.Product_Id;
                            resultItem.Product_Name = listView.Product_Name;
                            resultItem.Product_SecondName = listView.Product_SecondName;
                            resultItem.Product_ThirdName = listView.Product_ThirdName;
                            resultItem.Product_Lot = listView.Product_Lot;
                            resultItem.ItemStatus_Index = listView.ItemStatus_Index;
                            resultItem.ItemStatus_Id = listView.ItemStatus_Id;
                            resultItem.ItemStatus_Name = listView.ItemStatus_Name;
                            resultItem.Location_Index = listView.Suggest_Location_Index;
                            resultItem.Location_Id = listView.Suggest_Location_Id;
                            resultItem.Location_Name = listView.Suggest_Location_Name;
                            resultItem.Qty = listView.Qty;
                            resultItem.Ratio = listView.Ratio;
                            resultItem.TotalQty = listView.TotalQty;
                            resultItem.ProductConversion_Index = listView.ProductConversion_Index;
                            resultItem.ProductConversion_Id = listView.ProductConversion_Id;
                            resultItem.ProductConversion_Name = listView.ProductConversion_Name;
                            resultItem.MFG_Date = listView.MFG_Date;
                            resultItem.EXP_Date = listView.EXP_Date;
                            resultItem.UnitWeight = listView.UnitWeight;
                            resultItem.Weight = listView.Weight;
                            resultItem.UnitWidth = listView.UnitWidth;
                            resultItem.UnitLength = listView.UnitLength;
                            resultItem.UnitHeight = listView.UnitHeight;
                            resultItem.UnitVolume = listView.UnitVolume;
                            resultItem.Volume = listView.Volume;
                            resultItem.UnitPrice = listView.UnitPrice;
                            resultItem.Price = listView.Price;
                            resultItem.DocumentRef_No1 = listView.DocumentRef_No1;
                            resultItem.DocumentRef_No2 = listView.DocumentRef_No2;
                            resultItem.DocumentRef_No3 = listView.DocumentRef_No3;
                            resultItem.DocumentRef_No4 = listView.DocumentRef_No4;
                            resultItem.DocumentRef_No5 = listView.DocumentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.UDF_1 = listView.UDF_1;
                            resultItem.UDF_2 = listView.UDF_2;
                            resultItem.UDF_3 = listView.UDF_3;
                            resultItem.UDF_4 = listView.UDF_2;
                            resultItem.UDF_5 = listView.UDF_5;
                            resultItem.Ref_Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                            resultItem.Ref_Document_Index = listView.GoodsReceive_Index;
                            resultItem.Ref_Document_No = FindGR.GoodsReceive_No;
                            resultItem.Ref_Document_LineNum = listView.LineNum;
                            resultItem.Ref_DocumentItem_Index = listView.GoodsReceiveItem_Index;
                            resultItem.Create_By = Create_By;
                            resultItem.Create_Date = DateTime.Now;
                            resultItem.ERP_Location = listView.ERP_Location;
                            db.im_TaskGRItem.Add(resultItem);
                        }

                        #endregion



                    }

                }



                return "success";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskfilter
        public List<TaskfilterViewModel> taskfilter(TaskfilterViewModel model)
        {
            try
            {
                var query = db.View_TaskGRfilter.AsQueryable();

                var result = new List<TaskfilterViewModel>();

                if (model.listTaskViewModel.Count != 0)
                {

                    query = query.Where(c => model.listTaskViewModel.Select(s => s.goodsReceive_No).Contains(c.Ref_Document_No));

                    var queryresult = query.ToList();

                    foreach (var itemResult in queryresult)
                    {

                        var resultItem = new TaskfilterViewModel();

                        resultItem.taskGR_Index = itemResult.TaskGR_Index;
                        resultItem.goodsReceive_No = itemResult.Ref_Document_No;
                        resultItem.taskGR_No = itemResult.TaskGR_No;
                        resultItem.goodsReceive_Index = itemResult.Ref_Document_Index;
                        resultItem.userAssign = itemResult.UserAssign;
                        resultItem.create_By = itemResult.Create_By;
                        result.Add(resultItem);

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.goodsReceive_No))
                    {
                        query = query.Where(c => c.Ref_Document_No.Contains(model.goodsReceive_No));
                    }
                    else
                    {
                        return result;
                    }

                    var queryresult = query.ToList();


                    foreach (var item in queryresult)
                    {
                        var resultItem = new TaskfilterViewModel();

                        resultItem.taskGR_Index = item.TaskGR_Index;
                        resultItem.goodsReceive_No = item.Ref_Document_No;
                        resultItem.taskGR_No = item.TaskGR_No;
                        resultItem.goodsReceive_Index = item.Ref_Document_Index;
                        resultItem.userAssign = item.UserAssign;
                        resultItem.create_By = item.Create_By;
                        result.Add(resultItem);

                    }
                }


                return result;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region confirmTask
        public String confirmTask(TaskfilterViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                foreach (var item in data.listTaskViewModel)
                {
                    var Task = db.im_TaskGR.Find(item.taskGR_Index);

                    if (Task != null)
                    {
                        Task.Document_Status = 1;
                        Task.Update_By = item.update_By;
                        Task.Assign_By = item.update_By;
                        Task.Update_Date = DateTime.Now;
                        Task.UserAssign = item.userAssign;
                    }

                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("confirmTask", msglog);
                    transaction.Rollback();
                    throw exy;

                }

                return "Done";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region taskPopup
        public List<View_PreviewTaskGRViewModel> taskPopup(View_PreviewTaskGRViewModel model)
        {
            try
            {
                var query = db.View_PreviewTaskGR.AsQueryable();


                query = query.Where(c => c.Ref_Document_Index == model.ref_Document_Index && c.TaskGR_No == model.taskGR_No);



                var Item = query.OrderByDescending(o => o.Product_Id).ToList();

                var result = new List<View_PreviewTaskGRViewModel>();

                foreach (var item in Item)
                {
                    var resultItem = new View_PreviewTaskGRViewModel();

                    DateTime now = DateTime.Now;

                    resultItem.taskGR_Index = item.TaskGR_Index;
                    resultItem.taskGRItem_Index = item.TaskGR_Index;
                    resultItem.taskGR_No = item.TaskGR_No;
                    resultItem.ref_Document_Index = item.Ref_Document_Index;
                    resultItem.ref_Document_No = item.Ref_Document_No;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.location_Index = item.Location_Index;
                    resultItem.location_Id = item.Location_Id;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.qty = string.Format(String.Format("{0:N2}", item.Qty));
                    resultItem.tag_No = item.Tag_No;
                    resultItem.create_By = item.Create_By;
                    resultItem.create_Date = item.Create_Date.toString();
                    resultItem.create_Time = item.Create_Date.ToString("HH:mm");
                    resultItem.assign_By = item.Assign_By;
                    resultItem.userAssign = item.UserAssign;
                    result.Add(resultItem);

                }

                return result.OrderBy(o => o.product_Id).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region  autoGoodTaskGRNo
        public List<ItemListViewModel> autoGoodTaskGRNo(ItemListViewModel data)
        {
            try
            {
                var query = db.View_TaskGR.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Ref_Document_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.Ref_Document_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.Ref_Document_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  autoGoodsReceiveNo
        public List<ItemListViewModel> autoGoodsReceiveNo(ItemListViewModel data)
        {
            try
            {
                var query = db.IM_GoodsReceive.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.GoodsReceive_No.Contains(data.key));

                }

                var items = new List<ItemListViewModel>();

                var result = query.Select(c => new { c.GoodsReceive_No }).Distinct().Take(10).ToList();


                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        name = item.GoodsReceive_No
                    };
                    items.Add(resultItem);

                }

                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region DropdownUser
        public List<UserViewModel> dropdownUser(UserViewModel data)
        {
            try
            {
                var result = new List<UserViewModel>();

                var filterModel = new ProcessStatusViewModel();

                //GetConfig
                result = utils.SendDataApi<List<UserViewModel>>(new AppSettingConfig().GetUrl("dropdownUser"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CheckTagTask
        public TaskfilterViewModel CheckTagTask(TaskfilterViewModel data)
        {
            try
            {
                var result = new TaskfilterViewModel();

                var query = db.im_TaskGRItem.Where(c => c.TaskGR_No == data.taskGR_No && c.Tag_No == data.tag_No).FirstOrDefault();

                result.tag_No = query.Tag_No;
                result.taskGR_Index = query.TaskGR_Index;
                result.taskGR_No = query.TaskGR_No;
                result.taskGRItem_Index = query.TaskGRItem_Index;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CheckTaskSuccess
        public List<TaskfilterViewModel> CheckTaskSuccess(TaskfilterViewModel data)
        {
            try
            {
                var result = new List<TaskfilterViewModel>();

                var query = db.im_TaskGRItem.Where(c => c.TaskGR_No == data.taskGR_No && c.Document_Status == 0).ToList();

                foreach (var item in query)
                {
                    var resultItem = new TaskfilterViewModel();
                    resultItem.taskGR_Index = item.TaskGR_Index;
                    resultItem.taskGRItem_Index = item.TaskGRItem_Index;
                    resultItem.taskGR_No = item.TaskGR_No;
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

        #region deleteTask
        public actionResultTask deleteTask(TaskfilterViewModel model)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String msg = "";

            var actionResult = new actionResultTask();

            try
            {

                var GR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No == model.goodsReceive_No).FirstOrDefault();

                var GRL = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == GR.GoodsReceive_Index && c.Putaway_Status == 1).FirstOrDefault();


                if (GRL != null)
                {
                    actionResult.msg = "ใบรับสินค้า " + model.goodsReceive_No + " มี Putaway ไปแล้ว";

                }
                else
                {
                    var task = db.View_TaskGR.Where(c => c.Ref_Document_No == model.goodsReceive_No).ToList();

                    foreach (var item in task)
                    {

                        #region updateGI

                        var updateGI = db.IM_GoodsReceive.Find(item.Ref_Document_Index);

                        updateGI.Document_Status = 2;
                        updateGI.Update_By = model.userAssign;
                        updateGI.Update_Date = DateTime.Now;

                        #endregion

                        #region updateTask

                        var updateTask = db.im_TaskGR.Find(item.TaskGR_Index);

                        updateTask.Document_Status = -1;
                        updateTask.Update_By = model.userAssign;
                        updateTask.Update_Date = DateTime.Now;

                        #endregion

                        #region updateTaskItem

                        var queryTaskItem = db.im_TaskGRItem.Where(c => c.TaskGR_Index == item.TaskGR_Index).ToList();

                        foreach (var items in queryTaskItem)
                        {

                            var updateTaskItem = db.im_TaskGRItem.Find(items.TaskGRItem_Index);

                            updateTaskItem.Document_Status = -1;
                            updateTaskItem.Update_By = model.userAssign;
                            updateTaskItem.Update_Date = DateTime.Now;
                        }

                        #endregion

                        actionResult.msg = "Delete Success";
                    }
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("deleteTask", msglog);
                    transaction.Rollback();
                    throw exy;

                }

                return actionResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


    }
}
