using System;
using System.IO;
using System.Net.Sockets;

namespace ClientFTP
{
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
        public async void Work(int port)
        {
            while (true)
            {
                string command = Console.ReadLine();

                try
                {
                    using (var client = new TcpClient("localhost", port))
                    {
                        var stream = client.GetStream();

                        var writer = new StreamWriter(stream);
                        var reader = new StreamReader(stream);

                        await writer.WriteLineAsync(command);
                        await writer.FlushAsync();

                        string data = await reader.ReadToEndAsync();
                        Console.WriteLine(data);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Ошибка подключения к серверу");
                }
            }
        }
    }
}
