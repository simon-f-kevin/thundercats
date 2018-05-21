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
        private Song currentSongPlaying;
        public bool IsPlaying;
        private double lastTotalSeconds;




        #region Thread-safe singleton - use "AudioManager.Instance" to access
        private static readonly Lazy<AudioManager> lazy = new Lazy<AudioManager>(() => new AudioManager(), true);

        private AudioManager()
        {
            MediaPlayer.Volume = 0.3f;
        }

        public static AudioManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        /// <summary>
        /// Starts the audio manager and plays the songs in the queue
        /// </summary>
        public void StartAudioManager()
        {
            currentSongPlaying = songQueue.Dequeue();
            IsPlaying = true;
            MediaPlayer.Play(currentSongPlaying);
            songQueue.Enqueue(currentSongPlaying);
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        /// <summary>
        /// Plays a sound effect or song once
        /// </summary>
        /// <param name="name"></param>
        public void PlaySound(string name)
        {
            MediaPlayer.Play(AssetManager.Instance.GetContent<Song>(name));
        }

        /// <summary>
        /// Pauses the music
        /// </summary>
        /// <param name="gameTime"></param>
        public void Pause(GameTime gameTime)
        {
            if (Math.Abs(gameTime.TotalGameTime.TotalSeconds - lastTotalSeconds) > 0.5)
            {
                if (MediaPlayer.State == MediaState.Paused && IsPlaying == false)
                {
                    Resume();
                }
                else
                {
                    MediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes from paused state
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Enqueues songs to be played on repeat
        /// </summary>
        /// <param name="names"></param>
        public void EnqueueSongs(params string[] names)
        {
            if(songQueue == null) songQueue = new Queue<Song>();

            foreach (var songname in names)
            {
                songQueue.Enqueue(AssetManager.Instance.GetContent<Song>(songname));
            }
        }

        /// <summary>
        /// Plays the next song in the queue
        /// </summary>
        /// <param name="gameTime"></param>
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
            IsPlaying = false;
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
