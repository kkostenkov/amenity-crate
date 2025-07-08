using Cysharp.Threading.Tasks;

namespace Amenity.SaveSystem
{
    public interface IStorageHandler
    {
        bool CheckExistingSave();
        UniTask Save(string data);
        UniTask<string> Load();
        void Delete();
    }
}