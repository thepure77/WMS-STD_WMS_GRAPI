using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public class CheckDocumentStatusViewModel
    {
        public Guid? GoodsReceiveIndex { get; set; }
        public string PlanGoodsReceiveNo { get; set; }
        public int PlanGRDocumentStatus { get; set; }
        public int GRDocumentStatus { get; set; }

        public List<string> GoodsReceiveNoList { get; set; }
    }
}
