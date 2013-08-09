using System;
using System.Collections.Generic;
using DNT.Engine.Core.Validation;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace DNT.Engine.Core.Audio
{
    public class SoundManager
    {
        private readonly ContentManager _contentManager;

        public SoundManager(ContentManager contentManager)
        {
            Verify.That(contentManager).Named("contentManager").IsNotNull();
            _contentManager = contentManager;

            _soundEffects = new Dictionary<String, SoundEffect>();
            _songs = new Dictionary<String, Song>();
        }

        private readonly IDictionary<String, SoundEffect> _soundEffects;
        private readonly IDictionary<String, Song> _songs;

        public void AddSoundEffect(String assetName)
        {
            Verify.That(assetName).Named("assetName").IsNotNull();
            _soundEffects.Add(assetName, _contentManager.Load<SoundEffect>(assetName));
        }

        public void AddSong(String assetName)
        {
            Verify.That(assetName).Named("assetName").IsNotNull();
            _songs.Add(assetName, _contentManager.Load<Song>(assetName));
        }

        public void PlaySoundEffect(String assetName)
        {
            SoundEffect soundEffect;

            if (_soundEffects.TryGetValue(assetName, out soundEffect))
                soundEffect.Play();
        }

        public void PlaySoundEffect(String assetName, Single volume, Single pitch, Single pan)
        {
            SoundEffect soundEffect;

            if (_soundEffects.TryGetValue(assetName, out soundEffect))
                soundEffect.Play(volume, pitch, pan);
        }

        public void PlaySong(String songName)
        {
            PlaySong(songName, 1.0f, true);
        }

        public void PlaySong(String songName, Single volume, Boolean loop)
        {
            if (!MediaPlayer.GameHasControl)
                return;

            Song song;

            if (!_songs.TryGetValue(songName, out song))
                return;

            MediaPlayer.Volume = volume;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(song);
        }

        public static void PauseMusic()
        {
            if (MediaPlayer.GameHasControl)
                MediaPlayer.Pause();
        }

        public static void ResumeMusic()
        {
            if (MediaPlayer.GameHasControl)
                MediaPlayer.Resume();
        }

        public static void StopMusic()
        {
            if (MediaPlayer.GameHasControl)
                MediaPlayer.Stop();
        }
    }
}