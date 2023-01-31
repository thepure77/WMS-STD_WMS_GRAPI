using AspNetCore.Reporting;
using BomBusiness;
using Business.Library;
using Comone.Utils;
using DataAccess;
using DataAccessBinbalance;
using DataAccessBom;
using GRBusiness.AutoNumber;
using GRBusiness.ConfigModel;
using GRBusiness.GoodsReceiveImage;
using GRBusiness.Libs;
using GRBusiness.LPN;
using GRBusiness.PlanGoodsReceive;
using GRBusiness.Reports;
using GRDataAccess.Models;
using GRDataAccessBalance.Models;
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
using Microsoft.EntityFrameworkCore;
//using static GRBusiness.ConfigModel.PlanGoodsReceivePopupViewModel;
using PlanGRBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TransferBusiness.Transfer;
using static BomBusiness.BomViewModel;
using static GRBusiness.ConfigModel.ProductViewModel;
using static GRBusiness.GoodsReceive.GoodsReceiveDocViewModel;
using static GRBusiness.GoodsReceive.SearchGRModel;
using static GRBusiness.GoodsReceive.TagItemViewModel;
using static PlanGRBusiness.ViewModels.PlanGoodsReceivePopupViewModels;


namespace GRBusiness.GoodsReceive
{
    public class GoodsReceiveService
    {
        private GRDbContext db;
        private GRBinBalanceDbContext db2;
        private GRBomDbContext db3;


        public GoodsReceiveService()
        {
            db = new GRDbContext();
            db2 = new GRBinBalanceDbContext();
            db3 = new GRBomDbContext();


        }

        public GoodsReceiveService(GRDbContext db)
        {
            this.db = db;
            this.db2 = db2;

        }


        public List<GoodIssueViewModelItem> po_contrack(GoodsReceiveDocViewModel data)
        {
            try
            {
                List<GoodIssueViewModelItem> result_goodissue = new List<GoodIssueViewModelItem>();
                List<GoodIssueViewModelItem> result = new List<GoodIssueViewModelItem>();

               if (data.planGoodsIssue_Index != null)
                {
                    result = utils.GetDataApi<List<GoodIssueViewModelItem>>((new AppSettingConfig().GetUrl("PlanGIitem")), data.planGoodsIssue_Index.ToString());
                }
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        var filterModel = new { item.product_Index };
                        var result_product = utils.SendDataApi<List<Product_by_SubViewModel>>(new AppSettingConfig().GetUrl("GetProduct"), filterModel.sJson());
                        item.qty = item.totalQty.ToString();
                        item.ratio = 1;
                        item.totalQty = item.totalQty;
                        item.productConversion_Id = result_product[0].productConversion_Id;
                        item.productConversion_Index = result_product[0].productConversion_Index;
                        item.productConversion_Name = result_product[0].productConversion_Name;
                        result_goodissue.Add(item);
                    }
                }

                return result_goodissue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public actionResult CreateOrUpdate(GoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Boolean IsNew = false;
            Boolean IsNewLine = false;
            String GoodsReceiveNo = "";
            var actionResult = new actionResult();

            try
            {

                State = "IM_PlanGoodsReceive.Find";
                var userKey = db.IM_PlanGoodsReceive.Find(data.planGoodsReceive_Index);

                var itemDetail = new List<IM_GoodsReceiveItem>();

                var ItemStatusModel = new ItemStatusDocViewModel();
                var resultItemStatus = new List<ItemStatusDocViewModel>();

                ItemStatusModel.itemStatus_Index = new Guid("C043169D-1D73-421B-9E33-69C770DCC3B4");

                resultItemStatus = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), ItemStatusModel.sJson());
                
                
                State = "IM_GoodsReceive.Find";
                var isHaveGoodsReceive = db.IM_GoodsReceive.Find(data.goodsReceive_Index);

                #region Create

                if (isHaveGoodsReceive == null)
                {
                    IM_GoodsReceive itemHeader = new IM_GoodsReceive();

                    itemHeader.GoodsReceive_Index = Guid.NewGuid();

                    var filterModel = new DocumentTypeViewModel();
                    var result = new List<DocumentTypeViewModel>();

                    filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                    filterModel.documentType_Index = data.documentType_Index;
                    //GetConfig

                    State = "DropDownDocumentType";
                    result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());

                    DataTable resultDocumentType = CreateDataTable(result);

                    // Gen New Document No

                    var genDoc = new AutoNumberService();
                    string DocNo = "";
                    DateTime DocumentDate = (DateTime)data.goodsReceive_Date.toDate();
                    DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);

                    GoodsReceiveNo = DocNo;
                    State = "itemHeader";

                    var goodReceiveDate = data.goodsReceive_Date.toDate();
                    var document_status = 0;
                    var documentPriority_Status = 0;
                    var putaway_Status = 0;

                    itemHeader.GoodsReceive_No = DocNo;
                    itemHeader.Owner_Index = data.owner_Index.GetValueOrDefault();
                    itemHeader.Owner_Id = data.owner_Id;
                    itemHeader.Owner_Name = data.owner_Name;
                    itemHeader.DocumentType_Index = data.documentType_Index;
                    itemHeader.DocumentType_Id = data.documentType_Id;
                    itemHeader.DocumentType_Name = data.documentType_Name;
                    var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    itemHeader.GoodsReceive_Date = goodReceiveDate + time;
                    itemHeader.GoodsReceive_Due_Date = data.goodsReceive_Due_Date.toDate();
                    itemHeader.DocumentRef_No1 = data.planGoodsIssue_Index.GetValueOrDefault().ToString();
                    itemHeader.DocumentRef_No2 = data.documentRef_No2;
                    itemHeader.DocumentRef_No3 = data.documentRef_No3;
                    itemHeader.DocumentRef_No4 = data.documentRef_No4;
                    itemHeader.DocumentRef_No5 = data.documentRef_No5;
                    itemHeader.Document_Status = document_status;
                    itemHeader.UDF_1 = data.uDF_1;
                    itemHeader.UDF_2 = data.uDF_2;
                    itemHeader.UDF_3 = data.uDF_3;
                    itemHeader.UDF_4 = data.uDF_4;
                    itemHeader.UDF_5 = data.uDF_5;
                    itemHeader.DocumentPriority_Status = documentPriority_Status;
                    itemHeader.Document_Remark = data.document_Remark;
                    itemHeader.Warehouse_Index = data.warehouse_Index;
                    itemHeader.Warehouse_Id = data.warehouse_Id;
                    itemHeader.Warehouse_Name = data.warehouse_Name;
                    itemHeader.Warehouse_Index_To = data.warehouse_Index_To;
                    itemHeader.Warehouse_Id_To = data.warehouse_Id_To;
                    itemHeader.Warehouse_Name_To = data.warehouse_Name_To;
                    itemHeader.Putaway_Status = putaway_Status;
                    itemHeader.DockDoor_Index = data.dockDoor_Index;
                    itemHeader.DockDoor_Id = data.dockDoor_Id;
                    itemHeader.DockDoor_Name = data.dockDoor_Name;
                    itemHeader.VehicleType_Index = data.vehicleType_Index;
                    itemHeader.VehicleType_Id = data.vehicleType_Id;
                    itemHeader.VehicleType_Name = data.vehicleType_Name;
                    itemHeader.License_Name = data.license_Name;
                    itemHeader.Forwarder_Index = data.forwarder_Index;
                    itemHeader.Forwarder_Id = data.forwarder_Id;
                    itemHeader.Forwarder_Name = data.forwarder_Name;
                    itemHeader.ShipmentType_Index = data.shipmentType_Index;
                    itemHeader.ShipmentType_Id = data.shipmentType_Id;
                    itemHeader.ShipmentType_Name = data.shipmentType_Name;
                    itemHeader.CargoType_Index = data.cargoType_Index;
                    itemHeader.CargoType_Id = data.cargoType_Id;
                    itemHeader.CargoType_Name = data.cargoType_Name;
                    itemHeader.UnloadingType_Index = data.unloadingType_Index;
                    itemHeader.UnloadingType_Id = data.unloadingType_Id;
                    itemHeader.UnloadingType_Name = data.unloadingType_Name;
                    itemHeader.ContainerType_Index = data.containerType_Index;
                    itemHeader.ContainerType_Id = data.containerType_Id;
                    itemHeader.ContainerType_Name = data.containerType_Name;
                    itemHeader.Container_No1 = data.container_No1;
                    itemHeader.Container_No2 = data.container_No2;
                    itemHeader.Labur = data.labur;
                    itemHeader.Create_By = data.create_By;
                    itemHeader.Create_Date = DateTime.Now;
                    itemHeader.Checker_Name = data.checker_Name;
                    itemHeader.Driver_Name = data.driver_Name;
                    itemHeader.Vendor_Index = data.vendor_Index;
                    itemHeader.Vendor_Id = data.vendor_Id;
                    itemHeader.Vendor_Name = data.vendor_Name;
                    itemHeader.CostCenter_Index = data.costCenter_Index;
                    itemHeader.CostCenter_Id = data.costCenter_Id;
                    itemHeader.CostCenter_Name = data.costCenter_Name;
                    db.IM_GoodsReceive.Add(itemHeader);

                    //----Set Detail-----

                    State = "itemDetail";

