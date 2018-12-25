using System.Threading.Tasks;
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
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(port.Text.ToString(), out int intPort))
            {
                this.currentAddres = addres.Text.ToString();
                this.currentPort = intPort;
                await (DataContext as ClientViewModel).StartConnection(intPort, addres.Text.ToString());
            }
        }

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

        private async void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            var item = ObjectsList.SelectedItem;
            if (item != null)
            {
                var objectInfo = item as ObjectInfo;
                if (!objectInfo.IsDir)
                {
                    await (DataContext as ClientViewModel).DownloadFile(this.currentPort,
                        this.currentAddres, objectInfo, PathToDownload.Text.ToString(),
                        Dispatcher);
                }
            }
        }

        private async void ButtonDownloadAll_Click(object sender, RoutedEventArgs e)
        {
            await (DataContext as ClientViewModel).DownloadAllFiles(this.currentPort,
                this.currentAddres, PathToDownload.Text.ToString(), Dispatcher);
        }
    }
}
