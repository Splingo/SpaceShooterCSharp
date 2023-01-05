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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceShooterCSharp
{
    /// <summary>
    /// Interaction logic for Window_GameLostMenu.xaml
    /// </summary>
    public partial class Window_GameLostMenu : Window
    {
        public Window_GameLostMenu()
        {
            InitializeComponent();
            textBlock2.Inlines.Add($"YOUR SCORE: {MainWindow.game.ScoreAsInt}");
            Topmost = true;
        }

        private void Button_ExitGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Restart(object sender, RoutedEventArgs e)
        {
            MainWindow.game.StartGame(true);
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
