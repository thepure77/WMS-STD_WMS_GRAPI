
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class GoodsReceiveConfirmViewModel
    {
        [Key]
        public Guid? goodsReceive_Index { get; set; }

        public Guid owner_Index { get; set; }

        public string Create_By { get; set; }

    }
}
