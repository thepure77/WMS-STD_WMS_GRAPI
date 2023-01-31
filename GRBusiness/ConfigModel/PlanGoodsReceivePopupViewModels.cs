using GRBusiness;
using GRBusiness.PlanGoodsReceive;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGRBusiness.ViewModels
{
    public class PlanGoodsReceivePopupViewModels : Pagination
    {

        public Guid planGoodsReceive_Index { get; set; }

        public Guid planGoodsReceiveItem_Index { get; set; }

        public Guid owner_Index { get; set; }



        public string owner_Id { get; set; }



        public string owner_Name { get; set; }

        public Guid vendor_Index { get; set; }



        public string vendor_Id { get; set; }



        public string vendor_Name { get; set; }

        public Guid documentType_Index { get; set; }



        public string documentType_Id { get; set; }



        public string documentType_Name { get; set; }



        public string planGoodsReceive_No { get; set; }


        public string planGoodsReceive_Date { get; set; }
        public string planGoodsReceive_Due_Date { get; set; }



        public string documentRef_No1 { get; set; }


        public string documentRef_No2 { get; set; }


        public string documentRef_No3 { get; set; }


        public string documentRef_No4 { get; set; }


        public string documentRef_No5 { get; set; }

        public int? document_Status { get; set; }


        public string udf1 { get; set; }


        public string udf2 { get; set; }


        public string udf3 { get; set; }


        public string udf4 { get; set; }


        public string udf5 { get; set; }

        public int? documentPriority_Status { get; set; }


        public string document_Remark { get; set; }


        public string create_By { get; set; }


        public string create_Date { get; set; }


        public string update_By { get; set; }


        public string update_Date { get; set; }


        public string cancel_By { get; set; }


        public string cancel_Date { get; set; }

        public Guid? warehouse_Index { get; set; }


        public string warehouse_Id { get; set; }


        public string warehouse_Name { get; set; }

        public Guid? warehouse_Index_To { get; set; }


        public string warehouse_Id_To { get; set; }


        public string warehouse_Name_To { get; set; }

        public string chk { get; set; }
        public int count { get; set; }

        public Guid grDocumentType_Index { get; set; }



        public string grDocumentType_Id { get; set; }



        public string grDocumentType_Name { get; set; }


        public string key { get; set; }

   

        public class actionResultPlanGRViewModels
        {
            public IList<PlanGoodsReceivePopupViewModels> itemsPlanGR { get; set; }
            public Pagination pagination { get; set; }
        }

        public List<PlanGoodsReceiveItemViewModel> listPlanGoodsReceiveItemViewModel { get; set; }
    }
}
