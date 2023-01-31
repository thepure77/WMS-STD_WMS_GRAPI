using Business.Library;
using Comone.Utils;
using DataAccess;
using GRBusiness.AutoNumber;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceive;
using GRBusiness.PlanGoodsReceive;
using GRDataAccess.Models;
using MasterDataBusiness.Product;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using static GRBusiness.GoodsReceive.GoodsReceiveDocViewModel;
using static GRBusiness.GoodsReceive.ScanReceiveProductViewModel;
using static GRBusiness.GoodsReceive.ScanReceiveViewModel;
using static GRBusiness.GoodsReceive.SearchScanReceiveViewModel;

namespace GRBusiness.ScanReceiveService
{
    public class ScanReceiveService
    {
        private GRDbContext db;

        public ScanReceiveService()
        {
            db = new GRDbContext();
        }

        public ScanReceiveService(GRDbContext db)
        {
            this.db = db;
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

        public actionResultSearchScanReceiveViewModel scanDN(SearchScanReceiveViewModel data)
        {
            try
            {
                var result = new actionResultSearchScanReceiveViewModel();
                var resultGR = new List<ResultSearchScanReceiveViewModel>();



                var statusPlan = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == data.planGoodsReceive_No).FirstOrDefault();

                if (statusPlan == null)
                {
                    result.msg = "MSG_Alert_PlanGoodsReceive_Not_Found";
                    return result;
                }
                else if (statusPlan.Document_Status == 0)
                {
                    result.msg = "MSG_Alert_PlanGoodsReceive_Pending";
                }
                else if (statusPlan.Document_Status == 3)
                {
                    result.msg = "MSG_Alert_PlanGoodsReceive_Completed";
                    return result;
                }
                else if (statusPlan.Document_Status == -1)
                {
                    result.msg = "MSG_Alert_PlanGoodsReceive_Cancel";
                    return result;
                }
                else if (statusPlan.Document_Status == 4)
                {
                    result.msg = "MSG_Alert_PlanGoodsReceive_CloseDocument";
                    return result;
                }
                else
                {
                    var GRItem = db.View_CheckPlanGRScanGR.Where(c => c.Ref_Document_No == data.planGoodsReceive_No).FirstOrDefault();

                    var resultitem = new ResultSearchScanReceiveViewModel();


                    if (GRItem != null)
                    {
                        var GR = db.IM_GoodsReceive.Find(GRItem.GoodsReceive_Index);

                        if (GR.Document_Status == -1)
                        {
                            result.msg = "MSG_Alert_GoodsReceive_Cancel";
                            return result;
                        }
                        if (GR.Document_Status == 2)
                        {
                            result.msg = "MSG_Alert_GoodsReceive_Confirmed";
                            return result;
                        }
                        if (GR.Document_Status == 3)
                        {
                            result.msg = "MSG_Alert_GoodsReceive_Completed";
                            return result;
                        }
                        else
                        {
                            resultitem.goodsReceive_Index = GR.GoodsReceive_Index.ToString();
                            resultitem.goodsReceive_No = GR.GoodsReceive_No;
                            resultitem.owner_Index = GR.Owner_Index.ToString(); 
                            resultitem.owner_Id = GR.Owner_Id;
                            resultitem.owner_Name = GR.Owner_Name;
                            resultitem.vendor_Index = statusPlan.Vendor_Index.ToString();
                            resultitem.vendor_Id = statusPlan.Vendor_Id;
                            resultitem.vendor_Name = statusPlan.Vendor_Name;
                            resultitem.documentType_Index = GR.DocumentType_Index.ToString();
                            resultitem.documentType_Id = GR.DocumentType_Id;
                            resultitem.documentType_Name = GR.DocumentType_Name;
                            resultitem.goodsReceive_Date = GR.GoodsReceive_Date.toString();

                            resultGR.Add(resultitem);

                            result.item = resultGR;
                        }

                    }
                    else
                    {

                        resultitem.owner_Index = statusPlan.Owner_Index.ToString();
                        resultitem.owner_Id = statusPlan.Owner_Id;
                        resultitem.owner_Name = statusPlan.Owner_Name;
                        resultitem.vendor_Index = statusPlan.Vendor_Index.ToString();
                        resultitem.vendor_Id = statusPlan.Vendor_Id;
                        resultitem.vendor_Name = statusPlan.Vendor_Name;
                        resultitem.goodsReceive_Date = DateTime.Today.toString();
                        resultitem.documentType_Index = statusPlan.DocumentType_Index.ToString();
                        resultitem.documentType_Id = statusPlan.DocumentType_Id;
                        resultitem.documentType_Name = statusPlan.DocumentType_Name;

                        resultGR.Add(resultitem);

                        result.item = resultGR;
                    }
                }



                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public actionResultSearchScanReceiveViewModel scanGR(SearchScanReceiveViewModel data)
        {
            try
            {
                var result = new actionResultSearchScanReceiveViewModel();
                var resultGR = new List<ResultSearchScanReceiveViewModel>();



                var GR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No == data.goodsReceive_No).FirstOrDefault();
                if (GR == null)
                {
                    result.msg = "MSG_Alert_GoodsReceive_Not_Found";
                    return result;
                }
                if (GR.Document_Status == -1)
                {
                    result.msg = "MSG_Alert_GoodsReceive_Cancel";
                    return result;
                }
                if (GR.Document_Status == 2)
                {
                    result.msg = "MSG_Alert_GoodsReceive_Confirmed";
                    return result;
                }
                if (GR.Document_Status == 3)
                {
                    result.msg = "MSG_Alert_GoodsReceive_Completed";
                    return result;
                }
                else
                {
                    var resultitem = new ResultSearchScanReceiveViewModel();

                    resultitem.goodsReceive_Index = GR.GoodsReceive_Index.ToString();
                    resultitem.goodsReceive_No = GR.GoodsReceive_No;
                    resultitem.owner_Index = GR.Owner_Index.ToString();
                    resultitem.owner_Id = GR.Owner_Id;
                    resultitem.owner_Name = GR.Owner_Name;
                    resultitem.vendor_Index = GR.Vendor_Index.ToString();
                    resultitem.vendor_Id = GR.Vendor_Id;
                    resultitem.vendor_Name = GR.Vendor_Name;
                    resultitem.documentType_Index = GR.DocumentType_Index.ToString();
                    resultitem.documentType_Id = GR.DocumentType_Id;
                    resultitem.documentType_Name = GR.DocumentType_Name;
                    resultitem.goodsReceive_Date = GR.GoodsReceive_Date.toString();

                    resultGR.Add(resultitem);

                    result.item = resultGR;
                }



                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public actionResultSearchScanReceiveViewModel scanUPC(SearchScanReceiveViewModel data)
        {
            try
            {
               
                var result = new actionResultSearchScanReceiveViewModel();

                var resultBarcode = new List<BarcodeViewModel>();

                var resultItem = new List<ResultSearchScanReceiveViewModel>();


                var filterModel = new BarcodeViewModel();

                if (!string.IsNullOrEmpty(data.productConversionBarcode.ToString()))
                {
                    filterModel.productConversionBarcode = data.productConversionBarcode;
                }
                //GetConfig

                resultBarcode = utils.SendDataApi<List<BarcodeViewModel>>(new AppSettingConfig().GetUrl("configBarcode"), filterModel.sJson());

                if (resultBarcode.Count > 0)
                {
                    var plan = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == data.planGoodsReceive_No).FirstOrDefault();

                    var planItem = db.IM_PlanGoodsReceiveItem.Where(c => resultBarcode.Select(s => s.product_Index).Contains(c.Product_Index) && c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index).ToList();
                    if (!string.IsNullOrEmpty(data.planGoodsReceiveitem_Index)) {
                        planItem = planItem.Where(c => c.PlanGoodsReceiveItem_Index == Guid.Parse(data.planGoodsReceiveitem_Index)).ToList();
                    }

                    var scanbarcode = resultBarcode.Where(c => planItem.Select(s => s.Product_Id).Contains(c.product_Id) && c.productConversionBarcode == data.productConversionBarcode);
                    if (!string.IsNullOrEmpty(data.productConversion_Index.ToString())  && data.productConversion_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        scanbarcode = scanbarcode.Where(c => c.productConversion_Index == data.productConversion_Index).ToList();
                    }
                    var resultScanBarcode = scanbarcode.ToList();

                    if (planItem.Count == 0)
                    {
                        result.msg = "MSG_Alert_Barcode_Not_Found";
                        return result;
                    }

                    //if (planItem.Count > 1)
                    //{
                    //    result.msg = "MSG_Alert_PlanGoodsReceive_Product";

                    //    foreach (var item in planItem)
                    //    {
                    //        var resultPlan = new ResultSearchScanReceiveViewModel();

                    //        resultPlan.planGoodsReceiveitem_Index = item.PlanGoodsReceiveItem_Index.ToString();
                    //        resultPlan.product_Index = item.Product_Index;
                    //        resultPlan.product_Name = item.Product_Name;
                    //        resultPlan.product_SecondName = item.Product_SecondName;
                    //        resultPlan.product_ThirdName = item.Product_ThirdName;
                    //        resultPlan.qty = item.Qty;  
                    //        resultPlan.productConversion_Name = item.ProductConversion_Name;
                    //        resultItem.Add(resultPlan);
                    //    }

                    //    result.item = resultItem;

                    //}
                    if (resultScanBarcode.Count > 1)
                    {
                        result.msg = "MSG_Alert_PlanGoodsReceive_Product";

                        foreach (var item in resultScanBarcode)
                        {
                            var resultPlan = new ResultSearchScanReceiveViewModel();

                            var planItemProduct = planItem.Where(c => c.Product_Id == item.product_Id).Select(c => new { c.Qty , c.PlanGoodsReceiveItem_Index }).ToList();

                            resultPlan.planGoodsReceiveitem_Index = planItemProduct.FirstOrDefault().PlanGoodsReceiveItem_Index.ToString();  //data.planGoodsReceiveitem_Index;
                            resultPlan.product_Index = item.product_Index;
                            resultPlan.product_Name = item.product_Name;
                            resultPlan.product_SecondName = item.product_SecondName;
                            resultPlan.product_ThirdName = item.product_ThirdName;
                            resultPlan.qty = planItemProduct.FirstOrDefault().Qty;
                            resultPlan.productConversion_Name = item.productConversion_Name;
                            resultPlan.productConversion_Index =  Guid.Parse(item.productConversion_Index.ToString());
                            resultItem.Add(resultPlan);
                        }

                        result.item = resultItem;

                    }
                    else
                    {
                        resultBarcode = resultBarcode.Where(c => c.product_Index == planItem[0].Product_Index && c.productConversion_Index == resultScanBarcode[0].productConversion_Index).ToList();

                        var planGoodsReceiveItem_Ref = db.im_PlanGoodsReceiveItem_Ref.Where(c => c.PlanGoodsReceiveItem_Ref_Index == planItem[0].PlanGoodsReceiveItem_Index).FirstOrDefault();
                        var resultPlan = new ResultSearchScanReceiveViewModel();

                        resultPlan.product_Index = resultBarcode.FirstOrDefault().product_Index;
                        resultPlan.product_Id = resultBarcode.FirstOrDefault().product_Id;
                        resultPlan.product_Name = resultBarcode.FirstOrDefault().product_Name;
                        resultPlan.product_SecondName = resultBarcode.FirstOrDefault().product_SecondName;
                        resultPlan.product_ThirdName = resultBarcode.FirstOrDefault().product_ThirdName;
                        resultPlan.Ref_No2 = resultBarcode.FirstOrDefault().Ref_No2;
                        resultPlan.productConversion_Height = resultBarcode.FirstOrDefault().productConversion_Height;
                        resultPlan.productConversion_Length = resultBarcode.FirstOrDefault().productConversion_Length;
                        resultPlan.productConversion_Width = resultBarcode.FirstOrDefault().productConversion_Width;
                        resultPlan.volume_Index = resultBarcode.FirstOrDefault().volume_Index;
                        resultPlan.volume_Id = resultBarcode.FirstOrDefault().volume_Id;
                        resultPlan.volume_Name = resultBarcode.FirstOrDefault().volume_Name;
                        resultPlan.volume_Ratio = resultBarcode.FirstOrDefault().volume_Ratio;
                        resultPlan.isLot = resultBarcode.FirstOrDefault().isLot;
                        resultPlan.isExpDate = resultBarcode.FirstOrDefault().isExpDate;
                        resultPlan.isMfgDate = resultBarcode.FirstOrDefault().isMfgDate;
                        resultPlan.isSerial = resultBarcode.FirstOrDefault().isSerial;
                        resultPlan.ProductItemLife_D = resultBarcode.FirstOrDefault().ProductItemLife_D;
                        resultPlan.ProductItemLife_M = resultBarcode.FirstOrDefault().ProductItemLife_M;
                        resultPlan.ProductItemLife_Y = resultBarcode.FirstOrDefault().ProductItemLife_Y;
                        resultPlan.tihi = resultBarcode.FirstOrDefault().tihi;
                        resultPlan.qty_Per_Tag = resultBarcode.FirstOrDefault().qty_Per_Tag;
                        //resultPlan.conversion = " 1 " + resultBarcode.FirstOrDefault().productConversion_Name + " = " + Convert.ToInt32(resultBarcode.FirstOrDefault().productConversion_Ratio)  + " EA ";
                        resultPlan.conversion = resultBarcode.FirstOrDefault().Ref_No3;
                        resultPlan.planGoodsReceiveitem_Index = data.planGoodsReceiveitem_Index;
                        resultPlan.productConversion_Name = resultBarcode.FirstOrDefault().productConversion_Name;
                        resultPlan.productConversion_Index = Guid.Parse(resultBarcode.FirstOrDefault().productConversion_Index.ToString());
                        resultPlan.product_Lot = planItem.FirstOrDefault().Product_Lot;
                        if (planGoodsReceiveItem_Ref != null)
                        {
                            resultPlan.ERP_location = planGoodsReceiveItem_Ref.RECEI_SLOC;
                        }
                        
                        resultPlan.mFG_Date = DateTime.Now.toString();


                        resultItem.Add(resultPlan);

                        result.item = resultItem;
                    }

                }
                else
                {
                    result.msg = "MSG_Alert_Barcode_Not_Found";
                    return result;
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public actionResultSearchScanReceiveViewModel scanUPCUnpack(SearchScanReceiveViewModel data)
        {
            try
            {

                var result = new actionResultSearchScanReceiveViewModel();
                var resultItem = new List<ResultSearchScanReceiveViewModel>();
                var resultBarcode = new List<BarcodeViewModel>();
                
                var filterModel = new BarcodeViewModel();

                if (!string.IsNullOrEmpty(data.productConversionBarcode.ToString()))
                {
                    filterModel.productConversionBarcode = data.productConversionBarcode;
                }
                //GetConfig

                resultBarcode = utils.SendDataApi<List<BarcodeViewModel>>(new AppSettingConfig().GetUrl("configBarcode"), filterModel.sJson());

                if (resultBarcode.Count > 0)
                {
                    var resultPlan = new ResultSearchScanReceiveViewModel();

                    resultPlan.product_Index = resultBarcode.FirstOrDefault().product_Index;
                    resultPlan.product_Id = resultBarcode.FirstOrDefault().product_Id;
                    resultPlan.product_Name = resultBarcode.FirstOrDefault().product_Name;
                    resultPlan.product_SecondName = resultBarcode.FirstOrDefault().product_SecondName;
                    resultPlan.product_ThirdName = resultBarcode.FirstOrDefault().product_ThirdName;
                    resultPlan.Ref_No2 = resultBarcode.FirstOrDefault().Ref_No2;
                    resultPlan.productConversion_Height = resultBarcode.FirstOrDefault().productConversion_Height;
                    resultPlan.productConversion_Length = resultBarcode.FirstOrDefault().productConversion_Length;
                    resultPlan.productConversion_Width = resultBarcode.FirstOrDefault().productConversion_Width;
                    resultPlan.productConversion_Index = resultBarcode.FirstOrDefault().productConversion_Index.GetValueOrDefault();
                    resultPlan.volume_Index = resultBarcode.FirstOrDefault().volume_Index;
                    resultPlan.volume_Id = resultBarcode.FirstOrDefault().volume_Id;
                    resultPlan.volume_Name = resultBarcode.FirstOrDefault().volume_Name;
                    resultPlan.volume_Ratio = resultBarcode.FirstOrDefault().volume_Ratio;
                    resultPlan.isLot = resultBarcode.FirstOrDefault().isLot;
                    resultPlan.isExpDate = resultBarcode.FirstOrDefault().isExpDate;
                    resultPlan.isMfgDate = resultBarcode.FirstOrDefault().isMfgDate;
                    resultPlan.isSerial = resultBarcode.FirstOrDefault().isSerial;
                    resultPlan.tihi = resultBarcode.FirstOrDefault().tihi;
                    resultPlan.qty_Per_Tag = resultBarcode.FirstOrDefault().qty_Per_Tag;
                    resultPlan.conversion = resultBarcode.FirstOrDefault().Ref_No3;


                    resultItem.Add(resultPlan);

                    result.item = resultItem;
                }
                else
                {
                    result.msg = "MSG_Alert_Barcode_Not_Found";
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<GoodsReceiveItemViewModel> CheckGoodReceiveItem(GoodsReceiveItemModel model)
        {
            if (String.IsNullOrEmpty(model.goodsReceiveIndex.ToString()))
            {
                throw new NullReferenceException();
            }
            try
            {


                string pstring = " and GoodsReceive_Index = N'" + model.goodsReceiveIndex + "'";
                pstring += " and Product_Index = N'" + model.product_Index + "'";
                //pstring += " and ProductConversion_Index = '" + model.ProductConversionIndex + "'";
                var strwhere = new SqlParameter("@strwhere", pstring);
                var result = new List<GoodsReceiveItemViewModel>();
                using (var context = new GRDbContext())
                {

                    var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ProductConversion_Index)");
                    var ColumnName2 = new SqlParameter("@ColumnName2", "ProductConversion_Id");
                    var ColumnName3 = new SqlParameter("@ColumnName3", "ProductConversion_Name");
                    var ColumnName4 = new SqlParameter("@ColumnName4", "Convert(Nvarchar(50),ProductConversion_Ratio)");
                    var ColumnName5 = new SqlParameter("@ColumnName5", "Product_Id");
                    var TableName = new SqlParameter("@TableName", "View_ProductDetail");
                    var Where = new SqlParameter("@Where", "Where ProductConversionBarcode ='" + model.ProductBarcode + "'");
                    var DataPrd = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();


                    if (DataPrd == null)
                    {
                        return result;
                    }

                    var queryResult = db.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItemPopup @strwhere", strwhere).Where(c => c.Document_Status != -1).ToList();
                    foreach (var data in queryResult)
                    {
                        var item = new GoodsReceiveItemViewModel();

                        item.goodsReceive_Index = data.GoodsReceive_Index;
                        item.goodsReceiveItem_Index = data.GoodsReceiveItem_Index;
                        item.product_Index = data.Product_Index;
                        item.product_Id = data.Product_Id;
                        item.product_Name = data.Product_Name;
                        item.product_SecondName = data.Product_SecondName;
                        item.product_ThirdName = data.Product_ThirdName;
                        item.product_Lot = data.Product_Lot;
                        item.itemStatus_Index = data.ItemStatus_Index;
                        item.itemStatus_Id = data.ItemStatus_Id;
                        item.itemStatus_Name = data.ItemStatus_Name;
                        item.qty = data.Qty;
                        item.ratio = DataPrd.dataincolumn4.sParse<decimal>();
                        item.totalQty = data.TotalQty;
                        item.productConversion_Index = Guid.Parse(DataPrd.dataincolumn1);
                        item.productConversion_Id = DataPrd.dataincolumn2;
                        item.productConversion_Name = DataPrd.dataincolumn3;
                        //item.ProductConversionIndex = DataPrd.ProductConversion_Index;
                        //item.ProductConversionId = DataPrd.ProductConversion_Id;
                        //item.ProductConversionName = DataPrd.ProductConversion_Name;
                        item.ref_Document_No = data.Ref_Document_No;
                        item.mFG_Date = data.MFG_Date.toString();
                        item.eXP_Date = data.EXP_Date.toString();
                        item.weight = data.Weight;
                        item.unitWeight = data.UnitWeight;
                        item.unitWidth = data.UnitWidth;
                        item.unitLength = data.UnitLength;
                        item.unitHeight = null;
                        item.unitVolume = data.UnitVolume;
                        item.volume = data.Volume;
                        item.unitPrice = data.UnitPrice;
                        item.price = data.Price;
                        item.documentRef_No1 = data.DocumentRef_No1;
                        item.documentRef_No2 = data.DocumentRef_No2;
                        item.documentRef_No3 = data.DocumentRef_No3;
                        item.documentRef_No4 = data.DocumentRef_No4;
                        item.documentRef_No5 = data.DocumentRef_No5;
                        item.document_Status = data.Document_Status;
                        item.goodsReceive_Remark = "";
                        item.uDF_1 = data.UDF_1;
                        item.uDF_2 = data.UDF_2;
                        item.uDF_3 = data.UDF_3;
                        item.uDF_4 = data.UDF_4;
                        item.uDF_5 = data.UDF_5;

                        result.Add(item);
                    }
                }
                //if(result.Count() > 0)
                //{
                //    var chkQty = true;
                //    int number = 0;
                //    foreach (var dataResult in result)
                //    {
                //        number++;
                //        try { chkQty = CheckReceiveQty(dataResult.GoodsReceiveIndex.ToString(), dataResult.GoodsReceiveItemIndex.ToString(), model.product_Index, 1, dataResult.ratio, model.ProductConversionIndex.ToString()); } catch { chkQty = true; }
                //        if (chkQty)
                //        {
                //            break;
                //        }
                //        else if (result.Count() == number)
                //        {
                //            chkQty = false;
                //        }
                //    }
                //    if (!chkQty)
                //    {
                //        throw new Exception("");
                //    }
                //}
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public actionResult saveReceive(ResultSearchScanReceiveViewModel data)
        {

            String GoodsReceiveNo = "";

            var actionResult = new actionResult();
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var qureyGR = db.View_CheckPlanGRScanGR.Where(c => c.Ref_Document_No == data.planGoodsReceive_No).FirstOrDefault();


                if (qureyGR == null)
                {
                    IM_GoodsReceive itemHeader = new IM_GoodsReceive();

                    itemHeader.GoodsReceive_Index = Guid.NewGuid();

                    var filterModel = new DocumentTypeViewModel();
                    var result = new List<DocumentTypeViewModel>();

                    filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                    filterModel.documentType_Index = new Guid(data.documentType_Index);
                    result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());
                    DataTable resultDocumentType = CreateDataTable(result);


                    var plan = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == data.planGoodsReceive_No).FirstOrDefault();

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = DateTime.Now;
                    DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);

                    GoodsReceiveNo = DocNo;
                    var goodReceiveDate = data.goodsReceive_Date.toDate();
                    var document_status = 0;
                    var documentPriority_Status = 0;
                    var putaway_Status = 0;

                    itemHeader.GoodsReceive_No = DocNo;
                    itemHeader.Owner_Index = plan.Owner_Index;
                    itemHeader.Owner_Id = plan.Owner_Id;
                    itemHeader.Owner_Name = plan.Owner_Name;
                    itemHeader.DocumentType_Index = new Guid(data.documentType_Index);
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;
                    var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    itemHeader.GoodsReceive_Date = goodReceiveDate + time;
                    itemHeader.GoodsReceive_Due_Date = goodReceiveDate + time;
                    itemHeader.Document_Status = 0;

                    itemHeader.DocumentPriority_Status = documentPriority_Status;
                    itemHeader.Warehouse_Index = plan.Warehouse_Index;
                    itemHeader.Warehouse_Id = plan.Warehouse_Id;
                    itemHeader.Warehouse_Name = plan.Warehouse_Name;
                    itemHeader.Putaway_Status = putaway_Status;
                    itemHeader.DockDoor_Index = plan.Dock_Index;
                    itemHeader.DockDoor_Id = plan.Dock_Id;
                    itemHeader.DockDoor_Name = plan.Dock_Name;
                    itemHeader.VehicleType_Index = plan.VehicleType_Index;
                    itemHeader.VehicleType_Id = plan.VehicleType_Id;
                    itemHeader.VehicleType_Name = plan.VehicleType_Name;
                    itemHeader.License_Name = plan.License_Name;
                    itemHeader.Forwarder_Index = plan.Forwarder_Index;
                    itemHeader.Forwarder_Id = plan.Forwarder_Id;
                    itemHeader.Forwarder_Name = plan.Forwarder_Name;
                    itemHeader.ShipmentType_Index = plan.ShipmentType_Index;
                    itemHeader.ShipmentType_Id = plan.ShipmentType_Id;
                    itemHeader.ShipmentType_Name = plan.ShipmentType_Name;
                    itemHeader.CargoType_Index = plan.CargoType_Index;
                    itemHeader.CargoType_Id = plan.CargoType_Id;
                    itemHeader.CargoType_Name = plan.CargoType_Name;
                    itemHeader.UnloadingType_Index = plan.UnloadingType_Index;
                    itemHeader.UnloadingType_Id = plan.UnloadingType_Id;
                    itemHeader.UnloadingType_Name = plan.UnloadingType_Name;
                    itemHeader.ContainerType_Index = plan.ContainerType_Index;
                    itemHeader.ContainerType_Id = plan.ContainerType_Id;
                    itemHeader.ContainerType_Name = plan.ContainerType_Name;
                    itemHeader.Container_No1 = plan.Container_No1;
                    itemHeader.Container_No2 = plan.Container_No2;
                    itemHeader.Labur = plan.Labur;
                    itemHeader.Create_By = data.create_By;
                    itemHeader.Create_Date = DateTime.Now;
                    itemHeader.Checker_Name = data.create_By;
                    itemHeader.Driver_Name = plan.Driver_Name;
                    itemHeader.Vendor_Index = plan.Vendor_Index;
                    itemHeader.Vendor_Id = plan.Vendor_Id;
                    itemHeader.Vendor_Name = plan.Vendor_Name;
                    db.IM_GoodsReceive.Add(itemHeader);

                    var planItem = new IM_PlanGoodsReceiveItem();
                    if (data.planGoodsReceiveitem_Index == null)
                    {
                        planItem = db.IM_PlanGoodsReceiveItem.Where(c => c.Product_Index == data.product_Index && c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index).FirstOrDefault();
                    }
                    else {
                        planItem = db.IM_PlanGoodsReceiveItem.Where(c => c.Product_Index == data.product_Index && c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index && c.PlanGoodsReceiveItem_Index == Guid.Parse(data.planGoodsReceiveitem_Index)).FirstOrDefault();
                    }
                    
                    

                    //SA ทัช บอกให้เช็ค  24/09/2020
                    if (!string.IsNullOrEmpty(planItem.Product_Lot))
                    {
                        if (data.product_Lot != planItem.Product_Lot)
                        {
                            actionResult.goodsReceive_No = "false";
                            return actionResult;
                        }
                    }

                    IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                    // Gen Index for line item
                    resultItem.GoodsReceiveItem_Index = Guid.NewGuid();
                    resultItem.GoodsReceive_Index = itemHeader.GoodsReceive_Index;

                    resultItem.Product_Index = planItem.Product_Index.GetValueOrDefault();
                    resultItem.Product_Id = planItem.Product_Id;
                    resultItem.Product_Name = planItem.Product_Name;
                    resultItem.Product_SecondName = planItem.Product_SecondName;
                    resultItem.Product_ThirdName = planItem.Product_ThirdName;
                    resultItem.Product_Lot = data.product_Lot;

                    resultItem.ItemStatus_Index = data.itemStatus_Index;
                    resultItem.ItemStatus_Id = data.itemStatus_Id;
                    resultItem.ItemStatus_Name = data.itemStatus_Name;

                    resultItem.Qty = data.qty;
                    resultItem.Ratio = data.productconversion_Ratio;
                    if (data.productconversion_Ratio != 0)
                    {
                        //var totalqty = data.qty * planItem.Ratio;
                        var totalqty = data.qty * data.productconversion_Ratio;
                        data.totalQty = totalqty;
                    }
                    resultItem.TotalQty = data.totalQty;
                    resultItem.ProductConversion_Index = data.productConversion_Index;
                    resultItem.ProductConversion_Id = data.productConversion_Id;
                    resultItem.ProductConversion_Name = data.productConversion_Name;
                    resultItem.MFG_Date = data.mFG_Date.toDate();
                    resultItem.EXP_Date = data.eXP_Date.toDate();

                    resultItem.WeightRatio = planItem.WeightRatio;
                    resultItem.UnitWeight = planItem.UnitWeight;
                    resultItem.Weight = data.qty * (planItem.UnitWeight * planItem.WeightRatio);
                    resultItem.Weight_Index = planItem.Weight_Index;
                    resultItem.Weight_Id = planItem.Weight_Id;
                    resultItem.Weight_Name = planItem.Weight_Name;
                    resultItem.NetWeight = resultItem.Weight * data.qty;

                    resultItem.GrsWeightRatio = planItem.GrsWeightRatio;
                    resultItem.UnitGrsWeight = planItem.UnitGrsWeight;
                    resultItem.GrsWeight = data.qty * (planItem.UnitGrsWeight * planItem.GrsWeightRatio);
                    resultItem.GrsWeight_Index = planItem.GrsWeight_Index;
                    resultItem.GrsWeight_Id = planItem.GrsWeight_Id;
                    resultItem.GrsWeight_Name = planItem.GrsWeight_Name;

                    resultItem.WidthRatio = planItem.WidthRatio;
                    resultItem.UnitWidth = planItem.UnitWidth;
                    resultItem.Width = data.qty * (planItem.UnitWidth * planItem.WidthRatio);
                    resultItem.Width_Index = planItem.Width_Index;
                    resultItem.Width_Id = planItem.Width_Id;
                    resultItem.Width_Name = planItem.Width_Name;

                    resultItem.LengthRatio = planItem.LengthRatio;
                    resultItem.UnitLength = planItem.UnitLength;
                    resultItem.Length = data.qty * (planItem.UnitLength * planItem.LengthRatio);
                    resultItem.Length_Index = planItem.Length_Index;
                    resultItem.Length_Id = planItem.Length_Id;
                    resultItem.Length_Name = planItem.Length_Name;

                    resultItem.HeightRatio = planItem.HeightRatio;
                    resultItem.UnitHeight = planItem.UnitHeight;
                    resultItem.Height = data.qty * (planItem.UnitHeight * planItem.HeightRatio);
                    resultItem.Height_Index = planItem.Height_Index;
                    resultItem.Height_Id = planItem.Height_Id;
                    resultItem.Height_Name = planItem.Height_Name;

                    resultItem.UnitVolume = planItem.UnitVolume;
                    resultItem.Volume = planItem.Volume;


                    resultItem.UnitPrice = planItem.UnitPrice;
                    resultItem.Price = planItem.UnitPrice * planItem.Qty;
                    resultItem.TotalPrice = planItem.UnitPrice * planItem.Qty;

                    resultItem.Currency_Index = planItem.Currency_Index;
                    resultItem.Currency_Id = planItem.Currency_Id;
                    resultItem.Currency_Name = planItem.Currency_Name;

                    resultItem.Ref_Code1 = planItem.Ref_Code1;
                    resultItem.Ref_Code2 = planItem.Ref_Code2;
                    resultItem.Ref_Code3 = planItem.Ref_Code3;
                    resultItem.Ref_Code4 = planItem.Ref_Code4;
                    resultItem.Ref_Code5 = planItem.Ref_Code5;


                    resultItem.DocumentRef_No1 = planItem.DocumentRef_No1;
                    resultItem.DocumentRef_No2 = planItem.DocumentRef_No2;
                    resultItem.DocumentRef_No3 = planItem.DocumentRef_No3;
                    resultItem.DocumentRef_No4 = planItem.DocumentRef_No4;
                    resultItem.DocumentRef_No5 = planItem.DocumentRef_No5;
                    resultItem.Document_Status = 0;
                    resultItem.UDF_1 = planItem.UDF_1;
                    resultItem.UDF_2 = planItem.UDF_2;
                    resultItem.UDF_3 = planItem.UDF_3;
                    resultItem.UDF_4 = planItem.UDF_4;
                    resultItem.UDF_5 = planItem.UDF_5;
                    resultItem.Ref_Document_LineNum = planItem.LineNum;
                    resultItem.Ref_Document_Index = plan.PlanGoodsReceive_Index;
                    resultItem.Ref_DocumentItem_Index = planItem.PlanGoodsReceiveItem_Index;
                    resultItem.Ref_Document_No = plan.PlanGoodsReceive_No;
                    resultItem.ERP_Location = data.erp_location;


                    resultItem.Create_By = itemHeader.Create_By;
                    resultItem.Create_Date = DateTime.Now;

                    var resultPlanGR = new List<PlanGoodsReceiveItemViewModel>();

                    var checkPO = db.View_CheckQtyPlan.FirstOrDefault(c => c.PlanGoodsReceiveItem_Index == planItem.PlanGoodsReceiveItem_Index);
                    if (checkPO != null)
                    {
                        if (resultItem.TotalQty > checkPO.Remain_qty)
                        {
                            actionResult.goodsReceive_No = GoodsReceiveNo;
                            actionResult.Message = false;
                            return actionResult;
                        }
                    }


                    var listPlan = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = planItem.PlanGoodsReceiveItem_Index } };
                    var plans = new DocumentViewModel();
                    plans.listDocumentViewModel = listPlan;

                    resultPlanGR = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGRItem"), plans.sJson());

                    var checkGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_DocumentItem_Index == resultItem.Ref_DocumentItem_Index && c.Document_Status != -1)
                                   .GroupBy(c => new { c.Ref_DocumentItem_Index })
                                   .Select(c => new { c.Key.Ref_DocumentItem_Index, SumQty = c.Sum(s => s.TotalQty) }).ToList();

                    if (checkGR.Count > 0)
                    {
                        var QtyGr = checkGR.FirstOrDefault().SumQty + (resultItem.Qty * resultItem.Ratio);

                        if (resultPlanGR.FirstOrDefault().totalQty < QtyGr)
                        {
                            actionResult.goodsReceive_No = GoodsReceiveNo;
                            actionResult.Message = false;
                            actionResult.CheckqtyPO = true;
                            return actionResult;

                        }
                        else
                        {
                            db.IM_GoodsReceiveItem.Add(resultItem);
                        }
                    }
                    else
                    {
                        db.IM_GoodsReceiveItem.Add(resultItem);
                    }

                    if (resultPlanGR.FirstOrDefault().totalQty < (resultItem.Qty * resultItem.Ratio))
                    {
                        actionResult.goodsReceive_No = GoodsReceiveNo;
                        actionResult.Message = false;
                        return actionResult;
                    }

                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {

                        db.SaveChanges();
                        transactionx.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " exy Rollback " + exy.Message.ToString();
                        olog.logging("SaveGR", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }

                }

                else
                {
                    //var findGR = db.IM_GoodsReceive.Find(qureyGR.GoodsReceive_Index);
                    var findGR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == qureyGR.GoodsReceive_Index && (c.Document_Status == 0 || c.Document_Status == 1)).FirstOrDefault();

                    var plan = db.IM_PlanGoodsReceive.Where(c => c.PlanGoodsReceive_No == data.planGoodsReceive_No).FirstOrDefault();
                    var planItem = new IM_PlanGoodsReceiveItem();

                    if (data.planGoodsReceiveitem_Index == null)
                    {
                        planItem = db.IM_PlanGoodsReceiveItem.Where(c => c.Product_Index == data.product_Index && c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index).FirstOrDefault();
                    }
                    else {
                        planItem = db.IM_PlanGoodsReceiveItem.Where(c => c.Product_Index == data.product_Index && c.PlanGoodsReceive_Index == plan.PlanGoodsReceive_Index && c.PlanGoodsReceiveItem_Index == Guid.Parse(data.planGoodsReceiveitem_Index)).FirstOrDefault();
                    }
                    
                    //SA ทัช บอกให้เช็ค  24/09/2020
                    if (!string.IsNullOrEmpty(planItem.Product_Lot))
                    {
                        if (data.product_Lot != planItem.Product_Lot)
                        {
                            actionResult.goodsReceive_No = "false";
                            return actionResult;
                        }
                    }


                    IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                    // Gen Index for line item
                    resultItem.GoodsReceiveItem_Index = Guid.NewGuid();
                    resultItem.GoodsReceive_Index = qureyGR.GoodsReceive_Index;
                    GoodsReceiveNo = findGR.GoodsReceive_No;
                    resultItem.Product_Index = planItem.Product_Index.GetValueOrDefault();
                    resultItem.Product_Id = planItem.Product_Id;
                    resultItem.Product_Name = planItem.Product_Name;
                    resultItem.Product_SecondName = planItem.Product_SecondName;
                    resultItem.Product_ThirdName = planItem.Product_ThirdName;
                    resultItem.Product_Lot = data.product_Lot;

                    resultItem.ItemStatus_Index = data.itemStatus_Index;
                    resultItem.ItemStatus_Id = data.itemStatus_Id;
                    resultItem.ItemStatus_Name = data.itemStatus_Name;

                    resultItem.Qty = data.qty;
                    resultItem.Ratio = data.productconversion_Ratio;
                    if (data.productconversion_Ratio != 0)
                    {
                        var totalqty = data.qty * data.productconversion_Ratio;
                        data.totalQty = totalqty;
                    }
                    resultItem.TotalQty = data.totalQty;
                    resultItem.ProductConversion_Index = data.productConversion_Index;
                    resultItem.ProductConversion_Id = data.productConversion_Id;
                    resultItem.ProductConversion_Name = data.productConversion_Name;
                    resultItem.MFG_Date = data.mFG_Date.toDate();
                    resultItem.EXP_Date = data.eXP_Date.toDate();

                    resultItem.WeightRatio = planItem.WeightRatio;
                    resultItem.UnitWeight = planItem.UnitWeight;
                    resultItem.Weight = data.qty * (planItem.UnitWeight * planItem.WeightRatio);
                    resultItem.Weight_Index = planItem.Weight_Index;
                    resultItem.Weight_Id = planItem.Weight_Id;
                    resultItem.Weight_Name = planItem.Weight_Name;
                    resultItem.NetWeight = resultItem.Weight * data.qty;

                    resultItem.GrsWeightRatio = planItem.GrsWeightRatio;
                    resultItem.UnitGrsWeight = planItem.UnitGrsWeight;
                    resultItem.GrsWeight = data.qty * (planItem.UnitGrsWeight * planItem.GrsWeightRatio);
                    resultItem.GrsWeight_Index = planItem.GrsWeight_Index;
                    resultItem.GrsWeight_Id = planItem.GrsWeight_Id;
                    resultItem.GrsWeight_Name = planItem.GrsWeight_Name;

                    resultItem.WidthRatio = planItem.WidthRatio;
                    resultItem.UnitWidth = planItem.UnitWidth;
                    resultItem.Width = data.qty * (planItem.UnitWidth * planItem.WidthRatio);
                    resultItem.Width_Index = planItem.Width_Index;
                    resultItem.Width_Id = planItem.Width_Id;
                    resultItem.Width_Name = planItem.Width_Name;

                    resultItem.LengthRatio = planItem.LengthRatio;
                    resultItem.UnitLength = planItem.UnitGrsWeight;
                    resultItem.Length = data.qty * (planItem.UnitGrsWeight * planItem.LengthRatio);
                    resultItem.Length_Index = planItem.Length_Index;
                    resultItem.Length_Id = planItem.Length_Id;
                    resultItem.Length_Name = planItem.Length_Name;

                    resultItem.HeightRatio = planItem.HeightRatio;
                    resultItem.UnitHeight = planItem.UnitHeight;
                    resultItem.Height = data.qty * (planItem.UnitHeight * planItem.HeightRatio);
                    resultItem.Height_Index = planItem.Height_Index;
                    resultItem.Height_Id = planItem.Height_Id;
                    resultItem.Height_Name = planItem.Height_Name;

                    resultItem.UnitVolume = planItem.UnitVolume;
                    resultItem.Volume = planItem.Volume;


                    resultItem.UnitPrice = planItem.UnitPrice;
                    resultItem.Price = planItem.UnitPrice * planItem.Qty;
                    resultItem.TotalPrice = planItem.UnitPrice * planItem.Qty;

                    resultItem.Currency_Index = planItem.Currency_Index;
                    resultItem.Currency_Id = planItem.Currency_Id;
                    resultItem.Currency_Name = planItem.Currency_Name;

                    resultItem.Ref_Code1 = planItem.Ref_Code1;
                    resultItem.Ref_Code2 = planItem.Ref_Code2;
                    resultItem.Ref_Code3 = planItem.Ref_Code3;
                    resultItem.Ref_Code4 = planItem.Ref_Code4;
                    resultItem.Ref_Code5 = planItem.Ref_Code5;


                    resultItem.DocumentRef_No1 = planItem.DocumentRef_No1;
                    resultItem.DocumentRef_No2 = planItem.DocumentRef_No2;
                    resultItem.DocumentRef_No3 = planItem.DocumentRef_No3;
                    resultItem.DocumentRef_No4 = planItem.DocumentRef_No4;
                    resultItem.DocumentRef_No5 = planItem.DocumentRef_No5;
                    resultItem.Document_Status = 0;
                    resultItem.UDF_1 = planItem.UDF_1;
                    resultItem.UDF_2 = planItem.UDF_2;
                    resultItem.UDF_3 = planItem.UDF_3;
                    resultItem.UDF_4 = planItem.UDF_4;
                    resultItem.UDF_5 = planItem.UDF_5;
                    resultItem.Ref_Document_LineNum = planItem.LineNum;
                    resultItem.Ref_Document_Index = plan.PlanGoodsReceive_Index;
                    resultItem.Ref_DocumentItem_Index = planItem.PlanGoodsReceiveItem_Index;
                    resultItem.Ref_Document_No = plan.PlanGoodsReceive_No;
                    resultItem.ERP_Location = data.erp_location;


                    resultItem.Create_By = data.create_By;
                    resultItem.Create_Date = DateTime.Now;

                    var resultPlanGR = new List<PlanGoodsReceiveItemViewModel>();

                    var checkPO = db.View_CheckQtyPlan.FirstOrDefault(c => c.PlanGoodsReceiveItem_Index == planItem.PlanGoodsReceiveItem_Index);
                    if (checkPO != null)
                    {
                        if (resultItem.TotalQty > checkPO.Remain_qty)
                        {
                            actionResult.goodsReceive_No = GoodsReceiveNo;
                            actionResult.Message = false;
                            actionResult.CheckqtyPO = true;
                            return actionResult;
                        }
                    }

                    var listPlan = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = planItem.PlanGoodsReceiveItem_Index } };
                    var plans = new DocumentViewModel();
                    plans.listDocumentViewModel = listPlan;

                    resultPlanGR = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGRItem"), plans.sJson());

                    var checkGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_DocumentItem_Index == resultItem.Ref_DocumentItem_Index && c.Document_Status != -1)
                                   .GroupBy(c => new { c.Ref_DocumentItem_Index })
                                   .Select(c => new { c.Key.Ref_DocumentItem_Index, SumQty = c.Sum(s => s.TotalQty) }).ToList();

                    if (checkGR.Count > 0)
                    {
                        var QtyGr = checkGR.FirstOrDefault().SumQty + (resultItem.Qty * resultItem.Ratio);

                        if (resultPlanGR.FirstOrDefault().totalQty < QtyGr)
                        {
                            actionResult.goodsReceive_No = GoodsReceiveNo;
                            actionResult.Message = false;
                            return actionResult;

                        }
                        else
                        {
                            db.IM_GoodsReceiveItem.Add(resultItem);

                        }
                    }
                    else
                    {
                        if (resultPlanGR.FirstOrDefault().totalQty < (resultItem.Qty * resultItem.Ratio))
                        {
                            actionResult.goodsReceive_No = GoodsReceiveNo;
                            actionResult.Message = false;
                            return actionResult;

                        }
                        else
                        {
                            db.IM_GoodsReceiveItem.Add(resultItem);
                        }
                    }


                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {

                        db.SaveChanges();
                        transactionx.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " exy Rollback " + exy.Message.ToString();
                        olog.logging("SaveGR", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }
                }

                actionResult.goodsReceive_No = GoodsReceiveNo;
                actionResult.Message = true;
                return actionResult;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("saveReceive", msglog);
                throw ex;
            }
        }

