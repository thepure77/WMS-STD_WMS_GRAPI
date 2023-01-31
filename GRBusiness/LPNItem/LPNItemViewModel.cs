using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.LPNItem
{
    public partial class LPNItemViewModel
    {
        public Guid? tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public Guid? suggest_Location_Index { get; set; }

        public string suggest_Location_Id { get; set; }

        public string suggest_Location_Name { get; set; }

        public string qty { get; set; }

        public decimal? ratio { get; set; }

        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public string weight { get; set; }

        public string volume { get; set; }

        public DateTime? mFG_Date { get; set; }

        public DateTime? eXP_Date { get; set; }

        public string tagRef_No1 { get; set; }

        public string tagRef_No2 { get; set; }

        public string tagRef_No3 { get; set; }

        public string tagRef_No4 { get; set; }

        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string putaway_By { get; set; }
        public string putaway_Status { get; set; }
        public string erp_Location { get; set; }



        public List<LPNItemViewModel> listLPNItemViewModel { get; set; }
    }

    public class View_TagitemSugesstionViewModel
    {
        public Guid tagItem_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public string tag_No { get; set; }
        public Guid? process_Index { get; set; }
        public Guid product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string product_Lot { get; set; }
        public Guid itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public Guid? suggest_Location_Index { get; set; }
        public string suggest_Location_Id { get; set; }
        public string suggest_Location_Name { get; set; }
        public decimal? qty { get; set; }
        public decimal? ratio { get; set; }
        public decimal? totalQty { get; set; }
        public Guid productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public decimal? weight { get; set; }
        public decimal? volume { get; set; }
        public DateTime? mfg_Date { get; set; }
        public DateTime? exp_Date { get; set; }
        public string tagRef_No1 { get; set; }
        public string tagRef_No2 { get; set; }
        public string tagRef_No3 { get; set; }
        public string tagRef_No4 { get; set; }
        public string tagRef_No5 { get; set; }
        public int? tag_Status { get; set; }
        public string tagitem_UDF1 { get; set; }
        public string tagitem_UDF2 { get; set; }
        public string tagitem_UDF3 { get; set; }
        public string tagitem_UDF4 { get; set; }
        public string tagitem_UDF5 { get; set; }
        public string tagitem_UserAssign { get; set; }
        public Guid goodsReceiveItem_Index { get; set; }
        public string lineNum { get; set; }
        public decimal? qtyPlan { get; set; }
        public Guid? pallet_Index { get; set; }
        public decimal? unitWeight { get; set; }
        public decimal? unitWidth { get; set; }
        public decimal? unitLength { get; set; }
        public decimal? unitHeight { get; set; }
        public decimal? unitVolume { get; set; }
        public decimal? unitPrice { get; set; }
        public decimal? price { get; set; }
        public string goodsReceiveItem_DocumentRef_No1 { get; set; }
        public string goodsReceiveItem_DocumentRef_No2 { get; set; }
        public string goodsReceiveItem_DocumentRef_No3 { get; set; }
        public string goodsReceiveItem_DocumentRef_No4 { get; set; }
        public string goodsReceiveItem_DocumentRef_No5 { get; set; }
        public int? goodsReceiveItem_Document_Status { get; set; }
        public string goodsReceiveItem_UDF1 { get; set; }
        public string goodsReceiveItem_UDF2 { get; set; }
        public string goodsReceiveItem_UDF3 { get; set; }
        public string goodsReceiveItem_UDF4 { get; set; }
        public string goodsReceiveItem_UDF5 { get; set; }
        public Guid? ref_Process_Index { get; set; }
        public string ref_Document_No { get; set; }
        public string ref_Document_LineNum { get; set; }
        public Guid? ref_Document_Index { get; set; }
        public Guid? ref_DocumentItem_Index { get; set; }
        public string goodsReceive_Remark { get; set; }
        public string goodsReceive_DockDoor { get; set; }
        public Guid goodsReceive_Index { get; set; }
        public Guid owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string goodsReceive_No { get; set; }
        public DateTime goodsReceive_Date { get; set; }
        public string goodsReceive_DocumentRef_No1 { get; set; }
        public string goodsReceive_DocumentRef_No2 { get; set; }
        public string goodsReceive_DocumentRef_No3 { get; set; }
        public string goodsReceive_DocumentRef_No4 { get; set; }
        public string goodsReceive_DocumentRef_No5 { get; set; }
        public int? goodsReceive_Document_Status { get; set; }
        public string document_Remark { get; set; }
        public string goodsReceive_UDF1 { get; set; }
        public string goodsReceive_UDF2 { get; set; }
        public string goodsReceive_UDF3 { get; set; }
        public string goodsReceive_UDF4 { get; set; }
        public string goodsReceive_UDF5 { get; set; }
        public int? documentPriority_Status { get; set; }
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
        public string goodsReceive_UserAssign { get; set; }
        public string invoice_No { get; set; }
        public Guid? vendor_Index { get; set; }
        public string vendor_Id { get; set; }
        public string vendor_Name { get; set; }
        public Guid? whOwner_Index { get; set; }
        public string whOwner_Id { get; set; }
        public string whOwner_Name { get; set; }
        public Guid productCategory_Index { get; set; }
        public string productCategory_Id { get; set; }
        public string productCategory_Name { get; set; }
        public Guid productType_Index { get; set; }
        public Guid productSubType_Index { get; set; }
    }
}
