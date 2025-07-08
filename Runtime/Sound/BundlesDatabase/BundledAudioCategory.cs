using System;
using UnityEngine;

namespace Amenity.Sound
{
    [Serializable]
    public class BundledAudioCategory
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private BundledAudioData[] audios;

        public string Identifier {
            get => this.name;
            set => this.name = value;
        }

        public BundledAudioData[] Audios => this.audios;
    }
}