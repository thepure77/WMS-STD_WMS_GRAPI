using GRBusiness.GoodsReceiveImage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public class GoodsReceiveDocViewModel
    {
        public Guid? goodsReceive_Index { get; set; }

        public Guid? planGoodsIssue_Index { get; set; }

        public Guid? owner_Index { get; set; }


        public string owner_Id { get; set; }


        public string owner_Name { get; set; }

        public Guid? documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }


        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

        public string goodsReceive_Due_Date { get; set; }


        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string document_Remark { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }

        public int? documentPriority_Status { get; set; }


        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public int? putaway_Status { get; set; }

        public Guid? warehouse_Index { get; set; }


        public string warehouse_Id { get; set; }


        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public Guid? dockDoor_Index { get; set; }


        public string dockDoor_Id { get; set; }


        public string dockDoor_Name { get; set; }

        public Guid? vehicleType_Index { get; set; }


        public string vehicleType_Id { get; set; }


        public string vehicleType_Name { get; set; }

        public Guid? containerType_Index { get; set; }


        public string containerType_Id { get; set; }


        public string containerType_Name { get; set; }


        public string userAssign { get; set; }


        public string invoice_No { get; set; }

        public Guid? vendor_Index { get; set; }


        public string vendor_Id { get; set; }


        public string vendor_Name { get; set; }

        public Guid? wHOwner_Index { get; set; }


        public string wHOwner_Id { get; set; }


        public string wHOwner_Name { get; set; }


        public string license_Name { get; set; }

        public Guid? forwarder_Index { get; set; }


        public string forwarder_Id { get; set; }


        public string forwarder_Name { get; set; }

        public Guid? shipmentType_Index { get; set; }


        public string shipmentType_Id { get; set; }


        public string shipmentType_Name { get; set; }

        public Guid? cargoType_Index { get; set; }


        public string cargoType_Id { get; set; }


        public string cargoType_Name { get; set; }

        public Guid? unloadingType_Index { get; set; }


        public string unloadingType_Id { get; set; }


        public string unloadingType_Name { get; set; }


        public string containerType_Name1 { get; set; }


        public string container_No1 { get; set; }


        public string container_No2 { get; set; }


        public string labur { get; set; }

        public bool selected { get; set; }

        public List<string> index { get; set; }
        public string checker_Name { get; set; }
        public string driver_Name { get; set; }

        public Guid? costCenter_Index { get; set; }

        public string costCenter_Id { get; set; }

        public string costCenter_Name { get; set; }

        public Guid? goodsIssue_Index { get; set; }

        public string goodsIssue_No { get; set; }



        public List<GoodsReceiveItemViewModel> listGoodsReceiveItemViewModels { get; set; }
        public List<result_goodissue> result_goodissue { get; set; }
        public string userAssignKey { get; set; }
        public Guid? planGoodsReceive_Index { get; set; }
        public string planGoodsReceive_Id { get; set; }
        public string planGoodsReceive_Name { get; set; }
        public List<GoodsReceiveImageViewModel> docfile { get; set; }

        public class actionResult
        {
            public string goodsReceive_No { get; set; }
            public Boolean CheckqtyPO { get; set; }
            public Boolean Message { get; set; }
        }
    }
    public class ListGoodsReceiveDocViewModel
    {
        public List<GoodsReceiveDocViewModel> items { get; set; }
        public string userName { get; set; }
    }

    public class result_goodissue
    {
        public Guid Po_Index { get; set; }
        public string planGoodsIssueItem_Index { get; set; }
        public string planGoodsIssue_Index { get; set; }

        public string goodsIssueItem_Index { get; set; }

        public string goodsIssue_Index { get; set; }

        public string lineNum { get; set; }

        public string tagItem_Index { get; set; }

        public string tag_Index { get; set; }

        public string tag_No { get; set; }

        public string product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public string itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public string location_Index { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }
        public decimal? qtyPlan { get; set; }

        public decimal qty { get; set; }

        public decimal ratio { get; set; }

        public decimal totalQty { get; set; }

        public string productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }
        public string productConversion_Base { get; set; }

        public string mfg_Date { get; set; }

        public string exp_Date { get; set; }

        public decimal? unitWeight { get; set; }

        public decimal weight { get; set; }

        public decimal unitWidth { get; set; }

        public decimal unitLength { get; set; }

        public decimal unitHeight { get; set; }

        public decimal unitVolume { get; set; }

        public decimal volume { get; set; }

        public decimal? unitPrice { get; set; }

        public decimal price { get; set; }

        public string documentRef_No1 { get; set; }

        public string documentRef_No2 { get; set; }

        public string documentRef_No3 { get; set; }

        public string documentRef_No4 { get; set; }

        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }

        public string udf_1 { get; set; }

        public string udf_2 { get; set; }

        public string udf_3 { get; set; }

        public string udf_4 { get; set; }

        public string udf_5 { get; set; }

        public string ref_Process_Index { get; set; }

        public string ref_Document_No { get; set; }

        public string ref_Document_LineNum { get; set; }

        public string ref_Document_Index { get; set; }

        public string ref_DocumentItem_Index { get; set; }

        public string goodsReceiveItem_Index { get; set; }

        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string update_By { get; set; }

        public string update_Date { get; set; }

        public string cancel_By { get; set; }

        public string cancel_Date { get; set; }

        public int? picking_Status { get; set; }

        public string picking_By { get; set; }

        public string picking_Date { get; set; }

        public string picking_Ref1 { get; set; }

        public string picking_Ref2 { get; set; }

        public decimal? picking_Qty { get; set; }

        public decimal? picking_Ratio { get; set; }

        public decimal? picking_TotalQty { get; set; }

        public string picking_ProductConversion_Index { get; set; }

        public int? mashall_Status { get; set; }

        public decimal? mashall_Qty { get; set; }

        public int? cancel_Status { get; set; }
        public string goodsIssue_No { get; set; }

        public string goodsReceive_No { get; set; }
        public string goodsReceive_date { get; set; }
        public string goodsReceive_Index { get; set; }
        public string binBalance_Index { get; set; }

        public string warehouse_Name_To { get; set; }
        public string documentItem_Remark { get; set; }

        public string invoice_No { get; set; }
        public string invoice_No_Out { get; set; }
        public string declaration_No { get; set; }
        public string declaration_No_Out { get; set; }
        public string hs_Code { get; set; }
        public string conutry_of_Origin { get; set; }
        public decimal? tax1 { get; set; }
        public Guid? tax1_Currency_Index { get; set; }
        public string tax1_Currency_Id { get; set; }
        public string tax1_Currency_Name { get; set; }
        public decimal? tax2 { get; set; }
        public Guid? tax2_Currency_Index { get; set; }
        public string tax2_Currency_Id { get; set; }
        public string tax2_Currency_Name { get; set; }
        public decimal? tax3 { get; set; }
        public Guid? tax3_Currency_Index { get; set; }
        public string tax3_Currency_Id { get; set; }
        public string tax3_Currency_Name { get; set; }
        public decimal? tax4 { get; set; }
        public Guid? tax4_Currency_Index { get; set; }
        public string tax4_Currency_Id { get; set; }
        public string tax4_Currency_Name { get; set; }
        public decimal? tax5 { get; set; }
        public Guid? tax5_Currency_Index { get; set; }
        public string tax5_Currency_Id { get; set; }
        public string tax5_Currency_Name { get; set; }
        public string create_date_balance { get; set; }
        public string product_Id_RefNo2 { get; set; }
        public string ERP_location { get; set; }
    }
}
