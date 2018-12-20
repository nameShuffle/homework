using System;
using System.IO;
using System.Net.Sockets;

namespace ClientFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 5555;

            var client = new Client();

            client.Work(port);
        }
    }
}
