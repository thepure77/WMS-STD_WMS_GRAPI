using GRBusiness.PlanGoodsReceive;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{


    public partial class GoodsReceiveTagItemViewModel
    {
        [Key]
        public Guid tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }


        public string tag_No { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? process_Index { get; set; }

        public Guid product_Index { get; set; }



        public string product_Id { get; set; }



        public string product_Name { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }


        public string product_Lot { get; set; }

        public Guid itemStatus_Index { get; set; }


        public string itemStatus_Id { get; set; }


        public string itemStatus_Name { get; set; }


        public decimal? qty { get; set; }


        public decimal productConversion_Ratio { get; set; }


        public decimal totalQty { get; set; }

        public Guid productConversion_Index { get; set; }



        public string productConversion_Id { get; set; }



        public string productConversion_Name { get; set; }


        public decimal? weight { get; set; }


        public decimal? volume { get; set; }


        public string mfg_Date { get; set; }


        public string exp_Date { get; set; }

        public string tagRef_No1 { get; set; }


        public string tagRef_No2 { get; set; }


        public string tagRef_No3 { get; set; }


        public string tagRef_No4 { get; set; }


        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }


        public string udf_1 { get; set; }


        public string udf_2 { get; set; }


        public string udf_3 { get; set; }


        public string udf_4 { get; set; }


        public string udf_5 { get; set; }


        public string create_By { get; set; }


        public string create_Date { get; set; }


        public string update_By { get; set; }


        public string update_Date { get; set; }


        public string cancel_By { get; set; }


        public string cancel_Date { get; set; }

        public Guid planGoodsReceive_Index { get; set; }

        public Guid planGoodsReceiveItem_Index { get; set; }

        public Guid owner_Index { get; set; }


        public string owner_Id { get; set; }


        public string owner_Name { get; set; }

        public Guid documentType_Index { get; set; }


        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }


        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }




        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string document_Remark { get; set; }


        public string udf1 { get; set; }


        public string udf2 { get; set; }


        public string udf3 { get; set; }


        public string udf4 { get; set; }


        public string udf5 { get; set; }

        public int? documentPriority_Status { get; set; }

        public int? putaway_Status { get; set; }

        public Guid? warehouse_Index { get; set; }


        public string warehouse_Id { get; set; }


        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public Guid? dockDoor_Index { get; set; }


        public string dockDoor_Id { get; set; }


        public string dockDoor_Name { get; set; }

        public Guid? vehicleType_Index { get; set; }


        public string vehicleType_Id { get; set; }


        public string vehicleType_Name { get; set; }

        public Guid? containerType_Index { get; set; }


        public string containerType_Id { get; set; }


        public string containerType_Name { get; set; }

        public string planGoodsReceive_Date { get; set; }

        public List<GoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }

        public QtyGenTagViewModel qtyGenTag { get; set; }
        public Guid? suggest_Location_Index { get; set; }

        public string suggest_Location_Id { get; set; }
        public string suggest_Location_Name { get; set; }
        public string erp_Location { get; set; }
    }

    public class QtyGenTagViewModel
    {
        public decimal qty { get; set; }

        public decimal volume { get; set; }

        public decimal weight { get; set; }

        public decimal qtyPerTag { get; set; }

        public decimal qtyOfTag { get; set; }
    }
}