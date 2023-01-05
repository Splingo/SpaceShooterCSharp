using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Automation;
using System.Media;
using System.Windows.Shapes;

namespace SpaceShooterCSharp
{
    public class GameEngine
    {
        private int scoreAsInt = 0;
        private bool gameStarted;

        private static int enemySpawnTimer = 0;

        private List<Enemy> enemyList = new List<Enemy>();
        private List<Enemy> enemiesToRemove = new List<Enemy>();
        private List<Bullet> playerBulletList = new List<Bullet>();
        private List<Bullet> bulletsToRemove = new List<Bullet>();


        private DispatcherTimer gameTimer = new();

        private Background? background;

        private SoundEngine? soundEngine;

        private Player? player;

        public Canvas? GameCanvas;

        public int GameSpeed { get; internal set; }

        public void InitializeGame(Canvas gameCanvas)
        {
            GameCanvas = gameCanvas;
            player = new Player(gameCanvas);
            background = new Background();
            soundEngine = new SoundEngine();

            gameTimer.Interval = TimeSpan.FromMilliseconds(5);
            gameTimer.Tick += new EventHandler(GameTimerEvent);

            GameSpeed = 1;

            Window_HelpMenu help = new Window_HelpMenu();
            help.Show();

        }

        public void StartGame()
        {
            soundEngine?.PlaySound(SoundEngine.ESounds.StartGame);
            gameStarted = true;
            gameTimer.Start();
            soundEngine?.PlaySound(SoundEngine.ESounds.BackgroundMusic);

        }

        public void PauseGame()
        {
            gameTimer.Stop();
        }

        public void UnpauseGame()
        {
            gameTimer.Start();
        }


        private void GameTimerEvent(object? sender, EventArgs e)
        {
            EnemySpawner();
            background?.UpdateBG();
            player?.UpdatePlayer();
            foreach (Bullet bullet in playerBulletList)
            {
                bullet.Update();

                foreach (Enemy enemy in enemyList)
                {
                    if (bullet.GetBulletHitbox().IntersectsWith(enemy.GetEnemyHitbox()))
                    {
                        bullet.deleteBullet();
                        enemy.deleteEnemy();
                        enemiesToRemove.Add(enemy);
                        bulletsToRemove.Add(bullet);
                    }
                }

            }
            foreach (Enemy enemy in enemyList)
            {
                if (!enemy.Update())
                { enemiesToRemove.Add(enemy); }
            }
            EnemyRemover();
            BulletRemover();
        }

        private void EnemyRemover()
        {
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemyList.Remove(enemy);
            }
            enemiesToRemove.Clear();
        }

        private void BulletRemover()
        {
            foreach (Bullet bullet in bulletsToRemove)
            {
                playerBulletList.Remove(bullet);
            }
            bulletsToRemove.Clear();
        }

        private void EnemySpawner()
        {
            enemySpawnTimer--;
            if (enemyList.Count <= enemySpawnTimer / -5000)
                enemyList.Add(new Enemy(enemySpawnTimer / 5000 - 1));
        }

        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (gameStarted)
                player?.OnKeyDown(sender, e);
        }
        public void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (gameStarted)
                player?.OnKeyUp(sender, e);
        }

        internal void Shoot(int direction, Rectangle origin)
        {
            playerBulletList?.Add(new Bullet(direction, origin));
            if (direction > 0)
                soundEngine?.PlaySound(SoundEngine.ESounds.PlayerShot);
            else
                soundEngine?.PlaySound(SoundEngine.ESounds.EnemyShot);


        }
    }
}

