using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.ConfigModel
{
    public class View_LocationZoneputaway
    {
        public Guid locationZoneputaway_Index { get; set; }
        public string locationZoneputaway_Id { get; set; }

        public Guid zoneputaway_Index { get; set; }
        public string zoneputaway_Id { get; set; }
        public string zoneputaway_Name { get; set; }

        public Guid? location_Index { get; set; }
        public string location_Id { get; set; }
        public string location_Name { get; set; }

        public int PutAway_Seq { get; set; }

        public int? isActive { get; set; }
        public int? isDelete { get; set; }

    }
}
