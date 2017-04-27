using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new Command.Logon("192.168.10.41", 9990, "001", "0024511");
            var response = request.Execute();
            if (response.Success)
            {
                Console.WriteLine("登录成功");
                RemindInfoListener.Instance.Start(response.Response.Connection);
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
