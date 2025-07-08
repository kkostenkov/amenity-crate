using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Amenity.Sound
{
    public partial class SoundManager : ISoundManager
    {
        private const int SfxSourceAmount = 10;
        
        private readonly IAudioSourceGenerator audioSourceGenerator;
        private readonly List<AudioSource> musicSources = new();
        private List<AudioSource> sfxSources;
        private int currentSfxSourceIndex;
        private float previousGlobalVolume = 1;

        private readonly Dictionary<IAudioData, AudioSource> loopedSounds = new();

        public static bool IsMuted => AudioListener.volume == 0;
        public int MusicLayers => this.musicSources.Count;
        public void Mute() => SetGlobalVolume(0);
        public void Unmute() => AudioListener.volume = this.previousGlobalVolume;

        public SoundManager(IAudioSourceGenerator audioSourceGenerator)
        {
            this.audioSourceGenerator = audioSourceGenerator;
        }

        public Task Initialize()
        {
            this.sfxSources = new List<AudioSource>(SfxSourceAmount);
            for (int i = 0; i < SfxSourceAmount; i++) {
                this.sfxSources.Add(this.audioSourceGenerator.Get());
                this.sfxSources[i].loop = false;
                this.sfxSources[i].playOnAwake = false;
            }

            return LoadMixer();
        }

        public void SetMusicLayerVolume(int layer, float volume)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            source.volume = volume;
        }

        public float GetMusicLayerVolume(int layer)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            return source.volume;
        }

        public bool IsLayerPlaying(int layer)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            return source.isPlaying;
        }

        public bool IsMusicPlaying(AudioClip clip)
        {
            foreach (AudioSource source in this.musicSources) {
                if (source.clip == clip && source.isPlaying) {
                    return true;
                }
            }

            return false;
        }

        public void PlayMusic(AudioClip audioClip, int layer = 0)
        {
            AudioData data = new(name: "Custom", loop: true, clips: new[] { audioClip });
            PlayMusic(data, layer);
        }

        public void PlayMusic(AudioData data, int layer = 0)
        {
            GetOrCreateMusicSource(layer, out AudioSource musicSource);
            musicSource.clip = data.Clip;
            musicSource.Play();
        }

        public void StopMusic(int layer = 0)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            source.Stop();
            source.clip = null;
        }

        public void PlaySfx(AudioClip audioClip, bool loop = false)
        {
            AudioData data = new(name: "Custom", loop, clips: new[] { audioClip });
            PlaySfx(data);
        }

        public void PlaySfx(AudioData data)
        {
            var source = GetAvailableSource();
            source.loop = data.Loop;
            source.clip = data.Clip;
            source.Play();

            if (data.Loop) {
                this.loopedSounds.Add(data, source);
            }
        }

        public void StopSfx(IAudioData data)
        {
            if (!this.loopedSounds.TryGetValue(data, out var source)) {
                return;
            }

            source.Stop();
            this.loopedSounds.Remove(data);
        }

        public void SetGlobalVolume(float value)
        {
            if (!IsMuted) {
                this.previousGlobalVolume = value;
            }

            AudioListener.volume = value;
        }

        public void DeleteLayerAt(int index)
        {
            List<AudioSource> musicSourcesCopy = this.musicSources;
            musicSourcesCopy[index].Stop();
            musicSourcesCopy[index].clip = null;
            musicSourcesCopy.RemoveAt(index);
        }

        private void GetOrCreateMusicSource(int layer, out AudioSource source)
        {
            bool isValid = layer >= 0 && layer < this.musicSources.Count;
            source = isValid ? this.musicSources[layer] : CreateLayer();
        }

        private AudioSource CreateLayer()
        {
            AudioSource source = this.audioSourceGenerator.Get();
            source.loop = true;
            source.playOnAwake = false;
            this.musicSources.Add(source);

            return source;
        }

        private AudioSource GetAvailableSource()
        {
            int checks = 0;
            IncrementIndex();
            while (this.sfxSources[this.currentSfxSourceIndex].loop) {
                IncrementIndex();
                checks++;
                if (checks >= SfxSourceAmount) {
                    return this.sfxSources[0];
                }
            }

            if (this.currentSfxSourceIndex >= SfxSourceAmount) {
                this.currentSfxSourceIndex = 0;
            }
            
            if (this.currentSfxSourceIndex < 0) {
                this.currentSfxSourceIndex = 0;
            }
            else if (this.currentSfxSourceIndex >= this.sfxSources.Count) {
                this.currentSfxSourceIndex = this.sfxSources.Count - 1;
            }

            return this.sfxSources[this.currentSfxSourceIndex];

            void IncrementIndex()
            {
                this.currentSfxSourceIndex++;
                if (this.currentSfxSourceIndex >= SfxSourceAmount) {
                    this.currentSfxSourceIndex = 0;
                }
            }
        }
    }
}