        public List<GoodsReceiveItemCheckViewModel> filterGRItem(SearchScanReceiveViewModel data)
        {
            try
            {

                var result = new List<GoodsReceiveItemCheckViewModel>();

                var query = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_No == data.planGoodsReceive_No && c.Document_Status != -1).OrderByDescending(c => c.Create_Date).ToList();



                foreach (var item in query)
                {
                    var resultItem = new GoodsReceiveItemCheckViewModel();
                    resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.qty = item.Qty.ToString();


                    var filterModelConversion = new ProductConversionViewModelDoc();
                    var resultConversion = new List<ProductConversionViewModelDoc>();

                    filterModelConversion.productConversion_Index = item.ProductConversion_Index;
                    resultConversion = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModelConversion.sJson());

                    resultItem.TIHI = resultConversion.FirstOrDefault().ref_No1 + " * " + resultConversion.FirstOrDefault().ref_No2;

                    var resultProduct = new List<ProductModel>();

                    var filterModelProduct = new ProductModel();
                    filterModelProduct.product_Index = item.Product_Index;
                    //GetConfig
                    resultProduct = utils.SendDataApi<List<ProductModel>>(new AppSettingConfig().GetUrl("getProduct"), filterModelProduct.sJson());
                    resultItem.QtyPer = resultProduct.FirstOrDefault().qty_Per_Tag;
                    resultItem.mfg_Date = (item.MFG_Date != null) ? item.MFG_Date.GetValueOrDefault().ToString("dd/MM/yyyy") : "";
                    resultItem.exp_Date = (item.EXP_Date != null) ? item.EXP_Date.GetValueOrDefault().ToString("dd/MM/yyyy") : ""; 
                    resultItem.product_Lot = item.Product_Lot;
                    result.Add(resultItem);

                }
                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #region deleteItem
        public actionResult deleteItem(GoodsReceiveItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            bool isDelete = false;
            var olog = new logtxt();

            var actionResult = new actionResult();

            try
            {
                var deleteGoodsReceiveItem = db.IM_GoodsReceiveItem.Find(data.goodsReceiveItem_Index);
                var cancelStatusGoodsReceive = db.IM_GoodsReceive.Find(data.goodsReceive_Index);


                if (deleteGoodsReceiveItem != null && cancelStatusGoodsReceive.Document_Status == 0)
                {
                    deleteGoodsReceiveItem.Document_Status = -1;
                    deleteGoodsReceiveItem.Cancel_By = data.cancel_By;
                    deleteGoodsReceiveItem.Cancel_Date = DateTime.Now;

                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transactionx.Commit();
                        isDelete = true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("deleteItem", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }
                    //isDelete = true;
                }

                if (isDelete)
                {
                    var findGoodsReceiveItemDelete = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status != -1).ToList();

                    if (findGoodsReceiveItemDelete.Count == 0)
                    {
                        cancelStatusGoodsReceive.Document_Status = -1;
                        cancelStatusGoodsReceive.Cancel_By = data.cancel_By;
                        cancelStatusGoodsReceive.Cancel_Date = DateTime.Now;
                    }

                    var transactionc = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transactionc.Commit();
                        //isDelete = true;
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("deleteItem", msglog);
                        transactionc.Rollback();

                        throw exy;

                    }
                }
                

                

