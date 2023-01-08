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

namespace SpaceShooterCSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static GameEngine game = new();
        public MainWindow()
        {
            InitializeComponent();
            Show();
            LocationChanged += new EventHandler(Window_LocationChanged);
            game.InitializeGame(gameCanvas);
           

        }

        private void Window_LocationChanged(object? sender, EventArgs e)
        {
            foreach (Window window in OwnedWindows)
            {
                window.Top = Top + ((ActualHeight - window.ActualHeight) / 2);
                window.Left = Left + ((ActualWidth - window.ActualWidth) / 2);
            }
        }


        //KeyDown and KeyUp Events handed over to GameEngine
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.OnKeyDown(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            game.OnKeyUp(sender, e);
        }
    }
}
