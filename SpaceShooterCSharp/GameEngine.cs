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
using System.Linq;

namespace SpaceShooterCSharp
{
    public class GameEngine
    {
        #region Variables
        //public variables
        public int ScoreAsInt { get; set; }
        public int GameSpeed { get; internal set; }
        public Player? Player { get; internal set; }

        public Canvas? GameCanvas;

        //private variables
        private bool gameStarted;
        private bool playerDamaged;

        private List<Enemy> enemyList = new List<Enemy>();
        private List<Enemy> enemiesToRemove = new List<Enemy>();
        private List<Bullet> bulletList = new List<Bullet>();
        private List<Bullet> bulletsToRemove = new List<Bullet>();

        private int enemySpawnRandomizer;

        private DispatcherTimer? gameTimer;

        private Background? background;

        private SoundEngine? soundEngine;

        private ScoreHealthDisplay? scoreHealthDisplay;

        private MenuHelper menu;
        #endregion

        #region SetResetGame
        //initalize game variables and show HelpMenu
        public void InitializeGame(Canvas gameCanvas)
        {
            gameTimer = new();
            GameCanvas = gameCanvas;
            Player = new();
            background = new();
            soundEngine = new();
            scoreHealthDisplay = new();
            menu = new();


            gameTimer.Interval = TimeSpan.FromMilliseconds(5);
            gameTimer.Tick += new EventHandler(GameTimerEvent);

            menu.ShowHelpMenu();

        }


        //when game is restarted, resets player and gamestate
        // start timer and plays background sound otherwise
        public void StartGame(bool restartGame)
        {
            if (restartGame)
            {
                Player?.ResetPlayer();
                ResetGame();
            }
            if (GameCanvas != null)
                Player?.SetResetPlayerPos(GameCanvas);

            gameStarted = true;
            soundEngine?.PlaySound(SoundEngine.ESounds.StartGame);
            gameTimer?.Start();
            soundEngine?.PlaySound(SoundEngine.ESounds.BackgroundMusic);
            ScoreAsInt = 0;
            SetResetEnemySpawnRandomizer();
        }

        //method clears background, resets and re-adds various objects creates to restart the game
        //method clears Enemy and Bullet objects from lists
        public void ResetGame()
        {
            GameCanvas?.Children.Clear();
            background = new();
            soundEngine?.StopSound();
            scoreHealthDisplay?.AddDisplaysToCanvas();
            enemyList.Clear();
            bulletList.Clear();
        }

        #endregion

        //Events that are called each tick when Timer is running
        private void GameTimerEvent(object? sender, EventArgs e)
        {
            EnemySpawner();

            background?.UpdateBG();
            Player?.UpdatePlayer();

            BulletUpdateHelper();
            BulletRemover();

            EnemyUpdateHelper();
            EnemyRemover();

            scoreHealthDisplay?.UpdateDisplays();

            if (playerDamaged)
            {
                PlayerDamagedResetCanvas();
                playerDamaged = false;
            }
        }

        //simple method to spawn more enemies
        //enemy count is current score / 10, min 1.
        //enemy speed is determined by similar calculation 
        //more points = faster enemies, but caps at 3x initial speed (only X speed is changed)
        private void EnemySpawner()
        {
            if (enemyList.Count <= ScoreAsInt / 10)
                enemySpawnRandomizer++;
            if (enemySpawnRandomizer % 50 == 0)
            {
                int enemySpeed = 0;
                switch (ScoreAsInt / 20)
                {
                    case 0:
                        enemySpeed = -1;
                        break;
                    case 1:
                        enemySpeed = -2;
                        break;
                    default:
                        enemySpeed = -3;
                        break;
                }
                enemyList.Add(new Enemy(enemySpeed));
                SetResetEnemySpawnRandomizer();
            }
        }


        private void SetResetEnemySpawnRandomizer()
        {
            enemySpawnRandomizer = new Random().Next(5, 25);
        }

        //helpermethod that calls the Update method for each bullet for movement
        // and checks the bullet origin and calls specific collision check methods
        private void BulletUpdateHelper()
        {
            foreach (Bullet bullet in bulletList)
            {
                bullet.Update();

                if (bullet.CheckBulletTag() == "playerBullet")
                    CheckIfEnemyHit(bullet);
                else
                    CheckIfPlayerHitByBullet(bullet);
            }
        }

