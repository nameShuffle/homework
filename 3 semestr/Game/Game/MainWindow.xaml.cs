using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameNAndC game;

        public MainWindow()
        {
            InitializeComponent();

            game = new GameNAndC();
        }

        private void b11_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(0, 0);
            game.ChangeGamer();
        }

        private void b21_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(1, 0);
            game.ChangeGamer();
        }

        private void b31_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(2, 0);
            game.ChangeGamer();
        }

        private void b12_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(0, 1);
            game.ChangeGamer();
        }

        private void b22_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(1, 1);
            game.ChangeGamer();
        }

        private void b32_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(2, 1);
            game.ChangeGamer();
        }

        private void b13_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(0, 2);
            game.ChangeGamer();
        }

        private void b23_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(1, 2);
            game.ChangeGamer();
        }

        private void b33_Click(object sender, RoutedEventArgs e)
        {
            game.NewItem(2, 2);
            game.ChangeGamer();
        }
    }
}
