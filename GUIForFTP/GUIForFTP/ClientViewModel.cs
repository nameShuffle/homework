
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace GUIForFTP
{
    class ClientViewModel
    {
        ///public event PropertyChangedEventHandler PropertyChanged;
        readonly Client client = new Client();
        ObservableCollection<ObjectInfo> Objects { get; set; } = new ObservableCollection<ObjectInfo>();

        /// <summary>
        /// Отправляет начальный запрос от клиента с просьбой вернуть список
        /// объектов в директории homework.
        /// </summary>
        public void StartConnection(int port, string addres)
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent;
            string dirToServer = dir.FullName;

            var command = $"1 {dirToServer}";
            var responce = client.GetResponce(port, addres, command).Result;

            SetInfoToListOfObjects(responce);
        }

        /// <summary>
        /// Метод обрабатывает полученную строку, собирает объекты ObjectInfo
        /// и добавляет в коллекцию таких объектов.
        /// </summary>
        /// <param name="info">Информация, полученная от сервера.</param>
        private void SetInfoToListOfObjects(string info)
        {
            var objectsArray = info.Split(' ');
            if (int.TryParse(objectsArray[0], out int numberOfObjects))
            {
                for (int i = 1; i <= numberOfObjects; i++)
                {
                    if (objectsArray[2*i] == "true")
                    {
                        var obj = new ObjectInfo(true, objectsArray[2*i-1]);
                        Objects.Add(obj);
                    }
                    else
                    {
                        var obj = new ObjectInfo(false, objectsArray[2 * i - 1]);
                        Objects.Add(obj);
                    }
                }
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(objects)));
            }
        }
    }
}
