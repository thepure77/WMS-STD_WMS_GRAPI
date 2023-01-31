using GRBusiness.ConfigModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class ScanReceiveViewModel : Pagination
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
        public string product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public string productConversion_Name { get; set; }
        public string productqty { get; set; }
        public string ItemStatus_Name { get; set; }
        public string uomBase { get; set; }
        public string suggestLocation { get; set; }
        public string productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string baseProductConversion { get; set; }

        public int? isLot { get; set; }
        public int? isExpDate { get; set; }
        public int? isMfgDate { get; set; }
        public int? isCatchWeight { get; set; }
        public int? productConversion_Ratio { get; set; }
        public int? productitemlife_y { get; set; }
        public int? productitemlife_m { get; set; }
        public int? productitemlife_d { get; set; }
        public int? ratio { get; set; }
        public string itemStatus_Index { get; set; }
        public string itemStatus_Id { get; set; }
        public string itemStatus_Name { get; set; }
        public string volume { get; set; }
        public string weight { get; set; }
        public string tag_Index { get; set; }
        public string tag_No { get; set; }
        public decimal? qty { get; set; }
        public string EXPDate { get; set; }
        public string MFGDate { get; set; }
        public string ProductLot { get; set; }
        public string CatchWeight { get; set; }
        public decimal? total { get; set; }
        public string create_By { get; set; }
        public string planGoodsReceive_Date { get; set; }
        public string tagItem_Index { get; set; }
        public string tagRef_No1 { get; set; }
        public string tagRef_No2 { get; set; }
        public string tagRef_No3 { get; set; }
        public string tagRef_No4 { get; set; }
        public string tagRef_No5 { get; set; }
        public string update_By { get; set; }

        public List<GoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }
        public string documentRef_No1 { get; internal set; }
        public string documentRef_No2 { get; internal set; }
        public string documentRef_No3 { get; internal set; }
        public string documentRef_No4 { get; internal set; }
        public string documentRef_No5 { get; internal set; }
        public string udf_1 { get; internal set; }
        public string udf_2 { get; internal set; }
        public string udf_3 { get; internal set; }
        public string udf_4 { get; internal set; }
        public string udf_5 { get; internal set; }
        public string document_Remark { get; internal set; }
        public string update_Date { get; internal set; }
        public string cancel_By { get; internal set; }
        public string cancel_Date { get; internal set; }
        public string warehouse_Index { get; internal set; }
        public string warehouse_Id { get; internal set; }
        public string warehouse_Name { get; internal set; }
        public string warehouse_Index_To { get; internal set; }
        public string warehouse_Id_To { get; internal set; }
        public string warehouse_Name_To { get; internal set; }
        public string dockDoor_Index { get; internal set; }
        public string dockDoor_Id { get; internal set; }
        public string dockDoor_Name { get; internal set; }
        public string vehicleType_Index { get; internal set; }
        public string vehicleType_Id { get; internal set; }
        public string vehicleType_Name { get; internal set; }
        public string containerType_Index { get; internal set; }
        public string containerType_Id { get; internal set; }
        public string containerType_Name { get; internal set; }
        public decimal? price { get; set; }

        public class actionResultScanReceiveViewModel
        {
            public IList<ResultScanReceiveViewModel> item { get; set; }
            public Pagination pagination { get; set; }
            public string msg { get; set; }
            public bool isUse { get; set; }
        }

        public class ResultScanReceiveViewModel
        {
            public string goodsReceive_Index { get; set; }

            public string goodsReceive_No { get; set; }

            public string goodsReceive_Date { get; set; }

            public string goodsReceive_Date_To { get; set; }

            public string planGoodsReceive_Index { get; set; }

            public string planGoodsReceive_No { get; set; }

            public string owner_Index { get; set; }

            public string owner_Id { get; set; }

            public string owner_Name { get; set; }

            public string documentType_Index { get; set; }

            public string documentType_Id { get; set; }

            public string documentType_Name { get; set; }

            public string whOwner_Index { get; set; }

            public string whOwner_Id { get; set; }

            public string whOwner_Name { get; set; }
            public string planGoodsReceive_Date { get; set; }

        }
    }
}
