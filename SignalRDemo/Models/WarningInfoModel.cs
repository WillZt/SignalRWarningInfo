using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Models
{
    public class WarningInfoModel
    {
        public long MessageId { get; set; }

        public int PipeId { get; set; }

        public int AreaId { get; set; }

        public int WarningLevel { get; set; }

        public float WarningValue { get; set; }

        public DateTime WarningTime { get; set; }

        public int WarningStatus { get; set; }
    }
}