
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class CheckReceiveQtyViewModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid planGoodsReceive_Index { get; set; }

        public Guid planGoodsReceiveItem_Index { get; set; }



        public Guid product_Index { get; set; }


        public decimal qty { get; set; }


        public decimal ratio { get; set; }

        public decimal productConversion_Ratio { get; set; }

        public Guid productConversion_Index { get; set; }
    }
}
