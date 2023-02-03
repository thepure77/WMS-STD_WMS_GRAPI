using AspNetCore.Reporting;
using DataAccess;
using GRBusiness.GoodsReceive;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using GRBusiness.Reports.Tag;
using GRBusiness.Libs;
using Business.Library;
using GRBusiness.ConfigModel;
using Comone.Utils;
using System.Linq.Expressions;
using GRDataAccess.Models;
using QRCoder;
using System.Drawing;
using System.IO;
using GRBusiness.Reports;

namespace GRBusiness.LPNItem
{
    public class LPNItemService
    {
        private GRDbContext db;

        public LPNItemService()
        {
            db = new GRDbContext();
        }

        public LPNItemService(GRDbContext db)
        {
            this.db = db;
        }

        public List<LPNItemViewModel> FilterTagItem(SearchGRModel data)
        {
            try
            {
                var result = new List<LPNItemViewModel>();

                using (var context = new GRDbContext())
                {
                    var queryResult = db.wm_TagItem.AsQueryable();

                    if (!string.IsNullOrEmpty(data.goodsReceive_No))
                    {
                        var query = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No == data.goodsReceive_No).FirstOrDefault();

                        queryResult = queryResult.Where(c => c.GoodsReceive_Index == query.GoodsReceive_Index && c.Tag_Status != -1 && (c.Process_Index == new Guid("58400298-4347-488C-AF71-76B78A44232D") || c.Process_Index == new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA"))).OrderBy(o => o.Tag_No);
                    }

                    if (!string.IsNullOrEmpty(data.tag_No))
                    {
                        queryResult = queryResult.Where(c => c.Tag_No == data.tag_No && c.Tag_Status == 1);
                    }

                    if (data.product_Index != new Guid("00000000-0000-0000-0000-000000000000") && data.product_Index != null)
                    //if (!string.IsNullOrEmpty(data.product_Index.ToString()))
                    {
                        queryResult = queryResult.Where(c => c.Product_Index == data.product_Index);
                    }

                    var queryResultItem = queryResult.ToList();
                    var queryResultItem_index = queryResult.Select(s => s.GoodsReceive_Index).FirstOrDefault();

                    var queryGIL = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == queryResultItem_index).ToList();

                    var filterModel = new ProcessStatusViewModel();
                    filterModel.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");

                    var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                    foreach (var items in queryResultItem.OrderBy(o => o.Tag_No).OrderBy(o => o.Suggest_Location_Id))
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
                        model.qty = items.Qty.ToString();
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
                        // model.tag_Status = items.Tag_Status;
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
                        model.erp_Location = items.ERP_Location;

                        if (queryGIL.Count > 0)
                        {
                            var resultGIL = queryGIL.Where(c => c.GoodsReceiveItem_Index == model.goodsReceiveItem_Index && c.TagItem_Index == model.tagItem_Index).FirstOrDefault();
                            if (resultGIL != null)
                            {
                                model.putaway_By = resultGIL.Putaway_By;
                                var Putaway_Status = resultGIL.Putaway_Status.ToString();
                                model.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                                model.tag_Status = items.Tag_Status;

                            }
                        }
                        else
                        {
                            var Putaway_Status = 0.ToString();
                            model.putaway_Status = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                            model.tag_Status = 0;

                        }


                        result.Add(model);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string ConfirmTagItemLocation(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            string result = "";

            using (var context = new GRDbContext())
            {
                if (data.listLPNItemViewModel.Count == 0)
                {
                    return result;
                }

                //var GetViewBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinbalanceGreaterThanZero"), new { }.sJson());

                foreach (var item in data.listLPNItemViewModel)
                {
                    var tagItem = db.wm_TagItem.Find(item.tagItem_Index);

                    #region GetLocation
                    //var LocationViewModel = new LocationViewModel();
                    //LocationViewModel.location_Name = item.suggest_Location_Name;
                    //var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LocationViewModel.sJson());

                    try
                    {
                        List<View_CheckLocation> CheckLocation = db.View_CheckLocation.Where(c => c.Location_Name == item.suggest_Location_Name).ToList();

                        if (CheckLocation.Count() <= 0)
                        {
                            return "ไม่สามารถ แนะนำไปตำแน่งที่ถูกแนะนำแล้วหรือมีของอยู่ได้";
                        }
                    }
                    catch (Exception ex)
                    {

                        return "ไม่สามารถ ค้นหา location ได้" ;
                    }

                    var LocationViewModel = new { location_Name = item.suggest_Location_Name };
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LocationViewModel.sJson());

                    var DataLocation = GetLocation.FirstOrDefault(c => c.blockPut != 1);
                    #endregion

                    //if (GetLocation.Count > 0)
                    if (DataLocation != null)
                    {
                        var GetViewBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinbalanceGreaterThanZeroV2"), new { Location_Index = DataLocation.location_Index }.sJson());
                        var databinbalance = GetViewBinbalance.Where(c => c.Location_Index == DataLocation.location_Index && c.BinBalance_QtyBal > 0).Count();

                        List<int?> notstatus = new List<int?> { -1, 2 };
                        var checktag = db.wm_TagItem.Where(c => c.Suggest_Location_Index == DataLocation.location_Index && !notstatus.Contains(c.Tag_Status)).ToList();

                        if (DataLocation.max_Pallet - checktag.Count() > 0 && DataLocation.max_Pallet - databinbalance > 0)
                        {
                            tagItem.Suggest_Location_Index = DataLocation.location_Index;
                            tagItem.Suggest_Location_Id = DataLocation.location_Id;
                            tagItem.Suggest_Location_Name = DataLocation.location_Name;
                            tagItem.Update_By = data.update_By;
                            tagItem.Update_Date = DateTime.Now;
                            result = "S";
                        }
                        else
                        {
                            return "ตำแหน่งที่จัดเก็บ Art เต็ม";
                        }
                    }
                    else
                    {
                        return "ไม่พบตำแหน่งที่กำหนด";
                    }

                }

                var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("CreatePackItem", msglog);
                    transaction.Rollback();
                    throw ex;
                }
            }

            return result;
        }

        //#region findTagItem
        //public List<printTagModel> findTagItem(Guid? tagItemIndex)
        //{
        //    try
        //    {
        //        var query = db.wm_TagItem.Where(c => c.TagItem_Index == tagItemIndex && c.Tag_Status != -1).ToList();

        //        var result = new List<printTagModel>();

        //        foreach (var item in query)
        //        {
        //            var resultItem = new printTagModel();

        //            resultItem.TagItem_Index = item.TagItem_Index;
        //            resultItem.Tag_Index = item.Tag_Index;
        //            resultItem.Tag_No = item.Tag_No;
        //            resultItem.TagNo_Barcode = new NetBarcode.Barcode(item.Tag_No, NetBarcode.Type.Code128B).GetBase64Image();
        //            resultItem.GoodsReceive_Index = item.GoodsReceive_Index;
        //            resultItem.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
        //            resultItem.Product_Index = item.Product_Index;
        //            resultItem.Product_Id = item.Product_Id;
        //            resultItem.Product_Name = item.Product_Name;
        //            resultItem.Product_SecondName = item.Product_SecondName;
        //            resultItem.Product_ThirdName = item.Product_ThirdName;
        //            resultItem.Product_Lot = item.Product_Lot;
        //            resultItem.ItemStatus_Index = item.ItemStatus_Index;
        //            resultItem.ItemStatus_Id = item.ItemStatus_Id;
        //            resultItem.ItemStatus_Name = item.ItemStatus_Name;
        //            resultItem.Suggest_Location_Index = item.Suggest_Location_Index;
        //            resultItem.Suggest_Location_Id = item.Suggest_Location_Id;
        //            resultItem.Suggest_Location_Name = item.Suggest_Location_Name;
        //            resultItem.Qty = item.Qty;
        //            resultItem.Ratio = item.Ratio;
        //            resultItem.TotalQty = item.TotalQty;
        //            resultItem.ProductConversion_Index = item.ProductConversion_Index;
        //            resultItem.ProductConversion_Id = item.ProductConversion_Id;
        //            resultItem.ProductConversion_Name = item.ProductConversion_Name;
        //            resultItem.Weight = item.Weight;
        //            resultItem.Volume = item.Volume;
        //            resultItem.MFG_Date = item.MFG_Date;
        //            resultItem.EXP_Date = item.EXP_Date;
        //            resultItem.TagRef_No1 = item.TagRef_No1;
        //            resultItem.TagRef_No2 = item.TagRef_No2;
        //            resultItem.TagRef_No3 = item.TagRef_No3;
        //            resultItem.TagRef_No4 = item.TagRef_No4;
        //            resultItem.TagRef_No5 = item.TagRef_No5;
        //            resultItem.Tag_Status = item.Tag_Status;
        //            resultItem.UDF_1 = item.UDF_1;
        //            resultItem.UDF_2 = item.UDF_2;
        //            resultItem.UDF_3 = item.UDF_3;
        //            resultItem.UDF_4 = item.UDF_4;
        //            resultItem.UDF_5 = item.UDF_5;
        //            resultItem.Create_By = item.Create_By;
        //            resultItem.Create_Date = item.Create_Date;
        //            resultItem.Update_By = item.Update_By;
        //            resultItem.Update_Date = item.Update_Date;
        //            resultItem.Cancel_By = item.Cancel_By;
        //            resultItem.Cancel_Date = item.Cancel_Date;
        //            result.Add(resultItem);
        //        }
        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //#endregion

        #region PrintTag
        public string PrintTag(LPNItemViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var resTagItem = new List<printTagModel>();
                var queryGr = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == data.listLPNItemViewModel[0].goodsReceive_Index);

                string date = queryGr.GoodsReceive_Date.toString();
                string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                foreach (var item in data.listLPNItemViewModel)
                {
                    var resultItem = new printTagModel();

                    string strMFGDate = item.mFG_Date.toString();
                    string MFGDate = DateTime.ParseExact(strMFGDate.Substring(0, 8), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                    string strEXPDate = item.eXP_Date.toString();
                    string EXPDate = DateTime.ParseExact(strEXPDate.Substring(0, 8), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                    resultItem.tagNo_Barcode = new NetBarcode.Barcode(item.tag_No, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.product_Barcode = new NetBarcode.Barcode(item.product_Id, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.product_Name = item.product_Name;
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Lot = item.product_Lot;
                    resultItem.qty = Convert.ToDecimal(item.qty);
                    resultItem.eXP_Date = EXPDate;
                    resultItem.volume = Convert.ToDecimal(item.volume);
                    resultItem.mFG_Date = MFGDate;
                    resultItem.tag_No = item.tag_No;
                    resultItem.productConversion_Name = item.productConversion_Name;
                    resultItem.goodsReceive_No = queryGr.GoodsReceive_No;
                    resultItem.owner_Name = queryGr.Owner_Name;
                    resultItem.invoice_No = queryGr.Invoice_No;
                    resultItem.goodsReceive_Date = GRDate;

                    resTagItem.Add(resultItem);

                }

                resTagItem.ToList();



                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\Tag\\printTag.rdlc";
                var reportPath = rootPath + "\\Reports\\Tag\\printTag.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region PrintTagA5
        public string PrintTagA5(LPNItemViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var resTagItem = new List<TagA5Model>();
                var queryGr = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == data.listLPNItemViewModel[0].goodsReceive_Index);
                //var queryGrI = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == Guid.Parse("6F02FE11-F54A-4956-9FE9-DAA3BDC9114C")).ToList();

                string date = queryGr.GoodsReceive_Date.toString();
                string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                foreach (var item in data.listLPNItemViewModel)
                {
                    var resultItem = new TagA5Model();

                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.planGoodsReceive_Date = db.IM_GoodsReceiveItem.Find(item.goodsReceiveItem_Index).Ref_Document_No;
                    resultItem.documentRef_No1 = queryGr.DocumentRef_No1;
                    resultItem.documentRef_No2 = queryGr.DocumentRef_No2;
                    resultItem.owner_Name = queryGr.Owner_Name;
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Name = item.product_Name;
                    resultItem.qty = item.qty.sParse<decimal>();
                    resultItem.productConversion_Name = item.productConversion_Name;
                    resultItem.tag_No = item.tag_No;
                    //resultItem.product_Bacode = new NetBarcode.Barcode(item.Product_Id, NetBarcode.Type.Code128B).GetBase64Image;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.tag_No,
                    QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    resultItem.product_Bacode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));

                    resTagItem.Add(resultItem);

                }
                resTagItem.ToList();



                rootPath = rootPath.Replace("\\GRAPI", "");
                var reportPath = rootPath + "\\GRBusiness\\Reports\\TagA5\\TagA5.rdlc";
                //var reportPath = rootPath + "\\Reports\\TagA5\\TagA5.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region PrintTagA3
        public string PrintTagA3(LPNItemViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var resTagItem = new List<TagA3Model>();
                foreach (var item in data.listLPNItemViewModel)
                {
                    var resultItem = new TagA3Model();
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Name = item.product_Name;
                    resultItem.tag_No = item.tag_No;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.tag_No,
                    QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    resultItem.product_Bacode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));

                    resTagItem.Add(resultItem);

                }
                resTagItem.ToList();



                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\TagA3\\TagA3.rdlc";
                var reportPath = rootPath + "\\Reports\\TagA3\\TagA3.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region PrintTagA2
        public string PrintTagA2(LPNItemViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var resTagItem = new List<TagA3Model>();
                foreach (var item in data.listLPNItemViewModel)
                {
                    var resultItem = new TagA3Model();
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Name = item.product_Name;
                    resultItem.tag_No = item.tag_No;
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.tag_No,
                    QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    resultItem.product_Bacode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));

                    resTagItem.Add(resultItem);

                }
                resTagItem.ToList();



                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\TagA2\\TagA2.rdlc";
                var reportPath = rootPath + "\\Reports\\TagA2\\TagA2.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


        //#region PrintTag
        //public string PrintTag(LPNItemViewModel data, string rootPath = "")
        //{
        //    String State = "Start";
        //    String msglog = "";
        //    var olog = new logtxt();

        //    Guid? planGiItemIndex = new Guid();

        //    try
        //    {

        //        var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        //        try
        //        {

        //            List<printTagModel> resTagItem = findTagItem(data.listLPNItemViewModel[0].tagItem_Index);
        //            rootPath = rootPath.Replace("\\GRAPI", "");
        //            var reportPath = rootPath + "\\GRBusiness\\Reports\\Tag\\printTag.rdlc";
        //            //var reportPath = rootPath + "\\Report\\Pack\\StockMovementReport.rdlc";
        //            LocalReport report = new LocalReport(reportPath);
        //            report.AddDataSource("DataSet1", resTagItem);

        //            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //            string fileName = "";
        //            string fullPath = "";
        //            fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

        //            var renderedBytes = report.Execute(RenderType.Pdf);

        //            Utils objReport = new Utils();
        //            fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
        //            var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
        //            transaction.Commit();
        //            return saveLocation;
        //        }
        //        catch (Exception exy)
        //        {
        //            msglog = State + " ex Rollback " + exy.Message.ToString();
        //            olog.logging("ClosePack", msglog);
        //            transaction.Rollback();
        //            throw exy;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //#endregion

        #region DeleteTagItem
        public string DeleteTagItem(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            string result = "";

            using (var context = new GRDbContext())
            {
                foreach (var item in data.listLPNItemViewModel)
                {
                    var queryGIL = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == item.goodsReceive_Index && c.TagItem_Index == item.tagItem_Index && c.Putaway_Status == 1).FirstOrDefault();

                    if (queryGIL != null)
                    {
                        result = "CAN_NOT_DELETE";

                        return result;

                    }

                    var tagItem = db.wm_TagItem.Find(item.tagItem_Index);
                    tagItem.Cancel_By = data.cancel_By;
                    tagItem.Cancel_Date = DateTime.Now;
                    tagItem.Tag_Status = -1;
                }



                var transaction = db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();

                    var group_Index = data.listLPNItemViewModel.GroupBy(g => g.tag_Index).ToList();
                    foreach (var g in group_Index)
                    {
                        var checkTagItem = db.wm_TagItem.Where(c => c.Tag_Index == g.Key && c.Tag_Status != -1).Count();
                        if (checkTagItem == 0)
                        {
                            var tag = db.WM_Tag.Find(g.Key).Tag_Status = -1;
                        }

                    }

                    db.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    msglog = State + " ex Rollback " + ex.Message.ToString();
                    olog.logging("CreatePackItem", msglog);
                    transaction.Rollback();
                    throw ex;
                }
            }

