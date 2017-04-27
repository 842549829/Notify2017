using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient.Model
{
    class LogonInfo
    {
        public LogonInfo(string userName, string id, System.Net.Sockets.TcpClient connection)
        {
            this.UserName = userName;
            this.Id = id;
            this.Connection = connection;
        }
        public string UserName { get; private set; }
        public string Id { get; private set; }
        public System.Net.Sockets.TcpClient Connection { get; private set; }
    }
}
