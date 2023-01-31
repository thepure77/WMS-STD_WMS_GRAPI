using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace GRBusiness.LPN
{
    public class LPNService
    {
        private GRDbContext db;

        public LPNService()
        {
            db = new GRDbContext();
        }

        public LPNService(GRDbContext db)
        {
            this.db = db;
        }

        public List<LPNViewModel> Filter()
        {
            try
            {
                using (var context = new GRDbContext())
                {


                    var queryResult = context.WM_Tag.FromSql("sp_GetLastTag").FirstOrDefault();



                    var result = new List<LPNViewModel>();


                    var resultItem = new LPNViewModel();

                    resultItem.tag_Index = queryResult.Tag_Index;
                    resultItem.tag_No = queryResult.Tag_No;
                    resultItem.Pallet_No = queryResult.Pallet_No;
                    resultItem.Pallet_Index = queryResult.Pallet_Index;
                    resultItem.tagRef_No5 = queryResult.Tag_No;
                    resultItem.create_Date = queryResult.Create_Date.GetValueOrDefault();
                    resultItem.create_By = queryResult.Create_By;
                    resultItem.update_Date = queryResult.Update_Date.GetValueOrDefault();
                    resultItem.update_By = queryResult.Update_By;
                    resultItem.cancel_Date = queryResult.Cancel_Date.GetValueOrDefault();
                    resultItem.cancel_By = queryResult.Cancel_By;

                    result.Add(resultItem);

                    return result;

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region CheckType_Po_SubContrack
        public bool CheckType_Po_SubContrack(GoodsReceiveViewModel data)
        {
            try
            {
                var checkGR = (from GR in db.IM_GoodsReceive
                           join GRI in db.IM_GoodsReceiveItem on GR.GoodsReceive_Index equals GRI.GoodsReceive_Index
                           select new
                           {
                               GR.GoodsReceive_No,
                               GR.GoodsReceive_Index,
                               GRI.Ref_Document_Index
                           }).Where(c=> c.GoodsReceive_No == data.goodsReceive_No).GroupBy(c=> new { c.Ref_Document_Index ,c.GoodsReceive_No ,c.GoodsReceive_Index }).Select(c=> new { c.Key.GoodsReceive_Index , c.Key.Ref_Document_Index , c.Key.GoodsReceive_No}).ToList();

                if (checkGR.Count > 0)
                {
                    foreach (var item in checkGR)
                    {
                        IM_PlanGoodsReceive checkplan = db.IM_PlanGoodsReceive.FirstOrDefault(c => c.PlanGoodsReceive_Index == item.Ref_Document_Index);
                        if (checkplan == null) { continue; }
                        else
                        {
                            if (checkplan.UDF_3 != "" && checkplan.UDF_3 != null)
                            {
                                if (checkplan.UDF_3 == "PO SubContract")
                                {
                                    List<Po_subcontact> check_Po = db.Po_subcontact.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).ToList();
                                    if (check_Po.Count <=0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public String CreateTagHeader(GoodsReceiveViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";

            try
            {
                //var Tag = db.wm_TagItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index).FirstOrDefault();
                //if (Tag != null)
                //{
                //    var chkTag = db.wm_TagItem.Where(c => c.Tag_Index == Tag.Tag_Index).FirstOrDefault();
                //    if (chkTag != null)
                //    {
                //        return chkTag.Tag_Index.ToString();
                //    }
                //}



                var query = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index).FirstOrDefault();

                var dateNow = DateTime.Now;
                var filterModel = new DocumentTypeViewModel();
                var result = new List<DocumentTypeViewModel>();
                WM_Tag itemHeader = new WM_Tag();

                if (string.IsNullOrEmpty(data.tag_no))
                {

                    filterModel.process_Index = new Guid("58400298-4347-488c-af71-76b78a44232d");
                    filterModel.documentType_Index = new Guid("cebe721a-6bbc-4082-a585-6f75f06f0e31");
                    //GetConfig
                    result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    DataTable resultDocumentType = CreateDataTable(result);

                    var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
                    DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
                    DocumentType.Value = resultDocumentType;

                    var DocumentType_Index = new SqlParameter("@DocumentType_Index", filterModel.documentType_Index);
                    var DocDate = new SqlParameter("@DocDate", dateNow);
                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    resultParameter.Size = 2000; // some meaningfull value
                    resultParameter.Direction = ParameterDirection.Output;
                    db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
                    TagNo = resultParameter.Value.ToString();
                }
                else
                {
                    TagNo = data.tag_no;
                }

                State = "CreateTagHeader";
                //WM_Tag itemHeader = new WM_Tag();

                itemHeader.Tag_Index = Guid.NewGuid();
                itemHeader.Tag_No = data.tag_no == "" ? data.tag_no : TagNo;
                itemHeader.Pallet_No = "";
                itemHeader.Pallet_Index = null;
                itemHeader.TagRef_No1 = null;
                itemHeader.TagRef_No2 = null;
                itemHeader.TagRef_No3 = null;
                itemHeader.TagRef_No4 = null;
                itemHeader.TagRef_No5 = null;
                itemHeader.Tag_Status = 0;
                itemHeader.UDF_1 = query?.UDF_1;
                itemHeader.UDF_2 = query?.UDF_2;
                itemHeader.UDF_3 = query?.UDF_3;
                itemHeader.UDF_4 = query?.UDF_4;
                itemHeader.UDF_5 = query?.UDF_5;
                itemHeader.Create_By = data.create_By;
                itemHeader.Create_Date = DateTime.Now;

                db.WM_Tag.Add(itemHeader);
                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                    return itemHeader.Tag_Index.ToString();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("CreateTagHeader", msglog);
                    transaction.Rollback();
                    throw exy;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }




        #region CreateTagItems
        public String CreateTagItems(GoodsReceiveTagItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";
            Guid index_logid = Guid.NewGuid();
            try
            {
                olog.DataLogLines("CreateTagItems", "CreateTagItems", "Start : " + index_logid + " : " + DateTime.Now.ToString("yyyy-MM-dd-HHmmss "));
                olog.DataLogLines("CreateTagItems", "CreateTagItems", "item : " + index_logid + " : " + JsonConvert.SerializeObject(data));
                //var query = db.WM_Tag.Where(c => c.Tag_Index == data.tag_Index).FirstOrDefault();
                string tagIndex = "";


                foreach (var item in data.listPlanGoodsReceiveItemViewModel)
                {
                    if (data.tag_Index == null)
                    {
                        GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                        grModel.goodsReceive_Index = data.goodsReceive_Index;
                        grModel.create_By = data.create_By;

                        tagIndex = CreateTagHeader(grModel);
                    }
                    else
                    {
                        tagIndex = data.tag_Index.ToString();
                    }

                    var query = db.WM_Tag.Where(c => c.Tag_Index == new Guid(tagIndex)).FirstOrDefault();

                    var GRItem = db.IM_GoodsReceiveItem.Find(item.goodsReceiveItem_Index);

                    //Standard
                    WM_TagItem resultItem = new WM_TagItem();

                    resultItem.TagItem_Index = Guid.NewGuid();
                    resultItem.Tag_Index = new Guid(tagIndex);
                    resultItem.Tag_No = query.Tag_No;
                    resultItem.GoodsReceive_Index = item.goodsReceive_Index;
                    resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                    resultItem.Process_Index = item.ref_Process_Index;
                    resultItem.Product_Index = item.product_Index;
                    resultItem.Product_Id = item.product_Id;
                    resultItem.Product_Name = item.product_Name;
                    resultItem.Product_SecondName = item.product_SecondName;
                    resultItem.Product_ThirdName = item.product_ThirdName;
                    resultItem.Product_Lot = item.product_Lot;
                    resultItem.ItemStatus_Index = item.itemStatus_Index;
                    resultItem.ItemStatus_Id = item.itemStatus_Id;
                    resultItem.ItemStatus_Name = item.itemStatus_Name;
                    resultItem.Qty = item.remainingQty;
                    resultItem.Ratio = item.ratio;
                    resultItem.TotalQty = item.qty * item.ratio;
                    resultItem.ProductConversion_Index = item.productConversion_Index;
                    resultItem.ProductConversion_Id = item.productConversion_Id;
                    resultItem.ProductConversion_Name = item.productConversion_Name;

                    resultItem.Weight = (item.weight / item.qty) * item.remainingQty;//item.weight;
                    resultItem.UnitWeight = item.unitWeight;
                    resultItem.NetWeight = (GRItem.NetWeight / item.qty) * item.remainingQty;
                    resultItem.Weight_Index = GRItem.Weight_Index;
                    resultItem.Weight_Id = GRItem.Weight_Id;
                    resultItem.Weight_Name = GRItem.Weight_Name;
                    resultItem.WeightRatio = GRItem.WeightRatio;

                    resultItem.UnitGrsWeight = (GRItem.UnitGrsWeight / item.qty) * item.remainingQty;
                    resultItem.GrsWeight = (GRItem.GrsWeight / item.qty) * item.remainingQty;
                    resultItem.GrsWeight_Index = GRItem.GrsWeight_Index;
                    resultItem.GrsWeight_Id = GRItem.GrsWeight_Id;
                    resultItem.GrsWeight_Name = GRItem.GrsWeight_Name;
                    resultItem.GrsWeightRatio = GRItem.GrsWeightRatio;

                    resultItem.UnitWidth = (GRItem.UnitWidth / item.qty) * item.remainingQty;
                    resultItem.Width = (GRItem.Width / item.qty) * item.remainingQty;
                    resultItem.Width_Index = GRItem.Width_Index;
                    resultItem.Width_Id = GRItem.Width_Id;
                    resultItem.Width_Name = GRItem.Width_Name;
                    resultItem.WidthRatio = GRItem.WidthRatio;

                    resultItem.UnitLength = (GRItem.UnitLength / item.qty) * item.remainingQty;
                    resultItem.Length = (GRItem.Length / item.qty) * item.remainingQty;
                    resultItem.Length_Index = GRItem.Length_Index;
                    resultItem.Length_Id = GRItem.Length_Id;
                    resultItem.Length_Name = GRItem.Length_Name;
                    resultItem.LengthRatio = GRItem.LengthRatio;

                    resultItem.UnitHeight = (GRItem.UnitHeight / item.qty) * item.remainingQty;
                    resultItem.Height = (GRItem.Height / item.qty) * item.remainingQty;
                    resultItem.Height_Index = GRItem.Height_Index;
                    resultItem.Height_Id = GRItem.Height_Id;
                    resultItem.Height_Name = GRItem.Height_Name;
                    resultItem.HeightRatio = GRItem.HeightRatio;

                    resultItem.Volume = (item.volume / item.qty) * item.remainingQty;//item.volume;
                    resultItem.UnitVolume = (GRItem.UnitVolume / item.qty) * item.remainingQty;

                    resultItem.UnitPrice = (GRItem.UnitPrice / item.qty) * item.remainingQty;
                    resultItem.Price = (GRItem.Height / item.qty) * item.remainingQty;
                    resultItem.TotalPrice = GRItem.TotalPrice;

                    resultItem.Currency_Index = GRItem.Currency_Index;
                    resultItem.Currency_Id = GRItem.Currency_Id;
                    resultItem.Currency_Name = GRItem.Currency_Name;

                    resultItem.Ref_Code1 = GRItem.Ref_Code1;
                    resultItem.Ref_Code2 = GRItem.Ref_Code2;
                    resultItem.Ref_Code3 = GRItem.Ref_Code3;
                    resultItem.Ref_Code4 = GRItem.Ref_Code4;
                    resultItem.Ref_Code5 = GRItem.Ref_Code5;

                    resultItem.MFG_Date = item.mFG_Date.toDate();
                    resultItem.EXP_Date = item.eXP_Date.toDate();
                    resultItem.TagRef_No1 = "";
                    resultItem.TagRef_No2 = "";
                    resultItem.TagRef_No3 = "";
                    resultItem.TagRef_No4 = "";
                    resultItem.TagRef_No5 = "";
                    resultItem.Tag_Status = 0;
                    resultItem.UDF_1 = item.uDF_1;
                    resultItem.UDF_2 = item.uDF_2;
                    resultItem.UDF_3 = item.uDF_3;
                    resultItem.UDF_4 = item.uDF_4;
                    resultItem.UDF_5 = item.uDF_5;
                    resultItem.Create_By = data.create_By;
                    resultItem.Create_Date = DateTime.Now;
                    resultItem.ERP_Location = GRItem.ERP_Location;
                    olog.DataLogLines("CreateTagItems", "CreateTagItems", "resultItem : " + index_logid + " : " + JsonConvert.SerializeObject(resultItem));
                    db.wm_TagItem.Add(resultItem);
                    
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    olog.DataLogLines("CreateTagItems", "CreateTagItems", "ex : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("SaveTag", msglog);
                    transaction.Rollback();

                    throw ex;

                }

                return "";
            }
            catch (Exception ex)
            {
                olog.DataLogLines("CreateTagItems", "CreateTagItems", "ex_2 : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }
        #endregion

        #region CreateTagItems
        public String CreateTagItemsV2(GoodsReceiveTagItemViewModel item)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";
            Guid index_logid = Guid.NewGuid();
            olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "Start : "+index_logid +" : "+ DateTime.Now.ToString("yyyy-MM-dd-HHmmss "));
            olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "item : " + index_logid + " : " + JsonConvert.SerializeObject(item));
            try
            {
                //var query = db.WM_Tag.Where(c => c.Tag_Index == data.tag_Index).FirstOrDefault();



                var query = db.WM_Tag.Where(c => c.Tag_Index == new Guid(item.tag_Index.ToString())).FirstOrDefault();
                olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "query : " + index_logid + " : " + JsonConvert.SerializeObject(query));

                //Standard
                WM_TagItem resultItem = new WM_TagItem();


                if (!string.IsNullOrEmpty(item.tagItem_Index.ToString()))
                {
                    resultItem.TagItem_Index = item.tagItem_Index;
                }
                else
                {
                    resultItem.TagItem_Index = Guid.NewGuid();
                }

                resultItem.Tag_Index = new Guid(item.tag_Index?.ToString());
                resultItem.Tag_No = query.Tag_No;
                resultItem.GoodsReceive_Index = item.goodsReceive_Index;
                resultItem.GoodsReceiveItem_Index = new Guid(item.goodsReceiveItem_Index?.ToString());
                resultItem.Process_Index = item.process_Index;
                resultItem.Product_Index = item.product_Index;
                resultItem.Product_Id = item.product_Id;
                resultItem.Product_Name = item.product_Name;
                resultItem.Product_SecondName = item.product_SecondName;
                resultItem.Product_ThirdName = item.product_ThirdName;
                resultItem.Product_Lot = item.product_Lot;
                resultItem.ItemStatus_Index = item.itemStatus_Index;
                resultItem.ItemStatus_Id = item.itemStatus_Id;
                resultItem.ItemStatus_Name = item.itemStatus_Name;
                resultItem.Qty = item.qty;
                resultItem.Ratio = item.productConversion_Ratio;
                resultItem.TotalQty = item.totalQty;
                resultItem.ProductConversion_Index = item.productConversion_Index;
                resultItem.ProductConversion_Id = item.productConversion_Id;
                resultItem.ProductConversion_Name = item.productConversion_Name;
                resultItem.Weight = item.weight;
                resultItem.Volume = item.volume;
                resultItem.MFG_Date = item.mfg_Date.toDate();
                resultItem.EXP_Date = item.exp_Date.toDate();
                resultItem.TagRef_No1 = "";
                resultItem.TagRef_No2 = "";
                resultItem.TagRef_No3 = "";
                resultItem.TagRef_No4 = "";
                resultItem.TagRef_No5 = "";
                resultItem.Tag_Status = 0;
                resultItem.UDF_1 = item.udf_1;
                resultItem.UDF_2 = item.udf_2;
                resultItem.UDF_3 = item.udf_3;
                resultItem.UDF_4 = item.udf_4;
                resultItem.UDF_5 = item.udf_5;
                resultItem.Create_By = item.create_By;
                resultItem.Create_Date = DateTime.Now;
                resultItem.Suggest_Location_Index = item.suggest_Location_Index;
                resultItem.Suggest_Location_Id = item.suggest_Location_Id;
                resultItem.Suggest_Location_Name = item.suggest_Location_Name;
                resultItem.ERP_Location = item.erp_Location;

                olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "resultItem : " + index_logid + " : " + JsonConvert.SerializeObject(resultItem));
                db.wm_TagItem.Add(resultItem);

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "ex : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("SaveTag", msglog);
                    transaction.Rollback();

                    throw ex;

                }

                return "";
            }
            catch (Exception ex)
            {
                olog.DataLogLines("CreateTagItemsV2", "CreateTagItemsV2", "ex_2 : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }
        #endregion

        #region CreateTagItemsV3
        public Guid? CreateTagItemsV3(GoodsReceiveTagItemViewModel item)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";
            Guid index_logid = Guid.NewGuid();
            olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "Start : " + index_logid + " : " + DateTime.Now.ToString("yyyy-MM-dd-HHmmss "));
            olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "item : " + index_logid + " : " + JsonConvert.SerializeObject(item));
            try
            {
                //var query = db.WM_Tag.Where(c => c.Tag_Index == data.tag_Index).FirstOrDefault();



                var query = db.WM_Tag.Where(c => c.Tag_Index == new Guid(item.tag_Index.ToString())).FirstOrDefault();
                olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "query : " + index_logid + " : " + JsonConvert.SerializeObject(query));

                //Standard
                WM_TagItem resultItem = new WM_TagItem();


                if (!string.IsNullOrEmpty(item.tagItem_Index.ToString()))
                {
                    resultItem.TagItem_Index = item.tagItem_Index;
                }
                else
                {
                    resultItem.TagItem_Index = Guid.NewGuid();
                }

                resultItem.Tag_Index = new Guid(item.tag_Index?.ToString());
                resultItem.Tag_No = query.Tag_No;
                resultItem.GoodsReceive_Index = item.goodsReceive_Index;
                resultItem.GoodsReceiveItem_Index = new Guid(item.goodsReceiveItem_Index?.ToString());
                resultItem.Process_Index = item.process_Index;
                resultItem.Product_Index = item.product_Index;
                resultItem.Product_Id = item.product_Id;
                resultItem.Product_Name = item.product_Name;
                resultItem.Product_SecondName = item.product_SecondName;
                resultItem.Product_ThirdName = item.product_ThirdName;
                resultItem.Product_Lot = item.product_Lot;
                resultItem.ItemStatus_Index = item.itemStatus_Index;
                resultItem.ItemStatus_Id = item.itemStatus_Id;
                resultItem.ItemStatus_Name = item.itemStatus_Name;
                resultItem.Qty = item.qty;
                resultItem.Ratio = item.productConversion_Ratio;
                resultItem.TotalQty = item.totalQty;
                resultItem.ProductConversion_Index = item.productConversion_Index;
                resultItem.ProductConversion_Id = item.productConversion_Id;
                resultItem.ProductConversion_Name = item.productConversion_Name;
                resultItem.Weight = item.weight;
                resultItem.Volume = item.volume;
                resultItem.MFG_Date = item.mfg_Date.toDate();
                resultItem.EXP_Date = item.exp_Date.toDate();
                resultItem.TagRef_No1 = "";
                resultItem.TagRef_No2 = "";
                resultItem.TagRef_No3 = "";
                resultItem.TagRef_No4 = "";
                resultItem.TagRef_No5 = "";
                resultItem.Tag_Status = 0;
                resultItem.UDF_1 = item.udf_1;
                resultItem.UDF_2 = item.udf_2;
                resultItem.UDF_3 = item.udf_3;
                resultItem.UDF_4 = item.udf_4;
                resultItem.UDF_5 = item.udf_5;
                resultItem.Create_By = item.create_By;
                resultItem.Create_Date = DateTime.Now;
                resultItem.Suggest_Location_Index = item.suggest_Location_Index;
                resultItem.Suggest_Location_Id = item.suggest_Location_Id;
                resultItem.Suggest_Location_Name = item.suggest_Location_Name;
                resultItem.ERP_Location = item.erp_Location;
                olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "resultItem : " + index_logid + " : " + JsonConvert.SerializeObject(resultItem));
                db.wm_TagItem.Add(resultItem);


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "ex : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("SaveTag", msglog);
                    transaction.Rollback();

                    throw ex;

                }

                return resultItem.TagItem_Index;
            }
            catch (Exception ex)
            {
                olog.DataLogLines("CreateTagItemsV3", "CreateTagItemsV3", "ex_2 : " + index_logid + " : " + JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }
        #endregion

        #region CreateTagItemsOneToMany
        public String CreateTagItemsOneToMany(GoodsReceiveTagItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";

            try
            {
                decimal sumQty = 0;
                decimal qty = 0;
                decimal qtyPerTag = 0;
                decimal qtyOfTag = 0;

                decimal qtyTagGen = 0;
                decimal countTag = 0;
                decimal qtyTag = 0;

                decimal weightOfQty = 0;
                decimal weight = 0;

                decimal volumeOfQty = 0;
                decimal volume = 0;

                //decimal witdOfQty = 0;
                //decimal width = 0;

                //decimal lenghtOfQty = 0;
                //decimal lenght = 0;

                //decimal heightOfQty = 0;
                //decimal height = 0;


                if (data.qtyGenTag.qtyPerTag > 0 && data.qtyGenTag.qtyOfTag > 0)
                {
                    qty = data.qtyGenTag.qty;

                    qtyPerTag = data.qtyGenTag.qtyPerTag;
                    qtyTagGen = data.qtyGenTag.qtyOfTag;

                    weightOfQty = data.qtyGenTag.weight / data.qtyGenTag.qty;
                    weight = weightOfQty * qtyPerTag;

                    volumeOfQty = data.qtyGenTag.volume / data.qtyGenTag.qty;
                    volume = volumeOfQty * qtyPerTag;

                }
                else if (data.qtyGenTag.qtyPerTag > 0)
                {
                    qty = data.qtyGenTag.qty;
                    qtyPerTag = data.qtyGenTag.qtyPerTag;



                    weightOfQty = data.qtyGenTag.weight/ data.qtyGenTag.qty;
                    weight = weightOfQty * qtyPerTag;

                    volumeOfQty = data.qtyGenTag.volume / data.qtyGenTag.qty;
                    volume = volumeOfQty * qtyPerTag;

                    countTag = qty / qtyPerTag;
                    qtyTagGen = Math.Ceiling(countTag);
                }

                else if (data.qtyGenTag.qtyOfTag > 0)
                {
                    qty = data.qtyGenTag.qty;
                    qtyOfTag = data.qtyGenTag.qtyOfTag;

                

                    qtyTag = Math.Floor(qty / qtyOfTag);

                    weightOfQty = data.qtyGenTag.weight / data.qtyGenTag.qty;
                    weight = weightOfQty * qtyTag;

                    volumeOfQty = data.qtyGenTag.volume / data.qtyGenTag.qty;
                    volume = volumeOfQty * qtyTag;

                    qtyTagGen = qtyOfTag;
                }

                //var query = db.WM_Tag.Where(c => c.Tag_Index == data.tag_Index).FirstOrDefault();

                for (int i = 1; i <= qtyTagGen; i++)
                {
                    GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                    grModel.goodsReceive_Index = data.goodsReceive_Index;
                    grModel.create_By = data.create_By;

                    string tagIndex = CreateTagHeader(grModel);

                    var query = db.WM_Tag.Where(c => c.Tag_Index == new Guid(tagIndex)).FirstOrDefault();

                    WM_TagItem resultItem = new WM_TagItem();

                    GoodsReceiveItemViewModel item = data.listPlanGoodsReceiveItemViewModel[0];

                    var GRItem = db.IM_GoodsReceiveItem.Find(item.goodsReceiveItem_Index);


                    resultItem.TagItem_Index = Guid.NewGuid();
                    resultItem.Tag_Index = new Guid(tagIndex);
                    resultItem.Tag_No = query.Tag_No;
                    resultItem.GoodsReceive_Index = item.goodsReceive_Index;
                    resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                    resultItem.Process_Index = item.ref_Process_Index;
                    resultItem.Product_Index = item.product_Index;
                    resultItem.Product_Id = item.product_Id;
                    resultItem.Product_Name = item.product_Name;
                    resultItem.Product_SecondName = item.product_SecondName;
                    resultItem.Product_ThirdName = item.product_ThirdName;
                    resultItem.Product_Lot = item.product_Lot;
                    resultItem.ItemStatus_Index = item.itemStatus_Index;
                    resultItem.ItemStatus_Id = item.itemStatus_Id;
                    resultItem.ItemStatus_Name = item.itemStatus_Name;
                    //resultItem.Qty = item.qty;
                    resultItem.Ratio = item.ratio;
                    resultItem.ProductConversion_Index = item.productConversion_Index;
                    resultItem.ProductConversion_Id = item.productConversion_Id;
                    resultItem.ProductConversion_Name = item.productConversion_Name;
                    //resultItem.Weight = item.weight;
                    //resultItem.Volume = item.volume;
                    resultItem.MFG_Date = item.mFG_Date.toDate();
                    resultItem.EXP_Date = item.eXP_Date.toDate();
                    resultItem.TagRef_No1 = "";
                    resultItem.TagRef_No2 = "";
                    resultItem.TagRef_No3 = "";
                    resultItem.TagRef_No4 = "";
                    resultItem.TagRef_No5 = "";
                    resultItem.Tag_Status = 0;
                    resultItem.UDF_1 = item.uDF_1;
                    resultItem.UDF_2 = item.uDF_2;
                    resultItem.UDF_3 = item.uDF_3;
                    resultItem.UDF_4 = item.uDF_4;
                    resultItem.UDF_5 = item.uDF_5;
                    resultItem.Create_By = item.create_By;
                    resultItem.Create_Date = DateTime.Now;

                    resultItem.UnitWeight = GRItem.UnitWeight;
                    resultItem.WeightRatio = GRItem.WeightRatio;

                    resultItem.UnitGrsWeight = GRItem.UnitGrsWeight;
                    resultItem.GrsWeightRatio = GRItem.GrsWeightRatio;

                    resultItem.UnitWidth = GRItem.Width;
                    resultItem.WidthRatio = GRItem.WidthRatio;

                    resultItem.UnitLength = GRItem.Length;
                    resultItem.LengthRatio = GRItem.LengthRatio;

                    resultItem.UnitHeight = GRItem.UnitHeight;
                    resultItem.HeightRatio = GRItem.HeightRatio;

                    resultItem.UnitVolume = GRItem.UnitVolume;

                    resultItem.TotalPrice = GRItem.TotalPrice;

                    resultItem.Currency_Index = GRItem.Currency_Index;
                    resultItem.Currency_Id = GRItem.Currency_Id;
                    resultItem.Currency_Name = GRItem.Currency_Name;

                    resultItem.Ref_Code1 = GRItem.Ref_Code1;
                    resultItem.Ref_Code2 = GRItem.Ref_Code2;
                    resultItem.Ref_Code3 = GRItem.Ref_Code3;
                    resultItem.Ref_Code4 = GRItem.Ref_Code4;
                    resultItem.Ref_Code5 = GRItem.Ref_Code5;
                    resultItem.ERP_Location = GRItem.ERP_Location;

                    if (data.qtyGenTag.qtyPerTag > 0 && data.qtyGenTag.qtyOfTag > 0)
                    {
                        if ((qtyPerTag + sumQty) >= qty)
                        {
                            resultItem.Qty = qty - sumQty;
                            resultItem.Weight = weightOfQty * (qty - sumQty);
                            resultItem.Volume = volumeOfQty * (qty - sumQty);

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty* GRItem.UnitWeight;

                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            i = (int)qtyTagGen;
                        }
                        else
                        {
                            resultItem.Qty = qtyPerTag;
                            resultItem.Weight = weight;
                            resultItem.Volume = volume;
                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                        }
                    }
                    else if (data.qtyGenTag.qtyPerTag > 0)
                    {
                        if (i == qtyTagGen)
                        {
                            resultItem.Qty = qty - sumQty;
                            resultItem.Weight = weightOfQty * (qty - sumQty);
                            resultItem.Volume = volumeOfQty * (qty - sumQty);
                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                        }
                        else
                        {
                            resultItem.Qty = qtyPerTag;
                            resultItem.Weight = weight;
                            resultItem.Volume = volume;
                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                        }
                    }
                    else if (data.qtyGenTag.qtyOfTag > 0)
                    {
                        if (i == qtyTagGen)
                        {
                            resultItem.Qty = qty - sumQty;
                            resultItem.Weight = weightOfQty * (qty - sumQty);
                            resultItem.Volume = volumeOfQty * (qty - sumQty);
                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                        }
                        else
                        {
                            resultItem.Qty = qtyTag;
                            resultItem.Weight = weight;
                            resultItem.Volume = volume;
                            resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                            resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                            resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                            resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                            resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                            resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                        }
                    }

                    sumQty += data.qtyGenTag.qtyPerTag > 0 ? qtyPerTag : qtyTag;

                    db.wm_TagItem.Add(resultItem);
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("SaveTag", msglog);
                    transaction.Rollback();

                    throw ex;

                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public GoodsReceiveViewModel FilterGR(SearchGRModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    var queryResult = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No == data.goodsReceive_No).FirstOrDefault();

                    var resultItem = new GoodsReceiveViewModel();

                    resultItem.goodsReceive_Index = queryResult.GoodsReceive_Index;
                    resultItem.goodsReceive_No = queryResult.GoodsReceive_No;
                    resultItem.goodsReceive_Date = queryResult.GoodsReceive_Date.toString();
                    resultItem.document_Status = queryResult.Document_Status;
                    resultItem.documentType_Index = queryResult.DocumentType_Index;
                    resultItem.documentType_Name = queryResult.DocumentType_Name;
                    resultItem.documentType_Id = queryResult.DocumentType_Id;
                    resultItem.owner_Index = queryResult.Owner_Index;
                    resultItem.owner_Id = queryResult.Owner_Id;
                    resultItem.owner_Name = queryResult.Owner_Name;
                    resultItem.documentRef_No1 = queryResult.DocumentRef_No1;
                    resultItem.documentRef_No2 = queryResult.DocumentRef_No2;
                    resultItem.documentRef_No3 = queryResult.DocumentRef_No3;
                    resultItem.documentRef_No4 = queryResult.DocumentRef_No4;
                    resultItem.documentRef_No5 = queryResult.DocumentRef_No5;
                    resultItem.document_Status = queryResult.Document_Status;

                    var ProcessStatus = new List<ProcessStatusViewModel>();

                    var filterModel = new ProcessStatusViewModel();

                    filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                    //GetConfig
                    var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());

                    string Status = "";
                    Status = queryResult.Document_Status.ToString();
                    var ProcessStatusName = Process.Where(c => c.processStatus_Id == Status).FirstOrDefault();
                    resultItem.processStatus_Name = ProcessStatusName.processStatus_Name;

                    //resultItem.ProcessStatusName = queryResult.ProcessStatus_Name;
                    //var queryResultItem = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem @strwhere = {0}", pstring).FirstOrDefault();
                    //resultItem.RefDocumentIndex = queryResult.Ref_Document_Index;
                    //resultItem.RefDocumentNo = queryResult.Ref_Document_No;
                    resultItem.warehouse_Index = queryResult.Warehouse_Index;
                    resultItem.warehouse_Id = queryResult.Warehouse_Id;
                    resultItem.warehouse_Name = queryResult.Warehouse_Name;
                    resultItem.warehouse_Index_To = queryResult.Warehouse_Index_To;
                    resultItem.warehouse_Id_To = queryResult.Warehouse_Id_To;
                    resultItem.warehouse_Name_To = queryResult.Warehouse_Name_To;
                    resultItem.create_Date = queryResult.Create_Date.toString();
                    resultItem.create_By = queryResult.Create_By;
                    resultItem.update_Date = queryResult.Update_Date.toString();
                    resultItem.update_By = queryResult.Update_By;
                    resultItem.cancel_Date = queryResult.Cancel_Date.toString();
                    resultItem.cancel_By = queryResult.Cancel_By;
                    resultItem.dockDoor_Index = queryResult.DockDoor_Index;
                    resultItem.dockDoor_Id = queryResult.DockDoor_Id;
                    resultItem.dockDoor_Name = queryResult.DockDoor_Name;
                    resultItem.vehicleType_Index = queryResult.VehicleType_Index;
                    resultItem.vehicleType_Id = queryResult.VehicleType_Id;
                    resultItem.vehicleType_Name = queryResult.VehicleType_Name;
                    resultItem.containerType_Index = queryResult.ContainerType_Index;
                    resultItem.containerType_Id = queryResult.ContainerType_Id;
                    resultItem.containerType_Name = queryResult.ContainerType_Name;
                    resultItem.document_Remark = queryResult.Document_Remark;
                    resultItem.userAssign = queryResult.UserAssign;
                    resultItem.vendor_Index = queryResult.Vendor_Index;
                    resultItem.vendor_Id = queryResult.Vendor_Id;
                    resultItem.vendor_Name = queryResult.Vendor_Name;
                    //resultItem.soldTo_Index = queryResult.SoldTo_Index;
                    //resultItem.soldTo_Id = queryResult.SoldTo_Id;
                    //resultItem.soldTo_Name = queryResult.SoldTo_Name;
                    resultItem.invoice_No = queryResult.Invoice_No;
                    return resultItem;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GoodsReceiveItemWithTagViewModel> GetByGoodReceiveId(string id)
        {
            try
            {
                var result = new List<GoodsReceiveItemWithTagViewModel>();

                using (var context = new GRDbContext())
                {
                    Guid GoodsReceiveIndex = new Guid(id);

                    var queryResult = db.View_GoodsReceiveWithTag.Where(c => c.GoodsReceive_Index == GoodsReceiveIndex && c.Document_Status != -1).ToList();

                    var filterModel = new ProcessStatusViewModel();
                    filterModel.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");

                    var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                    foreach (var data in queryResult)
                    {
                        var item = new GoodsReceiveItemWithTagViewModel();
                        var planGR = data.Ref_Document_Index.ToString();
                        item.goodsReceive_Index = data.GoodsReceive_Index;
                        item.goodsReceiveItem_Index = data.GoodsReceiveItem_Index;
                        item.lineNum = data.LineNum;
                        item.product_Index = data.Product_Index;
                        item.product_Id = data.Product_Id;
                        item.product_Name = data.Product_Name;
                        item.product_SecondName = data.Product_SecondName;
                        item.product_ThirdName = data.Product_ThirdName;
                        item.product_Lot = data.Product_Lot;
                        item.itemStatus_Index = data.ItemStatus_Index;
                        item.itemStatus_Id = data.ItemStatus_Id;
                        item.itemStatus_Name = data.ItemStatus_Name;
                        item.qty = string.Format(String.Format("{0:N3}", data.Qty));
                        //item.qty = data.Qty;
                        item.ratio = data.Ratio;
                        item.totalQty = data.TotalQty;
                        item.productConversion_Index = data.ProductConversion_Index;
                        item.productConversion_Id = data.ProductConversion_Id;
                        item.productConversion_Name = data.ProductConversion_Name;
                        item.ref_Document_No = data.Ref_Document_No;
                        item.mFG_Date = data.MFG_Date.toString();
                        item.eXP_Date = data.EXP_Date.toString();
                        item.weight = string.Format(String.Format("{0:N3}", data.Weight));
                        //item.weight = data.Weight;
                        item.unitWeight = data.UnitWeight;
                        item.unitWidth = data.UnitWidth;
                        item.unitLength = data.UnitLength;
                        item.unitHeight = data.UnitHeight;
                        item.unitVolume = data.UnitVolume;
                        item.volume = string.Format(String.Format("{0:N3}", data.Volume));
                        //item.volume = data.Volume;
                        item.unitPrice = data.UnitPrice;
                        item.price = data.Price;
                        item.ref_Document_Index = data.Ref_Document_Index;
                        item.ref_DocumentItem_Index = data.Ref_DocumentItem_Index;
                        item.ref_Document_No = data.Ref_Document_No;
                        item.ref_Document_LineNum = data.Ref_Document_LineNum;
                        item.documentRef_No1 = data.DocumentRef_No1;
                        item.documentRef_No2 = data.DocumentRef_No2;
                        item.documentRef_No3 = data.DocumentRef_No3;
                        item.documentRef_No4 = data.DocumentRef_No4;
                        item.documentRef_No5 = data.DocumentRef_No5;
                        //item.document_Status = data.Document_Status;
                        item.goodsReceive_Remark = data.GoodsReceive_Remark;
                        item.uDF_1 = data.UDF_1;
                        item.uDF_2 = data.UDF_2;
                        item.uDF_3 = data.UDF_3;
                        item.uDF_4 = data.UDF_4;
                        item.uDF_5 = data.UDF_5;
                        item.gRIQty = data.GRIQty;
                        item.tagIQty = data.TagIQty;
                        item.remainingQty = data.RemainingQty.ToString();
                        item.remainingWeight = string.Format(String.Format("{0:N3}", data.RemainingWeight));
                        item.remainingVolume = string.Format(String.Format("{0:N3}", data.RemainingVolume));

                        var Putaway_Status = data.Document_Status.ToString();
                        item.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                        item.erp_Location = data.ERP_Location;
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

        public String genTagNo(GRDbContext context)
        {
            try
            {
                var DocumentType_Index = new SqlParameter("@DocumentType_Index", "CB8FEA3E-0683-44B8-A05F-BDB358ABF8D0");

                var DocDate = new SqlParameter("@DocDate", DateTime.Now.toString().toDate());

                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                resultParameter.Size = 2000; // some meaningfull value
                resultParameter.Direction = ParameterDirection.Output;
                context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
                //var result = resultParameter.Value;
                string TagNumber = resultParameter.Value.ToString();

                var ColumnName1 = new SqlParameter("@ColumnName1", "Tag_No");
                var ColumnName2 = new SqlParameter("@ColumnName2", "''");
                var ColumnName3 = new SqlParameter("@ColumnName3", "''");
                var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                var TableName = new SqlParameter("@TableName", "wm_Tag");
                var Where = new SqlParameter("@Where", "where Tag_No = '" + TagNumber + "'");
                var DataTagNo = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                //
                if (DataTagNo == null)
                {
                    return TagNumber;

                }
                else
                {
                    TagNumber = genTagNo(context);
                }



                return TagNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

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

        #region wm_Tag
        public List<LPNViewModel> wm_Tag(DocumentViewModel model)
        {
            try
            {
                var query = db.WM_Tag.AsQueryable();

                var result = new List<LPNViewModel>();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.Tag_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Tag_Status));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_No != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_No).Contains(c.Tag_No));
                }

                var queryresult = query.ToList();

                foreach (var item in queryresult)
                {
                    var resultItem = new LPNViewModel();

                    resultItem.tag_Index = item.Tag_Index;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.Pallet_Index = item.Pallet_Index;
                    resultItem.Pallet_No = item.Pallet_No;
                    resultItem.tagRef_No1 = item.TagRef_No1;
                    resultItem.tagRef_No2 = item.TagRef_No2;
                    resultItem.tagRef_No3 = item.TagRef_No3;
                    resultItem.tagRef_No4 = item.TagRef_No4;
                    resultItem.tagRef_No5 = item.TagRef_No5;
                    resultItem.tag_Status = item.Tag_Status;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.create_Date = item.Create_Date;
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date;
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date;
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

        #region autoTag
        public List<ItemListViewModel> autoTag(ItemListViewModel data)
        {
            try
            {

                var query = db.WM_Tag.AsQueryable();

                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Tag_No.Contains(data.key));
                }

                var items = new List<ItemListViewModel>();
                var result = query.Select(c => new { c.Tag_Index, c.Tag_No }).Distinct().Take(10).ToList();
                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {
                        index = item.Tag_Index,
                        name = item.Tag_No
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

        #region createPerTagItems
        public String createPerTagItems(GoodsReceiveTagItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            String TagNo = "";

            try
            {

                foreach (var GRitem in data.listPlanGoodsReceiveItemViewModel)
                {
                    var result = new List<ProductModel>();

                    var filterModel = new ProductModel();
                    filterModel.product_Index = GRitem.product_Index;
                    //GetConfig
                    result = utils.SendDataApi<List<ProductModel>>(new AppSettingConfig().GetUrl("getProduct"), filterModel.sJson());

                    if (result.FirstOrDefault().qty_Per_Tag == 0 || result.FirstOrDefault().qty_Per_Tag == null)
                    {
                        return "qty_Per_Tag";
                    }

                    decimal sumQty = 0;
                    decimal qty = 0;
                    decimal qtyPerTag = 0;
                    decimal qtyOfTag = 0;

                    decimal qtyTagGen = 0;
                    decimal countTag = 0;
                    decimal qtyTag = 0;

                    decimal weightOfQty = 0;
                    decimal weight = 0;

                    decimal evenweightOfQty = 0;
                    decimal evenweight = 0;

                    decimal volumeOfQty = 0;
                    decimal volume = 0;

                    decimal evenvolumeOfQty = 0;
                    decimal evenvolume = 0;
                    
                    Boolean Iseven = false;

                    //data.qtyGenTag.qtyPerTag = result.FirstOrDefault().qty_Per_Tag;
                    //data.qtyGenTag.qtyOfTag = 0;

                    //decimal QtyTag = (GRitem.qty / result.FirstOrDefault().qty_Per_Tag).GetValueOrDefault();

                    //decimal QtyTag = (GRitem.qty / result.FirstOrDefault().qty_Per_Tag).GetValueOrDefault();
                    var remainingqty_ratio = GRitem.remainingQty * GRitem.ratio;
                    decimal QtyTag = (remainingqty_ratio / result.FirstOrDefault().qty_Per_Tag).GetValueOrDefault();

                    var QtyofTag = Math.Ceiling(QtyTag);
                    //var QtyofTag = QtyTag;


                    if (QtyTag < 1)
                    {
                        QtyofTag = 1;
                    }



                    if (result.FirstOrDefault().qty_Per_Tag > 0 && QtyTag > 0)
                    {
                        //qty = GRitem.qty.GetValueOrDefault();
                        qty = remainingqty_ratio.GetValueOrDefault();

                        qtyPerTag = result.FirstOrDefault().qty_Per_Tag.GetValueOrDefault();
                        qtyTagGen = QtyofTag;

                        weightOfQty = GRitem.weight.GetValueOrDefault() / QtyofTag;
                        weight = weightOfQty;

                        volumeOfQty = GRitem.volume.GetValueOrDefault() / QtyofTag;
                        volume = volumeOfQty;
                    }
                    else if (result.FirstOrDefault().qty_Per_Tag > 0)
                    {
                        qty = QtyofTag;
                        qtyPerTag = result.FirstOrDefault().qty_Per_Tag.GetValueOrDefault();



                        weightOfQty = GRitem.weight.GetValueOrDefault() / QtyofTag;
                        weight = weightOfQty;

                        volumeOfQty = GRitem.weight.GetValueOrDefault() / QtyofTag;
                        volume = volumeOfQty;

                        countTag = qty / qtyPerTag;
                        qtyTagGen = Math.Ceiling(countTag);
                    }

                    else if (QtyofTag > 0)
                    {
                        qty = QtyofTag;
                        qtyOfTag = QtyofTag;



                        qtyTag = Math.Floor(qty / qtyOfTag);

                        weightOfQty = GRitem.weight.GetValueOrDefault() / QtyofTag;
                        weight = weightOfQty;

                        volumeOfQty = GRitem.volume.GetValueOrDefault() / QtyofTag;
                        volume = volumeOfQty;

                        qtyTagGen = qtyOfTag;
                    }

                    //var query = db.WM_Tag.Where(c => c.Tag_Index == data.tag_Index).FirstOrDefault();

                    for (int i = 1; i <= qtyTagGen; i++)
                    {
                        GoodsReceiveViewModel grModel = new GoodsReceiveViewModel();
                        grModel.goodsReceive_Index = data.goodsReceive_Index;
                        grModel.create_By = data.create_By;

                        string tagIndex = CreateTagHeader(grModel);

                        var query = db.WM_Tag.Where(c => c.Tag_Index == new Guid(tagIndex)).FirstOrDefault();

                        WM_TagItem resultItem = new WM_TagItem();

                        GoodsReceiveItemViewModel item = GRitem;

                        var GRItem = db.IM_GoodsReceiveItem.Find(GRitem.goodsReceiveItem_Index);


                        resultItem.TagItem_Index = Guid.NewGuid();
                        resultItem.Tag_Index = new Guid(tagIndex);
                        resultItem.Tag_No = query.Tag_No;
                        resultItem.GoodsReceive_Index = GRitem.goodsReceive_Index;
                        resultItem.GoodsReceiveItem_Index = GRitem.goodsReceiveItem_Index;
                        resultItem.Process_Index = GRitem.ref_Process_Index;
                        resultItem.Product_Index = GRitem.product_Index;
                        resultItem.Product_Id = GRitem.product_Id;
                        resultItem.Product_Name = GRitem.product_Name;
                        resultItem.Product_SecondName = GRitem.product_SecondName;
                        resultItem.Product_ThirdName = GRitem.product_ThirdName;
                        resultItem.Product_Lot = GRitem.product_Lot;
                        resultItem.ItemStatus_Index = GRitem.itemStatus_Index;
                        resultItem.ItemStatus_Id = GRitem.itemStatus_Id;
                        resultItem.ItemStatus_Name = GRitem.itemStatus_Name;
                        //resultItem.Qty = item.qty;
                        resultItem.Ratio = item.ratio;
                        resultItem.ProductConversion_Index = GRitem.productConversion_Index;
                        resultItem.ProductConversion_Id = GRitem.productConversion_Id;
                        resultItem.ProductConversion_Name = GRitem.productConversion_Name;
                        //resultItem.Weight = item.weight;
                        //resultItem.Volume = item.volume;
                        resultItem.MFG_Date = GRitem.mFG_Date.toDate();
                        resultItem.EXP_Date = GRitem.eXP_Date.toDate();
                        resultItem.TagRef_No1 = "";
                        resultItem.TagRef_No2 = "";
                        resultItem.TagRef_No3 = "";
                        resultItem.TagRef_No4 = "";
                        resultItem.TagRef_No5 = "";
                        resultItem.Tag_Status = 0;
                        resultItem.UDF_1 = GRitem.uDF_1;
                        resultItem.UDF_2 = GRitem.uDF_2;
                        resultItem.UDF_3 = GRitem.uDF_3;
                        resultItem.UDF_4 = GRitem.uDF_4;
                        resultItem.UDF_5 = GRitem.uDF_5;
                        resultItem.Create_By = GRitem.create_By;
                        resultItem.Create_Date = DateTime.Now;

                        resultItem.UnitWeight = GRItem.UnitWeight;
                        resultItem.WeightRatio = GRItem.WeightRatio;

                        resultItem.UnitGrsWeight = GRItem.UnitGrsWeight;
                        resultItem.GrsWeightRatio = GRItem.GrsWeightRatio;

                        resultItem.UnitWidth = GRItem.Width;
                        resultItem.WidthRatio = GRItem.WidthRatio;

                        resultItem.UnitLength = GRItem.Length;
                        resultItem.LengthRatio = GRItem.LengthRatio;

                        resultItem.UnitHeight = GRItem.UnitHeight;
                        resultItem.HeightRatio = GRItem.HeightRatio;

                        resultItem.UnitVolume = GRItem.UnitVolume;

                        resultItem.TotalPrice = GRItem.TotalPrice;

                        resultItem.Currency_Index = GRItem.Currency_Index;
                        resultItem.Currency_Id = GRItem.Currency_Id;
                        resultItem.Currency_Name = GRItem.Currency_Name;

                        resultItem.Ref_Code1 = GRItem.Ref_Code1;
                        resultItem.Ref_Code2 = GRItem.Ref_Code2;
                        resultItem.Ref_Code3 = GRItem.Ref_Code3;
                        resultItem.Ref_Code4 = GRItem.Ref_Code4;
                        resultItem.Ref_Code5 = GRItem.Ref_Code5;
                        resultItem.ERP_Location = GRItem.ERP_Location;

                        if (result.FirstOrDefault().qty_Per_Tag > 0 && QtyofTag > 0)
                        {
                            if ((qtyPerTag + sumQty) >= qty)
                            {
                                resultItem.Qty = (qty - sumQty) / item.ratio;
                                //resultItem.Weight = weightOfQty * (qty - sumQty);
                                //resultItem.Volume = volumeOfQty * (qty - sumQty);
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                if (Iseven) {
                                    resultItem.Weight = evenweightOfQty;
                                    resultItem.Volume = evenvolumeOfQty;
                                }

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;

                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                i = (int)qtyTagGen;
                            }
                            else
                            {
                                resultItem.Qty = qtyPerTag / item.ratio;
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                            }
                        }
                        else if (result.FirstOrDefault().qty_Per_Tag > 0)
                        {
                            if (i == qtyTagGen)
                            {
                                resultItem.Qty = qty - sumQty;
                                //resultItem.Weight = weightOfQty * (qty - sumQty);
                                //resultItem.Volume = volumeOfQty * (qty - sumQty);
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                            }
                            else
                            {
                                resultItem.Qty = qtyPerTag / item.ratio;
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                            }
                        }
                        else if (QtyofTag > 0)
                        {
                            if (i == qtyTagGen)
                            {
                                resultItem.Qty = qty - sumQty;
                                //resultItem.Weight = weightOfQty * (qty - sumQty);
                                //resultItem.Volume = volumeOfQty * (qty - sumQty);
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                            }
                            else
                            {
                                resultItem.Qty = qtyTag;
                                resultItem.Weight = weight;
                                resultItem.Volume = volume;
                                resultItem.TotalQty = Convert.ToDecimal(resultItem.Qty) * item.ratio;

                                resultItem.Width = resultItem.Qty * GRItem.UnitWidth;
                                resultItem.Length = resultItem.Qty * GRItem.UnitLength;
                                resultItem.Height = resultItem.Qty * GRItem.UnitHeight;
                                resultItem.Price = resultItem.Qty * GRItem.UnitPrice;
                                resultItem.NetWeight = resultItem.Qty * GRItem.UnitWeight;
                            }
                        }

                        sumQty += result.FirstOrDefault().qty_Per_Tag > 0 ? qtyPerTag : qtyTag;

                        db.wm_TagItem.Add(resultItem);
                    }
                }


                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("SaveTag", msglog);
                    transaction.Rollback();

                    throw ex;

                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
