
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class ProductBarcodeModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid owner_Index { get; set; }

        [Key]
        [Column(Order = 1)]

        public string productBarcode { get; set; }
        public string product_Id { get; set; }
        public Guid planGoodsReceive_Index { get; set; }
        public string planGoodsReceive_No { get; set; }
        public bool chkInternal { get; set; }

        public Guid product_Index { get; set; }
    }
}
