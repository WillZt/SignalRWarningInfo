using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDemo.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRDemo.Models
{
    public class WarningInfoCollector
    {
        private readonly static Lazy<WarningInfoCollector> _instance = new Lazy<WarningInfoCollector>(() => new WarningInfoCollector(GlobalHost.ConnectionManager.GetHubContext<WarningInfoHub>().Clients));

        public static WarningInfoCollector Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly List<WarningInfoModel> _warningInfo = new List<WarningInfoModel>();

        private readonly object _updateWarningInfoLock = new object();

        private volatile bool _updateWarningInfo = false;

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        private WarningInfoCollector(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            _warningInfo.Clear();
        }

        public void UpdateWarningInfo(IEnumerable<WarningInfoModel> infos)
        {
            lock (_updateWarningInfoLock)
            {
                if (!_updateWarningInfo)
                {
                    _updateWarningInfo = true;
                    BroadcastWarningInfo(infos);
                    _updateWarningInfo = false;
                }
            }
        }

        public IEnumerable<WarningInfoModel> GetAllWarningInfo()
        {
            var warningInfoList = new List<WarningInfoModel>();
            _warningInfo.Clear();
            try
            {
                using (var cx = new WarningInfoEntities())
                {
                    var infos = cx.tb_pipe_warning_info.Where(c => c.warning_status == 0).OrderByDescending(c => c.warning_time).ToList();
                    if (infos.Count > 0)
                    {
                        foreach (var item in infos)
                        {
                            WarningInfoModel warningInfo = new WarningInfoModel
                            {
                                MessageId = item.id,
                                PipeId = item.pipe_id,
                                AreaId = item.area_id,
                                WarningLevel = item.warning_level,
                                WarningValue = item.warning_value,
                                WarningTime = item.warning_time
                            };
                            _warningInfo.Add(warningInfo);
                        }
                        //infos.ForEach(item => _warningInfo.Add(item));
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
            return _warningInfo;
        }

        private void BroadcastWarningInfo(IEnumerable<WarningInfoModel> warningInfos)
        {
            Clients.All.updateWarningInfo(warningInfos);
        }
    }
}