                actionResult.Message = true;
                return actionResult;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        //public actionResultSearchScanReceiveViewModel scanReceivePlangGR(SearchScanReceiveViewModel model)
        //{
        //    try
        //    {
        //        var result = new actionResultSearchScanReceiveViewModel();

        //        var items = new List<ResultSearchScanReceiveViewModel>();

        //        var resultReturn = new List<PlanGoodsReceiveViewModel>();

        //        var filterModel = new PlanGoodsReceiveViewModel();
        //        if (!string.IsNullOrEmpty(model.planGoodsReceive_No))
        //        {
        //            filterModel.planGoodsReceive_No = model.planGoodsReceive_No;
        //        }

        //        //GetConfig
        //        resultReturn = utils.SendDataApi<List<PlanGoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("ScanPlanGR"), filterModel.sJson());

        //        if (resultReturn.Count > 0)
        //        {
        //            foreach (var r in resultReturn)
        //            {
        //                var GRI = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == r.planGoodsReceive_Index).OrderByDescending(o => o.Create_Date).ToList();

        //                var filterModelitem = new PlanGoodsReceiveItemViewModel { planGoodsReceive_Index = r.planGoodsReceive_Index };
        //                var resultReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("getScanPlanGRI"), filterModelitem.sJson());

        //                bool chkRemainQty = false;
        //                foreach (var ri in resultReceiveItem)
        //                {
        //                    var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == r.planGoodsReceive_Index && c.Ref_DocumentItem_Index == ri.planGoodsReceiveItem_Index).ToList().Sum(s => s.TotalQty);
        //                    if (TotalGR != ri.totalQty)
        //                    {
        //                        chkRemainQty = true;
        //                    }
        //                }

