
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class ViewModel_updateMatdoc :Result
    {
        public ViewModel_updateMatdoc()
        {
            list = new List<ViewModel_updateMatdoc>();
        }

        public Guid? RowIndex { get; set; }

        public string Product_Id { get; set; }

        public string Product_Lot { get; set; }

        public string Remark { get; set; }

        public int? CountMD { get; set; }

        public int? CountRobotReceive { get; set; }

        public int? CountTAG_Putaway { get; set; }

        public decimal? CountCalSAP { get; set; }

        public decimal? IN_QTY_Putaway { get; set; }

        public decimal? IN_QTY_SAP { get; set; }

        public decimal? IN_QTY_PartialPallet { get; set; }

        public decimal? IN_Ratio { get; set; }

        public DateTime? Create_Date { get; set; }

        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }

        public Guid? Ref_DocumentItem_Index { get; set; }

        public int? Is_Amz { get; set; }


        public List<ViewModel_updateMatdoc> list { get; set; }

    }
}
