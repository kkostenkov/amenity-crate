using System;
using UnityEngine;

namespace Amenity.Sound
{
    [Serializable]
    public class AudioCategory
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private AudioData[] audios;

        public string Identifier {
            get => this.name;
            set => this.name = value;
        }

        public AudioData[] Audios => this.audios;
    }
}