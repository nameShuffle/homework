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
                    var socket = listener.AcceptSocket();

                    var manager = new Task(clientSocket => ManageRequest((Socket)clientSocket), socket);

                    manager.Start();
                }
            }       

            /// <summary>
            /// Метод позволяет корректно обрабатывать команды подключившегося клиента.
            /// Распознает тип команды и вызывает метод, требуемый для выполнения задачи.
            /// </summary>
            public void ManageRequest(Socket socket)
            {
                var stream = new NetworkStream(socket);

                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                var request = reader.ReadLine();

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
            private void GetFileContent(string filePath, StreamWriter writer)
            {
                if (!File.Exists(filePath))
                {
                    writer.Write("-1");
                    writer.Flush();
                    return;
                }

                byte[] content = File.ReadAllBytes(filePath);
                long length = content.Length;
                writer.Write(length.ToString() + ' ' + Encoding.UTF8.GetString(content));
                writer.Flush();
            }
            
            /// <summary>
            /// Данный метод получает список файлов и подпапок по данному пути
            /// и записывает его в поток в требуемом формате.
            /// Если такой директории не существует, в поток заносится значение "-1".
            /// </summary>
            /// <param name="dirPath">Путь к директории.</param>
            /// <param name="writer">Позволяет записывать в нужный поток данные.</param>
            private void GetListOfFiles(string dirPath, StreamWriter writer)
            {
                var dir = new DirectoryInfo(dirPath);

                if (!dir.Exists)
                {
                    writer.Write("-1");
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
                writer.Flush();
            }
        }
    }
}