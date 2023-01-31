using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    public partial class SearchGRModel : Pagination
    {
        public SearchGRModel()
        {
            sort = new List<sortViewModel>();

            status = new List<statusViewModel>();

        }
        public Guid? goodsReceive_Index { get; set; }

        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }
        public string goodsReceive_Date_To { get; set; }

        
        public Guid? documentType_Index { get; set; }

        public string documentType_Id { get; set; }

        public string documentType_Name { get; set; }
        public Guid? owner_Index { get; set; }

        public string owner_Id { get; set; }

        public string owner_Name { get; set; }

        public Guid? vendor_Index { get; set; }

        public string vendor_Id { get; set; }

        public string vendor_Name { get; set; }

        public int? putaway_Status { get; set; }

        public string documentRef_No2 { get; set; }

        public string create_By { get; set; }

        public string create_Date { get; set; }

        public string ref_Plan_No { get; set; }

        public int? document_Status { get; set; }

        public string processStatus_Name { get; set; }


        public string column_Name { get; set; }

        public string order_by { get; set; }

        public string key { get; set; }

        public bool advanceSearch { get; set; }

        public string name { get; set; }
        public string qty { get; set; }
        public string tag_No { get; set; }
        public Guid? product_Index { get; set; }
        public string statusPutaway { get; set; }

        public Guid? location_Index { get; set; }

        public string plan_no { get; set; }



        public List<sortViewModel> sort { get; set; }
        public List<statusViewModel> status { get; set; }

        public class actionResultGRViewModel
        {
            public IList<SearchGRModel> itemsGR { get; set; }
            public Pagination pagination { get; set; }
        }

        public class sortViewModel
        {
            public string value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class statusViewModel
        {
            public int value { get; set; }
            public string display { get; set; }
            public int seq { get; set; }
        }

        public class SortModel
        {
            public string ColId { get; set; }
            public string Sort { get; set; }

            public string PairAsSqlExpression
            {
                get
                {
                    return $"{ColId} {Sort}";
                }
            }
        }

        public class StatusModel
        {
            public string Name { get; set; }
        }
    }
}
