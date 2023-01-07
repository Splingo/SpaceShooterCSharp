using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceShooterCSharp
{
    internal class Enemy
    {
        private double enemyXSpeed;
        private int enemyYSpeed;
        private int shooterTimer;
        private Rectangle enemy;

        //Constructor for enemie gets random number from -1,0,1 and sets initial Y Speed to that value.
        //this way each enemy moves randomly up/down or not in Y direction during spawn
        public Enemy(int direction)
        {
            shooterTimer = 0;
            enemyXSpeed = direction;
            enemyYSpeed = direction * new Random().Next(-1, 2);
            enemy = spawnEnemy();
        }

        //creates enemy rectangle and positions it randomly on the right side
        private Rectangle spawnEnemy()
        {
            enemy = new Rectangle
            {
                Tag = enemy,
                Height = 20,
                Width = 30,
                Fill = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/enemy.png")))
            };

            Canvas.SetLeft(enemy, Constants.WindowWidth - enemy.Width);
            Canvas.SetTop(enemy, new Random().Next(20, Constants.WindowHeight - 19));
            Panel.SetZIndex(enemy, 2);
            MainWindow.game.GameCanvas?.Children.Add(enemy);

            return enemy;
        }

        //method is called each tick in GameEngine, checks if enemy shoots, checks if Enemy is at any border and moves it otherwise
        //bool is returned to let GameEngine know if Enemy is at left border to delete
        //shooting happens every 100 Ticks after spawn, but as enemies are (mostly) spawned at random times
        //enemy shooting pattern is mostly random
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
            Canvas.SetLeft(enemy, Canvas.GetLeft(enemy) + enemyXSpeed);
            Canvas.SetTop(enemy, Canvas.GetTop(enemy) + enemyYSpeed);
        }

        //method updates Y Speed if top or bottom border are reached and removes Enemy if left border is reached
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

        //get hitbox for collision checks
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
