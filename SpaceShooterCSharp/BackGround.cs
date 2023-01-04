﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpaceShooterCSharp
{
    internal class Background
    {
        private List<BackgroundStar> BackgroundStars = new List<BackgroundStar>();

        public Background()
        {
            for (int i = 0; i < 50; i++)
            {
                BackgroundStars.Add(new BackgroundStar().Spawn());
            }

        }

        internal void UpdateBG()
        {
            foreach (BackgroundStar star in BackgroundStars)
            {
                star.Update();
            }
        }
    }
}
