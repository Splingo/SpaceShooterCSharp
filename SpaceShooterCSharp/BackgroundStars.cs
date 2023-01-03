using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Documents;

namespace SpaceShooterCSharp
{

    internal class BackgroundStar
    {
        private int starPosX;
        private int starPosY;
        private int starSize;
        private Rectangle? star;

        public BackgroundStar()
        { }

        public BackgroundStar Spawn()
        {
            PositionStar(false);
            return this;
        }

        public void Update()
        {
            if (Canvas.GetLeft(this.star) - 2 <= 5)
            {
                MainWindow.game.gameCanvas.Children.Remove(star);
                PositionStar(true);
            }
            else

                Canvas.SetLeft(this.star, Canvas.GetLeft(this.star) - 2);
        }
        public void PositionStar(bool starAtCanvasEnd)
        {
            RandomizeStats(starAtCanvasEnd);
            star = new Rectangle
            {
                Height = starSize,
                Width = starSize,
                Fill = Brushes.DarkSlateGray
            };
            Canvas.SetLeft(star, starPosX);
            Canvas.SetTop(star, starPosY);
            MainWindow.game.gameCanvas?.Children.Add(star);
        }

        public void RandomizeStats(bool starAtCanvasEnd)
        {
            Random random = new Random();
            starSize = random.Next(3, 9);

            if (!starAtCanvasEnd)
                starPosX = random.Next(0, Constants.windowWidth + 1);
            else if (starAtCanvasEnd)
                starPosX = Constants.windowWidth;

            starPosY = random.Next(0, Constants.windowHeight + 1);

        }

    }
}
