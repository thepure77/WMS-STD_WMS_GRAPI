using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness.Reports
{
    public class ReportTagPutawayViewModel
    {
        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string warehouse_Id { get; set; }

        public string location_Id { get; set; }

        public string suggest_Location_Name { get; set; }

        public string date_Print { get; set; }

        public string planGoodsReceive_No { get; set; }

        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

        public string owner_Name { get; set; }

        public string productConversion_Name { get; set; }

        public string ref_no1 { get; set; }

        public string ref_no2 { get; set; }

        public string tag_No { get; set; }

        public string tag_NoBarcode { get; set; }

        public decimal? qty { get; set; }

        public bool checkQuery { get; set; }

        public string ref_no3 { get; set; }

        public string shortNameUnit { get; set; }

        // Add new model same Production Setup WCS
        public string productionLineNo { get; set; }
        public string palletID { get; set; }
        public string sku { get; set; }
        public string skuBarcode { get; set; }
        public string isLastCarton { get; set; }
        public string description { get; set; }
        public string mainType { get; set; }
        public int quantityInCRT { get; set; }
        public int quantityInPC { get; set; }
        public string mfgDate { get; set; }
        public string expDate { get; set; }
        public string lotNo { get; set; }
        public string refDoc { get; set; }
        public int cartonPerPallet { get; set; }
        public int ti { get; set; }
        public int hi { get; set; }
        public int valTiHi { get; set; }
        public string receiverDate { get; set; }
        public string receiver { get; set; }
        public string status { get; set; }
        public string sapCreateDT { get; set; }
        public string productionEndDT { get; set; }
        public int saleQty { get; set; }
        public string saleUnit { get; set; }
        public string giBeforeDate { get; set; }
        public string batchNo { get; set; }
        public string type { get; set; }
        public int unitOnPallet { get; set; }
        public decimal palletWT { get; set; }
        public string bu { get; set; }
        public string supplier { get; set; }
        public string remark { get; set; }
        public int qtyInUnit { get; set; }
        public string unitOfInUnit { get; set; }
        public int qtyPOUnit { get; set; }
        public string unitOfPOUnit { get; set; }

    }


}
