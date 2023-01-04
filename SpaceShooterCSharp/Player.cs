using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;

namespace SpaceShooterCSharp
{
    public class Player
    {
        public int Health { get; set; }

        private double speed = 2.0;

        private bool goUp, goLeft, goRight, goDown;
        private Rectangle PlayerRectangle;

        private Rect playerHitbox;

        public Player(Canvas gameCanvas)
        {
            PlayerRectangle = spawnPlayer(gameCanvas);
        }

        private Rectangle spawnPlayer(Canvas gameCanvas)
        {
            Rectangle player = new Rectangle
            {
                Tag = "player",
                Height = 30,
                Width = 30,
                Fill = Brushes.Aqua
            };

            Canvas.SetLeft(player, Constants.playerStartPosX);
            Canvas.SetTop(player, Constants.playerStartPosY);

            Panel.SetZIndex(player, 1);

            gameCanvas.Children.Add(player);

            return player;
        }

        public void UpdatePlayer()
        {
            BorderCollisionCheck();
            UpdatePlayerPos();
            UpdatePlayerHitbox();
        }

        public Rect GetPlayerPos()
        {
            return playerHitbox;
        }

        private void UpdatePlayerHitbox()
        {
            playerHitbox = new Rect(Canvas.GetTop(PlayerRectangle), Canvas.GetLeft(PlayerRectangle), PlayerRectangle.Width, PlayerRectangle.Height);
        }

        public void UpdatePlayerPos()
        {
            CheckPlayerSpeed();

            if (goUp)
            {
                Canvas.SetTop(PlayerRectangle, Canvas.GetTop(PlayerRectangle) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(PlayerRectangle, Canvas.GetTop(PlayerRectangle) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(PlayerRectangle, Canvas.GetLeft(PlayerRectangle) - speed);
            }
            if (goRight)
            {
                Canvas.SetLeft(PlayerRectangle, Canvas.GetLeft(PlayerRectangle) + speed);
            }
        }

        private void CheckPlayerSpeed()
        {
            if ((goDown || goUp) && (goLeft || goRight))
                speed = Math.Sqrt(2.0);
            else
                speed = 2.0;
        }

        private void BorderCollisionCheck()
        {
            if (Canvas.GetTop(PlayerRectangle) - speed < 5)
            {
                goUp = false;
                Canvas.SetTop(PlayerRectangle, 5);
            }
            if (Canvas.GetLeft(PlayerRectangle) - speed < 5)
            {
                goLeft = false;
                Canvas.SetLeft(PlayerRectangle, 5);
            }
            if (Canvas.GetLeft(PlayerRectangle) + PlayerRectangle.ActualWidth + speed > Constants.WindowWidth - 5)
            {
                goRight = false;
                Canvas.SetLeft(PlayerRectangle, Constants.WindowWidth - PlayerRectangle.ActualWidth - 5);
            }
            if (Canvas.GetTop(PlayerRectangle) + PlayerRectangle.ActualHeight + speed > Constants.WindowHeight - 5)
            {
                goDown = false;
                Canvas.SetTop(PlayerRectangle, Constants.WindowHeight - PlayerRectangle.ActualHeight - 5);
            }
        }

        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    goUp = true;
                    break;
                case Key.Left:
                case Key.A:
                    goLeft = true;
                    break;
                case Key.Right:
                case Key.D:
                    goRight = true;
                    break;
                case Key.Down:
                case Key.S:
                    goDown = true;
                    break;
            }
        }

        public void OnKeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    goUp = false;
                    break;
                case Key.Left:
                case Key.A:
                    goLeft = false;
                    break;
                case Key.Right:
                case Key.D:
                    goRight = false;
                    break;
                case Key.Down:
                case Key.S:
                    goDown = false;
                    break;
                case Key.Space:
                    MainWindow.game.Shoot(1, PlayerRectangle);
                    break;
            }
        }
    }
}
