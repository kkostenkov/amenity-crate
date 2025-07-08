using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Amenity.SaveSystem
{
    internal class FileStorageHandler : IStorageHandler
    {
        private readonly string savePath;

        public FileStorageHandler(string fileName)
        {
            this.savePath = $"{Application.persistentDataPath}/{fileName}";
        }

        public bool CheckExistingSave()
        {
            return File.Exists(this.savePath);
        }

        public void Delete()
        {
            File.Delete(this.savePath);
        }

        public async UniTask<string> Load()
        {
            var textData = await File.ReadAllTextAsync(this.savePath);
            return textData;
        }

        public async UniTask Save(string data)
        {
            await using var writer = new StreamWriter(this.savePath);
            await writer.WriteAsync(data);
        }
    }
}