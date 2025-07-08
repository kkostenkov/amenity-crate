using UnityEngine;

namespace Amenity.Sound
{
    public interface ISoundManager
    {
        int MusicLayers { get; }
        void SetMusicLayerVolume(int layer, float volume);
        float GetMusicLayerVolume(int layer);
        bool IsLayerPlaying(int layer);
        bool IsMusicPlaying(AudioClip clip);
        void PlayMusic(AudioClip audioClip, int layer = 0);
        void PlayMusic(AudioData data, int layer = 0);
        void StopMusic(int layer = 0);
        void PlaySfx(AudioClip audioClip, bool loop = false);
        void PlaySfx(AudioData data);
        void StopSfx(IAudioData data);
        void SetGlobalVolume(float value);
        void Mute();
        void Unmute();
        void DeleteLayerAt(int index);
    }
}