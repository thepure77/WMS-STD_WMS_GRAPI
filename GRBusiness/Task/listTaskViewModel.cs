using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class View_GRTagViewModel
    {
        public Guid goodsReceive_Index { get; set; }


        public Guid goodsReceiveItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public Guid? tagItem_Index { get; set; }


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

        public Guid? suggest_Location_Index { get; set; }


        public string suggest_Location_Id { get; set; }


        public string suggest_Location_Name { get; set; }


        public decimal? qty { get; set; }


        public decimal? ratio { get; set; }


        public decimal? totalQty { get; set; }

        public Guid? productConversion_Index { get; set; }


        public string productConversion_Id { get; set; }


        public string productConversion_Name { get; set; }


        public DateTime? mFG_Date { get; set; }


        public DateTime? eXP_Date { get; set; }


        public decimal? unitWeight { get; set; }


        public decimal weight { get; set; }

        public decimal unitWidth { get; set; }

        public decimal unitLength { get; set; }


        public decimal unitHeight { get; set; }

        public decimal unitVolume { get; set; }

        public decimal volume { get; set; }

        public decimal unitPrice { get; set; }

        public decimal price { get; set; }


        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }


        public string uDF_1 { get; set; }


        public string uDF_2 { get; set; }


        public string uDF_3 { get; set; }


        public string uDF_4 { get; set; }


        public string uDF_5 { get; set; }


        public string lineNum { get; set; }


    }
}
