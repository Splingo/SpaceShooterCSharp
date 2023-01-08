using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpaceShooterCSharp
{
    class MenuHelper
    {
        //helper class for Menus. sets position of menus to center of game window
        internal void ShowGameLostMenu()
        {
            Window_GameLostMenu menu = new();
            menu.Owner = Window.GetWindow(Application.Current.MainWindow);
            menu.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            menu.Topmost = true;
            menu.Show();
        }

        internal void ShowHelpMenu()
        {
            Window_HelpMenu menu = new();
            menu.Owner = Window.GetWindow(Application.Current.MainWindow);
            menu.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            menu.Topmost = true;
            menu.Show();
        }
    }
}
