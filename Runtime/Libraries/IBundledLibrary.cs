using Cysharp.Threading.Tasks;

namespace Amenity.Libraries
{
    public interface IBundledLibrary<T>
    {
        UniTask<T> Get(string id);
        UniTask<T> GetFallback();
    }
}