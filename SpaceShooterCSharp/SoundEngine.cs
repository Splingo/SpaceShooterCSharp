using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static SpaceShooterCSharp.SoundEngine;

namespace SpaceShooterCSharp
{
    internal class SoundEngine
    {
        //mediaPlayerList is needed to easily track and manage Mediaplayers currently playing sounds
        private readonly List<MediaPlayer> mediaPlayerList = new();

        //enum for each sound Origin
        public enum ESounds
        {
            BackgroundMusic,
            EnemyKilled,
            EnemyShot,
            PlayerDamaged,
            PlayerShot,
            StartGame
        }

        //method creates a new mediaplayer, opens a sound file and adds it to an internal list of all Mediaplayers
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

        //eventhandler when mediaplayer has finished playing the sound. removes the player from list
        private void PlayerMediaEnded(object? sender, EventArgs e)
        {
            if (sender is MediaPlayer)
            {
                mediaPlayerList.Remove((MediaPlayer)sender);
            }
        }

        //eventhandler when mediaplayer has finished playing the sound. creates new mediaplayer and restarts sound
        //used for backgroundmusic
        private void PlayerLoop(object? sender, EventArgs e)
        {
            if (sender is MediaPlayer)
            {
                MediaPlayer media = (MediaPlayer)sender;
                media.Position = TimeSpan.Zero;
                media.Play();
            }
        }

        //method stops all Sounds currently playing and clears the mediaPlayerList
        public void StopSound()
        {
            foreach (var mp in mediaPlayerList)
            {
                mp.Stop();
            }
            mediaPlayerList.Clear();
        }
    }
}
