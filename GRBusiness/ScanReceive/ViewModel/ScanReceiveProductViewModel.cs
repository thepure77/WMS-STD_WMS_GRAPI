using GRBusiness.ConfigModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class ScanReceiveProductViewModel : Pagination
    {
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string planGoodsReceive_Index { get; set; }
        public string planGoodsReceive_No { get; set; }
        public string planGoodsReceiveItem_Index { get; set; }
        public OwnerViewModelV2 dropdownOwner { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public DocumentTypeViewModel dropdownDocumentType { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string whOwner_index { get; set; }
        public string whOwner_ID { get; set; }
        public string whOwner_Name { get; set; }
        public string goodsReceive_Date { get; set; }
        public string goodsReceive_Date_To { get; set; }
        public string productBarcode { get; set; }
        public class actionResultScanReceiveProductViewModel
        {
            public IList<ResultScanReceiveProductViewModel> item { get; set; }
            public Pagination pagination { get; set; }
            public string msg { get; set; }
            public bool isUse { get; set; }
        }

        public class ResultScanReceiveProductViewModel
        {
            //Header
            public string goodsReceive_Index { get; set; }
            public string goodsReceive_No { get; set; }
            public string goodsReceive_Date { get; set; }
            public string goodsReceive_Date_To { get; set; }
            public string planGoodsReceive_Index { get; set; }
            public string planGoodsReceive_No { get; set; }
            public string planGoodsReceive_Date { get; set; }
            public string owner_Index { get; set; }
            public string owner_Id { get; set; }
            public string owner_Name { get; set; }
            public string documentType_Index { get; set; }
            public string documentType_Id { get; set; }
            public string documentType_Name { get; set; }
            public string whOwner_Index { get; set; }
            public string whOwner_Id { get; set; }
            public string whOwner_Name { get; set; }
            public string productBarcode { get; set; }

            //Product
            public string product_Index { get; set; }
            public string product_Id { get; set; }
            public string product_Name { get; set; }
            public string productConversion_Index { get; set; }
            public string productConversion_Id { get; set; }
            public string productConversion_Name { get; set; }
            public string baseProductConversion { get; set; }
            public string product_SecondName { get; set; }
            public string product_ThirdName { get; set; }
            public int? isLot { get; set; }
            public int? isExpDate { get; set; }
            public int? isMfgDate { get; set; }
            public int? isCatchWeight { get; set; }
            public decimal productConversion_Ratio { get; set; }
            public int? productitemlife_y { get; set; }
            public int? productitemlife_m { get; set; }
            public int? productitemlife_d { get; set; }
            public string suggestLocation { get; set; }

            //GRItem
            public decimal? ratio { get; set; }
            public string planGoodsReceiveItem_Index { get; set; }
            public string itemStatus_Index { get; set; }
            public string itemStatus_Id { get; set; }
            public string itemStatus_Name { get; set; }
            public decimal? volume { get; set; }
            public decimal? weight { get; set; }
            public decimal? totalQty { get; set; }
            public decimal? qtyPlan { get; set; }

        }
    }
}
