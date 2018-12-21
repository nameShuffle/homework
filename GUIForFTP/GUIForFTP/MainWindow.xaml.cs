using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            var clienViewModel = new ClientViewModel();
            this.DataContext = clienViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(port.Text.ToString(), out int intPort))
            {
                var task = new Task (() => 
                    (DataContext as ClientViewModel).
                    StartConnection(intPort, addres.Text.ToString()));
                task.Start();
            }
        }
    }
}
