using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Amenity.SaveSystem
{
    public class LocalPersistentDataStorage<T> : IStorage<T>
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly ISerializer<T> serializer;

        private readonly IStorageHandler storageHandler;

        public LocalPersistentDataStorage(string fileName, ISerializer<T> serializer)
        {
            this.serializer = serializer;
#if UNITY_WEBGL || UNITY_EDITOR
            this.storageHandler = new WebGlStorageHandler(fileName);
#endif
#if !UNITY_WEBGL || UNITY_EDITOR
            var fileFullName = $"{fileName}.{serializer.FileExtension}";
            this.storageHandler = new FileStorageHandler(fileFullName);
#endif
        }

        public UniTask SaveImmediate(T profile)
        {
            return SaveInternal(profile);
        }

        public async UniTask<T> LoadAsync()
        {
            await this.semaphore.WaitAsync();
            try {
                if (!this.storageHandler.CheckExistingSave()) {
                    return default;
                }

                var textData = await this.storageHandler.Load();

                var deserialized = this.serializer.Deserialize(textData);
                return deserialized;
            }
            catch (Exception e) {
                Debug.LogException(e);
                return default;
            }
            finally {
                this.semaphore.Release();
            }
        }

        public async UniTask Delete()
        {
            await this.semaphore.WaitAsync();
            try {
                if (this.storageHandler.CheckExistingSave()) {
                    this.storageHandler.Delete();
                }
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
            finally {
                this.semaphore.Release();
            }
        }

        private async UniTask SaveInternal(T data)
        {
            await this.semaphore.WaitAsync();
            try {
                var toString = this.serializer.SerializeToString(data);
                await this.storageHandler.Save(toString);
            }
            catch (Exception e) {
                Debug.LogException(e);
            }
            finally {
                this.semaphore.Release();
            }
        }
    }
}