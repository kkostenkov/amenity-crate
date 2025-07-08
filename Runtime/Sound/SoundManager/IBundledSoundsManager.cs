using System.Threading.Tasks;

namespace Amenity.Sound
{
    public interface IBundledSoundsManager : ISoundManager
    {
        Task PlayMusic(BundledAudioData data, int layer = 0);
        Task PlaySfx(BundledAudioData data);
    }
}