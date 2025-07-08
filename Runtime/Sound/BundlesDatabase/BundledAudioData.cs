using System;
using Amenity.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Amenity.Sound
{
    [Serializable]
    public class BundledAudioData : IAudioData
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private AssetReferenceT<AudioClip>[] clips;

        [SerializeField]
        [MinMaxSlider(-0.5f, 1.5f)]
        private Vector2 pitchRange = Vector2.one;

        public string Identifier => this.name;

        public bool Loop => this.loop;

        public AssetReferenceT<AudioClip>[] Clips => this.clips;
        public AssetReferenceT<AudioClip> ClipReference => this.clips[Random.Range(0, this.clips.Length)];

        public float GetPitch(System.Random random)
        {
            return (float)GetRandomNumber(random, this.pitchRange.x, this.pitchRange.y);
        }

        private double GetRandomNumber(System.Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}