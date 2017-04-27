using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient.Model
{
    class RemindRecord
    {
        public RemindRecord(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
        public string Name { get; private set; }
        public int Count { get; private set; }
    }
}
