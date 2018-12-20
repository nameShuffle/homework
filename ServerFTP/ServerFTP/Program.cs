using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerFTP
{
    class Program
    {
        public static void Main(string[] args)
        {
            const int port = 5555;

            var server = new Server();

            server.Work(port);
        }
    }
}