                    int addNumber = 0;
                    int refDocLineNum = 0;
                    if (data.listGoodsReceiveItemViewModels.Count > 0)
                    {
                        foreach (var item in data.listGoodsReceiveItemViewModels)
                        {
                            addNumber++;

                            IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                            // Gen Index for line item
                            resultItem.GoodsReceiveItem_Index = Guid.NewGuid();
                            resultItem.GoodsReceive_Index = itemHeader.GoodsReceive_Index;
                            if (item.lineNum == null)
                            {
                                resultItem.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                resultItem.LineNum = item.lineNum;
                            }

                            resultItem.Product_Index = item.product_Index;
                            resultItem.Product_Id = item.product_Id;
                            resultItem.Product_Name = item.product_Name;
                            resultItem.Product_SecondName = item.product_SecondName;
                            resultItem.Product_ThirdName = item.product_ThirdName;
                            if (item.product_Lot != "")
                            {
                                resultItem.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                resultItem.Product_Lot = "";
                            }
                            if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                            {
                                resultItem.ItemStatus_Index = item.itemStatus_Index;
                            }
                            else
                            {
                                resultItem.ItemStatus_Index = resultItemStatus.FirstOrDefault().itemStatus_Index;
                            }

                            if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                            {
                                resultItem.ItemStatus_Id = item.itemStatus_Id;
                            }
                            else
                            {
                                resultItem.ItemStatus_Id = resultItemStatus.FirstOrDefault().itemStatus_Id;
                            }
                            if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                            {
                                resultItem.ItemStatus_Name = item.itemStatus_Name;
                            }
                            else
                            {
                                resultItem.ItemStatus_Name = resultItemStatus.FirstOrDefault().itemStatus_Name;
                            }

                            resultItem.Qty = item.qty;
                            resultItem.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = item.qty * item.ratio;
                                item.totalQty = totalqty;
                            }
                            resultItem.TotalQty = item.totalQty;
                            resultItem.ProductConversion_Index = item.productConversion_Index;
                            resultItem.ProductConversion_Id = item.productConversion_Id;
                            resultItem.ProductConversion_Name = item.productConversion_Name;
                            resultItem.MFG_Date = item.mFG_Date.toDate();
                            resultItem.EXP_Date = item.eXP_Date.toDate();

                            resultItem.WeightRatio = item.weightRatio;
                            resultItem.UnitWeight = item.weightUnit;
                            resultItem.Weight = item.qty * (item.weightUnit * item.weightRatio);
                            resultItem.Weight_Index = item.weight_Index;
                            resultItem.Weight_Id = item.weight_Id;
                            resultItem.Weight_Name = item.weight_Name;
                            resultItem.NetWeight = item.netWeight;

                            resultItem.GrsWeightRatio = item.grsWeightRatio;
                            resultItem.UnitGrsWeight = item.unitGrsWeight;
                            resultItem.GrsWeight = item.qty * (item.unitGrsWeight * item.grsWeightRatio);
                            resultItem.GrsWeight_Index = item.grsWeight_Index;
                            resultItem.GrsWeight_Id = item.grsWeight_Id;
                            resultItem.GrsWeight_Name = item.grsWeight_Name;

                            resultItem.WidthRatio = item.widthRatio;
                            resultItem.UnitWidth = item.unitWidth;
                            resultItem.Width = item.unitWidth * item.qty;
                            resultItem.Width_Index = item.width_Index;
                            resultItem.Width_Id = item.width_Id;
                            resultItem.Width_Name = item.width_Name;

                            resultItem.LengthRatio = item.lengthRatio;
                            resultItem.UnitLength = item.unitLength;
                            resultItem.Length = item.unitLength * item.qty;
                            resultItem.Length_Index = item.length_Index;
                            resultItem.Length_Id = item.length_Id;
                            resultItem.Length_Name = item.length_Name;

                            resultItem.HeightRatio = item.heightRatio;
                            resultItem.UnitHeight = item.unitHeight;
                            resultItem.Height = item.unitHeight * item.qty;
                            resultItem.Height_Index = item.height_Index;
                            resultItem.Height_Id = item.height_Id;
                            resultItem.Height_Name = item.height_Name;

                            var resultProcon = new List<ProductConversionViewModelDoc>();

                            var Procon = new ProductConversionViewModelDoc();

                            if (!string.IsNullOrEmpty(item.product_Index.ToString()))
                            {
                                Procon.product_Index = item.product_Index;
                            }
                            //GetConfig
                            resultProcon = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), Procon.sJson());

                            resultItem.UnitVolume = (resultItem.UnitWidth * resultItem.UnitLength * resultItem.UnitHeight) / resultProcon.FirstOrDefault().productConversion_VolumeRatio;
                            resultItem.Volume = resultItem.Qty * resultItem.UnitVolume;



                            var resultProduct = new List<ItemListViewModel>();

                            var product = new ItemListViewModel();

                            product.key = item.product_Id;

                            //GetConfig
                            resultProduct = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSKU"), product.sJson());



                            resultItem.UnitPrice = item.unitPrice;
                            resultItem.Price = item.unitPrice * item.qty;
                            resultItem.TotalPrice = resultItem.Price * resultItem.Qty;

                            resultItem.Currency_Index = item.currency_Index;
                            resultItem.Currency_Id = item.currency_Id;
                            resultItem.Currency_Name = item.currency_Name;

                            resultItem.Ref_Code1 = item.ref_Code1;
                            resultItem.Ref_Code2 = item.ref_Code2;
                            resultItem.Ref_Code3 = item.ref_Code3;
                            resultItem.Ref_Code4 = resultProduct.FirstOrDefault().value6;
                            resultItem.Ref_Code5 = item.ref_Code5;


                            resultItem.DocumentRef_No1 = item.documentRef_No1;
                            resultItem.DocumentRef_No2 = item.documentRef_No2;
                            resultItem.DocumentRef_No3 = item.documentRef_No3;
                            resultItem.DocumentRef_No4 = item.documentRef_No4;
                            resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = 0;
                            resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
                            resultItem.UDF_1 = item.uDF_1;
                            resultItem.UDF_2 = item.uDF_2;
                            resultItem.UDF_3 = item.uDF_3;
                            resultItem.UDF_4 = item.uDF_4;
                            resultItem.UDF_5 = item.uDF_5;
                            resultItem.Ref_Process_Index = item.ref_Process_Index;
                            resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                            resultItem.Ref_Document_Index = item.ref_Document_Index;
                            resultItem.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;
                            resultItem.Ref_Document_No = item.ref_Document_No;


                            resultItem.Invoice_No = item.invoice_No;
                            resultItem.Declaration_No = item.declaration_No;
                            resultItem.HS_Code = item.hS_Code;
                            resultItem.Conutry_of_Origin = item.conutry_of_Origin;
                            resultItem.Tax1 = item.tax1;
                            resultItem.Tax1_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax1_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax1_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Tax2 = item.tax2;
                            resultItem.Tax2_Currency_Index = item.tax2_Currency_Index;
                            resultItem.Tax2_Currency_Id = item.tax2_Currency_Id;
                            resultItem.Tax2_Currency_Name = item.tax2_Currency_Name;

                            resultItem.Tax3 = item.tax3;
                            resultItem.Tax3_Currency_Index = item.tax3_Currency_Index;
                            resultItem.Tax3_Currency_Id = item.tax3_Currency_Id;
                            resultItem.Tax3_Currency_Name = item.tax3_Currency_Name;

                            resultItem.Tax4 = item.tax4;
                            resultItem.Tax4_Currency_Index = item.tax4_Currency_Index;
                            resultItem.Tax4_Currency_Id = item.tax4_Currency_Id;
                            resultItem.Tax4_Currency_Name = item.tax4_Currency_Name;

                            resultItem.Tax5 = item.tax5;
                            resultItem.Tax5_Currency_Index = item.tax5_Currency_Index;
                            resultItem.Tax5_Currency_Id = item.tax5_Currency_Id;
                            resultItem.Tax5_Currency_Name = item.tax5_Currency_Name;

                            resultItem.Create_By = itemHeader.Create_By;
                            resultItem.Create_Date = DateTime.Now;

                            resultItem.ERP_Location = item.erp_Location;
                            db.IM_GoodsReceiveItem.Add(resultItem);


                            if (item.ref_Document_Index == null)
                            {
                                db.IM_GoodsReceiveItem.Add(resultItem);
                            }
                            else
                            {

                                if (data.documentType_Index == new Guid("a256b73d-2354-4187-b19b-d6301475e0ea"))
                                {

                                    var resultBom = new List<BomItemPopupViewModel>();


                                    var listBom = new List<DocumentViewModel> { new DocumentViewModel { document_Index = item.ref_Document_Index } };
                                    var bom = new DocumentViewModel();
                                    bom.listDocumentViewModel = listBom;

                                    resultBom = utils.SendDataApi<List<BomItemPopupViewModel>>(new AppSettingConfig().GetUrl("FindBomItem"), bom.sJson());

                                    var checkGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == item.ref_Document_Index && c.Document_Status != -1)
                                                   .GroupBy(c => new { c.Ref_Document_Index })
                                                   .Select(c => new { c.Key.Ref_Document_Index, SumQty = c.Sum(s => s.TotalQty) }).ToList();

                                    if (checkGR.Count > 0)
                                    {
                                        if (checkGR.FirstOrDefault().SumQty < resultBom.FirstOrDefault().totalQty)
                                        {
                                            db.IM_GoodsReceiveItem.Add(resultItem);

                                        }
                                        else
                                        {

                                            actionResult.goodsReceive_No = GoodsReceiveNo;
                                            actionResult.Message = false;
                                            return actionResult;
                                        }
                                    }
                                    else
                                    {
                                        db.IM_GoodsReceiveItem.Add(resultItem);
                                    }
                                }
                                else
                                {

                                    State = "sp_CheckSavePlanGR";

                                    var resultPlanGR = new List<PlanGoodsReceiveItemViewModel>();


                                    var listPlan = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = item.ref_DocumentItem_Index } };
                                    var plan = new DocumentViewModel();
                                    plan.listDocumentViewModel = listPlan;

                                    resultPlanGR = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGRItem"), plan.sJson());

                                    var checkGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_DocumentItem_Index == item.ref_DocumentItem_Index && c.Document_Status != -1)
                                                   .GroupBy(c => new { c.Ref_DocumentItem_Index })
                                                   .Select(c => new { c.Key.Ref_DocumentItem_Index, SumQty = c.Sum(s => s.TotalQty) }).ToList();

                                    if (checkGR.Count > 0)
                                    {
                                        if (checkGR.FirstOrDefault().SumQty < resultPlanGR.FirstOrDefault().totalQty)
                                        {
                                            db.IM_GoodsReceiveItem.Add(resultItem);

                                        }
                                        else
                                        {

                                            actionResult.goodsReceive_No = GoodsReceiveNo;
                                            actionResult.Message = false;
                                            return actionResult;
                                        }
                                    }
                                    else
                                    {
                                        db.IM_GoodsReceiveItem.Add(resultItem);
                                    }
                                }


                            }
                        }
                    }

                    if (data.result_goodissue != null) {
                        foreach (var item in data.result_goodissue)
                        {
                            var po_sub = new Po_subcontact {
                                Po_Index = Guid.NewGuid(),
                                GoodsReceive_Index = itemHeader.GoodsReceive_Index,
                                GoodsReceive_No = itemHeader.GoodsReceive_No,
                                GoodsIssueItem_Index = Guid.Parse(item.planGoodsIssueItem_Index),
                                GoodsIssue_Index = Guid.Parse(item.planGoodsIssue_Index),
                                GoodsIssue_No = item.goodsIssue_No,
                                Qty = item.qty,
                                Ratio = item.ratio,
                                TotalQty = item.qty * item.ratio,
                                Product_Id = item.product_Id,
                                Product_Index = Guid.Parse(item.product_Index),
                                Product_Name = item.product_Name,
                                ProductConversion_Index = Guid.Parse(item.productConversion_Index),
                                ProductConversion_Id = item.productConversion_Id,
                                ProductConversion_Name = item.productConversion_Name,
                                ERP_Location = item.ERP_location,
                                IsActive = 1,
                                IsDelete = 0,
                                IsDelete_Plant = 0,
                                IsSystem = 0,
                                Status_Id = 0,
                                Create_By = data.create_By,
                                Create_Date = DateTime.Now
                            };

                            db.Po_subcontact.Add(po_sub);
                        }
                       
                    }

                    //save image4
                    if (data.docfile != null)
                    {
                        if (data.docfile.Count > 0)
                        {
                            foreach (var d in data.docfile)
                            {
                                byte[] img = Convert.FromBase64String(d.base64);
                                var path = Directory.GetCurrentDirectory();
                                path += "\\" + "ImageFolder" + "\\";
                                if (!System.IO.Directory.Exists(path))
                                {
                                    System.IO.Directory.CreateDirectory(path);
                                }
                                System.IO.File.WriteAllBytes(new AppSettingConfig().GetUrl("configUploadImg") + d.name, img);

                                im_GoodsReceive_Image gri = new im_GoodsReceive_Image();

                                gri.GoodsReceiveImage_Index = Guid.NewGuid();
                                gri.GoodsReceive_Index = itemHeader.GoodsReceive_Index.sParse<Guid>();
                                gri.GoodsReceiveImage_path = new AppSettingConfig().GetUrl("configGetImg") + d.name.ToString();
                                gri.GoodsReceiveImage_type = d.type;
                                gri.Document_Status = 0;
                                gri.Create_By = data.create_By;
                                gri.Create_Date = DateTime.Now;
                                db.im_GoodsReceive_Image.Add(gri);
                            }
                        }
                    }
                }

                #endregion

                #region Update
                else
                {

                    State = "itemHeader";

                    isHaveGoodsReceive.GoodsReceive_Index = data.goodsReceive_Index;
                    isHaveGoodsReceive.GoodsReceive_No = data.goodsReceive_No;
                    isHaveGoodsReceive.Owner_Index = data.owner_Index.GetValueOrDefault();
                    isHaveGoodsReceive.Owner_Id = data.owner_Id;
                    isHaveGoodsReceive.Owner_Name = data.owner_Name;
                    isHaveGoodsReceive.DocumentType_Index = data.documentType_Index;
                    isHaveGoodsReceive.DocumentType_Id = data.documentType_Id;
                    isHaveGoodsReceive.DocumentType_Name = data.documentType_Name;
                    isHaveGoodsReceive.GoodsReceive_Date = data.goodsReceive_Date.toDate();
                    isHaveGoodsReceive.GoodsReceive_Due_Date = data.goodsReceive_Due_Date.toDate();
                    isHaveGoodsReceive.DocumentRef_No1 = data.planGoodsIssue_Index.GetValueOrDefault().ToString();
                    isHaveGoodsReceive.DocumentRef_No2 = data.documentRef_No2;
                    isHaveGoodsReceive.DocumentRef_No3 = data.documentRef_No3;
                    isHaveGoodsReceive.DocumentRef_No4 = data.documentRef_No4;
                    isHaveGoodsReceive.DocumentRef_No5 = data.documentRef_No5;
                    isHaveGoodsReceive.Document_Status = data.document_Status;
                    isHaveGoodsReceive.UDF_1 = data.uDF_1;
                    isHaveGoodsReceive.UDF_2 = data.uDF_2;
                    isHaveGoodsReceive.UDF_3 = data.uDF_3;
                    isHaveGoodsReceive.UDF_4 = data.uDF_4;
                    isHaveGoodsReceive.UDF_5 = data.uDF_5;
                    isHaveGoodsReceive.DocumentPriority_Status = data.documentPriority_Status;
                    isHaveGoodsReceive.Document_Remark = data.document_Remark;
                    isHaveGoodsReceive.Warehouse_Index = data.warehouse_Index;
                    isHaveGoodsReceive.Warehouse_Id = data.warehouse_Id;
                    isHaveGoodsReceive.Warehouse_Name = data.warehouse_Name;
                    isHaveGoodsReceive.Warehouse_Index_To = data.warehouse_Index_To;
                    isHaveGoodsReceive.Warehouse_Id_To = data.warehouse_Id_To;
                    isHaveGoodsReceive.Warehouse_Name_To = data.warehouse_Name_To;
                    isHaveGoodsReceive.Putaway_Status = data.putaway_Status;
                    isHaveGoodsReceive.DockDoor_Index = data.dockDoor_Index;
                    isHaveGoodsReceive.DockDoor_Id = data.dockDoor_Id;
                    isHaveGoodsReceive.DockDoor_Name = data.dockDoor_Name;
                    isHaveGoodsReceive.VehicleType_Index = data.vehicleType_Index;
                    isHaveGoodsReceive.VehicleType_Id = data.vehicleType_Id;
                    isHaveGoodsReceive.VehicleType_Name = data.vehicleType_Name;
                    isHaveGoodsReceive.License_Name = data.license_Name;
                    isHaveGoodsReceive.Forwarder_Index = data.forwarder_Index;
                    isHaveGoodsReceive.Forwarder_Id = data.forwarder_Id;
                    isHaveGoodsReceive.Forwarder_Name = data.forwarder_Name;
                    isHaveGoodsReceive.ShipmentType_Index = data.shipmentType_Index;
                    isHaveGoodsReceive.ShipmentType_Id = data.shipmentType_Id;
                    isHaveGoodsReceive.ShipmentType_Name = data.shipmentType_Name;
                    isHaveGoodsReceive.CargoType_Index = data.cargoType_Index;
                    isHaveGoodsReceive.CargoType_Id = data.cargoType_Id;
                    isHaveGoodsReceive.CargoType_Name = data.cargoType_Name;
                    isHaveGoodsReceive.UnloadingType_Index = data.unloadingType_Index;
                    isHaveGoodsReceive.UnloadingType_Id = data.unloadingType_Id;
                    isHaveGoodsReceive.UnloadingType_Name = data.unloadingType_Name;
                    isHaveGoodsReceive.ContainerType_Index = data.containerType_Index;
                    isHaveGoodsReceive.ContainerType_Id = data.containerType_Id;
                    isHaveGoodsReceive.ContainerType_Name = data.containerType_Name;
                    isHaveGoodsReceive.Container_No1 = data.container_No1;
                    isHaveGoodsReceive.Container_No2 = data.container_No2;
                    isHaveGoodsReceive.Labur = data.labur;
                    isHaveGoodsReceive.Create_By = data.create_By;
                    isHaveGoodsReceive.Create_Date = DateTime.Now;
                    isHaveGoodsReceive.Checker_Name = data.checker_Name;
                    isHaveGoodsReceive.Driver_Name = data.driver_Name;
                    isHaveGoodsReceive.Vendor_Index = data.vendor_Index;
                    isHaveGoodsReceive.Vendor_Id = data.vendor_Id;
                    isHaveGoodsReceive.Vendor_Name = data.vendor_Name;
                    isHaveGoodsReceive.CostCenter_Index = data.costCenter_Index;
                    isHaveGoodsReceive.CostCenter_Id = data.costCenter_Id;
                    isHaveGoodsReceive.CostCenter_Name = data.costCenter_Name;

                    foreach (var item in data.listGoodsReceiveItemViewModels)
                    {

                        var resultProduct = new List<ItemListViewModel>();

                        var product = new ItemListViewModel();

                        product.key = item.product_Id;

                        //GetConfig
                        resultProduct = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSKU"), product.sJson());

                        var GoodsReceiveItemOld = db.IM_GoodsReceiveItem.Find(item.goodsReceiveItem_Index);

                        IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                        #region Update Line Item
                        if (GoodsReceiveItemOld != null)
                        {
                            int addNumber = 0;


                            addNumber++;

                            GoodsReceiveItemOld.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                            GoodsReceiveItemOld.GoodsReceive_Index = item.goodsReceive_Index;

                            var goodsReceive = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == GoodsReceiveItemOld.GoodsReceive_Index).FirstOrDefault();
                            GoodsReceiveNo = goodsReceive.GoodsReceive_No;

                            if (item.lineNum == null)
                            {
                                GoodsReceiveItemOld.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                GoodsReceiveItemOld.LineNum = item.lineNum;
                            }

                            GoodsReceiveItemOld.Product_Index = item.product_Index;
                            GoodsReceiveItemOld.Product_Id = item.product_Id;
                            GoodsReceiveItemOld.Product_Name = item.product_Name;
                            GoodsReceiveItemOld.Product_SecondName = item.product_SecondName;
                            GoodsReceiveItemOld.Product_ThirdName = item.product_ThirdName;
                            if (item.product_Lot != null)
                            {
                                GoodsReceiveItemOld.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                GoodsReceiveItemOld.Product_Lot = "";
                            }
                            GoodsReceiveItemOld.ItemStatus_Index = item.itemStatus_Index;
                            GoodsReceiveItemOld.ItemStatus_Id = item.itemStatus_Id;
                            GoodsReceiveItemOld.ItemStatus_Name = item.itemStatus_Name;

                            GoodsReceiveItemOld.ProductConversion_Index = item.productConversion_Index;
                            GoodsReceiveItemOld.ProductConversion_Id = item.productConversion_Id;
                            GoodsReceiveItemOld.ProductConversion_Name = item.productConversion_Name;
                            GoodsReceiveItemOld.MFG_Date = item.mFG_Date.toDate();
                            GoodsReceiveItemOld.EXP_Date = item.eXP_Date.toDate();

                            GoodsReceiveItemOld.Qty = item.qty;
                            GoodsReceiveItemOld.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = item.qty * item.ratio;
                                item.totalQty = totalqty;
                            }
                            GoodsReceiveItemOld.TotalQty = item.totalQty;
                            GoodsReceiveItemOld.ProductConversion_Index = item.productConversion_Index;
                            GoodsReceiveItemOld.ProductConversion_Id = item.productConversion_Id;
                            GoodsReceiveItemOld.ProductConversion_Name = item.productConversion_Name;
                            GoodsReceiveItemOld.MFG_Date = item.mFG_Date.toDate();
                            GoodsReceiveItemOld.EXP_Date = item.eXP_Date.toDate();

                            GoodsReceiveItemOld.WeightRatio = item.weightRatio;
                            GoodsReceiveItemOld.UnitWeight = item.weightUnit;
                            GoodsReceiveItemOld.Weight = item.qty * (item.weightUnit * item.weightRatio);
                            GoodsReceiveItemOld.Weight_Index = item.weight_Index;
                            GoodsReceiveItemOld.Weight_Id = item.weight_Id;
                            GoodsReceiveItemOld.Weight_Name = item.weight_Name;
                            GoodsReceiveItemOld.NetWeight = item.netWeight;

                            GoodsReceiveItemOld.GrsWeightRatio = item.grsWeightRatio;
                            GoodsReceiveItemOld.UnitGrsWeight = item.unitGrsWeight;
                            GoodsReceiveItemOld.GrsWeight = item.qty * (item.unitGrsWeight * item.grsWeightRatio);
                            GoodsReceiveItemOld.GrsWeight_Index = item.grsWeight_Index;
                            GoodsReceiveItemOld.GrsWeight_Id = item.grsWeight_Id;
                            GoodsReceiveItemOld.GrsWeight_Name = item.grsWeight_Name;

                            GoodsReceiveItemOld.WidthRatio = item.widthRatio;
                            GoodsReceiveItemOld.UnitWidth = item.unitWidth;
                            GoodsReceiveItemOld.Width = item.unitWidth * item.qty;
                            GoodsReceiveItemOld.Width_Index = item.width_Index;
                            GoodsReceiveItemOld.Width_Id = item.width_Id;
                            GoodsReceiveItemOld.Width_Name = item.width_Name;

                            GoodsReceiveItemOld.LengthRatio = item.lengthRatio;
                            GoodsReceiveItemOld.UnitLength = item.unitLength;
                            GoodsReceiveItemOld.Length = item.unitLength * item.qty;
                            GoodsReceiveItemOld.Length_Index = item.length_Index;
                            GoodsReceiveItemOld.Length_Id = item.length_Id;
                            GoodsReceiveItemOld.Length_Name = item.length_Name;

                            GoodsReceiveItemOld.HeightRatio = item.heightRatio;
                            GoodsReceiveItemOld.UnitHeight = item.unitHeight;
                            GoodsReceiveItemOld.Height = item.unitHeight * item.qty;
                            GoodsReceiveItemOld.Height_Index = item.height_Index;
                            GoodsReceiveItemOld.Height_Id = item.height_Id;
                            GoodsReceiveItemOld.Height_Name = item.height_Name;

                            GoodsReceiveItemOld.UnitVolume = item.volumeUnit;
                            GoodsReceiveItemOld.Volume = item.volume;

                            GoodsReceiveItemOld.UnitVolume = (resultItem.UnitWidth * resultItem.UnitLength * resultItem.UnitHeight) / item.volume_Ratio;
                            GoodsReceiveItemOld.Volume = resultItem.Qty * GoodsReceiveItemOld.UnitVolume;

                            GoodsReceiveItemOld.UnitPrice = item.unitPrice;
                            GoodsReceiveItemOld.Price = item.unitPrice * item.qty;
                            GoodsReceiveItemOld.TotalPrice = resultItem.Price * resultItem.Qty;

                            GoodsReceiveItemOld.Currency_Index = item.currency_Index;
                            GoodsReceiveItemOld.Currency_Id = item.currency_Id;
                            GoodsReceiveItemOld.Currency_Name = item.currency_Name;

                            GoodsReceiveItemOld.Ref_Code1 = item.ref_Code1;
                            GoodsReceiveItemOld.Ref_Code2 = item.ref_Code2;
                            GoodsReceiveItemOld.Ref_Code3 = item.ref_Code3;
                            GoodsReceiveItemOld.Ref_Code4 = resultProduct.FirstOrDefault().value6;
                            GoodsReceiveItemOld.Ref_Code5 = item.ref_Code5;

                            GoodsReceiveItemOld.DocumentRef_No1 = item.documentRef_No1;
                            GoodsReceiveItemOld.DocumentRef_No2 = item.documentRef_No2;
                            GoodsReceiveItemOld.DocumentRef_No3 = item.documentRef_No3;
                            GoodsReceiveItemOld.DocumentRef_No4 = item.documentRef_No4;
                            GoodsReceiveItemOld.DocumentRef_No5 = item.documentRef_No5;
                            GoodsReceiveItemOld.Document_Status = 0;
                            GoodsReceiveItemOld.UDF_1 = data.selected ? "X" : "";// item.udf_1;
                            GoodsReceiveItemOld.UDF_2 = item.uDF_2;
                            GoodsReceiveItemOld.UDF_3 = item.uDF_3;
                            GoodsReceiveItemOld.UDF_4 = item.uDF_4;
                            GoodsReceiveItemOld.UDF_5 = item.uDF_5;
                            GoodsReceiveItemOld.GoodsReceive_Remark = item.goodsReceive_Remark;
                            GoodsReceiveItemOld.Ref_Process_Index = item.ref_Process_Index;
                            GoodsReceiveItemOld.Ref_Document_LineNum = item.ref_Document_LineNum;
                            GoodsReceiveItemOld.Ref_Document_Index = item.ref_Document_Index;
                            GoodsReceiveItemOld.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;
                            GoodsReceiveItemOld.Ref_Document_No = item.ref_Document_No;


                            GoodsReceiveItemOld.Invoice_No = item.invoice_No;
                            GoodsReceiveItemOld.Declaration_No = item.declaration_No;
                            GoodsReceiveItemOld.HS_Code = item.hS_Code;
                            GoodsReceiveItemOld.Conutry_of_Origin = item.conutry_of_Origin;
                            GoodsReceiveItemOld.Tax1 = item.tax1;
                            GoodsReceiveItemOld.Tax1_Currency_Index = item.tax1_Currency_Index;
                            GoodsReceiveItemOld.Tax1_Currency_Id = item.tax1_Currency_Id;
                            GoodsReceiveItemOld.Tax1_Currency_Name = item.tax1_Currency_Name;

                            GoodsReceiveItemOld.Tax2 = item.tax1;
                            GoodsReceiveItemOld.Tax2_Currency_Index = item.tax1_Currency_Index;
                            GoodsReceiveItemOld.Tax2_Currency_Id = item.tax1_Currency_Id;
                            GoodsReceiveItemOld.Tax2_Currency_Name = item.tax1_Currency_Name;

                            GoodsReceiveItemOld.Tax3 = item.tax1;
                            GoodsReceiveItemOld.Tax3_Currency_Index = item.tax1_Currency_Index;
                            GoodsReceiveItemOld.Tax3_Currency_Id = item.tax1_Currency_Id;
                            GoodsReceiveItemOld.Tax3_Currency_Name = item.tax1_Currency_Name;

                            GoodsReceiveItemOld.Tax4 = item.tax1;
                            GoodsReceiveItemOld.Tax4_Currency_Index = item.tax1_Currency_Index;
                            GoodsReceiveItemOld.Tax4_Currency_Id = item.tax1_Currency_Id;
                            GoodsReceiveItemOld.Tax4_Currency_Name = item.tax1_Currency_Name;

                            GoodsReceiveItemOld.Tax5 = item.tax1;
                            GoodsReceiveItemOld.Tax5_Currency_Index = item.tax1_Currency_Index;
                            GoodsReceiveItemOld.Tax5_Currency_Id = item.tax1_Currency_Id;
                            GoodsReceiveItemOld.Tax5_Currency_Name = item.tax1_Currency_Name;

                            GoodsReceiveItemOld.Update_By = data.update_By;
                            GoodsReceiveItemOld.Update_Date = DateTime.Now;

                            GoodsReceiveItemOld.ERP_Location = item.erp_Location;

                        }

                        #endregion


                        #region Create NewLine Item
                        else
                        {

                            State = "Update ms_ItemStatus";


                            int refDocLineNum = 0;
                            int addNumber = 0;

                            addNumber++;


                            State = "Update resultItem";

                            // Gen Index for line item
                            if (item.goodsReceiveItem_Index.ToString() == "00000000-0000-0000-0000-000000000000" || item.isCopy == true)
                            {
                                item.goodsReceiveItem_Index = Guid.NewGuid();
                            }
                            resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;

                            // Index From Header
                            resultItem.GoodsReceive_Index = data.goodsReceive_Index;
                            if (item.lineNum == null)
                            {
                                resultItem.LineNum = addNumber.ToString();
                            }
                            else
                            {
                                resultItem.LineNum = item.lineNum;
                            }

                            resultItem.Product_Index = item.product_Index;
                            resultItem.Product_Id = item.product_Id;
                            resultItem.Product_Name = item.product_Name;
                            resultItem.Product_SecondName = item.product_SecondName;
                            resultItem.Product_ThirdName = item.product_ThirdName;
                            if (item.product_Lot != "")
                            {
                                resultItem.Product_Lot = item.product_Lot;
                            }
                            else
                            {
                                resultItem.Product_Lot = "";
                            }
                            if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                            {
                                resultItem.ItemStatus_Index = item.itemStatus_Index;
                            }
                            else
                            {
                                resultItem.ItemStatus_Index = resultItemStatus.FirstOrDefault().itemStatus_Index;
                            }
                            if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                            {
                                resultItem.ItemStatus_Id = item.itemStatus_Id;
                            }
                            else
                            {
                                resultItem.ItemStatus_Id = resultItemStatus.FirstOrDefault().itemStatus_Id;
                            }
                            if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                            {
                                resultItem.ItemStatus_Name = item.itemStatus_Name;
                            }
                            else
                            {
                                resultItem.ItemStatus_Name = resultItemStatus.FirstOrDefault().itemStatus_Name;
                            }

                            resultItem.UDF_1 = item.uDF_1;
                            resultItem.ProductConversion_Index = item.productConversion_Index;
                            resultItem.ProductConversion_Id = item.productConversion_Id;
                            resultItem.ProductConversion_Name = item.productConversion_Name;
                            resultItem.MFG_Date = item.mFG_Date.toDate();
                            resultItem.EXP_Date = item.eXP_Date.toDate();


                            if (item.ref_Document_No == null)
                            {
                                item.ref_Document_No = "";
                            }
                            resultItem.Ref_Document_No = item.ref_Document_No;
                            if (item.ref_Document_LineNum == null)
                            {
                                resultItem.Ref_Document_LineNum = refDocLineNum.ToString();
                            }
                            else
                            {
                                resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                            }

                            resultItem.Qty = item.qty;
                            resultItem.Ratio = item.ratio;
                            if (item.ratio != 0)
                            {
                                var totalqty = item.qty * item.ratio;
                                item.totalQty = totalqty;
                            }
                            resultItem.TotalQty = item.totalQty;
                            resultItem.ProductConversion_Index = item.productConversion_Index;
                            resultItem.ProductConversion_Id = item.productConversion_Id;
                            resultItem.ProductConversion_Name = item.productConversion_Name;
                            resultItem.MFG_Date = item.mFG_Date.toDate();
                            resultItem.EXP_Date = item.eXP_Date.toDate();

                            resultItem.WeightRatio = item.weightRatio;
                            resultItem.UnitWeight = item.weightUnit;
                            resultItem.Weight = item.qty * (item.weightUnit * item.weightRatio);
                            resultItem.Weight_Index = item.weight_Index;
                            resultItem.Weight_Id = item.weight_Id;
                            resultItem.Weight_Name = item.weight_Name;
                            resultItem.NetWeight = item.netWeight;

                            resultItem.GrsWeightRatio = item.grsWeightRatio;
                            resultItem.UnitGrsWeight = item.unitGrsWeight;
                            resultItem.GrsWeight = item.qty * (item.unitGrsWeight * item.grsWeightRatio);
                            resultItem.GrsWeight_Index = item.grsWeight_Index;
                            resultItem.GrsWeight_Id = item.grsWeight_Id;
                            resultItem.GrsWeight_Name = item.grsWeight_Name;

                            resultItem.WidthRatio = item.widthRatio;
                            resultItem.UnitWidth = item.unitWidth;
                            resultItem.Width = item.unitWidth * item.qty;
                            resultItem.Width_Index = item.width_Index;
                            resultItem.Width_Id = item.width_Id;
                            resultItem.Width_Name = item.width_Name;

                            resultItem.LengthRatio = item.lengthRatio;
                            resultItem.UnitLength = item.unitLength;
                            resultItem.Length = item.unitLength * item.qty;
                            resultItem.Length_Index = item.length_Index;
                            resultItem.Length_Id = item.length_Id;
                            resultItem.Length_Name = item.length_Name;

                            resultItem.HeightRatio = item.heightRatio;
                            resultItem.UnitHeight = item.unitHeight;
                            resultItem.Height = item.unitHeight * item.qty;
                            resultItem.Height_Index = item.height_Index;
                            resultItem.Height_Id = item.height_Id;
                            resultItem.Height_Name = item.height_Name;

                            resultItem.UnitVolume = (resultItem.UnitWidth * resultItem.UnitLength * resultItem.UnitHeight) / item.volume_Ratio;
                            resultItem.Volume = resultItem.Qty * resultItem.UnitVolume;


                            resultItem.UnitPrice = item.unitPrice;
                            resultItem.Price = item.unitPrice * item.qty;
                            resultItem.TotalPrice = resultItem.Price * resultItem.Qty;

                            resultItem.Currency_Index = item.currency_Index;
                            resultItem.Currency_Id = item.currency_Id;
                            resultItem.Currency_Name = item.currency_Name;

                            resultItem.Ref_Code1 = item.ref_Code1;
                            resultItem.Ref_Code2 = item.ref_Code2;
                            resultItem.Ref_Code3 = item.ref_Code3;
                            resultItem.Ref_Code4 = resultProduct.FirstOrDefault().value6;
                            resultItem.Ref_Code5 = item.ref_Code5;

                            resultItem.Ref_Document_Index = item.ref_Document_Index;
                            resultItem.Ref_DocumentItem_Index = item.ref_DocumentItem_Index;
                            resultItem.DocumentRef_No1 = item.documentRef_No1;
                            resultItem.DocumentRef_No2 = item.documentRef_No2;
                            resultItem.DocumentRef_No3 = item.documentRef_No3;
                            resultItem.DocumentRef_No4 = item.documentRef_No4;
                            resultItem.DocumentRef_No5 = item.documentRef_No5;
                            resultItem.Document_Status = data.document_Status;
                            resultItem.UDF_1 = data.selected ? "X" : "";// item.udf_1;
                            resultItem.UDF_2 = item.uDF_2;
                            resultItem.UDF_3 = item.uDF_3;
                            resultItem.UDF_4 = item.uDF_4;
                            resultItem.UDF_5 = item.uDF_5;
                            resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
                            resultItem.GoodsReceive_DockDoor = "";

                            resultItem.Invoice_No = item.invoice_No;
                            resultItem.Declaration_No = item.declaration_No;
                            resultItem.HS_Code = item.hS_Code;
                            resultItem.Conutry_of_Origin = item.conutry_of_Origin;
                            resultItem.Tax1 = item.tax1;
                            resultItem.Tax1_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax1_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax1_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Tax2 = item.tax1;
                            resultItem.Tax2_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax2_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax2_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Tax3 = item.tax1;
                            resultItem.Tax3_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax3_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax3_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Tax4 = item.tax1;
                            resultItem.Tax4_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax4_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax4_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Tax5 = item.tax1;
                            resultItem.Tax5_Currency_Index = item.tax1_Currency_Index;
                            resultItem.Tax5_Currency_Id = item.tax1_Currency_Id;
                            resultItem.Tax5_Currency_Name = item.tax1_Currency_Name;

                            resultItem.Create_By = data.create_By;
                            resultItem.Create_Date = DateTime.Now;

                            resultItem.ERP_Location = item.erp_Location;

                            db.IM_GoodsReceiveItem.Add(resultItem);

                        }

                        #endregion


                    }

                    if (data.result_goodissue.Count > 0)
                    {
                        foreach (var item in data.result_goodissue)
                        {
                            var GoodsReceiveItemOld = db.Po_subcontact.FirstOrDefault(c=> c.Po_Index == item.Po_Index);
                            if (GoodsReceiveItemOld != null)
                            {
                                GoodsReceiveItemOld.Qty = item.qty;
                                GoodsReceiveItemOld.TotalQty = item.qty * item.ratio;
                            }
                            else {
                                var po_sub = new Po_subcontact
                                {
                                    Po_Index = Guid.NewGuid(),
                                    GoodsReceive_Index = isHaveGoodsReceive.GoodsReceive_Index,
                                    GoodsReceive_No = isHaveGoodsReceive.GoodsReceive_No,
                                    GoodsIssueItem_Index = Guid.Parse(item.planGoodsIssueItem_Index),
                                    GoodsIssue_Index = Guid.Parse(item.planGoodsIssue_Index),
                                    GoodsIssue_No = item.goodsIssue_No,
                                    Qty = item.qty,
                                    Ratio = item.ratio,
                                    TotalQty = item.qty * item.ratio,
                                    Product_Id = item.product_Id,
                                    Product_Index = Guid.Parse(item.product_Index),
                                    Product_Name = item.product_Name,
                                    ProductConversion_Index = Guid.Parse(item.productConversion_Index),
                                    ProductConversion_Id = item.productConversion_Id,
                                    ProductConversion_Name = item.productConversion_Name,
                                    ERP_Location = item.ERP_location,
                                    IsActive = 1,
                                    IsDelete = 0,
                                    IsDelete_Plant = 0,
                                    IsSystem = 0,
                                    Status_Id = 0,
                                    Create_By = data.create_By,
                                    Create_Date = DateTime.Now
                                };
                                db.Po_subcontact.Add(po_sub);
                            }
                        }
                    }
                    

                    var deleteItem = db.IM_GoodsReceiveItem.Where(c => !data.listGoodsReceiveItemViewModels.Select(s => s.goodsReceiveItem_Index).Contains(c.GoodsReceiveItem_Index)
                                    && c.GoodsReceive_Index == isHaveGoodsReceive.GoodsReceive_Index).ToList();

                    foreach (var c in deleteItem)
                    {
                        var deleteGoodsReceiveItem = db.IM_GoodsReceiveItem.Find(c.GoodsReceiveItem_Index);

                        deleteGoodsReceiveItem.Document_Status = -1;
                        deleteGoodsReceiveItem.Update_By = data.update_By;
                        deleteGoodsReceiveItem.Update_Date = DateTime.Now;

                    }


                    //save image
                    if (data.docfile != null)
                    {
                        if (data.docfile.Count > 0)
                        {
                            var chkGoodsreceiveImage = db.im_GoodsReceive_Image.Where(c => c.GoodsReceive_Index == data.docfile.FirstOrDefault().goodsReceive_Index && c.Document_Status == 0).ToList();
                            if (chkGoodsreceiveImage.Count() > 0)
                            {
                                foreach (var updateGoodsreceiveImage in chkGoodsreceiveImage)
                                {
                                    updateGoodsreceiveImage.Document_Status = -1;
                                    updateGoodsreceiveImage.Cancel_By = data.create_By;
                                    updateGoodsreceiveImage.Cancel_Date = DateTime.Now;
                                }
                            }
                            foreach (var d in data.docfile)
                            {
                                var chkinsert = db.im_GoodsReceive_Image.Find(d.goodsReceiveImage_Index);

                                if (chkinsert == null)
                                {
                                    byte[] img = Convert.FromBase64String(d.base64);
                                    var path = Directory.GetCurrentDirectory();
                                    path += "\\" + "ImageFolder" + "\\";
                                    if (!System.IO.Directory.Exists(path))
                                    {
                                        System.IO.Directory.CreateDirectory(path);
                                    }
                                    System.IO.File.WriteAllBytes(new AppSettingConfig().GetUrl("configUploadImg") + d.name, img);

                                    im_GoodsReceive_Image gri = new im_GoodsReceive_Image();

                                    gri.GoodsReceiveImage_Index = Guid.NewGuid();
                                    gri.GoodsReceive_Index = isHaveGoodsReceive.GoodsReceive_Index.sParse<Guid>();
                                    gri.GoodsReceiveImage_path = new AppSettingConfig().GetUrl("configGetImg") + d.name.ToString();
                                    gri.GoodsReceiveImage_type = d.type;
                                    gri.Document_Status = 0;
                                    gri.Create_By = data.create_By;
                                    gri.Create_Date = DateTime.Now;
                                    db.im_GoodsReceive_Image.Add(gri);
                                }
                                else
                                {
                                    chkinsert.Document_Status = 0;
                                    chkinsert.Update_By = null;
                                    chkinsert.Update_Date = null;
                                }
                            }
                        }
                        else
                        {
                            var chkGoodsreceiveImage = db.im_GoodsReceive_Image.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status == 0).ToList();
                            if (chkGoodsreceiveImage.Count() > 0)
                            {
                                foreach (var updateGoodsreceiveImage in chkGoodsreceiveImage)
                                {
                                    updateGoodsreceiveImage.Document_Status = -1;
                                    updateGoodsreceiveImage.Update_By = data.create_By;
                                    updateGoodsreceiveImage.Update_Date = DateTime.Now;
                                }
                            }
                        }
                    }
                }

                #endregion

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    State = "SaveChanges";

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


                actionResult.goodsReceive_No = GoodsReceiveNo;
                actionResult.Message = true;
                return actionResult;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("SaveGR", msglog);
                throw ex;

            }
        }


        public string Delete(GoodsReceiveDocViewModel itemList)
        {
            try
            {

                var results = "";

                var query = db.IM_GoodsReceive.Find(itemList.goodsReceive_Index);
                if (query == null)
                {
                    return "ไม่พบข้อมูล";
                }
                else if (query.Document_Status == 4 || query.Document_Status == 3 || query.Document_Status == 2)
                {
                    var queryResult = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == query.GoodsReceive_Index).ToList();


                    if (query.Is_Amz != null)
                    {
                        return "ไม่สามารถลบได้";
                    }

                    var checkPutaway = queryResult.Where(s => s.Putaway_Status == 1).ToList();
                    if (checkPutaway.Count > 0)
                    {
                        return "ไม่สามารถลบได้";

                    }

                    #region cancel task
                    var queryResultTaskItem = db.im_TaskGRItem.Where(c => c.Ref_Document_Index == query.GoodsReceive_Index && c.Document_Status != -1).ToList();

                    foreach (var taskItem in queryResultTaskItem)
                    {
                        // cancel task
                        var updateTaskGR = db.im_TaskGR.Find(taskItem.TaskGR_Index);
                        if (updateTaskGR != null)
                        {
                            updateTaskGR.Document_Status = -1;
                            updateTaskGR.Update_By = itemList.cancel_By;
                            updateTaskGR.Update_Date = DateTime.Now;
                        }

                        var updateTaskGRItem = db.im_TaskGRItem.Find(taskItem.TaskGRItem_Index);
                        if (updateTaskGRItem != null)
                        {
                            updateTaskGRItem.Document_Status = -1;
                            updateTaskGRItem.Update_By = itemList.cancel_By;
                            updateTaskGRItem.Update_Date = DateTime.Now;
                        }
                    }

                    #endregion

                    var ListBinCard = new List<BinCardViewModelV2>();
                    foreach (var item in queryResult)
                    {
                        //
                        var tag = db.WM_Tag.Find(item.Tag_Index);
                        tag.Tag_Status = -1;
                        tag.Update_By = itemList.cancel_By;
                        tag.Update_Date = DateTime.Now;
                        //cancel Tagitem
                        var tagitem = db.wm_TagItem.Find(item.TagItem_Index);
                        tagitem.Tag_Status = -1;
                        tagitem.Update_By = itemList.cancel_By;
                        tagitem.Update_Date = DateTime.Now;

                        //cancel gril


                        ////--------------------Bin Card --------------------
                        var BinCard = new BinCardViewModelV2();
                        BinCard.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");//item.Process_Index;
                        BinCard.documentType_Index = query.DocumentType_Index; //item.DocumentType_Index;
                        BinCard.documentType_Id = query.DocumentType_Id;//item.DocumentType_Id;
                        BinCard.documentType_Name = query.DocumentType_Name;//item.DocumentType_Name;
                        BinCard.goodsreceive_Index = item.GoodsReceive_Index;
                        BinCard.goodsreceiveItem_Index = item.GoodsReceiveItem_Index;
                        BinCard.goodsreceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                        BinCard.bincard_No = query.GoodsReceive_No; ; //item.BinCard_No;
                        BinCard.binCard_Date = query.GoodsReceive_Date.sParse<DateTime>(); ; //item.BinCard_Date;
                        BinCard.tagitem_Index = item.TagItem_Index;
                        BinCard.tag_index = item.Tag_Index;
                        BinCard.tag_no = item.Tag_No;
                        //BinCard.Tag_Index_To = item.TagItem_Index; //item.Tag_Index_To;
                        //BinCard.Tag_No_To = item.Tag_No; //item.Tag_No_To;
                        BinCard.product_Index = item.Product_Index;
                        BinCard.product_Id = item.Product_Id;
                        BinCard.product_Name = item.Product_Name;
                        BinCard.product_SecondName = item.Product_SecondName;
                        BinCard.product_ThirdName = item.Product_ThirdName;
                        BinCard.product_Index_To = item.Product_Index; //item.Product_Index_To;
                        BinCard.product_Id_To = item.Product_Id;
                        BinCard.product_Name_To = item.Product_Name;
                        BinCard.product_SecondName_To = item.Product_SecondName;
                        BinCard.product_ThirdName_To = item.Product_ThirdName;
                        BinCard.product_Lot = item.Product_Lot;
                        BinCard.product_Lot_To = item.Product_Lot;
                        BinCard.itemstatus_Index = item.ItemStatus_Index;
                        BinCard.itemstatus_Id = item.ItemStatus_Id;
                        BinCard.itemstatus_Name = item.ItemStatus_Name;
                        BinCard.itemstatus_Index_To = item.ItemStatus_Index;
                        BinCard.itemstatus_Id_To = item.ItemStatus_Id;
                        BinCard.itemstatus_Name_To = item.ItemStatus_Name;
                        BinCard.productConversion_Index = item.ProductConversion_Index;
                        BinCard.productConversion_Id = item.ProductConversion_Id;
                        BinCard.productConversion_Name = item.ProductConversion_Name;
                        BinCard.owner_index = query.Owner_Index;//item.Owner_Index;
                        BinCard.owner_Id = query.Owner_Id;//item.Owner_Id;
                        BinCard.owner_Name = query.Owner_Name; // item.Owner_Name;

                        BinCard.owner_index_To = query.Owner_Index;
                        BinCard.owner_Id_To = query.Owner_Id;
                        BinCard.owner_Name_To = query.Owner_Name;

                        BinCard.location_Index = item.Location_Index;//item.Location_Index;
                        BinCard.location_Id = item.Location_Id;//item.Location_Id;
                        BinCard.location_Name = item.Location_Name; //item.Location_Name;
                        BinCard.location_Index_To = item.Location_Index;
                        BinCard.location_Id_To = item.Location_Id;
                        BinCard.location_Name_To = item.Location_Name;
                        BinCard.goodsReceive_EXP_Date = item.EXP_Date;
                        BinCard.goodsReceive_EXP_Date_To = item.EXP_Date;
                        BinCard.bincard_QtyIn = (0 - item.TotalQty);
                        BinCard.bincard_QtyOut = 0;
                        BinCard.bincard_QtySign = (0 - item.TotalQty);
                        BinCard.bincard_WeightIn = (0 - item.Weight);
                        BinCard.bincard_WeightOut = 0;
                        BinCard.bincard_WeightSign = (0 - item.Weight);
                        BinCard.bincard_VolumeIn = (0 - item.Volume);
                        BinCard.bincard_VolumeOut = 0;
                        BinCard.bincard_VolumeSign = (0 - item.Volume);
                        BinCard.ref_document_No = query.GoodsReceive_No;
                        BinCard.ref_document_Index = item.GoodsReceive_Index; //tem.Ref_Document_Index;
                        BinCard.ref_documentItem_Index = item.GoodsReceiveItem_Index;
                        BinCard.create_By = itemList.create_By;
                        BinCard.isCheckBinCard = true;
                        BinCard.totalQty = item.TotalQty;
                        ListBinCard.Add(BinCard);
                    }
                    var resultcancel = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("CancelBinCardGRToList"), new { items = ListBinCard }.sJson());

                    if (!string.IsNullOrEmpty(resultcancel))
                    {
                        return "ใบสั่งซื้อวัสดุนี้ไม่สามารถยกเลิกได้ เนื่องจากมีการทำใบรับวัสดุแล้ว";
                    }

                }
                //else if (query.Document_Status == 2)
                //    return "Cannot be deleted Status Assign Job";

                #region Update GoodsReceive Status
                query.Document_Status = -1;
                query.Cancel_By = itemList.cancel_By;
                query.Cancel_Date = DateTime.Now;
                #endregion

                var queryitem = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == query.GoodsReceive_Index).ToList();

                #region Update GoodsReceiveItem Update By
                //var ItemList = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == itemList.goodsReceive_Index).ToList();
                queryitem.ForEach(x => { x.Document_Status = -1; x.Cancel_By = itemList.cancel_By; x.Cancel_Date = DateTime.Now; });
                if (queryitem.Count() > 0)
                {
                    queryitem.ForEach(x => { x.Document_Status = -1; x.Cancel_By = itemList.cancel_By; x.Cancel_Date = DateTime.Now; });
                    var plangoodsiss = queryitem.GroupBy(g => g.Ref_Document_No).ToList();
                    if (plangoodsiss.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(plangoodsiss.FirstOrDefault().Key))
                        {
                            results = "คุณต้องการยกเลิกใบสั่งซื้อวัสดุ : " + string.Join(",", plangoodsiss.Select(s => s.Key)) + "นี้ใช่หรือไม่";
                        }
                        else
                        {
                            results = "ยกเลิกใบสั่งซื้อสำเร็จ";
                        }
                    }

                }

                #endregion

                #region Update Tag Status

                var qTagItem = db.wm_TagItem.Where(c => c.GoodsReceive_Index == query.GoodsReceive_Index).ToList();

                if (qTagItem.Count > 0)
                {
                    foreach (var item in qTagItem)
                    {
                        var tag = db.WM_Tag.Find(item.Tag_Index);

                        if (tag != null)
                        {
                            tag.Tag_Status = -1;
                            tag.Cancel_By = itemList.cancel_By;
                            tag.Cancel_Date = DateTime.Now;
                        }

                        var tagItem = db.wm_TagItem.Find(item.TagItem_Index);

                        if (tagItem != null)
                        {
                            tagItem.Tag_Status = -1;
                            tagItem.Cancel_By = itemList.cancel_By;
                            tagItem.Cancel_Date = DateTime.Now;
                        }

                    }
                }




                #endregion

                //#region check pgr

                //if (queryitem.Count > 0)
                //{
                //    if (queryitem.FirstOrDefault().Ref_Document_Index != null)
                //    {
                //        var resultPlanGR = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("DeletePlanGoodsReceive"), new { planGoodsReceive_Index = queryitem.FirstOrDefault().Ref_Document_Index }.sJson());
                //        var resultPlanGRI = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("DeletePlanGoodsReceiveItem"), new { planGoodsReceive_Index = queryitem.FirstOrDefault().Ref_Document_Index }.sJson());
                //    }
                //}

                //#endregion

                var transactionx = db.Database.BeginTransaction();
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                    results = results == "ยกเลิกใบสั่งซื้อสำเร็จ" ? "ยกเลิกใบสั่งซื้อสำเร็จ" : "Confirm," + results;


                }

                catch (Exception exy)
                {
                    results = "fasle";
                    transactionx.Rollback();

                    throw exy;

                }

                return results;

            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeletePGR(GoodsReceiveDocViewModel itemList)
        {
            try
            {

                var results = "ยกเลิกใบสั่งซื้อสำเร็จ";

                var queryitem = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == itemList.goodsReceive_Index).GroupBy(g => g.Ref_Document_Index).ToList();

                var GRI = db.IM_GoodsReceiveItem.Where(c => queryitem.Select(s => s.Key).Contains(c.Ref_Document_Index) && c.Document_Status != -1).GroupBy(g => g.GoodsReceive_Index).ToList();

                if (GRI.Count() > 0)
                {
                    return "ใบสั่งซื้อวัสดุนี้ไม่สามารถยกเลิกได้ เนื่องจากมีการทำใบรับวัสดุแล้ว";
                }

                #region check pgr

                if (queryitem.Count > 0)
                {
                    foreach (var item in queryitem)
                    {
                        var resultPlanGR = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("DeletePlanGoodsReceive"), new { planGoodsReceive_Index = item.Key }.sJson());
                        var resultPlanGRI = utils.SendDataApi<bool>(new AppSettingConfig().GetUrl("DeletePlanGoodsReceiveItem"), new { planGoodsReceive_Index = item.Key }.sJson());
                    }
                }

                #endregion

                //db.SaveChanges();

                return results;

            }


            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<GoodsReceiveDocViewModel> confirmStatus(GoodsReceiveDocViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    var result = new List<GoodsReceiveDocViewModel>();

                    #region Update GoodsReceive Status
                    var ItemHeader = db.IM_GoodsReceive.Find(data.goodsReceive_Index);
                    ItemHeader.Document_Status = 1;
                    ItemHeader.Update_By = data.update_By;
                    ItemHeader.Update_Date = DateTime.Now;
                    #endregion

                    #region Update GoodsReceiveItem Update By
                    var ItemList = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status != -1).ToList();
                    ItemList.ForEach(x => { x.Update_By = data.update_By; x.Update_Date = DateTime.Now; });
                    #endregion


                    db.SaveChanges();
                    return result;
                }

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

        public GoodsReceiveViewModel find(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            try
            {
                using (var context = new GRDbContext())
                {

                    var queryResult = db.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == id).FirstOrDefault();

                    var filterModel = new ProcessStatusViewModel();
                    filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                    var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                    var resultItem = new GoodsReceiveViewModel();
                    var Document_Status = queryResult.Document_Status.ToString();

                    resultItem.goodsReceive_Index = queryResult.GoodsReceive_Index;
                    resultItem.goodsReceive_No = queryResult.GoodsReceive_No;
                    resultItem.goodsReceive_Date = queryResult.GoodsReceive_Date.toString();
                    resultItem.goodsReceive_Due_Date = queryResult.GoodsReceive_Due_Date.toString();
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
                    resultItem.processStatus_Name = Process.Where(a => a.processStatus_Id == Document_Status).Select(c => c.processStatus_Name).FirstOrDefault();

                    resultItem.warehouse_Index = queryResult.Warehouse_Index;
                    resultItem.warehouse_Id = queryResult.Warehouse_Id;
                    resultItem.warehouse_Name = queryResult.Warehouse_Name;
                    resultItem.warehouse_Index_To = queryResult.Warehouse_Index_To;
                    resultItem.warehouse_Id_To = queryResult.Warehouse_Id_To;
                    resultItem.warehouse_Name_To = queryResult.Warehouse_Name_To;
                    //resultItem.create_Date = queryResult.Create_Date.toString();
                    resultItem.create_By = queryResult.Create_By;
                    //resultItem.update_Date = queryResult.Update_Date.toString();
                    resultItem.update_By = queryResult.Update_By;
                    // resultItem.cancel_Date = queryResult.Cancel_Date.toString();
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
                    resultItem.whOwner_Index = queryResult.WHOwner_Index;
                    resultItem.whOwner_Id = queryResult.WHOwner_Id;
                    resultItem.whOwner_Name = queryResult.WHOwner_Name;
                    resultItem.invoice_No = queryResult.Invoice_No;

                    resultItem.forwarder_Index = queryResult.Forwarder_Index;
                    resultItem.forwarder_Id = queryResult.Forwarder_Id;
                    resultItem.forwarder_Name = queryResult.Forwarder_Name;
                    resultItem.shipmentType_Index = queryResult.ShipmentType_Index;
                    resultItem.shipmentType_Id = queryResult.ShipmentType_Id;
                    resultItem.shipmentType_Name = queryResult.ShipmentType_Name;
                    resultItem.cargoType_Index = queryResult.CargoType_Index;
                    resultItem.cargoType_Id = queryResult.CargoType_Id;
                    resultItem.cargoType_Name = queryResult.CargoType_Name;
                    resultItem.unloadingType_Index = queryResult.UnloadingType_Index;
                    resultItem.unloadingType_Id = queryResult.UnloadingType_Id;
                    resultItem.unloadingType_Name = queryResult.UnloadingType_Name;
                    resultItem.containerType_Index = queryResult.ContainerType_Index;
                    resultItem.containerType_Id = queryResult.ContainerType_Id;
                    resultItem.containerType_Name = queryResult.ContainerType_Name;
                    resultItem.container_No1 = queryResult.Container_No1;
                    resultItem.container_No2 = queryResult.Container_No2;
                    resultItem.labur = queryResult.Labur;
                    resultItem.checker_Name = queryResult.Checker_Name;
                    resultItem.license_Name = queryResult.License_Name;
                    resultItem.driver_Name = queryResult.Driver_Name;

                    resultItem.costCenter_Index = queryResult.CostCenter_Index;
                    resultItem.costCenter_Id = queryResult.CostCenter_Id;
                    resultItem.costCenter_Name = queryResult.CostCenter_Name;

                    //resultItem.docfile = new List<GoodsReceiveImageViewModel>();
                    //var image = db.im_GoodsReceive_Image.Where(c => c.GoodsReceive_Index == queryResult.GoodsReceive_Index && c.Document_Status == 0).ToList();
                    //foreach (var i in image)
                    //{
                    //    var fileimage = new GoodsReceiveImageViewModel();
                    //    fileimage.goodsReceiveImage_Index = i.GoodsReceiveImage_Index;
                    //    fileimage.goodsReceive_Index = i.GoodsReceive_Index.sParse<Guid>();
                    //    fileimage.goodsReceiveImage_path = i.GoodsReceiveImage_path;
                    //    fileimage.goodsReceiveImage_type = i.GoodsReceiveImage_type;
                    //    fileimage.name = i.GoodsReceiveImage_path;
                    //    fileimage.src = i.GoodsReceiveImage_path;
                    //    fileimage.type = i.GoodsReceiveImage_type;
                    //    resultItem.docfile.Add(fileimage);

                    //}

                    return resultItem;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PlanGoodsReceiveViewModel> ScanPlanGR(PlanGoodsReceiveViewModel data)
        {
            try
            {
                var result = new List<PlanGoodsReceiveViewModel>();

                var filterModel = new PlanGoodsReceiveViewModel();
                if (!string.IsNullOrEmpty(data.planGoodsReceive_No))
                {
                    filterModel.planGoodsReceive_No = data.planGoodsReceive_No;
                }

                //GetConfig
                result = utils.SendDataApi<List<PlanGoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("ScanPlanGR"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<LPNViewModel> CheckTAG(String Tag_No)
        {
            try
            {
                //var query = db.WM_Tag.AsQueryable();
                //string msg = "";
                //string pstring = " and Tag_No ='" + Tag_No + "'";
                //var strwhere = new SqlParameter("@strwhere", pstring);
                var result = new List<LPNViewModel>();
                //var strwhere1 = new SqlParameter("@strwhere", " and Tag_No ='" + Tag_No + "' and BinBalance_QtyBal > 0 ");
                var resultItem = new LPNViewModel();
                var filterModel = new DocumentTypeViewModel();
                Guid DocumentType_Index = new Guid("CEBE721A-6BBC-4082-A585-6F75F06F0E31");


                filterModel.documentType_Index = DocumentType_Index;
                //GetConfig
                var results = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("GetFormateDocumentType"), filterModel.sJson());
                var DataDocumentType = results.Select(c => new { c.format_Text, c.format_Date, c.format_Running }).FirstOrDefault();

                var FormatLPN = DataDocumentType.format_Text + DataDocumentType.format_Date + DataDocumentType.format_Running;
                var FormatLPNNum = FormatLPN.Length;
                int LPNNum = Tag_No.Length;
                if (LPNNum == FormatLPNNum)
                {
                    var FormatText = DataDocumentType.format_Text.Length;
                    var FormatDate = DataDocumentType.format_Date.Length;
                    var FormatRunning = DataDocumentType.format_Running.Length;
                    var LPNText = Tag_No.Substring(0, FormatText);
                    var LPNDate = Tag_No.Substring(FormatText, FormatDate);
                    var LPNRunning = Tag_No.Substring((FormatDate + FormatText), FormatRunning);
                    var chekNumeric = Tag_No.Substring(FormatText, (FormatRunning + FormatDate));
                    var isNumeric = int.TryParse(chekNumeric, out int n);


                    //เช๊ค Format_Text
                    if (LPNText.Length != FormatText)
                    {
                        resultItem.tag_No = "false";
                        result.Add(resultItem);
                    }
                    //เช๊ค Format_Date
                    else if (LPNDate.Length != FormatDate)
                    {
                        resultItem.tag_No = "false";
                        result.Add(resultItem);
                    }
                    //เช๊ค Format_Running
                    else if (LPNRunning.Length != FormatRunning)
                    {
                        resultItem.tag_No = "false";
                        result.Add(resultItem);
                    }
                    //เช๊ค 3 ตัวหน้าตรงกับ Format_Text หรือป่าว
                    else if (LPNText != DataDocumentType.format_Text)
                    {
                        resultItem.tag_No = "false";
                        result.Add(resultItem);
                    }
                    //เช๊ค Formate_Date && Formate_Running เป็นตัวเลขหรือป่าว
                    else if (isNumeric != true)
                    {
                        resultItem.tag_No = "false";
                        result.Add(resultItem);
                    }
                    else
                    {
                        //var chkTag = db.wm_BinBalance.Where(c => c.Tag_No == Tag_No && c.BinBalance_QtyBal > 0).ToList();
                        //if (chkTag == null || chkTag.Count <= 0)
                        //{
                        var queryResult = db.WM_Tag.Where(c => c.Tag_No == Tag_No).ToList();


                        foreach (var item in queryResult)
                        {
                            resultItem = new LPNViewModel();

                            resultItem.tag_Index = item.Tag_Index;
                            resultItem.tag_No = item.Tag_No;
                            resultItem.Pallet_No = item.Pallet_No;
                            resultItem.Pallet_Index = item.Pallet_Index;
                            resultItem.create_Date = item.Create_Date.GetValueOrDefault();
                            resultItem.create_By = item.Create_By;
                            resultItem.update_Date = item.Update_Date.GetValueOrDefault();
                            resultItem.update_By = item.Update_By;
                            resultItem.cancel_Date = item.Cancel_Date.GetValueOrDefault();
                            resultItem.cancel_By = item.Cancel_By;

                            result.Add(resultItem);
                        }
                        //}
                        //else
                        //{
                        //    resultItem = new LPNViewModel();

                        //    resultItem.Tag_Index = new Guid();
                        //    resultItem.Tag_No = "CannotSave";
                        //    resultItem.Pallet_No = " CannotSave";
                        //    resultItem.Pallet_Index = new Guid();

                        //    result.Add(resultItem);
                        //}
                    }
                }
                else
                {
                    resultItem.tag_No = "false";
                    result.Add(resultItem);
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String CreateScanLPN(LPNViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())

                {

                    string msg = "";
                    Boolean chkAllComplete = true;
                    Guid Index;
                    string TagNumber = data.tag_No;
                    int tagStatus = 0;
                    String State = "Start";
                    String msglog = "";
                    var olog = new logtxt();


                    //save
                    if (data.tag_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        Index = Guid.NewGuid();
                    }

                    var Tag_Index = new SqlParameter("Tag_Index", Index);

                    var strwhere1 = new SqlParameter("@strwhere", "and Tag_No = '" + data.tag_No + "'");
                    //var queryResult = context.WM_Tag.FromSql("sp_GetTag @strwhere", strwhere1).ToList();

                    var queryResult = context.WM_Tag.AsQueryable().Where(c => c.Tag_No == data.tag_No).ToList();
                    if (queryResult.Count != 0)
                    {
                        return msg = "Can not Create LPN";
                    }


                    WM_Tag Tag = new WM_Tag();

                    Tag.Tag_Index = Index;
                    Tag.Tag_No = data.tag_No;
                    Tag.Pallet_No = data.Pallet_No;
                    if (data.Pallet_Index != null)
                    {
                        Tag.Pallet_Index = data.Pallet_Index;
                    }
                    else
                    {
                        Tag.Pallet_Index = Guid.NewGuid();
                    }
                    Tag.TagRef_No1 = data.tagRef_No1;
                    Tag.TagRef_No2 = data.tagRef_No2;
                    Tag.TagRef_No3 = data.tagRef_No3;
                    Tag.TagRef_No4 = data.tagRef_No4;
                    Tag.TagRef_No5 = data.tagRef_No5;
                    Tag.Tag_Status = tagStatus;
                    Tag.UDF_1 = data.uDF_1;
                    Tag.UDF_2 = data.uDF_2;
                    Tag.UDF_3 = data.uDF_3;
                    Tag.UDF_4 = data.uDF_4;
                    Tag.UDF_5 = data.uDF_5;
                    Tag.Create_By = data.create_By;
                    Tag.Create_Date = DateTime.Now;
                    Tag.Update_By = data.update_By;
                    Tag.Update_Date = null;
                    Tag.Cancel_By = data.cancel_By;
                    Tag.Cancel_Date = null;
                    db.WM_Tag.Add(Tag);



                    //var TagRef_No1 = new SqlParameter("TagRef_No1", "");
                    //var TagRef_No2 = new SqlParameter("TagRef_No2", "");
                    //var TagRef_No3 = new SqlParameter("TagRef_No3", "");
                    //var TagRef_No4 = new SqlParameter("TagRef_No4", "");
                    //var TagRef_No5 = new SqlParameter("TagRef_No5", "");
                    //var Tag_Status = new SqlParameter("Tag_Status", tagStatus);
                    //var UDF_1 = new SqlParameter("UDF_1", "");
                    //var UDF_2 = new SqlParameter("UDF_2", "");
                    //var UDF_3 = new SqlParameter("UDF_3", "");
                    //var UDF_4 = new SqlParameter("UDF_4", "");
                    //var UDF_5 = new SqlParameter("UDF_5", "");
                    //var Create_By = new SqlParameter("Create_By", data.Create_By);
                    //var Create_Date = new SqlParameter("Create_Date", DateTime.Now.Date);
                    //var Update_By = new SqlParameter("Update_By", "");
                    //var Update_Date = new SqlParameter("Update_Date", DateTime.Now.Date);
                    //var Cancel_By = new SqlParameter("Cancel_By", "");
                    //var Cancel_Date = new SqlParameter("Cancel_Date", DateTime.Now.Date);
                    //var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_wm_Tag  @Tag_Index,@Tag_No,@Pallet_No,@Pallet_Index,@TagRef_No1,@TagRef_No2,@TagRef_No3,@TagRef_No4,@TagRef_No5,@Tag_Status,@UDF_1,@UDF_2,@UDF_3,@UDF_4,@UDF_5,@Create_By,@Create_Date,@Update_By,@Update_Date,@Cancel_By,@Cancel_Date ", Tag_Index, Tag_No, Pallet_No, Pallet_Index, TagRef_No1, TagRef_No2, TagRef_No3, TagRef_No4, TagRef_No5, Tag_Status, UDF_1, UDF_2, UDF_3, UDF_4, UDF_5, Create_By, Create_Date, Update_By, Update_Date, Cancel_By, Cancel_Date);
                    ////Check 
                    //// if  ok
                    //if (rowsAffected != 0)
                    //{
                    //    msg += "OK";
                    //}
                    ////else
                    //else
                    //{
                    //    msg += "Error";
                    //}

                    chkAllComplete = false;
                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transactionx.Commit();
                        msg += "OK";
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("SavePlanGR", msglog);
                        transactionx.Rollback();
                        msg += "Error";
                        throw exy;

                    }
                    // Check All Complete
                    return msg;
                }
            }



            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean CheckReceiveQtyV2(Guid planGoodsReceive_Index, Guid planGoodsReceiveItem_Index, String product_Index, decimal qty, decimal productConversion_Ratio, String productConversion_Index)
        {
            try
            {
                var PlanGRI = new List<IM_PlanGoodsReceiveItem>();

                var filterModel = new IM_PlanGoodsReceiveItem();
                //Guid PlanGR_Index = new Guid("planGoodsReceive_Index");
                //Guid PlanGRI_Index = new Guid("planGoodsReceiveItem_Index");


                filterModel.PlanGoodsReceive_Index = planGoodsReceive_Index;
                filterModel.PlanGoodsReceiveItem_Index = planGoodsReceiveItem_Index;

                //GetConfig
                PlanGRI = utils.SendDataApi<List<IM_PlanGoodsReceiveItem>>(new AppSettingConfig().GetUrl("GetPlanGoodsReceiveIem"), filterModel.sJson());


                var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == planGoodsReceive_Index && c.Ref_DocumentItem_Index == planGoodsReceiveItem_Index && c.Document_Status != -1).Select(a => a.TotalQty).FirstOrDefault();
                var TotalPlanGRI = PlanGRI.Select(c => c.TotalQty).FirstOrDefault();


                var Total = (TotalPlanGRI - (TotalGR + (qty * productConversion_Ratio)));
                if (Total >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean CheckReceiveQtyProduct(String GoodsReceive_Index, String GoodsReceiveItem_Index, String Product_Index)
        {
            try
            {
                using (var context = new GRDbContext())
                {



                    var pGoodsReceive_Index = new SqlParameter("@GoodsReceive_Index", GoodsReceive_Index);
                    var pGoodsReceiveItem_Index = new SqlParameter("@GoodsReceiveItem_Index", GoodsReceiveItem_Index);
                    var pProduct_Index = new SqlParameter("@Product_Index", Product_Index);

                    var resultParameter = new SqlParameter("@result", SqlDbType.NVarChar);
                    resultParameter.Size = 2000; // some meaningfull value
                    resultParameter.Direction = ParameterDirection.Output;
                    context.Database.ExecuteSqlCommand("EXEC CheckReceiveQtyProduct  @GoodsReceive_Index,@GoodsReceiveItem_Index,@Product_Index,@result OUTPUT", pGoodsReceive_Index, pGoodsReceiveItem_Index, pProduct_Index, resultParameter);

                    if (resultParameter.Value.ToString() == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SaveTAGChanges(GoodsReceiveTagItemViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var PlanGRI = new CheckReceiveQtyViewModel();

                    String State = "Start";
                    String msglog = "";
                    var olog = new logtxt();
                    var document_status = 0;
                    var documentPriority_Status = 0;
                    var putaway_Status = 0;
                    var Goods_ReceiveRemark = "";
                    int refDocLineNum = 0;
                    var DocumentDate = data.planGoodsReceive_Date.toDate();
                    var goodReceiveDate = data.goodsReceive_Date.toDate();

                    PlanGRI.planGoodsReceive_Index = data.planGoodsReceive_Index;
                    PlanGRI.planGoodsReceiveItem_Index = data.planGoodsReceiveItem_Index;

                    var GR = context.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == data.planGoodsReceive_Index && c.Document_Status != -1).FirstOrDefault();

                    var chkQty = CheckReceiveQtyV2(PlanGRI.planGoodsReceive_Index, PlanGRI.planGoodsReceiveItem_Index, data.product_Index.ToString(), (decimal)data.qty, data.productConversion_Ratio, data.productConversion_Index.ToString());
                    if (!chkQty)
                    {
                        return "";
                    }
                    else
                    {
                        if (GR != null)
                        {
                            if (GR.Document_Status != 3)
                            {
                                //db.IM_GoodsReceive.Add(GoodsReceiveOld);

                                //var itemDetail = new List<GoodsReceiveItemViewModel>();
                                int addNumber = 0;
                                foreach (var item in data.listPlanGoodsReceiveItemViewModel)
                                {
                                    var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
                                    var ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
                                    var ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
                                    var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                                    var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                                    var TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[ms_ItemStatus]");
                                    var Where = new SqlParameter("@Where", "");
                                    var DataItemStatus = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                                    addNumber++;
                                    IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                                    if (item.ref_Document_Index != null)
                                    {
                                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                                        ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),Process_Index)");
                                        ColumnName3 = new SqlParameter("@ColumnName3", "''");
                                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
                                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
                                        TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[ms_DocumentType]");
                                        Where = new SqlParameter("@Where", "Where DocumentType_Index in (select DocumentType_Index from im_PlanGoodsReceive where PlanGoodsReceive_Index ='" + item.ref_Document_Index + "')");
                                        var DataDocumentType = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();
                                        resultItem.Ref_Process_Index = new Guid(DataDocumentType[0].dataincolumn2);
                                    }
                                    else
                                    {
                                        resultItem.Ref_Process_Index = new Guid("00000000-0000-0000-0000-000000000000");
                                    }

                                    // Gen Index for line item
                                    if (item.goodsReceiveItem_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                                    {
                                        item.goodsReceiveItem_Index = Guid.NewGuid();
                                    }
                                    resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;

                                    // Index From Header
                                    resultItem.GoodsReceive_Index = GR.GoodsReceive_Index;
                                    if (item.lineNum == null)
                                    {
                                        resultItem.LineNum = addNumber.ToString();
                                    }
                                    else
                                    {
                                        resultItem.LineNum = item.lineNum;
                                    }
                                    resultItem.Product_Index = item.product_Index;
                                    resultItem.Product_Id = item.product_Id;
                                    resultItem.Product_Name = item.product_Name;
                                    resultItem.Product_SecondName = item.product_SecondName;
                                    resultItem.Product_ThirdName = item.product_ThirdName;
                                    if (item.product_Lot != "")
                                    {
                                        resultItem.Product_Lot = item.product_Lot;
                                    }
                                    else
                                    {
                                        resultItem.Product_Lot = "";
                                    }
                                    if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                                    {
                                        resultItem.ItemStatus_Index = item.itemStatus_Index;
                                    }
                                    else
                                    {
                                        resultItem.ItemStatus_Index = new Guid(DataItemStatus.dataincolumn1);
                                    }
                                    if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                                    {
                                        resultItem.ItemStatus_Id = item.itemStatus_Id;
                                    }
                                    else
                                    {
                                        resultItem.ItemStatus_Id = DataItemStatus.dataincolumn2;
                                    }
                                    if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                                    {
                                        resultItem.ItemStatus_Name = item.itemStatus_Name;
                                    }
                                    else
                                    {
                                        resultItem.ItemStatus_Name = DataItemStatus.dataincolumn3;
                                    }
                                    resultItem.Qty = item.qty;
                                    resultItem.Ratio = item.ratio;
                                    resultItem.TotalQty = item.qty * item.ratio;
                                    resultItem.UDF_1 = item.uDF_1;
                                    resultItem.ProductConversion_Index = item.productConversion_Index;
                                    resultItem.ProductConversion_Id = item.productConversion_Id;
                                    resultItem.ProductConversion_Name = item.productConversion_Name;
                                    resultItem.MFG_Date = item.mFG_Date.toDate();
                                    resultItem.EXP_Date = item.eXP_Date.toDate();
                                    if (item.unitWeight != null)
                                    {
                                        resultItem.UnitWeight = item.unitWeight;
                                    }
                                    else
                                    {
                                        resultItem.UnitWeight = 0;
                                    }

                                    if (item.weight != null)
                                    {
                                        resultItem.Weight = item.weight;
                                    }
                                    else
                                    {
                                        resultItem.Weight = 0;
                                    }

                                    if (item.unitWidth != null)
                                    {
                                        resultItem.UnitWidth = item.unitWidth;
                                    }
                                    else
                                    {
                                        resultItem.UnitWidth = 0;
                                    }

                                    if (item.unitLength != null)
                                    {
                                        resultItem.UnitLength = item.unitLength;
                                    }
                                    else
                                    {
                                        resultItem.UnitLength = 0;
                                    }

                                    if (item.unitHeight != null)
                                    {
                                        resultItem.UnitHeight = item.unitHeight;
                                    }
                                    else
                                    {
                                        resultItem.UnitHeight = 0;
                                    }

                                    if (item.unitVolume != null)
                                    {
                                        resultItem.UnitVolume = item.unitVolume;
                                    }
                                    else
                                    {
                                        resultItem.UnitVolume = 0;
                                    }

                                    if (item.volume != null)
                                    {
                                        resultItem.Volume = item.volume;
                                    }
                                    else
                                    {
                                        resultItem.Volume = 0;
                                    }

                                    if (item.unitPrice != null)
                                    {
                                        resultItem.UnitPrice = item.unitPrice;
                                    }
                                    else
                                    {
                                        resultItem.UnitPrice = 0;
                                    }

                                    if (item.price != null)
                                    {
                                        resultItem.Price = item.price;
                                    }
                                    else
                                    {
                                        resultItem.Price = 0;
                                    }

                                    if (item.ref_Document_No == null)
                                    {
                                        item.ref_Document_No = "";
                                    }
                                    resultItem.Ref_Document_No = item.ref_Document_No;
                                    if (item.ref_Document_LineNum == null)
                                    {
                                        resultItem.Ref_Document_LineNum = refDocLineNum.ToString();
                                    }
                                    else
                                    {
                                        resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                                    }
                                    //var itemlist = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem").Where(c => c.GoodsReceive_Index == itemHeader.goodsReceive_Index).ToList();
                                    resultItem.Ref_Document_Index = data.planGoodsReceive_Index;
                                    resultItem.Ref_DocumentItem_Index = data.planGoodsReceiveItem_Index;
                                    resultItem.DocumentRef_No1 = item.documentRef_No1;
                                    resultItem.DocumentRef_No2 = item.documentRef_No2;
                                    resultItem.DocumentRef_No3 = item.documentRef_No3;
                                    resultItem.DocumentRef_No4 = item.documentRef_No4;
                                    resultItem.DocumentRef_No5 = item.documentRef_No5;
                                    resultItem.Document_Status = document_status;
                                    resultItem.UDF_1 = item.uDF_1;
                                    resultItem.UDF_2 = item.uDF_2;
                                    resultItem.UDF_3 = item.uDF_3;
                                    resultItem.UDF_4 = item.uDF_4;
                                    resultItem.UDF_5 = item.uDF_5;
                                    resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
                                    resultItem.GoodsReceive_DockDoor = "";
                                    resultItem.Create_By = data.create_By;
                                    resultItem.Create_Date = DateTime.Now;
                                    db.IM_GoodsReceiveItem.Add(resultItem);


                                    WM_TagItem TagItem = new WM_TagItem();

                                    if (data.tagItem_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                                    {
                                        data.tagItem_Index = Guid.NewGuid();
                                    }
                                    TagItem.TagItem_Index = data.tagItem_Index;
                                    TagItem.Tag_Index = data.tag_Index;
                                    TagItem.Tag_No = data.tag_No;
                                    TagItem.GoodsReceive_Index = GR.GoodsReceive_Index;
                                    TagItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                                    TagItem.Product_Index = item.product_Index;
                                    TagItem.Product_Id = item.product_Id;
                                    TagItem.Product_Name = item.product_Name;
                                    TagItem.Product_SecondName = item.product_SecondName;
                                    TagItem.Product_ThirdName = item.product_ThirdName;
                                    if (item.product_Lot != "")
                                    {
                                        TagItem.Product_Lot = item.product_Lot;
                                    }
                                    else
                                    {
                                        TagItem.Product_Lot = "";
                                    }
                                    if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                                    {
                                        TagItem.ItemStatus_Index = item.itemStatus_Index;
                                    }
                                    else
                                    {
                                        TagItem.ItemStatus_Index = new Guid(DataItemStatus.dataincolumn1);
                                    }
                                    if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                                    {
                                        TagItem.ItemStatus_Id = item.itemStatus_Id;
                                    }
                                    else
                                    {
                                        TagItem.ItemStatus_Id = DataItemStatus.dataincolumn2;
                                    }
                                    if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                                    {
                                        TagItem.ItemStatus_Name = item.itemStatus_Name;
                                    }
                                    else
                                    {
                                        TagItem.ItemStatus_Name = DataItemStatus.dataincolumn3;
                                    }
                                    TagItem.Qty = item.qty;
                                    TagItem.Ratio = item.ratio;
                                    TagItem.TotalQty = item.totalQty;
                                    TagItem.ProductConversion_Index = item.productConversion_Index;
                                    TagItem.ProductConversion_Id = item.productConversion_Id;
                                    TagItem.ProductConversion_Name = item.productConversion_Name;
                                    resultItem.MFG_Date = item.mFG_Date.toDate();
                                    resultItem.EXP_Date = item.eXP_Date.toDate();
                                    if (item.weight != null)
                                    {
                                        TagItem.Weight = item.weight;
                                    }
                                    else
                                    {
                                        TagItem.Weight = 0;
                                    }
                                    if (item.volume != null)
                                    {
                                        TagItem.Volume = item.volume;
                                    }
                                    else
                                    {
                                        TagItem.Volume = 0;
                                    }
                                    TagItem.TagRef_No1 = data.tagRef_No1;
                                    TagItem.TagRef_No2 = data.tagRef_No2;
                                    TagItem.TagRef_No3 = data.tagRef_No3;
                                    TagItem.TagRef_No4 = data.tagRef_No4;
                                    TagItem.TagRef_No5 = data.tagRef_No5;
                                    TagItem.UDF_1 = item.uDF_1;
                                    TagItem.UDF_2 = item.uDF_2;
                                    TagItem.UDF_3 = item.uDF_3;
                                    TagItem.UDF_4 = item.uDF_4;
                                    TagItem.UDF_5 = item.uDF_5;
                                    TagItem.Create_By = data.create_By;
                                    TagItem.Create_Date = DateTime.Now;
                                    db.wm_TagItem.Add(TagItem);
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                        else
                        {
                            if (data.goodsReceive_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                            {
                                data.goodsReceive_Index = Guid.NewGuid();
                            }

                            //----Set Header------
                            IM_GoodsReceive itemHeader = new IM_GoodsReceive();

                            itemHeader.GoodsReceive_Index = data.goodsReceive_Index;
                            itemHeader.GoodsReceive_No = data.goodsReceive_No;
                            itemHeader.Owner_Index = data.owner_Index;
                            itemHeader.Owner_Id = data.owner_Id;
                            itemHeader.Owner_Name = data.owner_Name;

                            var pstring = "";
                            Guid DocumentTypeIndex = new Guid("45A7790A-8477-44D4-959E-A4B845870507");
                            if (data.documentType_Index == DocumentTypeIndex)
                            {
                                pstring = "Where DocumentType_Index = '" + data.documentType_Index + "' and DocumentType_Name_To = 'Manual create order'";
                            }
                            else
                            {
                                pstring = "Where DocumentType_Index = '" + data.documentType_Index + "'";
                            }
                            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                            var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),DocumentType_Index_To)");
                            var ColumnName3 = new SqlParameter("@ColumnName3", "DocumentType_Id_To");
                            var ColumnName4 = new SqlParameter("@ColumnName4", "DocumentType_Name_To");
                            var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            var TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[sy_DocumentTypeRef]");
                            var Where = new SqlParameter("@Where", pstring);
                            var DataDocumentTyperef = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();
                            if (data.documentType_Index != null)
                            {
                                itemHeader.DocumentType_Index = new Guid(DataDocumentTyperef.dataincolumn2);
                                itemHeader.DocumentType_Id = DataDocumentTyperef.dataincolumn3;
                                itemHeader.DocumentType_Name = DataDocumentTyperef.dataincolumn4;

                            }

                            if (data.goodsReceive_No == null)
                            {
                                var DocumentType_Index = new SqlParameter("@DocumentType_Index", DataDocumentTyperef.dataincolumn2.ToString());
                                var DocDate = new SqlParameter("@DocDate", DocumentDate);
                                var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                                resultParameter.Size = 2000; // some meaningfull value
                                resultParameter.Direction = ParameterDirection.Output;
                                db.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
                                //var result = resultParameter.Value;
                                data.goodsReceive_No = resultParameter.Value.ToString();
                            }
                            itemHeader.GoodsReceive_No = data.goodsReceive_No;
                            var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            itemHeader.GoodsReceive_Date = DateTime.Now;
                            itemHeader.DocumentRef_No1 = data.documentRef_No1;
                            itemHeader.DocumentRef_No2 = data.documentRef_No2;
                            itemHeader.DocumentRef_No3 = data.documentRef_No3;
                            itemHeader.DocumentRef_No4 = data.documentRef_No4;
                            itemHeader.DocumentRef_No5 = data.documentRef_No5;
                            itemHeader.Document_Status = document_status;
                            itemHeader.UDF_1 = data.udf_1;
                            itemHeader.UDF_2 = data.udf_2;
                            itemHeader.UDF_3 = data.udf_3;
                            itemHeader.UDF_4 = data.udf_4;
                            itemHeader.UDF_5 = data.udf_5;
                            itemHeader.DocumentPriority_Status = documentPriority_Status;
                            itemHeader.Document_Remark = data.document_Remark;
                            if (itemHeader.Create_By == null || itemHeader.Create_By == "")
                            {
                                itemHeader.Create_Date = DateTime.Now;
                            }
                            else
                            {
                                itemHeader.Create_Date = data.create_Date.toDate();
                            }
                            //itemHeader.Create_Date = (itemHeader.Create_By == null || itemHeader.Create_By == "") ? DateTime.Now: data.Create_Date.toDate(); 
                            itemHeader.Create_By = data.create_By;
                            itemHeader.Update_By = data.update_By;
                            itemHeader.Update_Date = data.update_Date.toDate();
                            itemHeader.Cancel_By = data.cancel_By;
                            itemHeader.Cancel_Date = data.cancel_Date.toDate();
                            itemHeader.Warehouse_Index = data.warehouse_Index;
                            itemHeader.Warehouse_Id = data.warehouse_Id;
                            itemHeader.Warehouse_Name = data.warehouse_Name;
                            itemHeader.Warehouse_Index_To = data.warehouse_Index_To;
                            itemHeader.Warehouse_Id_To = data.warehouse_Id_To;
                            itemHeader.Warehouse_Name_To = data.warehouse_Name_To;
                            itemHeader.Putaway_Status = putaway_Status;
                            itemHeader.DockDoor_Index = data.dockDoor_Index;
                            itemHeader.DockDoor_Id = data.dockDoor_Id;
                            itemHeader.DockDoor_Name = data.dockDoor_Name;
                            itemHeader.VehicleType_Index = data.vehicleType_Index;
                            itemHeader.VehicleType_Id = data.vehicleType_Id;
                            itemHeader.VehicleType_Name = data.vehicleType_Name;
                            itemHeader.ContainerType_Index = data.containerType_Index;
                            itemHeader.ContainerType_Id = data.containerType_Id;
                            itemHeader.ContainerType_Name = data.containerType_Name;

                            //----Set Detail-----
                            //var itemDetail = new List<GoodsReceiveItemViewModel>();
                            db.IM_GoodsReceive.Add(itemHeader);
                            int addNumber = 0;
                            foreach (var item in data.listPlanGoodsReceiveItemViewModel)
                            {


                                ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ItemStatus_Index)");
                                ColumnName2 = new SqlParameter("@ColumnName2", "ItemStatus_Id");
                                ColumnName3 = new SqlParameter("@ColumnName3", "ItemStatus_Name");
                                ColumnName4 = new SqlParameter("@ColumnName4", "''");
                                ColumnName5 = new SqlParameter("@ColumnName5", "''");
                                TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[ms_ItemStatus]");
                                Where = new SqlParameter("@Where", "");
                                var DataItemStatus = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();

                                addNumber++;
                                IM_GoodsReceiveItem resultItem = new IM_GoodsReceiveItem();

                                if (item.ref_Document_Index != null)
                                {
                                    ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                                    ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),Process_Index)");
                                    ColumnName3 = new SqlParameter("@ColumnName3", "''");
                                    ColumnName4 = new SqlParameter("@ColumnName4", "''");
                                    ColumnName5 = new SqlParameter("@ColumnName5", "''");
                                    TableName = new SqlParameter("@TableName", "[WMSDB_Master]..[ms_DocumentType]");
                                    Where = new SqlParameter("@Where", "Where DocumentType_Index in (select DocumentType_Index from im_PlanGoodsReceive where PlanGoodsReceive_Index ='" + item.ref_Document_Index + "')");
                                    var DataDocumentType = db.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();
                                    resultItem.Ref_Process_Index = new Guid(DataDocumentType[0].dataincolumn2);
                                }
                                else
                                {
                                    resultItem.Ref_Process_Index = new Guid("00000000-0000-0000-0000-000000000000");
                                }

                                // Gen Index for line item
                                if (item.goodsReceiveItem_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                                {
                                    item.goodsReceiveItem_Index = Guid.NewGuid();
                                }
                                resultItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;

                                // Index From Header
                                resultItem.GoodsReceive_Index = data.goodsReceive_Index;
                                if (item.lineNum == null)
                                {
                                    resultItem.LineNum = addNumber.ToString();
                                }
                                else
                                {
                                    resultItem.LineNum = item.lineNum;
                                }

                                resultItem.Product_Index = item.product_Index;
                                resultItem.Product_Id = item.product_Id;
                                resultItem.Product_Name = item.product_Name;
                                resultItem.Product_SecondName = item.product_SecondName;
                                resultItem.Product_ThirdName = item.product_ThirdName;
                                if (item.product_Lot != "")
                                {
                                    resultItem.Product_Lot = item.product_Lot;
                                }
                                else
                                {
                                    resultItem.Product_Lot = "";
                                }
                                if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                                {
                                    resultItem.ItemStatus_Index = item.itemStatus_Index;
                                }
                                else
                                {
                                    resultItem.ItemStatus_Index = new Guid(DataItemStatus.dataincolumn1);
                                }
                                if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                                {
                                    resultItem.ItemStatus_Id = item.itemStatus_Id;
                                }
                                else
                                {
                                    resultItem.ItemStatus_Id = DataItemStatus.dataincolumn2;
                                }
                                if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                                {
                                    resultItem.ItemStatus_Name = item.itemStatus_Name;
                                }
                                else
                                {
                                    resultItem.ItemStatus_Name = DataItemStatus.dataincolumn3;
                                }
                                resultItem.Qty = item.qty;
                                resultItem.Ratio = item.ratio;
                                resultItem.TotalQty = item.qty * item.ratio;
                                resultItem.UDF_1 = item.uDF_1;
                                resultItem.ProductConversion_Index = item.productConversion_Index;
                                resultItem.ProductConversion_Id = item.productConversion_Id;
                                resultItem.ProductConversion_Name = item.productConversion_Name;
                                resultItem.MFG_Date = item.mFG_Date.toDate();
                                resultItem.EXP_Date = item.eXP_Date.toDate();
                                if (item.unitWeight != null)
                                {
                                    resultItem.UnitWeight = item.unitWeight;
                                }
                                else
                                {
                                    resultItem.UnitWeight = 0;
                                }

                                if (item.weight != null)
                                {
                                    resultItem.Weight = item.weight;
                                }
                                else
                                {
                                    resultItem.Weight = 0;
                                }

                                if (item.unitWidth != null)
                                {
                                    resultItem.UnitWidth = item.unitWidth;
                                }
                                else
                                {
                                    resultItem.UnitWidth = 0;
                                }

                                if (item.unitLength != null)
                                {
                                    resultItem.UnitLength = item.unitLength;
                                }
                                else
                                {
                                    resultItem.UnitLength = 0;
                                }

                                if (item.unitHeight != null)
                                {
                                    resultItem.UnitHeight = item.unitHeight;
                                }
                                else
                                {
                                    resultItem.UnitHeight = 0;
                                }

                                if (item.unitVolume != null)
                                {
                                    resultItem.UnitVolume = item.unitVolume;
                                }
                                else
                                {
                                    resultItem.UnitVolume = 0;
                                }

                                if (item.volume != null)
                                {
                                    resultItem.Volume = item.volume;
                                }
                                else
                                {
                                    resultItem.Volume = 0;
                                }

                                if (item.unitPrice != null)
                                {
                                    resultItem.UnitPrice = item.unitPrice;
                                }
                                else
                                {
                                    resultItem.UnitPrice = 0;
                                }

                                if (item.price != null)
                                {
                                    resultItem.Price = item.price;
                                }
                                else
                                {
                                    resultItem.Price = 0;
                                }

                                if (item.ref_Document_No == null)
                                {
                                    item.ref_Document_No = "";
                                }
                                resultItem.Ref_Document_No = item.ref_Document_No;
                                if (item.ref_Document_LineNum == null)
                                {
                                    resultItem.Ref_Document_LineNum = refDocLineNum.ToString();
                                }
                                else
                                {
                                    resultItem.Ref_Document_LineNum = item.ref_Document_LineNum;
                                }
                                //var itemlist = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItem").Where(c => c.GoodsReceive_Index == itemHeader.goodsReceive_Index).ToList();
                                resultItem.Ref_Document_Index = data.planGoodsReceive_Index;
                                resultItem.Ref_DocumentItem_Index = data.planGoodsReceiveItem_Index;
                                resultItem.DocumentRef_No1 = item.documentRef_No1;
                                resultItem.DocumentRef_No2 = item.documentRef_No2;
                                resultItem.DocumentRef_No3 = item.documentRef_No3;
                                resultItem.DocumentRef_No4 = item.documentRef_No4;
                                resultItem.DocumentRef_No5 = item.documentRef_No5;
                                resultItem.Document_Status = document_status;
                                resultItem.UDF_1 = item.uDF_1;
                                resultItem.UDF_2 = item.uDF_2;
                                resultItem.UDF_3 = item.uDF_3;
                                resultItem.UDF_4 = item.uDF_4;
                                resultItem.UDF_5 = item.uDF_5;
                                resultItem.GoodsReceive_Remark = item.goodsReceive_Remark;
                                resultItem.GoodsReceive_DockDoor = "";
                                resultItem.Create_By = data.create_By;
                                resultItem.Create_Date = DateTime.Now;
                                //itemDetail.Add(resultItem);
                                db.IM_GoodsReceiveItem.Add(resultItem);

                                WM_TagItem TagItem = new WM_TagItem();

                                if (data.tagItem_Index.ToString() == "00000000-0000-0000-0000-000000000000")
                                {
                                    data.tagItem_Index = Guid.NewGuid();
                                }
                                TagItem.TagItem_Index = data.tagItem_Index;
                                TagItem.Tag_Index = data.tag_Index;
                                TagItem.Tag_No = data.tag_No;
                                TagItem.GoodsReceive_Index = data.goodsReceive_Index;
                                TagItem.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                                TagItem.Product_Index = item.product_Index;
                                TagItem.Product_Id = item.product_Id;
                                TagItem.Product_Name = item.product_Name;
                                TagItem.Product_SecondName = item.product_SecondName;
                                TagItem.Product_ThirdName = item.product_ThirdName;
                                if (item.product_Lot != "")
                                {
                                    TagItem.Product_Lot = item.product_Lot;
                                }
                                else
                                {
                                    TagItem.Product_Lot = "";
                                }
                                if (item.itemStatus_Index.ToString() != "00000000-0000-0000-0000-000000000000" && item.itemStatus_Index.ToString() != "")
                                {
                                    TagItem.ItemStatus_Index = item.itemStatus_Index;
                                }
                                else
                                {
                                    TagItem.ItemStatus_Index = new Guid(DataItemStatus.dataincolumn1);
                                }
                                if (item.itemStatus_Id != "" && item.itemStatus_Id != null)
                                {
                                    TagItem.ItemStatus_Id = item.itemStatus_Id;
                                }
                                else
                                {
                                    TagItem.ItemStatus_Id = DataItemStatus.dataincolumn2;
                                }
                                if (item.itemStatus_Name != "" && item.itemStatus_Name != null)
                                {
                                    TagItem.ItemStatus_Name = item.itemStatus_Name;
                                }
                                else
                                {
                                    TagItem.ItemStatus_Name = DataItemStatus.dataincolumn3;
                                }
                                TagItem.Qty = item.qty;
                                TagItem.Ratio = item.ratio;
                                TagItem.TotalQty = item.totalQty;
                                TagItem.ProductConversion_Index = item.productConversion_Index;
                                TagItem.ProductConversion_Id = item.productConversion_Id;
                                TagItem.ProductConversion_Name = item.productConversion_Name;
                                resultItem.MFG_Date = item.mFG_Date.toDate();
                                resultItem.EXP_Date = item.eXP_Date.toDate();
                                if (item.weight != null)
                                {
                                    TagItem.Weight = item.weight;
                                }
                                else
                                {
                                    TagItem.Weight = 0;
                                }
                                if (item.volume != null)
                                {
                                    TagItem.Volume = item.volume;
                                }
                                else
                                {
                                    TagItem.Volume = 0;
                                }
                                TagItem.TagRef_No1 = data.tagRef_No1;
                                TagItem.TagRef_No2 = data.tagRef_No2;
                                TagItem.TagRef_No3 = data.tagRef_No3;
                                TagItem.TagRef_No4 = data.tagRef_No4;
                                TagItem.TagRef_No5 = data.tagRef_No5;
                                TagItem.UDF_1 = item.uDF_1;
                                TagItem.UDF_2 = item.uDF_2;
                                TagItem.UDF_3 = item.uDF_3;
                                TagItem.UDF_4 = item.uDF_4;
                                TagItem.UDF_5 = item.uDF_5;
                                TagItem.Create_By = data.create_By;
                                TagItem.Create_Date = DateTime.Now;

                                db.wm_TagItem.Add(TagItem);

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
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("SaveGR", msglog);
                            transactionx.Rollback();

                            throw exy;

                        }
                        return "a";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlanGoodsReceiveItemViewModel> CheckGoodReceiveItem(PlanGoodsReceiveItemViewModel data)
        {
            try
            {
                var result = new List<PlanGoodsReceiveItemViewModel>();

                var filterModel = new PlanGoodsReceiveItemViewModel();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()) && data.planGoodsReceive_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    filterModel.planGoodsReceive_Index = data.planGoodsReceive_Index;

                }
                if (!string.IsNullOrEmpty(data.product_Index.ToString()) && data.product_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    filterModel.product_Index = data.product_Index;

                }

                //GetConfig
                result = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("getScanPlanGRI"), filterModel.sJson());

                if (result.Count > 0)
                {
                    //var GRI = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == data.planGoodsReceive_Index).ToList();
                    var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == data.planGoodsReceive_Index && c.Ref_DocumentItem_Index == result.FirstOrDefault().planGoodsReceiveItem_Index && c.Document_Status != -1).Select(a => a.TotalQty).FirstOrDefault();
                    var TotalPlanGRI = result.Select(c => c.totalQty).FirstOrDefault();
                    if (TotalGR > 0)
                    {
                        result.FirstOrDefault().totalQty = (TotalPlanGRI - TotalGR);
                    }
                }


                //var Total = (TotalPlanGRI - (TotalGR + (model.qty * model.productConversion_Ratio)));


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean GoodsReceiveConfirmFirst(GoodsReceiveConfirmViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    string pstring = " and GoodsReceive_Index = N'" + data.goodsReceive_Index + "'";
                    pstring += " and Tag_Status != -1 ";
                    var strwhere = new SqlParameter("@strwhere", pstring);

                    var queryResult = context.wm_TagItem.FromSql("sp_GetTagItem @strwhere", strwhere).ToList();

                    var listGoodsReceiveItemLocation = new List<GoodsReceiveItemLocationViewModel>();
                    var listBinBalance = new List<BinBalanceViewModel>();
                    var listBinCard = new List<BinCardViewModel>();
                    foreach (var item in queryResult)
                    {
                        var GoodsReceiveItemLocation = new GoodsReceiveItemLocationViewModel();
                        var BinBalance = new BinBalanceViewModel();
                        var BinCard = new BinCardViewModel();


                        // Prepare DATA
                        // Get Owner 
                        var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Owner_Index)");
                        var ColumnName2 = new SqlParameter("@ColumnName2", "Owner_Id");
                        var ColumnName3 = new SqlParameter("@ColumnName3", "Owner_Name");
                        var ColumnName4 = new SqlParameter("@ColumnName4", "GoodsReceive_No");
                        var ColumnName5 = new SqlParameter("@ColumnName5", "Convert(Nvarchar(50),GoodsReceive_Date,120) ");
                        var TableName = new SqlParameter("@TableName", "im_GoodsReceive");
                        var Where = new SqlParameter("@Where", "Where GoodsReceive_Index ='" + item.GoodsReceive_Index + "'");
                        var DataOwner = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                        // Get  LocationSuggestPutawayByProduct
                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
                        ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
                        ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        TableName = new SqlParameter("@TableName", "sy_SuggestPutawayByProduct");
                        Where = new SqlParameter("@Where", "Where Product_Index ='" + item.Product_Index.ToString() + "'");
                        var DataLocationSuggest = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).FirstOrDefault();


                        var SuggestLocation = DataLocationSuggest.dataincolumn1.ToString();





                        // Get Owner Location
                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Location_Index)");
                        ColumnName2 = new SqlParameter("@ColumnName2", "Location_Id");
                        ColumnName3 = new SqlParameter("@ColumnName3", "Location_Name");
                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        TableName = new SqlParameter("@TableName", "ms_Location");
                        Where = new SqlParameter("@Where", "Where Location_Index ='" + SuggestLocation.ToString() + "'");
                        var DataLocation = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();


                        // Get Owner 
                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),DocumentType_Index)");
                        ColumnName2 = new SqlParameter("@ColumnName2", "DocumentType_Id");
                        ColumnName3 = new SqlParameter("@ColumnName3", "Owner_Name");
                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        TableName = new SqlParameter("@TableName", "im_GoodsReceive");
                        Where = new SqlParameter("@Where", "Where GoodsReceive_Index ='" + item.GoodsReceive_Index + "'");
                        var DataDocumentType = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                        // Get Product 
                        ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ProductConversion_Index)");
                        ColumnName2 = new SqlParameter("@ColumnName2", "ProductConversion_Id");
                        ColumnName3 = new SqlParameter("@ColumnName3", "ProductConversion_Name");
                        ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        TableName = new SqlParameter("@TableName", "ms_Product");
                        Where = new SqlParameter("@Where", "Where Product_Index ='" + item.Product_Index + "'");
                        var DataProduct = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                        ////-------------------- GR Location --------------------
                        var GoodsReceiveItemLocation_Index = Guid.NewGuid();

                        GoodsReceiveItemLocation.goodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;
                        GoodsReceiveItemLocation.goodsReceive_Index = item.GoodsReceive_Index;
                        GoodsReceiveItemLocation.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        GoodsReceiveItemLocation.tagItem_Index = item.TagItem_Index;
                        GoodsReceiveItemLocation.tag_Index = item.Tag_Index;
                        GoodsReceiveItemLocation.tag_No = item.Tag_No;
                        GoodsReceiveItemLocation.product_Index = item.Product_Index;
                        GoodsReceiveItemLocation.product_Id = item.Product_Id;
                        GoodsReceiveItemLocation.product_Name = item.Product_Name;
                        GoodsReceiveItemLocation.product_SecondName = item.Product_SecondName;
                        GoodsReceiveItemLocation.product_ThirdName = item.Product_ThirdName;
                        GoodsReceiveItemLocation.product_Lot = item.Product_Lot;
                        GoodsReceiveItemLocation.itemStatus_Index = item.ItemStatus_Index;
                        GoodsReceiveItemLocation.itemStatus_Id = item.ItemStatus_Id;
                        GoodsReceiveItemLocation.itemStatus_Name = item.ItemStatus_Name;
                        GoodsReceiveItemLocation.productConversion_Index = new Guid(DataProduct[0].dataincolumn1);
                        GoodsReceiveItemLocation.productConversion_Id = DataProduct[0].dataincolumn2;
                        GoodsReceiveItemLocation.productConversion_Name = DataProduct[0].dataincolumn3;
                        GoodsReceiveItemLocation.mfg_Date = item.MFG_Date;
                        GoodsReceiveItemLocation.exp_Date = item.EXP_Date;
                        GoodsReceiveItemLocation.unitWeight = item.Weight / item.Qty; // item.UnitWeight;
                        GoodsReceiveItemLocation.weight = item.Weight;
                        GoodsReceiveItemLocation.unitWidth = 0;// item.UnitWidth;
                        GoodsReceiveItemLocation.unitLength = 0; //item.UnitLength;
                        GoodsReceiveItemLocation.unitHeight = 0; //item.UnitHeight;
                        GoodsReceiveItemLocation.unitVolume = 0; //item.UnitVolume;
                        GoodsReceiveItemLocation.volume = 0;//item.Volume;
                        GoodsReceiveItemLocation.unitPrice = 0; //item.UnitPrice;
                        GoodsReceiveItemLocation.price = 0; //item.Price;


                        GoodsReceiveItemLocation.owner_Index = new Guid(DataOwner[0].dataincolumn1);  //item.Owner_Index;
                        GoodsReceiveItemLocation.owner_Id = DataOwner[0].dataincolumn2; //item.Owner_Id;
                        GoodsReceiveItemLocation.owner_Name = DataOwner[0].dataincolumn3; //item.Owner_Name;


                        if (DataLocation.Count > 0)
                        {
                            GoodsReceiveItemLocation.location_Index = new Guid(DataLocation[0].dataincolumn1);//item.Location_Index;
                            GoodsReceiveItemLocation.location_Id = DataLocation[0].dataincolumn2;//item.Location_Id;
                            GoodsReceiveItemLocation.location_Name = DataLocation[0].dataincolumn3; //item.Location_Name;

                        }

                        GoodsReceiveItemLocation.qty = item.Qty;
                        GoodsReceiveItemLocation.ratio = item.Ratio;
                        GoodsReceiveItemLocation.totalQty = item.TotalQty;
                        GoodsReceiveItemLocation.udf_1 = item.UDF_1;
                        GoodsReceiveItemLocation.udf_2 = item.UDF_2;
                        GoodsReceiveItemLocation.udf_3 = item.UDF_3;
                        GoodsReceiveItemLocation.udf_4 = item.UDF_4;
                        GoodsReceiveItemLocation.udf_5 = item.UDF_5;
                        GoodsReceiveItemLocation.create_By = data.Create_By;
                        GoodsReceiveItemLocation.create_Date = item.Create_Date;
                        GoodsReceiveItemLocation.update_By = item.Update_By;
                        GoodsReceiveItemLocation.update_Date = item.Update_Date;
                        GoodsReceiveItemLocation.cancel_By = item.Cancel_By;
                        GoodsReceiveItemLocation.cancel_Date = item.Cancel_Date;
                        GoodsReceiveItemLocation.putaway_Status = 0;// item.Putaway_Status;
                        GoodsReceiveItemLocation.putaway_By = "";// item.Putaway_By;
                                                                 //   GoodsReceiveItemLocation.Putaway_Date = item.Putaway_Date;
                                                                 //   GoodsReceiveItemLocation.Suggest_Location_Index =  item.Suggest_Location_Index;

                        ////--------------------Bin Balance --------------------


                        BinBalance.BinBalance_Index = Guid.NewGuid();
                        BinBalance.Owner_Index = new Guid(DataOwner[0].dataincolumn1);//item.Owner_Index;
                        BinBalance.Owner_Id = DataOwner[0].dataincolumn2;//item.Owner_Id;
                        BinBalance.Owner_Name = DataOwner[0].dataincolumn3; // item.Owner_Name;
                        if (DataLocation.Count > 0)
                        {
                            BinBalance.Location_Index = new Guid(DataLocation[0].dataincolumn1);//item.Location_Index;
                            BinBalance.Location_Id = DataLocation[0].dataincolumn2; //item.Location_Id;
                            BinBalance.Location_Name = DataLocation[0].dataincolumn3;//item.Location_Name;
                        }
                        BinBalance.GoodsReceive_Index = item.GoodsReceive_Index;
                        BinBalance.GoodsReceive_No = DataOwner[0].dataincolumn4; //item.GoodsReceive_No;
                        DateTime oDate = DateTime.ParseExact(DataOwner[0].dataincolumn5, "yyyy-MM-dd HH:mm:ss", null);
                        BinBalance.GoodsReceive_Date = oDate;  //item.GoodsReceive_Date;
                        BinBalance.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        BinBalance.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                        BinBalance.TagItem_Index = item.TagItem_Index;
                        BinBalance.Tag_Index = new Guid(item.Tag_Index.ToString());
                        BinBalance.Tag_No = item.Tag_No;
                        BinBalance.Product_Index = item.Product_Index;
                        BinBalance.Product_Id = item.Product_Id;
                        BinBalance.Product_Name = item.Product_Name;
                        BinBalance.Product_SecondName = item.Product_SecondName;
                        BinBalance.Product_ThirdName = item.Product_ThirdName;
                        BinBalance.Product_Lot = item.Product_Lot;
                        BinBalance.ItemStatus_Index = item.ItemStatus_Index;
                        BinBalance.ItemStatus_Id = item.ItemStatus_Id;
                        BinBalance.ItemStatus_Name = item.ItemStatus_Name;
                        BinBalance.GoodsReceive_MFG_Date = item.MFG_Date;
                        BinBalance.GoodsReceive_EXP_Date = item.EXP_Date;
                        BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
                        BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
                        BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
                        BinBalance.BinBalance_Ratio = item.Ratio;
                        BinBalance.BinBalance_QtyBegin = item.TotalQty;
                        BinBalance.BinBalance_WeightBegin = item.Weight;
                        BinBalance.BinBalance_VolumeBegin = item.Volume;
                        BinBalance.BinBalance_QtyBal = item.TotalQty;
                        BinBalance.BinBalance_WeightBal = item.Weight;
                        BinBalance.BinBalance_VolumeBal = item.Volume;
                        BinBalance.BinBalance_QtyReserve = 0;
                        BinBalance.BinBalance_WeightReserve = 0;
                        BinBalance.BinBalance_VolumeReserve = 0;
                        BinBalance.ProductConversion_Index = new Guid(DataProduct[0].dataincolumn1);
                        BinBalance.ProductConversion_Id = DataProduct[0].dataincolumn2;
                        BinBalance.ProductConversion_Name = DataProduct[0].dataincolumn3;
                        BinBalance.UDF_1 = item.UDF_1;
                        BinBalance.UDF_2 = item.UDF_2;
                        BinBalance.UDF_3 = item.UDF_3;
                        BinBalance.UDF_4 = item.UDF_4;
                        BinBalance.UDF_5 = item.UDF_5;
                        BinBalance.Create_By = data.Create_By;
                        BinBalance.Create_Date = item.Create_Date;
                        BinBalance.Update_By = item.Update_By;
                        BinBalance.Update_Date = item.Update_Date;
                        BinBalance.Cancel_By = item.Cancel_By;
                        BinBalance.Cancel_Date = item.Cancel_Date;


                        ////--------------------Bin Card --------------------

                        BinCard.BinCard_Index = Guid.NewGuid();
                        BinCard.Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");//item.Process_Index;
                        BinCard.DocumentType_Index = new Guid(DataDocumentType[0].dataincolumn1); //item.DocumentType_Index;
                        BinCard.DocumentType_Id = DataDocumentType[0].dataincolumn2;//item.DocumentType_Id;
                        BinCard.DocumentType_Name = DataDocumentType[0].dataincolumn3;//item.DocumentType_Name;
                        BinCard.GoodsReceive_Index = item.GoodsReceive_Index;
                        BinCard.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                        BinCard.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                        BinCard.BinCard_No = DataOwner[0].dataincolumn4; //item.BinCard_No;
                        BinCard.BinCard_Date = oDate; //item.BinCard_Date;
                        BinCard.TagItem_Index = item.TagItem_Index;
                        BinCard.Tag_Index = item.Tag_Index;
                        BinCard.Tag_No = item.Tag_No;
                        BinCard.Tag_Index_To = item.TagItem_Index; //item.Tag_Index_To;
                        BinCard.Tag_No_To = item.Tag_No; //item.Tag_No_To;
                        BinCard.Product_Index = item.Product_Index;
                        BinCard.Product_Id = item.Product_Id;
                        BinCard.Product_Name = item.Product_Name;
                        BinCard.Product_SecondName = item.Product_SecondName;
                        BinCard.Product_ThirdName = item.Product_ThirdName;
                        BinCard.Product_Index_To = item.Product_Index; //item.Product_Index_To;
                        BinCard.Product_Id_To = item.Product_Id;
                        BinCard.Product_Name_To = item.Product_Name;
                        BinCard.Product_SecondName_To = item.Product_SecondName;
                        BinCard.Product_ThirdName_To = item.Product_ThirdName;
                        BinCard.Product_Lot = item.Product_Lot;
                        BinCard.Product_Lot_To = item.Product_Lot;
                        BinCard.ItemStatus_Index = item.ItemStatus_Index;
                        BinCard.ItemStatus_Id = item.ItemStatus_Id;
                        BinCard.ItemStatus_Name = item.ItemStatus_Name;
                        BinCard.ItemStatus_Index_To = item.ItemStatus_Index;
                        BinCard.ItemStatus_Id_To = item.ItemStatus_Id;
                        BinCard.ItemStatus_Name_To = item.ItemStatus_Name;
                        BinCard.ProductConversion_Index = new Guid(DataProduct[0].dataincolumn1);
                        BinCard.ProductConversion_Id = DataProduct[0].dataincolumn2;
                        BinCard.ProductConversion_Name = DataProduct[0].dataincolumn3;
                        BinCard.Owner_Index = new Guid(DataOwner[0].dataincolumn1);//item.Owner_Index;
                        BinCard.Owner_Id = DataOwner[0].dataincolumn2;//item.Owner_Id;
                        BinCard.Owner_Name = DataOwner[0].dataincolumn3; // item.Owner_Name;

                        BinCard.Owner_Index_To = new Guid(DataOwner[0].dataincolumn1);
                        BinCard.Owner_Id_To = DataOwner[0].dataincolumn2;
                        BinCard.Owner_Name_To = DataOwner[0].dataincolumn3;
                        if (DataLocation.Count > 0)
                        {
                            BinCard.Location_Index = new Guid(DataLocation[0].dataincolumn1);//item.Location_Index;
                            BinCard.Location_Id = DataLocation[0].dataincolumn2; //item.Location_Id;
                            BinCard.Location_Name = DataLocation[0].dataincolumn3;//item.Location_Name;
                            BinCard.Location_Index_To = new Guid(DataLocation[0].dataincolumn1);
                            BinCard.Location_Id_To = DataLocation[0].dataincolumn2;
                            BinCard.Location_Name_To = DataLocation[0].dataincolumn3;
                        }
                        BinCard.GoodsReceive_EXP_Date = item.EXP_Date;
                        BinCard.GoodsReceive_EXP_Date_To = item.EXP_Date;
                        BinCard.BinCard_QtyIn = item.TotalQty;
                        BinCard.BinCard_QtyOut = 0;
                        BinCard.BinCard_QtySign = item.TotalQty;
                        BinCard.BinCard_WeightIn = item.Weight;
                        BinCard.BinCard_WeightOut = 0;
                        BinCard.BinCard_WeightSign = item.Weight;
                        BinCard.BinCard_VolumeIn = item.Volume;
                        BinCard.BinCard_VolumeOut = 0;
                        BinCard.BinCard_VolumeSign = item.Volume;
                        BinCard.Ref_Document_No = DataOwner[0].dataincolumn4;
                        BinCard.Ref_Document_Index = item.GoodsReceive_Index; //tem.Ref_Document_Index;
                        BinCard.Ref_DocumentItem_Index = item.GoodsReceiveItem_Index;
                        BinCard.Create_By = data.Create_By;
                        BinCard.Create_Date = item.Create_Date;

                        ////------------------------------------------------
                        listGoodsReceiveItemLocation.Add(GoodsReceiveItemLocation);

                        listBinBalance.Add(BinBalance);
                        listBinCard.Add(BinCard);
                    }


                    DataTable dtDetailLocation = CreateDataTable(listGoodsReceiveItemLocation);

                    DataTable dtBinBalance = CreateDataTable(listBinBalance);

                    DataTable dtBinCard = CreateDataTable(listBinCard);


                    var pGoodsReceiveItemLocation = new SqlParameter("GoodsReceiveItemLocation", SqlDbType.Structured);
                    pGoodsReceiveItemLocation.TypeName = "[dbo].[im_GoodsReceiveItemLocationData]";
                    pGoodsReceiveItemLocation.Value = dtDetailLocation;

                    var pBinBalance = new SqlParameter("BinBalance", SqlDbType.Structured);
                    pBinBalance.TypeName = "[dbo].[wm_BinBalanceData]";
                    pBinBalance.Value = dtBinBalance;

                    var pBinCard = new SqlParameter("BinCard", SqlDbType.Structured);
                    pBinCard.TypeName = "[dbo].[wm_BinCardData]";
                    pBinCard.Value = dtBinCard;
                    //
                    var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", pGoodsReceiveItemLocation, pBinBalance, pBinCard);

                    if (rowsAffected.ToString() != "0")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //  return rowsAffected.ToString();



                    //   return result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GoodsReceiveConfirmV3Auto()
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {
                var Models = new GoodsReceiveConfirmViewModel();
                var queryView = db.IM_GoodsReceive.AsQueryable();
                queryView = queryView.Where(c => c.Create_By == "ImportStock");
                // context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status == 2)

                string msgretrun = "";
                var listdataView = queryView.ToList();

                foreach (var item in listdataView)
                {
                    Models.goodsReceive_Index = item.GoodsReceive_Index;
                    Models.Create_By = item.Create_By;

                    msgretrun += item.GoodsReceive_No + " : ";
                    msgretrun += GoodsReceiveConfirmV3(Models);
                    msgretrun += Environment.NewLine;
                }




                return msgretrun;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean GoodsReceiveConfirmUnPack(GoodsReceiveConfirmViewModel data)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var contextGRBinbalance = new GRBinBalanceDbContext();

            try
            {
                using (var context = new GRDbContext())
                {
                    olog.logging("GoodsReceiveConfirm", "Start");

                    context.Database.SetCommandTimeout(3600);
                    var chkStatus = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status == 2).Any();
                    if (chkStatus == true)
                    {
                        olog.logging("GoodsReceiveConfirm", "chkStatus_fasle");

                        return false;
                    }
                    else
                    {
                        olog.logging("GoodsReceiveConfirm", "chkStatus_true");
                        olog.logging("GoodsReceiveConfirm", "sp_GetTagItem_GrConfirm");
                        var queryResult = context.wm_TagItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Tag_Status != -1).ToList();


                        if (queryResult.Count <= 0)
                        {
                            return false;
                        }

                        // Check Gentag isComplete
                        var lstGoodsReceiveNotComplete = db.View_GoodsReceiveWithTag.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status != -1 && c.IsCompleted == 0).ToList();

                        if (lstGoodsReceiveNotComplete.Count > 0)
                        {
                            return false;
                        }

                        var listGoodsReceiveItemLocation = new List<GoodsReceiveItemLocationViewModel>();
                        var listBinBalance = new List<BinBalanceViewModel>();
                        var listBinCard = new List<BinCardViewModel>();
                        IM_GoodsReceiveItem oldGoodsReceiveItem = new IM_GoodsReceiveItem();

                        foreach (var item in queryResult)
                        {
                            IM_GoodsReceiveItemLocation GoodsReceiveItemLocation = new IM_GoodsReceiveItemLocation();

                            WM_BinBalance BinBalance = new WM_BinBalance();
                            WM_BinCard BinCard = new WM_BinCard();

                            var GoodsReceive = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).FirstOrDefault();

                            var GoodsReceiveItem = context.IM_GoodsReceiveItem.Where(c => c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index).FirstOrDefault();


                            #region Config Suggest Location
                            var LocationIndex = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_GR_Staging" }.sJson());
                            #endregion

                            #region Get Location

                            var LocationViewModel = new { location_Index = new Guid(LocationIndex) };
                            var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LocationViewModel.sJson());

                            var DataLocation = GetLocation.FirstOrDefault();
                            #endregion


                            #region Get Product
                            var ProductViewModel = new ProductViewModel();
                            ProductViewModel.product_Index = item.Product_Index;
                            var GetProduct = utils.SendDataApi<List<ProductModel>>(new AppSettingConfig().GetUrl("getProduct"), ProductViewModel.sJson());

                            var DataProduct = GetProduct.FirstOrDefault();
                            #endregion

                            ////-------------------- GR Location --------------------
                            var GoodsReceiveItemLocation_Index = Guid.NewGuid();

                            GoodsReceiveItemLocation.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;
                            GoodsReceiveItemLocation.GoodsReceive_Index = item.GoodsReceive_Index;
                            GoodsReceiveItemLocation.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                            GoodsReceiveItemLocation.TagItem_Index = item.TagItem_Index;
                            GoodsReceiveItemLocation.Tag_Index = item.Tag_Index;
                            GoodsReceiveItemLocation.Tag_No = item.Tag_No;
                            GoodsReceiveItemLocation.Product_Index = item.Product_Index;
                            GoodsReceiveItemLocation.Product_Id = item.Product_Id;
                            GoodsReceiveItemLocation.Product_Name = item.Product_Name;
                            GoodsReceiveItemLocation.Product_SecondName = item.Product_SecondName;
                            GoodsReceiveItemLocation.Product_ThirdName = item.Product_ThirdName;
                            GoodsReceiveItemLocation.Product_Lot = item.Product_Lot;
                            GoodsReceiveItemLocation.ItemStatus_Index = item.ItemStatus_Index;
                            GoodsReceiveItemLocation.ItemStatus_Id = item.ItemStatus_Id;
                            GoodsReceiveItemLocation.ItemStatus_Name = item.ItemStatus_Name;
                            GoodsReceiveItemLocation.ProductConversion_Index = DataProduct.productConversion_Index;
                            GoodsReceiveItemLocation.ProductConversion_Id = DataProduct.productConversion_Id;
                            GoodsReceiveItemLocation.ProductConversion_Name = DataProduct.productConversion_Name;
                            GoodsReceiveItemLocation.MFG_Date = item.MFG_Date;
                            GoodsReceiveItemLocation.EXP_Date = item.EXP_Date;

                            GoodsReceiveItemLocation.WeightRatio = item.WeightRatio;
                            GoodsReceiveItemLocation.UnitWeight = item.UnitWeight;
                            GoodsReceiveItemLocation.Weight = item.Weight;
                            GoodsReceiveItemLocation.Weight_Index = item.Weight_Index;
                            GoodsReceiveItemLocation.Weight_Id = item.Weight_Id;
                            GoodsReceiveItemLocation.Weight_Name = item.Weight_Name;
                            GoodsReceiveItemLocation.NetWeight = item.NetWeight;

                            GoodsReceiveItemLocation.GrsWeightRatio = item.GrsWeightRatio;
                            GoodsReceiveItemLocation.UnitGrsWeight = item.UnitGrsWeight;
                            GoodsReceiveItemLocation.GrsWeight = item.GrsWeight;
                            GoodsReceiveItemLocation.GrsWeight_Index = item.GrsWeight_Index;
                            GoodsReceiveItemLocation.GrsWeight_Id = item.GrsWeight_Id;
                            GoodsReceiveItemLocation.GrsWeight_Name = item.GrsWeight_Name;

                            GoodsReceiveItemLocation.WidthRatio = item.WidthRatio;
                            GoodsReceiveItemLocation.UnitWidth = item.UnitWidth;
                            GoodsReceiveItemLocation.Width = item.Width;
                            GoodsReceiveItemLocation.Width_Index = item.Width_Index;
                            GoodsReceiveItemLocation.Width_Id = item.Width_Id;
                            GoodsReceiveItemLocation.Width_Name = item.Width_Name;

                            GoodsReceiveItemLocation.LengthRatio = item.LengthRatio;
                            GoodsReceiveItemLocation.UnitLength = item.UnitLength;
                            GoodsReceiveItemLocation.Length = item.Length;
                            GoodsReceiveItemLocation.Length_Index = item.Length_Index;
                            GoodsReceiveItemLocation.Length_Id = item.Length_Id;
                            GoodsReceiveItemLocation.Length_Name = item.Length_Name;

                            GoodsReceiveItemLocation.HeightRatio = item.HeightRatio;
                            GoodsReceiveItemLocation.UnitHeight = item.UnitHeight;
                            GoodsReceiveItemLocation.Height = item.Height;
                            GoodsReceiveItemLocation.Height_Index = item.Height_Index;
                            GoodsReceiveItemLocation.Height_Id = item.Height_Id;
                            GoodsReceiveItemLocation.Height_Name = item.Height_Name;

                            GoodsReceiveItemLocation.UnitVolume = item.UnitVolume;
                            GoodsReceiveItemLocation.Volume = item.Volume;


                            GoodsReceiveItemLocation.UnitPrice = item.UnitPrice;
                            GoodsReceiveItemLocation.Price = item.Price;
                            GoodsReceiveItemLocation.TotalPrice = item.TotalPrice;

                            GoodsReceiveItemLocation.Currency_Index = item.Currency_Index;
                            GoodsReceiveItemLocation.Currency_Id = item.Currency_Id;
                            GoodsReceiveItemLocation.Currency_Name = item.Currency_Name;

                            GoodsReceiveItemLocation.Ref_Code1 = item.Ref_Code1;
                            GoodsReceiveItemLocation.Ref_Code2 = item.Ref_Code2;
                            GoodsReceiveItemLocation.Ref_Code3 = item.Ref_Code3;
                            GoodsReceiveItemLocation.Ref_Code4 = item.Ref_Code4;
                            GoodsReceiveItemLocation.Ref_Code5 = item.Ref_Code5;


                            GoodsReceiveItemLocation.Owner_Index = GoodsReceive.Owner_Index;
                            GoodsReceiveItemLocation.Owner_Id = GoodsReceive.Owner_Id;
                            GoodsReceiveItemLocation.Owner_Name = GoodsReceive.Owner_Name;


                            if (GetLocation.Count > 0)
                            {
                                //DataLocation.select(c => c.Location_Index).sParse<Guid?>();
                                GoodsReceiveItemLocation.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                GoodsReceiveItemLocation.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                GoodsReceiveItemLocation.Location_Name = DataLocation.location_Name; //item.Location_Name;

                            }

                            GoodsReceiveItemLocation.Qty = item.Qty;
                            GoodsReceiveItemLocation.Ratio = item.Ratio;
                            GoodsReceiveItemLocation.TotalQty = item.TotalQty;
                            GoodsReceiveItemLocation.UDF_1 = item.UDF_1;
                            GoodsReceiveItemLocation.UDF_2 = item.UDF_2;
                            GoodsReceiveItemLocation.UDF_3 = item.UDF_3;
                            GoodsReceiveItemLocation.UDF_4 = item.UDF_4;
                            GoodsReceiveItemLocation.UDF_5 = item.UDF_5;
                            GoodsReceiveItemLocation.Create_By = data.Create_By;
                            GoodsReceiveItemLocation.Create_Date = DateTime.Now;
                            GoodsReceiveItemLocation.Update_By = item.Update_By;
                            //GoodsReceiveItemLocation.Update_Date = item.Update_Date;
                            GoodsReceiveItemLocation.Cancel_By = item.Cancel_By;
                            //GoodsReceiveItemLocation.Cancel_Date = item.Cancel_Date;
                            //GoodsReceiveItemLocation.Putaway_By = "";// item.Putaway_By;
                            GoodsReceiveItemLocation.Putaway_Status = 0;
                            //GoodsReceiveItemLocation.Putaway_Date = DateTime.Now;
                            GoodsReceiveItemLocation.ERP_Location = item.ERP_Location;


                            ////--------------------Bin Balance --------------------


                            BinBalance.BinBalance_Index = Guid.NewGuid();
                            BinBalance.Owner_Index = GoodsReceive.Owner_Index;//item.Owner_Index;
                            BinBalance.Owner_Id = GoodsReceive.Owner_Id;//item.Owner_Id;
                            BinBalance.Owner_Name = GoodsReceive.Owner_Name; // item.Owner_Name;
                            if (GetLocation.Count > 0)
                            {
                                BinBalance.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                BinBalance.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                BinBalance.Location_Name = DataLocation.location_Name; //item.Location_Name;
                            }
                            BinBalance.GoodsReceive_Index = item.GoodsReceive_Index.GetValueOrDefault();
                            BinBalance.GoodsReceive_No = GoodsReceive.GoodsReceive_No; ; //item.GoodsReceive_No;
                            //DateTime oDate = DateTime.ParseExact(GoodsReceive.GoodsReceive_Date.toString(), "yyyy-MM-dd HH:mm:ss", null);
                            BinBalance.GoodsReceive_Date = GoodsReceive.GoodsReceive_Date.sParse<DateTime>();  //item.GoodsReceive_Date;
                            BinBalance.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index.GetValueOrDefault();
                            BinBalance.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                            BinBalance.TagItem_Index = item.TagItem_Index.sParse<Guid>();
                            BinBalance.Tag_Index = new Guid(item.Tag_Index.ToString());
                            BinBalance.Tag_No = item.Tag_No;
                            BinBalance.Product_Index = item.Product_Index;
                            BinBalance.Product_Id = item.Product_Id;
                            BinBalance.Product_Name = item.Product_Name;
                            BinBalance.Product_SecondName = item.Product_SecondName;
                            BinBalance.Product_ThirdName = item.Product_ThirdName;
                            BinBalance.Product_Lot = item.Product_Lot;
                            BinBalance.ItemStatus_Index = item.ItemStatus_Index.GetValueOrDefault();
                            BinBalance.ItemStatus_Id = item.ItemStatus_Id;
                            BinBalance.ItemStatus_Name = item.ItemStatus_Name;
                            BinBalance.GoodsReceive_MFG_Date = item.MFG_Date;
                            BinBalance.GoodsReceive_EXP_Date = item.EXP_Date;
                            BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
                            BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
                            BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
                            BinBalance.BinBalance_Ratio = 1;
                            BinBalance.BinBalance_QtyBegin = item.TotalQty;

                            #region Begin 
                            BinBalance.BinBalance_WeightBegin = item.Weight;
                            BinBalance.BinBalance_WeightBegin_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightBegin_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightBegin_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightBeginRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightBegin = item.NetWeight;
                            BinBalance.BinBalance_NetWeightBegin_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightBegin_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightBegin_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightBeginRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightBegin = item.GrsWeight;
                            BinBalance.BinBalance_GrsWeightBegin_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightBegin_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightBegin_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightBeginRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthBegin = item.Width;
                            BinBalance.BinBalance_WidthBegin_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthBegin_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthBegin_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthBeginRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthBegin = item.Length;
                            BinBalance.BinBalance_LengthBegin_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthBegin_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthBegin_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthBeginRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightBegin = item.Height;
                            BinBalance.BinBalance_HeightBegin_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightBegin_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightBegin_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightBeginRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeBegin = item.UnitVolume;
                            BinBalance.BinBalance_VolumeBegin = item.Volume;
                            #endregion

                            #region Bal
                            BinBalance.BinBalance_QtyBal = item.TotalQty;

                            BinBalance.BinBalance_WeightBal = item.Weight;
                            BinBalance.BinBalance_WeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_UnitWeightBal = item.UnitWeight;
                            BinBalance.BinBalance_UnitWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_UnitWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_UnitWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_UnitWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightBal = item.NetWeight;
                            BinBalance.BinBalance_NetWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_UnitNetWeightBal = item.UnitWeight;
                            BinBalance.BinBalance_UnitNetWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_UnitNetWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_UnitNetWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_UnitNetWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightBal = item.GrsWeight;
                            BinBalance.BinBalance_GrsWeightBal_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightBal_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightBal_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightBalRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_UnitGrsWeightBal = item.UnitGrsWeight;
                            BinBalance.BinBalance_UnitGrsWeightBal_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_UnitGrsWeightBal_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_UnitGrsWeightBal_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_UnitGrsWeightBalRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthBal = item.Width;
                            BinBalance.BinBalance_WidthBal_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthBal_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthBal_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthBalRatio = item.WidthRatio;

                            BinBalance.BinBalance_UnitWidthBal = item.UnitWidth;
                            BinBalance.BinBalance_UnitWidthBal_Index = item.Width_Index;
                            BinBalance.BinBalance_UnitWidthBal_Id = item.Width_Id;
                            BinBalance.BinBalance_UnitWidthBal_Name = item.Width_Name;
                            BinBalance.BinBalance_UnitWidthBalRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthBal = item.Length;
                            BinBalance.BinBalance_UnitLengthBal = item.UnitLength;
                            BinBalance.BinBalance_LengthBal_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthBal_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthBal_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthBalRatio = item.LengthRatio;

                            BinBalance.BinBalance_UnitLengthBal = item.UnitLength;
                            BinBalance.BinBalance_UnitLengthBal_Index = item.Length_Index;
                            BinBalance.BinBalance_UnitLengthBal_Id = item.Length_Id;
                            BinBalance.BinBalance_UnitLengthBal_Name = item.Length_Name;
                            BinBalance.BinBalance_UnitLengthBalRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightBal = item.Height;
                            BinBalance.BinBalance_UnitHeightBal = item.UnitHeight;
                            BinBalance.BinBalance_HeightBal_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightBal_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightBal_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightBalRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitHeightBal = item.UnitHeight;
                            BinBalance.BinBalance_UnitHeightBal_Index = item.Height_Index;
                            BinBalance.BinBalance_UnitHeightBal_Id = item.Height_Id;
                            BinBalance.BinBalance_UnitHeightBal_Name = item.Height_Name;
                            BinBalance.BinBalance_UnitHeightBalRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeBal = item.UnitVolume;
                            BinBalance.BinBalance_VolumeBal = item.Volume;
                            #endregion

                            #region Reserve
                            BinBalance.BinBalance_QtyReserve = 0;

                            BinBalance.BinBalance_WeightReserve = 0;
                            BinBalance.BinBalance_WeightReserveRatio = item.WeightRatio;
                            BinBalance.BinBalance_WeightReserve_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightReserve_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightReserve_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightReserveRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightReserve = 0;
                            BinBalance.BinBalance_NetWeightReserve_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightReserve_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightReserve_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightReserveRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightReserve = 0;
                            BinBalance.BinBalance_GrsWeightReserve_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightReserve_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightReserve_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightReserveRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthReserve = 0;
                            BinBalance.BinBalance_WidthReserve_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthReserve_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthReserve_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthReserveRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthReserve = 0;
                            BinBalance.BinBalance_LengthReserve_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthReserve_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthReserve_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthReserveRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightReserve = 0;
                            BinBalance.BinBalance_HeightReserve_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightReserve_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightReserve_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightReserveRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeReserve = 0;
                            BinBalance.BinBalance_VolumeReserve = 0;
                            #endregion



                            BinBalance.Invoice_No = GoodsReceiveItem.Invoice_No;
                            BinBalance.Declaration_No = GoodsReceiveItem.Declaration_No;
                            BinBalance.HS_Code = GoodsReceiveItem.HS_Code;
                            BinBalance.Conutry_of_Origin = GoodsReceiveItem.Conutry_of_Origin;

                            BinBalance.Tax1 = GoodsReceiveItem.Tax1;
                            BinBalance.Tax1_Currency_Index = GoodsReceiveItem.Tax1_Currency_Index;
                            BinBalance.Tax1_Currency_Id = GoodsReceiveItem.Tax1_Currency_Id;
                            BinBalance.Tax1_Currency_Name = GoodsReceiveItem.Tax1_Currency_Name;

                            BinBalance.Tax2 = GoodsReceiveItem.Tax2;
                            BinBalance.Tax2_Currency_Index = GoodsReceiveItem.Tax2_Currency_Index;
                            BinBalance.Tax2_Currency_Id = GoodsReceiveItem.Tax2_Currency_Id;
                            BinBalance.Tax2_Currency_Name = GoodsReceiveItem.Tax2_Currency_Name;

                            BinBalance.Tax3 = GoodsReceiveItem.Tax3;
                            BinBalance.Tax3_Currency_Index = GoodsReceiveItem.Tax3_Currency_Index;
                            BinBalance.Tax3_Currency_Id = GoodsReceiveItem.Tax3_Currency_Id;
                            BinBalance.Tax3_Currency_Name = GoodsReceiveItem.Tax3_Currency_Name;

                            BinBalance.Tax4 = GoodsReceiveItem.Tax4;
                            BinBalance.Tax4_Currency_Index = GoodsReceiveItem.Tax4_Currency_Index;
                            BinBalance.Tax4_Currency_Id = GoodsReceiveItem.Tax4_Currency_Id;
                            BinBalance.Tax4_Currency_Name = GoodsReceiveItem.Tax4_Currency_Name;

                            BinBalance.Tax5 = GoodsReceiveItem.Tax5;
                            BinBalance.Tax5_Currency_Index = GoodsReceiveItem.Tax5_Currency_Index;
                            BinBalance.Tax5_Currency_Id = GoodsReceiveItem.Tax5_Currency_Id;
                            BinBalance.Tax5_Currency_Name = GoodsReceiveItem.Tax5_Currency_Name;


                            BinBalance.ProductConversion_Index = DataProduct.productConversion_Index;
                            BinBalance.ProductConversion_Id = DataProduct.productConversion_Id;
                            BinBalance.ProductConversion_Name = DataProduct.productConversion_Name;
                            BinBalance.UDF_1 = item.UDF_1;
                            BinBalance.UDF_2 = item.UDF_2;
                            BinBalance.UDF_3 = item.UDF_3;
                            BinBalance.UDF_4 = item.UDF_4;
                            BinBalance.UDF_5 = item.UDF_5;
                            BinBalance.Create_By = data.Create_By;
                            BinBalance.Create_Date = DateTime.Now;
                            BinBalance.Update_By = item.Update_By;
                            //BinBalance.Update_Date = item.Update_Date;
                            BinBalance.Cancel_By = item.Cancel_By;
                            //BinBalance.Cancel_Date = item.Cancel_Date;
                            BinBalance.ERP_Location = item.ERP_Location;


                            ////--------------------Bin Card --------------------

                            BinCard.BinCard_Index = Guid.NewGuid();
                            BinCard.Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");//item.Process_Index;
                            BinCard.DocumentType_Index = GoodsReceive.DocumentType_Index; //item.DocumentType_Index;
                            BinCard.DocumentType_Id = GoodsReceive.DocumentType_Id;//item.DocumentType_Id;
                            BinCard.DocumentType_Name = GoodsReceive.DocumentType_Name;//item.DocumentType_Name;
                            BinCard.GoodsReceive_Index = item.GoodsReceive_Index;
                            BinCard.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                            BinCard.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                            BinCard.BinCard_No = GoodsReceive.GoodsReceive_No; ; //item.BinCard_No;
                            BinCard.BinCard_Date = GoodsReceive.GoodsReceive_Date.sParse<DateTime>(); ; //item.BinCard_Date;
                            BinCard.TagItem_Index = item.TagItem_Index;
                            BinCard.Tag_Index = item.Tag_Index;
                            BinCard.Tag_No = item.Tag_No;
                            BinCard.Tag_Index_To = item.TagItem_Index; //item.Tag_Index_To;
                            BinCard.Tag_No_To = item.Tag_No; //item.Tag_No_To;
                            BinCard.Product_Index = item.Product_Index;
                            BinCard.Product_Id = item.Product_Id;
                            BinCard.Product_Name = item.Product_Name;
                            BinCard.Product_SecondName = item.Product_SecondName;
                            BinCard.Product_ThirdName = item.Product_ThirdName;
                            BinCard.Product_Index_To = item.Product_Index; //item.Product_Index_To;
                            BinCard.Product_Id_To = item.Product_Id;
                            BinCard.Product_Name_To = item.Product_Name;
                            BinCard.Product_SecondName_To = item.Product_SecondName;
                            BinCard.Product_ThirdName_To = item.Product_ThirdName;
                            BinCard.Product_Lot = item.Product_Lot;
                            BinCard.Product_Lot_To = item.Product_Lot;
                            BinCard.ItemStatus_Index = item.ItemStatus_Index;
                            BinCard.ItemStatus_Id = item.ItemStatus_Id;
                            BinCard.ItemStatus_Name = item.ItemStatus_Name;
                            BinCard.ItemStatus_Index_To = item.ItemStatus_Index;
                            BinCard.ItemStatus_Id_To = item.ItemStatus_Id;
                            BinCard.ItemStatus_Name_To = item.ItemStatus_Name;
                            BinCard.ProductConversion_Index = DataProduct.productConversion_Index;
                            BinCard.ProductConversion_Id = DataProduct.productConversion_Id;
                            BinCard.ProductConversion_Name = DataProduct.productConversion_Name;
                            BinCard.Owner_Index = GoodsReceive.Owner_Index;//item.Owner_Index;
                            BinCard.Owner_Id = GoodsReceive.Owner_Id;//item.Owner_Id;
                            BinCard.Owner_Name = GoodsReceive.Owner_Name; // item.Owner_Name;

                            BinCard.Owner_Index_To = GoodsReceive.Owner_Index;
                            BinCard.Owner_Id_To = GoodsReceive.Owner_Id;
                            BinCard.Owner_Name_To = GoodsReceive.Owner_Name;
                            if (GetLocation.Count > 0)
                            {
                                BinCard.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                BinCard.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                BinCard.Location_Name = DataLocation.location_Name; //item.Location_Name;
                                BinCard.Location_Index_To = DataLocation.location_Index;
                                BinCard.Location_Id_To = DataLocation.location_Id;
                                BinCard.Location_Name_To = DataLocation.location_Name;
                            }
                            BinCard.GoodsReceive_EXP_Date = item.EXP_Date;
                            BinCard.GoodsReceive_EXP_Date_To = item.EXP_Date;

                            #region In

                            BinCard.BinCard_QtyIn = item.TotalQty;

                            BinCard.BinCard_UnitWeightIn = item.UnitWeight;
                            BinCard.BinCard_UnitWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_UnitWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_UnitWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_UnitWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_WeightIn = item.Weight;
                            BinCard.BinCard_WeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_WeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_WeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_WeightInRatio = item.WeightRatio;

                            BinCard.BinCard_UnitNetWeightIn = item.UnitWeight;
                            BinCard.BinCard_UnitNetWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_UnitNetWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_UnitNetWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_UnitNetWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_NetWeightIn = item.NetWeight;
                            BinCard.BinCard_NetWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_NetWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_NetWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_NetWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_UnitGrsWeightIn = item.UnitGrsWeight;
                            BinCard.BinCard_UnitGrsWeightIn_Index = item.GrsWeight_Index;
                            BinCard.BinCard_UnitGrsWeightIn_Id = item.GrsWeight_Id;
                            BinCard.BinCard_UnitGrsWeightIn_Name = item.GrsWeight_Name;
                            BinCard.BinCard_UnitGrsWeightInRatio = item.GrsWeightRatio;

                            BinCard.BinCard_GrsWeightIn = item.GrsWeight;
                            BinCard.BinCard_GrsWeightIn_Index = item.GrsWeight_Index;
                            BinCard.BinCard_GrsWeightIn_Id = item.GrsWeight_Id;
                            BinCard.BinCard_GrsWeightIn_Name = item.GrsWeight_Name;
                            BinCard.BinCard_GrsWeightInRatio = item.GrsWeightRatio;

                            BinCard.BinCard_UnitWidthIn = item.UnitWidth;
                            BinCard.BinCard_UnitWidthIn_Index = item.Width_Index;
                            BinCard.BinCard_UnitWidthIn_Id = item.Width_Id;
                            BinCard.BinCard_UnitWidthIn_Name = item.Width_Name;
                            BinCard.BinCard_UnitWidthInRatio = item.WidthRatio;

                            BinCard.BinCard_WidthIn = item.Width;
                            BinCard.BinCard_WidthIn_Index = item.Width_Index;
                            BinCard.BinCard_WidthIn_Id = item.Width_Id;
                            BinCard.BinCard_WidthIn_Name = item.Width_Name;
                            BinCard.BinCard_WidthInRatio = item.WidthRatio;

                            BinCard.BinCard_UnitLengthIn = item.UnitLength;
                            BinCard.BinCard_UnitLengthIn_Index = item.Length_Index;
                            BinCard.BinCard_UnitLengthIn_Id = item.Length_Id;
                            BinCard.BinCard_UnitLengthIn_Name = item.Length_Name;
                            BinCard.BinCard_UnitLengthInRatio = item.LengthRatio;

                            BinCard.BinCard_LengthIn = item.Length;
                            BinCard.BinCard_LengthIn_Index = item.Length_Index;
                            BinCard.BinCard_LengthIn_Id = item.Length_Id;
                            BinCard.BinCard_LengthIn_Name = item.Length_Name;
                            BinCard.BinCard_LengthInRatio = item.LengthRatio;

                            BinCard.BinCard_UnitHeightIn = item.UnitHeight;
                            BinCard.BinCard_UnitHeightIn_Index = item.Height_Index;
                            BinCard.BinCard_UnitHeightIn_Id = item.Height_Id;
                            BinCard.BinCard_UnitHeightIn_Name = item.Height_Name;
                            BinCard.BinCard_UnitHeightInRatio = item.HeightRatio;

                            BinCard.BinCard_HeightIn = item.Height;
                            BinCard.BinCard_HeightIn_Index = item.Height_Index;
                            BinCard.BinCard_HeightIn_Id = item.Height_Id;
                            BinCard.BinCard_HeightIn_Name = item.Height_Name;
                            BinCard.BinCard_HeightInRatio = item.HeightRatio;

                            BinCard.BinCard_UnitVolumeIn = item.UnitVolume;
                            BinCard.BinCard_VolumeIn = item.Volume;

                            #endregion

                            #region Out

                            BinCard.BinCard_QtyOut = 0;

                            BinCard.BinCard_UnitWeightOut = 0;
                            BinCard.BinCard_UnitWeightOutRatio = 0;

                            BinCard.BinCard_WeightOut = 0;
                            BinCard.BinCard_WeightOutRatio = 0;

                            BinCard.BinCard_UnitNetWeightOut = 0;
                            BinCard.BinCard_UnitNetWeightOutRatio = 0;

                            BinCard.BinCard_NetWeightOut = 0;
                            BinCard.BinCard_NetWeightOutRatio = 0;

                            BinCard.BinCard_UnitGrsWeightOut = 0;
                            BinCard.BinCard_UnitGrsWeightOutRatio = 0;

                            BinCard.BinCard_GrsWeightOut = 0;
                            BinCard.BinCard_GrsWeightOutRatio = 0;

                            BinCard.BinCard_UnitWidthOut = 0;
                            BinCard.BinCard_UnitWidthOutRatio = 0;

                            BinCard.BinCard_WidthOut = 0;
                            BinCard.BinCard_WidthOutRatio = 0;

                            BinCard.BinCard_UnitLengthOut = 0;
                            BinCard.BinCard_UnitLengthOutRatio = 0;

                            BinCard.BinCard_LengthOut = 0;
                            BinCard.BinCard_LengthOutRatio = 0;

                            BinCard.BinCard_UnitHeightOut = 0;
                            BinCard.BinCard_UnitHeightOutRatio = 0;

                            BinCard.BinCard_HeightOut = 0;
                            BinCard.BinCard_HeightOutRatio = 0;

                            #endregion

                            #region Sign

                            BinCard.BinCard_QtySign = item.TotalQty;

                            BinCard.BinCard_UnitWeightSign = item.UnitWeight;
                            BinCard.BinCard_UnitWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_UnitWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_UnitWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_UnitWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_WeightSign = item.Weight;
                            BinCard.BinCard_WeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_WeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_WeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_WeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_UnitNetWeightSign = item.UnitWeight;
                            BinCard.BinCard_UnitNetWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_UnitNetWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_UnitNetWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_UnitNetWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_NetWeightSign = item.NetWeight;
                            BinCard.BinCard_NetWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_NetWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_NetWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_NetWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_UnitGrsWeightSign = item.UnitGrsWeight;
                            BinCard.BinCard_UnitGrsWeightSign_Index = item.GrsWeight_Index;
                            BinCard.BinCard_UnitGrsWeightSign_Id = item.GrsWeight_Id;
                            BinCard.BinCard_UnitGrsWeightSign_Name = item.GrsWeight_Name;
                            BinCard.BinCard_UnitGrsWeightSignRatio = item.GrsWeightRatio;

                            BinCard.BinCard_GrsWeightSign = item.GrsWeight;
                            BinCard.BinCard_GrsWeightSign_Index = item.GrsWeight_Index;
                            BinCard.BinCard_GrsWeightSign_Id = item.GrsWeight_Id;
                            BinCard.BinCard_GrsWeightSign_Name = item.GrsWeight_Name;
                            BinCard.BinCard_GrsWeightSignRatio = item.GrsWeightRatio;

                            BinCard.BinCard_UnitWidthSign = item.UnitWidth;
                            BinCard.BinCard_UnitWidthSign_Index = item.Width_Index;
                            BinCard.BinCard_UnitWidthSign_Id = item.Width_Id;
                            BinCard.BinCard_UnitWidthSign_Name = item.Width_Name;
                            BinCard.BinCard_UnitWidthSignRatio = item.WidthRatio;

                            BinCard.BinCard_WidthSign = item.Width;
                            BinCard.BinCard_WidthSign_Index = item.Width_Index;
                            BinCard.BinCard_WidthSign_Id = item.Width_Id;
                            BinCard.BinCard_WidthSign_Name = item.Width_Name;
                            BinCard.BinCard_WidthSignRatio = item.WidthRatio;

                            BinCard.BinCard_UnitLengthSign = item.UnitLength;
                            BinCard.BinCard_UnitLengthSign_Index = item.Length_Index;
                            BinCard.BinCard_UnitLengthSign_Id = item.Length_Id;
                            BinCard.BinCard_UnitLengthSign_Name = item.Length_Name;
                            BinCard.BinCard_UnitLengthSignRatio = item.LengthRatio;

                            BinCard.BinCard_LengthSign = item.Length;
                            BinCard.BinCard_LengthSign_Index = item.Length_Index;
                            BinCard.BinCard_LengthSign_Id = item.Length_Id;
                            BinCard.BinCard_LengthSign_Name = item.Length_Name;
                            BinCard.BinCard_LengthSignRatio = item.LengthRatio;

                            BinCard.BinCard_UnitHeightSign = item.UnitHeight;
                            BinCard.BinCard_UnitHeightSign_Index = item.Height_Index;
                            BinCard.BinCard_UnitHeightSign_Id = item.Height_Id;
                            BinCard.BinCard_UnitHeightSign_Name = item.Height_Name;
                            BinCard.BinCard_UnitHeightSignRatio = item.HeightRatio;

                            BinCard.BinCard_HeightSign = item.Height;
                            BinCard.BinCard_HeightSign_Index = item.Height_Index;
                            BinCard.BinCard_HeightSign_Id = item.Height_Id;
                            BinCard.BinCard_HeightSign_Name = item.Height_Name;
                            BinCard.BinCard_HeightSignRatio = item.HeightRatio;

                            BinCard.BinCard_UnitVolumeSign = item.UnitVolume;
                            BinCard.BinCard_VolumeSign = item.Volume;

                            #endregion

                            BinCard.Ref_Document_No = GoodsReceive.GoodsReceive_No;
                            BinCard.Ref_Document_Index = item.GoodsReceive_Index; //tem.Ref_Document_Index;
                            BinCard.Ref_DocumentItem_Index = item.GoodsReceiveItem_Index;
                            BinCard.Create_By = data.Create_By;
                            BinCard.Create_Date = DateTime.Now;
                            BinCard.BinBalance_Index = BinBalance.BinBalance_Index; // Add new
                            BinCard.ERP_Location = item.ERP_Location; // Add new
                            BinCard.ERP_Location_To = item.ERP_Location; // Add new

                            ////------------------------------------------------
                            context.IM_GoodsReceiveItemLocation.Add(GoodsReceiveItemLocation);
                            contextGRBinbalance.wm_BinBalance.Add(BinBalance);
                            contextGRBinbalance.wm_BinCard.Add(BinCard);


                            ///-------------------------------------------------
                            var oldTag = context.WM_Tag.Where(c => c.Tag_Index == item.Tag_Index && c.Tag_Status != -1).FirstOrDefault();
                            var oldTagItem = context.wm_TagItem.Where(c => c.TagItem_Index == item.TagItem_Index && c.Tag_Status != -1).FirstOrDefault();
                            var oldGoodsReceive = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).FirstOrDefault();
                            oldGoodsReceiveItem = context.IM_GoodsReceiveItem.Where(c => c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index).FirstOrDefault();

                            oldTag.Tag_Status = 1;
                            oldTagItem.Tag_Status = 1;
                            oldGoodsReceive.Document_Status = 2;

                        }

                        if (!string.IsNullOrEmpty(oldGoodsReceiveItem.Ref_Document_Index.ToString()))
                        {
                            var resultIsComplete = db.View_GoodsReceiveWithTag_V2.Where(c => c.Ref_Document_Index == oldGoodsReceiveItem.Ref_Document_Index && c.Document_Status != -1 && c.IsCompleted == 0).ToList();

                            if (resultIsComplete.Count == 0)
                            {
                                #region Update PlanGoodsReceive Status 3
                                var PlanGRViewModel = new PlanGoodsReceiveViewModel();
                                PlanGRViewModel.planGoodsReceive_Index = new Guid(oldGoodsReceiveItem.Ref_Document_Index.ToString());
                                PlanGRViewModel.document_Status = 3;
                                var updatePlanGRStatus = utils.SendDataApi<Boolean>(new AppSettingConfig().GetUrl("updatePlanGRStatus"), PlanGRViewModel.sJson());
                                #endregion
                            }
                        }
                        #region GetDate
                        var oldGoodsreceive = context.IM_GoodsReceive.Find(data.goodsReceive_Index);

                        #endregion
                        #region Clear UserAssign
                        olog.logging("GoodsReceiveConfirm", "Clear UserAssign");

                        oldGoodsreceive.UserAssign = "";
                        #endregion

                        olog.logging("GoodsReceiveConfirm", "GoodsReceiveItemLocation");
                        olog.logging("GoodsReceiveConfirm", "BinBalance");
                        olog.logging("GoodsReceiveConfirm", "BinCard");

                        olog.logging("GoodsReceiveConfirm", "sp_Save_GoodsReceiveConfirm");
                       
                    }
                    var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        context.SaveChanges();
                        contextGRBinbalance.SaveChanges();
                        transactionx.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("SaveGoodsReceivConfirm", msglog);
                        transactionx.Rollback();

                        throw exy;

                    }

                }
                var assignJobService = new AssignService();
                var assignmodel = new GoodIssue.AssignJobViewModel();
                assignmodel.Template = "1";
                assignmodel.Create_By = data.Create_By;
                var grList = new List<GoodIssue.listGoodsReceiveViewModel>();
                var gr = new GoodIssue.listGoodsReceiveViewModel();
                gr.goodsReceive_Index = data.goodsReceive_Index;
                gr.owner_Index = data.owner_Index;
                grList.Add(gr);
                assignmodel.listGoodsReceiveViewModel = grList;
                var assignResult = assignJobService.assign(assignmodel);
                return true;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("GoodsReceiveConfirm", msglog);
                throw ex;
            }
        }

        public Boolean GoodsReceiveConfirmV3(GoodsReceiveConfirmViewModel data)
        {

            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var contextGRBinbalance = new GRBinBalanceDbContext();

            try
            {
                using (var context = new GRDbContext())
                {
                    olog.logging("GoodsReceiveConfirm", "Start" + data.goodsReceive_Index.ToString());

                    context.Database.SetCommandTimeout(3600);
                    //var GoodsReceive = context.IM_GoodsReceive.AsQueryable();
                    var chkStatus = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status == 2).Any();
                    if (chkStatus == true)
                    {
                        olog.logging("GoodsReceiveConfirm", "chkStatus_fasle");

                        return false;
                    }
                    else
                    {
                        olog.logging("GoodsReceiveConfirm", "chkStatus_true");
                        olog.logging("GoodsReceiveConfirm", "sp_GetTagItem_GrConfirm");
                        var queryResult = context.wm_TagItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Tag_Status != -1).ToList();


                        if (queryResult.Count <= 0)
                        {
                            return false;
                        }

                        // Check Gentag isComplete
                        var lstGoodsReceiveNotComplete = db.View_GoodsReceiveWithTag.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index && c.Document_Status != -1 && c.IsCompleted == 0).ToList();

                        if (lstGoodsReceiveNotComplete.Count > 0)
                        {
                            return false;
                        }

                        var listGoodsReceiveItemLocation = new List<GoodsReceiveItemLocationViewModel>();
                        var listBinBalance = new List<BinBalanceViewModel>();
                        var listBinCard = new List<BinCardViewModel>();
                        IM_GoodsReceiveItem oldGoodsReceiveItem = new IM_GoodsReceiveItem();

                        foreach (var item in queryResult)
                        {
                            IM_GoodsReceiveItemLocation GoodsReceiveItemLocation = new IM_GoodsReceiveItemLocation();

                            WM_BinBalance BinBalance = new WM_BinBalance();
                            WM_BinCard BinCard = new WM_BinCard();

                            var GoodsReceive = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).FirstOrDefault();

                            var GoodsReceiveItem = context.IM_GoodsReceiveItem.Where(c => c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index).FirstOrDefault();


                            #region Config Suggest Location
                            //var GRDocument_Type = GoodsReceive.DocumentType_Index.ToString();
                            var LocationIndex = utils.SendDataApi<string>(new AppSettingConfig().GetUrl("getConfigFromBase"), new { Config_Key = "Config_GR_Staging" }.sJson());
                            //if (GRDocument_Type == "D5BE0561-C089-4B21-BA27-D346E6164327")
                            //{
                            //    LocationIndex = "FA699ED7-A269-0646-94AD-1ED25A4F6C93";
                            //}
                            //else if (GRDocument_Type == "9AC3C7F0-EB96-4E4E-AD6C-CA796CD03A54")
                            //{
                            //    LocationIndex = "5C588813-3FDC-42E6-84A9-8FA5AB024A86";
                            //}
                            //else
                            //{
                            //    LocationIndex = "FA699ED7-A269-0646-94AD-1ED25A4F6C93";
                            //}
                            #endregion

                            #region Get Location
                            //var LocationViewModel = new LocationViewModel();
                            //LocationViewModel.location_Index = new Guid(LocationIndex);

                            var LocationViewModel = new { location_Index = new Guid(LocationIndex) };
                            var GetLocation = utils.SendDataApi<List<LocationViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LocationViewModel.sJson());

                            var DataLocation = GetLocation.FirstOrDefault();
                            #endregion


                            #region Get Product
                            var ProductViewModel = new ProductViewModel();
                            ProductViewModel.product_Index = item.Product_Index;
                            var GetProduct = utils.SendDataApi<List<ProductModel>>(new AppSettingConfig().GetUrl("getProduct"), ProductViewModel.sJson());

                            var DataProduct = GetProduct.FirstOrDefault();
                            #endregion

                            ////-------------------- GR Location --------------------
                            var GoodsReceiveItemLocation_Index = Guid.NewGuid();

                            GoodsReceiveItemLocation.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;
                            GoodsReceiveItemLocation.GoodsReceive_Index = item.GoodsReceive_Index;
                            GoodsReceiveItemLocation.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                            GoodsReceiveItemLocation.TagItem_Index = item.TagItem_Index;
                            GoodsReceiveItemLocation.Tag_Index = item.Tag_Index;
                            GoodsReceiveItemLocation.Tag_No = item.Tag_No;
                            GoodsReceiveItemLocation.Product_Index = item.Product_Index;
                            GoodsReceiveItemLocation.Product_Id = item.Product_Id;
                            GoodsReceiveItemLocation.Product_Name = item.Product_Name;
                            GoodsReceiveItemLocation.Product_SecondName = item.Product_SecondName;
                            GoodsReceiveItemLocation.Product_ThirdName = item.Product_ThirdName;
                            GoodsReceiveItemLocation.Product_Lot = item.Product_Lot;
                            GoodsReceiveItemLocation.ItemStatus_Index = item.ItemStatus_Index;
                            GoodsReceiveItemLocation.ItemStatus_Id = item.ItemStatus_Id;
                            GoodsReceiveItemLocation.ItemStatus_Name = item.ItemStatus_Name;
                            GoodsReceiveItemLocation.ProductConversion_Index = DataProduct.productConversion_Index;
                            GoodsReceiveItemLocation.ProductConversion_Id = DataProduct.productConversion_Id;
                            GoodsReceiveItemLocation.ProductConversion_Name = DataProduct.productConversion_Name;
                            GoodsReceiveItemLocation.MFG_Date = item.MFG_Date;
                            GoodsReceiveItemLocation.EXP_Date = item.EXP_Date;

                            GoodsReceiveItemLocation.WeightRatio = item.WeightRatio;
                            GoodsReceiveItemLocation.UnitWeight = item.UnitWeight;
                            GoodsReceiveItemLocation.Weight = item.Weight;
                            GoodsReceiveItemLocation.Weight_Index = item.Weight_Index;
                            GoodsReceiveItemLocation.Weight_Id = item.Weight_Id;
                            GoodsReceiveItemLocation.Weight_Name = item.Weight_Name;
                            GoodsReceiveItemLocation.NetWeight = item.NetWeight;

                            GoodsReceiveItemLocation.GrsWeightRatio = item.GrsWeightRatio;
                            GoodsReceiveItemLocation.UnitGrsWeight = item.UnitGrsWeight;
                            GoodsReceiveItemLocation.GrsWeight = item.GrsWeight;
                            GoodsReceiveItemLocation.GrsWeight_Index = item.GrsWeight_Index;
                            GoodsReceiveItemLocation.GrsWeight_Id = item.GrsWeight_Id;
                            GoodsReceiveItemLocation.GrsWeight_Name = item.GrsWeight_Name;

                            GoodsReceiveItemLocation.WidthRatio = item.WidthRatio;
                            GoodsReceiveItemLocation.UnitWidth = item.UnitWidth;
                            GoodsReceiveItemLocation.Width = item.Width;
                            GoodsReceiveItemLocation.Width_Index = item.Width_Index;
                            GoodsReceiveItemLocation.Width_Id = item.Width_Id;
                            GoodsReceiveItemLocation.Width_Name = item.Width_Name;

                            GoodsReceiveItemLocation.LengthRatio = item.LengthRatio;
                            GoodsReceiveItemLocation.UnitLength = item.UnitLength;
                            GoodsReceiveItemLocation.Length = item.Length;
                            GoodsReceiveItemLocation.Length_Index = item.Length_Index;
                            GoodsReceiveItemLocation.Length_Id = item.Length_Id;
                            GoodsReceiveItemLocation.Length_Name = item.Length_Name;

                            GoodsReceiveItemLocation.HeightRatio = item.HeightRatio;
                            GoodsReceiveItemLocation.UnitHeight = item.UnitHeight;
                            GoodsReceiveItemLocation.Height = item.Height;
                            GoodsReceiveItemLocation.Height_Index = item.Height_Index;
                            GoodsReceiveItemLocation.Height_Id = item.Height_Id;
                            GoodsReceiveItemLocation.Height_Name = item.Height_Name;

                            GoodsReceiveItemLocation.UnitVolume = item.UnitVolume;
                            GoodsReceiveItemLocation.Volume = item.Volume;


                            GoodsReceiveItemLocation.UnitPrice = item.UnitPrice;
                            GoodsReceiveItemLocation.Price = item.Price;
                            GoodsReceiveItemLocation.TotalPrice = item.TotalPrice;

                            GoodsReceiveItemLocation.Currency_Index = item.Currency_Index;
                            GoodsReceiveItemLocation.Currency_Id = item.Currency_Id;
                            GoodsReceiveItemLocation.Currency_Name = item.Currency_Name;

                            GoodsReceiveItemLocation.Ref_Code1 = item.Ref_Code1;
                            GoodsReceiveItemLocation.Ref_Code2 = item.Ref_Code2;
                            GoodsReceiveItemLocation.Ref_Code3 = item.Ref_Code3;
                            GoodsReceiveItemLocation.Ref_Code4 = item.Ref_Code4;
                            GoodsReceiveItemLocation.Ref_Code5 = item.Ref_Code5;


                            GoodsReceiveItemLocation.Owner_Index = GoodsReceive.Owner_Index;
                            GoodsReceiveItemLocation.Owner_Id = GoodsReceive.Owner_Id;
                            GoodsReceiveItemLocation.Owner_Name = GoodsReceive.Owner_Name;


                            if (GetLocation.Count > 0)
                            {
                                //DataLocation.select(c => c.Location_Index).sParse<Guid?>();
                                GoodsReceiveItemLocation.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                GoodsReceiveItemLocation.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                GoodsReceiveItemLocation.Location_Name = DataLocation.location_Name; //item.Location_Name;

                            }

                            GoodsReceiveItemLocation.Qty = item.Qty;
                            GoodsReceiveItemLocation.Ratio = item.Ratio;
                            GoodsReceiveItemLocation.TotalQty = item.TotalQty;
                            GoodsReceiveItemLocation.UDF_1 = item.UDF_1;
                            GoodsReceiveItemLocation.UDF_2 = item.UDF_2;
                            GoodsReceiveItemLocation.UDF_3 = item.UDF_3;
                            GoodsReceiveItemLocation.UDF_4 = item.UDF_4;
                            GoodsReceiveItemLocation.UDF_5 = item.UDF_5;
                            GoodsReceiveItemLocation.Create_By = data.Create_By;
                            GoodsReceiveItemLocation.Create_Date = DateTime.Now;
                            GoodsReceiveItemLocation.Update_By = item.Update_By;
                            //GoodsReceiveItemLocation.Update_Date = item.Update_Date;
                            GoodsReceiveItemLocation.Cancel_By = item.Cancel_By;
                            //GoodsReceiveItemLocation.Cancel_Date = item.Cancel_Date;
                            //GoodsReceiveItemLocation.Putaway_By = "";// item.Putaway_By;
                            GoodsReceiveItemLocation.Putaway_Status = 0;
                            //GoodsReceiveItemLocation.Putaway_Date = DateTime.Now;
                            GoodsReceiveItemLocation.ERP_Location = item.ERP_Location;


                            ////--------------------Bin Balance --------------------


                            BinBalance.BinBalance_Index = Guid.NewGuid();
                            BinBalance.Owner_Index = GoodsReceive.Owner_Index;//item.Owner_Index;
                            BinBalance.Owner_Id = GoodsReceive.Owner_Id;//item.Owner_Id;
                            BinBalance.Owner_Name = GoodsReceive.Owner_Name; // item.Owner_Name;
                            if (GetLocation.Count > 0)
                            {
                                BinBalance.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                BinBalance.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                BinBalance.Location_Name = DataLocation.location_Name; //item.Location_Name;
                            }
                            BinBalance.GoodsReceive_Index = item.GoodsReceive_Index.GetValueOrDefault();
                            BinBalance.GoodsReceive_No = GoodsReceive.GoodsReceive_No; ; //item.GoodsReceive_No;
                            //DateTime oDate = DateTime.ParseExact(GoodsReceive.GoodsReceive_Date.toString(), "yyyy-MM-dd HH:mm:ss", null);
                            BinBalance.GoodsReceive_Date = GoodsReceive.GoodsReceive_Date.sParse<DateTime>();  //item.GoodsReceive_Date;
                            BinBalance.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index.GetValueOrDefault();
                            BinBalance.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                            BinBalance.TagItem_Index = item.TagItem_Index.sParse<Guid>();
                            BinBalance.Tag_Index = new Guid(item.Tag_Index.ToString());
                            BinBalance.Tag_No = item.Tag_No;
                            BinBalance.Product_Index = item.Product_Index;
                            BinBalance.Product_Id = item.Product_Id;
                            BinBalance.Product_Name = item.Product_Name;
                            BinBalance.Product_SecondName = item.Product_SecondName;
                            BinBalance.Product_ThirdName = item.Product_ThirdName;
                            BinBalance.Product_Lot = item.Product_Lot;
                            BinBalance.ItemStatus_Index = item.ItemStatus_Index.GetValueOrDefault();
                            BinBalance.ItemStatus_Id = item.ItemStatus_Id;
                            BinBalance.ItemStatus_Name = item.ItemStatus_Name;
                            BinBalance.GoodsReceive_MFG_Date = item.MFG_Date;
                            BinBalance.GoodsReceive_EXP_Date = item.EXP_Date;
                            BinBalance.GoodsReceive_ProductConversion_Index = item.ProductConversion_Index;
                            BinBalance.GoodsReceive_ProductConversion_Id = item.ProductConversion_Id;
                            BinBalance.GoodsReceive_ProductConversion_Name = item.ProductConversion_Name;
                            BinBalance.BinBalance_Ratio = 1;
                            BinBalance.BinBalance_QtyBegin = item.TotalQty;

                            #region Begin 
                            BinBalance.BinBalance_WeightBegin = item.Weight;
                            BinBalance.BinBalance_WeightBegin_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightBegin_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightBegin_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightBeginRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightBegin = item.NetWeight;
                            BinBalance.BinBalance_NetWeightBegin_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightBegin_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightBegin_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightBeginRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightBegin = item.GrsWeight;
                            BinBalance.BinBalance_GrsWeightBegin_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightBegin_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightBegin_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightBeginRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthBegin = item.Width;
                            BinBalance.BinBalance_WidthBegin_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthBegin_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthBegin_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthBeginRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthBegin = item.Length;
                            BinBalance.BinBalance_LengthBegin_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthBegin_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthBegin_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthBeginRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightBegin = item.Height;
                            BinBalance.BinBalance_HeightBegin_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightBegin_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightBegin_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightBeginRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeBegin = item.UnitVolume;
                            BinBalance.BinBalance_VolumeBegin = item.Volume;
                            #endregion

                            #region Bal
                            BinBalance.BinBalance_QtyBal = item.TotalQty;

                            BinBalance.BinBalance_WeightBal = item.Weight;
                            BinBalance.BinBalance_WeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_UnitWeightBal = item.UnitWeight;
                            BinBalance.BinBalance_UnitWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_UnitWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_UnitWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_UnitWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightBal = item.NetWeight;
                            BinBalance.BinBalance_NetWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_UnitNetWeightBal = item.UnitWeight;
                            BinBalance.BinBalance_UnitNetWeightBal_Index = item.Weight_Index;
                            BinBalance.BinBalance_UnitNetWeightBal_Id = item.Weight_Id;
                            BinBalance.BinBalance_UnitNetWeightBal_Name = item.Weight_Name;
                            BinBalance.BinBalance_UnitNetWeightBalRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightBal = item.GrsWeight;
                            BinBalance.BinBalance_GrsWeightBal_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightBal_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightBal_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightBalRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_UnitGrsWeightBal = item.UnitGrsWeight;
                            BinBalance.BinBalance_UnitGrsWeightBal_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_UnitGrsWeightBal_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_UnitGrsWeightBal_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_UnitGrsWeightBalRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthBal = item.Width;
                            BinBalance.BinBalance_WidthBal_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthBal_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthBal_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthBalRatio = item.WidthRatio;

                            BinBalance.BinBalance_UnitWidthBal = item.UnitWidth;
                            BinBalance.BinBalance_UnitWidthBal_Index = item.Width_Index;
                            BinBalance.BinBalance_UnitWidthBal_Id = item.Width_Id;
                            BinBalance.BinBalance_UnitWidthBal_Name = item.Width_Name;
                            BinBalance.BinBalance_UnitWidthBalRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthBal = item.Length;
                            BinBalance.BinBalance_UnitLengthBal = item.UnitLength;
                            BinBalance.BinBalance_LengthBal_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthBal_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthBal_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthBalRatio = item.LengthRatio;

                            BinBalance.BinBalance_UnitLengthBal = item.UnitLength;
                            BinBalance.BinBalance_UnitLengthBal_Index = item.Length_Index;
                            BinBalance.BinBalance_UnitLengthBal_Id = item.Length_Id;
                            BinBalance.BinBalance_UnitLengthBal_Name = item.Length_Name;
                            BinBalance.BinBalance_UnitLengthBalRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightBal = item.Height;
                            BinBalance.BinBalance_UnitHeightBal = item.UnitHeight;
                            BinBalance.BinBalance_HeightBal_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightBal_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightBal_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightBalRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitHeightBal = item.UnitHeight;
                            BinBalance.BinBalance_UnitHeightBal_Index = item.Height_Index;
                            BinBalance.BinBalance_UnitHeightBal_Id = item.Height_Id;
                            BinBalance.BinBalance_UnitHeightBal_Name = item.Height_Name;
                            BinBalance.BinBalance_UnitHeightBalRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeBal = item.UnitVolume;
                            BinBalance.BinBalance_VolumeBal = item.Volume;
                            #endregion

                            #region Reserve
                            BinBalance.BinBalance_QtyReserve = 0;

                            BinBalance.BinBalance_WeightReserve = 0;
                            BinBalance.BinBalance_WeightReserveRatio = item.WeightRatio;
                            BinBalance.BinBalance_WeightReserve_Index = item.Weight_Index;
                            BinBalance.BinBalance_WeightReserve_Id = item.Weight_Id;
                            BinBalance.BinBalance_WeightReserve_Name = item.Weight_Name;
                            BinBalance.BinBalance_WeightReserveRatio = item.WeightRatio;

                            BinBalance.BinBalance_NetWeightReserve = 0;
                            BinBalance.BinBalance_NetWeightReserve_Index = item.Weight_Index;
                            BinBalance.BinBalance_NetWeightReserve_Id = item.Weight_Id;
                            BinBalance.BinBalance_NetWeightReserve_Name = item.Weight_Name;
                            BinBalance.BinBalance_NetWeightReserveRatio = item.WeightRatio;

                            BinBalance.BinBalance_GrsWeightReserve = 0;
                            BinBalance.BinBalance_GrsWeightReserve_Index = item.GrsWeight_Index;
                            BinBalance.BinBalance_GrsWeightReserve_Id = item.GrsWeight_Id;
                            BinBalance.BinBalance_GrsWeightReserve_Name = item.GrsWeight_Name;
                            BinBalance.BinBalance_GrsWeightReserveRatio = item.GrsWeightRatio;

                            BinBalance.BinBalance_WidthReserve = 0;
                            BinBalance.BinBalance_WidthReserve_Index = item.Width_Index;
                            BinBalance.BinBalance_WidthReserve_Id = item.Width_Id;
                            BinBalance.BinBalance_WidthReserve_Name = item.Width_Name;
                            BinBalance.BinBalance_WidthReserveRatio = item.WidthRatio;

                            BinBalance.BinBalance_LengthReserve = 0;
                            BinBalance.BinBalance_LengthReserve_Index = item.Length_Index;
                            BinBalance.BinBalance_LengthReserve_Id = item.Length_Id;
                            BinBalance.BinBalance_LengthReserve_Name = item.Length_Name;
                            BinBalance.BinBalance_LengthReserveRatio = item.LengthRatio;

                            BinBalance.BinBalance_HeightReserve = 0;
                            BinBalance.BinBalance_HeightReserve_Index = item.Height_Index;
                            BinBalance.BinBalance_HeightReserve_Id = item.Height_Id;
                            BinBalance.BinBalance_HeightReserve_Name = item.Height_Name;
                            BinBalance.BinBalance_HeightReserveRatio = item.HeightRatio;

                            BinBalance.BinBalance_UnitVolumeReserve = 0;
                            BinBalance.BinBalance_VolumeReserve = 0;
                            #endregion



                            BinBalance.Invoice_No = GoodsReceiveItem.Invoice_No;
                            BinBalance.Declaration_No = GoodsReceiveItem.Declaration_No;
                            BinBalance.HS_Code = GoodsReceiveItem.HS_Code;
                            BinBalance.Conutry_of_Origin = GoodsReceiveItem.Conutry_of_Origin;

                            BinBalance.Tax1 = GoodsReceiveItem.Tax1;
                            BinBalance.Tax1_Currency_Index = GoodsReceiveItem.Tax1_Currency_Index;
                            BinBalance.Tax1_Currency_Id = GoodsReceiveItem.Tax1_Currency_Id;
                            BinBalance.Tax1_Currency_Name = GoodsReceiveItem.Tax1_Currency_Name;

                            BinBalance.Tax2 = GoodsReceiveItem.Tax2;
                            BinBalance.Tax2_Currency_Index = GoodsReceiveItem.Tax2_Currency_Index;
                            BinBalance.Tax2_Currency_Id = GoodsReceiveItem.Tax2_Currency_Id;
                            BinBalance.Tax2_Currency_Name = GoodsReceiveItem.Tax2_Currency_Name;

                            BinBalance.Tax3 = GoodsReceiveItem.Tax3;
                            BinBalance.Tax3_Currency_Index = GoodsReceiveItem.Tax3_Currency_Index;
                            BinBalance.Tax3_Currency_Id = GoodsReceiveItem.Tax3_Currency_Id;
                            BinBalance.Tax3_Currency_Name = GoodsReceiveItem.Tax3_Currency_Name;

                            BinBalance.Tax4 = GoodsReceiveItem.Tax4;
                            BinBalance.Tax4_Currency_Index = GoodsReceiveItem.Tax4_Currency_Index;
                            BinBalance.Tax4_Currency_Id = GoodsReceiveItem.Tax4_Currency_Id;
                            BinBalance.Tax4_Currency_Name = GoodsReceiveItem.Tax4_Currency_Name;

                            BinBalance.Tax5 = GoodsReceiveItem.Tax5;
                            BinBalance.Tax5_Currency_Index = GoodsReceiveItem.Tax5_Currency_Index;
                            BinBalance.Tax5_Currency_Id = GoodsReceiveItem.Tax5_Currency_Id;
                            BinBalance.Tax5_Currency_Name = GoodsReceiveItem.Tax5_Currency_Name;


                            BinBalance.ProductConversion_Index = DataProduct.productConversion_Index;
                            BinBalance.ProductConversion_Id = DataProduct.productConversion_Id;
                            BinBalance.ProductConversion_Name = DataProduct.productConversion_Name;
                            BinBalance.UDF_1 = item.UDF_1;
                            BinBalance.UDF_2 = item.UDF_2;
                            BinBalance.UDF_3 = item.UDF_3;
                            BinBalance.UDF_4 = item.UDF_4;
                            BinBalance.UDF_5 = item.UDF_5;
                            BinBalance.Create_By = data.Create_By;
                            BinBalance.Create_Date = DateTime.Now;
                            BinBalance.Update_By = item.Update_By;
                            //BinBalance.Update_Date = item.Update_Date;
                            BinBalance.Cancel_By = item.Cancel_By;
                            //BinBalance.Cancel_Date = item.Cancel_Date;
                            BinBalance.ERP_Location = item.ERP_Location;


                            ////--------------------Bin Card --------------------

                            BinCard.BinCard_Index = Guid.NewGuid();
                            BinCard.Process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");//item.Process_Index;
                            BinCard.DocumentType_Index = GoodsReceive.DocumentType_Index; //item.DocumentType_Index;
                            BinCard.DocumentType_Id = GoodsReceive.DocumentType_Id;//item.DocumentType_Id;
                            BinCard.DocumentType_Name = GoodsReceive.DocumentType_Name;//item.DocumentType_Name;
                            BinCard.GoodsReceive_Index = item.GoodsReceive_Index;
                            BinCard.GoodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                            BinCard.GoodsReceiveItemLocation_Index = GoodsReceiveItemLocation_Index;//item.GoodsReceiveItemLocation_Index;
                            BinCard.BinCard_No = GoodsReceive.GoodsReceive_No; ; //item.BinCard_No;
                            BinCard.BinCard_Date = GoodsReceive.GoodsReceive_Date.sParse<DateTime>(); ; //item.BinCard_Date;
                            BinCard.TagItem_Index = item.TagItem_Index;
                            BinCard.Tag_Index = item.Tag_Index;
                            BinCard.Tag_No = item.Tag_No;
                            BinCard.Tag_Index_To = item.TagItem_Index; //item.Tag_Index_To;
                            BinCard.Tag_No_To = item.Tag_No; //item.Tag_No_To;
                            BinCard.Product_Index = item.Product_Index;
                            BinCard.Product_Id = item.Product_Id;
                            BinCard.Product_Name = item.Product_Name;
                            BinCard.Product_SecondName = item.Product_SecondName;
                            BinCard.Product_ThirdName = item.Product_ThirdName;
                            BinCard.Product_Index_To = item.Product_Index; //item.Product_Index_To;
                            BinCard.Product_Id_To = item.Product_Id;
                            BinCard.Product_Name_To = item.Product_Name;
                            BinCard.Product_SecondName_To = item.Product_SecondName;
                            BinCard.Product_ThirdName_To = item.Product_ThirdName;
                            BinCard.Product_Lot = item.Product_Lot;
                            BinCard.Product_Lot_To = item.Product_Lot;
                            BinCard.ItemStatus_Index = item.ItemStatus_Index;
                            BinCard.ItemStatus_Id = item.ItemStatus_Id;
                            BinCard.ItemStatus_Name = item.ItemStatus_Name;
                            BinCard.ItemStatus_Index_To = item.ItemStatus_Index;
                            BinCard.ItemStatus_Id_To = item.ItemStatus_Id;
                            BinCard.ItemStatus_Name_To = item.ItemStatus_Name;
                            BinCard.ProductConversion_Index = DataProduct.productConversion_Index;
                            BinCard.ProductConversion_Id = DataProduct.productConversion_Id;
                            BinCard.ProductConversion_Name = DataProduct.productConversion_Name;
                            BinCard.Owner_Index = GoodsReceive.Owner_Index;//item.Owner_Index;
                            BinCard.Owner_Id = GoodsReceive.Owner_Id;//item.Owner_Id;
                            BinCard.Owner_Name = GoodsReceive.Owner_Name; // item.Owner_Name;

                            BinCard.Owner_Index_To = GoodsReceive.Owner_Index;
                            BinCard.Owner_Id_To = GoodsReceive.Owner_Id;
                            BinCard.Owner_Name_To = GoodsReceive.Owner_Name;
                            if (GetLocation.Count > 0)
                            {
                                BinCard.Location_Index = DataLocation.location_Index;//item.Location_Index;
                                BinCard.Location_Id = DataLocation.location_Id;//item.Location_Id;
                                BinCard.Location_Name = DataLocation.location_Name; //item.Location_Name;
                                BinCard.Location_Index_To = DataLocation.location_Index;
                                BinCard.Location_Id_To = DataLocation.location_Id;
                                BinCard.Location_Name_To = DataLocation.location_Name;
                            }
                            BinCard.GoodsReceive_EXP_Date = item.EXP_Date;
                            BinCard.GoodsReceive_EXP_Date_To = item.EXP_Date;

                            #region In

                            BinCard.BinCard_QtyIn = item.TotalQty;

                            BinCard.BinCard_UnitWeightIn = item.UnitWeight;
                            BinCard.BinCard_UnitWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_UnitWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_UnitWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_UnitWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_WeightIn = item.Weight;
                            BinCard.BinCard_WeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_WeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_WeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_WeightInRatio = item.WeightRatio;

                            BinCard.BinCard_UnitNetWeightIn = item.UnitWeight;
                            BinCard.BinCard_UnitNetWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_UnitNetWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_UnitNetWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_UnitNetWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_NetWeightIn = item.NetWeight;
                            BinCard.BinCard_NetWeightIn_Index = item.Weight_Index;
                            BinCard.BinCard_NetWeightIn_Id = item.Weight_Id;
                            BinCard.BinCard_NetWeightIn_Name = item.Weight_Name;
                            BinCard.BinCard_NetWeightInRatio = item.WeightRatio;

                            BinCard.BinCard_UnitGrsWeightIn = item.UnitGrsWeight;
                            BinCard.BinCard_UnitGrsWeightIn_Index = item.GrsWeight_Index;
                            BinCard.BinCard_UnitGrsWeightIn_Id = item.GrsWeight_Id;
                            BinCard.BinCard_UnitGrsWeightIn_Name = item.GrsWeight_Name;
                            BinCard.BinCard_UnitGrsWeightInRatio = item.GrsWeightRatio;

                            BinCard.BinCard_GrsWeightIn = item.GrsWeight;
                            BinCard.BinCard_GrsWeightIn_Index = item.GrsWeight_Index;
                            BinCard.BinCard_GrsWeightIn_Id = item.GrsWeight_Id;
                            BinCard.BinCard_GrsWeightIn_Name = item.GrsWeight_Name;
                            BinCard.BinCard_GrsWeightInRatio = item.GrsWeightRatio;

                            BinCard.BinCard_UnitWidthIn = item.UnitWidth;
                            BinCard.BinCard_UnitWidthIn_Index = item.Width_Index;
                            BinCard.BinCard_UnitWidthIn_Id = item.Width_Id;
                            BinCard.BinCard_UnitWidthIn_Name = item.Width_Name;
                            BinCard.BinCard_UnitWidthInRatio = item.WidthRatio;

                            BinCard.BinCard_WidthIn = item.Width;
                            BinCard.BinCard_WidthIn_Index = item.Width_Index;
                            BinCard.BinCard_WidthIn_Id = item.Width_Id;
                            BinCard.BinCard_WidthIn_Name = item.Width_Name;
                            BinCard.BinCard_WidthInRatio = item.WidthRatio;

                            BinCard.BinCard_UnitLengthIn = item.UnitLength;
                            BinCard.BinCard_UnitLengthIn_Index = item.Length_Index;
                            BinCard.BinCard_UnitLengthIn_Id = item.Length_Id;
                            BinCard.BinCard_UnitLengthIn_Name = item.Length_Name;
                            BinCard.BinCard_UnitLengthInRatio = item.LengthRatio;

                            BinCard.BinCard_LengthIn = item.Length;
                            BinCard.BinCard_LengthIn_Index = item.Length_Index;
                            BinCard.BinCard_LengthIn_Id = item.Length_Id;
                            BinCard.BinCard_LengthIn_Name = item.Length_Name;
                            BinCard.BinCard_LengthInRatio = item.LengthRatio;

                            BinCard.BinCard_UnitHeightIn = item.UnitHeight;
                            BinCard.BinCard_UnitHeightIn_Index = item.Height_Index;
                            BinCard.BinCard_UnitHeightIn_Id = item.Height_Id;
                            BinCard.BinCard_UnitHeightIn_Name = item.Height_Name;
                            BinCard.BinCard_UnitHeightInRatio = item.HeightRatio;

                            BinCard.BinCard_HeightIn = item.Height;
                            BinCard.BinCard_HeightIn_Index = item.Height_Index;
                            BinCard.BinCard_HeightIn_Id = item.Height_Id;
                            BinCard.BinCard_HeightIn_Name = item.Height_Name;
                            BinCard.BinCard_HeightInRatio = item.HeightRatio;

                            BinCard.BinCard_UnitVolumeIn = item.UnitVolume;
                            BinCard.BinCard_VolumeIn = item.Volume;

                            #endregion

                            #region Out

                            BinCard.BinCard_QtyOut = 0;

                            BinCard.BinCard_UnitWeightOut = 0;
                            BinCard.BinCard_UnitWeightOutRatio = 0;

                            BinCard.BinCard_WeightOut = 0;
                            BinCard.BinCard_WeightOutRatio = 0;

                            BinCard.BinCard_UnitNetWeightOut = 0;
                            BinCard.BinCard_UnitNetWeightOutRatio = 0;

                            BinCard.BinCard_NetWeightOut = 0;
                            BinCard.BinCard_NetWeightOutRatio = 0;

                            BinCard.BinCard_UnitGrsWeightOut = 0;
                            BinCard.BinCard_UnitGrsWeightOutRatio = 0;

                            BinCard.BinCard_GrsWeightOut = 0;
                            BinCard.BinCard_GrsWeightOutRatio = 0;

                            BinCard.BinCard_UnitWidthOut = 0;
                            BinCard.BinCard_UnitWidthOutRatio = 0;

                            BinCard.BinCard_WidthOut = 0;
                            BinCard.BinCard_WidthOutRatio = 0;

                            BinCard.BinCard_UnitLengthOut = 0;
                            BinCard.BinCard_UnitLengthOutRatio = 0;

                            BinCard.BinCard_LengthOut = 0;
                            BinCard.BinCard_LengthOutRatio = 0;

                            BinCard.BinCard_UnitHeightOut = 0;
                            BinCard.BinCard_UnitHeightOutRatio = 0;

                            BinCard.BinCard_HeightOut = 0;
                            BinCard.BinCard_HeightOutRatio = 0;

                            //BinCard.BinCard_UnitVolumeIn = 0;
                            //BinCard.BinCard_VolumeIn = 0;

                            #endregion

                            #region Sign

                            BinCard.BinCard_QtySign = item.TotalQty;

                            BinCard.BinCard_UnitWeightSign = item.UnitWeight;
                            BinCard.BinCard_UnitWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_UnitWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_UnitWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_UnitWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_WeightSign = item.Weight;
                            BinCard.BinCard_WeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_WeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_WeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_WeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_UnitNetWeightSign = item.UnitWeight;
                            BinCard.BinCard_UnitNetWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_UnitNetWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_UnitNetWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_UnitNetWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_NetWeightSign = item.NetWeight;
                            BinCard.BinCard_NetWeightSign_Index = item.Weight_Index;
                            BinCard.BinCard_NetWeightSign_Id = item.Weight_Id;
                            BinCard.BinCard_NetWeightSign_Name = item.Weight_Name;
                            BinCard.BinCard_NetWeightSignRatio = item.WeightRatio;

                            BinCard.BinCard_UnitGrsWeightSign = item.UnitGrsWeight;
                            BinCard.BinCard_UnitGrsWeightSign_Index = item.GrsWeight_Index;
                            BinCard.BinCard_UnitGrsWeightSign_Id = item.GrsWeight_Id;
                            BinCard.BinCard_UnitGrsWeightSign_Name = item.GrsWeight_Name;
                            BinCard.BinCard_UnitGrsWeightSignRatio = item.GrsWeightRatio;

                            BinCard.BinCard_GrsWeightSign = item.GrsWeight;
                            BinCard.BinCard_GrsWeightSign_Index = item.GrsWeight_Index;
                            BinCard.BinCard_GrsWeightSign_Id = item.GrsWeight_Id;
                            BinCard.BinCard_GrsWeightSign_Name = item.GrsWeight_Name;
                            BinCard.BinCard_GrsWeightSignRatio = item.GrsWeightRatio;

                            BinCard.BinCard_UnitWidthSign = item.UnitWidth;
                            BinCard.BinCard_UnitWidthSign_Index = item.Width_Index;
                            BinCard.BinCard_UnitWidthSign_Id = item.Width_Id;
                            BinCard.BinCard_UnitWidthSign_Name = item.Width_Name;
                            BinCard.BinCard_UnitWidthSignRatio = item.WidthRatio;

                            BinCard.BinCard_WidthSign = item.Width;
                            BinCard.BinCard_WidthSign_Index = item.Width_Index;
                            BinCard.BinCard_WidthSign_Id = item.Width_Id;
                            BinCard.BinCard_WidthSign_Name = item.Width_Name;
                            BinCard.BinCard_WidthSignRatio = item.WidthRatio;

                            BinCard.BinCard_UnitLengthSign = item.UnitLength;
                            BinCard.BinCard_UnitLengthSign_Index = item.Length_Index;
                            BinCard.BinCard_UnitLengthSign_Id = item.Length_Id;
                            BinCard.BinCard_UnitLengthSign_Name = item.Length_Name;
                            BinCard.BinCard_UnitLengthSignRatio = item.LengthRatio;

                            BinCard.BinCard_LengthSign = item.Length;
                            BinCard.BinCard_LengthSign_Index = item.Length_Index;
                            BinCard.BinCard_LengthSign_Id = item.Length_Id;
                            BinCard.BinCard_LengthSign_Name = item.Length_Name;
                            BinCard.BinCard_LengthSignRatio = item.LengthRatio;

                            BinCard.BinCard_UnitHeightSign = item.UnitHeight;
                            BinCard.BinCard_UnitHeightSign_Index = item.Height_Index;
                            BinCard.BinCard_UnitHeightSign_Id = item.Height_Id;
                            BinCard.BinCard_UnitHeightSign_Name = item.Height_Name;
                            BinCard.BinCard_UnitHeightSignRatio = item.HeightRatio;

                            BinCard.BinCard_HeightSign = item.Height;
                            BinCard.BinCard_HeightSign_Index = item.Height_Index;
                            BinCard.BinCard_HeightSign_Id = item.Height_Id;
                            BinCard.BinCard_HeightSign_Name = item.Height_Name;
                            BinCard.BinCard_HeightSignRatio = item.HeightRatio;

                            BinCard.BinCard_UnitVolumeSign = item.UnitVolume;
                            BinCard.BinCard_VolumeSign = item.Volume;

                            #endregion

                            BinCard.Ref_Document_No = GoodsReceive.GoodsReceive_No;
                            BinCard.Ref_Document_Index = item.GoodsReceive_Index; //tem.Ref_Document_Index;
                            BinCard.Ref_DocumentItem_Index = item.GoodsReceiveItem_Index;
                            BinCard.Create_By = data.Create_By;
                            BinCard.Create_Date = DateTime.Now;
                            BinCard.BinBalance_Index = BinBalance.BinBalance_Index; // Add new
                            BinCard.ERP_Location = item.ERP_Location; // Add new
                            BinCard.ERP_Location_To = item.ERP_Location; // Add new

                            ////------------------------------------------------
                            context.IM_GoodsReceiveItemLocation.Add(GoodsReceiveItemLocation);
                            contextGRBinbalance.wm_BinBalance.Add(BinBalance);
                            contextGRBinbalance.wm_BinCard.Add(BinCard);


                            ///-------------------------------------------------
                            var oldTag = context.WM_Tag.Where(c => c.Tag_Index == item.Tag_Index && c.Tag_Status != -1).FirstOrDefault();
                            var oldTagItem = context.wm_TagItem.Where(c => c.TagItem_Index == item.TagItem_Index && c.Tag_Status != -1).FirstOrDefault();
                            var oldGoodsReceive = context.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).FirstOrDefault();
                            oldGoodsReceiveItem = context.IM_GoodsReceiveItem.Where(c => c.GoodsReceiveItem_Index == item.GoodsReceiveItem_Index).FirstOrDefault();

                            oldTag.Tag_Status = 1;
                            oldTagItem.Tag_Status = 1;
                            oldGoodsReceive.Document_Status = 2;

                        }

                        olog.logging("GoodsReceiveConfirm", "View_GoodsReceiveWithTag_V2" + data.goodsReceive_Index.ToString());
                        if (!string.IsNullOrEmpty(oldGoodsReceiveItem.Ref_Document_Index.ToString()))
                        {
                            var resultIsComplete = db.View_GoodsReceiveWithTag_V2.Where(c => c.Ref_Document_Index == oldGoodsReceiveItem.Ref_Document_Index && c.Document_Status != -1 && c.IsCompleted == 0).ToList();

                            if (resultIsComplete.Count == 0)
                            {
                                olog.logging("GoodsReceiveConfirm", "updatePlanGRStatus" + data.goodsReceive_Index.ToString());

                                #region Update PlanGoodsReceive Status 3
                                var PlanGRViewModel = new PlanGoodsReceiveViewModel();
                                PlanGRViewModel.planGoodsReceive_Index = new Guid(oldGoodsReceiveItem.Ref_Document_Index.ToString());
                                PlanGRViewModel.document_Status = 3;
                                var updatePlanGRStatus = utils.SendDataApi<Boolean>(new AppSettingConfig().GetUrl("updatePlanGRStatus"), PlanGRViewModel.sJson());
                                #endregion
                            }
                        }

                        //var ItemList = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index).ToList();
                        //ItemList.ForEach(x => { x.Update_By = data.update_By; x.Update_Date = DateTime.Now; });
                        #region GetDate
                        var oldGoodsreceive = context.IM_GoodsReceive.Find(data.goodsReceive_Index);

                        #endregion
                        #region Clear UserAssign
                        olog.logging("GoodsReceiveConfirm", "Clear UserAssign");

                        oldGoodsreceive.UserAssign = "";
                        #endregion
                        #region Update Status
                        //GoodsReceive Status
                        //oldGoodsreceive.Document_Status = 3;


                        #endregion

                        olog.logging("GoodsReceiveConfirm", "GoodsReceiveItemLocation");
                        olog.logging("GoodsReceiveConfirm", "BinBalance");
                        olog.logging("GoodsReceiveConfirm", "BinCard");

                        olog.logging("GoodsReceiveConfirm", "sp_Save_GoodsReceiveConfirm");
                        //int rowsAffected;
                        //try
                        //{

                        //    rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirm @GoodsReceiveItemLocation,@BinBalance,@BinCard", pGoodsReceiveItemLocation, pBinBalance, pBinCard);

                        //}
                        //catch (Exception ex)
                        //{
                        //    msglog = State + " ex Rollback " + ex.Message.ToString();
                        //    olog.logging("GoodsReceiveConfirm", msglog);
                        //    throw ex;
                        //}

                        //if (rowsAffected.ToString() != "0")
                        //{
                        //    olog.logging("GoodsReceiveConfirm", "true");

                        //    var rowsAffectedDelete = context.Database.ExecuteSqlCommand("sp_Save_GoodsReceiveConfirmDelete @GoodsReceiveItemLocation", pGoodsReceiveItemLocation);

                        //    return true;

                        //}
                        //else
                        //{
                        //    olog.logging("GoodsReceiveConfirm", "false");

                        //    return false;

                        //}

                    }
                    //  return rowsAffected.ToString();
                    //var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);

                    //olog.logging("GoodsReceiveConfirm", "S.SaveChanges" + data.goodsReceive_Index.ToString());

                    //var transactionx = db.Database.BeginTransaction();
                    //try
                    //{
                    //    context.SaveChanges();
                    //    contextGRBinbalance.SaveChanges();
                    //    transactionx.Commit();

                    //    olog.logging("GoodsReceiveConfirm", "E.SaveChanges" + data.goodsReceive_Index.ToString());
                    //}

                    //catch (Exception exy)
                    //{
                    //    msglog = State + " ex Rollback " + exy.Message.ToString();
                    //    olog.logging("SaveGoodsReceivConfirm", msglog);
                    //    transactionx.Rollback();

                    //    throw exy;

                    //}

                    olog.logging("GoodsReceiveConfirm", "S.SaveChanges" + data.goodsReceive_Index.ToString());


                    var transactionGR = context.Database.BeginTransaction();
                    var transactionBB = contextGRBinbalance.Database.BeginTransaction();
                    try
                    {
                        context.SaveChanges();
                        contextGRBinbalance.SaveChanges();



                        transactionGR.Commit();
                        transactionBB.Commit();

                        olog.logging("GoodsReceiveConfirm", "E.SaveChanges" + data.goodsReceive_Index.ToString());
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("SaveGoodsReceivConfirm", msglog);
                        olog.logging("GoodsReceiveConfirm", "E.SaveChanges" + data.goodsReceive_Index.ToString());
                        transactionGR.Rollback();
                        transactionBB.Commit();
                        throw exy;

                    }

                }
                olog.logging("GoodsReceiveConfirm", "S.assignJobService" + data.goodsReceive_Index.ToString());
                var assignJobService = new AssignService();
                var assignmodel = new GoodIssue.AssignJobViewModel();
                assignmodel.Template = "2";
                assignmodel.Create_By = data.Create_By;
                var grList = new List<GoodIssue.listGoodsReceiveViewModel>();
                var gr = new GoodIssue.listGoodsReceiveViewModel();
                gr.goodsReceive_Index = data.goodsReceive_Index;
                gr.owner_Index = data.owner_Index;
                grList.Add(gr);
                assignmodel.listGoodsReceiveViewModel = grList;
                var assignResult = assignJobService.assign(assignmodel);
                olog.logging("GoodsReceiveConfirm", "E.assignJobService" + data.goodsReceive_Index.ToString());
                return true;
            }
            catch (Exception ex)
            {
                msglog = State + " ex Rollback " + ex.Message.ToString();
                olog.logging("GoodsReceiveConfirm", msglog);
                throw ex;
            }
        }


        public actionResultTagItemViewModel FilterTag(TagItemViewModel model)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    string pstring = "";

                    if (model.GoodsReceiveNo != null)
                    {
                        pstring += " and GoodsReceive_Index IN (SELECT GoodsReceive_Index FROM im_GoodsReceive WHERE GoodsReceive_No = '" + model.GoodsReceiveNo + "') and Tag_Status != -1 ";
                    }
                    else
                    {
                        return new actionResultTagItemViewModel();
                    }
                    var strwhere = new SqlParameter("@strwhere", pstring);
                    var query = context.View_GetTagItem.FromSql("sp_GetViewTagItem @strwhere", strwhere).ToList();
                    //var queryResult = context.wm_TagItem.FromSql("sp_GetTagItem").Where(c => c.Tag_Status == 1 && c.go == id).OrderByDescending(c => c.Create_Date);
                    var perpages = model.PerPage == 0 ? query.ToList() : query.Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();

                    var result = new List<TagItemViewModel>();
                    foreach (var item in perpages)
                    {
                        var resultItem = new TagItemViewModel();

                        //var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),ProductConversion_Index)");
                        //var ColumnName2 = new SqlParameter("@ColumnName2", "ProductConversion_Id");
                        //var ColumnName3 = new SqlParameter("@ColumnName3", "ProductConversion_Name");
                        //var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                        //var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                        //var TableName = new SqlParameter("@TableName", "ms_Product");
                        //var Where = new SqlParameter("@Where", "Where Product_Index ='" + item.Product_Index + "'");
                        //var DataProduct = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                        resultItem.TagIndex = item.Tag_Index;
                        resultItem.TagItemIndex = item.TagItem_Index;
                        resultItem.GoodsReceiveIndex = item.GoodsReceive_Index;
                        resultItem.GoodsReceiveNo = item.GoodsReceive_No;
                        resultItem.ProductId = item.Product_Id;
                        resultItem.ProductName = item.Product_Name;
                        resultItem.ProductIndex = item.Product_Index;
                        resultItem.Uom = item.Product_ProductConvertion;
                        resultItem.ItemStatusName = item.ItemStatus_Name;
                        resultItem.LPN = item.Tag_No;
                        resultItem.Qty = item.Qty;
                        resultItem.TotalQty = item.GRITotalQty;
                        resultItem.GoodsReceiveProductConversion_Name = item.ProductConversion_Name;
                        resultItem.TagStatus = item.Tag_Status;

                        resultItem.isCheckQty = CheckReceiveQtyProduct(item.GoodsReceive_Index.ToString(), item.GoodsReceiveItem_Index.ToString(), item.Product_Index.ToString());

                        result.Add(resultItem);
                    }
                    var count = query.Count;
                    var actionResultTagItem = new actionResultTagItemViewModel();
                    actionResultTagItem.items = result.ToList();
                    actionResultTagItem.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };

                    return actionResultTagItem;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public actionResultGRViewModel filter(SearchGRModel model)
        {
            try
            {
                var filterModel = new ProcessStatusViewModel();
                filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());



                var filterModelPutaway = new ProcessStatusViewModel();
                filterModelPutaway.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");
                var ProcessPutaway = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModelPutaway.sJson());


                var query = db.View_filterGR.AsQueryable();
                //AdvanceSearch
                if (model.advanceSearch == true)
                {
                    if (!string.IsNullOrEmpty(model.goodsReceive_No))
                    {
                        query = query.Where(c => c.GoodsReceive_No == (model.goodsReceive_No));
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name));
                    }
                    if (!string.IsNullOrEmpty(model.document_Status.ToString()))
                    {
                        query = query.Where(c => c.Document_Status == (model.document_Status));
                    }

                    if (!string.IsNullOrEmpty(model.processStatus_Name))
                    {
                        int DocumentStatue = 0;

                        var StatusName = new List<ProcessStatusViewModel>();

                        var StatusModel = new ProcessStatusViewModel();

                        StatusModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                        StatusModel.processStatus_Name = model.processStatus_Name;

                        //GetConfig
                        StatusName = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("processStatus"), StatusModel.sJson());

                        if (StatusName.Count > 0)
                        {
                            DocumentStatue = StatusName.FirstOrDefault().processStatus_Id.sParse<int>();
                        }

                        query = query.Where(c => c.Document_Status == DocumentStatue);
                    }

                    if (!string.IsNullOrEmpty(model.documentType_Index.ToString()) && model.documentType_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        query = query.Where(c => c.DocumentType_Index == (model.documentType_Index));
                    }

                    if (!string.IsNullOrEmpty(model.goodsReceive_Date) && !string.IsNullOrEmpty(model.goodsReceive_Date_To))
                    {
                        var dateStart = model.goodsReceive_Date.toBetweenDate();
                        var dateEnd = model.goodsReceive_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date >= dateStart.start && c.GoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsReceive_Date))
                    {
                        var GoodsReceive_date_From = model.goodsReceive_Date.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date >= GoodsReceive_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsReceive_Date_To))
                    {
                        var goodsReceive_date_To = model.goodsReceive_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date <= goodsReceive_date_To.start);
                    }

                }
                //BasicSearch
                else
                {
                    if (!string.IsNullOrEmpty(model.plan_no))
                    {
                        query = query.Where(c => c.Ref_Plan_No.Contains(model.plan_no));
                    }

                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.GoodsReceive_No.Contains(model.key));
                    }

                    if (!string.IsNullOrEmpty(model.owner_Name))
                    {
                        query = query.Where(c => c.Owner_Name.Contains(model.owner_Name)
                                            //|| c.Owner_Name.Contains(model.key)
                                            //|| c.Create_By.Contains(model.key)
                                            //|| c.DocumentRef_No1.Contains(model.key)
                                            //|| c.DocumentType_Name.Contains(model.key)
                                            );
                    }

                    if (!string.IsNullOrEmpty(model.goodsReceive_Date) && !string.IsNullOrEmpty(model.goodsReceive_Date_To))
                    {
                        var dateStart = model.goodsReceive_Date.toBetweenDate();
                        var dateEnd = model.goodsReceive_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date >= dateStart.start && c.GoodsReceive_Date <= dateEnd.end);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsReceive_Date))
                    {
                        var goodsReceive_date_From = model.goodsReceive_Date.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date >= goodsReceive_date_From.start);
                    }
                    else if (!string.IsNullOrEmpty(model.goodsReceive_Date_To))
                    {
                        var goodsReceive_date_To = model.goodsReceive_Date_To.toBetweenDate();
                        query = query.Where(c => c.GoodsReceive_Date <= goodsReceive_date_To.start);
                    }

                    var statusModels = new List<int?>();
                    var sortModels = new List<SortModel>();

                    if (model.status.Count > 0)
                    {
                        foreach (var item in model.status)
                        {
                            statusModels.Add(item.value);
                            //if (item.value == 0)
                            //{
                            //    statusModels.Add(0);
                            //}
                            //if (item.value == 1)
                            //{
                            //    statusModels.Add(1);
                            //}
                            //if (item.value == 2)
                            //{
                            //    statusModels.Add(2);
                            //}
                            //if (item.value == 3)
                            //{
                            //    statusModels.Add(3);
                            //}
                            //if (item.value == -1)
                            //{
                            //    statusModels.Add(-1);
                            //}
                            //if (item.value == -2)
                            //{
                            //    statusModels.Add(-2);
                            //}
                        }
                        query = query.Where(c => statusModels.Contains(c.Document_Status));
                    }

                    if (model.sort.Count > 0)
                    {
                        foreach (var item in model.sort)
                        {

                            if (item.value == "GoodsReceive_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "GoodsReceive_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Ref_Plan_No")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Ref_Plan_No",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "DocumentType_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "DocumentType_Name",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Owner_Name")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Owner_Name",
                                    Sort = "desc"
                                });
                            }
                            if (item.value == "Product_Lot")
                            {
                                sortModels.Add(new SortModel
                                {
                                    ColId = "Product_Lot",
                                    Sort = "desc"
                                });

                            }

                        }
                        query = query.KWOrderBy(sortModels);

                    }


                }
                //var perpages = model.PerPage == 0 ? query.ToList() : query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).Skip((model.CurrentPage - 1) * model.PerPage).Take(model.PerPage).ToList();


                var perpages = new List<View_filterGR>();

                var TotalRow = new List<View_filterGR>();


                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                if (model.sort.Count > 0)
                {
                    perpages = query.ToList();
                }
                else
                {
                    perpages = query.OrderByDescending(c => c.Create_Date).ToList();
                }


                var result = new List<SearchGRModel>();
                foreach (var item in perpages)
                {
                    var resultItem = new SearchGRModel();

                    var Document_Status = item.Document_Status.ToString();
                    //var Putaway_Status = item.Putaway_Status.ToString();

                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.goodsReceive_No = item.GoodsReceive_No;
                    resultItem.goodsReceive_Date = item.GoodsReceive_Date.toString();
                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentType_Name = item.DocumentType_Name;

                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.owner_Index = item.Owner_Index;

                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;

                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.create_By = item.Create_By;
                    resultItem.document_Status = item.Document_Status;

                    resultItem.create_Date = item.Create_Date.toString();
                    resultItem.processStatus_Name = Process.Where(a => a.processStatus_Id == Document_Status).Select(c => c.processStatus_Name).FirstOrDefault();

                    var checkPutaway = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index).ToList();

                    var Putaway_Status = "";

                    if (checkPutaway.Count <= 0)
                    {
                        Putaway_Status = "0";
                    }
                    else
                    {
                        var chkPutaway = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == item.GoodsReceive_Index && (c.Putaway_Status == 0 || c.Putaway_Status == null)).FirstOrDefault();

                        if (chkPutaway == null)
                        {
                            Putaway_Status = "1";
                        }
                        else
                        {
                            Putaway_Status = "0";
                        }

                    }

                    resultItem.statusPutaway = ProcessPutaway.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();

                    resultItem.document_Status = item.Document_Status;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.ref_Plan_No = item.Ref_Plan_No;
                    //resultItem.putaway_Status = item.Putaway_Status;
                    result.Add(resultItem);

                }

                var count = TotalRow.Count;
                var actionResultGR = new actionResultGRViewModel();
                actionResultGR.itemsGR = result.ToList();
                actionResultGR.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage };

                return actionResultGR;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TagItemViewModel> getDeleteScan(TagItemViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new List<TagItemViewModel>();
                    var GoodsReceive_Index = new SqlParameter("GoodsReceive_Index", data.GoodsReceiveIndex);
                    var Tag_Index = new SqlParameter("Tag_Index", data.TagIndex);
                    var TagItem_Index = new SqlParameter("TagItem_Index", data.TagItemIndex);
                    var Cancel_By = new SqlParameter("Cancel_By", data.CancelBy);
                    var rowsAffected = context.Database.ExecuteSqlCommand("sp_Delete_TagItem @GoodsReceive_Index,@Tag_Index,@TagItem_Index,@Cancel_By", GoodsReceive_Index, Tag_Index, TagItem_Index, Cancel_By);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String AutoScanReceive(GoodsReceiveTagItemViewModel data)
        {
            try
            {

                using (var context = new GRDbContext())
                {
                    string msg = "";

                    string pstring1 = "";
                    pstring1 += " and GoodsReceive_Index = '" + data.goodsReceive_Index + "'";
                    pstring1 += " and Tag_Status != -1 ";
                    var strwhere1 = new SqlParameter("@strwhere", pstring1);
                    var queryResult = context.wm_TagItem.FromSql("sp_GetTagItem_GrConfirm @strwhere", strwhere1).ToList();
                    {

                        var result = new List<GoodsReceiveTagItemViewModel>();
                        foreach (var item in queryResult)
                        {
                            var resultItem = new GoodsReceiveTagItemViewModel();
                            resultItem.goodsReceive_Index = item.GoodsReceive_Index;

                            result.Add(resultItem);
                        }
                    }
                    if (queryResult.Count != 0)
                    {
                        return msg;
                    }
                    string TagNumber = "1";

                    var DocumentType_Index = new SqlParameter("@DocumentType_Index", "CB8FEA3E-0683-44B8-A05F-BDB358ABF8D0");

                    var DocDate = new SqlParameter("@DocDate", DateTime.Now.toString().toDate());

                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    resultParameter.Size = 2000; // some meaningfull value
                    resultParameter.Direction = ParameterDirection.Output;
                    context.Database.ExecuteSqlCommand("EXEC sp_Gen_DocumentNumber @DocumentType_Index , @DocDate ,@txtReturn OUTPUT", DocumentType_Index, DocDate, resultParameter);
                    //var result = resultParameter.Value;
                    TagNumber = resultParameter.Value.ToString();
                    msg += "LPN : " + TagNumber;
                    var PalletIndex = Guid.NewGuid();
                    int tagStatus = 0;
                    var Pallet_No = new SqlParameter("Pallet_No", "");
                    var Pallet_Index = new SqlParameter("Pallet_Index", PalletIndex);

                    var queryResults = context.IM_GoodsReceiveItem.FromSql("sp_GetGoodsReceiveItemById {0}", data.goodsReceive_Index).Where(c => c.Document_Status != -1).ToList();
                    // save LPN
                    data.tag_Index = Guid.NewGuid();
                    var Tag_Index = new SqlParameter("Tag_Index", data.tag_Index);
                    var Tag_No = new SqlParameter("Tag_No", TagNumber);
                    var TagRef_No1 = new SqlParameter("TagRef_No1", "");
                    var TagRef_No2 = new SqlParameter("TagRef_No2", "");
                    var TagRef_No3 = new SqlParameter("TagRef_No3", "");
                    var TagRef_No4 = new SqlParameter("TagRef_No4", "");
                    var TagRef_No5 = new SqlParameter("TagRef_No5", "");
                    var Tag_Status = new SqlParameter("Tag_Status", tagStatus);
                    var UDF_1 = new SqlParameter("UDF_1", "");
                    var UDF_2 = new SqlParameter("UDF_2", "");
                    var UDF_3 = new SqlParameter("UDF_3", "");
                    var UDF_4 = new SqlParameter("UDF_4", "");
                    var UDF_5 = new SqlParameter("UDF_5", "");
                    var Create_By = new SqlParameter("Create_By", data.create_By);
                    var Create_Date = new SqlParameter("Create_Date", DateTime.Now);
                    var Update_By = new SqlParameter("Update_By", "");
                    var Update_Date = new SqlParameter("Update_Date", DateTime.Now);
                    var Cancel_By = new SqlParameter("Cancel_By", "");
                    var Cancel_Date = new SqlParameter("Cancel_Date", DateTime.Now);
                    var rowsAffected = context.Database.ExecuteSqlCommand("sp_Save_wm_Tag  @Tag_Index,@Tag_No,@Pallet_No,@Pallet_Index,@TagRef_No1,@TagRef_No2,@TagRef_No3,@TagRef_No4,@TagRef_No5,@Tag_Status,@UDF_1,@UDF_2,@UDF_3,@UDF_4,@UDF_5,@Create_By,@Create_Date,@Update_By,@Update_Date,@Cancel_By,@Cancel_Date ", Tag_Index, Tag_No, Pallet_No, Pallet_Index, TagRef_No1, TagRef_No2, TagRef_No3, TagRef_No4, TagRef_No5, Tag_Status, UDF_1, UDF_2, UDF_3, UDF_4, UDF_5, Create_By, Create_Date, Update_By, Update_Date, Cancel_By, Cancel_Date);

                    //if (rowsAffected != 0)
                    //{
                    //    msg += " OK " + Environment.NewLine;
                    //}
                    //else
                    //{
                    //    msg += " Error " + Environment.NewLine;
                    //}



                    {
                        var result = new List<GoodsReceiveItemViewModel>();
                        foreach (var item in queryResults)
                        {
                            var ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Product_Index)");
                            var ColumnName2 = new SqlParameter("@ColumnName2", "Convert(Nvarchar(50),isExpDate)");
                            var ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),isMfgDate)");
                            var ColumnName4 = new SqlParameter("@ColumnName4", "''");
                            var ColumnName5 = new SqlParameter("@ColumnName5", "''");
                            var TableName = new SqlParameter("@TableName", "ms_Product");
                            var Where = new SqlParameter("@Where", "Where Product_Index ='" + item.Product_Index + "'");
                            var DataProduct = context.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();

                            var contextT = new GRDbContext();

                            ColumnName1 = new SqlParameter("@ColumnName1", "Convert(Nvarchar(50),Product_Index)");
                            ColumnName2 = new SqlParameter("@ColumnName2", "''");
                            ColumnName3 = new SqlParameter("@ColumnName3", "Convert(Nvarchar(50),isnull(ProductItemLife_Y,0))");
                            ColumnName4 = new SqlParameter("@ColumnName4", "Convert(Nvarchar(50),isnull(ProductItemLife_M,0))");
                            ColumnName5 = new SqlParameter("@ColumnName5", "Convert(Nvarchar(50),isnull(ProductItemLife_D,0))");
                            TableName = new SqlParameter("@TableName", "ms_Product");
                            Where = new SqlParameter("@Where", "Where Product_Index ='" + item.Product_Index + "'");
                            var DataProductItemLief = contextT.GetValueByColumn.FromSql("sp_GetValueByColumn @ColumnName1,@ColumnName2,@ColumnName3,@ColumnName4,@ColumnName5,@TableName,@Where ", ColumnName1, ColumnName2, ColumnName3, ColumnName4, ColumnName5, TableName, Where).ToList();
                            //int d = Convert.ToInt32(DataProduct[0].dataincolumn5);
                            //int m = Convert.ToInt32(DataProduct[0].dataincolumn4);
                            //int y = Convert.ToInt32(DataProduct[0].dataincolumn3);

                            // save TAGITEM
                            //var tagStatus = 0;
                            Tag_Index = new SqlParameter("Tag_Index", data.tag_Index);
                            data.tagItem_Index = Guid.NewGuid();
                            var TagItem_Index = new SqlParameter("TagItem_Index", data.tagItem_Index);
                            Tag_No = new SqlParameter("Tag_No", TagNumber);
                            var GoodsReceive_Index = new SqlParameter("GoodsReceive_Index", data.goodsReceive_Index);
                            var GoodsReceiveItem_Index = new SqlParameter("GoodsReceiveItem_Index", item.GoodsReceiveItem_Index);
                            var Product_Index = new SqlParameter("Product_Index", item.Product_Index);
                            var Product_Id = new SqlParameter("Product_Id", item.Product_Id);
                            var Product_Name = new SqlParameter("Product_Name", item.Product_Name);
                            if (item.Product_SecondName == null)
                            {
                                item.Product_SecondName = "";
                            }
                            var Product_SecondName = new SqlParameter("Product_SecondName", item.Product_SecondName);
                            if (item.Product_ThirdName == null)
                            {
                                item.Product_ThirdName = "";
                            }
                            var Product_ThirdName = new SqlParameter("Product_ThirdName", item.Product_ThirdName);
                            var Product_Lot = new SqlParameter("Product_Lot", item.Product_Lot != null ? item.Product_Lot : item.Product_Lot = "");
                            var ItemStatus_Index = new SqlParameter("ItemStatus_Index", item.ItemStatus_Index);
                            var ItemStatus_Id = new SqlParameter("ItemStatus_Id", item.ItemStatus_Id);
                            var ItemStatus_Name = new SqlParameter("ItemStatus_Name", item.ItemStatus_Name);
                            var Qty = new SqlParameter("Qty", item.Qty);
                            var Ratio = new SqlParameter("Ratio", item.Ratio);
                            var TotalQty = new SqlParameter("TotalQty", (item.Qty * item.Ratio));
                            var ProductConversion_Index = new SqlParameter("ProductConversion_Index", item.ProductConversion_Index);
                            var ProductConversion_Id = new SqlParameter("ProductConversion_Id", item.ProductConversion_Id);
                            var ProductConversion_Name = new SqlParameter("ProductConversion_Name", item.ProductConversion_Name);
                            var Weight = new SqlParameter("Weight", item.Weight);
                            var Volume = new SqlParameter("Volume", item.Volume);

                            var MFG_Date = new SqlParameter("MFG_Date", DBNull.Value);
                            if (item.MFG_Date.HasValue)
                            {
                                MFG_Date = new SqlParameter("MFG_Date", item.MFG_Date.Value.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                if (DataProduct[0].dataincolumn3 == "1")
                                {
                                    //MFG_Date = new SqlParameter("MFG_Date", DateTime.Now);
                                    MFG_Date = new SqlParameter("MFG_Date", DBNull.Value);

                                }
                                else
                                {
                                    MFG_Date = new SqlParameter("MFG_Date", DBNull.Value);
                                }
                            }

                            var EXP_Date = new SqlParameter("EXP_Date", DBNull.Value);
                            if (item.EXP_Date.HasValue)
                            {
                                EXP_Date = new SqlParameter("EXP_Date", item.EXP_Date.Value.ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                if (DataProduct[0].dataincolumn2 == "1")
                                {

                                    //EXP_Date = new SqlParameter("EXP_Date", DateTime.Now.AddDays(Convert.ToInt32(DataProductItemLief[0].dataincolumn5)).AddMonths(Convert.ToInt32(DataProductItemLief[0].dataincolumn4)).AddYears(Convert.ToInt32(DataProductItemLief[0].dataincolumn3)));
                                    EXP_Date = new SqlParameter("EXP_Date", DBNull.Value);
                                }
                                else
                                {
                                    EXP_Date = new SqlParameter("EXP_Date", DBNull.Value);
                                }
                            }

                            TagRef_No1 = new SqlParameter("TagRef_No1", "");
                            TagRef_No2 = new SqlParameter("TagRef_No2", "");
                            TagRef_No3 = new SqlParameter("TagRef_No3", "");
                            TagRef_No4 = new SqlParameter("TagRef_No4", "");
                            TagRef_No5 = new SqlParameter("TagRef_No5", "");
                            Tag_Status = new SqlParameter("Tag_Status", tagStatus);
                            if (item.UDF_1 == null)
                            {
                                item.UDF_1 = "";
                            }
                            UDF_1 = new SqlParameter("UDF_1", item.UDF_1);
                            if (item.UDF_2 == null)
                            {
                                item.UDF_2 = "";
                            }
                            //UDF_2 = new SqlParameter("UDF_2", item.UDF_2);
                            //if (item.UDF_3 == null)
                            //{
                            //    item.UDF_3 = "";
                            //}
                            //UDF_3 = new SqlParameter("UDF_3", item.UDF_3);
                            //if (item.UDF_4 == null)
                            //{
                            //    item.UDF_4 = "";
                            //}
                            //UDF_4 = new SqlParameter("UDF_4", item.UDF_4);
                            //if (item.UDF_5 == null)
                            //{
                            //    item.UDF_5 = "";
                            //}
                            UDF_2 = new SqlParameter("UDF_2", "");
                            UDF_3 = new SqlParameter("UDF_3", "");
                            UDF_4 = new SqlParameter("UDF_4", "");
                            UDF_5 = new SqlParameter("UDF_5", "");
                            Create_By = new SqlParameter("Create_By", data.create_By);
                            Create_Date = new SqlParameter("Create_Date", DateTime.Now);
                            Update_By = new SqlParameter("Update_By", "");
                            Update_Date = new SqlParameter("Update_Date", DateTime.Now);
                            Cancel_By = new SqlParameter("Cancel_By", "");
                            Cancel_Date = new SqlParameter("Cancel_Date", DateTime.Now);

                            var rowsAffected2 = context.Database.ExecuteSqlCommand("sp_Save_wm_TagItem  @TagItem_Index,@Tag_Index,@Tag_No,@GoodsReceive_Index,@GoodsReceiveItem_Index,@Product_Index,@Product_Id,@Product_Name,@Product_SecondName,@Product_ThirdName,@Product_Lot,@ItemStatus_Index,@ItemStatus_Id,@ItemStatus_Name,@Qty,@Ratio,@TotalQty,@ProductConversion_Index,@ProductConversion_Id,@ProductConversion_Name,@Weight,@Volume,@MFG_Date,@EXP_Date,@TagRef_No1,@TagRef_No2,@TagRef_No3,@TagRef_No4,@TagRef_No5,@Tag_Status,@UDF_1,@UDF_2,@UDF_3,@UDF_4,@UDF_5,@Create_By,@Create_Date,@Update_By,@Update_Date,@Cancel_By,@Cancel_Date ", TagItem_Index, Tag_Index, Tag_No, GoodsReceive_Index, GoodsReceiveItem_Index, Product_Index, Product_Id, Product_Name, Product_SecondName, Product_ThirdName, Product_Lot, ItemStatus_Index, ItemStatus_Id, ItemStatus_Name, Qty, Ratio, TotalQty, ProductConversion_Index, ProductConversion_Id, ProductConversion_Name, Weight, Volume, MFG_Date, EXP_Date, TagRef_No1, TagRef_No2, TagRef_No3, TagRef_No4, TagRef_No5, Tag_Status, UDF_1, UDF_2, UDF_3, UDF_4, UDF_5, Create_By, Create_Date, Update_By, Update_Date, Cancel_By, Cancel_Date);


                        }
                        //Clear UserAssign
                        String SqlUpdateGoodsReceive = " Update im_GoodsReceive set " +
                                           " UserAssign =  '' " +
                                           " where Convert(Varchar(200),GoodsReceive_Index) ='" + data.goodsReceive_Index + "'";
                        var row = context.Database.ExecuteSqlCommand(SqlUpdateGoodsReceive);

                        return msg;


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String updateUserAssign(GoodsReceiveDocViewModel item)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {

                var GoodsReceive = db.IM_GoodsReceive.Find(item.goodsReceive_Index);

                if (GoodsReceive != null)
                {
                    GoodsReceive.UserAssign = item.userAssign;

                    var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                    try
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("UpdateUserAssign", msglog);
                        transaction.Rollback();
                        throw exy;
                    }
                }

                var FindUser = db.IM_GoodsReceive.Find(item.goodsReceive_Index);

                return FindUser.UserAssign.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String updateUserAssignScanReceive(GoodsReceiveDocViewModel item)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new GoodsReceiveDocViewModel();

                    var contextM = new GRDbContext();

                    String SqlUpdatePlanReceive = " Update im_GoodsReceive set " +
                                               " UserAssign =  N'" + item.userAssign + "'" +
                                               " where Convert(Varchar(200),GoodsReceive_Index) ='" + item.goodsReceive_Index + "'";
                    var row = context.Database.ExecuteSqlCommand(SqlUpdatePlanReceive);

                    //String Sql1 = " Select  DATEDIFF(minute,UserAssignTimeStamp, GETDATE()) as UserAssignTime " +
                    //                            " From wm_TagItem" +
                    //                            " where Convert(Varchar(200),GoodsReceive_Index) ='" + item.goodsReceive_Index + "'";
                    //var row1 = context.Database.ExecuteSqlCommand(Sql1);

                    var strwhere = new SqlParameter("@strwhere", " and GoodsReceive_Index ='" + item.goodsReceive_Index + "'");
                    var CheckUser = contextM.IM_GoodsReceive.FromSql("sp_GetGoodsReceive @strwhere", strwhere).FirstOrDefault();

                    return CheckUser.UserAssign.ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String checkUserAssign(GoodsReceiveDocViewModel item)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new GoodsReceiveDocViewModel();

                    var contextM = new GRDbContext();
                    //var strwhere = new SqlParameter("@strwhere", " and GoodsReceive_Index = '" + item.goodsReceive_Index + "'");
                    var CheckUser = contextM.IM_GoodsReceive.Where(c => c.GoodsReceive_Index == item.goodsReceive_Index).FirstOrDefault();
                    var UserAssign = CheckUser.UserAssign ?? "";
                    //if (!string.IsNullOrEmpty(CheckUser.UserAssign.ToString().Replace(null, "")))
                    //{
                    //    UserAssign = CheckUser.UserAssign.ToString().Replace(null, "");
                    //}

                    return UserAssign;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String deleteUserAssign(GoodsReceiveDocViewModel item)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                if (!string.IsNullOrEmpty(item.goodsReceive_Index.ToString().Replace("00000000-0000-0000-0000-000000000000", "")))
                {
                    var GoodsReceive = db.IM_GoodsReceive.Find(item.goodsReceive_Index);

                    if (GoodsReceive != null)
                    {
                        GoodsReceive.UserAssign = "";

                        var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                        try
                        {
                            db.SaveChanges();
                            transaction.Commit();
                        }

                        catch (Exception exy)
                        {
                            msglog = State + " ex Rollback " + exy.Message.ToString();
                            olog.logging("deleteUserAssign", msglog);
                            transaction.Rollback();
                            throw exy;
                        }
                    }

                    var FindUser = db.IM_GoodsReceive.Find(item.goodsReceive_Index);

                    return FindUser.UserAssign.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String updateUserAssignKey(GoodsReceiveDocViewModel item)
        {
            try
            {
                using (var context = new GRDbContext())
                {
                    var result = new GoodsReceiveDocViewModel();

                    var contextM = new GRDbContext();

                    String SqlUpdatePlanReceive = " UPDATE im_PlanGoodsReceive set " +
                                               " UserAssignKey =  N'" + item.userAssign + "'" +
                                               " where Convert(Varchar(200),PlanGoodsReceive_Index) ='" + item.goodsReceive_Index + "'";

                    var row = context.Database.ExecuteSqlCommand(SqlUpdatePlanReceive);

                    return "";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean CheckPalletLocationLPN(GoodsReceiveTagItemPutawayLpnViewModel model)
        {
            try
            {
                using (var context = new GRDbContext())
                {



                    var LocationName = new SqlParameter("@LocationName", model.LocationName);
                    var resultParameter = new SqlParameter("@txtReturn", SqlDbType.NVarChar);
                    resultParameter.Size = 2000; // some meaningfull value
                    resultParameter.Direction = ParameterDirection.Output;
                    context.Database.ExecuteSqlCommand("EXEC get_ViewCheckPlletLocation @LocationName  ,@txtReturn OUTPUT", LocationName, resultParameter);
                    var chkLPN = resultParameter.Value.ToString();

                    if (chkLPN == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<ProductDetailViewModel> GetProductBarcodes(ProductBarcodeModel model)

        {
            try
            {
                using (var context = new GRDbContext())
                {
                    #region Scan ProductBarcode
                    var result = new List<ProductDetailViewModel>();

                    var filterModel = new ProductDetailViewModel();

                    filterModel.owner_Index = model.owner_Index;
                    filterModel.productConversionBarcode = model.productBarcode;
                    //if (!string.IsNullOrEmpty(model.product_Index.ToString()) && model.product_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    //{
                    //    filterModel.product_Index = model.product_Index;
                    //}

                    //GetConfig
                    result = utils.SendDataApi<List<ProductDetailViewModel>>(new AppSettingConfig().GetUrl("productDetail"), filterModel.sJson());

                    #endregion





                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> PlanGRfilter(ItemListViewModel data)
        {
            try
            {
                var items = new List<ItemListViewModel>();

                #region AutoPlanGR Page Search
                if (data.chk == 1)
                {
                    var query = db.View_GrProcessStatus.AsQueryable();

                    if (data.key == "-")
                    {

                    }
                    else if (!string.IsNullOrEmpty(data.key))
                    {
                        query = query.Where(c => c.Ref_Document_No.Contains(data.key));

                    }


                    var result = query.Select(c => new { c.Ref_Document_Index, c.Ref_Document_No }).Distinct().Take(10).ToList();

                    foreach (var item in result)
                    {
                        var resultItem = new ItemListViewModel
                        {
                            index = item.Ref_Document_Index,
                            id = item.Ref_Document_No,
                            name = item.Ref_Document_No

                        };

                        items.Add(resultItem);
                    }
                }
                #endregion
                #region AutoPlanGR Page Create
                else if (data.chk == 2)
                {
                    var filterModel = new ItemListViewModel();
                    if (!string.IsNullOrEmpty(data.key))
                    {
                        filterModel.key = data.key;
                    }

                    //GetConfig
                    items = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("PlanGRfilter"), filterModel.sJson());


                }
                #endregion
                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> Ownerfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("OwnerFilter"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoGI_SUB(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoGI_SUB"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoPlanGI_SUB(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();
                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoPlanGI_SUB"), filterModel.sJson());
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> Warehousefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("Warehousefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ItemListViewModel> Statusfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                filterModel.chk = data.chk;
                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("Statusfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> DocumentTypefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                filterModel.chk = data.chk;
                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("DocumentTypefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoProduct(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                filterModel.chk = data.chk;
                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoProduct"), filterModel.sJson());


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoSKU(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }
                filterModel.chk = data.chk;
                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoSKU"), filterModel.sJson());


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> VahicleTypefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("VehicleTypefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> AutoFilterGoodsReceive(ItemListViewModel data)
        {
            try
            {

                using (var context = new GRDbContext())
                {
                    var query = db.IM_GoodsReceive.AsQueryable();

                    if (!string.IsNullOrEmpty(data.key))
                    {
                        query = query.Where(c => c.GoodsReceive_No.Contains(data.key));

                    }

                    var items = new List<ItemListViewModel>();

                    var result = query.Select(c => new { c.GoodsReceive_Index, c.GoodsReceive_No }).Distinct().Take(10).ToList();

                    foreach (var item in result)
                    {
                        var resultItem = new ItemListViewModel
                        {
                            index = item.GoodsReceive_Index,
                            id = item.GoodsReceive_No,
                            name = item.GoodsReceive_No

                        };

                        items.Add(resultItem);
                    }
                    return items;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ItemListViewModel> DockDoorfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("DockDoorfilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> ContainerTypefilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("ContainerTypefilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public actionResultPlanGRViewModels PlanGRfilterPopup(PlanGoodsReceivePopupViewModels data)
        {
            try
            {
                var result = new List<PlanGoodsReceivePopupViewModels>();

                var filterModel = new PlanGoodsReceivePopupViewModels();

                if (!string.IsNullOrEmpty(data.planGoodsReceive_No))
                {
                    filterModel.planGoodsReceive_No = data.planGoodsReceive_No;
                }
                else if (!string.IsNullOrEmpty(data.vendor_Name))
                {
                    filterModel.vendor_Name = data.vendor_Name;
                }
                else if (!string.IsNullOrEmpty(data.planGoodsReceive_Date))
                {
                    filterModel.planGoodsReceive_Date = data.planGoodsReceive_Date;
                }
                else if (!string.IsNullOrEmpty(data.planGoodsReceive_Due_Date))
                {
                    filterModel.planGoodsReceive_Due_Date = data.planGoodsReceive_Due_Date;
                }
                else if (!string.IsNullOrEmpty(data.owner_Name))
                {
                    filterModel.owner_Name = data.owner_Name;
                }
                else if (!string.IsNullOrEmpty(data.owner_Index.ToString()))
                {
                    filterModel.owner_Index = data.owner_Index;
                }
                filterModel.documentType_Index = data.documentType_Index;
                filterModel.chk = data.chk;

                //GetConfig
                result = utils.SendDataApi<List<PlanGoodsReceivePopupViewModels>>(new AppSettingConfig().GetUrl("PopupPlanGR"), filterModel.sJson()).Where(c => c.document_Status != -1 && c.document_Status != -2 && c.document_Status != 0).ToList();


                var items = data.PerPage == 0 ? result.ToList() : result.OrderByDescending(o => o.planGoodsReceive_Due_Date).Skip((data.CurrentPage - 1) * data.PerPage).Take(data.PerPage).ToList();

                var listPGR_Index = db.IM_GoodsReceiveItem.Where(c => (c.UDF_1 == "X") && c.Document_Status != -1).GroupBy(g => g.Ref_Document_Index).ToList();

                if (listPGR_Index.Count > 0)
                {
                    items.RemoveAll(c => listPGR_Index.Select(s => s.Key).Contains(c.planGoodsReceive_Index));
                }

                //var checkudf1 = db.IM_GoodsReceiveItem.Where(c => items.Select(s => s.planGoodsReceive_Index).Contains(c.Ref_Document_Index.sParse<Guid>()) && !(c.UDF_1 == "" || c.UDF_1 == null) && c.Document_Status != -1).GroupBy(g => g.Ref_Document_Index).ToList();

                //if (checkudf1.Count() > 0)
                //{
                //    items.RemoveAll(c => checkudf1.Select(s => s.Key).Contains(c.planGoodsReceive_Index));
                //}

                var count = result.Count;
                var actionResultPlanGRPopup = new actionResultPlanGRViewModels();
                actionResultPlanGRPopup.itemsPlanGR = items;
                actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };

                return actionResultPlanGRPopup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultProductViewModel ProductFilterPopup(ProductViewModel data)
        {
            try
            {
                var result = new List<ProductViewModel>();

                var filterModel = new ProductViewModel();

                if (!string.IsNullOrEmpty(data.product_Id))
                {
                    filterModel.product_Id = data.product_Id;
                }

                if (!string.IsNullOrEmpty(data.product_SecondName))
                {
                    filterModel.product_SecondName = data.product_SecondName;
                }

                if (data.owner_Index != null && data.owner_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    filterModel.owner_Index = data.owner_Index;
                }

                filterModel.NumPerPage = data.NumPerPage;
                filterModel.CurrentPage = data.CurrentPage;
                //GetConfig
                result = utils.SendDataApi<List<ProductViewModel>>(new AppSettingConfig().GetUrl("ProductPopupFilter"), filterModel.sJson());


                var count = result.Count;
                var actionResultProduct = new ProductViewModel.actionResultProductViewModel();
                actionResultProduct.itemsProduct = result.ToList();
                actionResultProduct.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage };


                return actionResultProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlanGoodsReceiveItemViewModel> ProductLineitemPopup(PlanGoodsReceiveItemViewModel data)
        {
            try
            {
                using (var context = new GRDbContext())
                {

                    var result = new List<PlanGoodsReceiveItemViewModel>();

                    var filterModel = new PlanGoodsReceiveItemViewModel();

                    if (!string.IsNullOrEmpty(data.product_Index.ToString()) && data.product_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        filterModel.product_Index = data.product_Index;
                    }
                    if (!string.IsNullOrEmpty(data.planGoodsReceive_Index.ToString()) && data.planGoodsReceive_Index.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        filterModel.planGoodsReceive_Index = data.planGoodsReceive_Index;
                    }

                    //GetConfig
                    result = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("PopupPlanGRIfilter"), filterModel.sJson());

                    foreach (var item in result)
                    {
                        var TotalGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == data.planGoodsReceive_Index && c.Ref_DocumentItem_Index == item.planGoodsReceiveItem_Index && c.Document_Status != -1).ToList();
                        if (TotalGR.Count > 0)
                        {
                            item.totalQty = (item.totalQty - TotalGR.Select(s => s.TotalQty).Sum());
                            var qty = (item.totalQty / item.ratio);
                            item.qty = qty;
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
        public List<ItemListViewModel> autobasicSuggestion(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            var result = new List<ItemListViewModel>();
            var filterModel = new ItemListViewModel();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query1 = db.View_GrProcessStatus.Where(c => c.GoodsReceive_No.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        name = s.GoodsReceive_No,
                        index = s.GoodsReceive_Index

                    }).Distinct();

                    //var query2 = db.View_GrProcessStatus.Where(c => c.Owner_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    //{
                    //    name = s.Owner_Name,
                    //    index = s.Owner_Index,
                    //}).Distinct();

                    //var query3 = db.View_GrProcessStatus.Where(c => c.DocumentType_Name.Contains(data.key)).Select(s => new ItemListViewModel
                    //{
                    //    name = s.DocumentType_Name,
                    //    index = s.DocumentType_Index

                    //}).Distinct();

                    //var query4 = db.View_GrProcessStatus.Where(c => c.Ref_Document_No.Contains(data.key)).Select(s => new ItemListViewModel
                    //{
                    //    name = s.Ref_Document_No,
                    //    index = s.Ref_Document_Index
                    //}).Distinct();

                    //filterModel.key = data.key;

                    //var query5 = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("getProduct"), filterModel.sJson());


                    //var query = query1.Union(query2).Union(query2).Union(query3).Union(query4);

                    var query = query1;


                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

                return items;


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


        public List<ProcessStatusViewModel> dropdownProcessStatus(ProcessStatusViewModel data)
        {
            try
            {
                var result = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();


                filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                //GetConfig
                result = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatusDropDown"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ItemListViewModel> autoWHOwnerfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("WHOwnerFilter"), filterModel.sJson());

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> Venderfilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();

                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("VendorFilter"), filterModel.sJson());

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProductConversionViewModelDoc> dropdownProductconversion(ProductConversionViewModelDoc data)
        {
            try
            {
                var result = new List<ProductConversionViewModelDoc>();

                var filterModel = new ProductConversionViewModelDoc();

                if (!string.IsNullOrEmpty(data.product_Index.ToString()))
                {
                    filterModel.product_Index = data.product_Index;
                }
                //GetConfig
                result = utils.SendDataApi<List<ProductConversionViewModelDoc>>(new AppSettingConfig().GetUrl("dropdownProductconversion"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region dropdownItemStatus
        public List<ItemStatusDocViewModel> dropdownItemStatus(ItemStatusDocViewModel data)
        {
            try
            {
                var result = new List<ItemStatusDocViewModel>();

                var filterModel = new ItemStatusDocViewModel();

                //GetConfig
                result = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region dropdownStorageLoc
        public List<StorageLocViewModel> dropdownStorageLoc(StorageLocViewModel data)
        {
            try
            {
                var result = new List<StorageLocViewModel>();

                var filterModel = new StorageLocViewModel();

                if (data.warehouse_Index_To != new Guid("00000000-0000-0000-0000-000000000000".ToString()) && data.warehouse_Index_To != null)
                {
                    filterModel.warehouse_Index_To = data.warehouse_Index_To;
                }


                //GetConfig
                result = utils.SendDataApi<List<StorageLocViewModel>>(new AppSettingConfig().GetUrl("dropdownStorageLoc"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        public List<ItemListViewModel> autoInvoice(ItemListViewModel data)
        {
            try
            {
                var filterModel = new ItemListViewModel();
                var query = db.IM_GoodsReceive.AsQueryable();
                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Invoice_No.Contains(data.key));

                }
                var items = new List<ItemListViewModel>();
                var result = query.Select(c => new { c.Invoice_No }).Distinct().Take(10).ToList();
                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {

                        name = item.Invoice_No

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

        //public List<ItemListViewModel> autoProductLot(ItemListViewModel data)
        //{
        //    var items = new List<ItemListViewModel>();
        //    var result = new List<ItemListViewModel>();
        //    var filterModel = new ItemListViewModel();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(data.key))
        //        {
        //            var query = db.IM_GoodsReceiveItem.Where(c => c.Product_Lot.Contains(data.key)).Select(s => new ItemListViewModel
        //            {
        //                name = s.Product_Lot,
        //            }).Distinct();


        //            items = query.OrderBy(c => c.name).Take(10).ToList();
        //        }

        //        return items;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<ItemListViewModel> autoProductLot(ItemListViewModel data)
        {
            try
            {
                var filterModel = new ItemListViewModel();
                var query = db.IM_GoodsReceiveItem.AsQueryable();
                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.Product_Lot.Contains(data.key));

                }
                var items = new List<ItemListViewModel>();
                var result = query.Select(c => new { c.Product_Lot }).Distinct().Take(10).ToList();
                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {

                        name = item.Product_Lot

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

        //public List<ItemListViewModel> autoDocumentRef(ItemListViewModel data)
        //{
        //    var items = new List<ItemListViewModel>();
        //    var result = new List<ItemListViewModel>();
        //    var filterModel = new ItemListViewModel();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(data.key))
        //        {
        //            var query = db.IM_GoodsReceive.Where(c => c.DocumentRef_No1.Contains(data.key)).Select(s => new ItemListViewModel
        //            {
        //                name = s.DocumentRef_No1,
        //            }).Distinct();

        //            items = query.OrderBy(c => c.name).Take(10).ToList();
        //        }

        //        return items;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<ItemListViewModel> autoDocumentRef(ItemListViewModel data)
        {
            try
            {
                var filterModel = new ItemListViewModel();
                var query = db.IM_GoodsReceive.AsQueryable();
                if (data.key == "-")
                {


                }
                else if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.DocumentRef_No1.Contains(data.key));

                }
                var items = new List<ItemListViewModel>();
                var result = query.Select(c => new { c.DocumentRef_No1 }).Distinct().Take(10).ToList();
                foreach (var item in result)
                {
                    var resultItem = new ItemListViewModel
                    {

                        name = item.DocumentRef_No1

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

        public List<View_GoodsReceiveViewModel> configGoodsReceiveItemLocation(View_GoodsReceiveViewModel data)
        {
            try
            {


                var query = db.View_GoodsReceive.AsQueryable();

                if (data.goodsReceiveItem_Index != new Guid("00000000-0000-0000-0000-000000000000".ToString()))
                {
                    query = query.Where(c => c.GoodsReceiveItem_Index == data.goodsReceiveItem_Index);
                }

                if (data.tagItem_Index != new Guid("00000000-0000-0000-0000-000000000000".ToString()))
                {
                    query = query.Where(c => c.TagItem_Index == data.tagItem_Index);
                }

                var items = new List<View_GoodsReceiveViewModel>();

                var result = query.ToList();

                foreach (var item in result)
                {
                    var resultItem = new View_GoodsReceiveViewModel();

                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.goodsReceiveItem_Index = item.GoodsReceiveItem_Index;
                    resultItem.goodsReceiveItemLocation_Index = item.GoodsReceiveItemLocation_Index;
                    resultItem.goodsReceive_No = item.GoodsReceive_No;
                    resultItem.goodsReceive_Date = item.GoodsReceive_Date;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.product_Index = item.Product_Index;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.product_SecondName = item.Product_SecondName;
                    resultItem.product_ThirdName = item.Product_ThirdName;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.qty = item.Qty;
                    resultItem.ratio = item.Ratio;
                    resultItem.totalQty = item.TotalQty;
                    resultItem.location_Index = item.Location_Index;
                    resultItem.location_Id = item.Location_Id;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.mFG_Date = item.MFG_Date;
                    resultItem.eXP_Date = item.EXP_Date;
                    resultItem.tag_Index = item.Tag_Index;
                    resultItem.tagItem_Index = item.Tag_Index;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.weight = item.Weight;
                    resultItem.volume = item.Volume;


                    items.Add(resultItem);
                }
                return items;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoTag(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query = db.WM_Tag.Where(c => c.Tag_No.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        index = s.Tag_Index,
                        name = s.Tag_No,
                    }).Distinct();

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

                return items;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoGr(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                if (!string.IsNullOrEmpty(data.key))
                {
                    var query = db.IM_GoodsReceive.Where(c => c.GoodsReceive_No.Contains(data.key)).Select(s => new ItemListViewModel
                    {
                        index = s.GoodsReceive_Index,
                        name = s.GoodsReceive_No,
                    }).Distinct();

                    items = query.OrderBy(c => c.name).Take(10).ToList();
                }

                return items;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region im_GoodsReceive
        public List<GoodsReceiveViewModel> im_GoodsReceive(DocumentViewModel model)
        {
            try
            {
                var query = db.IM_GoodsReceive.AsQueryable();

                var result = new List<GoodsReceiveViewModel>();


                if (model.listDocumentViewModel.FirstOrDefault().document_Index != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Index).Contains(c.GoodsReceive_Index));
                }

                else if (model.listDocumentViewModel.FirstOrDefault().document_Status != null)
                {
                    query = query.Where(c => model.listDocumentViewModel.Select(s => s.document_Status).Contains(c.Document_Status));
                }

                var queryresult = query.ToList();

                foreach (var item in queryresult)
                {
                    var resultItem = new GoodsReceiveViewModel();
                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;
                    resultItem.documentType_Index = item.DocumentType_Index;
                    resultItem.documentType_Id = item.DocumentType_Id;
                    resultItem.documentType_Name = item.DocumentType_Name;
                    resultItem.goodsReceive_No = item.GoodsReceive_No;
                    resultItem.goodsReceive_Date = item.GoodsReceive_Date.toString();
                    resultItem.documentRef_No1 = item.DocumentRef_No1;
                    resultItem.documentRef_No2 = item.DocumentRef_No2;
                    resultItem.documentRef_No3 = item.DocumentRef_No3;
                    resultItem.documentRef_No4 = item.DocumentRef_No4;
                    resultItem.documentRef_No5 = item.DocumentRef_No5;
                    resultItem.document_Remark = item.Document_Remark;
                    resultItem.document_Status = item.Document_Status;
                    resultItem.uDF_1 = item.UDF_1;
                    resultItem.uDF_2 = item.UDF_2;
                    resultItem.uDF_3 = item.UDF_3;
                    resultItem.uDF_4 = item.UDF_4;
                    resultItem.uDF_5 = item.UDF_5;
                    resultItem.documentPriority_Status = item.DocumentPriority_Status;
                    resultItem.create_Date = item.Create_Date.toString();
                    resultItem.create_By = item.Create_By;
                    resultItem.update_Date = item.Update_Date.toString();
                    resultItem.update_By = item.Update_By;
                    resultItem.cancel_Date = item.Cancel_Date.toString();
                    resultItem.cancel_By = item.Cancel_By;
                    resultItem.warehouse_Index = item.Warehouse_Index;
                    resultItem.warehouse_Id = item.Warehouse_Id;
                    resultItem.warehouse_Name = item.Warehouse_Name;
                    resultItem.warehouse_Index_To = item.Warehouse_Index_To;
                    resultItem.warehouse_Id_To = item.Warehouse_Id_To;
                    resultItem.warehouse_Name_To = item.Warehouse_Name_To;
                    resultItem.userAssign = item.UserAssign;
                    resultItem.dockDoor_Index = item.DockDoor_Index;
                    resultItem.dockDoor_Id = item.DockDoor_Id;
                    resultItem.dockDoor_Name = item.DockDoor_Name;
                    resultItem.vehicleType_Index = item.VehicleType_Index;
                    resultItem.vehicleType_Id = item.VehicleType_Id;
                    resultItem.vehicleType_Name = item.VehicleType_Name;
                    resultItem.containerType_Index = item.ContainerType_Index;
                    resultItem.containerType_Id = item.ContainerType_Id;
                    resultItem.containerType_Name = item.ContainerType_Name;
                    resultItem.invoice_No = item.Invoice_No;
                    resultItem.vendor_Index = item.Vendor_Index;
                    resultItem.vendor_Id = item.Vendor_Id;
                    resultItem.vendor_Name = item.Vendor_Name;
                    resultItem.whOwner_Index = item.WHOwner_Index;
                    resultItem.whOwner_Id = item.WHOwner_Id;
                    resultItem.whOwner_Name = item.WHOwner_Name;

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


        #region popupGoodsReceivefilter
        public List<SearchGRModel> popupGoodsReceivefilter(SearchGRModel data)
        {
            try
            {

                var items = new List<SearchGRModel>();



                var query = db.View_GoodsReceivePop.AsQueryable();



                if (!string.IsNullOrEmpty(data.goodsReceive_No))
                {
                    query = query.Where(c => c.GoodsReceive_No == data.goodsReceive_No);
                }


                var result = query.Take(100).OrderByDescending(o => o.Create_Date).ToList();

                var ProcessStatus = new List<ProcessStatusViewModel>();

                var filterModel = new ProcessStatusViewModel();

                filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");

                //GetConfig
                ProcessStatus = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                foreach (var item in result)
                {
                    var resultItem = new SearchGRModel();

                    String Statue = "";
                    Statue = item.Document_Status.ToString();
                    var ProcessStatusName = ProcessStatus.Where(c => c.processStatus_Id == Statue).FirstOrDefault();


                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.goodsReceive_No = item.GoodsReceive_No;
                    resultItem.goodsReceive_Date = item.GoodsReceive_Date.toString();
                    resultItem.owner_Index = item.Owner_Index;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.qty = string.Format(String.Format("{0:N2}", item.Qty)); ;
                    resultItem.processStatus_Name = ProcessStatusName?.processStatus_Name;

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


        #region PrintGoodsReceive
        public string PrintGR(ReportGRModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? GRItemIndex = new Guid();

            try
            {
                var queryHead = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == data.goodsReceive_Index);
                var queryItem = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);
                var result = new List<ReportGRModel>();

                string date = queryHead.GoodsReceive_Date.toString();
                string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                foreach (var item in queryItem)
                {
                    var resultItem = new ReportGRModel();
                    resultItem.goodsReceive_Index = queryHead.GoodsReceive_Index;
                    resultItem.goodsReceive_No = queryHead.GoodsReceive_No;
                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.create_Date = queryHead.Create_Date.toString();
                    resultItem.documentType_Index = queryHead.DocumentType_Index;
                    resultItem.documentType_Id = queryHead.DocumentType_Id;
                    resultItem.documentType_Name = queryHead.DocumentType_Name;
                    resultItem.vendor_Id = queryHead.Vendor_Id;
                    resultItem.vendor_Index = queryHead.Vendor_Index;
                    resultItem.vendor_Name = queryHead.Vendor_Name;
                    resultItem.owner_Index = queryHead.Owner_Index;
                    resultItem.owner_Id = queryHead.Owner_Id;
                    resultItem.owner_Name = queryHead.Owner_Name;
                    resultItem.wHOwner_Index = queryHead.WHOwner_Index;
                    resultItem.wHOwner_Id = queryHead.WHOwner_Id;
                    resultItem.wHOwner_Name = queryHead.WHOwner_Name;
                    resultItem.invoice_No = queryHead.Invoice_No;
                    resultItem.document_Remark = queryHead.Document_Remark;
                    resultItem.goodsReceiveNo_Barcode = new NetBarcode.Barcode(queryHead.GoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();

                    //item
                    resultItem.qty = Convert.ToInt32(item.Qty);
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.weight = Convert.ToDecimal(string.Format("{0:0.00}", item.Weight));
                    resultItem.volume = item.Volume;
                    resultItem.price = item.Price;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;

                    result.Add(resultItem);
                }
                result.ToList();

                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportGR\\Report1.rdlc";
                var reportPath = rootPath + "\\Reports\\ReportGR\\Report1.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

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

        #region PrintGoodsReceivePutaway
        public string PrintGRPutaway(ReportGRPutawayModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? GRItemIndex = new Guid();

            try
            {
                var queryHead = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == data.goodsReceive_Index);
                var queryItem = db.IM_GoodsReceiveItemLocation.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);

                var result = new List<ReportGRPutawayModel>();

                string date = queryHead.GoodsReceive_Date.toString();
                string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                foreach (var item in queryItem)
                {
                    var queryItem2 = db.WM_Tag.FirstOrDefault(c => c.Tag_Index.ToString() == item.Tag_Index.ToString()).Pallet_Index;
                    var resultItem = new ReportGRPutawayModel();
                    resultItem.goodsReceive_Index = queryHead.GoodsReceive_Index;
                    resultItem.goodsReceive_No = queryHead.GoodsReceive_No;
                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.vendor_Id = queryHead.Vendor_Id;
                    resultItem.vendor_Index = queryHead.Vendor_Index;
                    resultItem.vendor_Name = queryHead.Vendor_Name;
                    resultItem.owner_Index = queryHead.Owner_Index;
                    resultItem.owner_Id = queryHead.Owner_Id;
                    resultItem.owner_Name = queryHead.Owner_Name;
                    resultItem.wHOwner_Index = queryHead.WHOwner_Index;
                    resultItem.wHOwner_Id = queryHead.WHOwner_Id;
                    resultItem.wHOwner_Name = queryHead.WHOwner_Name;
                    resultItem.invoice_No = queryHead.Invoice_No;
                    resultItem.goodsReceiveNo_Barcode = new NetBarcode.Barcode(queryHead.GoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();


                    //item
                    resultItem.pallet_No = queryItem2.ToString();

                    resultItem.qty = Convert.ToInt32(item.Qty);
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.productConversion_Index = item.ProductConversion_Index;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.product_Lot = item.Product_Lot;
                    resultItem.itemStatus_Index = item.ItemStatus_Index;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.location_Name = item.Location_Name;

                    result.Add(resultItem);
                }
                result.ToList();

                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportGRPutaway\\ReportGRPutaway.rdlc";
                var reportPath = rootPath + "\\Reports\\ReportGRPutaway\\ReportGRPutaway.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

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

        #region PrintReceipt
        public string PrintReceipt(ReportReceiptViewModel data, string rootPath = "")
        {
            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            Guid? GRItemIndex = new Guid();

            var log = db.im_Signatory_log.Where(c => c.Ref_Document_Index == new Guid(data.goodsReceive_Index)).ToList();

            if (log.Count <= 0)
            {

                if (data.isRecipent == true)
                {
                    im_Signatory_log RrecipenLogNew = new im_Signatory_log();

                    RrecipenLogNew.Signatory_Index = Guid.NewGuid();
                    RrecipenLogNew.SignatoryType_Name = "recipent";
                    RrecipenLogNew.User_Index = new Guid(data.recipent_user_Index);
                    RrecipenLogNew.User_Id = data.recipent_user_Id;
                    RrecipenLogNew.User_Name = data.recipent_user_Name;
                    RrecipenLogNew.First_Name = data.recipent_first_Name;
                    RrecipenLogNew.Last_Name = data.recipent_last_Name;
                    RrecipenLogNew.DocumentType_Index = data.documentType_Index.GetValueOrDefault();
                    RrecipenLogNew.DocumentType_Id = data.documentType_Id;
                    RrecipenLogNew.DocumentType_Name = data.documentType_Name;
                    RrecipenLogNew.Ref_Document_Index = new Guid(data.goodsReceive_Index);
                    RrecipenLogNew.Ref_Document_No = data.goodsReceive_No;
                    RrecipenLogNew.Position_Code = data.recipent_position_Code;
                    RrecipenLogNew.Position_Name = data.recipent_pos_Name;
                    RrecipenLogNew.IsActive = 1;
                    RrecipenLogNew.IsDelete = 0;
                    RrecipenLogNew.IsSystem = 0;
                    RrecipenLogNew.Status_Id = 0;
                    RrecipenLogNew.Create_By = data.user;
                    RrecipenLogNew.Create_Date = DateTime.Now;
                    db.im_Signatory_log.Add(RrecipenLogNew);
                }

                if (data.isRecorder == true)
                {
                    im_Signatory_log RecorderLogNew = new im_Signatory_log();

                    RecorderLogNew.Signatory_Index = Guid.NewGuid();
                    RecorderLogNew.SignatoryType_Name = "recorder";
                    RecorderLogNew.User_Index = new Guid(data.recorder_user_Index);
                    RecorderLogNew.User_Id = data.recorder_user_Id;
                    RecorderLogNew.User_Name = data.recorder_user_Name;
                    RecorderLogNew.First_Name = data.recorder_first_Name;
                    RecorderLogNew.Last_Name = data.recorder_last_Name;
                    RecorderLogNew.DocumentType_Index = data.documentType_Index.GetValueOrDefault();
                    RecorderLogNew.DocumentType_Id = data.documentType_Id;
                    RecorderLogNew.DocumentType_Name = data.documentType_Name;
                    RecorderLogNew.Ref_Document_Index = new Guid(data.goodsReceive_Index);
                    RecorderLogNew.Ref_Document_No = data.goodsReceive_No;
                    RecorderLogNew.Position_Code = data.recorder_position_Code;
                    RecorderLogNew.Position_Name = data.recorder_pos_Name;
                    RecorderLogNew.IsActive = 1;
                    RecorderLogNew.IsDelete = 0;
                    RecorderLogNew.IsSystem = 0;
                    RecorderLogNew.Status_Id = 0;
                    RecorderLogNew.Create_By = data.user;
                    RecorderLogNew.Create_Date = DateTime.Now;
                    db.im_Signatory_log.Add(RecorderLogNew);

                }

            }

            else
            {
                foreach (var item in log)
                {
                    var logOld = db.im_Signatory_log.Find(item.Signatory_Index);

                    logOld.Update_Date = DateTime.Now;
                    logOld.Update_By = data.user;

                    #region Recipent
                    var findRecipent = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "recipent").FirstOrDefault();

                    if (findRecipent == null)
                    {
                        if (data.isRecipent == true)
                        {
                            im_Signatory_log RrecipenLogNew = new im_Signatory_log();

                            RrecipenLogNew.Signatory_Index = Guid.NewGuid();
                            RrecipenLogNew.SignatoryType_Name = "recipent";
                            RrecipenLogNew.User_Index = new Guid(data.recipent_user_Index);
                            RrecipenLogNew.User_Id = data.recipent_user_Id;
                            RrecipenLogNew.User_Name = data.recipent_user_Name;
                            RrecipenLogNew.First_Name = data.recipent_first_Name;
                            RrecipenLogNew.Last_Name = data.recipent_last_Name;
                            RrecipenLogNew.DocumentType_Index = data.documentType_Index.GetValueOrDefault();
                            RrecipenLogNew.DocumentType_Id = data.documentType_Id;
                            RrecipenLogNew.DocumentType_Name = data.documentType_Name;
                            RrecipenLogNew.Ref_Document_Index = new Guid(data.goodsReceive_Index);
                            RrecipenLogNew.Ref_Document_No = data.goodsReceive_No;
                            RrecipenLogNew.Position_Code = data.recipent_position_Code;
                            RrecipenLogNew.Position_Name = data.recipent_pos_Name;
                            RrecipenLogNew.IsActive = 1;
                            RrecipenLogNew.IsDelete = 0;
                            RrecipenLogNew.IsSystem = 0;
                            RrecipenLogNew.Status_Id = 0;
                            RrecipenLogNew.Create_By = data.user;
                            RrecipenLogNew.Create_Date = DateTime.Now;
                            db.im_Signatory_log.Add(RrecipenLogNew);
                        }
                    }

                    else
                    {
                        if (data.isRecipent == true && logOld.SignatoryType_Name == "recipent")
                        {
                            logOld.User_Index = new Guid(data.recipent_user_Index);
                            logOld.User_Id = data.recipent_user_Id;
                            logOld.User_Name = data.recipent_user_Name;
                            logOld.First_Name = data.recipent_first_Name;
                            logOld.Last_Name = data.recipent_last_Name;
                            logOld.Position_Code = data.recipent_position_Code;
                            logOld.Position_Name = data.recipent_pos_Name;
                            logOld.IsActive = 1;
                            logOld.IsDelete = 0;
                        }
                        if (data.isRecipent == false && logOld.SignatoryType_Name == "recipent")
                        {
                            logOld.IsActive = 0;
                            logOld.IsDelete = 1;
                        }
                    }


                    #endregion



                    #region recorder

                    var findRecorder = db.im_Signatory_log.Where(c => c.Ref_Document_Index == item.Ref_Document_Index && c.SignatoryType_Name == "recorder").FirstOrDefault();

                    if (findRecorder == null)
                    {
                        #region InsertRecorder
                        if (data.isRecorder == true)
                        {
                            im_Signatory_log RecorderLogNew = new im_Signatory_log();

                            RecorderLogNew.Signatory_Index = Guid.NewGuid();
                            RecorderLogNew.SignatoryType_Name = "recorder";
                            RecorderLogNew.User_Index = new Guid(data.recorder_user_Index);
                            RecorderLogNew.User_Id = data.recorder_user_Id;
                            RecorderLogNew.User_Name = data.recorder_user_Name;
                            RecorderLogNew.First_Name = data.recorder_first_Name;
                            RecorderLogNew.Last_Name = data.recorder_last_Name;
                            RecorderLogNew.DocumentType_Index = data.documentType_Index.GetValueOrDefault();
                            RecorderLogNew.DocumentType_Id = data.documentType_Id;
                            RecorderLogNew.DocumentType_Name = data.documentType_Name;
                            RecorderLogNew.Ref_Document_Index = new Guid(data.goodsReceive_Index);
                            RecorderLogNew.Ref_Document_No = data.goodsReceive_No;
                            RecorderLogNew.Position_Code = data.recorder_position_Code;
                            RecorderLogNew.Position_Name = data.recorder_pos_Name;
                            RecorderLogNew.IsActive = 1;
                            RecorderLogNew.IsDelete = 0;
                            RecorderLogNew.IsSystem = 0;
                            RecorderLogNew.Status_Id = 0;
                            RecorderLogNew.Create_By = data.user;
                            RecorderLogNew.Create_Date = DateTime.Now;
                            db.im_Signatory_log.Add(RecorderLogNew);

                        }

                        #endregion

                    }
                    else
                    {

                        if (data.isRecorder == true && logOld.SignatoryType_Name == "recorder")
                        {
                            logOld.User_Index = new Guid(data.recorder_user_Index);
                            logOld.User_Id = data.recorder_user_Id;
                            logOld.User_Name = data.recorder_user_Name;
                            logOld.First_Name = data.recorder_first_Name;
                            logOld.Last_Name = data.recorder_last_Name;
                            logOld.Position_Code = data.recorder_position_Code;
                            logOld.Position_Name = data.recorder_pos_Name;
                            logOld.IsActive = 1;
                            logOld.IsDelete = 0;
                        }
                        if (data.isRecorder == false && logOld.SignatoryType_Name == "recorder")
                        {
                            logOld.IsActive = 0;
                            logOld.IsDelete = 1;
                        }
                    }


                    #endregion


                }
            }


            try
            {

                var result = new List<ReportReceiptViewModel>();
                //var query = (from GRI in db.IM_GoodsReceiveItem
                //             join GR in db.IM_GoodsReceive on GRI.GoodsReceive_Index equals GR.GoodsReceive_Index
                //             where GR.GoodsReceive_Index == data.goodsReceive_Index.sParse<Guid>()
                //             select new ReportReceiptViewModel
                //             {
                //                 goodsReceive_No = GR.GoodsReceive_No,
                //                 goodsReceive_Date = GR.GoodsReceive_Date.toString(),
                //                 goodsReceive_Due_Date = GR.GoodsReceive_Due_Date.toString(),
                //                 documentRef_No3 = GR.DocumentRef_No3,
                //                 planGoodsReceive_No = GRI.Ref_Document_No,
                //                 documentRef_No1 = GR.DocumentRef_No1,
                //                 warehouse_Code = GRI.UDF_1,
                //                 sloc_Code = GRI.UDF_2,
                //                 pgr_DocumentRef_No2 = "",
                //                 gr_DocumentRef_No2 = GR.DocumentRef_No2,
                //                 vendor_Id = "",
                //                 vendor_Name = "",
                //                 documentRef_No4 = GR.Document_Remark,

                //                 product_Id = GRI.Product_Id,
                //                 productConversion_Name = GRI.ProductConversion_Name,
                //                 product_Name = GRI.Product_Name,
                //                 qty = GRI.TotalQty,
                //                 documentRef_No2 = GRI.DocumentRef_No2,

                //                 isRecipent = data.isRecipent,
                //                 recipent_Name = data.recipent_Name,
                //                 recipent_pos_Name = data.recipent_pos_Name,
                //                 isRecorder = data.isRecorder,
                //                 recorder_Name = data.recorder_Name,
                //                 recorder_pos_Name = data.recorder_pos_Name,
                //             }
                //    ).ToList();

                var GR = db.IM_GoodsReceive.Find(Guid.Parse(data.goodsReceive_Index));
                if (GR == null)
                {
                    return "";
                }

                var GRI = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == GR.GoodsReceive_Index).ToList();

                var listPlan = new List<DocumentViewModel> { new DocumentViewModel { document_Index = GRI?.FirstOrDefault()?.Ref_Document_Index } };
                var plan = new DocumentViewModel();
                plan.listDocumentViewModel = listPlan;

                var resultPlanGRI = utils.SendDataApi<List<PlanGoodsReceiveItemViewModel>>(new AppSettingConfig().GetUrl("FindPlanGRItem"), plan.sJson());
                var resultPlanGR = utils.SendDataApi<List<PlanGoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("FindPlanGR"), plan.sJson());
                foreach (var g in GRI)
                {
                    var planGRI = resultPlanGRI.FirstOrDefault(f => f.planGoodsReceiveItem_Index == g.Ref_DocumentItem_Index);
                    string GoodsReceive_Date = "";
                    string GoodsReceive_Due_Date = "";
                    if (GR.GoodsReceive_Date != null && GR.GoodsReceive_Due_Date != null)
                    {
                        System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
                        DateTime dateThai = Convert.ToDateTime(GR.GoodsReceive_Date, _cultureTHInfo);
                        dateThai = dateThai.AddYears(543);
                        GoodsReceive_Date = dateThai.ToString("dd MMM yyyy", _cultureTHInfo);

                        DateTime dateThai2 = Convert.ToDateTime(GR.GoodsReceive_Due_Date, _cultureTHInfo);
                        dateThai2 = dateThai2.AddYears(543);
                        GoodsReceive_Due_Date = dateThai2.ToString("dd MMM yyyy", _cultureTHInfo);
                    }
                    //*** Thai Format


                    var r = new ReportReceiptViewModel
                    {
                        goodsReceive_No = GR.GoodsReceive_No,
                        goodsReceive_Date = GoodsReceive_Date,
                        goodsReceive_Due_Date = GoodsReceive_Due_Date,
                        documentRef_No3 = GR.DocumentRef_No3,
                        planGoodsReceive_No = g.Ref_Document_No,
                        documentRef_No1 = GR.DocumentRef_No1,
                        warehouse_Code = g.UDF_1,
                        sloc_Code = g.UDF_2,
                        pgr_DocumentRef_No2 = planGRI?.documentRef_No2,
                        gr_DocumentRef_No2 = GR.DocumentRef_No2,
                        vendor_Id = resultPlanGR?.FirstOrDefault().vendor_Id,
                        vendor_Name = resultPlanGR?.FirstOrDefault().vendor_Name,
                        documentRef_No4 = GR.Document_Remark,
                        documentType_Id = GR.DocumentType_Id,

                        product_Id = g.Product_Id,
                        productConversion_Name = g.ProductConversion_Name,
                        product_Name = g.Product_Name,
                        qty = g.TotalQty,
                        documentRef_No2 = g.DocumentRef_No2,

                        isRecipent = data.isRecipent,
                        recipent_Name = data.recipent_Name,
                        recipent_pos_Name = data.recipent_pos_Name,
                        isRecorder = data.isRecorder,
                        recorder_Name = data.recorder_Name,
                        recorder_pos_Name = data.recorder_pos_Name,
                    };
                    result.Add(r);
                }



                rootPath = rootPath.Replace("\\GRAPI", "");
                var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportReceipt\\ReportReceipt.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportReceipt\\ReportReceipt.rdlc";
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("Update_im_Signatory_log", msglog);
                    transactionx.Rollback();

                    throw exy;

                }
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public string SentToSap(ListGoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var resultMsg = "";
                var itemstatus = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), new { }.sJson());

                if (data.items.Count() == 0)
                {
                    return "กรุณาเลือกเอกสาร,";
                }

                foreach (var item in data.items)
                {
                    State = "Select GR";
                    var GR = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == item.goodsReceive_Index && !(c.Document_Status == 0 || c.Document_Status == 1));

                    if (GR == null)
                    {
                        resultMsg += item.goodsReceive_No + " เอกสารนี้ยังไม่เสร็จสิ้น,";
                        continue;
                    }
                    if (!string.IsNullOrEmpty(GR.DocumentRef_No5))
                    {
                        resultMsg += GR.GoodsReceive_No + " เอกสารนี้เคยส่งแล้ว,";
                        continue;
                    }

                    State = "Select GRI";
                    var GRI = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == GR.GoodsReceive_Index).ToList();


                    var Request = new GRRequestViewModel();
                    Request.PstngDate = DateTime.Now.toString().Substring(0, 8);
                    Request.DocDate = GR.GoodsReceive_Date.toString().Substring(0, 8);
                    Request.RefDocNo = GR.DocumentRef_No1;
                    Request.GrNo = GR.GoodsReceive_No;
                    Request.HeaderTxt = !string.IsNullOrEmpty(GR.DocumentRef_No2) ? GR.DocumentRef_No2.Trim() : "";
                    Request.GmCode = "01";

                    foreach (var i in GRI)
                    {
                        var PGR = db.IM_PlanGoodsReceiveItem.FirstOrDefault(c => c.PlanGoodsReceiveItem_Index == i.Ref_DocumentItem_Index);

                        var RequestDetail = new GRRequestDetail();
                        RequestDetail.Material = i.Product_Id;
                        RequestDetail.Plant = PGR.DocumentRef_No1;
                        RequestDetail.StgeLoc = PGR.DocumentRef_No1 == "9900" ? "9930" : "3330";
                        RequestDetail.Batch = !string.IsNullOrEmpty(i.Product_Lot) ? i.Product_Lot.Trim() : "";
                        RequestDetail.MoveType = "101";
                        RequestDetail.StckType = itemstatus.FirstOrDefault(c => c.itemStatus_Id == i.ItemStatus_Id).stck_Type;
                        RequestDetail.Vendor = !string.IsNullOrEmpty(i.DocumentRef_No1) ? i.DocumentRef_No1.Trim() : "";
                        RequestDetail.EntryQnt = Math.Round(i.Qty.sParse<decimal>(), 3);
                        RequestDetail.EntryUom = i.ProductConversion_Name;
                        RequestDetail.PoNumber = i.Ref_Document_No;
                        RequestDetail.PoItem = PGR.LineNum;
                        RequestDetail.NoMoreGR = !string.IsNullOrEmpty(i.UDF_1) ? i.DocumentRef_No1.Trim() : "";
                        RequestDetail.ItemText = !string.IsNullOrEmpty(i.GoodsReceive_Remark) ? i.GoodsReceive_Remark.Trim() : "";
                        Request.Detail.Add(RequestDetail);
                    }

                    State = "Sent To Sap";
                    //var result = utils.SendDataApi<GRResponseViewModel>(new AppSettingConfig().GetUrl("SentToSap"), Request.sJson());
                    var result = new GRResponseViewModel { status = "SUCCESS", message = new GRMessage { eMaterailDocField = "01", eFiDocumentField = "02" } };

                    if (result.status == "SUCCESS")
                    {
                        State = "response SUCCESS";
                        GR.DocumentRef_No5 = result.message.eMaterailDocField;
                        GR.DocumentRef_No3 = result.message.eFiDocumentField;
                        GR.Update_By = data.userName;
                        GR.Update_Date = DateTime.Now;
                        resultMsg += GR.GoodsReceive_No + " สำเร็จ ,";
                    }
                    else if (result.status == "ERROR")
                    {
                        State = "response ERROR";
                        resultMsg += GR.GoodsReceive_No + " ไม่สำเร็จ " + result.message.eMessageField + ",";
                    }
                    else
                    {
                        State = "response";
                        resultMsg += GR.GoodsReceive_No + " ไม่สำเร็จ,";
                    }

                }

                var transactionx = db.Database.BeginTransaction();
                try
                {
                    State = "SaveChanges";

                    db.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " exy Rollback " + exy.Message.ToString();
                    olog.logging("SentToSapGr", msglog);
                    transactionx.Rollback();

                    throw exy;

                }

                return resultMsg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SentToSapGetJson(string data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                var resultMsg = "";
                var itemstatus = utils.SendDataApi<List<ItemStatusDocViewModel>>(new AppSettingConfig().GetUrl("dropdownItemStatus"), new { }.sJson());

                State = "Select GR";
                var GR = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_Index == Guid.Parse(data) && !(c.Document_Status == 0 || c.Document_Status == 1));

                if (GR == null)
                {
                    resultMsg += " Order Not Complete,";
                }
                if (!string.IsNullOrEmpty(GR.DocumentRef_No5))
                {
                    resultMsg += " Data sent,";
                }

                State = "Select GRI";
                var GRI = db.IM_GoodsReceiveItem.Where(c => c.GoodsReceive_Index == GR.GoodsReceive_Index).ToList();


                var Request = new GRRequestViewModel();
                Request.PstngDate = DateTime.Now.toString().Substring(0, 8);
                Request.DocDate = GR.GoodsReceive_Date.toString().Substring(0, 8);
                Request.RefDocNo = GR.DocumentRef_No1;
                Request.GrNo = GR.GoodsReceive_No;
                Request.HeaderTxt = !string.IsNullOrEmpty(GR.DocumentRef_No2) ? GR.DocumentRef_No2.Trim() : "";
                Request.GmCode = "01";

                foreach (var i in GRI)
                {
                    var PGR = db.IM_PlanGoodsReceiveItem.FirstOrDefault(c => c.PlanGoodsReceiveItem_Index == i.Ref_DocumentItem_Index);

                    var RequestDetail = new GRRequestDetail();
                    RequestDetail.Material = i.Product_Id;
                    RequestDetail.Plant = PGR.DocumentRef_No1;
                    RequestDetail.StgeLoc = PGR.DocumentRef_No1 == "9900" ? "9930" : "3330";
                    RequestDetail.Batch = !string.IsNullOrEmpty(i.Product_Lot) ? i.Product_Lot.Trim() : "";
                    RequestDetail.MoveType = "101";
                    RequestDetail.StckType = itemstatus.FirstOrDefault(c => c.itemStatus_Id == i.ItemStatus_Id).stck_Type;
                    RequestDetail.Vendor = !string.IsNullOrEmpty(i.DocumentRef_No1) ? i.DocumentRef_No1.Trim() : "";
                    RequestDetail.EntryQnt = Math.Round(i.Qty.sParse<decimal>(), 3);
                    RequestDetail.EntryUom = i.ProductConversion_Name;
                    RequestDetail.PoNumber = i.Ref_Document_No;
                    RequestDetail.PoItem = PGR.LineNum;
                    RequestDetail.NoMoreGR = !string.IsNullOrEmpty(i.UDF_1) ? i.DocumentRef_No1.Trim() : "";
                    RequestDetail.ItemText = !string.IsNullOrEmpty(i.GoodsReceive_Remark) ? i.GoodsReceive_Remark.Trim() : "";
                    Request.Detail.Add(RequestDetail);
                }

                State = "Sent To Sap";
                resultMsg = Request.sJson();


                return resultMsg;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region genDocumentNo
        public String genDocumentNo(GoodsReceiveDocViewModel data)
        {
            try
            {
                var result = new List<DocumentTypeViewModel>();

                var filterModel = new DocumentTypeViewModel();

                filterModel.process_Index = new Guid("5F147725-520C-4CA6-B1D2-2C0E65E7AAAA");
                filterModel.documentType_Index = data.documentType_Index;
                //GetConfig
                result = utils.SendDataApi<List<DocumentTypeViewModel>>(new AppSettingConfig().GetUrl("DropDownDocumentType"), filterModel.sJson());
                var genDoc = new AutoNumberService();
                string DocNo = "";
                DateTime DocumentDate = (DateTime)data.goodsReceive_Date.toDate();
                DocNo = genDoc.genAutoDocmentNumber(result, DocumentDate);

                return DocNo;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        public List<im_Signatory_logViewModel> findUser(im_Signatory_logViewModel data)
        {
            try
            {

                var items = new List<im_Signatory_logViewModel>();



                var query = db.im_Signatory_log.AsQueryable();



                if (!string.IsNullOrEmpty(data.goodsReceive_No))
                {
                    query = query.Where(c => c.Ref_Document_No == data.goodsReceive_No);
                }


                var result = query.Take(100).OrderByDescending(o => o.Create_Date).ToList();

                foreach (var item in result)
                {
                    var resultItem = new im_Signatory_logViewModel();

                    resultItem.signatory_Index = item.Signatory_Index;
                    resultItem.signatoryType_Id = item.SignatoryType_Id;
                    resultItem.signatoryType_Name = item.SignatoryType_Name;
                    resultItem.first_Name = item.First_Name;
                    resultItem.last_Name = item.Last_Name;
                    resultItem.position_Name = item.Position_Name;

                    items.Add(resultItem);
                }


                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region dropdownCostCenter
        public List<CostCenterViewModel> dropdownCostCenter(CostCenterViewModel data)
        {
            try
            {
                var result = new List<CostCenterViewModel>();

                var filterModel = new CostCenterViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CostCenterViewModel>>(new AppSettingConfig().GetUrl("dropdownCostCenter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownWeight
        public List<WeightViewModel> dropdownWeight(WeightViewModel data)
        {
            try
            {
                var result = new List<WeightViewModel>();

                var filterModel = new WeightViewModel();

                //GetConfig
                result = utils.SendDataApi<List<WeightViewModel>>(new AppSettingConfig().GetUrl("dropdownWeight"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownCurrency
        public List<CurrencyViewModel> dropdownCurrency(CurrencyViewModel data)
        {
            try
            {
                var result = new List<CurrencyViewModel>();

                var filterModel = new CurrencyViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CurrencyViewModel>>(new AppSettingConfig().GetUrl("dropdownCurrency"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownVolume
        public List<VolumeViewModel> dropdownVolume(VolumeViewModel data)
        {
            try
            {
                var result = new List<VolumeViewModel>();

                var filterModel = new VolumeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VolumeViewModel>>(new AppSettingConfig().GetUrl("dropdownVolume"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownDocumentPriority
        public List<DocumentPriorityViewModel> dropdownDocumentPriority(DocumentPriorityViewModel data)
        {
            try
            {
                var result = new List<DocumentPriorityViewModel>();

                var filterModel = new DocumentPriorityViewModel();

                //GetConfig
                result = utils.SendDataApi<List<DocumentPriorityViewModel>>(new AppSettingConfig().GetUrl("dropdownDocumentPriority"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DropdownWarehouse
        public List<WarehouseViewModel> dropdownWarehouse(WarehouseViewModel data)
        {
            try
            {
                var result = new List<WarehouseViewModel>();

                var filterModel = new WarehouseViewModel();

                //GetConfig
                result = utils.SendDataApi<List<WarehouseViewModel>>(new AppSettingConfig().GetUrl("dropdownWarehouse"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownCargoType
        public List<CargoTypeViewModel> dropdownCargoType(CargoTypeViewModel data)
        {
            try
            {
                var result = new List<CargoTypeViewModel>();

                var filterModel = new CargoTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<CargoTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownCargoType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownVehicleType
        public List<VehicleTypeViewModel> dropdownVehicleType(VehicleTypeViewModel data)
        {
            try
            {
                var result = new List<VehicleTypeViewModel>();

                var filterModel = new VehicleTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<VehicleTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownVehicleType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownUnloadingType
        public List<UnloadingTypeViewModel> dropdownUnloadingType(UnloadingTypeViewModel data)
        {
            try
            {
                var result = new List<UnloadingTypeViewModel>();

                var filterModel = new UnloadingTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<UnloadingTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownUnloadingType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dropdownShipmentType
        public List<ShipmentTypeViewModel> dropdownShipmentType(ShipmentTypeViewModel data)
        {
            try
            {
                var result = new List<ShipmentTypeViewModel>();

                var filterModel = new ShipmentTypeViewModel();

                //GetConfig
                result = utils.SendDataApi<List<ShipmentTypeViewModel>>(new AppSettingConfig().GetUrl("dropdownShipmentType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownContainerType
        public List<ContainerTypeViewModelV2> dropdownContainerType(ContainerTypeViewModelV2 data)
        {
            try
            {
                var result = new List<ContainerTypeViewModelV2>();

                var filterModel = new ContainerTypeViewModelV2();

                //GetConfig
                result = utils.SendDataApi<List<ContainerTypeViewModelV2>>(new AppSettingConfig().GetUrl("dropdownContainerType"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region dropdownDockDoor
        public List<DockDoorViewModelV2> dropdownDockDoor(DockDoorViewModelV2 data)
        {
            try
            {
                var result = new List<DockDoorViewModelV2>();

                var filterModel = new DockDoorViewModelV2();

                //GetConfig
                result = utils.SendDataApi<List<DockDoorViewModelV2>>(new AppSettingConfig().GetUrl("dropdownDockDoor"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SearchTagPutaway
        public List<SearchTagPutawayViewModel> SearchTagPutaway(SearchTagPutawayViewModel data)
        {
            try
            {
                var result = new List<SearchTagPutawayViewModel>();



                var filterModel = new ProcessStatusViewModel();
                filterModel.process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");

                var Process = utils.SendDataApi<List<ProcessStatusViewModel>>(new AppSettingConfig().GetUrl("ProcessStatus"), filterModel.sJson());


                var query = db.View_TagPutaway.AsQueryable();


                if (!string.IsNullOrEmpty(data.goodsReceive_Index.ToString()))
                {
                    query = query.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);
                }

                if (!string.IsNullOrEmpty(data.product_Id))
                {
                    query = query.Where(c => c.Product_Id.Contains(data.product_Id));
                }

                if (!string.IsNullOrEmpty(data.product_Name))
                {
                    query = query.Where(c => c.Product_Name.Contains(data.product_Name));
                }

                if (data.putaway_Status != null)
                {
                    query = query.Where(c => c.Putaway_Status == data.putaway_Status);
                }

                var resultQuery = query.ToList();

                foreach (var item in resultQuery)
                {
                    var resultItem = new SearchTagPutawayViewModel();

                    resultItem.goodsReceive_Index = item.GoodsReceive_Index;
                    resultItem.tag_Index = item.Tag_Index;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.LocationType_Name = item.LocationType_Name;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;

                    var GIL = db.IM_GoodsReceiveItemLocation.Where(c => c.Product_Index == item.Product_Index && c.GoodsReceive_Index == item.GoodsReceive_Index && c.Tag_Index == item.Tag_Index).FirstOrDefault();

                    if (GIL.Putaway_Date == null)
                    {
                        resultItem.putaway_Date = "-";
                    }
                    else
                    {
                        resultItem.putaway_Date = GIL.Putaway_Date.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                    }

                    var get_tracklocation = db.wm_TagItem.FirstOrDefault(c => c.Tag_Index == item.Tag_Index);
                    if (get_tracklocation != null)
                    {
                        resultItem.WaitTostore_date = get_tracklocation.Create_Date.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                        resultItem.WaitTostore_location = "Dock Inbound";
                        resultItem.Docktostaging_date = get_tracklocation.UpdateDockToStaging_Date.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                        resultItem.Docktostaging_location = get_tracklocation.Location_Name;
                        resultItem.Pallet_Inspection_date = get_tracklocation.UpdatePallet_Inspection_Date.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss");
                        resultItem.Pallet_Inspection_location = get_tracklocation.IsPallet_Inspection_Location_Name;
                    }

                    var locationnow = utils.SendDataApi<BinBalanceViewModel>(new AppSettingConfig().GetUrl("findBinbalance"), resultItem.sJson());


                    var Putaway_Status = item.Putaway_Status.ToString();
                    resultItem.statusPutaway = Process.Where(a => a.processStatus_Id == Putaway_Status).Select(c => c.processStatus_Name).FirstOrDefault();
                    resultItem.putaway_By = item.putaway_By;
                    resultItem.putaway_location = locationnow.Location_Id;

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

        #region ReportPrintOutGR
        public string ReportPrintOutGR(ReportPrintOutGRViewModel data, string rootPath = "")
        {

            var GR_DB = new GRDbContext();

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            try
            {
                var queryGR = GR_DB.View_RPT_PrintOutGR_V2.AsQueryable();
                queryGR = queryGR.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);

                var ListGR = queryGR.ToList();

                long DateTicks = DateTime.Now.Ticks;
                int day = new DateTime(DateTicks).Day;
                int month = new DateTime(DateTicks).Month;
                int year = new DateTime(DateTicks).Year;
                var time = DateTime.Now.ToString("HH:mm");


                var thaiMonth = "";
                switch (month)
                {
                    case 1:
                        thaiMonth = "มกราคม";
                        break;
                    case 2:
                        thaiMonth = "กุมภาพันธ์";
                        break;
                    case 3:
                        thaiMonth = "มีนาคม";
                        break;
                    case 4:
                        thaiMonth = "เมษายน";
                        break;
                    case 5:
                        thaiMonth = "พฤษภาคม";
                        break;
                    case 6:
                        thaiMonth = "มิถุนายน";
                        break;
                    case 7:
                        thaiMonth = "กรกฎาคม";
                        break;
                    case 8:
                        thaiMonth = "สิงหาคม";
                        break;
                    case 9:
                        thaiMonth = "กันยายน";
                        break;
                    case 10:
                        thaiMonth = "ตุลาคม";
                        break;
                    case 11:
                        thaiMonth = "พฤศจิกายน";
                        break;
                    case 12:
                        thaiMonth = "ธันวาคม";
                        break;
                }

                var refNo2 = "";
                var refNo3 = "";

                var OwnerModel = new OwnerViewModelV3();
                var resultOwner = new List<OwnerViewModelV3>();
                resultOwner = utils.SendDataApi<List<OwnerViewModelV3>>(new AppSettingConfig().GetUrl("dropdownOwner"), OwnerModel.sJson());
                if (resultOwner.Count > 0 && resultOwner != null)
                {
                    var DataOwner = resultOwner.Find(c => c.owner_Index == data.owner_Index);
                    refNo2 = DataOwner.owner_TaxID;
                    refNo3 = DataOwner.ref_No3;
                }


                var result = new List<ReportPrintOutGRViewModel>();
                var rowData = 1;

                if (data.documentType_Index != new Guid("A256B73D-2354-4187-B19B-D6301475E0EA"))
                {
                    var query = (from gr in ListGR
                                 join pgr in GR_DB.IM_PlanGoodsReceive on gr.Ref_Document_Index equals pgr.PlanGoodsReceive_Index into ps
                                 from r in ps.DefaultIfEmpty()
                                 orderby gr.GoodsReceive_Date ascending
                                 select new
                                 {
                                     PlanGR = r,
                                     GR = gr
                                 }).ToList();

                    if (query.Count == 0)
                    {
                        var resultItem = new ReportPrintOutGRViewModel();

                        resultItem.checkQuery = true;

                        result.Add(resultItem);
                    }
                    else
                    {
                        foreach (var item in query.OrderBy(oo => oo.GR.Product_Id))
                        {
                            var resultItem = new ReportPrintOutGRViewModel();
                            string date = item.GR.GoodsReceive_Date.toString();
                            string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                            System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                            var ProductViewModel = new ProductViewModel();
                            ProductViewModel.product_Index = item.GR.Product_Index;
                            var GetProduct = utils.SendDataApi<List<ProductModel>>(new AppSettingConfig().GetUrl("getProduct"), ProductViewModel.sJson());


                            resultItem.goodsReceive_Date = GRDate;
                            resultItem.rowCount = rowData;
                            resultItem.product_Id = item.GR.Product_Id;
                            resultItem.product_Name = item.GR.Product_Name;
                            resultItem.qty = item.GR.Qty;
                            resultItem.productConversion_Name = item.GR.ProductConversion_Name;
                            resultItem.goodsReceive_No = item.GR.GoodsReceive_No;
                            resultItem.owner_Id = item.GR.Owner_Id;
                            resultItem.owner_Name = item.GR.Owner_Name;
                            resultItem.warehouse_Id = item.GR.Warehouse_Id;
                            resultItem.documentType_Name = item.GR.DocumentType_Name;
                            resultItem.goodsReceiveNo_Barcode = new NetBarcode.Barcode(item.GR.GoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();
                            resultItem.ref_No2 = refNo2;
                            resultItem.lineNum = item.GR.LineNum;
                            resultItem.date_Now =  day.ToString() + "/" + month + "/" + year.ToString() + " เวลา :" + time;
                            resultItem.total =  item.GR.TotalQty;
                            resultItem.batch =  item.GR.product_lot;
                            resultItem.status_item =  item.GR.ItemStatus_Name;
                            resultItem.ref_po =  item.GR.Ref_Document_No;
                            resultItem.vehicle_no = item.GR.License_Name;
                            resultItem.dock_name = item.GR.Dock_Name;
                            resultItem.weight = item.GR.weight.ToString();
                            if (item.GR.Appointment_id !=null)
                            {
                                resultItem.Appointment_BarCode = new NetBarcode.Barcode(item.GR.Appointment_id, NetBarcode.Type.Code128B).GetBase64Image();
                                resultItem.Appointment_id = item.GR.Appointment_id;
                            }

                            resultItem.conversion = GetProduct[0].productConversion_Name;
                            if (item.PlanGR == null)
                            {
                                resultItem.documentRef_No2 = null;
                                resultItem.document_Remark = null;
                                resultItem.planGoodsReceive_No = null;
                                resultItem.planGoodsReceive_Date = null;

                            }
                            else
                            {
                                resultItem.documentRef_No2 = refNo3; // item.PlanGR.DocumentRef_No2;
                                resultItem.document_Remark = item.PlanGR.Document_Remark;
                                resultItem.planGoodsReceive_No = item.PlanGR.PlanGoodsReceive_No;
                                string planDate = item.PlanGR.PlanGoodsReceive_Date.toString();
                                string PlanGRDate = DateTime.ParseExact(planDate.Substring(0, 8), "yyyyMMdd",
                                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                                resultItem.planGoodsReceive_Date = PlanGRDate;
                            }

                            resultItem.msgDN = "เลข Delivery Note :";
                            resultItem.msgDate = "วันที่แจ้งฝากสินค้า :";
                            resultItem.erp_location = item.GR.Erp_location;

                            rowData = rowData + 1;
                            result.Add(resultItem);
                        }
                    }
                }
                else
                {
                    var ListDocumentViewModel = new List<DocumentViewModel>();
                    foreach (var item in ListGR)
                    {
                        var DocumentViewModel = new DocumentViewModel();
                        DocumentViewModel.document_Index = item.Ref_Document_Index;
                        ListDocumentViewModel.Add(DocumentViewModel);
                    }
                    var Bom = utils.SendDataApi<List<BomViewModel>>(new AppSettingConfig().GetUrl("FilterBom"), new DocumentViewModel { listDocumentViewModel = ListDocumentViewModel }.sJson());

                    var query = (from gr in ListGR
                                 join pgr in Bom on gr.Ref_Document_Index equals pgr.BOM_Index into ps
                                 from r in ps.DefaultIfEmpty()
                                 orderby gr.GoodsReceive_Date ascending
                                 select new
                                 {
                                     PlanGR = r,
                                     GR = gr
                                 }).ToList();

                    if (query.Count == 0)
                    {
                        var resultItem = new ReportPrintOutGRViewModel();

                        resultItem.checkQuery = true;

                        result.Add(resultItem);
                    }
                    else
                    {
                        foreach (var item in query.OrderBy(oo => oo.GR.Product_Id))
                        {
                            var resultItem = new ReportPrintOutGRViewModel();
                            string date = item.GR.GoodsReceive_Date.toString();
                            string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                            System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                            resultItem.goodsReceive_Date = GRDate;
                            resultItem.rowCount = rowData;
                            resultItem.product_Id = item.GR.Product_Id;
                            resultItem.product_Name = item.GR.Product_Name;
                            resultItem.qty = item.GR.Qty;
                            resultItem.productConversion_Name = item.GR.ProductConversion_Name;
                            resultItem.goodsReceive_No = item.GR.GoodsReceive_No;
                            resultItem.owner_Id = item.GR.Owner_Id;
                            resultItem.owner_Name = item.GR.Owner_Name;
                            resultItem.warehouse_Id = item.GR.Warehouse_Id;
                            resultItem.documentType_Name = item.GR.DocumentType_Name;
                            resultItem.goodsReceiveNo_Barcode = new NetBarcode.Barcode(item.GR.GoodsReceive_No, NetBarcode.Type.Code128B).GetBase64Image();
                            resultItem.ref_No2 = refNo2;
                            resultItem.lineNum = item.GR.LineNum;
                            resultItem.date_Now = day.ToString() + "/" + month + "/" + year.ToString() + " เวลา :" + time;
                            resultItem.total = item.GR.TotalQty;
                            resultItem.batch = item.GR.product_lot;
                            resultItem.status_item = item.GR.ItemStatus_Name;
                            resultItem.ref_po = item.GR.Ref_Document_No;
                            resultItem.vehicle_no = item.GR.License_Name;
                            resultItem.dock_name = item.GR.Dock_Name;
                            resultItem.weight = item.GR.weight.ToString();
                            if (item.GR.Appointment_id != null)
                            {
                                resultItem.Appointment_BarCode = new NetBarcode.Barcode(item.GR.Appointment_id, NetBarcode.Type.Code128B).GetBase64Image();
                                resultItem.Appointment_id = item.GR.Appointment_id;
                            }
                            if (item.PlanGR == null)
                            {
                                resultItem.documentRef_No2 = null;
                                resultItem.document_Remark = null;
                                resultItem.planGoodsReceive_No = null;
                                resultItem.planGoodsReceive_Date = null;

                            }
                            else
                            {
                                resultItem.documentRef_No2 = item.PlanGR.DocumentRef_No2;
                                resultItem.document_Remark = item.PlanGR.Document_Remark;
                                resultItem.planGoodsReceive_No = item.PlanGR.BOM_No;
                                string planDate = item.PlanGR.BOM_Date;
                                string PlanGRDate = DateTime.ParseExact(planDate.Substring(0, 8), "yyyyMMdd",
                                System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);
                                resultItem.planGoodsReceive_Date = PlanGRDate;
                            }

                            resultItem.msgDN = "เลขที่ BOM :";
                            resultItem.msgDate = "วันที่ประกอบ :";
                            resultItem.erp_location = item.GR.Erp_location;

                            rowData = rowData + 1;
                            result.Add(resultItem);
                        }
                    }

                }
                result.ToList();

                rootPath = rootPath.Replace("\\GRAPI", "");
                //var reportPath = rootPath + "\\GRBusiness\\Reports\\ReportPrintOutGR\\ReportPrintOutGR.rdlc";
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPrintOutGR");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

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



        public actionResultBomViewModels popupBomIfilter(BomViewModel data)
        {
            try
            {
                var result = new List<BomViewModel>();

                var filterModel = new BomViewModel();

                if (!string.IsNullOrEmpty(data.BOM_No))
                {
                    filterModel.BOM_No = data.BOM_No;
                }
                if (!string.IsNullOrEmpty(data.Owner_Index.ToString()))
                {
                    filterModel.Index = data.Owner_Index;
                }


                //GetConfig
                result = utils.SendDataApi<List<BomViewModel>>(new AppSettingConfig().GetUrl("PopupBom"), filterModel.sJson()).ToList();


                var items = data.PerPage == 0 ? result.ToList() : result.OrderByDescending(o => o.BOM_Date).Skip((data.CurrentPage - 1) * data.PerPage).Take(data.PerPage).ToList();

                var listBom_Index = db.IM_GoodsReceiveItem.Where(c => (c.UDF_1 == "X") && c.Document_Status != -1).GroupBy(g => g.Ref_Document_Index).ToList();

                if (listBom_Index.Count > 0)
                {
                    items.RemoveAll(c => listBom_Index.Select(s => s.Key).Contains(c.BOM_Index));
                }


                var count = result.Count;
                var actionResultPlanGRPopup = new actionResultBomViewModels();
                actionResultPlanGRPopup.itemsBom = items;
                actionResultPlanGRPopup.pagination = new Pagination() { TotalRow = count, CurrentPage = data.CurrentPage, PerPage = data.PerPage };

                return actionResultPlanGRPopup;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public actionResult updateStatusBom(GoodsReceiveDocViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            var result = new actionResult();
            try
            {

                foreach (var item in data.listGoodsReceiveItemViewModels)
                {

                    var resultBom = new List<BomItemPopupViewModel>();


                    var listBom = new List<DocumentViewModel> { new DocumentViewModel { document_Index = item.ref_Document_Index } };
                    var bom = new DocumentViewModel();
                    bom.listDocumentViewModel = listBom;

                    resultBom = utils.SendDataApi<List<BomItemPopupViewModel>>(new AppSettingConfig().GetUrl("FindBom"), bom.sJson());


                    var checkGR = db.IM_GoodsReceiveItem.Where(c => c.Ref_Document_Index == item.ref_Document_Index && c.Document_Status != -1)
                   .GroupBy(c => new { c.Ref_Document_Index })
                   .Select(c => new { c.Key.Ref_Document_Index, SumQty = c.Sum(s => s.TotalQty) }).ToList();






                    if (checkGR.FirstOrDefault().SumQty == resultBom.FirstOrDefault().totalQty)
                    {
                        var BomOld = db3.im_BOM.Find(item.ref_Document_Index);

                        if (BomOld != null)
                        {

                            BomOld.Document_Status = 4;
                        }
                    }
                }


                var transactionx = db3.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db3.SaveChanges();
                    transactionx.Commit();
                    result.Message = true;
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("updateBom", msglog);
                    transactionx.Rollback();
                    throw exy;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ItemListViewModel> autoLocationFilter(ItemListViewModel data)
        {
            try
            {
                var result = new List<ItemListViewModel>();


                var filterModel = new ItemListViewModel();

                if (!string.IsNullOrEmpty(data.key))
                {
                    filterModel.key = data.key;
                }

                //GetConfig
                result = utils.SendDataApi<List<ItemListViewModel>>(new AppSettingConfig().GetUrl("autoLocationFilter"), filterModel.sJson());

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region TaskGR_update
        public string Task_GR_Update(TaskfilterViewModel data) {
            try
            {
                var result = "";
                Guid? TaskGRIndex = new Guid();
                TaskfilterViewModel model = new TaskfilterViewModel();
                AssignService assignService = new AssignService();
                TaskfilterViewModel resultTask = assignService.CheckTagTask(data);
                TaskGRIndex = resultTask.taskGR_Index;

                var TaskGRItemOld = db.im_TaskGRItem.Find(resultTask.taskGRItem_Index);

                TaskGRItemOld.Document_Status = 1;
                TaskGRItemOld.Update_By = data.create_By;
                TaskGRItemOld.Update_Date = DateTime.Now;

                var tagitem = db.wm_TagItem.Find(TaskGRItemOld.TagItem_Index);
                tagitem.IsPallet_Inspection = 1;
                tagitem.UpdatePallet_Inspection_By = data.create_By;
                tagitem.UpdatePallet_Inspection_Date = DateTime.Now;

                var transactionx = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionx.Commit();
                    
                }

                catch (Exception exy)
                {
                    transactionx.Rollback();
                    throw exy;
                }

                var resultCheckTask = assignService.CheckTaskSuccess(data);

                if (resultCheckTask.Count <= 0)
                {
                    var TaskGROld = db.im_TaskGR.Find(TaskGRIndex);

                    TaskGROld.Document_Status = 2;
                    TaskGROld.Update_By = data.create_By;
                    TaskGROld.Update_Date = DateTime.Now;
                    result = "Task Success";

                }
                else {
                    result = "Save Success";
                }

                var transactionxF = db.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db.SaveChanges();
                    transactionxF.Commit();
                }

                catch (Exception exy)
                {
                    transactionxF.Rollback();
                    throw exy;
                }

                return "";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        public string GetAllGR(SearchGRModel models)
        {
            try
            {
                var gr_index = db.IM_GoodsReceive.FirstOrDefault(c => c.GoodsReceive_No == models.goodsReceive_No);
                GoodsReceiveDocViewModel gr_index_confirm = new GoodsReceiveDocViewModel();
                gr_index_confirm.goodsReceive_Index = gr_index.GoodsReceive_Index;
                var gr_confirm = confirmStatus(gr_index_confirm);
                string inedex = gr_index.GoodsReceive_Index.ToString();
                return inedex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
