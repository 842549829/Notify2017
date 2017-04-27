using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notify.Code.Extension;
using Notify.Code.Net;
using TcpService.DataSource;
using TcpService.Model;

namespace TcpService.Command
{
    internal class Remind
    {
        TcpProcessor m_processor = null;
        IEnumerable<RemindInfo> m_datas = null;
        Encoding m_encoding = Encoding.GetEncoding("gb2312");

        public Remind(TcpProcessor processor, IEnumerable<RemindInfo> remindDatas)
        {
            this.m_processor = processor;
            this.m_datas = remindDatas;
        }

        public string Execute()
        {
            if (this.m_datas != null)
            {
                var dataString = this.m_datas.Join("|", item => string.Format("{0}-{1}", item.CustomNO, item.Id));
                var data = this.m_encoding.GetBytes("REMIND/" + dataString);
                this.m_processor.AsyncSend(data, Callback, this.m_datas.First().Id);
                return dataString;
            }
            return string.Empty;
        }

        public void Callback(object state)
        {
            string id = (string)state;
            DataCenter.Instance.RemoveData(id);
        }
    }
}