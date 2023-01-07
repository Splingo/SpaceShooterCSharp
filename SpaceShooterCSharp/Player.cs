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
using System.Windows.Media.Imaging;

namespace SpaceShooterCSharp
{
    public class Player
    {
        public int Health { get; internal set; }

        private double speed;
        private bool goUp, goLeft, goRight, goDown;
        private Rectangle PlayerRectangle;

        public Player()
        {
            PlayerRectangle = spawnPlayer();
        }

        private Rectangle spawnPlayer()
        {
            Rectangle player = new Rectangle
            {
                Tag = "player",
                Height = 28,
                Width = 32,
                Fill = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/player.png")))
            };

            ResetPlayer();

            Panel.SetZIndex(player, 2);

            return player;

        }

        public void SetResetPlayerPos(Canvas gameCanvas)
        {
            Canvas.SetLeft(PlayerRectangle, Constants.playerStartPosX);
            Canvas.SetTop(PlayerRectangle, Constants.playerStartPosY);
            if (!gameCanvas.Children.Contains(PlayerRectangle))
                gameCanvas.Children.Add(PlayerRectangle);

        }

        //method resets player hp and stops movement if game is restarted
        public void ResetPlayer()
        {
            goUp = false;
            goLeft = false;
            goRight = false;
            goDown = false;
            Health = 3;
        }

        //method is called each tick from GameEngine
        public void UpdatePlayer()
        {
            BorderCollisionCheck();
            UpdatePlayerPos();
        }


        //method checks if any border is reached and locks player position player 5px away from it
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

        //method checks player speed and moves player according to set movement directions
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

        //adjusts player speed if moving diagonally
        private void CheckPlayerSpeed()
        {
            if ((goDown || goUp) && (goLeft || goRight))
                speed = Math.Sqrt(2.0);
            else
                speed = 2.0;
        }

        //get hitbox for collision checks
        //hitbox gets small adjustment to match spaceship proportions
        public Rect GetPlayerHitbox()
        {
            return new Rect(Canvas.GetLeft(PlayerRectangle) + 5, Canvas.GetTop(PlayerRectangle) + 6, PlayerRectangle.Width - 10, PlayerRectangle.Height - 12);
        }


        //EventHandlers sets movement directions if WASD/Arrow Key are pressed/released and calls Shoot method if Space is released
        #region KeyEventHandlers
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
        #endregion
    }
}
