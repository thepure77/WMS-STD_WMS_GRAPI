using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodIssue
{

    public partial class AssignJobViewModel
    {
        public string Template { get; set; }

        public string Create_By { get; set; }

        public List<listGoodsReceiveViewModel> listGoodsReceiveViewModel { get; set; }

    }

    public class View_AssignJobViewModel
    {

        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public Guid? goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public Guid? goodsReceiveItem_Index { get; set; }
        public Guid? warehouse_Index { get; set; }
        public Guid? tag_Index { get; set; }
        public Guid? tagItem_Index { get; set; }

        public Guid? zone_Index { get; set; }
        public Guid? route_Index { get; set; }
        public string product_Id { get; set; }
        public DateTime? goodsReceive_Date { get; set; }

        public decimal? qty { get; set; }

        public decimal? totalQty { get; set; }

        public List<View_AssignJobViewModel> ResultItem { get; set; }


    }
}
