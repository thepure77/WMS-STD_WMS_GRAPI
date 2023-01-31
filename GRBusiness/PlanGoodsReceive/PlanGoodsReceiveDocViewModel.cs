using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    public class PlanGoodsReceiveDocViewModel 
    {
        [Key]
        public Guid PlanGoodsReceiveIndex { get; set; }

        public Guid PlanGoodsReceiveItemIndex { get; set; }

        public Guid OwnerIndex { get; set; }



        public string OwnerId { get; set; }



        public string OwnerName { get; set; }

        public Guid VendorIndex { get; set; }



        public string VendorId { get; set; }



        public string VendorName { get; set; }

        public Guid DocumentTypeIndex { get; set; }



        public string DocumentTypeId { get; set; }



        public string DocumentTypeName { get; set; }



        public string PlanGoodsReceiveNo { get; set; }

        
        public string PlanGoodsReceiveDate { get; set; }        
        public string PlanGoodsReceiveDueDate { get; set; }
        


        public string DocumentRefNo1 { get; set; }


        public string DocumentRefNo2 { get; set; }


        public string DocumentRefNo3 { get; set; }


        public string DocumentRefNo4 { get; set; }


        public string DocumentRefNo5 { get; set; }

        public int? DocumentStatus { get; set; }


        public string UDF1 { get; set; }


        public string UDF2 { get; set; }


        public string UDF3 { get; set; }


        public string UDF4 { get; set; }


        public string UDF5 { get; set; }

        public int? DocumentPriorityStatus { get; set; }


        public string DocumentRemark { get; set; }


        public string Create_By { get; set; }

       
        public string Create_Date { get; set; }


        public string Update_By { get; set; }

        
        public string Update_Date { get; set; }


        public string Cancel_By { get; set; }

        
        public string Cancel_Date { get; set; }

        public Guid? WarehouseIndex { get; set; }


        public string WarehouseId { get; set; }


        public string WarehouseName { get; set; }

        public Guid? WarehouseIndexTo { get; set; }


        public string WarehouseIdTo { get; set; }


        public string WarehouseNameTo { get; set; }

        public int count { get; set; }

        public string ProcessStatusName { get; set; }

        public string UserAssign { get; set; }

        public string Item_Document_Remark { get; set; }

        public List<PlanGoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }

      
    }
}
