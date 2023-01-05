using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace SpaceShooterCSharp
{
    internal class Enemy
    {
        private int enemyXSpeed;
        private int enemyYSpeed;
        private int shooterTimer;

        private Rectangle enemy;

        public Enemy(int direction)
        {
            shooterTimer = 0;
            enemyXSpeed = direction;
            enemyYSpeed = direction * new Random().Next(-1, 2);
            enemy = spawnEnemy();
        }

        private Rectangle spawnEnemy()
        {
            enemy = new Rectangle
            {
                Tag = enemy,
                Height = 20,
                Width = 30,
                Fill = Brushes.Lime
            };

            Canvas.SetLeft(enemy, Constants.WindowWidth - enemy.Width);
            Canvas.SetTop(enemy, new Random().Next(20, Constants.WindowHeight - 19));
            Panel.SetZIndex(enemy, 2);
            MainWindow.game.GameCanvas?.Children.Add(enemy);

            return enemy;
        }

        public bool Update()
        {
            shooterTimer++;
            if (shooterTimer % 100 == 0)
            {
                MainWindow.game.Shoot(-1, enemy);
            }
            if (CheckBorderCollision())
            {
                MoveEnemy();
                return true;
            }
            else
                return false;
        }

        private void MoveEnemy()
        {
            Canvas.SetLeft(enemy, Canvas.GetLeft(enemy) + enemyXSpeed * 1.5);
            Canvas.SetTop(enemy, Canvas.GetTop(enemy) + enemyYSpeed);
        }

        private bool CheckBorderCollision()
        {
            if (Canvas.GetLeft(enemy) - enemyXSpeed <= 5)
            {
                MainWindow.game.GameCanvas?.Children.Remove(enemy);
                return false;
            }
            if (Canvas.GetTop(enemy) + enemyYSpeed < 5)
                enemyYSpeed *= -1;
            if (Canvas.GetTop(enemy) + enemyYSpeed + enemy?.Height > Constants.WindowHeight - 5)
                enemyYSpeed *= -1;
            return true;

        }

        public Rect GetEnemyHitbox()
        {
            return new Rect(Canvas.GetLeft(enemy), Canvas.GetTop(enemy), enemy.Width, enemy.Height);
        }
        public void deleteEnemy()
        {
            MainWindow.game.GameCanvas?.Children.Remove(enemy);
        }
    }
}
