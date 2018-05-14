using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Game_Engine.Managers
{
    public class AudioManager
    {
        private static AudioManager _instance;
        private Queue<Song> songQueue;
        private Dictionary<string, Song> soundDictionary;
        private Song currentSongPlaying;
        private bool isPlaying;
        private double lastTotalSeconds;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    MediaPlayer.Volume = 0.3f;
                    _instance = new AudioManager();
                }
                return _instance;
            }
        }

        public void StartAudioManager()
        {
            currentSongPlaying = songQueue.Dequeue();
            isPlaying = true;
            MediaPlayer.Play(currentSongPlaying);
            songQueue.Enqueue(currentSongPlaying);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        public void PlaySound(string name)
        {
            if(soundDictionary.TryGetValue(name, out var song)) MediaPlayer.Play(song);
        }

        public void Pause(GameTime gameTime)
        {
            if (Math.Abs(gameTime.TotalGameTime.TotalSeconds - lastTotalSeconds) > 0.5)
            {
                if (MediaPlayer.State == MediaState.Paused && isPlaying == false)
                {
                    Resume();
                }
                else
                {
                    MediaPlayer.Pause();
                }
            }
        }

        public void Resume()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Adds new audio to the AudioManagers' dictionary
        /// </summary>
        /// <param name="name"></param>
        /// <param name="song"></param>
        public void AddNewAudio(string name, Song song)
        {
            if (soundDictionary == null)
            {
                soundDictionary = new Dictionary<string, Song>();
            }
            soundDictionary.Add(name, song);
        }

        /// <summary>
        /// Enqueues songs to be played as music
        /// </summary>
        /// <param name="names"></param>
        public void EnqueueSongs(params string[] names)
        {
            if(songQueue == null) songQueue = new Queue<Song>();

            foreach (var songname in names)
            {
                if (soundDictionary.TryGetValue(songname, out var song))
                {
                    songQueue.Enqueue(song);
                }
            }
        }

        public void PlayNextInQueue(GameTime gameTime)
        {
            if (Math.Abs(gameTime.TotalGameTime.TotalSeconds - lastTotalSeconds) > 0.5)
            {
                StartAudioManager();
                lastTotalSeconds = gameTime.TotalGameTime.TotalSeconds;
            }
        }

        public void ClearSongs()
        {
            isPlaying = false;
            MediaPlayer.Stop();
            songQueue?.Clear(); //thank you resharper :)
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                currentSongPlaying = songQueue.Dequeue();
                MediaPlayer.Play(currentSongPlaying);
                songQueue.Enqueue(currentSongPlaying);
            }
        }
    }
}
