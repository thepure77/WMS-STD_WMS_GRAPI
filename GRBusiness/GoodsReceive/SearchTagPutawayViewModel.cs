using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class SearchTagPutawayViewModel
    {
        public Guid? goodsReceive_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

        public Guid? product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string location_Name { get; set; }
        public string LocationType_Name { get; set; }

        public string itemStatus_Name { get; set; }

        public int? putaway_Status { get; set; }

        public string putaway_Date { get; set; }
        public string statusPutaway { get; set; }
        public string putaway_By { get; set; }
        public string putaway_location { get; set; }
        public string WaitTostore_date { get; set; }
        public string WaitTostore_location { get; set; }
        public string Docktostaging_date { get; set; }
        public string Docktostaging_location { get; set; }
        public string Pallet_Inspection_date { get; set; }
        public string Pallet_Inspection_location { get; set; }



    }
}
