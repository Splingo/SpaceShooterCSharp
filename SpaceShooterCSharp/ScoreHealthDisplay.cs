using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SpaceShooterCSharp
{
    internal class ScoreHealthDisplay
    {
        private TextBlock scoreTextBlock = new();
        private TextBlock healthTextBlock = new();

        public ScoreHealthDisplay()
        {
            AddDisplaysToCanvas();
        }

        //sets layout for scoredisplay and adds it (without content) to canvas
        private void CreateScoreDisplay()
        {
            scoreTextBlock.FontSize = 20;
            scoreTextBlock.Background = Brushes.DarkGray;
            scoreTextBlock.Foreground = Brushes.DarkGreen;
            scoreTextBlock.FontFamily = new FontFamily("Symtext");
            scoreTextBlock.FontWeight = FontWeights.Bold;
            MainWindow.game.GameCanvas?.Children.Add(scoreTextBlock);
        }
        //sets layout for healthdisplay and adds it (without content) to canvas
        private void CreateHealthDisplay()
        {
            healthTextBlock.FontSize = 20;
            healthTextBlock.Background = Brushes.DarkGray;
            healthTextBlock.Foreground = Brushes.DarkGreen;
            healthTextBlock.FontFamily = new FontFamily("Symtext");
            healthTextBlock.FontWeight = FontWeights.Bold;
            MainWindow.game.GameCanvas?.Children.Add(healthTextBlock);
        }

        //sets or resets the displays
        public void AddDisplaysToCanvas()
        {
            CreateScoreDisplay();
            CreateHealthDisplay();
            Panel.SetZIndex(scoreTextBlock, 1);
            Panel.SetZIndex(healthTextBlock, 1);
        }

        //method called each tick from GameEngine
        public void UpdateDisplays()
        {
            UpdateScoreDisplay();
            UpdateHealthDisplay();
        }

        //updates content of score display
        private void UpdateScoreDisplay()
        {
            scoreTextBlock.Text = " YOUR SCORE: " + MainWindow.game.ScoreAsInt.ToString() + " ";
            Canvas.SetLeft(scoreTextBlock, Constants.WindowWidth - scoreTextBlock.ActualWidth - 10);
            Canvas.SetTop(scoreTextBlock, Constants.WindowHeight - scoreTextBlock.ActualHeight - 10);
        }

        //updates content of health display
        private void UpdateHealthDisplay()
        {
            string healthText = " HP: ";
            for (int i = 0; i < MainWindow.game.player?.Health; i++)
            {
                healthText += Convert.ToChar(0x00002665);
            }
            healthTextBlock.Text = healthText;
            Canvas.SetLeft(healthTextBlock, 10);
            Canvas.SetTop(healthTextBlock, 10);
        }
    }
}
