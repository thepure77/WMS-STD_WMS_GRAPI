using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class View_ProductCategoryViewModel
    {
        public Guid productCategory_Index { get; set; }
        public string productCategory_Id { get; set; }
        public string productCategory_Name { get; set; }
        public int productCategory_IsActive { get; set; }
        public int productCategory_IsDelete { get; set; }
        public Guid product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public string product_ThirdName { get; set; }
        public Guid productConversion_Index { get; set; }
        public string productConversion_Id { get; set; }
        public string productConversion_Name { get; set; }
        public Guid productType_Index { get; set; }
        public Guid productSubType_Index { get; set; }
        public int? productItemLife_Y { get; set; }
        public int? productItemLife_M { get; set; }
        public int? productItemLife_D { get; set; }
        public string productImage_Path { get; set; }
        public int? isLot { get; set; }
        public int? isExpDate { get; set; }
        public int? isMfgDate { get; set; }
        public int? isCatchWeight { get; set; }
        public int? isPack { get; set; }
        public int? isSerial { get; set; }
        public int product_IsActive { get; set; }
        public int product_IsDelete { get; set; }

    }
}
