using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class listTaskViewModel
    {
        public Guid? taskGR_Index { get; set; }

        public string update_By { get; set; }

        public Guid? goodsReceive_Index { get; set; }

        public string document_Status { get; set; }
        public string goodsReceive_No { get; set; }
        public string userAssign { get; set; }


    }
}
