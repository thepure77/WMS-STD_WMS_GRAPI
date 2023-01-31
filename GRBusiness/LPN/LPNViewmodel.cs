using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.LPN
{
    public class LPNViewModel
    {
        [Key]
        public Guid tag_Index { get; set; }

        public string tag_No { get; set; }

        public string Pallet_No { get; set; }

        public Guid? Pallet_Index { get; set; }

        public string tagRef_No1 { get; set; }

        public string tagRef_No2 { get; set; }

        public string tagRef_No3 { get; set; }

        public string tagRef_No4 { get; set; }

        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }

        public int? printNumber { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }

        public string column_Name { get; set; }

        public string order_by { get; set; }

        public string key { get; set; }

        public bool advanceSearch { get; set; }

        public class actionResultLPNViewModel
        {
            public IList<LPNViewModel> itemsLPN { get; set; }
            public Pagination pagination { get; set; }
        }
    }
}
