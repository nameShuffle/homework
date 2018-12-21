using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GUIForFTP
{
    /// <summary>
    /// Класс, реализующий логику клиентской стороны. Позволяет подключаться
    /// к серверу и передавать ему запросы, введенные с клавиатуры.
    /// </summary>
    public class Client
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
        public async Task<string> GetResponce(int port, string addres, string command)
        {
            string data = null;

            try
            {
                using (var client = new TcpClient(addres, port))
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
