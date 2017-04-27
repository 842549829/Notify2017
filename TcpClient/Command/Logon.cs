using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpClient.Model;

namespace TcpClient.Command
{
    class Logon : RequestProcessor<LogonInfo>
    {
        public Logon(string host, int port, string userName, string password)
            : base(host, port, Guid.Empty)
        {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName
        {
            get;
            private set;
        }
        public string Password
        {
            get;
            private set;
        }

        protected override bool KeepConnection
        {
            get
            {
                return true;
            }
        }

        protected override string Command
        {
            get { return "LOGON"; }
        }

        protected override string PrepareRequestContent()
        {
            return string.Format("{0}/{1}", this.UserName, this.Password);
        }

        protected override LogonInfo ParseResponseCore(string content)
        {
            return new Model.LogonInfo(this.UserName, content, this.Connection);
        }
    }
}
