using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class PlanGoodsReceiveAutoViewModel
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


        public string uDF1 { get; set; }


        public string uDF2 { get; set; }


        public string uDF3 { get; set; }


        public string uDF4 { get; set; }


        public string uDF5 { get; set; }

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

        public Guid? warehouse_IndexTo { get; set; }


        public string warehouse_IdTo { get; set; }


        public string warehouse_NameTo { get; set; }

        public string chk { get; set; }
        public int count { get; set; }

        public Guid grDocumentTypeIndex { get; set; }



        public string grDocumentTypeId { get; set; }



        public string grDocumentTypeName { get; set; }

        public string key { get; set; }

        public string name { get; set; }
    }
}
