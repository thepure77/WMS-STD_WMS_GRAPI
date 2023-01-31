using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public class View_TagitemSugesstion
    {
        [Key]
        public Guid TagItem_Index { get; set; }
        public Guid? Tag_Index { get; set; }
        public string Tag_No { get; set; }
        public Guid? Process_Index { get; set; }
        public Guid Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_SecondName { get; set; }
        public string Product_ThirdName { get; set; }
        public string Product_Lot { get; set; }
        public Guid ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }
        public Guid? Suggest_Location_Index { get; set; }
        public string Suggest_Location_Id { get; set; }
        public string Suggest_Location_Name { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalQty { get; set; }
        public Guid ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public DateTime? MFG_Date { get; set; }
        public DateTime? EXP_Date { get; set; }
        public string TagRef_No1 { get; set; }
        public string TagRef_No2 { get; set; }
        public string TagRef_No3 { get; set; }
        public string TagRef_No4 { get; set; }
        public string TagRef_No5 { get; set; }
        public int? Tag_Status { get; set; }
        public string Tagitem_UDF1 { get; set; }
        public string Tagitem_UDF2 { get; set; }
        public string Tagitem_UDF3 { get; set; }
        public string Tagitem_UDF4 { get; set; }
        public string Tagitem_UDF5 { get; set; }
        public string Tagitem_UserAssign { get; set; }
        public Guid GoodsReceiveItem_Index { get; set; }
        public string LineNum { get; set; }
        public decimal? QtyPlan { get; set; }
        public Guid? Pallet_Index { get; set; }
        public decimal? UnitWeight { get; set; }
        public decimal? UnitWidth { get; set; }
        public decimal? UnitLength { get; set; }
        public decimal? UnitHeight { get; set; }
        public decimal? UnitVolume { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Price { get; set; }
        public string GoodsReceiveItem_DocumentRef_No1 { get; set; }
        public string GoodsReceiveItem_DocumentRef_No2 { get; set; }
        public string GoodsReceiveItem_DocumentRef_No3 { get; set; }
        public string GoodsReceiveItem_DocumentRef_No4 { get; set; }
        public string GoodsReceiveItem_DocumentRef_No5 { get; set; }
        public int? GoodsReceiveItem_Document_Status { get; set; }
        public string GoodsReceiveItem_UDF1 { get; set; }
        public string GoodsReceiveItem_UDF2 { get; set; }
        public string GoodsReceiveItem_UDF3 { get; set; }
        public string GoodsReceiveItem_UDF4 { get; set; }
        public string GoodsReceiveItem_UDF5 { get; set; }
        public Guid? Ref_Process_Index { get; set; }
        public string Ref_Document_No { get; set; }
        public string Ref_Document_LineNum { get; set; }
        public Guid? Ref_Document_Index { get; set; }
        public Guid? Ref_DocumentItem_Index { get; set; }
        public string GoodsReceive_Remark { get; set; }
        public string GoodsReceive_DockDoor { get; set; }
        public Guid GoodsReceive_Index { get; set; }
        public Guid Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Guid? DocumentType_Index { get; set; }
        public string DocumentType_Id { get; set; }
        public string DocumentType_Name { get; set; }
        public string GoodsReceive_No { get; set; }
        public DateTime GoodsReceive_Date { get; set; }
        public string GoodsReceive_DocumentRef_No1 { get; set; }
        public string GoodsReceive_DocumentRef_No2 { get; set; }
        public string GoodsReceive_DocumentRef_No3 { get; set; }
        public string GoodsReceive_DocumentRef_No4 { get; set; }
        public string GoodsReceive_DocumentRef_No5 { get; set; }
        public int? GoodsReceive_Document_Status { get; set; }
        public string Document_Remark { get; set; }
        public string GoodsReceive_UDF1 { get; set; }
        public string GoodsReceive_UDF2 { get; set; }
        public string GoodsReceive_UDF3 { get; set; }
        public string GoodsReceive_UDF4 { get; set; }
        public string GoodsReceive_UDF5 { get; set; }
        public int? DocumentPriority_Status { get; set; }
        public int? Putaway_Status { get; set; }
        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public Guid? Warehouse_Index_To { get; set; }
        public string Warehouse_Id_To { get; set; }
        public string Warehouse_Name_To { get; set; }
        public Guid? DockDoor_Index { get; set; }
        public string DockDoor_Id { get; set; }
        public string DockDoor_Name { get; set; }
        public Guid? VehicleType_Index { get; set; }
        public string VehicleType_Id { get; set; }
        public string VehicleType_Name { get; set; }
        public Guid? ContainerType_Index { get; set; }
        public string ContainerType_Id { get; set; }
        public string ContainerType_Name { get; set; }
        public string GoodsReceive_UserAssign { get; set; }
        public string Invoice_No { get; set; }
        public Guid? Vendor_Index { get; set; }
        public string Vendor_Id { get; set; }
        public string Vendor_Name { get; set; }
        public Guid? WHOwner_Index { get; set; }
        public string WHOwner_Id { get; set; }
        public string WHOwner_Name { get; set; }
    }
}
