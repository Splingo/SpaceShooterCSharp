using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceShooterCSharp
{
    /// <summary>
    /// Interaction logic for Window_HelpMenu.xaml
    /// </summary>
    public partial class Window_HelpMenu : Window
    {
        public Window_HelpMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_StartGame(object sender, RoutedEventArgs e)
        {
            MainWindow.game.StartGame(false);
            Close();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

    }
}
