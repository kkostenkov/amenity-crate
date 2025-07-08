using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace Amenity.Sound
{
    public partial class SoundManager : IBundledSoundsManager, IDisposable, IVolumeController
    {
        private const string AudioMixerAddressableKey = "AudioMixer";
        private const string MusicMixerGroupName = "Music";
        private const string SfxMixerGroupName = "SFX";
        private const string SfxVolumeParameter = "SfxVolume";
        private const string MusicVolumeParameter = "MusicVolume";

        private Dictionary<AssetReferenceT<AudioClip>, AsyncOperationHandle<AudioClip>> clipLoadingHandles = new();

        private AsyncOperationHandle<AudioMixer> mixerHandle;

        private AudioMixer mainMixer;
        private AudioMixerGroup musicMixerGroup;
        private AudioMixerGroup sfxMixerGroup;

        private readonly Random random = new();

        public async Task LoadMixer()
        {
            this.mixerHandle = Addressables.LoadAssetAsync<AudioMixer>(AudioMixerAddressableKey);
            this.mainMixer = await this.mixerHandle.Task;
            this.musicMixerGroup = this.mainMixer.FindMatchingGroups(MusicMixerGroupName)[0];
            this.sfxMixerGroup = this.mainMixer.FindMatchingGroups(SfxMixerGroupName)[0];
        }

        public async Task PlayMusic(BundledAudioData data, int layer = 0)
        {
            var clipReference = data.ClipReference;
            var clip = await LoadAudioClip(clipReference);

            if (clip == null) {
                throw new MissingReferenceException($"Failed to load clip from {data.Identifier}");
            }

            GetOrCreateMusicSource(layer, out var musicSource);
            musicSource.outputAudioMixerGroup = this.musicMixerGroup;
            musicSource.clip = clip;
            musicSource.Play();
        }

        public async Task PlaySfx(BundledAudioData data)
        {
            var clipReference = data.ClipReference;
            var clip = await LoadAudioClip(clipReference);

            if (clip == null) {
                throw new MissingReferenceException($"Failed to load clip from {data.Identifier}");
            }

            var source = GetAvailableSource();
            source.outputAudioMixerGroup = this.sfxMixerGroup;
            source.loop = data.Loop;
            source.clip = clip;
            source.pitch = data.GetPitch(this.random);
            source.Play();

            if (data.Loop) {
                this.loopedSounds.Add(data, source);
            }
        }

        private async Task<AudioClip> LoadAudioClip(AssetReferenceT<AudioClip> clipReference)
        {
            AudioClip clip;
            if (this.clipLoadingHandles.TryGetValue(clipReference, out var cachedHandle)) {
                clip = await cachedHandle.Task;
            }
            else {
                var handle = Addressables.LoadAssetAsync<AudioClip>(clipReference);
                this.clipLoadingHandles.Add(clipReference, handle);
                clip = await handle.Task;
            }

            return clip;
        }

        private void ReleaseAndClear()
        {
            foreach (var handle in this.clipLoadingHandles.Values) {
                Addressables.Release(handle);
                Debug.Log("Release audio clip setting");
            }

            this.clipLoadingHandles?.Clear();
            Addressables.Release(this.mixerHandle);
        }

        public void Dispose()
        {
            ReleaseAndClear();
        }

        public void SetSfxVolume(float sliderValue)
        {
            this.mainMixer.SetFloat(SfxVolumeParameter, MapToRange(sliderValue));
        }

        public void SetMusicVolume(float sliderValue)
        {
            this.mainMixer.SetFloat(MusicVolumeParameter, MapToRange(sliderValue));
        }
        
        private float MapToRange(float value)
        {
            const float fromMin = 0f;
            const float fromMax = 1f;
            const float toMin = -80f;
            const float toMax = 0f;

            value = Math.Clamp(value, fromMin, fromMax);

            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }
    }
}