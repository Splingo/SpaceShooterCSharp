using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceShooterCSharp
{
    internal class Bullet
    {
        private int bulletSpeed;

        private Rectangle? bullet;

        public Bullet(int direction, Rectangle origin)
        {
            bulletSpeed = direction * 5;
            spawnBullet(origin);
        }

        private void spawnBullet(Rectangle origin)
        {
            bullet = new Rectangle
            {
                Tag = bullet,
                Height = 4,
                Width = 8,
                Fill = Brushes.Lime
            };

            if (bulletSpeed > 0)
            {
                Canvas.SetLeft(bullet, Canvas.GetLeft(origin) + origin.ActualWidth);
                bullet.Fill = Brushes.Red;
                bullet.Tag = "playerBullet";
            }
            else
                Canvas.SetLeft(bullet, Canvas.GetLeft(origin));


            Canvas.SetTop(bullet, Canvas.GetTop(origin) + origin.ActualHeight / 2);

            Panel.SetZIndex(bullet, 1);

            MainWindow.game.GameCanvas?.Children.Add(bullet);
        }

        internal void Update()
        {
            if (CheckBorderCollision())
            {
                Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + bulletSpeed);
            }
        }
        private bool CheckBorderCollision()
        {
            if (Canvas.GetLeft(bullet) > Constants.WindowWidth || Canvas.GetLeft(bullet) < 0)
            {
                deleteBullet();
                return false;
            }
            return true;
        }

        public string? CheckBulletTag()
        {
            return bullet?.Tag.ToString();
        }

        public Rect GetBulletHitbox()
        {
            return new Rect(Canvas.GetLeft(bullet), Canvas.GetTop(bullet), bullet.Width, bullet.Height); ;
        }

        public void deleteBullet()
        {
            MainWindow.game.GameCanvas?.Children.Remove(bullet);
        }
    }
}
