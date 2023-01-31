using System;
using System.Collections.Generic;
using System.Text;

namespace GRBusiness
{
    public class Result
    {
        public bool resultIsUse { get; set; }

        public string resultMsg { get; set; }

        public List<PTLPickingModel> models { get; set; }

        public class PTLPickingModel
        {
            public string product_Id { get; set; }
            public string product_Name { get; set; }
            public decimal? qty { get; set; }
            public string productConversion_Name { get; set; }
            public string status { get; set; }
        }
    }
}
