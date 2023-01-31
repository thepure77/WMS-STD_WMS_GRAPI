
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class GoodsReceiveTagModel
    {
        [Key]
        [Column(Order = 0)]
        public Guid GoodsReceive_Index { get; set; }


        public string Tag_No { get; set; }
    }
}
