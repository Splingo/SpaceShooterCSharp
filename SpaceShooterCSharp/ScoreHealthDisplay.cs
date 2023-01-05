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

        public void GenerateDisplays()
        {
            AddScoreDisplay();
            AddHealthDisplay();
            Panel.SetZIndex(scoreTextBlock, 1);
            Panel.SetZIndex(healthTextBlock, 1);

        }

        private void AddScoreDisplay()
        {
            scoreTextBlock.FontSize = 20;
            scoreTextBlock.Background = Brushes.DarkGray;
            scoreTextBlock.Foreground = Brushes.DarkGreen;
            scoreTextBlock.FontFamily = new FontFamily("Symtext");
            scoreTextBlock.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(scoreTextBlock, Constants.WindowWidth - scoreTextBlock.ActualWidth - 10);
            Canvas.SetTop(scoreTextBlock, Constants.WindowHeight - scoreTextBlock.ActualHeight - 10);

            MainWindow.game.GameCanvas?.Children.Add(scoreTextBlock);
        }

        private void AddHealthDisplay()
        {
            healthTextBlock.FontSize = 20;
            healthTextBlock.Background = Brushes.DarkGray;
            healthTextBlock.Foreground = Brushes.DarkGreen;
            healthTextBlock.FontFamily = new FontFamily("Symtext");
            healthTextBlock.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(healthTextBlock, 10);
            Canvas.SetTop(healthTextBlock, 10);

            MainWindow.game.GameCanvas?.Children.Add(healthTextBlock);
        }


        public void UpdateDisplays()
        {
            UpdateScoreDisplay();

            UpdateHealthDisplay();
        }
        private void UpdateScoreDisplay()
        {
            scoreTextBlock.Text = " YOUR SCORE: " + MainWindow.game.ScoreAsInt.ToString() + " ";
            Canvas.SetLeft(scoreTextBlock, Constants.WindowWidth - scoreTextBlock.ActualWidth - 10);
            Canvas.SetTop(scoreTextBlock, Constants.WindowHeight - scoreTextBlock.ActualHeight - 10);
        }

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
