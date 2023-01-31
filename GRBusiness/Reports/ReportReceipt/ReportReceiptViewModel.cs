using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.Reports
{
    public class ReportReceiptViewModel
    {
        //header
        public string goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public string goodsReceive_Date { get; set; }
        public string goodsReceive_Due_Date { get; set; }
        public string documentRef_No3 { get; set; }
        public string planGoodsReceive_No { get; set; }
        public string documentRef_No1 { get; set; }
        public string warehouse_Code { get; set; }
        public string sloc_Code { get; set; }
        public string pgr_DocumentRef_No2 { get; set; }
        public string gr_DocumentRef_No2 { get; set; }
        public string vendor_Id { get; set; }
        public string vendor_Name { get; set; }
        public string documentRef_No4 { get; set; }
        public string documentType_Id { get; set; }

        //detail
        public string product_Id { get; set; }
        public string productConversion_Name { get; set; }
        public string product_Name { get; set; }
        public decimal? qty { get; set; }
        public string documentRef_No2 { get; set; }

        //footer
        public bool isRecipent { get; set; }
        public string recipent_Name { get; set; }
        public string recipent_pos_Name { get; set; }
        public bool isRecorder { get; set; }
        public string recorder_Name { get; set; }
        public string recorder_pos_Name { get; set; }

        //log
        public Guid? documentType_Index { get; set; }
        public string documentType_Name { get; set; }

        public string user { get; set; }
        public string recipent_first_Name { get; set; }
        public string recipent_last_Name { get; set; }
        public string recipent_user_Index { get; set; }
        public string recipent_user_Id { get; set; }
        public string recipent_user_Name { get; set; }
        public string recipent_position_Code { get; set; }

        public string recorder_first_Name { get; set; }
        public string recorder_last_Name { get; set; }
        public string recorder_user_Index { get; set; }
        public string recorder_user_Id { get; set; }
        public string recorder_user_Name { get; set; }
        public string recorder_position_Code { get; set; }


    }
}
