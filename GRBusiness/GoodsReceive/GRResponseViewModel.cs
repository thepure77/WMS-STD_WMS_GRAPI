using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRBusiness.GoodsReceive
{
    public class GRResponseViewModel
    {
        public string status { get; set; }
        //public string message { get; set; }
        public GRMessage message { get; set; }

    }

    public class GRMessage
    {
        public string eFiDocumentField { get; set; }
        public string eMaterailDocField { get; set; }
        public string eMessageField { get; set; }
    }

}