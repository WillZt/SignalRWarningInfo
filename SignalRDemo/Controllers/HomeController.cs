using SignalRDemo.Models;
using SignalRDemo.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SignalRDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdataWarningInfo(int id)
        {
            JsonObj json = new JsonObj { Status = 1, Error = String.Empty};
            //List<WarningInfoModel> warningInfos = new List<WarningInfoModel>();
            try
            {
                Thread childThread = new Thread(childref);
                childThread.Start();

            }
            catch(Exception e)
            {
                json.Status = 0;
                json.Error = e.ToString();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        void childref()
        {
            Thread.Sleep(1000);
            try
            {
                List<WarningInfoModel> warningInfos = new List<WarningInfoModel>();
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
                            warningInfos.Add(warningInfo);
                        }

                        WarningInfoCollector.Instance.UpdateWarningInfo(warningInfos);
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}