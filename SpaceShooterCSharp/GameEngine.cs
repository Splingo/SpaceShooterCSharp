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
        public int ScoreAsInt { get; set; }

        private bool gameStarted;

        private List<Enemy> enemyList = new List<Enemy>();
        private List<Enemy> enemiesToRemove = new List<Enemy>();
        private List<Bullet> bulletList = new List<Bullet>();
        private List<Bullet> bulletsToRemove = new List<Bullet>();


        private DispatcherTimer? gameTimer;

        private Background? background;

        private SoundEngine? soundEngine;

        public Player? player { get; internal set; }

        public Canvas? GameCanvas;

        private ScoreHealthDisplay? scoreHealthDisplay;

        public int GameSpeed { get; internal set; }

        public void InitializeGame(Canvas gameCanvas)
        {
            ScoreAsInt = 0;
            gameTimer = new();
            GameCanvas = gameCanvas;
            player = new(gameCanvas);
            background = new();
            soundEngine = new();
            scoreHealthDisplay = new();

            gameTimer.Interval = TimeSpan.FromMilliseconds(5);
            gameTimer.Tick += new EventHandler(GameTimerEvent);

            GameSpeed = 1;

            Window_HelpMenu help = new();
            help.Show();

        }

        public void StartGame(bool restartGame)
        {
            if (restartGame)
            {
                ResetGame();
            }
            gameStarted = true;
            soundEngine?.PlaySound(SoundEngine.ESounds.StartGame);
            gameTimer?.Start();
            soundEngine?.PlaySound(SoundEngine.ESounds.BackgroundMusic);
            scoreHealthDisplay?.GenerateDisplays();
        }

        public void ResetGame()
        {
            GameCanvas?.Children.Clear();
            ScoreAsInt = 0;
            if (GameCanvas != null)
                player = new Player(GameCanvas);
            background = new();
            soundEngine?.StopSound();
            soundEngine = new();
            scoreHealthDisplay = new();
            enemyList.Clear();
            bulletList.Clear();
        }


        public void PauseGame()
        {
            gameTimer?.Stop();
        }

        public void UnpauseGame()
        {
            gameTimer?.Start();
        }


        private void GameTimerEvent(object? sender, EventArgs e)
        {

            EnemySpawner();

            background?.UpdateBG();
            player?.UpdatePlayer();

            foreach (Bullet bullet in bulletList)
            {
                bullet.Update();

                if (bullet.CheckBulletTag() == "playerBullet")
                    CheckIfEnemyHit(bullet);
                else
                    CheckIfPlayerHit(bullet);

            }
            foreach (Enemy enemy in enemyList)
            {
                if (!enemy.Update())
                { enemiesToRemove.Add(enemy); }
            }
            EnemyRemover();
            BulletRemover();
            scoreHealthDisplay?.UpdateDisplays();

        }

        private void CheckIfEnemyHit(Bullet bullet)
        {
            foreach (Enemy enemy in enemyList)
            {
                if (bullet.GetBulletHitbox().IntersectsWith(enemy.GetEnemyHitbox()))
                {
                    EnemyKilled(bullet, enemy);
                }
            }
        }
        private void CheckIfPlayerHit(Bullet bullet)
        {
            if (player != null)
                if (bullet.GetBulletHitbox().IntersectsWith(player.GetPlayerHitbox()))
                {
                    PlayerDamaged(bullet);
                }

        }
        private void EnemyKilled(Bullet bullet, Enemy enemy)
        {
            ScoreAsInt++;
            soundEngine?.PlaySound(SoundEngine.ESounds.EnemyKilled);
            bullet.deleteBullet();
            enemy.deleteEnemy();
            enemiesToRemove.Add(enemy);
            bulletsToRemove.Add(bullet);
        }
        private void PlayerDamaged(Bullet bullet)
        {
            if (player != null)
            {
                player.Health--;

                soundEngine?.PlaySound(SoundEngine.ESounds.PlayerDamaged);
                bullet.deleteBullet();
                bulletsToRemove.Add(bullet);
                if (player.Health == 0)
                {
                    gameTimer?.Stop();
                    new Window_GameLostMenu().Show();
                }
            }
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
                bulletList.Remove(bullet);
            }
            bulletsToRemove.Clear();
        }

        private void EnemySpawner()
        {
            if (enemyList.Count <= ScoreAsInt / 10)
                enemyList.Add(new Enemy(ScoreAsInt / 10 - 1));
        }

        internal void Shoot(int direction, Rectangle origin)
        {
            bulletList?.Add(new Bullet(direction, origin));
            if (direction > 0)
                soundEngine?.PlaySound(SoundEngine.ESounds.PlayerShot);
            else
                soundEngine?.PlaySound(SoundEngine.ESounds.EnemyShot);
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

    }
}

