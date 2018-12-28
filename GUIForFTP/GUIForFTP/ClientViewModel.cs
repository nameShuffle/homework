using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GUIForFTP
{
    class ClientViewModel : INotifyPropertyChanged
    {
        private string currentDirectory;
        private string finalDirectory;
        
        private Client client = new Client();

        public ObservableCollection<ObjectInfo> Objects { get; private set; } = new ObservableCollection<ObjectInfo>();
        public ObservableCollection<string> DownloadList { get; private set; } = new ObservableCollection<string>();

        public string Warning { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Отправляет начальный запрос от клиента с просьбой вернуть список
        /// объектов в директории homework.
        /// </summary>
        public async Task StartConnection(int port, string addres)
        {
            var connectionCommand = "startconnection";
            try
            {
                this.Warning = "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));

                var dirFromServer = await Task.Run(() => client.GetDirectoryList(port, addres, connectionCommand));

                this.currentDirectory = dirFromServer;
                this.finalDirectory = dirFromServer;

                var command = $"1 {dirFromServer}";
                var response = await Task.Run(() => client.GetDirectoryList(port, addres, command));

                SetInfoToListOfObjects(response);
            }
            catch (ObjectDisposedException ex)
            {
                this.Warning = ex.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            }
        }

        /// <summary>
        /// Осуществляется переход в новую директорию - получается информация 
        /// о файлах и поддиректория по указанному пути.
        /// </summary>
        /// <param name="path">Путь к нужной директории.</param>
        public async Task GoToDirectory(int port, string addres, string path)
        {
            this.currentDirectory = path;
            var command = $"1 {path}";
            try
            {
                var response = await Task.Run(() => client.GetDirectoryList(port, addres, command));
                SetInfoToListOfObjects(response);
            }
            catch (DirectoryNotFoundException ex)
            {
                this.Warning = ex.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            }
            catch (ObjectDisposedException ex)
            {
                this.Warning = ex.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            }
        }

        /// <summary>
        /// Данных метод асинхронно скачивает все файлы из текущей директории.
        /// </summary>
        /// <param name="pathToDowmload">Путь для скачивания файла.</param>
        public void DownloadAllFiles(int port, string addres, string pathToDowmload, 
            Dispatcher dispatcher)
        {
            this.DownloadList.Clear();
            foreach (var obj in Objects)
            {
                if (!obj.IsDir)
                {   
                    Task.Run(() => DownloadFile(port, addres, obj, pathToDowmload, dispatcher));
                }
            }
        }

        public void DownloadOneFile(int port, string addres, ObjectInfo file, 
            string pathToDowmload, Dispatcher dispatcher)
        {
            this.DownloadList.Clear();
            Task.Run(() => DownloadFile(port, addres, file, pathToDowmload, dispatcher));
        }

        /// <summary>
        /// Метод для скачивания одного файла по указанному пути.
        /// </summary>
        /// <param name="file">Объект, содержащий всю необходимую информацию о файле.</param>
        /// <param name="pathToDownload">Путь для скачивания файла.</param>
        public async Task DownloadFile(int port, string addres, ObjectInfo file, 
            string pathToDownload, Dispatcher dispatcher)
        {
            if (pathToDownload == "")
            {
                this.Warning = "Введите путь для скачивания!";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
                return;
            }
            try
            {
                var newFileToDownload = $"{file.Name} downloading...";
                
                await dispatcher.InvokeAsync(() => this.DownloadList.Add(newFileToDownload));
                
                var command = $"2 {file.FullPath}";
                var filePath = pathToDownload + $"/{file.Name}";
                await Task.Run(() => client.DownloadFile(port, addres, command, filePath));
                
                await dispatcher.InvokeAsync(() => this.DownloadList.Remove(newFileToDownload));
                await dispatcher.InvokeAsync(() => this.DownloadList.Add($"{file.Name} download finished"));
                
            }
            catch (FileNotFoundException ex)
            {
                this.Warning = ex.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            }
            catch (ObjectDisposedException ex)
            {
                this.Warning = ex.Message;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            }
        }

        /// <summary>
        /// Метод обрабатывает полученную строку, собирает объекты ObjectInfo
        /// и добавляет их в коллекцию.
        /// </summary>
        /// <param name="info">Информация, полученная от сервера.</param>
        private void SetInfoToListOfObjects(string info)
        {
            Objects.Clear();
            var objectsArray = info.Split('\n');
            if (int.TryParse(objectsArray[0], out int numberOfObjects))
            {
                for (int i = 1; i <= numberOfObjects; i++)
                {
                    if (objectsArray[2 * i] == "true")
                    {
                        var name = objectsArray[2 * i - 1];
                        var nameForList = name + " - directory";
                        var fullPath = this.currentDirectory + $"/{objectsArray[2 * i - 1]}";
                        var obj = new ObjectInfo(true, name, nameForList, fullPath);
                        Objects.Add(obj);
                    }
                    else
                    {
                        var name = objectsArray[2 * i - 1];
                        var nameForList = name + " - file";
                        var fullPath = this.currentDirectory + $"/{objectsArray[2 * i - 1]}";
                        var obj = new ObjectInfo(false, name, nameForList, fullPath);
                        Objects.Add(obj);
                    }
                }

                if (this.currentDirectory != this.finalDirectory)
                {
                    var parent = new ObjectInfo(true, "<-", "<-", (new DirectoryInfo(this.currentDirectory)).Parent.FullName);
                    Objects.Add(parent);
                }
            }
        }
    }
}
