using Cysharp.Threading.Tasks;

namespace Amenity.SaveSystem
{
    public interface IStorage<T>
    {
        UniTask SaveImmediate(T profile);
        UniTask<T> LoadAsync();
        UniTask Delete();
    }
}