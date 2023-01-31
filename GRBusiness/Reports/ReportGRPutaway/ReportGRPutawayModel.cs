using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.Reports
{
    public  class ReportGRPutawayModel
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
    }
}
