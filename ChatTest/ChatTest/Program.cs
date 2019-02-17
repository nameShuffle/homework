using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isServer;
            if (args.Length == 2)
            {
                isServer = false;
                var server = new Chat();
                if (!int.TryParse(args[0], out int port))
                {
                    Console.WriteLine("неверный формат данных");
                    return;
                }
                string adress = args[1];
               
                server.Work(isServer, port, adress);
            }
            else
            {
                isServer = true;
                var server = new Chat();
                if (!int.TryParse(args[0], out int port))
                {
                    Console.WriteLine("неверный формат данных");
                    return;
                }

                server.Work(isServer, port, "");
            }
            
        }

        /// <summary>
        /// Данный класс реализует консольный сетевой чат.
        /// </summary>
        class Chat
        {
            /// <summary>
            /// Реализует работу часа - создает клиентское или серверное приложение
            /// и реализует общение с собеседником.
            /// </summary>
            /// <param name="isServer">Сообщает сервер или клиента требуется создать.</param>
            /// <param name="port">Номер порта.</param>
            /// <param name="adress">IP-адрес</param>
            public void Work(bool isServer, int port, string adress)
            {
                if (isServer)
                {
                    var listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();

                    var socket = listener.AcceptSocket();

                    var stream = new NetworkStream(socket);

                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);

                    var mainReader = new Task(readerClient => ReadFromClient((StreamReader)readerClient), reader);
                    mainReader.Start();

                    while (true)
                    {
                        string message = Console.ReadLine();
                        writer.WriteLine(message);
                        writer.Flush();
                        if (message == "exit")
                        {
                            break;
                        }
                    }

                    socket.Close();
                }
                else
                {
                    try
                    {
                        using (var client = new TcpClient(adress, port))
                        {
                            var stream = client.GetStream();

                            var writer = new StreamWriter(stream);
                            var reader = new StreamReader(stream);

                            var mainReader = new Task(readerClient => ReadFromClient((StreamReader)readerClient), reader);
                            mainReader.Start();

                            while (true)
                            {
                                string message = Console.ReadLine();
                                writer.WriteLine(message);
                                writer.Flush();
                                if (message == "exit")
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                
            }

            /// <summary>
            /// Метод принимает сообщения со стороны собеседника и выводит их на консоль.
            /// </summary>
            /// <param name="reader"></param>
            private void ReadFromClient(StreamReader reader)
            {
                while(true)
                {
                    string message = reader.ReadLine();
                    Console.WriteLine(message);
                    if (message == "exit")
                    {
                        break;
                    }
                }
            }
        }
    }
}
