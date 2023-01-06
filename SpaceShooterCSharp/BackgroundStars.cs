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

        //Update method checks if a star reached the left side of Canvas and deletes it and calls PositionStar method to
        //respawn it or moves it further left
        public void Update()
        {
            if (Canvas.GetLeft(star) - 2 <= 5)
            {
                MainWindow.game.GameCanvas?.Children.Remove(star);
                PositionStar(true);
            }
            else
                Canvas.SetLeft(star, Canvas.GetLeft(star) - 1);
        }

        //PositionStar gets random stats from RandomizeStats method for size, x/y positions and spawns a star on the canvas
        public BackgroundStar PositionStar(bool starAtCanvasEnd)
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
            MainWindow.game.GameCanvas?.Children.Add(star);
            return this;
        }


        //RandomizeStats randomizes star size and x/y positions during first spawning.
        //x position is later set to WindowWidth to spawn them on the right side of Window and only size and Y-position is random
        public void RandomizeStats(bool starAtCanvasEnd)
        {
            Random random = new Random();
            starSize = random.Next(3, 9);

            if (!starAtCanvasEnd)
                starPosX = random.Next(0, Constants.WindowWidth + 1);
            else
                starPosX = Constants.WindowWidth;

            starPosY = random.Next(0, Constants.WindowHeight + 1);

        }

    }
}
