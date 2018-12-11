using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

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

        class Client
        {
            public void Work(int port)
            {
                while (true)
                {
                    try
                    {
                        using (var client = new TcpClient("localhost", port))
                        {
                            var stream = client.GetStream();

                            var writer = new StreamWriter(stream);

                            string commandNumber;
                            Console.WriteLine("Введите номер команды");
                            commandNumber = Console.ReadLine();

                            if (commandNumber == "1")
                            {
                                Console.WriteLine("Введите путь к директории");
                                string dirPath = Console.ReadLine();

                                writer.WriteLine(commandNumber + " " + dirPath);
                                writer.Flush();
                            }
                            else if (commandNumber == "2")
                            {
                                Console.WriteLine("Введите путь к файлу");
                                string filePath = Console.ReadLine();

                                writer.WriteLine(commandNumber + " " + filePath);
                                writer.Flush();
                            }
                            else
                            {
                                Console.WriteLine("Такой команды не существует");

                                continue;
                            }


                            var reader = new StreamReader(stream);
                            string data = reader.ReadLine();
                            Console.WriteLine(data);
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

    }
}
