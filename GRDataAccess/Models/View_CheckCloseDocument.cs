using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class View_CheckCloseDocument
    {
        //[Key]
        public Guid? GoodsReceive_Index { get; set; }

        public Guid? GoodsReceiveItem_Index { get; set; }

        public Guid? Product_Index { get; set; }


        public decimal? Qty { get; set; }


        public decimal? Ratio { get; set; }

        public Guid? ProductConversion_Index { get; set; }
        [Key]
        public Guid? PlanGoodsReceive_Index { get; set; }

    }
}
