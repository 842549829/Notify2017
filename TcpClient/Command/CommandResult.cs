using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient.Command
{
    class CommandResult<TResponse>
    {
        public bool Success
        {
            get;
            internal set;
        }

        public string ErrorMessage
        {
            get;
            internal set;
        }

        public TResponse Response
        {
            get;
            internal set;
        }
    }
}
