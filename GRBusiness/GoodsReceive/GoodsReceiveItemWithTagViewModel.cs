using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class GoodsReceiveItemWithTagViewModel
    {
        public Guid goodsReceiveItem_Index { get; set; }

        public Guid goodsReceive_Index { get; set; }

        public string lineNum { get; set; }

        public Guid product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public decimal qtyPlan { get; set; }

        public string qty { get; set; }

        public decimal ratio { get; set; }

        public decimal totalQty { get; set; }

        public Guid productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public Guid pallet_Index { get; set; }

        public string mFG_Date { get; set; }

        public string eXP_Date { get; set; }

        public decimal? unitWeight { get; set; }

        public string weight { get; set; }

        public decimal? unitWidth { get; set; }

        public decimal? unitLength { get; set; }

        public decimal? unitHeight { get; set; }

        public decimal? unitVolume { get; set; }

        public string volume { get; set; }

        public decimal? unitPrice { get; set; }

        public decimal? price { get; set; }

        public string documentRef_No1 { get; set; }

        public string documentRef_No2 { get; set; }

        public string documentRef_No3 { get; set; }

        public string documentRef_No4 { get; set; }

        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public Guid ref_Process_Index { get; set; }

        public string ref_Document_No { get; set; }

        public string ref_Document_LineNum { get; set; }

        public Guid? ref_Document_Index { get; set; }

        public Guid? ref_DocumentItem_Index { get; set; }

        public string goodsReceive_Remark { get; set; }

        public string goodsReceive_DockDoor { get; set; }

        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string update_By { get; set; }

        public string update_Date { get; set; }

        public string cancel_By { get; set; }

        public string cancel_Date { get; set; }

        public decimal? gRIQty { get; set; }

        public decimal? tagIQty { get; set; }

        public string remainingQty { get; set; }
        public string remainingWeight { get; set; }
        public string remainingVolume { get; set; }

        public int? isCompleted { get; set; }
        public string putaway_Status { get; set; }
        public string erp_Location { get; set; }

    }
}
