using GRBusiness.ConfigModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class SearchScanReceiveViewModel : Pagination
    {
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string planGoodsReceive_Index { get; set; }
        public string planGoodsReceiveitem_Index { get; set; }
        public string planGoodsReceive_No { get; set; }
        public OwnerViewModelV2 dropdownOwner { get; set; }
        public string owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public DocumentTypeViewModel dropdownDocumentType { get; set; }
        public string documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public string whOwner_Index { get; set; }
        public string whOwner_Id { get; set; }
        public string whOwner_Name { get; set; }
        public string goodsReceive_Date { get; set; }
        public string productConversionBarcode { get; set; }
        public string product_Index { get; set; }
        public Guid? productConversion_Index { get; set; }


        public class actionResultSearchScanReceiveViewModel
        {
            public IList<ResultSearchScanReceiveViewModel> item { get; set; }
            public Pagination pagination { get; set; }
            public string msg { get; set; }
            public bool isUse { get; set; }
            public Guid? goodsReceive_Index { get; set; }
            public string goodsReceive_No { get; set; }

        }

        public class ResultSearchScanReceiveViewModel
        {
            public string goodsReceive_Index { get; set; }

            public string goodsReceive_No { get; set; }

            public string goodsReceive_Date { get; set; }

            public string goodsReceive_Date_To { get; set; }

            public string planGoodsReceive_Index { get; set; }
            public string planGoodsReceiveitem_Index { get; set; }

            public string planGoodsReceive_No { get; set; }
            public string planGoodsReceive_Date { get; set; }

            public string owner_Index { get; set; }

            public string owner_Id { get; set; }

            public string owner_Name { get; set; }
            public string vendor_Index { get; set; }

            public string vendor_Id { get; set; }

            public string vendor_Name { get; set; }

            public string documentType_Index { get; set; }

            public string documentType_Id { get; set; }

            public string documentType_Name { get; set; }

            public Guid? product_Index { get; set; }

            public string product_Id { get; set; }

            public string product_Name { get; set; }

            public string product_SecondName { get; set; }

            public string product_ThirdName { get; set; }

            public string Ref_No2 { get; set; }

            public decimal? productConversion_Width { get; set; }

            public decimal? productConversion_Length { get; set; }

            public decimal? productConversion_Height { get; set; }

            public string productConversionBarcode { get; set; }

            public string productConversionWLH{ get; set; }
            public Guid productConversion_Index { get; set; }

            public string productConversion_Id { get; set; }

            public string productConversion_Name { get; set; }
            public decimal? productconversion_Ratio { get; set; }

            public Guid volume_Index { get; set; }

            public string volume_Id { get; set; }

            public string volume_Name { get; set; }

            public decimal? volume_Ratio { get; set; }

            public string create_By { get; set; }

            public Guid? itemStatus_Index { get; set; }


            public string itemStatus_Id { get; set; }


            public string itemStatus_Name { get; set; }
            public decimal? qty { get; set; }
            public decimal? ratio { get; set; }
            public decimal? totalQty { get; set; }
            public string mFG_Date { get; set; }

            public string eXP_Date { get; set; }
            public int? isLot { get; set; }
            public int? isExpDate { get; set; }
            public int? isMfgDate { get; set; }
            public int? isSerial { get; set; }
            public int? ProductItemLife_D { get; set; }
            public int? ProductItemLife_Y { get; set; }
            public int? ProductItemLife_M { get; set; }
            public string product_Lot { get; set; }
            public string ERP_location { get; set; }
            public string tihi { get; set; }
            public decimal? qty_Per_Tag { get; set; }
            public string conversion { get; set; }
            public string erp_location { get; set; }


        }
    }
}
