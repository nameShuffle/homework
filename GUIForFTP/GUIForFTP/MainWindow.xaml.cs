using System.Windows;
using System.Windows.Controls;

namespace GUIForFTP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentPort;
        private string currentAddres;

        public MainWindow()
        {
            InitializeComponent();
            
            var clientViewModel = new ClientViewModel();
            this.DataContext = clientViewModel;
        }
        
        /// <summary>
        /// Обработчик кнопки подключения к серверу.
        /// </summary>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(port.Text.ToString(), out int intPort))
            {
                this.currentAddres = addres.Text.ToString();
                this.currentPort = intPort;
                await (DataContext as ClientViewModel).StartConnection(intPort, addres.Text.ToString());
            }
        }

        /// <summary>
        /// Обработчик двойного нажатия на элемент ListBox. Если данный
        /// элемент является файлом, то ничего не проиходит. Если элемент - 
        /// директория, то обновляется информация в ListBox.
        /// </summary>
        private async void ObjectsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem;
            var objectInfo = item as ObjectInfo;

            if (objectInfo.IsDir)
            {
                await (DataContext as ClientViewModel).GoToDirectory(this.currentPort, 
                    this.currentAddres, objectInfo.FullPath);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку скачивания. Если никакой элемент
        /// ListBox-а не выделен в настоящий момент, то ничего не проиходит.
        /// Аналогично если выделенный элемент - директория. Если это файл, то
        /// производится скачивание этого файла.
        /// </summary>
        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                var objectInfo = item as ObjectInfo;
                if (!objectInfo.IsDir)
                {
                    (DataContext as ClientViewModel).DownloadOneFile(this.currentPort,
                        this.currentAddres, objectInfo, PathToDownload.Text.ToString(),
                        Dispatcher);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "скачать все". Производится скачивание
        /// всех файлов, лежащих в текущей директории.
        /// </summary>
        private void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ClientViewModel).DownloadAllFiles(this.currentPort,
                this.currentAddres, PathToDownload.Text.ToString(), Dispatcher);
        }
    }
}