        //                if (GRI.Count == 0)
        //                {
        //                    switch (r.document_Status)
        //                    {
        //                        case 0:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้ยังไม่ทำการยืนยันเอกสารก่อนสร้างใบรับ";
        //                            break;
        //                        case 1:
        //                            var item = new ResultSearchScanReceiveViewModel();
        //                            item.planGoodsReceive_Index = resultReturn.FirstOrDefault().planGoodsReceive_Index.ToString();
        //                            item.planGoodsReceive_No = resultReturn.FirstOrDefault().planGoodsReceive_No;
        //                            item.goodsReceive_Index = "";
        //                            item.goodsReceive_No = "";
        //                            item.owner_Index = resultReturn.FirstOrDefault().owner_Index.ToString();
        //                            item.owner_Id = resultReturn.FirstOrDefault().owner_Id;
        //                            item.owner_Name = resultReturn.FirstOrDefault().owner_Name;
        //                            item.documentType_Index = "";
        //                            item.documentType_Id = "";
        //                            item.documentType_Name = "";
        //                            item.whOwner_Index = "";
        //                            item.whOwner_Id = "";
        //                            item.whOwner_Name = "";
        //                            item.goodsReceive_Date = DateTime.Now.toString();
        //                            item.planGoodsReceive_Date = resultReturn.FirstOrDefault().planGoodsReceive_Date;

