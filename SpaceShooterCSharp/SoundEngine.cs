using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static SpaceShooterCSharp.SoundEngine;

namespace SpaceShooterCSharp
{
    internal class SoundEngine
    {
        private readonly List<MediaPlayer> mediaPlayerList = new();
        public enum ESounds
        {
            BackgroundMusic,
            EnemyKilled,
            EnemyShot,
            PlayerDamaged,
            PlayerShot,
            StartGame

        }

        public void PlaySound(ESounds sound)
        {
            MediaPlayer mp = new();
            switch (sound)
            {
                case ESounds.BackgroundMusic:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/backgroundmusic.wav"));
                    mp.MediaEnded += PlayerLoop;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;

                case ESounds.EnemyKilled:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/enemykilled.wav"));
                    mp.MediaEnded += PlayerMediaEnded;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;
                case ESounds.EnemyShot:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/enemyshot.wav"));
                    mp.MediaEnded += PlayerMediaEnded;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;
                case ESounds.PlayerDamaged:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/playerdamaged.wav"));
                    mp.MediaEnded += PlayerMediaEnded;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;
                case ESounds.PlayerShot:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/playershot.wav"));
                    mp.MediaEnded += PlayerMediaEnded;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;
                case ESounds.StartGame:
                    mp.Open(new Uri("pack://siteoforigin:,,,/sounds/startgame.wav"));
                    mp.MediaEnded += PlayerMediaEnded;
                    mediaPlayerList.Add(mp);
                    mp.Play();
                    break;
            }

        }

        private void PlayerMediaEnded(object? sender, EventArgs e)
        {
            if (sender is MediaPlayer)
            {
                mediaPlayerList.Remove((MediaPlayer)sender);
            }
        }


        private void PlayerLoop(object? sender, EventArgs e)
        {
            if (sender is MediaPlayer)
            {
                MediaPlayer media = (MediaPlayer)sender;
                media.Position = TimeSpan.Zero;
                media.Play();
            }
        }

        public void StopSound()
        {
            foreach (var mp in mediaPlayerList)
            {
                mp.Stop();
            }
        }
    }
}
