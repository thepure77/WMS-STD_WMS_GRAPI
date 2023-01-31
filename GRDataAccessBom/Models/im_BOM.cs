namespace BomDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class im_BOM
    {
        [Key]
        public Guid BOM_Index { get; set; }
        public Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Guid? DocumentType_Index { get; set; }
        public string DocumentType_Id { get; set; }
        public string DocumentType_Name { get; set; }
        public string BOM_No { get; set; }
        public DateTime? BOM_Date { get; set; }
        public DateTime? BOM_Due_Date { get; set; }
        public Guid? Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_SecondName { get; set; }
        public string Product_ThirdName { get; set; }
        public string Product_Lot { get; set; }
        public Guid? ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Ratio { get; set; }
        public decimal? TotalQty { get; set; }
        public Guid? ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public DateTime? MFG_Date { get; set; }
        public DateTime? EXP_Date { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Price { get; set; }
        public string DocumentRef_No1 { get; set; }
        public string DocumentRef_No2 { get; set; }
        public string DocumentRef_No3 { get; set; }
        public string DocumentRef_No4 { get; set; }
        public string DocumentRef_No5 { get; set; }
        public string Document_Remark { get; set; }
        public int? Document_Status { get; set; }
        public string UDF_1 { get; set; }
        public string UDF_2 { get; set; }
        public string UDF_3 { get; set; }
        public string UDF_4 { get; set; }
        public string UDF_5 { get; set; }
        public int? DocumentPriority_Status { get; set; }
        public Guid? Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public Guid? Warehouse_Index_To { get; set; }
        public string Warehouse_Id_To { get; set; }
        public string Warehouse_Name_To { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Update_By { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public DateTime? Cancel_Date { get; set; }
        public Guid? Round_Index { get; set; }
        public string Round_Id { get; set; }
        public string Round_Name { get; set; }
        public Guid? Route_Index { get; set; }
        public string Route_Id { get; set; }
        public string Route_Name { get; set; }
        public int? BackOrderStatus { get; set; }
        public Guid? ReasonCode_Index { get; set; }
        public string ReasonCode_Id { get; set; }
        public string ReasonCode_Name { get; set; }
        public string UserAssign { get; set; }
        public Guid? Ref_WavePick_index { get; set; }
        public decimal? UnitWeight { get; set; }
        public decimal? Weight { get; set; }
        public decimal? NetWeight { get; set; }
        public Guid? Weight_Index { get; set; }
        public string Weight_Id { get; set; }
        public string Weight_Name { get; set; }
        public decimal? WeightRatio { get; set; }
        public decimal? UnitGrsWeight { get; set; }
        public decimal? GrsWeight { get; set; }
        public Guid? GrsWeight_Index { get; set; }
        public string GrsWeight_Id { get; set; }
        public string GrsWeight_Name { get; set; }
        public decimal? GrsWeightRatio { get; set; }
        public decimal? UnitWidth { get; set; }
        public decimal? Width { get; set; }
        public Guid? Width_Index { get; set; }
        public string Width_Id { get; set; }
        public string Width_Name { get; set; }
        public decimal? WidthRatio { get; set; }
        public decimal? UnitLength { get; set; }
        public decimal? Length { get; set; }
        public Guid? Length_Index { get; set; }
        public string Length_Id { get; set; }
        public string Length_Name { get; set; }
        public decimal? LengthRatio { get; set; }
        public decimal? UnitHeight { get; set; }
        public decimal? Height { get; set; }
        public Guid? Height_Index { get; set; }
        public string Height_Id { get; set; }
        public string Height_Name { get; set; }
        public decimal? HeightRatio { get; set; }
        public decimal? UnitVolume { get; set; }
        public decimal? Volume { get; set; }
    }
}