        //                            items.Add(item);
        //                            result.isUse = true;
        //                            result.item = items;
        //                            break;
        //                        case 3:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้รับสินค้าครบแล้ว";
        //                            break;
        //                        case 4:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้ปิดเอกสารเรียบร้อยแล้ว";
        //                            break;
        //                        case -1:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้สถานะ ยกเลิกไม่สามารถทำรายการได้";
        //                            break;
        //                        default:
        //                            result.isUse = false;
        //                            result.msg = "Plan GR Not Found";
        //                            break;
        //                    }
        //                }
        //                else if (GRI.Count > 0 && chkRemainQty)
        //                {
        //                    var item = new ResultSearchScanReceiveViewModel();
        //                    item.planGoodsReceive_Index = resultReturn.FirstOrDefault().planGoodsReceive_Index.ToString();
        //                    item.planGoodsReceive_No = resultReturn.FirstOrDefault().planGoodsReceive_No;
        //                    item.goodsReceive_Index = "";
        //                    item.goodsReceive_No = "";
        //                    item.owner_Index = resultReturn.FirstOrDefault().owner_Index.ToString();
        //                    item.owner_Id = resultReturn.FirstOrDefault().owner_Id;
        //                    item.owner_Name = resultReturn.FirstOrDefault().owner_Name;
        //                    item.documentType_Index = "";
        //                    item.documentType_Id = "";
        //                    item.documentType_Name = "";
        //                    item.whOwner_Index = "";
        //                    item.whOwner_Id = "";
        //                    item.whOwner_Name = "";
        //                    item.goodsReceive_Date = DateTime.Now.toString();
        //                    item.planGoodsReceive_Date = resultReturn.FirstOrDefault().planGoodsReceive_Date;

        //                    items.Add(item);
        //                    result.isUse = true;
        //                    result.item = items;
        //                }
        //                else
        //                {
        //                    var GR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == GRI.FirstOrDefault().GoodsReceive_Index).OrderByDescending(o => o.Create_Date).FirstOrDefault();
        //                    switch (GR.Document_Status)
        //                    {
        //                        case 0:
        //                            result.msg = "Data exists GR No : " + resultReturn.FirstOrDefault()?.planGoodsReceive_No + " Do you Want to use this GR ?";

        //                            //var Receive = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == GR.GoodsReceive_Index);

        //                            var item = new ResultSearchScanReceiveViewModel();
        //                            item.planGoodsReceive_Index = resultReturn.FirstOrDefault().planGoodsReceive_Index.ToString();
        //                            item.planGoodsReceive_No = resultReturn.FirstOrDefault().planGoodsReceive_No;
        //                            item.goodsReceive_Index = GR.GoodsReceive_Index.ToString();
        //                            item.goodsReceive_No = GR.GoodsReceive_No;
        //                            item.owner_Index = resultReturn.FirstOrDefault().owner_Index.ToString();
        //                            item.owner_Id = resultReturn.FirstOrDefault().owner_Id;
        //                            item.owner_Name = resultReturn.FirstOrDefault().owner_Name;
        //                            item.documentType_Index = GR.DocumentType_Index.ToString();
        //                            item.documentType_Id = GR.DocumentType_Id;
        //                            item.documentType_Name = GR.DocumentType_Name;
        //                            item.whOwner_Index = GR.WHOwner_Index.ToString();
        //                            item.whOwner_Id = GR.WHOwner_Id;
        //                            item.whOwner_Name = GR.WHOwner_Name;
        //                            item.goodsReceive_Date = GR.GoodsReceive_Date.toString();
        //                            item.planGoodsReceive_Date = resultReturn.FirstOrDefault().planGoodsReceive_Date;

        //                            items.Add(item);
        //                            result.isUse = true;
        //                            result.item = items;
        //                            break;
        //                        case 1:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้ทำการยืนยันเอกสารรอจัดเก็บแล้ว";
        //                            break;
        //                        case 3:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้ทำการจัดเก็บเรียบร้อยแล้ว";
        //                            break;
        //                        case -1:
        //                            result.isUse = false;
        //                            result.msg = "เอกสารนี้สถานะยกเลิก ไม่สามารถทำรายการได้";
        //                            break;
        //                        default:
        //                            result.isUse = false;
        //                            result.msg = "GR Not Found";
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            result.isUse = false;
        //            result.msg = "Plan GR Not Found";
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public actionResultSearchScanReceiveViewModel scanReceiveGR(SearchScanReceiveViewModel model)
        //{
        //    try
        //    {
        //        var result = new actionResultSearchScanReceiveViewModel();

        //        var items = new List<ResultSearchScanReceiveViewModel>();

        //        var data = db.View_GrProcessStatus.Where(c => c.GoodsReceive_No == model.goodsReceive_No).OrderByDescending(c => c.Create_Date).FirstOrDefault();
        //        if (data != null)
        //        {
        //            switch (data.Document_Status)
        //            {
        //                case 0:
        //                    var item = new ResultSearchScanReceiveViewModel();
        //                    item.planGoodsReceive_Index = data.Ref_Document_Index.ToString();
        //                    item.planGoodsReceive_No = data.Ref_Document_No;
        //                    item.goodsReceive_Index = data.GoodsReceive_Index.ToString();
        //                    item.goodsReceive_No = data.GoodsReceive_No;
        //                    item.owner_Index = data.Owner_Index.ToString();
        //                    item.owner_Id = data.Owner_Id;
        //                    item.owner_Name = data.Owner_Name;
        //                    item.documentType_Index = data.DocumentType_Index.ToString();
        //                    item.documentType_Id = data.DocumentType_Id;
        //                    item.documentType_Name = data.DocumentType_Name;
        //                    item.whOwner_Index = data.whOwner_Index.ToString();
        //                    item.whOwner_Id = data.whOwner_Id;
        //                    item.whOwner_Name = data.whOwner_Name;
        //                    item.goodsReceive_Date = data.GoodsReceive_Date.toString();

        //                    items.Add(item);
        //                    break;
        //                case 1:
        //                    result.msg = "เอกสารนี้ทำการยืนยันเอกสารรอจัดเก็บแล้ว";
        //                    break;
        //                case 3:
        //                    result.msg = "เอกสารนี้ทำการจัดเก็บเรียบร้อยแล้ว";
        //                    break;
        //                case -1:
        //                    result.msg = "เอกสารนี้สถานะยกเลิก ไม่สามารถทำรายการได้";
        //                    break;
        //                default:
        //                    result.msg = "GR Not Found";
        //                    break;
        //            }

        //        }
        //        else
        //        {
        //            result.msg = "GR Not Found";
        //        }

        //        result.item = items;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public actionResultScanReceiveViewModel SaveTagscanReceive(ScanReceiveViewModel model)
        //{
        //    try
        //    {
        //        var result = new actionResultScanReceiveViewModel();

        //        #region CheckReceiveQty
        //        var PlanGRI = new List<PlanGoodsReceiveItemViewModel>();
        //        var filterModel = new IM_PlanGoodsReceiveItem();

        //        filterModel.PlanGoodsReceive_Index = new Guid(model.planGoodsReceive_Index);
        //        filterModel.PlanGoodsReceiveItem_Index = new Guid(model.planGoodsReceiveItem_Index);

        //        //GetConfig
        //        PlanGRI = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("GetPlanGoodsReceiveIem"), filterModel.sJson());

        //        var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == filterModel.PlanGoodsReceive_Index && c.Ref_DocumentItem_Index == filterModel.PlanGoodsReceiveItem_Index).Select(a => a.TotalQty).ToList();
        //        var TotalPlanGRI = PlanGRI.Select(c => c.totalQty).FirstOrDefault();


        //        var Total = (TotalPlanGRI - (TotalGR.Sum() + (model.qty * model.productConversion_Ratio)));
        //        if (Total < 0)
        //        {
        //            result.isUse = false;
        //            result.msg = "ไม่สามารถกรอก QTY เกิน ได้";
        //            return result;
        //        }
        //        #endregion

        //        #region savetag

        //        String State = "Start";
        //        String msglog = "";
        //        var olog = new logtxt();
        //        var goodsReceive_Index = Guid.NewGuid();
        //        var tagItem_Index = Guid.NewGuid();
        //        var document_status = 0;
        //        var documentPriority_Status = 0;
        //        var putaway_Status = 0;
        //        var DocumentDate = model.planGoodsReceive_Date.toDate();
        //        int refDocLineNum = 0;

        //        var GR = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index.ToString() == model.goodsReceive_Index).FirstOrDefault();
        //        if (GR != null)
        //        {
        //            if (GR.Document_Status == 0)
        //            {

        //                var Receive = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == GR.GoodsReceive_Index).OrderByDescending(c => c.Create_Date).FirstOrDefault();
        //                if (Receive.WHOwner_Index.ToString() != model.whOwner_index)
        //                {
        //                    Receive.WHOwner_Index = Guid.Parse(model.whOwner_index);
        //                    Receive.WHOwner_Id = model.whOwner_ID;
        //                    Receive.WHOwner_Name = model.whOwner_Name;
        //                    Receive.Update_Date = DateTime.Now;
        //                    Receive.Update_By = model.create_By + "-Scan Receive";
        //                }
        //                //db.IM_GoodsReceive.Add(GoodsReceiveOld);

        //                //var itemDetail = new List<GoodsReceiveItemViewModel>();
        //                int addNumber = 0;
        //                foreach (var item in model.listPlanGoodsReceiveItemViewModel)
        //                {
        //                    addNumber++;
        //                    IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

