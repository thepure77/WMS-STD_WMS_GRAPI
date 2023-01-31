using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    
    public partial class GoodsReceiveItemLocationViewModel
    {
        [Key]
        public Guid goodsReceiveItemLocation_Index { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        public Guid? goodsReceiveItem_Index { get; set; }

        public Guid? tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }


        public string tag_No { get; set; }

        public Guid? product_Index { get; set; }



        public string product_Id { get; set; }



        public string product_Name { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }


        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }



        public string itemStatus_Id { get; set; }



        public string itemStatus_Name { get; set; }

        public Guid? productConversion_Index { get; set; }



        public string productConversion_Id { get; set; }



        public string productConversion_Name { get; set; }


        public DateTime? mfg_Date { get; set; }


        public DateTime? exp_Date { get; set; }


        public decimal? unitWeight { get; set; }


        public decimal? weight { get; set; }


        public decimal? unitWidth { get; set; }


        public decimal? unitLength { get; set; }


        public decimal? unitHeight { get; set; }


        public decimal? unitVolume { get; set; }


        public decimal? volume { get; set; }


        public decimal? unitPrice { get; set; }


        public decimal? price { get; set; }

        public Guid? owner_Index { get; set; }



        public string owner_Id { get; set; }



        public string owner_Name { get; set; }

        public Guid? location_Index { get; set; }


        public string location_Id { get; set; }


        public string location_Name { get; set; }


        public decimal? qty { get; set; }


        public decimal? ratio { get; set; }


        public decimal? totalQty { get; set; }


        public string udf_1 { get; set; }


        public string udf_2 { get; set; }


        public string udf_3 { get; set; }


        public string udf_4 { get; set; }


        public string udf_5 { get; set; }


        public string create_By { get; set; }


        public DateTime? create_Date { get; set; }


        public string update_By { get; set; }


        public DateTime? update_Date { get; set; }


        public string cancel_By { get; set; }


        public DateTime? cancel_Date { get; set; }

        public int? putaway_Status { get; set; }


        public string putaway_By { get; set; }


        public DateTime? putaway_Date { get; set; }

        public Guid? suggest_Location_Index { get; set; }

        public string invoice_No { get; set; }

        

    }
}
