using RollerTest.Domain.Abstract;
using RollerTest.WebUI.Models.WTTESTMODEL;
using RollerTest.WebUI.Tools;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Models
{
    public class SettingViewModel
    {
        private IBaseRepository baserepo;
        private IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo;
        private IReadRepository<WTSAMPLEINFO> wtsampleinfo;
        public SettingViewModel(IBaseRepository baserepo,IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo, IReadRepository<WTSAMPLEINFO> wtsampleinfo)
        {
            this.baserepo = baserepo;
            this.wtequipmentrepo = wtequipmentrepo;
            this.wtsampleinfo = wtsampleinfo;
        }

        //public IEnumerable<SelectListItem> GetDeviceList()
        //{
        //    var selectList = baserepo.RollerBaseStations.Distinct(a => a.Device).Select(a => new SelectListItem
        //    {
        //        Text = a.Device,
        //        Value = a.Device.ToString()
        //    });

        //    return selectList;
        //}
        public IEnumerable<SelectListItem> GetStationList(string device)
        {

            var selectList = baserepo.RollerBaseStations.Where(a=>a.Device==device&&a.RollerSampleInfo.Where(x=>!x.State.Equals("结束")).Count()==0).Select(a => new SelectListItem
            {
                Text = a.Station,
                Value = a.RollerBaseStationID.ToString()
            });

            return selectList;
        }
        public IEnumerable<SelectListItem> GetSampleIDList(int TestID)
        {

            var selectList = wtsampleinfo.QueryEntities.Where(a => a.TestInfos.Where(x => x.ID == TestID).Count() != 0).Select(a => new SelectListItem {
                Text = a.SERIAL_NO,
                Value=a.SERIAL_NO
            });

            return selectList;
        }
        public IEnumerable<SelectListItem> GetStationList()
        {

            var selectList = baserepo.RollerBaseStations.Where(a=>a.RollerSampleInfo.Where(x => !x.State.Equals("结束")).Count() == 0).Select(a => new SelectListItem
            {
                Text = a.Station,
                Value = a.RollerBaseStationID.ToString()
            });

            return selectList;
        }
        public List<SelectListItem> GetTestTypeList()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "定时截尾试验",
                Value = "定时截尾试验"
            });
            list.Add(new SelectListItem()
            {
                Text = "寿命试验",
                Value = "寿命试验"
            });

            return list;
        }
        public IEnumerable<SelectListItem> GetLISDeviceList()
        {
            var selectlist = wtequipmentrepo.QueryEntities.Select(x => new SelectListItem
            {
                Text = '['+x.MNGNUM+']'+' '+x.NAME,
                Value=x.MNGNUM
            });
            return selectlist;
        }

    }
}