using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;

namespace SpaceShooterCSharp
{
    public class GameEngine
    {
        private int scoreAsInt = 0;
        private bool gameStarted;

        private List<Enemy> enemyList = new List<Enemy>();
        private List<Bullet> bulletList = new List<Bullet>();

        private DispatcherTimer gameTimer = new();

        private Background? background;

        private Player? player;

        public Canvas? gameCanvas;

        public void InitializeGame(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            player = new Player();
            background = new Background(false);


            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += new EventHandler(GameTimerEvent);
            gameTimer.Start();

        }

        private void GameTimerEvent(object? sender, EventArgs e)
        {
            background?.UpdateBG();
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

