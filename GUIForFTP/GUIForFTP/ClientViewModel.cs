
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GUIForFTP
{
    class ClientViewModel
    {
        ///public event PropertyChangedEventHandler PropertyChanged;
        private string currentDirectory;
        private string finalDirectory;

        private Client client = new Client();

        private Object lockObject = new Object();

        public ObservableCollection<ObjectInfo> Objects { get; private set; } = new ObservableCollection<ObjectInfo>();
        public ObservableCollection<string> DownloadList { get; private set; } = new ObservableCollection<string>();

        /// <summary>
        /// Отправляет начальный запрос от клиента с просьбой вернуть список
        /// объектов в директории homework.
        /// </summary>
        public async Task StartConnection(int port, string addres)
        {
            var connectionCommand = "startconnection";
            try
            {
                var dirFromServer = await Task.Run(() => client.GetResponce(port, addres, connectionCommand));

                this.currentDirectory = dirFromServer;
                this.finalDirectory = dirFromServer;

                var command = $"1 {dirFromServer}";
                var responce = await Task.Run(() => client.GetResponce(port, addres, command));

                await SetInfoToListOfObjects(responce);
            } 
            catch
            {

            }
        }

        public async Task GoToDirectory(int port, string addres, string path)
        {
            this.currentDirectory = path;
            var command = $"1 {path}";
            var responce = await Task.Run(() => client.GetResponce(port, addres, command));

            await SetInfoToListOfObjects(responce);
        }


        public async Task DownloadAllFiles(int port, string addres, string pathToDowmload, Dispatcher dispatcher)
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

        public async Task DownloadFile(int port, string addres, ObjectInfo file, string pathToDownload, Dispatcher dispatcher)
        {
            //var file2 = new FileToDownload($"{file.Name} downloading...");
            
            var newFileToDownload = $"{file.Name} downloading...";
            lock (lockObject)
            {
                dispatcher.InvokeAsync(() => this.DownloadList.Add(newFileToDownload));
            }
            
            var command = $"2 {file.FullPath}";
            var responce = await Task.Run(() => client.GetResponce(port, addres, command));

            var fileContent = responce.Substring(responce.IndexOf(' ') + 1);
            var filePath = pathToDownload + $"/{file.Name}";

            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(fileContent);
                sw.Close();
            }

            //file2.Status = $"{file.Name} download finished";
            lock (lockObject)
            {
                dispatcher.InvokeAsync(() => this.DownloadList.Remove(newFileToDownload));
                dispatcher.InvokeAsync(() => this.DownloadList.Add($"{file.Name} download finished"));
            }
            
            //await CreateFile(fileContent, pathToDownload + $"/{file.Name}");
        }

        private async Task CreateFile(string fileContent, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(fileContent);
                sw.Close();
            }
        }
        /// <summary>
        /// Метод обрабатывает полученную строку, собирает объекты ObjectInfo
        /// и добавляет в коллекцию таких объектов.
        /// </summary>
        /// <param name="info">Информация, полученная от сервера.</param>
        private async Task SetInfoToListOfObjects(string info)
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
