﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClientFTP
{
    /// <summary>
    /// Класс, реализующий логику клиентской стороны. Позволяет подключаться
    /// к серверу и передавать ему запросы, введенные с клавиатуры.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Основной метод, описывающий логику работы клиента. 
        /// Отправляет запрос на серверв, после чего получает информацию, сформированную сервером после обработки
        /// запроса, и возвращает ее.
        /// </summary>
        /// <param name="port">Номер порта.</param>
        public async Task<string> GetResponce(int port, string command)
        {
            string data = null;

            try
            {
                using (var client = new TcpClient("localhost", port))
                {
                    var stream = client.GetStream();

                    var writer = new StreamWriter(stream);
                    var reader = new StreamReader(stream);

                    await writer.WriteLineAsync(command);
                    await writer.FlushAsync();

                    data = await reader.ReadToEndAsync();
                    return data;
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Ошибка подключения к серверу");
                return data;
            }
        }
    }
}