        //methods gets hitboxes from enemy and bullet and checks if they intersect
        //calls EnemyKilled method if true        
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
        //methods gets hitboxes from player and bullet and checks if they intersect
        //calls PlayerDamaged method if true        
        private void CheckIfPlayerHitByBullet(Bullet bullet)
        {
            if (Player != null)
                if (bullet.GetBulletHitbox().IntersectsWith(Player.GetPlayerHitbox()))
                {
                    PlayerDamaged();
                }
        }

        //method increases score, plays killsound, removes bullet and enemy from canvas and marks objects 
        //for later removal
        private void EnemyKilled(Bullet bullet, Enemy enemy)
        {
            ScoreAsInt++;
            soundEngine?.PlaySound(SoundEngine.ESounds.EnemyKilled);
            bullet.deleteBullet();
            enemy.deleteEnemy();
            enemiesToRemove.Add(enemy);
            bulletsToRemove.Add(bullet);
        }

        //method decreased player HP, plays sound, checks if Health left and sets playerDamaged flag or stops game and shows 
        //game lost Menu
        private void PlayerDamaged()
        {
            if (Player != null)
            {
                Player.Health--;

                soundEngine?.PlaySound(SoundEngine.ESounds.PlayerDamaged);
                if (Player.Health == 0)
                {
                    gameStarted = false;
                    gameTimer?.Stop();
                    menu.ShowGameLostMenu();
                }
                else
                {
                    playerDamaged = true;
                }
            }
        }

        //helpermethod that calls the Update method for each enemy for movement
        //marks enemy to remove if enemy is at left border
        //calls CheckIfPlayerHitByEnemy method for enemy-player collision detection
        private void EnemyUpdateHelper()
        {
            foreach (Enemy enemy in enemyList)
            {
                if (!enemy.Update())
                    enemiesToRemove.Add(enemy);
                else
                    CheckIfPlayerHitByEnemy(enemy);
            }
        }

        //methods gets hitboxes from player and enemy and checks if they intersect
        //calls PlayerDamaged method if true
        private void CheckIfPlayerHitByEnemy(Enemy enemy)
        {
            if (Player != null)
                if (enemy.GetEnemyHitbox().IntersectsWith(Player.GetPlayerHitbox()))
                {
                    PlayerDamaged();
                }
        }

        //method is called if player was damaged
        //resets player position and deletes all enemy and bullet visuals from canvas
        //enemy and bulletlist are cleared so that garbage collector deletes remaining objects
        private void PlayerDamagedResetCanvas()
        {
            if (GameCanvas != null)
            {
                Player?.SetResetPlayerPos(GameCanvas);
            }
            int temp = enemyList.Count;
            for (int i = 0; i < temp; i++)
            {
                enemyList[i].deleteEnemy();
            }
            temp = bulletList.Count;
            for (int i = 0; i < temp; i++)
            {
                bulletList[i].deleteBullet();
            }
            enemyList.Clear();
            bulletList.Clear();
        }

        //shooting method used for player and enemies
        //direction can be used to differentiate between enemy and player
        //bullets are added to a list for easier tracking and managing
        internal void Shoot(int direction, Rectangle origin)
        {
            bulletList?.Add(new Bullet(direction, origin));
            if (direction > 0)
                soundEngine?.PlaySound(SoundEngine.ESounds.PlayerShot);
            else
                soundEngine?.PlaySound(SoundEngine.ESounds.EnemyShot);
        }

        #region RemoverHelpers
        //remover helper. visuals are deleted and objects are removed from list.
        //garbage collector removes remaining objects
        private void EnemyRemover()
        {
            foreach (Enemy enemy in enemiesToRemove)
            {
                enemy.deleteEnemy();
                enemyList.Remove(enemy);
            }
            enemiesToRemove.Clear();
        }

        //remover helper. visuals are deleted and objects are removed from list.
        //garbage collector removes remaining objects
        private void BulletRemover()
        {
            foreach (Bullet bullet in bulletsToRemove)
            {
                bullet.deleteBullet();
                bulletList.Remove(bullet);
            }
            bulletsToRemove.Clear();
        }

        #endregion

        #region KeyEvents
        //Key Events are handed over to player if game is started. otherwise nothing happens
        public void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (gameStarted)
                Player?.OnKeyDown(sender, e);
        }
        public void OnKeyUp(object? sender, KeyEventArgs e)
        {
            if (gameStarted)
                Player?.OnKeyUp(sender, e);
        }
        #endregion

        #region unused

        //methods to pause and unpause the game -> eg while displaying Menu
        public void PauseGame()
        {
            gameTimer?.Stop();
        }

        public void UnpauseGame()
        {
            gameTimer?.Start();
        }
        #endregion
    }
}

