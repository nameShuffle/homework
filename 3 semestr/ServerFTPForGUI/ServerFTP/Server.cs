using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ServerFTP
{
    /// <summary>
    /// Класс, предоставляющий возможность создания сервера и корректной обработки
    /// запросов, поступающих с клиента.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Главный метод, реализующий логику работы сервера. 
        /// При попытке подключения клиента создается и запускается новый Task,
        /// в котором выполняется обработка запросов, поступающих с клиента.
        /// </summary>
        /// <param name="port">Номер порта.</param>
        public async Task Work()
        {
            int port = 5555;

            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                var socket = await listener.AcceptSocketAsync();
                var manager = new Task(clientSocket => ManageRequest((Socket)clientSocket), socket);
                manager.Start();
            }
        }

        /// <summary>
        /// Метод позволяет корректно обрабатывать команды подключившегося клиента.
        /// Распознает тип команды и вызывает метод, требуемый для выполнения задачи.
        /// </summary>
        private async void ManageRequest(Socket socket)
        {
            //var stream = new NetworkStream(socket);
            using (var stream = new NetworkStream(socket))
            {
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                var request = await reader.ReadLineAsync();

                if (request == "startconnection")
                {
                    var dir = (new DirectoryInfo(Directory.GetCurrentDirectory())).Parent.Parent.Parent.Parent;
                    await writer.WriteAsync(dir.FullName);
                    await writer.FlushAsync();
                    socket.Close();
                    return;
                }

                if (request[0] != '1' && request[0] != '2')
                {
                    await writer.WriteAsync("Неверный формат команды");
                    await writer.FlushAsync();
                    socket.Close();
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
                    GetFileContent(filePath, writer, stream);
                }

                socket.Close();
            }
        }

        /// <summary>
        /// Метод получает содержимое файла по данному пути и записывает его
        /// в поток в требуемом формате.
        /// Если такого файла не существует, в поток записывается значение
        /// "-1".
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="writer">Позволяет записывать данные в нужный поток.</param>
        private async void GetFileContent(string filePath, StreamWriter writer, NetworkStream stream)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    await writer.WriteLineAsync("-1");
                    await writer.FlushAsync();
                    return;
                }

                var fileLength = (new FileInfo(filePath)).Length;
                await writer.WriteLineAsync(fileLength.ToString());
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    fileStream.CopyTo(stream);
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Не удается передать информацию. Возможно, разорвано соединение.");
            }
        }

        /// <summary>
        /// Метод получает список файлов и подпапок по данному пути
        /// и записывает его в поток в требуемом формате.
        /// Если такой директории не существует, в поток заносится значение "-1".
        /// </summary>
        /// <param name="dirPath">Путь к директории.</param>
        /// <param name="writer">Позволяет записывать в нужный поток данные.</param>
        private async void GetListOfFiles(string dirPath, StreamWriter writer)
        {
            try
            {
                var dir = new DirectoryInfo(dirPath);

                if (!dir.Exists)
                {
                    await writer.WriteLineAsync("-1");
                    await writer.FlushAsync();
                    return;
                }

                var directorysList = dir.GetDirectories();
                var filesList = dir.GetFiles();

                int objectsNumber = directorysList.Length + filesList.Length;

                var answer = objectsNumber.ToString();

                foreach (var subDir in directorysList)
                {
                    answer += $"\n{subDir.Name}\ntrue";
                }

                foreach (var file in filesList)
                {
                    answer += $"\n{file.Name}\nfalse";
                }
                await writer.WriteAsync(answer);
                await writer.FlushAsync();
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Не удается передать информацию. Возможно, разорвано соединение.");
            }
            
        }
    }
}
