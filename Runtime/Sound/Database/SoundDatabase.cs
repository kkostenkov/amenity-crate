using System.Linq;
using UnityEngine;

namespace Amenity.Sound
{
    [CreateAssetMenu(fileName = "Sound Database", menuName = "Amenity/Databases/Sound Database")]
    public partial class SoundDatabase : ScriptableObject
    {
        [SerializeField]
        private AudioCategory[] categories;

        public AudioData Get(string groupName, string dataName)
        {
            var groups = categories.FirstOrDefault(c => c.Identifier == groupName);
            var data = groups.Audios.FirstOrDefault(d => d.Identifier == dataName);
            if (data == null) {
                Debug.LogError($"Sound data {groupName} {dataName} was not found");
            }
            return data;
        }
    }
}