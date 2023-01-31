using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{

    public partial class View_PreviewTaskGRViewModel
    {
        public long? rowIndex { get; set; }

        public Guid? taskGR_Index { get; set; }
        public Guid? taskGRItem_Index { get; set; }

        public string taskGR_No { get; set; }

        public Guid? ref_Document_Index { get; set; }
        public string ref_Document_No { get; set; }

        public string tag_No { get; set; }
        public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string create_Time { get; set; }

        public string update_By { get; set; }
        public Guid? product_Index { get; set; }
        public string product_Id { get; set; }
        public string product_Name { get; set; }
        public string product_SecondName { get; set; }
        public Guid? productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }
        public string qty { get; set; }
        public string assign_By { get; set; }


    }
}
