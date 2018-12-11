using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        class Server
        {

            public void Work(int port)
            {
                var listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                while (true)
                {
                    var socket = listener.AcceptSocket();

                    Console.WriteLine("Слушаю");

                    var manager = new Task(clientSocket => ManageRequest((Socket)clientSocket), socket);

                    manager.Start();
                }
            }       

            public void ManageRequest(Socket socket)
            {
                var stream = new NetworkStream(socket);

                var reader = new StreamReader(stream);

                Console.WriteLine("Хочу прочитать");

                var request = reader.ReadLine();

                Console.WriteLine("Я прочитал");

                var writer = new StreamWriter(stream);

                if (request[0] != '1' && request[0] != '2')
                {
                    writer.WriteLine("Неверный формат команды");
                    writer.Flush();
                    return;
                }

                if (request[0] == '1')
                {
                    var dirPath = request.Substring(2);
                    GetListOfFiles(dirPath, writer);
                }
                else
                {
                    var filePath = request.Substring(2);
                    GetFileContent(filePath, writer);
                }
            }

            private void GetFileContent(string filePath, StreamWriter writer)
            {
                if (!File.Exists(filePath))
                {
                    writer.WriteLine("-1");
                    writer.Flush();
                    return;
                }

                using (Stream source = File.OpenRead(filePath))
                {
                    writer.Write(source.Length);

                    byte[] buffer = new byte[2048];
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(Encoding.UTF8.GetString(buffer));
                    }
                    writer.WriteLine();
                    writer.Flush();
                }
            }

            private void GetListOfFiles(string dirPath, StreamWriter writer)
            {
                var dir = new DirectoryInfo(dirPath);

                if (!dir.Exists)
                {
                    writer.WriteLine("-1");
                    writer.Flush();
                    return;
                }

                var directorysList = dir.GetDirectories();
                var filesList = dir.GetFiles();

                int objectsNumber = directorysList.Length + filesList.Length;
                writer.Write(objectsNumber.ToString() + ' ');
                writer.Flush();

                foreach (var subDir in directorysList)
                {
                    writer.Write(subDir.Name + " - true ");
                }

                foreach (var files in filesList)
                {
                    writer.Write(files.Name + " - false ");
                }
                writer.WriteLine();
                writer.Flush();
            }
        }
    }
}
