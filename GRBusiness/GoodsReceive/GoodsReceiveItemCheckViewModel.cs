using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class GoodsReceiveItemCheckViewModel
    {

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? goodsReceive_Index { get; set; }


        public string LineNum { get; set; }

        public Guid product_Index { get; set; }


        public string product_Id { get; set; }


        public string product_Name { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }


        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }


        public string itemStatus_Id { get; set; }


        public string itemStatus_Name { get; set; }


        public decimal qtyPlan { get; set; }


        public string qty { get; set; }


        public decimal? ratio { get; set; }


        public decimal? totalQty { get; set; }

        public Guid productConversion_Index { get; set; }



        public string productConversion_Id { get; set; }


        public string productConversion_Name { get; set; }

        public Guid pallet_Index { get; set; }


        public string mfg_Date { get; set; }


        public string exp_Date { get; set; }


        

        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string udf_1 { get; set; }


        public string udf_2 { get; set; }


        public string udf_3 { get; set; }


        public string udf_4 { get; set; }


        public string udf_5 { get; set; }

        public Guid ref_Process_Index { get; set; }

        public string ref_Document_No { get; set; }

        public string ref_Document_LineNum { get; set; }

        public Guid? ref_Document_Index { get; set; }

        public Guid? ref_DocumentItem_Index { get; set; }



        public string goodsReceive_Remark { get; set; }
        public string erp_Location { get; set; }

        public string goodsReceive_DockDoor { get; set; }


        public string create_By { get; set; }


        public string create_Date { get; set; }


        public string update_By { get; set; }


        public string update_Date { get; set; }


        public string cancel_By { get; set; }


        public string cancel_Date { get; set; }

        public decimal TotalQtyPlanGR { get; set; }
        public decimal? unitWeight { get; set; }


        public decimal? weight { get; set; }


        public decimal? netWeight { get; set; }

        public Guid? weight_Index { get; set; }


        public string weight_Id { get; set; }


        public string weight_Name { get; set; }
        public decimal? weightRatio { get; set; }


        public decimal? unitGrsWeight { get; set; }


        public decimal? grsWeight { get; set; }

        public Guid? grsWeight_Index { get; set; }


        public string grsWeight_Id { get; set; }


        public string grsWeight_Name { get; set; }
        public decimal? grsWeightRatio { get; set; }


        public decimal? unitWidth { get; set; }


        public decimal? width { get; set; }

        public Guid? width_Index { get; set; }


        public string width_Id { get; set; }


        public string width_Name { get; set; }

        public decimal? widthRatio { get; set; }

        public decimal? unitLength { get; set; }


        public decimal? length { get; set; }

        public Guid? length_Index { get; set; }


        public string length_Id { get; set; }


        public string length_Name { get; set; }
        public decimal? lengthRatio { get; set; }



        public decimal? unitHeight { get; set; }


        public decimal? height { get; set; }

        public Guid? height_Index { get; set; }


        public string height_Id { get; set; }


        public string height_Name { get; set; }
        public decimal? heightRatio { get; set; }


        public decimal? unitVolume { get; set; }


        public decimal? volume { get; set; }

        public decimal? unitPrice { get; set; }


        public decimal? price { get; set; }
        public decimal? totalPrice { get; set; }


        public Guid? currency_Index { get; set; }


        public string currency_Id { get; set; }


        public string currency_Name { get; set; }


        public string ref_Code1 { get; set; }


        public string ref_Code2 { get; set; }


        public string ref_Code3 { get; set; }


        public string ref_Code4 { get; set; }


        public string ref_Code5 { get; set; }

        public string invoice_No { get; set; }

        public string declaration_No { get; set; }
        public string hS_Code { get; set; }
        public string conutry_of_Origin { get; set; }

        public decimal? tax1 { get; set; }
        public Guid? tax1_Currency_Index { get; set; }
        public string tax1_Currency_Id { get; set; }
        public string tax1_Currency_Name { get; set; }

        public decimal? tax2 { get; set; }
        public Guid? tax2_Currency_Index { get; set; }
        public string tax2_Currency_Id { get; set; }
        public string tax2_Currency_Name { get; set; }

        public decimal? tax3 { get; set; }
        public Guid? tax3_Currency_Index { get; set; }
        public string tax3_Currency_Id { get; set; }
        public string tax3_Currency_Name { get; set; }

        public decimal? tax4 { get; set; }
        public Guid? tax4_Currency_Index { get; set; }
        public string tax4_Currency_Id { get; set; }
        public string tax4_Currency_Name { get; set; }

        public decimal? tax5 { get; set; }
        public Guid? tax5_Currency_Index { get; set; }
        public string tax5_Currency_Id { get; set; }
        public string tax5_Currency_Name { get; set; }
        public string TIHI { get; set; }
        public decimal? QtyPer { get; set; }


    }
}
