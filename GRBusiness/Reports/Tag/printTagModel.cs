using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.Reports.Tag
{
    public class printTagModel
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

        public decimal? qty { get; set; }

        public decimal ratio { get; set; }

        public decimal totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public decimal? weight { get; set; }

        public decimal? volume { get; set; }

        public string mFG_Date { get; set; }

        public string eXP_Date { get; set; }

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

        public string tagNo_Barcode { get; set; }

        public string product_Barcode { get; set; }
        
        public Guid owner_Index { get; set; }
        
        public string owner_Id { get; set; }
        
        public string owner_Name { get; set; }

        public string invoice_No { get; set; }

        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

    }
    //public class printTagModel
    //{
    //    [DataMember]
    //    public Guid? TagItem_Index { get; set; }

    //    [DataMember]
    //    public Guid? Tag_Index { get; set; }

    //    [DataMember]
    //    public string Tag_No { get; set; }

    //    [DataMember]
    //    public string TagNo_Barcode { get; set; }

    //    [DataMember]
    //    public Guid? GoodsReceive_Index { get; set; }

    //    [DataMember]
    //    public Guid? GoodsReceiveItem_Index { get; set; }

    //    [DataMember]
    //    public Guid? Product_Index { get; set; }

    //    [DataMember]
    //    public string Product_Id { get; set; }

    //    [DataMember]
    //    public string Product_Name { get; set; }

    //    [DataMember]
    //    public string Product_SecondName { get; set; }

    //    [DataMember]
    //    public string Product_ThirdName { get; set; }

    //    [DataMember]
    //    public string Product_Lot { get; set; }

    //    [DataMember]
    //    public Guid? ItemStatus_Index { get; set; }

    //    [DataMember]
    //    public string ItemStatus_Id { get; set; }

    //    [DataMember]
    //    public string ItemStatus_Name { get; set; }

    //    [DataMember]
    //    public Guid? Suggest_Location_Index { get; set; }

    //    [DataMember]
    //    public string Suggest_Location_Id { get; set; }

    //    [DataMember]
    //    public string Suggest_Location_Name { get; set; }

    //    [DataMember]
    //    public decimal? Qty { get; set; }

    //    [DataMember]
    //    public decimal Ratio { get; set; }

    //    [DataMember]
    //    public decimal TotalQty { get; set; }

    //    [DataMember]
    //    public Guid? ProductConversion_Index { get; set; }

    //    [DataMember]
    //    public string ProductConversion_Id { get; set; }

    //    [DataMember]
    //    public string ProductConversion_Name { get; set; }

    //    [DataMember]
    //    public decimal? Weight { get; set; }

    //    [DataMember]
    //    public decimal? Volume { get; set; }

    //    [DataMember]
    //    public DateTime? MFG_Date { get; set; }

    //    [DataMember]
    //    public DateTime? EXP_Date { get; set; }

    //    [DataMember]
    //    public string TagRef_No1 { get; set; }

    //    [DataMember]
    //    public string TagRef_No2 { get; set; }

    //    [DataMember]
    //    public string TagRef_No3 { get; set; }

    //    [DataMember]
    //    public string TagRef_No4 { get; set; }

    //    [DataMember]
    //    public string TagRef_No5 { get; set; }

    //    [DataMember]
    //    public int? Tag_Status { get; set; }

    //    [DataMember]
    //    public string UDF_1 { get; set; }

    //    [DataMember]
    //    public string UDF_2 { get; set; }

    //    [DataMember]
    //    public string UDF_3 { get; set; }

    //    [DataMember]
    //    public string UDF_4 { get; set; }

    //    [DataMember]
    //    public string UDF_5 { get; set; }

    //    [DataMember]
    //    public string Create_By { get; set; }

    //    [DataMember]
    //    public DateTime? Create_Date { get; set; }

    //    [DataMember]
    //    public string Update_By { get; set; }

    //    [DataMember]
    //    public DateTime? Update_Date { get; set; }

    //    [DataMember]
    //    public string Cancel_By { get; set; }

    //    [DataMember]
    //    public DateTime? Cancel_Date { get; set; }
    //}
}
