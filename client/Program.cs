
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync("127.0.0.1", 8888);
            var strm = tcpClient.GetStream();

            while (true)
            {
                byte[] data = new byte[1024];
                var message = Console.ReadLine();

                byte[] requestData = Encoding.UTF8.GetBytes(message);
                strm.Write(requestData, 0, requestData.Length);
                Console.WriteLine("Сообщение отправлено");

                int bytes = strm.Read(data, 0, data.Length);
                var result = Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine(result);
            }
        }
    }
}
