using System.Linq;
using UnityEngine;

namespace Amenity.Sound
{
    [CreateAssetMenu(fileName = "Bundled Sound Database", menuName = "Amenity/Databases/Bundled Sound Database")]
    public class BundledSoundDatabase : ScriptableObject
    {
        [SerializeField]
        internal BundledAudioCategory[] categories;

        public BundledAudioData Get(string groupName, string dataName)
        {
            var groups = categories.FirstOrDefault(c => c.Identifier == groupName);
            var data = groups.Audios.FirstOrDefault(d => d.Identifier == dataName);
            if (data == null) {
                Debug.LogError($"Sound data not found. Group: {groupName} name: {dataName}");
            }

            return data;
        }
    }
}