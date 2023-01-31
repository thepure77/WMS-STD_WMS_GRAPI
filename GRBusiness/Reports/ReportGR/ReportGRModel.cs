using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.Reports
{
    public  class ReportGRModel
    {
        [DataMember]
        public  Guid? goodsReceive_Index { get; set; }
        [DataMember]
        public string goodsReceive_No { get; set; }
        [DataMember]
        public string goodsReceive_Date { get; set; }
        [DataMember]
        public string create_Date { get; set; }
        [DataMember]
        public Guid? documentType_Index { get; set; }
        [DataMember]
        public string documentType_Id { get; set; }
        [DataMember]
        public string documentType_Name { get; set; }
        [DataMember]
        public Guid? vendor_Index { get; set; }
        [DataMember]
        public string vendor_Id { get; set; }
        [DataMember]
        public string vendor_Name { get; set; }
        [DataMember]
        public Guid? owner_Index { get; set; }
        [DataMember]
        public string owner_Id { get; set; }
        [DataMember]
        public string owner_Name { get; set; }
        [DataMember]
        public Guid? wHOwner_Index { get; set; }
        [DataMember]
        public string wHOwner_Id { get; set; }
        [DataMember]
        public string wHOwner_Name { get; set; }
        [DataMember]
        public string invoice_No { get; set; }
        [DataMember]
        public string document_Remark { get; set; }
        [DataMember]
        public string planGoodsReceive_No { get; set; }
        [DataMember]
        public Guid? product_Index { get; set; }
        [DataMember]
        public string product_Id { get; set; }
        [DataMember]
        public string product_Name { get; set; }
        [DataMember]
        public decimal? qty { get; set; }
        [DataMember]
        public Guid? productConversion_Index { get; set; }
        [DataMember]
        public string productConversion_Id { get; set; }
        [DataMember]
        public string productConversion_Name { get; set; }
        [DataMember]
        public string product_Lot { get; set; }
        [DataMember]
        public decimal? weight { get; set; }
        [DataMember]
        public decimal? volume { get; set; }
        [DataMember]
        public decimal? price { get; set; }
        [DataMember]
        public Guid? itemStatus_Index { get; set; }
        [DataMember]
        public string itemStatus_Id { get; set; }
        [DataMember]
        public string itemStatus_Name { get; set; }
        [DataMember]
        public string goodsReceiveNo_Barcode { get; set; }
        [DataMember]
        public Guid? tagItem_Index { get; set; }
        [DataMember]
        public Guid? tag_Index { get; set; }
        [DataMember]
        public string tag_No { get; set; }
        [DataMember]
        public Guid? location_Index { get; set; }
        [DataMember]
        public string location_Id { get; set; }
        [DataMember]
        public string location_Name { get; set; }
        [DataMember]
        public Guid? pallet_Index { get; set; }
        [DataMember]
        public string pallet_No { get; set; }

        //[DataMember]
        //public Guid? DocumentType_Index { get; set; }

        //[DataMember]
        //public string DocumentType_Name { get; set; }

        //[DataMember]
        //public Guid? Vendor_Index { get; set; }

        //[DataMember]
        //public string Vendor_Name { get; set; }

        //[DataMember]
        //public Guid? Owner_Index { get; set; }

        //[DataMember]
        //public string Owner_Name { get; set; }

        //[DataMember] 
        //public Guid? WHOwner_Index { get; set; }

        //[DataMember]
        //public string WHOwner_Name { get; set; }

        //[DataMember]
        //public string Invoice_No { get; set; }

        //[DataMember]
        //public string Document_Remark { get; set; }

        //[DataMember]
        //public string PlanGoodsReceive_No { get; set; }

        //[DataMember]
        //public Guid? Product_Index { get; set; }

        //[DataMember]
        //public string Product_Id { get; set; }

        //[DataMember]
        //public string Product_Name { get; set; }

        //[DataMember]
        //public string GoodsReceive_No { get; set; }

        //[DataMember]
        //public string TagNo_Barcode { get; set; }

        //[DataMember]
        //public Guid? GoodsReceive_Index { get; set; }

        //[DataMember]
        //public Guid? GoodsReceiveItem_Index { get; set; }


        //[DataMember]
        //public string Product_SecondName { get; set; }

        //[DataMember]
        //public string Product_ThirdName { get; set; }

        //[DataMember]
        //public string Product_Lot { get; set; }

        //[DataMember]
        //public Guid? ItemStatus_Index { get; set; }

        //[DataMember]
        //public string ItemStatus_Id { get; set; }

        //[DataMember]
        //public string ItemStatus_Name { get; set; }

        //[DataMember]
        //public Guid? Suggest_Location_Index { get; set; }

        //[DataMember]
        //public string Suggest_Location_Id { get; set; }

        //[DataMember]
        //public string Suggest_Location_Name { get; set; }

        //[DataMember]
        //public decimal? Qty { get; set; }

        //[DataMember]
        //public Guid? ProductConversion_Index { get; set; }

        //[DataMember]
        //public string ProductConversion_Name { get; set; }

        //[DataMember]
        //public string ProductConversion_Id { get; set; }


        //[DataMember]
        //public decimal? Weight { get; set; }

        //[DataMember]
        //public decimal? Volume { get; set; }

        //[DataMember]
        //public decimal? Price { get; set; }

        //[DataMember]
        //public string Create_By { get; set; }

        //[DataMember]
        //public DateTime? Create_Date { get; set; }

        //[DataMember]
        //public string Update_By { get; set; }

        //[DataMember]
        //public DateTime? GoodsReceive_Date { get; set; }

        //[DataMember]
        //public string Cancel_By { get; set; }

        //[DataMember]
        //public DateTime? Cancel_Date { get; set; }
    }
}
