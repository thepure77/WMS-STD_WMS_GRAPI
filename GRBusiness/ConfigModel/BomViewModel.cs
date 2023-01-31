using GRBusiness;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BomBusiness
{

    public partial class BomViewModel : Pagination
    {
        public System.Guid BOM_Index { get; set; }
        public System.Guid? Owner_Index { get; set; }
        public string Owner_Id { get; set; }
        public string Owner_Name { get; set; }
        public Nullable<System.Guid> DocumentType_Index { get; set; }
        public string DocumentType_Id { get; set; }
        public string DocumentType_Name { get; set; }
        public string BOM_No { get; set; }
        public string BOM_Date { get; set; }
        public string BOM_Due_Date { get; set; }
        public Nullable<System.Guid> Product_Index { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_SecondName { get; set; }
        public string Product_ThirdName { get; set; }
        public string Product_Lot { get; set; }
        public Nullable<System.Guid> ItemStatus_Index { get; set; }
        public string ItemStatus_Id { get; set; }
        public string ItemStatus_Name { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> Ratio { get; set; }
        public Nullable<decimal> TotalQty { get; set; }
        public Nullable<System.Guid> ProductConversion_Index { get; set; }
        public string ProductConversion_Id { get; set; }
        public string ProductConversion_Name { get; set; }
        public String MFG_Date { get; set; }
        public String EXP_Date { get; set; }
        public Nullable<decimal> UnitWeight { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<decimal> UnitWidth { get; set; }
        public Nullable<decimal> UnitLength { get; set; }
        public Nullable<decimal> UnitHeight { get; set; }
        public Nullable<decimal> UnitVolume { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string DocumentRef_No1 { get; set; }
        public string DocumentRef_No2 { get; set; }
        public string DocumentRef_No3 { get; set; }
        public string DocumentRef_No4 { get; set; }
        public string DocumentRef_No5 { get; set; }
        public string Document_Remark { get; set; }
        public Nullable<int> Document_Status { get; set; }
        public string UDF_1 { get; set; }
        public string UDF_2 { get; set; }
        public string UDF_3 { get; set; }
        public string UDF_4 { get; set; }
        public string UDF_5 { get; set; }
        public Nullable<int> DocumentPriority_Status { get; set; }
        public Nullable<System.Guid> Warehouse_Index { get; set; }
        public string Warehouse_Id { get; set; }
        public string Warehouse_Name { get; set; }
        public Nullable<System.Guid> Warehouse_Index_To { get; set; }
        public string Warehouse_Id_To { get; set; }
        public string Warehouse_Name_To { get; set; }
        public string Create_By { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public string Update_By { get; set; }
        public Nullable<System.DateTime> Update_Date { get; set; }
        public string Cancel_By { get; set; }
        public Nullable<System.DateTime> Cancel_Date { get; set; }
        public Nullable<System.Guid> Round_Index { get; set; }
        public string Round_Id { get; set; }
        public string Round_Name { get; set; }
        public Nullable<System.Guid> Route_Index { get; set; }
        public string Route_Id { get; set; }
        public string Route_Name { get; set; }
        public Nullable<int> BackOrderStatus { get; set; }
        public Nullable<System.Guid> ReasonCode_Index { get; set; }
        public string ReasonCode_Id { get; set; }
        public string ReasonCode_Name { get; set; }
        public string UserAssign { get; set; }
        public Nullable<System.Guid> Ref_WavePick_index { get; set; }
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
        public decimal? Width { get; set; }
        public Guid? Width_Index { get; set; }
        public string Width_Id { get; set; }
        public string Width_Name { get; set; }
        public decimal? WidthRatio { get; set; }
        public decimal? Length { get; set; }
        public Guid? Length_Index { get; set; }
        public string Length_Id { get; set; }
        public string Length_Name { get; set; }
        public decimal? LengthRatio { get; set; }
        public decimal? Height { get; set; }
        public Guid? Height_Index { get; set; }
        public string Height_Id { get; set; }
        public string Height_Name { get; set; }
        public decimal? HeightRatio { get; set; }
        public string processStatus_Name { get; set; }

        public Guid? Index { get; set; }

        public class actionResultBomViewModels
        {
            public IList<BomViewModel> itemsBom{ get; set; }
            public Pagination pagination { get; set; }
        }
    }
}
