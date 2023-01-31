using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class BinCardViewModelV2
    {
        public Guid? process_Index { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public Guid? goodsreceive_Index { get; set; }
        public Guid? goodsreceiveItem_Index { get; set; }
        public Guid? goodsreceiveItemLocation_Index { get; set; }
        public string bincard_No { get; set; }
        public DateTime? binCard_Date { get; set; }
        public Guid? tagitem_Index { get; set; }
        public Guid? tag_index { get; set; }
        public string tag_no { get; set; }
        public Guid? tag_index_To { get; set; }
        public string tag_no_To { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public Guid? product_Index_To { get; set; }
        public string product_Id_To { get; set; }
        public string product_Name_To { get; set; }
        public string product_SecondName_To { get; set; }
        public string product_ThirdName_To { get; set; }
        public string product_Lot { get; set; }
        public string product_Lot_To { get; set; }
        public Guid? itemstatus_Index { get; set; }
        public string itemstatus_Id { get; set; }
        public string itemstatus_Name { get; set; }
        public Guid? itemstatus_Index_To { get; set; }
        public string itemstatus_Id_To { get; set; }
        public string itemstatus_Name_To { get; set; }
        public Guid? productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public Guid? owner_index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? owner_index_To { get; set; }
        public string owner_Id_To { get; set; }
        public string owner_Name_To { get; set; }
        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public Guid? location_Index_To { get; set; }
        public string location_Id_To { get; set; }
        public string location_Name_To { get; set; }
        public DateTime? goodsReceive_EXP_Date { get; set; }
        public DateTime? goodsReceive_EXP_Date_To { get; set; }
        public decimal? bincard_QtyIn { get; set; }
        public decimal? bincard_QtyOut { get; set; }
        public decimal? bincard_QtySign { get; set; }
        public decimal? bincard_WeightIn { get; set; }
        public decimal? bincard_WeightOut { get; set; }
        public decimal? bincard_WeightSign { get; set; }
        public decimal? bincard_VolumeIn { get; set; }
        public decimal? bincard_VolumeOut { get; set; }
        public decimal? bincard_VolumeSign { get; set; }
        public string ref_document_No { get; set; }
        public Guid? ref_document_Index { get; set; }
        public Guid? ref_documentItem_Index { get; set; }
        public string tagoutItem_Index { get; set; }
        public Guid? tagout_Index { get; set; }
        public string tagout_No { get; set; }
        public Guid? tagout_Index_To { get; set; }
        public string tagout_No_To { get; set; }
        public string udf_1 { get; set; }
        public string udf_2 { get; set; }
        public string udf_3 { get; set; }
        public string udf_4 { get; set; }
        public string udf_5 { get; set; }
        public string create_By { get; set; }
        public DateTime? create_Date { get; set; }
        public Guid? binbalance_Index { get; set; }

        public decimal? totalQty { get; set; }
        public decimal? weight { get; set; }
        public decimal? volume { get; set; }
        public bool isCheckBinCard { get; set; }
    }
}
