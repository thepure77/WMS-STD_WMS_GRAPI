using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.Reports
{
    public class ReportPrintOutGRViewModel
    {
        public int rowCount { get; set; }
        public Guid? goodsReceive_Index { get; set; }
        public string goodsReceive_No { get; set; }
        public Guid? documentType_Index { get; set; }
        public string documentType_Id { get; set; }
        public string documentType_Name { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public Guid? owner_Index { get; set; }
        public string owner_Id { get; set; }
        public string owner_Name { get; set; }
        public Guid? warehouse_Index { get; set; }
        public string warehouse_Id { get; set; }
        public string warehouse_Name { get; set; }
        public string productConversion_Name { get; set; }
        public string lineNum { get; set; }
        public decimal? qty { get; set; }
        public string goodsReceive_Date { get; set; }

        public string date_Now { get; set; }
        public string documentRef_No2 { get; set; }
        public string document_Remark { get; set; }
        public string planGoodsReceive_No { get; set; }
        public string planGoodsReceive_Date { get; set; }
        public string goodsReceiveNo_Barcode { get; set; }
  
        public string ref_No2 { get; set; }

        public bool checkQuery { get; set; }
        public string msgDN { get; set; }
        public string msgDate { get; set; }
        public string vehicle_no { get; set; }
        public string dock_name { get; set; }
        public string batch { get; set; }
        public decimal? total { get; set; }
        public string conversion { get; set; }
        public string weight { get; set; }
        public string status_item { get; set; }
        public string ref_po { get; set; }
        public string Appointment_id { get; set; }
        public string Appointment_BarCode { get; set; }
        public string erp_location  { get; set; }
    }


}
