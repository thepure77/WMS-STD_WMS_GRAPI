using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GRBusiness.PlanGoodsReceive
{
    [DataContract]
    [Serializable]
    public class CloseDocumentViewModel
    {
        [DataMember]
        public string[] id { get; set; }

        [DataMember]
        public string[] username { get; set; }
    }
}
