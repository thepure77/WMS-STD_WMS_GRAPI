
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class GoodsReceiveItemModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid goodsReceiveIndex { get; set; }

        [Key]
        [Column(Order = 1)]

        public string product_Index { get; set; }
        public string ProductId { get; set; }
        public string ProductConversionIndex { get; set; }
        public string ProductBarcode { get; set; }
    }
}
