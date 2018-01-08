using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDemo.Models;

namespace SignalRDemo.Tools
{
    [HubName("warningInfoHub")]
    public class WarningInfoHub : Hub
    {
        private readonly WarningInfoCollector _warningInfoCollector;

        public WarningInfoHub() : this(WarningInfoCollector.Instance) { }

        public WarningInfoHub(WarningInfoCollector warningInfoCollector)
        {
            _warningInfoCollector = warningInfoCollector;
        }

        public IEnumerable<WarningInfoModel> GetAllWarningInfo()
        {
            return _warningInfoCollector.GetAllWarningInfo();
        }
    }
}