        //                    resultItem.Ref_Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

        //                    // Gen Index for line item
        //                    if (string.IsNullOrEmpty(item.goodsReceiveItem_Index.ToString()))
        //                    {
        //                        item.goodsReceiveItem_Index = Guid.NewGuid();
        //                    }
        //                    resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index.sParse<Guid>();

        //                    // Index From Header
        //                    resultItem.GoodsReceive_Index = GR.GoodsReceive_Index;
        //                    if (item.lineNum == null)
        //                    {
        //                        resultItem.LineNum = addNumber.ToString();
        //                    }
        //                    else
        //                    {
        //                        resultItem.LineNum = item.lineNum;
        //                    }
        //                    resultItem.Product_Index = item.product_Index.sParse<Guid>();
        //                    resultItem.Product_Id = item.product_Id;
        //                    resultItem.Product_Name = item.product_Name;
        //                    resultItem.Product_SecondName = item.product_SecondName;
        //                    resultItem.Product_ThirdName = item.product_ThirdName;
        //                    if (item.product_Lot != "")
        //                    {
        //                        resultItem.Product_Lot = item.product_Lot;
        //                    }
        //                    else
        //                    {
        //                        resultItem.Product_Lot = "";
        //                    }

        //                    if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
        //                    {
        //                        resultItem.ItemStatus_Index = item.itemStatus_Index.sParse<Guid>();
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                    }

        //                    if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
        //                    {
        //                        resultItem.ItemStatus_Id = item.itemStatus_Id;
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatus_Id = model.itemStatus_Id;
        //                    }

        //                    if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
        //                    {
        //                        resultItem.ItemStatus_Name = item.itemStatus_Name;
        //                    }
        //                    else
        //                    {
        //                        resultItem.ItemStatus_Name = model.itemStatus_Name;
        //                    }
        //                    //if (!string.IsNullOrEmpty(model.itemStatus_Index))
        //                    //{
        //                    //    resultItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                    //}
        //                    //if (!string.IsNullOrEmpty(model.itemStatus_Id))
        //                    //{
        //                    //    resultItem.ItemStatus_Id = model.itemStatus_Id;
        //                    //}
        //                    //if (!string.IsNullOrEmpty(model.itemStatus_Name))
        //                    //{
        //                    //    resultItem.ItemStatus_Name = model.itemStatus_Name;
        //                    //}


        //                    resultItem.QtyPlan = item.qtyPlan;
        //                    resultItem.Qty = item.qty;
        //                    resultItem.Ratio = item.ratio;
        //                    resultItem.TotalQty = item.qty * item.ratio;
        //                    resultItem.UDF_1 = item.uDF_1;
        //                    resultItem.ProductConversion_Index = item.productConversion_Index.sParse<Guid>();
        //                    resultItem.ProductConversion_Id = item.productConversion_Id;
        //                    resultItem.ProductConversion_Name = item.productConversion_Name;
        //                    resultItem.MFG_Date = model.MFGDate.toDate();
        //                    resultItem.EXP_Date = model.EXPDate.toDate();
        //                    if (item.unitWeight != null)
        //                    {
        //                        resultItem.UnitWeight = item.unitWeight;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitWeight = 0;
        //                    }

        //                    if (item.weight != null)
        //                    {
        //                        resultItem.Weight = item.weight;
        //                    }
        //                    else
        //                    {
        //                        resultItem.Weight = 0;
        //                    }

        //                    if (item.unitWidth != null)
        //                    {
        //                        resultItem.UnitWidth = item.unitWidth;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitWidth = 0;
        //                    }

        //                    if (item.unitLength != null)
        //                    {
        //                        resultItem.UnitLength = item.unitLength;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitLength = 0;
        //                    }

        //                    if (item.unitHeight != null)
        //                    {
        //                        resultItem.UnitHeight = item.unitHeight;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitHeight = 0;
        //                    }

        //                    if (item.unitVolume != null)
        //                    {
        //                        resultItem.UnitVolume = item.unitVolume;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitVolume = 0;
        //                    }

        //                    if (item.volume != null)
        //                    {
        //                        resultItem.Volume = item.volume;
        //                    }
        //                    else
        //                    {
        //                        resultItem.Volume = 0;
        //                    }

        //                    if (item.unitPrice != null)
        //                    {
        //                        resultItem.UnitPrice = item.unitPrice;
        //                    }
        //                    else
        //                    {
        //                        resultItem.UnitPrice = 0;
        //                    }

        //                    if (item.price != null)
        //                    {
        //                        resultItem.Price = item.price;
        //                    }
        //                    else
        //                    {
        //                        resultItem.Price = 0;
        //                    }

        //                    if (item.ref_Document_No == null)
        //                    {
        //                        item.ref_Document_No = "";
        //                    }
        //                    if (item.ref_Document_LineNum == null)
        //                    {
        //                        resultItem.Ref_Document_LineNum = refDocLineNum.ToString();
        //                    }
        //                    else
        //                    {
        //                        resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
        //                    }
        //                    //var itemlist = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem").Where(c => c.GoodsReceive_Index == itemHeader.GoodsReceiveIndex).ToList();
        //                    resultItem.Ref_Document_Index = new Guid(model.planGoodsReceive_Index);
        //                    resultItem.Ref_DocumentItem_Index = new Guid(model.planGoodsReceiveItem_Index);
        //                    resultItem.Ref_Document_No = model.planGoodsReceive_No;
        //                    resultItem.DocumentRef_No1 = item.documentRef_No1;
        //                    resultItem.DocumentRef_No2 = item.documentRef_No2;
        //                    resultItem.DocumentRef_No3 = item.documentRef_No3;
        //                    resultItem.DocumentRef_No4 = item.documentRef_No4;
        //                    resultItem.DocumentRef_No5 = item.documentRef_No5;
        //                    resultItem.Document_Status = document_status;
        //                    resultItem.UDF_1 = item.uDF_1;
        //                    resultItem.UDF_2 = item.uDF_2;
        //                    resultItem.UDF_3 = item.uDF_3;
        //                    resultItem.UDF_4 = item.uDF_4;
        //                    resultItem.UDF_5 = item.uDF_5;
        //                    resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
        //                    resultItem.GoodsReceive_DockDoor = "";
        //                    resultItem.Price = model.price;
        //                    resultItem.Create_By = model.create_By;
        //                    resultItem.Create_Date = DateTime.Now;
        //                    resultItem.Update_By = model.update_By;
        //                    resultItem.Update_Date = item.update_Date.toDate();
        //                    resultItem.Cancel_By = item.cancel_By;
        //                    resultItem.Cancel_Date = item.cancel_Date.toDate();
        //                    //itemDetail.Add(resultItem);
        //                    db.IM_GoodsReceiveItem.Add(resultItem);


        //                    WM_TagItem TagItem = new WM_TagItem();

        //                    if (!string.IsNullOrEmpty(model.tagItem_Index))
        //                    {
        //                        tagItem_Index = new Guid(model.tagItem_Index);
        //                    }
        //                    TagItem.TagItem_Index = tagItem_Index;
        //                    TagItem.Tag_Index = new Guid(model.tag_Index);
        //                    TagItem.Tag_No = model.tag_No;
        //                    TagItem.Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
        //                    TagItem.GoodsReceive_Index = GR.GoodsReceive_Index;
        //                    TagItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index.sParse<Guid>();
        //                    TagItem.Product_Index = item.product_Index.sParse<Guid>();
        //                    TagItem.Product_Id = item.product_Id;
        //                    TagItem.Product_Name = item.product_Name;
        //                    TagItem.Product_SecondName = item.product_SecondName;
        //                    TagItem.Product_ThirdName = item.product_ThirdName;
        //                    if (item.product_Lot != "")
        //                    {
        //                        TagItem.Product_Lot = item.product_Lot;
        //                    }
        //                    else
        //                    {
        //                        TagItem.Product_Lot = "";
        //                    }
        //                    if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
        //                    {
        //                        TagItem.ItemStatus_Index = item.itemStatus_Index.sParse<Guid>();
        //                    }
        //                    else
        //                    {
        //                        TagItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                    }
        //                    if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
        //                    {
        //                        TagItem.ItemStatus_Id = item.itemStatus_Id;
        //                    }
        //                    else
        //                    {
        //                        TagItem.ItemStatus_Id = model.itemStatus_Id;
        //                    }
        //                    if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
        //                    {
        //                        TagItem.ItemStatus_Name = item.itemStatus_Name;
        //                    }
        //                    else
        //                    {
        //                        TagItem.ItemStatus_Name = model.itemStatus_Name;
        //                    }

        //                    if (!string.IsNullOrEmpty(model.itemStatus_Index))
        //                    {
        //                        TagItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                    }
        //                    if (!string.IsNullOrEmpty(model.itemStatus_Id))
        //                    {
        //                        TagItem.ItemStatus_Id = model.itemStatus_Id;
        //                    }
        //                    if (!string.IsNullOrEmpty(model.itemStatus_Name))
        //                    {
        //                        TagItem.ItemStatus_Name = model.itemStatus_Name;
        //                    }


        //                    TagItem.Qty = item.qty;
        //                    TagItem.Ratio = item.ratio;
        //                    TagItem.TotalQty = item.qty * item.ratio;
        //                    TagItem.ProductConversion_Index = item.productConversion_Index.sParse<Guid>();
        //                    TagItem.ProductConversion_Id = item.productConversion_Id;
        //                    TagItem.ProductConversion_Name = item.productConversion_Name;
        //                    TagItem.MFG_Date = model.MFGDate.toDate();
        //                    TagItem.EXP_Date = model.EXPDate.toDate();
        //                    TagItem.Tag_Status = 0;
        //                    if (item.weight != null)
        //                    {
        //                        TagItem.Weight = item.weight;
        //                    }
        //                    else
        //                    {
        //                        TagItem.Weight = 0;
        //                    }
        //                    if (item.volume != null)
        //                    {
        //                        TagItem.Volume = item.volume;
        //                    }
        //                    else
        //                    {
        //                        TagItem.Volume = 0;
        //                    }
        //                    TagItem.TagRef_No1 = model.tagRef_No1;
        //                    TagItem.TagRef_No2 = model.tagRef_No2;
        //                    TagItem.TagRef_No3 = model.tagRef_No3;
        //                    TagItem.TagRef_No4 = model.tagRef_No4;
        //                    TagItem.TagRef_No5 = model.tagRef_No5;
        //                    TagItem.UDF_1 = item.uDF_1;
        //                    TagItem.UDF_2 = item.uDF_2;
        //                    TagItem.UDF_3 = item.uDF_3;
        //                    TagItem.UDF_4 = item.uDF_4;
        //                    TagItem.UDF_5 = item.uDF_5;
        //                    TagItem.Create_By = model.create_By;
        //                    TagItem.Create_Date = DateTime.Now;
        //                    TagItem.Update_By = model.update_By;
        //                    TagItem.Update_Date = item.update_Date.toDate();
        //                    TagItem.Cancel_By = item.cancel_By;
        //                    TagItem.Cancel_Date = item.cancel_Date.toDate();

