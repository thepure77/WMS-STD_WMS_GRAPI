
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class ProductDetailModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid Owner_Index { get; set; }

        [Key]
        [Column(Order = 1)]

        public string Product_Index { get; set; }
    }
}
