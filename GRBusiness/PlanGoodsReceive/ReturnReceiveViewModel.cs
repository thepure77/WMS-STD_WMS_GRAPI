using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public class ReturnReceiveViewModel : Pagination
    {
        [Key]
        public Guid PlanGoodsIssueIndex { get; set; }

        public Guid OwnerIndex { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }


        public Guid? DocumentTypeIndex { get; set; }

        public string DocumentTypeId { get; set; }


        public string DocumentTypeName { get; set; }


        public string PlanGoodsIssueNo { get; set; }

        public string PlanGoodsIssueDate { get; set; }

        public string PlanGoodsIssueDueDate { get; set; }

        public string DocumentRefNo1 { get; set; }
        public string DocumentRemark { get; set; }

        public int? DocumentStatus { get; set; }

        public int? DocumentPriorityStatus { get; set; }

        public Guid? WarehouseIndex { get; set; }

        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }

        public string WarehouseIdTo { get; set; }

        public string WarehouseNameTo { get; set; }

        public string CreateBy { get; set; }

        public string CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public string UpdateDate { get; set; }

        public string CancelBy { get; set; }

        public string CancelDate { get; set; }

        public string RefPlanGoodsIssueNo { get; set; }

        public class actionResultPlanGIPopupViewModel
        {
            public IList<ReturnReceiveViewModel> itemsPlanGI { get; set; }
            public Pagination pagination { get; set; }
        }
    }
}
