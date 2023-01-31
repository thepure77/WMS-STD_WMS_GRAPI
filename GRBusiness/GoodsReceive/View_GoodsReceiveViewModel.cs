
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class View_GoodsReceiveViewModel
    {

        [Key]
        public Guid goodsReceive_Index { get; set; }

        public Guid goodsReceiveItem_Index { get; set; }

        public Guid goodsReceiveItemLocation_Index { get; set; }
        public string goodsReceive_No { get; set; }

        public DateTime? goodsReceive_Date { get; set; }

        public Guid owner_Index { get; set; }

        public string owner_Id { get; set; }

        public string owner_Name { get; set; }

        public Guid? documentType_Index { get; set; }

        public string documentType_Id { get; set; }


        public string documentType_Name { get; set; }

        public Guid? product_Index { get; set; }


        public string product_Id { get; set; }


        public string product_Name { get; set; }


        public string product_Lot { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }
        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public decimal qty { get; set; }

        public decimal ratio { get; set; }

        public decimal totalQty { get; set; }

        public decimal volume { get; set; }

        public decimal weight { get; set; }

        public Guid itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public DateTime? mFG_Date { get; set; }

        public DateTime? eXP_Date { get; set; }

        public Guid? location_Index { get; set; }


        public string location_Id { get; set; }


        public string location_Name { get; set; }
        public Guid? tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

    }
}
