// https://frarees.github.io/default-gist-license
// source https://gist.github.com/frarees/9791517

using System;
using UnityEngine;

namespace Amenity.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float Min { get; set; }
        public float Max { get; set; }
        public bool DataFields { get; set; } = true;
        public bool FlexibleFields { get; set; } = true;
        public bool Bound { get; set; } = true;
        public bool Round { get; set; } = true;

        public MinMaxSliderAttribute() : this(0, 1)
        {
        }

        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}