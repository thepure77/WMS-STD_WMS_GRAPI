using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.Reports.Tag
{
    public class TagA5Model
    {
        public string goodsReceive_Date { get; set; }
        public string planGoodsReceive_Date { get; set; }
        public string documentRef_No1 { get; set; }
        public string documentRef_No2 { get; set; }
        public string owner_Name { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public decimal qty { get; set; }
        public string productConversion_Name { get; set; }
        public string tag_No { get; set; }
        public string product_Bacode { get; set; }
    }
}
