using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public class PopupPlanGoodsReceiveViewModel
    {
        [Key]
        public Guid planGoodsReceiveItem_Index { get; set; }

        public Guid planGoodsReceive_Index { get; set; }

        [StringLength(50)]
        public string lineNum { get; set; }

        public Guid? product_Index { get; set; }


        [StringLength(50)]
        public string product_Id { get; set; }

        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string product_SecondName { get; set; }

        [StringLength(200)]
        public string product_ThirdName { get; set; }


        [StringLength(50)]
        public string product_Lot { get; set; }


        [Column(TypeName = "numeric")]
        public decimal qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }


        [StringLength(50)]
        public string productConversion_Id { get; set; }


        [StringLength(200)]
        public string productConversion_Name { get; set; }

        [Column(TypeName = "date")]
        public string mfg_Date { get; set; }

        [Column(TypeName = "date")]
        public string exp_Date { get; set; }

        [StringLength(200)]
        public string documentRef_No1 { get; set; }

        [StringLength(200)]
        public string documentRef_No2 { get; set; }

        [StringLength(200)]
        public string documentRef_No3 { get; set; }

        [StringLength(200)]
        public string documentRef_No4 { get; set; }

        [StringLength(200)]
        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }

        [StringLength(200)]
        public string documentItem_Remark { get; set; }

        [StringLength(200)]
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public string cancel_Date { get; set; }

        [StringLength(200)]
        public string planGoodsReceive_No { get; set; }

        public Guid? owner_Index { get; set; }

        public List<string> id { get; set; }
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
        public string erp_Location { get; set; }
        public int? isLot { get; set; }
        public int? isMfgDate { get; set; }
        public int? isExpDate { get; set; }
        public string mfgDate_Default { get; set; }
        public string expDate_Default { get; set; }
    }
}
