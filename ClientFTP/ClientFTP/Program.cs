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

        /// <summary>
        /// Класс, реализующий логику клиентской стороны. Позволяет подключаться
        /// к серверу и передавать ему запросы, введенные с клавиатуры.
        /// </summary>
        class Client
        {
            /// <summary>
            /// Основной метод, описывающий логику работы клиента. Предлагает пользователю
            /// ввести команду. Если она была введена в неверном формате, сообщает об 
            /// этом. Если команда была введена верно, то пробует подключиться к серверу
            /// и передать ему введеную команду.
            /// После чего получает информацию, сформированную сервером после обработки
            /// запроса, и выводит ее на экран.
            /// </summary>
            /// <param name="port">Номер порта.</param>
            public void Work(int port)
            {
                while (true)
                {
                    Console.WriteLine("Введите номер команды");
                    string commandNumber = Console.ReadLine();

                    string fullCommand;
                    if (commandNumber == "1")
                    {
                        Console.WriteLine("Введите путь к директории");
                        string dirPath = Console.ReadLine();
                        fullCommand = commandNumber + ' ' + dirPath;
                    }
                    else if (commandNumber == "2")
                    {
                        Console.WriteLine("Введите путь к файлу");
                        string filePath = Console.ReadLine();
                        fullCommand = commandNumber + ' ' + filePath;
                    }
                    else
                    {
                        Console.WriteLine("Такой команды не существует");
                        continue;
                    }

                    try
                    {
                        using (var client = new TcpClient("localhost", port))
                        {
                            var stream = client.GetStream();

                            var writer = new StreamWriter(stream);
                            var reader = new StreamReader(stream);

                            writer.WriteLine(fullCommand);
                            writer.Flush();

                            string data = reader.ReadToEnd();
                            Console.WriteLine(data);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

    }
}