            return result;
        }
        #endregion

        #region wm_TagItem
        public List<LPNItemViewModel> wm_TagItem(DocumentViewModel model)
        {
            try
            {
                var query = db.wm_TagItem.AsQueryable();

                var result = new List<LPNItemViewModel>();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.Tag_Index));
                }

                if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Tag_Status));
                }

                if (model.listDocumentViewModel.FirstOrDefault().document_No != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_No).Contains(c.Tag_No));
                }

                if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.TagItem_Index));
                }

                if (model.listDocumentViewModel.FirstOrDefault().ref_documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.ref_documentItem_Index).Contains(c.GoodsReceiveItem_Index));
                }


                var queryresult = query.ToList();

                foreach (var item in queryresult)
                {
                    var resultItem = new LPNItemViewModel();

                    resultItem.tag_Index = item.Tag_Index;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.tagItem_Index = item.TagItem_Index;
                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.suggest_Location_Index = item.Suggest_Location_Index;
                    resultItem.suggest_Location_Id = item.Suggest_Location_Id;
                    resultItem.suggest_Location_Name = item.Suggest_Location_Name;
                    resultItem.qty = item.Qty.ToString();
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.weight = item.Weight.ToString();
                    resultItem.volume = item.Volume.ToString();
                    resultItem.mFG_Date = item.MFG_Date;
                    resultItem.eXP_Date = item.EXP_Date;
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
                    resultItem.tag_Status = item.Tag_Status;
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


        public string SugesstionLocationOld(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            string result = "";
            var tagitem_index = new List<Guid?>();
            try
            {
                using (var context = new GRDbContext())
                {
                    if (data.listLPNItemViewModel.Count == 0)
                    {
                        result = "กรุณาเลือกเลขที่พาเลท";
                        return result;
                    }

                    tagitem_index = data.listLPNItemViewModel.Select(s => s.tagItem_Index).ToList();

                    var updatestatustagitem = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && w.Tag_Status == 0).ToList();
                    foreach (var u in updatestatustagitem)
                    {
                        u.Tag_Status = -2;
                    }
                    if (updatestatustagitem.Count() > 0)
                    {
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception ex)
                        {
                            msglog = State + " ex Rollback " + ex.Message.ToString();
                            olog.logging("CreatePackItem", msglog);
                            transaction.Rollback();
                            throw ex;
                        }
                    }

                    var viewtag = db.View_TagitemSugesstion.Where(w => tagitem_index.Contains(w.TagItem_Index)).ToList();
                    var GetViewProductCategory = utils.SendDataApi<List<View_ProductCategoryViewModel>>(new AppSettingConfig().GetUrl("GetViewProductCategory"), new { }.sJson());

                    //join tag and ProductCategory 
                    var datatag = (from v in viewtag
                                   join g in GetViewProductCategory on v.Product_Index equals g.product_Index
                                   select new View_TagitemSugesstionViewModel
                                   {
                                       tagItem_Index = v.TagItem_Index,
                                       tag_Index = v.Tag_Index,
                                       tag_No = v.Tag_No,
                                       process_Index = v.Process_Index,
                                       product_Index = v.Product_Index,
                                       product_Id = v.Product_Id,
                                       product_Name = v.Product_Name,
                                       product_SecondName = v.Product_SecondName,
                                       product_ThirdName = v.Product_ThirdName,
                                       product_Lot = v.Product_Lot,
                                       itemStatus_Index = v.ItemStatus_Index,
                                       itemStatus_Id = v.ItemStatus_Id,
                                       itemStatus_Name = v.ItemStatus_Name,
                                       suggest_Location_Index = v.Suggest_Location_Index,
                                       suggest_Location_Id = v.Suggest_Location_Id,
                                       suggest_Location_Name = v.Suggest_Location_Name,
                                       qty = v.Qty,
                                       ratio = v.Ratio,
                                       totalQty = v.TotalQty,
                                       productConversion_Index = v.ProductConversion_Index,
                                       productConversion_Id = v.ProductConversion_Id,
                                       productConversion_Name = v.ProductConversion_Name,
                                       weight = v.Weight,
                                       volume = v.Volume,
                                       mfg_Date = v.MFG_Date,
                                       exp_Date = v.EXP_Date,
                                       tagRef_No1 = v.TagRef_No1,
                                       tagRef_No2 = v.TagRef_No2,
                                       tagRef_No3 = v.TagRef_No3,
                                       tagRef_No4 = v.TagRef_No4,
                                       tagRef_No5 = v.TagRef_No5,
                                       tag_Status = v.Tag_Status,
                                       tagitem_UDF1 = v.Tagitem_UDF1,
                                       tagitem_UDF2 = v.Tagitem_UDF2,
                                       tagitem_UDF3 = v.Tagitem_UDF3,
                                       tagitem_UDF4 = v.Tagitem_UDF4,
                                       tagitem_UDF5 = v.Tagitem_UDF5,
                                       tagitem_UserAssign = v.Tagitem_UserAssign,
                                       goodsReceiveItem_Index = v.GoodsReceiveItem_Index,
                                       lineNum = v.LineNum,
                                       qtyPlan = v.QtyPlan,
                                       pallet_Index = v.Pallet_Index,
                                       unitWeight = v.UnitWeight,
                                       unitWidth = v.UnitWidth,
                                       unitLength = v.UnitLength,
                                       unitHeight = v.UnitHeight,
                                       unitVolume = v.UnitVolume,
                                       unitPrice = v.UnitPrice,
                                       price = v.Price,
                                       goodsReceiveItem_DocumentRef_No1 = v.GoodsReceiveItem_DocumentRef_No1,
                                       goodsReceiveItem_DocumentRef_No2 = v.GoodsReceiveItem_DocumentRef_No2,
                                       goodsReceiveItem_DocumentRef_No3 = v.GoodsReceiveItem_DocumentRef_No3,
                                       goodsReceiveItem_DocumentRef_No4 = v.GoodsReceiveItem_DocumentRef_No4,
                                       goodsReceiveItem_DocumentRef_No5 = v.GoodsReceiveItem_DocumentRef_No5,
                                       goodsReceiveItem_Document_Status = v.GoodsReceiveItem_Document_Status,
                                       goodsReceiveItem_UDF1 = v.GoodsReceiveItem_UDF1,
                                       goodsReceiveItem_UDF2 = v.GoodsReceiveItem_UDF2,
                                       goodsReceiveItem_UDF3 = v.GoodsReceiveItem_UDF3,
                                       goodsReceiveItem_UDF4 = v.GoodsReceiveItem_UDF4,
                                       goodsReceiveItem_UDF5 = v.GoodsReceiveItem_UDF5,
                                       ref_Process_Index = v.Ref_Process_Index,
                                       ref_Document_No = v.Ref_Document_No,
                                       ref_Document_LineNum = v.Ref_Document_LineNum,
                                       ref_Document_Index = v.Ref_Document_Index,
                                       ref_DocumentItem_Index = v.Ref_DocumentItem_Index,
                                       goodsReceive_Remark = v.GoodsReceive_Remark,
                                       goodsReceive_DockDoor = v.GoodsReceive_DockDoor,
                                       goodsReceive_Index = v.GoodsReceive_Index,
                                       owner_Index = v.Owner_Index,
                                       owner_Id = v.Owner_Id,
                                       owner_Name = v.Owner_Name,
                                       documentType_Index = v.DocumentType_Index,
                                       documentType_Id = v.DocumentType_Id,
                                       documentType_Name = v.DocumentType_Name,
                                       goodsReceive_No = v.GoodsReceive_No,
                                       goodsReceive_Date = v.GoodsReceive_Date,
                                       goodsReceive_DocumentRef_No1 = v.GoodsReceive_DocumentRef_No1,
                                       goodsReceive_DocumentRef_No2 = v.GoodsReceive_DocumentRef_No2,
                                       goodsReceive_DocumentRef_No3 = v.GoodsReceive_DocumentRef_No3,
                                       goodsReceive_DocumentRef_No4 = v.GoodsReceive_DocumentRef_No4,
                                       goodsReceive_DocumentRef_No5 = v.GoodsReceive_DocumentRef_No5,
                                       goodsReceive_Document_Status = v.GoodsReceive_Document_Status,
                                       document_Remark = v.Document_Remark,
                                       goodsReceive_UDF1 = v.GoodsReceive_UDF1,
                                       goodsReceive_UDF2 = v.GoodsReceive_UDF2,
                                       goodsReceive_UDF3 = v.GoodsReceive_UDF3,
                                       goodsReceive_UDF4 = v.GoodsReceive_UDF4,
                                       goodsReceive_UDF5 = v.GoodsReceive_UDF5,
                                       documentPriority_Status = v.DocumentPriority_Status,
                                       putaway_Status = v.Putaway_Status,
                                       warehouse_Index = v.Warehouse_Index,
                                       warehouse_Id = v.Warehouse_Id,
                                       warehouse_Name = v.Warehouse_Name,
                                       warehouse_Index_To = v.Warehouse_Index_To,
                                       warehouse_Id_To = v.Warehouse_Id_To,
                                       warehouse_Name_To = v.Warehouse_Name_To,
                                       dockDoor_Index = v.DockDoor_Index,
                                       dockDoor_Id = v.DockDoor_Id,
                                       dockDoor_Name = v.DockDoor_Name,
                                       vehicleType_Index = v.VehicleType_Index,
                                       vehicleType_Id = v.VehicleType_Id,
                                       vehicleType_Name = v.VehicleType_Name,
                                       containerType_Index = v.ContainerType_Index,
                                       containerType_Id = v.ContainerType_Id,
                                       containerType_Name = v.ContainerType_Name,
                                       goodsReceive_UserAssign = v.GoodsReceive_UserAssign,
                                       invoice_No = v.Invoice_No,
                                       vendor_Index = v.Vendor_Index,
                                       vendor_Id = v.Vendor_Id,
                                       vendor_Name = v.Vendor_Name,
                                       whOwner_Index = v.WHOwner_Index,
                                       whOwner_Id = v.WHOwner_Id,
                                       whOwner_Name = v.WHOwner_Name,
                                       productCategory_Index = g.productCategory_Index,
                                       productCategory_Id = g.productCategory_Id,
                                       productCategory_Name = g.productCategory_Name,
                                       productSubType_Index = g.productSubType_Index,
                                       productType_Index = g.productType_Index
                                   }).AsQueryable();

                    //binbalance
                    var GetViewBinbalance = utils.SendDataApi<List<GetViewBinbalanceViewModel>>(new AppSettingConfig().GetUrl("GetViewBinbalance"), new { }.sJson()).AsQueryable();

                    //LocationZoneputaway

                    //GetRuleputaway
                    var GetRuleputaway = utils.SendDataApi<List<RuleputawayViewModel>>(new AppSettingConfig().GetUrl("GetRuleputaway"), new { }.sJson());
                    //SugesstionPutaway
                    var GetSugesstionPutaway = utils.SendDataApi<List<View_SugesstionPutaway>>(new AppSettingConfig().GetUrl("GetSugesstionPutaway"), new { }.sJson());
                    GetRuleputaway = GetRuleputaway.OrderBy(o => o.Ruleputaway_Seq).ToList();
                    foreach (var Ruleputaway in GetRuleputaway)
                    {
                        var SugesstionPutaway = GetSugesstionPutaway.Where(c => c.Ruleputaway_Index == Ruleputaway.Ruleputaway_Index).ToList();
                        foreach (var g in SugesstionPutaway)
                        {
                            if (g.RuleputawayConditionOperator == "IN")
                            {
                                var param = Expression.Parameter(typeof(GetViewBinbalanceViewModel), "x");
                                var predicate = Expression.Lambda<Func<GetViewBinbalanceViewModel, bool>>(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param, g.RuleputawayConditionField_Name)
                                    ), param);

                                GetViewBinbalance = GetViewBinbalance.Where(predicate);

                                var param2 = Expression.Parameter(typeof(View_TagitemSugesstionViewModel), "x");
                                var predicate2 = Expression.Lambda<Func<View_TagitemSugesstionViewModel, bool>>(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param2, g.RuleputawayConditionField_Name)
                                    ), param2);

                                datatag = datatag.Where(predicate2);

                            }
                            else if (g.RuleputawayConditionOperator == "NOT IN")
                            {

                                var param = Expression.Parameter(typeof(GetViewBinbalanceViewModel), "x");
                                var predicate = Expression.Lambda<Func<GetViewBinbalanceViewModel, bool>>(
                                    Expression.Not(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param, g.RuleputawayConditionField_Name)
                                    )), param);

                                GetViewBinbalance = GetViewBinbalance.Where(predicate);

                                var param2 = Expression.Parameter(typeof(View_TagitemSugesstionViewModel), "x");
                                var predicate2 = Expression.Lambda<Func<View_TagitemSugesstionViewModel, bool>>(
                                    Expression.Not(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param2, g.RuleputawayConditionField_Name)
                                    )), param2);

                                datatag = datatag.Where(predicate2);

                            }
                        }

                        var GetLocationZoneputaway = utils.SendDataApi<List<View_LocationZoneputaway>>(new AppSettingConfig().GetUrl("GetLocationZoneputaway"), new { }.sJson()).ToList();

                        var locationZonePutaway = GetLocationZoneputaway.Where(c => GetSugesstionPutaway.Select(s => s.Zoneputaway_Index).ToList().Contains(c.zoneputaway_Index)).ToList();

                        var tagitem = datatag.ToList();
                        var binbalance = GetViewBinbalance.Where(c => locationZonePutaway.Select(s => s.location_Index).ToList().Contains(c.location_Index)).ToList();

                        if (tagitem.Count == 0)
                        {
                            continue;
                        }

                        bool chkupdate = false;
                        foreach (var t in tagitem)
                        {
                            chkupdate = false;
                            foreach (var b in binbalance)
                            {
                                if (!chkupdate)
                                {
                                    var remain = (b.binBalance_QtyBal - b.binBalance_QtyReserve);


                                    var checklocation = db.wm_TagItem.Where(c => c.Tag_Status == 0 && c.Suggest_Location_Name == b.location_Name);

                                    var sumQtyTagitem = checklocation.Sum(s => s.TotalQty);

                                    if (remain > sumQtyTagitem && remain > t.totalQty)
                                    {
                                        var tagItem = db.wm_TagItem.FirstOrDefault(c => c.TagItem_Index == t.tagItem_Index && c.Tag_Status == -2);

                                        if (tagItem != null)
                                        {

                                            var LocationViewModel = new { location_Name = b.location_Name };
                                            var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LocationViewModel.sJson());

                                            var DataLocation = GetLocation.FirstOrDefault();

                                            if (GetLocation.Count > 0)
                                            {
                                                tagItem.Suggest_Location_Index = DataLocation.location_Index;
                                                tagItem.Suggest_Location_Id = DataLocation.location_Id;
                                                tagItem.Suggest_Location_Name = DataLocation.location_Name;
                                                tagItem.Update_By = data.update_By;
                                                tagItem.Update_Date = DateTime.Now;
                                                tagItem.Tag_Status = 0;
                                                result += t.tag_No + ":" + DataLocation.location_Name + " Success,";
                                                chkupdate = true;
                                                var transaction = db.Database.BeginTransaction();
                                                try
                                                {
                                                    db.SaveChanges();
                                                    transaction.Commit();
                                                }

                                                catch (Exception ex)
                                                {
                                                    msglog = State + " ex Rollback " + ex.Message.ToString();
                                                    olog.logging("CreatePackItem", msglog);
                                                    transaction.Rollback();
                                                    throw ex;
                                                }
                                            }
                                        }
                                    }
                                    else if (checklocation.Count() > 0)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                var updatestatustagitem2 = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && w.Tag_Status == -2).ToList();
                foreach (var u in updatestatustagitem2)
                {
                    u.Tag_Status = 0;
                }
                if (updatestatustagitem2.Count() > 0)
                {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exx)
                    {
                        msglog = State + " ex Rollback " + exx.Message.ToString();
                        olog.logging("CreatePackItem", msglog);
                        transaction.Rollback();
                        throw exx;
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                var updatestatustagitem = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && w.Tag_Status == -2).ToList();
                foreach (var u in updatestatustagitem)
                {
                    u.Tag_Status = 0;
                }
                if (updatestatustagitem.Count() > 0)
                {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exx)
                    {
                        msglog = State + " ex Rollback " + exx.Message.ToString();
                        olog.logging("CreatePackItem", msglog);
                        transaction.Rollback();
                        throw exx;
                    }
                }
                throw;
            }
        }

        public string SugesstionLocation(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            string result = "แนะนำสำเร็จ,";
            var tagitem_index = new List<Guid?>();
            int? tag_status = 0;
            try
            {
                olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "Start SugesstionLocation" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                using (var context = new GRDbContext())
                {
                    if (data.listLPNItemViewModel.Count == 0)
                    {
                        result = "กรุณาเลือกเลขที่พาเลท";
                        return result;
                    }

                    tagitem_index = data.listLPNItemViewModel.Where(c => c.tagItem_Index != null).Select(s => s.tagItem_Index).ToList();

                    List<int?> status = new List<int?> { 0, 1 };
                    var updatestatustagitem = new List<WM_TagItem>();
                    if (tagitem_index.Count() > 0)
                    {
                        updatestatustagitem = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && status.Contains(w.Tag_Status)).ToList();
                    }
                    else
                    {
                        updatestatustagitem = db.wm_TagItem.Where(w => w.Tag_Index == data.listLPNItemViewModel.FirstOrDefault().tag_Index && status.Contains(w.Tag_Status)).ToList();
                    }

                    if (updatestatustagitem.Count() == 0)
                    {
                        return "Can't find Tag";
                    }

                    foreach (var u in updatestatustagitem)
                    {
                        tag_status = u.Tag_Status;
                        u.Tag_Status = -2;
                    }
                    if (updatestatustagitem.Count() > 0)
                    {
                        var transaction = db.Database.BeginTransaction();
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception ex)
                        {
                            msglog = State + " ex Rollback " + ex.Message.ToString();
                            olog.logging("SugesstionLocation", msglog);
                            transaction.Rollback();
                            throw ex;
                        }
                    }

                    var viewtag = db.View_TagitemSugesstion.Where(w => updatestatustagitem.Select(s => s.TagItem_Index).Contains(w.TagItem_Index)).AsQueryable();
                    var GetViewProductCategory = utils.SendDataApi<List<View_ProductCategoryViewModel>>(new AppSettingConfig().GetUrl("GetViewProductCategory"), new { }.sJson());

                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetViewProductCategory" + GetViewProductCategory.Count());

                    //join tag and ProductCategory 
                    var datatag = (from v in viewtag
                                   join g in GetViewProductCategory on v.Product_Index equals g.product_Index
                                   select new View_TagitemSugesstionViewModel
                                   {
                                       tagItem_Index = v.TagItem_Index,
                                       tag_Index = v.Tag_Index,
                                       tag_No = v.Tag_No,
                                       process_Index = v.Process_Index,
                                       product_Index = v.Product_Index,
                                       product_Id = v.Product_Id,
                                       product_Name = v.Product_Name,
                                       product_SecondName = v.Product_SecondName,
                                       product_ThirdName = v.Product_ThirdName,
                                       product_Lot = v.Product_Lot,
                                       itemStatus_Index = v.ItemStatus_Index,
                                       itemStatus_Id = v.ItemStatus_Id,
                                       itemStatus_Name = v.ItemStatus_Name,
                                       suggest_Location_Index = v.Suggest_Location_Index,
                                       suggest_Location_Id = v.Suggest_Location_Id,
                                       suggest_Location_Name = v.Suggest_Location_Name,
                                       qty = v.Qty,
                                       ratio = v.Ratio,
                                       totalQty = v.TotalQty,
                                       productConversion_Index = v.ProductConversion_Index,
                                       productConversion_Id = v.ProductConversion_Id,
                                       productConversion_Name = v.ProductConversion_Name,
                                       weight = v.Weight,
                                       volume = v.Volume,
                                       mfg_Date = v.MFG_Date,
                                       exp_Date = v.EXP_Date,
                                       tagRef_No1 = v.TagRef_No1,
                                       tagRef_No2 = v.TagRef_No2,
                                       tagRef_No3 = v.TagRef_No3,
                                       tagRef_No4 = v.TagRef_No4,
                                       tagRef_No5 = v.TagRef_No5,
                                       tag_Status = v.Tag_Status,
                                       tagitem_UDF1 = v.Tagitem_UDF1,
                                       tagitem_UDF2 = v.Tagitem_UDF2,
                                       tagitem_UDF3 = v.Tagitem_UDF3,
                                       tagitem_UDF4 = v.Tagitem_UDF4,
                                       tagitem_UDF5 = v.Tagitem_UDF5,
                                       tagitem_UserAssign = v.Tagitem_UserAssign,
                                       goodsReceiveItem_Index = v.GoodsReceiveItem_Index,
                                       lineNum = v.LineNum,
                                       qtyPlan = v.QtyPlan,
                                       pallet_Index = v.Pallet_Index,
                                       unitWeight = v.UnitWeight,
                                       unitWidth = v.UnitWidth,
                                       unitLength = v.UnitLength,
                                       unitHeight = v.UnitHeight,
                                       unitVolume = v.UnitVolume,
                                       unitPrice = v.UnitPrice,
                                       price = v.Price,
                                       goodsReceiveItem_DocumentRef_No1 = v.GoodsReceiveItem_DocumentRef_No1,
                                       goodsReceiveItem_DocumentRef_No2 = v.GoodsReceiveItem_DocumentRef_No2,
                                       goodsReceiveItem_DocumentRef_No3 = v.GoodsReceiveItem_DocumentRef_No3,
                                       goodsReceiveItem_DocumentRef_No4 = v.GoodsReceiveItem_DocumentRef_No4,
                                       goodsReceiveItem_DocumentRef_No5 = v.GoodsReceiveItem_DocumentRef_No5,
                                       goodsReceiveItem_Document_Status = v.GoodsReceiveItem_Document_Status,
                                       goodsReceiveItem_UDF1 = v.GoodsReceiveItem_UDF1,
                                       goodsReceiveItem_UDF2 = v.GoodsReceiveItem_UDF2,
                                       goodsReceiveItem_UDF3 = v.GoodsReceiveItem_UDF3,
                                       goodsReceiveItem_UDF4 = v.GoodsReceiveItem_UDF4,
                                       goodsReceiveItem_UDF5 = v.GoodsReceiveItem_UDF5,
                                       ref_Process_Index = v.Ref_Process_Index,
                                       ref_Document_No = v.Ref_Document_No,
                                       ref_Document_LineNum = v.Ref_Document_LineNum,
                                       ref_Document_Index = v.Ref_Document_Index,
                                       ref_DocumentItem_Index = v.Ref_DocumentItem_Index,
                                       goodsReceive_Remark = v.GoodsReceive_Remark,
                                       goodsReceive_DockDoor = v.GoodsReceive_DockDoor,
                                       goodsReceive_Index = v.GoodsReceive_Index,
                                       owner_Index = v.Owner_Index,
                                       owner_Id = v.Owner_Id,
                                       owner_Name = v.Owner_Name,
                                       documentType_Index = v.DocumentType_Index,
                                       documentType_Id = v.DocumentType_Id,
                                       documentType_Name = v.DocumentType_Name,
                                       goodsReceive_No = v.GoodsReceive_No,
                                       goodsReceive_Date = v.GoodsReceive_Date,
                                       goodsReceive_DocumentRef_No1 = v.GoodsReceive_DocumentRef_No1,
                                       goodsReceive_DocumentRef_No2 = v.GoodsReceive_DocumentRef_No2,
                                       goodsReceive_DocumentRef_No3 = v.GoodsReceive_DocumentRef_No3,
                                       goodsReceive_DocumentRef_No4 = v.GoodsReceive_DocumentRef_No4,
                                       goodsReceive_DocumentRef_No5 = v.GoodsReceive_DocumentRef_No5,
                                       goodsReceive_Document_Status = v.GoodsReceive_Document_Status,
                                       document_Remark = v.Document_Remark,
                                       goodsReceive_UDF1 = v.GoodsReceive_UDF1,
                                       goodsReceive_UDF2 = v.GoodsReceive_UDF2,
                                       goodsReceive_UDF3 = v.GoodsReceive_UDF3,
                                       goodsReceive_UDF4 = v.GoodsReceive_UDF4,
                                       goodsReceive_UDF5 = v.GoodsReceive_UDF5,
                                       documentPriority_Status = v.DocumentPriority_Status,
                                       putaway_Status = v.Putaway_Status,
                                       warehouse_Index = v.Warehouse_Index,
                                       warehouse_Id = v.Warehouse_Id,
                                       warehouse_Name = v.Warehouse_Name,
                                       warehouse_Index_To = v.Warehouse_Index_To,
                                       warehouse_Id_To = v.Warehouse_Id_To,
                                       warehouse_Name_To = v.Warehouse_Name_To,
                                       dockDoor_Index = v.DockDoor_Index,
                                       dockDoor_Id = v.DockDoor_Id,
                                       dockDoor_Name = v.DockDoor_Name,
                                       vehicleType_Index = v.VehicleType_Index,
                                       vehicleType_Id = v.VehicleType_Id,
                                       vehicleType_Name = v.VehicleType_Name,
                                       containerType_Index = v.ContainerType_Index,
                                       containerType_Id = v.ContainerType_Id,
                                       containerType_Name = v.ContainerType_Name,
                                       goodsReceive_UserAssign = v.GoodsReceive_UserAssign,
                                       invoice_No = v.Invoice_No,
                                       vendor_Index = v.Vendor_Index,
                                       vendor_Id = v.Vendor_Id,
                                       vendor_Name = v.Vendor_Name,
                                       whOwner_Index = v.WHOwner_Index,
                                       whOwner_Id = v.WHOwner_Id,
                                       whOwner_Name = v.WHOwner_Name,
                                       productCategory_Index = g.productCategory_Index,
                                       productCategory_Id = g.productCategory_Id,
                                       productCategory_Name = g.productCategory_Name,
                                       productSubType_Index = g.productSubType_Index,
                                       productType_Index = g.productType_Index
                                   });
                    #region Get Data Master
                    //binbalance
                    
                    //    var GetViewBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinbalanceGreaterThanZero"), new { }.sJson());
                    var GetViewBinbalance = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinbalanceGreaterThanZeroV2"), new { }.sJson());
                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetViewBinbalance" + GetViewBinbalance.Count());

                    //Location 
                    var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), new { }.sJson());
                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetLocation" + GetLocation.Count());
                    //LocationZoneputaway
                    var GetLocationZoneputaway = utils.SendDataApi<List<View_LocationZoneputaway>>(new AppSettingConfig().GetUrl("GetLocationZoneputaway"), new { }.sJson()).ToList();
                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetLocationZoneputaway" + GetLocationZoneputaway.Count());

                    //GetRuleputaway
                    var GetRuleputaway = utils.SendDataApi<List<RuleputawayViewModel>>(new AppSettingConfig().GetUrl("GetRuleputaway"), new { }.sJson());
                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetRuleputaway" + GetRuleputaway.Count());

                    //SugesstionPutaway
                    var GetSugesstionPutaway = utils.SendDataApi<List<View_SugesstionPutaway>>(new AppSettingConfig().GetUrl("GetSugesstionPutaway"), new { }.sJson());
                    olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetSugesstionPutaway" + GetSugesstionPutaway.Count());
                    #endregion


                    GetRuleputaway = GetRuleputaway.OrderBy(o => o.Ruleputaway_Seq).ToList();

                    foreach (var Ruleputaway in GetRuleputaway)
                    {
                        var SugesstionPutaway = GetSugesstionPutaway.Where(c => c.Ruleputaway_Index == Ruleputaway.Ruleputaway_Index).ToList();
                        foreach (var g in SugesstionPutaway)
                        {
                            var d = datatag.Where(c => c.tag_Status == -2).AsQueryable();
                            if (d.Count() == 0)
                            {
                                break;
                            }
                            if (g.RuleputawayConditionOperator == "IN")
                            {
                                var param2 = Expression.Parameter(typeof(View_TagitemSugesstionViewModel), "x");
                                var predicate2 = Expression.Lambda<Func<View_TagitemSugesstionViewModel, bool>>(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param2, g.RuleputawayConditionField_Name)
                                    ), param2);

                                d = d.Where(predicate2);

                            }
                            else if (g.RuleputawayConditionOperator == "NOT IN")
                            {
                                var param2 = Expression.Parameter(typeof(View_TagitemSugesstionViewModel), "x");
                                var predicate2 = Expression.Lambda<Func<View_TagitemSugesstionViewModel, bool>>(
                                    Expression.Not(
                                    Expression.Call(
                                        Expression.Constant(g.RuleputawayCondition_Param),
                                        "Contains", null, Expression.PropertyOrField(param2, g.RuleputawayConditionField_Name)
                                    )), param2);

                                d = d.Where(predicate2);

                            }

                            if (d.Count() == 0)
                            {
                                continue;
                            }

                            bool chkupdate = false;
                            foreach (var t in d)
                            {
                                chkupdate = false;

                                var viewbinbalance = GetViewBinbalance.AsQueryable();
                                var locationZonePutaway = GetLocationZoneputaway.Where(c => c.zoneputaway_Index == g.Zoneputaway_Index).OrderBy(c => c.PutAway_Seq).ThenBy(c=> c.location_Name).ToList();

                                var location = GetLocation.Where(c => locationZonePutaway.Select(s => s.location_Index).Contains(c.location_Index) && c.isActive == 1 && c.blockPut != 1).ToList();


                                var databinbalance = viewbinbalance.Where(c => locationZonePutaway.Select(s => s.location_Index).Contains(c.Location_Index) && c.BinBalance_QtyBal > 0 && c.Location_Id != "GT-Bulk-01")
                                    .GroupBy(gg => new
                                    {
                                        gg.Location_Index,
                                        gg.Location_Name,
                                        gg.Tag_No
                                    })
                                    .Select(ss => new
                                    {
                                        ss.Key.Location_Index,
                                        ss.Key.Location_Name,
                                        tag_index = ss.Count(),
                                        binBalance_QtyBal = ss.Sum(sum => sum.BinBalance_QtyBal),
                                        binBalance_WeightBal = ss.Sum(sum => sum.BinBalance_WeightBal),
                                        binBalance_VolumeBal = ss.Sum(sum => sum.BinBalance_VolumeBal),
                                    }).ToList();

                                var binbalance = (from LC in location
                                                  join BB in databinbalance on LC.location_Index equals BB?.Location_Index into gj
                                                  from L in gj.DefaultIfEmpty()
                                                  where ((LC?.max_Qty ?? 0) - (L?.binBalance_QtyBal ?? 0)) > 0
                                                  && ((LC?.max_Volume ?? 0) - (L?.binBalance_VolumeBal ?? 0)) > 0
                                                  && ((LC?.max_Weight ?? 0) - (L?.binBalance_WeightBal ?? 0)) > 0
                                                  && ((LC?.max_Pallet ?? 0) - (L?.tag_index ?? 0)) > 0
                                                  select new
                                                  {
                                                      Location_Index = LC.location_Index,
                                                      Location_Id = LC.location_Id,
                                                      Location_Name = LC.location_Name,
                                                      Max_Qty = LC.max_Qty,
                                                      Max_Weight = LC.max_Weight,
                                                      Max_Volume = LC.max_Volume,
                                                      Max_Pallet = LC.max_Pallet,
                                                      PutAway_Seq = LC.putAway_Seq,
                                                      BBLocation_index = L?.Location_Index,
                                                      BBLocation_Name = L?.Location_Name,
                                                      tag_index = L?.tag_index,
                                                      binBalance_QtyBal = L?.binBalance_QtyBal,
                                                      binBalance_WeightBal = L?.binBalance_WeightBal,
                                                      binBalance_VolumeBal = L?.binBalance_VolumeBal,
                                                  }
                                                  ).GroupBy(gb => new
                                                  {
                                                      Location_Index = gb.Location_Index,
                                                      Location_Id = gb.Location_Id,
                                                      Location_Name = gb.Location_Name,
                                                      Max_Qty = gb.Max_Qty,
                                                      Max_Weight = gb.Max_Weight,
                                                      Max_Volume = gb.Max_Volume,
                                                      Max_Pallet = gb.Max_Pallet,
                                                      PutAway_Seq = gb.PutAway_Seq,
                                                  }).Select(sel => new
                                                  {
                                                      sel.Key.Location_Index,
                                                      sel.Key.Location_Id,
                                                      sel.Key.Location_Name,
                                                      sel.Key.Max_Qty,
                                                      sel.Key.Max_Weight,
                                                      sel.Key.Max_Volume,
                                                      sel.Key.Max_Pallet,
                                                      sel.Key.PutAway_Seq,
                                                      tag_index = sel.Sum(sum => sum.tag_index),
                                                      binBalance_QtyBal = sel.Sum(sum => sum.binBalance_QtyBal),
                                                      binBalance_WeightBal = sel.Sum(sum => sum.binBalance_WeightBal),
                                                      binBalance_VolumeBal = sel.Sum(sum => sum.binBalance_VolumeBal),
                                                  }).OrderBy(o => o.PutAway_Seq).ToList();

                                var Location_Index = new Guid?();
                                var Location_Id = "";
                                var Location_Name = "";
                                var chkmsg = false;
                                foreach (var b in binbalance.OrderBy(c => c.PutAway_Seq))
                                {
                                    List<int?> notstatus = new List<int?> { -1, 2 };
                                    var checktag = db.wm_TagItem.Where(c => c.Suggest_Location_Index == b.Location_Index && !notstatus.Contains(c.Tag_Status)).ToList();
                                    var sumQtyTag = checktag.Sum(s => s.TotalQty);

                                    msglog = "LOC : " + b.Location_Name;
                                    olog.logging("SugesstionLocation", msglog);


                                    if ((!chkupdate || Location_Index == null)
                                        && (b.Max_Qty - (b.binBalance_QtyBal ?? 0)) >= sumQtyTag
                                        //&& (b.Max_Pallet - (b.tag_index ?? 0)) >= checktag.Count())
                                        && b.Max_Pallet - (b.tag_index ?? 0) > 0
                                        && b.Max_Pallet - checktag.Count() > 0)
                                    {
                                     

                                        var tagItem = db.wm_TagItem.FirstOrDefault(c => c.TagItem_Index == t.tagItem_Index && c.Tag_Status == -2);

                                        if (tagitem_index.Count() == 0 && b.Location_Index == tagItem.Suggest_Location_Index)
                                        {

                                            msglog = "tagitem_index.Count() == 0";
                                            olog.logging("SugesstionLocation", msglog);



                                            Location_Index = null;
                                            Location_Id = null;
                                            Location_Name = null;
                                        }
                                        else
                                        {
                                            chkmsg = true;
                                            Location_Index = b.Location_Index;
                                            Location_Id = b.Location_Id;
                                            Location_Name = b.Location_Name;
                                        }
                                        olog.DataLogLines("SugesstionLocation", "SugesstionLocation", "GetSugesstionPutaway" + GetSugesstionPutaway.Count());
                                        if (tagItem != null)
                                        {

                                            msglog = "tagitem_index.Count() == 0";
                                            olog.logging("SugesstionLocation", msglog);


                                         
                                            //tagItem.Tag_Status = tagitem_index.Count() == 0 ? 0 : 1;
                                            if (binbalance.Count() > 0 && Location_Index == null)
                                            {
                                            }
                                            else
                                            {
                                
                                                var checkLocationResult = CheckSuggestionLocation(Location_Index.ToString());
                                                if (checkLocationResult != "")
                                                {
                                                    result += checkLocationResult + ",";
                                                }
                                            }
                                            chkupdate = true;

                                            var Temp_Index = Guid.NewGuid();
                                            string cmd1 = "";
                                            string cmd2 = "";
                                            string cmd3 = "";
                                            cmd1 = "  INSERT INTO  WMSDB_Master..tmp_SuggestLocationPutAway ";
                                            cmd1 += "            ([Temp_Index]						 ";
                                            cmd1 += "            ,[PalletID]						 ";
                                            cmd1 += "            ,[Location_Index]					 ";
                                            cmd1 += "            ,[Location_Id]						 ";
                                            cmd1 += "            ,[Location_Name]					 ";
                                            cmd1 += "            ,[Create_By]						 ";
                                            cmd1 += "            ,[Create_Date]						 ";
                                            cmd1 += "            ,[IsComplete]						 ";
                                            cmd1 += "     ) VALUES	(								 ";
                                            cmd1 += "             '" + Temp_Index.ToString () + "' ";  //<Temp_Index, uniqueidentifier,>	 
                                            cmd1 += "            ,'" + tagItem.Tag_No + "' ";  //<PalletID, nvarchar(50),>			 
                                            cmd1 += "            ,'" + Location_Index.ToString() + "' ";  //<Location_Index, uniqueidentifier,>
                                            cmd1 += "            ,'" + Location_Id + "' ";  //<Location_Id, nvarchar(50),>		 
                                            cmd1 += "            ,'" + Location_Name + "' ";  //<Location_Name, nvarchar(200),>	 
                                            cmd1 += "            ,'" + data.update_By + "' ";  //<Create_By, nvarchar(200),>		 
                                            cmd1 += "            , getdate() ";  //<Create_Date, datetime,>			 
                                            cmd1 += "            , 0 )";  //<IsComplete, int,>)	
                                         

                                            olog.logging("SugesstionLocation", "ExecuteSqlCommand cmd1 : " + cmd1);


                                            cmd2 += "    UPdate  WMSDB_Master..tmp_SuggestLocationPutAway   set ";
                                            cmd2 += "  [Location_Name] = [Location_Name] ";
                                            cmd2 += " WHERE Location_Index = '" + Location_Index.ToString() + "'";

                                            olog.logging("SugesstionLocation", "ExecuteSqlCommand cmd2 : " + cmd2);

                                            int rowAff1 = 0;
                                            int rowAff2 = 0;
                                            int rowAff3 = 0;
                                            var transtmp = db.Database.BeginTransaction();
                                            try
                                            {
                                                //var strSQL1 = new SqlParameter("@strSQL", cmd1 );
                                                //rowAff1 = db.Database.ExecuteSqlCommand("EXEC sp_RunExec @strSQL", strSQL1);
                                                rowAff1 = db.Database.ExecuteSqlCommand(cmd1);
                       
                                                transtmp.Commit();
                                            }
                                            catch (Exception exxx)
                                            {
                                                msglog = State + " exxx Rollback " + exxx.Message.ToString();
                                                olog.logging("SugesstionLocation", msglog);
                                                transtmp.Rollback();
                                                // throw exxx;

                                            }

                                            var transtmp2 = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
                                            try
                                            {
                                                //var strSQL2 = new SqlParameter("@strSQL", cmd2);
                                                //rowAff1 = db.Database.ExecuteSqlCommand("EXEC sp_RunExec @strSQL", strSQL2);

                                                rowAff2 = db.Database.ExecuteSqlCommand(cmd2);
                                                transtmp2.Commit();
                                            }

                                            catch (Exception exxx2)
                                            {

                                                msglog = State + " exxx2 Rollback " + exxx2.Message.ToString();
                                                olog.logging("SugesstionLocation", msglog);
                                                transtmp2.Rollback();
                                      

                                            }
                                            if (rowAff1 != rowAff2 || rowAff1 == 0 || rowAff2 == 0)
                                            {



                                                cmd3  = "  delete  WMSDB_Master..tmp_SuggestLocationPutAway    ";
                                                cmd3 += " WHERE Temp_Index = '" + Temp_Index.ToString() + "'";

                                                olog.logging("SugesstionLocation", "ExecuteSqlCommand cmd3 : " + cmd3);

                                                var transtmp3 = db.Database.BeginTransaction( );
                                                try
                                                {


                                                    rowAff3 = db.Database.ExecuteSqlCommand(cmd3);
                                                    transtmp3.Commit();
                                                }

                                                catch (Exception exxx3)
                                                {

                                                    msglog = State + " exxx3 Rollback " + exxx3.Message.ToString();
                                                    olog.logging("SugesstionLocation", msglog);
                                                    transtmp3.Rollback();


                                                }

                                                msglog = "continue tmp_SuggestLocationPutAway = 0";
                                                olog.logging("SugesstionLocation", msglog);

                                                continue;
                                            }
                                            else
                                            {
                                              
                                                //xxxx
                                            }

                                            // Update Location 

                                            tagItem.Suggest_Location_Index = Location_Index;
                                            tagItem.Suggest_Location_Id = Location_Id;
                                            tagItem.Suggest_Location_Name = Location_Name;
                                            tagItem.Update_By = data.update_By;
                                            tagItem.Update_Date = DateTime.Now;

                                            result += "เลขที่พาเลท : " + t.tag_No + ",ตำแหน่งจัดเก็บ : " + Location_Name + ",";

                                            var transaction = db.Database.BeginTransaction();
                                            try
                                            {

                                                msglog = "db.SaveChanges()  TAG_NO : " + tagItem.Tag_No.ToString() + " Location : " + tagItem.Suggest_Location_Name.ToString();
                                                olog.logging("SugesstionLocation", msglog);

                                                tagItem.Tag_Status = tag_status;
                                                db.SaveChanges();
                                                transaction.Commit();
                                            }

                                            catch (Exception ex)
                                            {
                                                msglog = State + " ex Rollback " + ex.Message.ToString();
                                                olog.logging("SugesstionLocation", msglog);
                                                transaction.Rollback();
                                                throw ex;
                                            }

                                        }
                                        else
                                        {

                                            result += t.tag_No + ": ไม่สามารถเพื่มตำแหน่งจัดเก็บได้,";
                                        }
                                        break;

                                    }
                                    else if ((!chkupdate || Location_Index == null)
                                        && (b.Max_Qty - (b.binBalance_QtyBal ?? 0)) >= sumQtyTag
                                        //&& (b.Max_Pallet - (b.tag_index ?? 0)) >= checktag.Count())
                                        && b.Max_Pallet - (b.tag_index ?? 0) > 0)
                                    {

                                        msglog = "continue";
                                        olog.logging("SugesstionLocation", msglog);

                                        continue;
                                    }
                                    else
                                    {
                                        result += chkmsg ? "" : t.tag_No + ": ตำแหน่งที่จัดเก็บ สินค้า เต็ม,";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                var updatestatustagitem2 = new List<WM_TagItem>();
                if (tagitem_index.Count() > 0)
                {
                    updatestatustagitem2 = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && w.Tag_Status == -2).ToList();
                    foreach (var u in updatestatustagitem2)
                    {
                        u.Tag_Status = tag_status;
                    }
                }
                else
                {
                    updatestatustagitem2 = db.wm_TagItem.Where(w => w.Tag_Index == data.listLPNItemViewModel.FirstOrDefault().tag_Index && w.Tag_Status == -2).ToList();
                    foreach (var u in updatestatustagitem2)
                    {
                        u.Tag_Status = tag_status;
                    }
                }

                if (updatestatustagitem2.Count() > 0)
                {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exx)
                    {
                        msglog = State + " exx Rollback " + exx.Message.ToString();
                        olog.logging("SugesstionLocation", msglog);
                        transaction.Rollback();
                        throw exx;
                    }
                }

                result = result == "แนะนำสำเร็จ," ? "สินค้ายังไม่ได้ตั้งค่าแนะนำตำแหน่งจัดเก็บ" : result;

                //var checkResult = CheckSuggestionLocation(data);
                //result += " , " + checkResult;
                return result;

            }
            catch (Exception ex)
            {

                var updatestatustagitem = new List<WM_TagItem>();

                if (tagitem_index.Count() > 0)
                {
                    updatestatustagitem = db.wm_TagItem.Where(w => tagitem_index.Contains(w.TagItem_Index) && w.Tag_Status == -2).ToList();
                    foreach (var u in updatestatustagitem)
                    {
                        u.Tag_Status = tag_status;
                    }
                }
                else
                {
                    updatestatustagitem = db.wm_TagItem.Where(w => w.Tag_Index == data.listLPNItemViewModel.FirstOrDefault().tag_Index && w.Tag_Status == -2).ToList();
                    foreach (var u in updatestatustagitem)
                    {
                        u.Tag_Status = tag_status;
                    }
                }


                if (updatestatustagitem.Count() > 0)
                {
                    var transaction = db.Database.BeginTransaction();
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();


                        msglog = State + " ex Rollback " + ex.Message.ToString();
                        olog.logging("SugesstionLocation", msglog);


                    }

                    catch (Exception exx)
                    {
                        msglog = State + " exx Rollback " + exx.Message.ToString();
                        olog.logging("SugesstionLocation", msglog);
                        transaction.Rollback();
                        throw exx;
                    }
                }
                throw;
            }
        }

        public string CheckSuggestionLocation(string location_Index)
        {
            try
            {
                //var location = db.View_Location_Alert_MSG.Where(c => c.Location_Index == Guid.Parse(location_Index)).FirstOrDefault();
                return "";
                //if (location != null)
                //{
                //    if (!string.IsNullOrEmpty(location.MSG))
                //    {
                //        return location.MSG;
                //    }
                //    else
                //    {
                //        return "";
                //    }
                //}
                //else
                //{
                //    return "ไม่สามารถเช็คตำแหน่งได้";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region ReportTagPutaway
        public string ReportTagPutaway(LPNItemViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var resTagItem = new List<ReportTagPutawayViewModel>();
                //var queryGr = db.View_RPT_PrintOutTag.FirstOrDefault(c => c.GoodsReceive_Index == data.listLPNItemViewModel[0].goodsReceive_Index);

                //string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
                //var time = DateTime.Now.ToString("HH:mm");

                //string date = queryGr.GoodsReceive_Date.toString();
                //string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                //System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                var ProductBarcode = "";
                var RefNo1 = "";
                var RefNo2 = "";
                var RefNo3 = "";
                var ShortNameUnit = "";

                var datatag = (from tag in data.listLPNItemViewModel
                               join gri in db.IM_GoodsReceiveItem on tag.goodsReceiveItem_Index equals gri.GoodsReceiveItem_Index
                               select new
                               {
                                   tag.productConversion_Index,
                                   tag.productConversion_Name,
                                   tag.product_Id,
                                   tag.product_Name,
                                   tag.tag_No,
                                   tag.qty,
                                   tag.suggest_Location_Id,
                                   gri.Create_Date,
                                   gri.GoodsReceive_Index,
                                   gri.GoodsReceiveItem_Index
                               }
                               ).ToList();

                var ProductConversionModel = new ProductConversionViewModel();
                var resultProductConversion = new List<ProductConversionViewModel>();
                resultProductConversion = utils.SendDataApi<List<ProductConversionViewModel>>(new AppSettingConfig().GetUrl("dropdownProductConversionV2"), ProductConversionModel.sJson());


                foreach (var item in datatag.OrderBy(o => o.tag_No))
                {

                    var GoodsReceive_Index = new SqlParameter("@GoodsReceive_Index", item.GoodsReceive_Index);
                    var GoodsReceiveItem_Index = new SqlParameter("@GoodsReceiveItem_Index", item.GoodsReceiveItem_Index);

                    var queryGr = db.View_RPT_PrintOutTag.FromSql("sp_RPT_PrintOutTag @GoodsReceive_Index, @GoodsReceiveItem_Index", GoodsReceive_Index, GoodsReceiveItem_Index).FirstOrDefault();

                    if (queryGr != null)
                    {
                        db.Entry(queryGr).State = EntityState.Detached;

                    }
                    //                        _oo0oo_
                    //                       o8888888o
                    //                       88" . "88
                    //                       (| -_- |)
                    //                       0\  =  /0
                    //                     ___/`---'\___
                    //                   .' \|     |// '.
                    //                  / \|||  :  |||// \
                    //                 / _||||| -:- |||||- \
                    //                |   | \\  -  /// |   |
                    //                | \_|  ''\---/''  |_/ |
                    //                \  .-\__  '-'  ___/-. /
                    //              ___'. .'  /--.--\  `. .'___
                    //           ."" '<  `.___\_<|>_/___.' >' "".
                    //          | | :  `- \`.;`\ _ /`;.`/ - ` : | |
                    //          \  \ `_.   \_ __\ /__ _/   .-` /  /
                    //      =====`-.____`.___ \_____/___.-`___.-'=====
                    //                        `=---='

                    //var queryGr = new View_RPT_PrintOutTag();
                    //queryGr = queryGr_V.FirstOrDefault();
                    //var queryGr = db.View_RPT_PrintOutTag.FirstOrDefault(c => c.GoodsReceive_Index == item.GoodsReceive_Index && c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index);

                    string DatePrint = DateTime.Now.ToString("dd/MM/yyyy", culture);
                    var time = DateTime.Now.ToString("HH:mm");

                    string date = queryGr.GoodsReceive_Date.toString();
                    string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                    var resultItem = new ReportTagPutawayViewModel();
                    if (item.productConversion_Index != null)
                    {
                        
                        if (resultProductConversion.Count > 0 && resultProductConversion != null)
                        {
                            var DataProductConversion = resultProductConversion.Find(c => c.productConversion_Index == item.productConversion_Index);
                            if (DataProductConversion != null)
                            {
                                RefNo1 = DataProductConversion.ref_No1;
                                RefNo2 = DataProductConversion.ref_No2;
                                RefNo3 = DataProductConversion.ref_No3;
                                ShortNameUnit = (DataProductConversion.sale_UNIT == 0 && DataProductConversion.in_UNIT == 0) ? "" : (DataProductConversion.sale_UNIT == 1) ? "SU" : "IU" ;
                            }
                        }

                        //var ProductConversionBarcodeModel = new ProductConversionBarcodeViewModel();
                        //var resultProductConversionBarcode = new List<ProductConversionBarcodeViewModel>();
                        //resultProductConversionBarcode = utils.SendDataApi<List<ProductConversionBarcodeViewModel>>(new AppSettingConfig().GetUrl("dropdownProductBarcode"), ProductConversionBarcodeModel.sJson());
                        //if (resultProductConversionBarcode.Count > 0 && resultProductConversionBarcode != null)
                        //{
                        //    var DataProductConversionBarcode = resultProductConversionBarcode.Find(c => c.productConversion_Index == item.productConversion_Index);
                        //    if (DataProductConversionBarcode != null)
                        //    {
                        //        ProductBarcode = DataProductConversionBarcode.productConversionBarcode;
                        //    }

                        //}
                    }
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(item.tag_No.Replace("I", ""), QRCodeGenerator.ECCLevel.Q); //queryGr.PalletID
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);


                    resultItem.warehouse_Id = "R_" + queryGr.Warehouse_Id;
                    resultItem.location_Id = item.suggest_Location_Id;
                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.planGoodsReceive_No = queryGr.Ref_Document_No;
                    resultItem.goodsReceive_No = queryGr.GoodsReceive_No;
                    resultItem.suggest_Location_Name = item.suggest_Location_Id;
                    resultItem.owner_Name = queryGr.Owner_Name;
                    resultItem.product_Id = item.product_Id;
                    resultItem.product_Name = item.product_Name;
                    resultItem.qty = Convert.ToDecimal(item.qty);
                    resultItem.tag_NoBarcode = Convert.ToBase64String(BitmapToBytes(qrCodeImage));
                    //resultItem.tag_NoBarcode = new NetBarcode.Barcode(item.tag_No, NetBarcode.Type.Code128B).GetBase64Image();
                    resultItem.productConversion_Name = item.productConversion_Name;
                    resultItem.tag_No = item.tag_No;
                    resultItem.ref_no1 = RefNo1;
                    resultItem.ref_no2 = RefNo2;
                    resultItem.ref_no3 = RefNo3;
                    resultItem.date_Print = DatePrint + " " + time;
                    resultItem.shortNameUnit = ShortNameUnit;

                    //resultItem.receiverDate = GRDate;
                    //resultItem.supplier = queryGr.Supplier;


                    resultItem.productionLineNo = queryGr.ProductionLineNo;
                    resultItem.palletID = item.tag_No.Replace("I", ""); //queryGr.PalletID;
                    resultItem.sku = queryGr.SKU;
                    resultItem.skuBarcode = queryGr.SKUBarcode;
                    resultItem.isLastCarton = queryGr.IsLastCarton;
                    resultItem.description = queryGr.Description;
                    resultItem.mainType = queryGr.MainType;
                    //resultItem.quantityInCRT = queryGr.QuantityInCRT;
                    //resultItem.quantityInPC = queryGr.QuantityInPC;
                    resultItem.mfgDate = (queryGr.MFGDate != null) ? queryGr.MFGDate : " - ";
                    resultItem.expDate = (queryGr.EXPDate != null) ? queryGr.EXPDate : " - ";
                    resultItem.lotNo = (!string.IsNullOrEmpty(queryGr.LotNo)) ? queryGr.LotNo : " - ";
                    resultItem.refDoc = queryGr.RefDoc;
                    resultItem.cartonPerPallet = queryGr.CartonPerPallet;
                    resultItem.ti = queryGr.Ti;
                    resultItem.hi = queryGr.Hi;
                    resultItem.valTiHi = queryGr.Ti * queryGr.Hi;
                    resultItem.receiverDate = GRDate;
                    resultItem.receiver = queryGr.Receiver;
                    resultItem.status = queryGr.Status;

                    string sapCreateDT = DateTime.ParseExact(queryGr.SapCreateDT.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.sapCreateDT = sapCreateDT;

                    string productionEndDT = DateTime.ParseExact(queryGr.ProductionEndDT.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.productionEndDT = productionEndDT;

                    resultItem.saleQty = queryGr.SaleQty;
                    resultItem.saleUnit = queryGr.SaleUnit;

                    string giBeforeDate = DateTime.ParseExact(queryGr.GiBeforeDate.toString().Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                    resultItem.giBeforeDate = giBeforeDate;

                    resultItem.batchNo = (!string.IsNullOrEmpty(queryGr.BatchNo)) ? queryGr.BatchNo : " - ";
                    resultItem.type = queryGr.Type;
                    resultItem.unitOnPallet = queryGr.UnitOnPallet;
                    resultItem.palletWT = queryGr.PalletWT;
                    resultItem.bu = queryGr.BU;
                    resultItem.supplier = queryGr.Supplier;
                    resultItem.remark = queryGr.Remark;
                    resultItem.qtyInUnit = queryGr.QtyInUnit;
                    resultItem.unitOfInUnit = queryGr.UnitOfInUnit;
                    resultItem.qtyPOUnit = queryGr.QtyPOUnit;
                    resultItem.unitOfPOUnit = queryGr.UnitOfPOUnit;

                    resTagItem.Add(resultItem);

                }
                resTagItem.ToList();



                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportTagPutaway\\ReportTagPutaway.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportTagPutaway\\ReportTagPutaway.rdlc";
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportTagPutaway");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", resTagItem);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region wm_TagItem
        public List<View_RPT_PrintOutTag> View_RPT_PrintOutTag(DocumentViewModel model)
        {
            try
            {
                var query = db.View_RPT_PrintOutTag.AsQueryable();

                var result = new List<View_RPT_PrintOutTag>();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.GoodsReceive_Index));
                }

                if (model.listDocumentViewModel.FirstOrDefault().document_No != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_No).Contains(c.Tag_No));
                }

                if (model.listDocumentViewModel.FirstOrDefault().documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItem_Index).Contains(c.GoodsReceiveItem_Index));
                }

                if (model.listDocumentViewModel.FirstOrDefault().ref_documentItem_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.documentItemLocation_Index).Contains(c.Location_Index));
                }


                var queryresult = query.ToList();

                foreach (var item in queryresult)
                {
                    var resultItem = new View_RPT_PrintOutTag();

                    resultItem.Row_Index = item.Row_Index;
                    resultItem.GoodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.GoodsReceive_No = item.GoodsReceive_No;
                    resultItem.GoodsReceive_Date = item.GoodsReceive_Date;
                    resultItem.Owner_Index = item.Owner_Index;
                    resultItem.Owner_Id = item.Owner_Id;
                    resultItem.Owner_Name = item.Owner_Name;
                    resultItem.Warehouse_Index = item.Warehouse_Index;
                    resultItem.Warehouse_Id = item.Warehouse_Id;
                    resultItem.Warehouse_Name = item.Warehouse_Name;
                    resultItem.Product_Index = item.Product_Index;
                    resultItem.Product_Id = item.Product_Id;
                    resultItem.Product_Name = item.Product_Name;
                    resultItem.ProductConversion_Index = item.ProductConversion_Index;
                    resultItem.ProductConversion_Id = item.ProductConversion_Id;
                    resultItem.ProductConversion_Name = item.ProductConversion_Name;
                    resultItem.Qty = item.Qty;
                    resultItem.Ref_Document_No = item.Ref_Document_No;
                    resultItem.Location_Index = item.Location_Index;
                    resultItem.Location_Id = item.Location_Id;
                    resultItem.Location_Name = item.Location_Name;
                    resultItem.Tag_No = item.Tag_No;

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
