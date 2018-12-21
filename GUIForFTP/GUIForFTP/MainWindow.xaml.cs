using System.Threading.Tasks;
using System.Windows;

namespace GUIForFTP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var clientViewModel = new ClientViewModel();
            this.DataContext = clientViewModel;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(port.Text.ToString(), out int intPort))
            {
                var task = new Task(() =>
                (DataContext as ClientViewModel).StartConnection(intPort, addres.Text.ToString()));

                task.Start();
            }
        }
    }
}
