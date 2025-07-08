#if UNITY_WEBGL || UNITY_EDITOR
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Amenity.SaveSystem
{
    public class WebGlStorageHandler : IStorageHandler
    {
        [DllImport("__Internal")]
        private static extern void saveData(string key, string data);

        [DllImport("__Internal")]
        private static extern string loadData(string key);

        [DllImport("__Internal")]
        private static extern string deleteKey(string key);

        [DllImport("__Internal")]
        private static extern string deleteAllKeys(string prefix);

        private string SAVE_PATH => $"idbfs/{Application.productName}/";
        private readonly string fileName;

        public WebGlStorageHandler(string fileName)
        {
            this.fileName = fileName;
        }

        private string PrefixKey(string key)
        {
            return SAVE_PATH + key;
        }

        public bool CheckExistingSave()
        {
            var value = GetKey(this.fileName);
            return !string.IsNullOrEmpty(value);
        }

        public UniTask Save(string data)
        {
            SetKey(this.fileName, data);
            return UniTask.CompletedTask;
        }

        public UniTask<string> Load()
        {
            var textData = GetKey(this.fileName);
            return UniTask.FromResult(textData);
        }

        public void Delete()
        {
            DeleteKey(this.fileName);
        }

        public static void DeleteAllKeys()
        {
            deleteAllKeys(string.Empty);
        }

        private void SetKey(string key, string data)
        {
            saveData(PrefixKey(key), data);
        }

        private string GetKey(string key)
        {
            return loadData(PrefixKey(key));
        }

        private void DeleteKey(string key)
        {
            deleteKey(PrefixKey(key));
        }
    }
}
#endif