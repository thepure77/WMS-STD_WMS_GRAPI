using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRBusiness.GoodsReceive
{
    public class GRRequestViewModel
    {
        public GRRequestViewModel()
        {
            Detail = new List<GRRequestDetail>();
        }
        public string PstngDate { get; set; }
        public string DocDate { get; set; }
        public string RefDocNo { get; set; }
        public string GrNo { get; set; }
        public string HeaderTxt { get; set; }
        public string GmCode { get; set; }
        public List<GRRequestDetail> Detail { get; set; }

    }

    public class GRRequestDetail
    {
        public string Material { get; set; }
        public string Plant { get; set; }
        public string StgeLoc { get; set; }
        public string Batch { get; set; }
        public string MoveType { get; set; }
        public string StckType { get; set; }
        public string Vendor { get; set; }
        public decimal EntryQnt { get; set; }
        public string EntryUom { get; set; }
        public string PoNumber { get; set; }
        public string PoItem { get; set; }
        public string NoMoreGR { get; set; }
        public string ItemText { get; set; }

    }
}
