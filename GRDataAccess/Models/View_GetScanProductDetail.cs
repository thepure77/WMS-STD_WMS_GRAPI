using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_GetScanProductDetail
    {
        [Key]
        public Guid GoodsReceive_Index { get; set; }
        public Guid Product_Index { get; set; }

        public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Product_SecondName { get; set; }

        public string Product_ThirdName { get; set; }

        public Guid ProductConversion_Index { get; set; }

        public string ProductConversion_Id { get; set; }

        public string ProductConversion_Name { get; set; }

        public decimal ProductConversion_Ratio { get; set; }

        public Guid Owner_Index { get; set; }

        public string Owner_Id { get; set; }

        public string Owner_Name { get; set; }

        public Guid ProductType_Index { get; set; }

        public Guid ProductSubType_Index { get; set; }

        public Guid ProductCategory_Index { get; set; }

        public int? IsExpDate { get; set; }

        public int? IsLot { get; set; }

        public int? ProductItemLife_Y { get; set; }

        public int? ProductItemLife_M { get; set; }

        public int? ProductItemLife_D { get; set; }

        public string BaseProductConversion { get; set; }

        public int? IsMfgDate { get; set; }

        public int? IsCatchWeight { get; set; }
    }
}