        //                    db.wm_TagItem.Add(TagItem);
        //                }
        //            }
        //            else
        //            {
        //                switch (GR.Document_Status)
        //                {
        //                    case 1:
        //                        result.msg = "เอกสารนี้ทำการยืนยันเอกสารรอจัดเก็บแล้ว";
        //                        break;
        //                    case 3:
        //                        result.msg = "เอกสารนี้ทำการจัดเก็บเรียบร้อยแล้ว";
        //                        break;
        //                    case -1:
        //                        result.msg = "เอกสารนี้สถานะยกเลิก ไม่สามารถทำรายการได้";
        //                        break;
        //                    default:
        //                        result.msg = "GR Not Found";
        //                        break;
        //                }
        //                result.isUse = false;
        //                return result;
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(model.goodsReceive_Index))
        //            {
        //                goodsReceive_Index = new Guid(model.goodsReceive_Index);
        //            }

        //            //----Set Header------
        //            IM_GoodsReceive itemHeader = new IM_GoodsReceive();



        //            itemHeader.GoodsReceive_Index = goodsReceive_Index;
        //            itemHeader.GoodsReceive_No = model.goodsReceive_No;
        //            itemHeader.Owner_Index = new Guid(model.owner_Index);
        //            itemHeader.Owner_Id = model.owner_Id;
        //            itemHeader.Owner_Name = model.owner_Name;

        //            var pstring = "";
        //            string DocumentTypeIndex = new Guid("1CA67A6F-790B-4C32-92B0-E235F23BF103").ToString();
        //            if (model.documentType_Index == DocumentTypeIndex)
        //            {
        //                pstring = "Where DocumentType_Index = '" + DocumentTypeIndex + "' and DocumentType_Name_To = 'Manual create order'";
        //            }
        //            else
        //            {
        //                pstring = "Where DocumentType_Index = '" + DocumentTypeIndex + "'";
        //            }
        //            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
        //            var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index_To)");
        //            var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Id_To");
        //            var ColumnName4 = new SqlParameter("@ColumnName4", "DocumentType_Name_To");
        //            var ColumnName5 = new SqlParameter("@ColumnName5", "''");
        //            var TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[sy_DocumentTypeRef]");
        //            var Where = new SqlParameter("@Where", pstring);
        //            var DataDocumentTyperef = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();
        //            if (string.IsNullOrEmpty(model.dropdownDocumentType.documentType_Index.ToString()))
        //            {
        //                itemHeader.DocumentType_Index = new Guid(DataDocumentTyperef.dataincolumn2);
        //                itemHeader.DocumentType_Id = DataDocumentTyperef.dataincolumn3;
        //                itemHeader.DocumentType_Name = DataDocumentTyperef.dataincolumn4;

        //            }
        //            else
        //            {
        //                itemHeader.DocumentType_Index = model.dropdownDocumentType.documentType_Index;
        //                itemHeader.DocumentType_Id = model.dropdownDocumentType.documentType_Id;
        //                itemHeader.DocumentType_Name = model.dropdownDocumentType.documentType_Name;

        //            }

        //            if (string.IsNullOrEmpty(model.goodsReceive_No))
        //            {
        //                var doctype = new DocumentTypeViewModel();
        //                doctype.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
        //                doctype.documentType_Index = itemHeader.DocumentType_Index;
        //                //GetConfig
        //                var resultdoctype = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), doctype.sJson());

        //                DataTable resultDocumentType = CreateDataTable(resultdoctype);

        //                var DocumentType = new SqlParameter("DocumentType", SqlDbType.Structured);
        //                DocumentType.TypeName = "[dbo].[ms_DocumentTypeData]";
        //                DocumentType.Value = resultDocumentType;

        //                var DocumentType_Index = new SqlParameter("@DocumentType_Index", itemHeader.DocumentType_Index);
        //                var DocDate = new SqlParameter("@DocDate", model.goodsReceive_Date.toDate());
        //                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
        //                resultParameter.Size = 2000; // some meaningfull value
        //                resultParameter.Direction = ParameterDirection.Output;
        //                db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate, @DocumentType, @txtReturn OUTPUT", DocumentType_Index, DocDate, DocumentType, resultParameter);
        //                model.goodsReceive_No = resultParameter.Value.ToString();
        //            }
        //            itemHeader.GoodsReceive_No = model.goodsReceive_No;
        //            var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //            itemHeader.GoodsReceive_Date = !string.IsNullOrEmpty(model.goodsReceive_Date) ? model.goodsReceive_Date.toDate() : DateTime.Now;
        //            itemHeader.DocumentRef_No1 = model.documentRef_No1;
        //            itemHeader.DocumentRef_No2 = model.documentRef_No2;
        //            itemHeader.DocumentRef_No3 = model.documentRef_No3;
        //            itemHeader.DocumentRef_No4 = model.documentRef_No4;
        //            itemHeader.DocumentRef_No5 = model.documentRef_No5;
        //            itemHeader.Document_Status = document_status;
        //            itemHeader.UDF_1 = model.udf_1;
        //            itemHeader.UDF_2 = model.udf_2;
        //            itemHeader.UDF_3 = model.udf_3;
        //            itemHeader.UDF_4 = model.udf_4;
        //            itemHeader.UDF_5 = model.udf_5;
        //            itemHeader.DocumentPriority_Status = documentPriority_Status;
        //            itemHeader.Document_Remark = model.document_Remark;
        //            itemHeader.Create_Date = DateTime.Now;
        //            itemHeader.Create_By = model.create_By;
        //            itemHeader.Update_By = model.update_By;
        //            itemHeader.Update_Date = model.update_Date.toDate();
        //            itemHeader.Cancel_By = model.cancel_By;
        //            itemHeader.Cancel_Date = model.cancel_Date.toDate();
        //            itemHeader.Warehouse_Index = model.warehouse_Index.sParse<Guid?>();
        //            itemHeader.Warehouse_Id = model.warehouse_Id;
        //            itemHeader.Warehouse_Name = model.warehouse_Name;
        //            itemHeader.Warehouse_Index_To = model.warehouse_Index_To.sParse<Guid?>();
        //            itemHeader.Warehouse_Id_To = model.warehouse_Id_To;
        //            itemHeader.Warehouse_Name_To = model.warehouse_Name_To;
        //            itemHeader.Putaway_Status = putaway_Status;
        //            itemHeader.DockDoor_Index = model.dockDoor_Index.sParse<Guid?>();
        //            itemHeader.DockDoor_Id = model.dockDoor_Id;
        //            itemHeader.DockDoor_Name = model.dockDoor_Name;
        //            itemHeader.VehicleType_Index = model.vehicleType_Index.sParse<Guid?>();
        //            itemHeader.VehicleType_Id = model.vehicleType_Id;
        //            itemHeader.VehicleType_Name = model.vehicleType_Name;
        //            itemHeader.ContainerType_Index = model.containerType_Index.sParse<Guid?>();
        //            itemHeader.ContainerType_Id = model.containerType_Id;
        //            itemHeader.ContainerType_Name = model.containerType_Name;
        //            itemHeader.WHOwner_Index = model.whOwner_index.sParse<Guid?>();
        //            itemHeader.WHOwner_Id = model.whOwner_ID;
        //            itemHeader.WHOwner_Name = model.whOwner_Name;

        //            //----Set Detail-----
        //            //var itemDetail = new List<GoodsReceiveItemViewModel>();
        //            db.IM_GoodsReceive.Add(itemHeader);
        //            int addNumber = 0;
        //            foreach (var item in model.listPlanGoodsReceiveItemViewModel)
        //            {

        //                addNumber++;
        //                IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

        //                resultItem.Ref_Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

        //                // Gen Index for line item
        //                if (string.IsNullOrEmpty(item.goodsReceiveItem_Index.ToString()))
        //                {
        //                    item.goodsReceiveItem_Index = Guid.NewGuid();
        //                }
        //                resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index.sParse<Guid>();

        //                // Index From Header
        //                resultItem.GoodsReceive_Index = goodsReceive_Index;
        //                if (item.lineNum == null)
        //                {
        //                    resultItem.LineNum = addNumber.ToString();
        //                }
        //                else
        //                {
        //                    resultItem.LineNum = item.lineNum;
        //                }

        //                resultItem.Product_Index = item.product_Index.sParse<Guid>();
        //                resultItem.Product_Id = item.product_Id;
        //                resultItem.Product_Name = item.product_Name;
        //                resultItem.Product_SecondName = item.product_SecondName;
        //                resultItem.Product_ThirdName = item.product_ThirdName;
        //                if (item.product_Lot != "")
        //                {
        //                    resultItem.Product_Lot = item.product_Lot;
        //                }
        //                else
        //                {
        //                    resultItem.Product_Lot = "";
        //                }
        //                if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
        //                {
        //                    resultItem.ItemStatus_Index = item.itemStatus_Index.sParse<Guid>();
        //                }
        //                else
        //                {
        //                    resultItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                }
        //                if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
        //                {
        //                    resultItem.ItemStatus_Id = item.itemStatus_Id;
        //                }
        //                else
        //                {
        //                    resultItem.ItemStatus_Id = model.itemStatus_Id;
        //                }
        //                if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
        //                {
        //                    resultItem.ItemStatus_Name = item.itemStatus_Name;
        //                }
        //                else
        //                {
        //                    resultItem.ItemStatus_Name = model.itemStatus_Name;
        //                }
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Index))
        //                //{
        //                //    resultItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                //}
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Id))
        //                //{
        //                //    resultItem.ItemStatus_Id = model.itemStatus_Id;
        //                //}
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Name))
        //                //{
        //                //    resultItem.ItemStatus_Name = model.itemStatus_Name;
        //                //}


        //                resultItem.Qty = item.qty;
        //                resultItem.Ratio = item.ratio;
        //                resultItem.TotalQty = item.qty * item.ratio;
        //                resultItem.UDF_1 = item.uDF_1;
        //                resultItem.ProductConversion_Index = item.productConversion_Index.sParse<Guid>();
        //                resultItem.ProductConversion_Id = item.productConversion_Id;
        //                resultItem.ProductConversion_Name = item.productConversion_Name;
        //                resultItem.MFG_Date = model.MFGDate.toDate();
        //                resultItem.EXP_Date = model.EXPDate.toDate();
        //                if (item.unitWeight != null)
        //                {
        //                    resultItem.UnitWeight = item.unitWeight;
        //                }
        //                else
        //                {
        //                    resultItem.UnitWeight = 0;
        //                }

        //                if (item.weight != null)
        //                {
        //                    resultItem.Weight = item.weight;
        //                }
        //                else
        //                {
        //                    resultItem.Weight = 0;
        //                }

        //                if (item.unitWidth != null)
        //                {
        //                    resultItem.UnitWidth = item.unitWidth;
        //                }
        //                else
        //                {
        //                    resultItem.UnitWidth = 0;
        //                }

        //                if (item.unitLength != null)
        //                {
        //                    resultItem.UnitLength = item.unitLength;
        //                }
        //                else
        //                {
        //                    resultItem.UnitLength = 0;
        //                }

        //                if (item.unitHeight != null)
        //                {
        //                    resultItem.UnitHeight = item.unitHeight;
        //                }
        //                else
        //                {
        //                    resultItem.UnitHeight = 0;
        //                }

        //                if (item.unitVolume != null)
        //                {
        //                    resultItem.UnitVolume = item.unitVolume;
        //                }
        //                else
        //                {
        //                    resultItem.UnitVolume = 0;
        //                }

        //                if (item.volume != null)
        //                {
        //                    resultItem.Volume = item.volume;
        //                }
        //                else
        //                {
        //                    resultItem.Volume = 0;
        //                }

        //                if (item.unitPrice != null)
        //                {
        //                    resultItem.UnitPrice = item.unitPrice;
        //                }
        //                else
        //                {
        //                    resultItem.UnitPrice = 0;
        //                }

        //                if (item.price != null)
        //                {
        //                    resultItem.Price = item.price;
        //                }
        //                else
        //                {
        //                    resultItem.Price = 0;
        //                }

        //                if (item.ref_Document_No == null)
        //                {
        //                    item.ref_Document_No = "";
        //                }
        //                if (item.ref_Document_LineNum == null)
        //                {
        //                    resultItem.Ref_Document_LineNum = refDocLineNum.ToString();
        //                }
        //                else
        //                {
        //                    resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
        //                }
        //                //var itemlist = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem").Where(c => c.GoodsReceive_Index == itemHeader.GoodsReceiveIndex).ToList();
        //                resultItem.Ref_Document_Index = new Guid(model.planGoodsReceive_Index);
        //                resultItem.Ref_DocumentItem_Index = new Guid(model.planGoodsReceiveItem_Index);
        //                resultItem.Ref_Document_No = model.planGoodsReceive_No;
        //                resultItem.DocumentRef_No1 = item.documentRef_No1;
        //                resultItem.DocumentRef_No2 = item.documentRef_No2;
        //                resultItem.DocumentRef_No3 = item.documentRef_No3;
        //                resultItem.DocumentRef_No4 = item.documentRef_No4;
        //                resultItem.DocumentRef_No5 = item.documentRef_No5;
        //                resultItem.Document_Status = document_status;
        //                resultItem.UDF_1 = item.uDF_1;
        //                resultItem.UDF_2 = item.uDF_2;
        //                resultItem.UDF_3 = item.uDF_3;
        //                resultItem.UDF_4 = item.uDF_4;
        //                resultItem.UDF_5 = item.uDF_5;
        //                resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
        //                resultItem.GoodsReceive_DockDoor = "";
        //                resultItem.Price = model.price;
        //                resultItem.Create_By = model.create_By;
        //                resultItem.Create_Date = DateTime.Now;
        //                resultItem.Update_By = model.update_By;
        //                resultItem.Update_Date = item.update_Date.toDate();
        //                resultItem.Cancel_By = item.cancel_By;
        //                resultItem.Cancel_Date = item.cancel_Date.toDate();
        //                //itemDetail.Add(resultItem);
        //                db.IM_GoodsReceiveItem.Add(resultItem);

        //                WM_TagItem TagItem = new WM_TagItem();

        //                if (!string.IsNullOrEmpty(model.tagItem_Index))
        //                {
        //                    tagItem_Index = new Guid(model.tagItem_Index);
        //                }
        //                TagItem.TagItem_Index = tagItem_Index;
        //                TagItem.Tag_Index = new Guid(model.tag_Index);
        //                TagItem.Tag_No = model.tag_No;
        //                TagItem.Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
        //                TagItem.GoodsReceive_Index = goodsReceive_Index;
        //                TagItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index.sParse<Guid>();
        //                TagItem.Product_Index = item.product_Index.sParse<Guid>();
        //                TagItem.Product_Id = item.product_Id;
        //                TagItem.Product_Name = item.product_Name;
        //                TagItem.Product_SecondName = item.product_SecondName;
        //                TagItem.Product_ThirdName = item.product_ThirdName;
        //                if (item.product_Lot != "")
        //                {
        //                    TagItem.Product_Lot = item.product_Lot;
        //                }
        //                else
        //                {
        //                    TagItem.Product_Lot = "";
        //                }
        //                if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
        //                {
        //                    TagItem.ItemStatus_Index = item.itemStatus_Index.sParse<Guid>();
        //                }
        //                else
        //                {
        //                    TagItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                }
        //                if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
        //                {
        //                    TagItem.ItemStatus_Id = item.itemStatus_Id;
        //                }
        //                else
        //                {
        //                    TagItem.ItemStatus_Id = model.itemStatus_Id;
        //                }
        //                if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
        //                {
        //                    TagItem.ItemStatus_Name = item.itemStatus_Name;
        //                }
        //                else
        //                {
        //                    TagItem.ItemStatus_Name = model.itemStatus_Name;
        //                }
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Index))
        //                //{
        //                //    TagItem.ItemStatus_Index = new Guid(model.itemStatus_Index);
        //                //}
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Id))
        //                //{
        //                //    TagItem.ItemStatus_Id = model.itemStatus_Id;
        //                //}
        //                //if (!string.IsNullOrEmpty(model.itemStatus_Name))
        //                //{
        //                //    TagItem.ItemStatus_Name = model.itemStatus_Name;
        //                //}


        //                TagItem.Qty = item.qty;
        //                TagItem.Ratio = item.ratio;
        //                TagItem.TotalQty = item.qty * item.ratio;
        //                TagItem.ProductConversion_Index = item.productConversion_Index.sParse<Guid>();
        //                TagItem.ProductConversion_Id = item.productConversion_Id;
        //                TagItem.ProductConversion_Name = item.productConversion_Name;
        //                TagItem.MFG_Date = item.mFG_Date.toDate();
        //                TagItem.EXP_Date = item.eXP_Date.toDate();
        //                TagItem.Tag_Status = 0;
        //                if (item.weight != null)
        //                {
        //                    TagItem.Weight = item.weight;
        //                }
        //                else
        //                {
        //                    TagItem.Weight = 0;
        //                }
        //                if (item.volume != null)
        //                {
        //                    TagItem.Volume = item.volume;
        //                }
        //                else
        //                {
        //                    TagItem.Volume = 0;
        //                }
        //                TagItem.TagRef_No1 = model.tagRef_No1;
        //                TagItem.TagRef_No2 = model.tagRef_No2;
        //                TagItem.TagRef_No3 = model.tagRef_No3;
        //                TagItem.TagRef_No4 = model.tagRef_No4;
        //                TagItem.TagRef_No5 = model.tagRef_No5;
        //                TagItem.UDF_1 = item.uDF_1;
        //                TagItem.UDF_2 = item.uDF_2;
        //                TagItem.UDF_3 = item.uDF_3;
        //                TagItem.UDF_4 = item.uDF_4;
        //                TagItem.UDF_5 = item.uDF_5;
        //                TagItem.Create_By = model.create_By;
        //                TagItem.Create_Date = DateTime.Now;
        //                TagItem.Update_By = model.update_By;
        //                TagItem.Update_Date = item.update_Date.toDate();
        //                TagItem.Cancel_By = item.cancel_By;
        //                TagItem.Cancel_Date = item.cancel_Date.toDate();

        //                db.wm_TagItem.Add(TagItem);

        //            }

        //            var listResultitem = new List<ResultScanReceiveViewModel>();
        //            var resultitem = new ResultScanReceiveViewModel();
        //            resultitem.planGoodsReceive_Index = model.planGoodsReceive_Index;
        //            resultitem.planGoodsReceive_No = model.planGoodsReceive_No;
        //            resultitem.goodsReceive_Index = goodsReceive_Index.ToString();
        //            resultitem.goodsReceive_No = model.goodsReceive_No;
        //            resultitem.owner_Index = model.owner_Index.ToString();
        //            resultitem.owner_Id = model.owner_Id;
        //            resultitem.owner_Name = model.owner_Name;
        //            resultitem.documentType_Index = model.dropdownDocumentType.documentType_Index.ToString();
        //            resultitem.documentType_Id = model.dropdownDocumentType.documentType_Id;
        //            resultitem.documentType_Name = model.dropdownDocumentType.documentType_Name;
        //            resultitem.whOwner_Index = model.whOwner_index.ToString();
        //            resultitem.whOwner_Id = model.whOwner_ID;
        //            resultitem.whOwner_Name = model.whOwner_Name;
        //            resultitem.goodsReceive_Date = model.goodsReceive_Date;
        //            resultitem.planGoodsReceive_Date = model.planGoodsReceive_Date;
        //            listResultitem.Add(resultitem);
        //            result.item = listResultitem;
        //            result.isUse = true;
        //            result.msg = "NEW";


        //        }
        //        var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
        //        try
        //        {
        //            db.SaveChanges();
        //            transactionx.Commit();
        //        }

        //        catch (Exception exy)
        //        {
        //            msglog = State + " ex Rollback " + exy.Message.ToString();
        //            olog.logging("SaveGR", msglog);
        //            transactionx.Rollback();

        //            throw exy;

        //        }

        //        #endregion


        //        result.isUse = true;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //public actionResultScanReceiveProductViewModel ScanReceiveProduct(ScanReceiveViewModel model)
        //{
        //    var result = new actionResultScanReceiveProductViewModel();
        //    try
        //    {
        //        var resultProduct = new List<ProductDetailViewModel>();

        //        var filterModelProduct = new ProductDetailViewModel();

        //        filterModelProduct.owner_Index = Guid.Parse(model.owner_Index);
        //        filterModelProduct.productConversionBarcode = model.productBarcode;

        //        resultProduct = utils.SendDataApi<List<ProductDetailViewModel>>(new AppSettingConfig().GetUrl("productDetail"), filterModelProduct.sJson());

        //        if (resultProduct.Count == 0)
        //        {
        //            result.isUse = false;
        //            result.msg = "Not found!!!";
        //            return result;
        //        }

        //        var resultReceiveItem = new List<PlanGoodsReceiveItemViewModel>();

        //        var filterModel = new PlanGoodsReceiveItemViewModel();

        //        if (!string.IsNullOrEmpty(model.planGoodsReceive_Index))
        //        {
        //            filterModel.planGoodsReceive_Index = Guid.Parse(model.planGoodsReceive_Index);

        //        }
        //        if (!string.IsNullOrEmpty(resultProduct.FirstOrDefault().product_Index.ToString()))
        //        {
        //            filterModel.product_Index = resultProduct.FirstOrDefault().product_Index;

        //        }

        //        //GetConfig
        //        resultReceiveItem = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("getScanPlanGRI"), filterModel.sJson());
        //        var items = new List<ResultScanReceiveProductViewModel>();
        //        if (resultReceiveItem.Count > 0)
        //        {
        //            foreach (var r in resultReceiveItem)
        //            {
        //                var item = new ResultScanReceiveProductViewModel();

        //                item.qtyPlan = r.totalQty;
        //                var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == Guid.Parse(model.planGoodsReceive_Index) && c.Ref_DocumentItem_Index == r.planGoodsReceiveItem_Index).FirstOrDefault();
        //                if (TotalGR != null)
        //                {
        //                    r.totalQty = (r.totalQty - TotalGR.TotalQty);
        //                    r.qty = (r.qty - TotalGR.Qty);
        //                }

        //                var product = resultProduct.FirstOrDefault();

        //                //Header
        //                item.goodsReceive_Index = model.goodsReceive_Index;
        //                item.goodsReceive_No = model.goodsReceive_No;
        //                item.goodsReceive_Date = model.goodsReceive_Date;
        //                item.goodsReceive_Date_To = model.goodsReceive_Date_To;
        //                item.planGoodsReceive_Index = model.planGoodsReceive_Index;
        //                item.planGoodsReceive_No = model.planGoodsReceive_No;
        //                item.planGoodsReceive_Date = model.planGoodsReceive_Date;
        //                item.owner_Index = model.owner_Index;
        //                item.owner_Id = model.owner_Id;
        //                item.owner_Name = model.owner_Name;
        //                item.documentType_Index = model.dropdownDocumentType.documentType_Index.ToString();
        //                item.documentType_Id = model.dropdownDocumentType.documentType_Id;
        //                item.documentType_Name = model.dropdownDocumentType.documentType_Name;
        //                item.whOwner_Index = model.whOwner_index;
        //                item.whOwner_Id = model.whOwner_ID;
        //                item.whOwner_Name = model.whOwner_Name;
        //                item.productBarcode = model.productBarcode;
        //                //product
        //                item.product_Index = product.product_Index.ToString();
        //                item.product_Id = product.product_Id;
        //                item.product_Name = product.product_Name;
        //                item.productConversion_Index = product.productConversion_Index.ToString();
        //                item.productConversion_Id = product.productConversion_Id;
        //                item.productConversion_Name = product.productConversion_Name;
        //                item.baseProductConversion = product.baseProductConversion;
        //                item.product_SecondName = product.product_SecondName;
        //                item.product_ThirdName = product.product_ThirdName;
        //                item.isLot = product.isLot;
        //                item.isExpDate = product.isExpDate;
        //                item.isMfgDate = product.isMfgDate;
        //                item.isCatchWeight = product.isCatchWeight;
        //                item.productConversion_Ratio = product.productConversion_Ratio;
        //                item.productitemlife_y = product.productItemLife_Y;
        //                item.productitemlife_m = product.productItemLife_M;
        //                item.productitemlife_d = product.productItemLife_D;
        //                item.suggestLocation = product.suggestLocation;
        //                //GRItem
        //                item.ratio = r.ratio;
        //                item.planGoodsReceiveItem_Index = r.planGoodsReceiveItem_Index.ToString();
        //                item.itemStatus_Index = r.itemStatus_Index.ToString();
        //                item.itemStatus_Id = r.itemStatus_Id;
        //                item.itemStatus_Name = r.itemStatus_Name;
        //                item.volume = r.volume;
        //                item.weight = r.weight;
        //                item.totalQty = r.totalQty;

        //                items.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            result.isUse = false;
        //            result.msg = "GR Item Not Found";
        //            return result;
        //        }
        //        result.isUse = true;
        //        result.item = items;

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        result.isUse = false;
        //        result.msg = ex.Message;
        //        return result;
        //    }
        //}
    }
}
