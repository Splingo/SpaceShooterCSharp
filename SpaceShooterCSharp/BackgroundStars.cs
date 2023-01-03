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
        private Rectangle star;

        public BackgroundStar(int posX, int posY)
        {
            Random random = new Random();
            starSize = random.Next(3, 9);
            this.starPosX = posX;
            this.starPosY = posY;
        }


        public BackgroundStar Spawn()
        {
            Rectangle star = new Rectangle
            {
                Height = starSize,
                Width = starSize,
                Fill = Brushes.DarkSlateGray
            };
            this.star = star;
            Canvas.SetLeft(star, starPosX);
            Canvas.SetTop(star, starPosY);
            MainWindow.game.gameCanvas?.Children.Add(star);
            return this;
        }

        public void Update()
        {
            if (Canvas.GetLeft(this.star) - 2 <= 0)
            {
            }
            else

                Canvas.SetLeft(this.star, Canvas.GetLeft(this.star) - 2);
        }

    }
}
