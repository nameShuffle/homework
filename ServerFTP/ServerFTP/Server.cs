using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerFTP
{
    /// <summary>
    /// Класс, предоставляющий возможность создания сервера и корректной обработки
    /// запросов, поступающих с клиента.
    /// </summary>
    class Server
    {
        /// <summary>
        /// Главный метод, реализующий логику работы сервера. 
        /// При попытке подключения клиента создается и запускается новый Task,
        /// в котором выполняется обработка запросов, поступающих с клиента.
        /// </summary>
        /// <param name="port">Номер порта.</param>
        public void Work(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            while (true)
            {
                var socket = listener.AcceptSocketAsync();
                var manager = new Task(clientSocket => ManageRequest((Socket)clientSocket), socket);
                manager.Start();
            }
        }

        /// <summary>
        /// Метод позволяет корректно обрабатывать команды подключившегося клиента.
        /// Распознает тип команды и вызывает метод, требуемый для выполнения задачи.
        /// </summary>
        public async void ManageRequest(Socket socket)
        {
            var stream = new NetworkStream(socket);

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);

            var request = await reader.ReadLineAsync();

            if (request[0] != '1' && request[0] != '2')
            {
                await writer.WriteLineAsync("Неверный формат команды");
                await writer.FlushAsync();
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

            socket.Close();
        }

        /// <summary>
        /// Метод получает содержимое файла по данному пути и записывает его
        /// в поток в требуемом формате.
        /// Если такого файла не существует, в поток записывается значение
        /// "-1".
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="writer">Позволяет записывать данные в нужный поток.</param>
        private async void GetFileContent(string filePath, StreamWriter writer)
        {
            if (!File.Exists(filePath))
            {
                await writer.WriteAsync("-1");
                await writer.FlushAsync();
                return;
            }

            byte[] content = File.ReadAllBytes(filePath);
            long length = content.Length;
            await writer.WriteAsync(length.ToString() + ' ' + Encoding.UTF8.GetString(content));
            await writer.FlushAsync();
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
            var dir = new DirectoryInfo(dirPath);

            if (!dir.Exists)
            {
                await writer.WriteAsync("-1");
                await writer.FlushAsync();
                return;
            }

            var directorysList = dir.GetDirectories();
            var filesList = dir.GetFiles();

            int objectsNumber = directorysList.Length + filesList.Length;
            await writer.WriteAsync(objectsNumber.ToString() + ' ');
            await writer.FlushAsync();

            foreach (var subDir in directorysList)
            {
                await writer.WriteAsync(subDir.Name + " - true ");
            }

            foreach (var files in filesList)
            {
                await writer.WriteAsync(files.Name + " - false ");
            }
            await writer.FlushAsync();
        }
    }
}
