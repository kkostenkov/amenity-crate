using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Amenity.Sound
{
    [Serializable]
    public class AudioData : IAudioData
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private bool loop;

        [SerializeField]
        private AudioClip[] clips;

        public string Identifier {
            get => this.name;
            set => this.name = value;
        }

        public bool Loop {
            get => this.loop;
            set => this.loop = value;
        }

        public AudioClip[] Clips => this.clips;
        public AudioClip Clip => this.clips[Random.Range(0, this.clips.Length)];

        public AudioData(string name, bool loop, AudioClip[] clips)
        {
            this.name = name;
            this.loop = loop;
            this.clips = clips;
        }
    }
}