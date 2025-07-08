using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Amenity.Libraries
{
    public abstract class AbstractBundledAssetLibrary<T> : ScriptableObject, IBundledLibrary<T>, IDisposable
        where T : Object
    {
        protected virtual string AddressPrefix => string.Empty;
        protected virtual string AddressPostfix => ".asset";
        protected virtual string FallbackEntryId => string.Empty;

        [SerializeField]
        private List<AssetReferenceT<T>> itemsToPreload = new();

        private readonly Dictionary<string, AsyncOperationHandle<T>> entryHandles = new();

        [NonSerialized]
        private bool isInitialized;

        private string AssetAddressPattern => AddressPrefix + "{0}" + AddressPostfix;

        public UniTask<T> Get(string id)
        {
            return GetInternal(id);
        }

        private async UniTask<T> GetInternal(string id, bool isFallbackCall = false)
        {
            if (string.IsNullOrEmpty(id)) {
                return null;
            }

            await TryInitialize();

            if (this.entryHandles.TryGetValue(id, out var cachedHandle)) {
                var cachedAsset = await cachedHandle;
                return cachedAsset;
            }

            var addressableKey = string.Format(AssetAddressPattern, id);
            var handle = Addressables.LoadAssetAsync<T>(addressableKey);
            T asset;
            this.entryHandles.Add(id, handle);
            try {
                asset = await handle;
            }
            catch (InvalidKeyException invalidKeyException) {
                this.entryHandles.Remove(id);
                if (isFallbackCall) {
                    Debug.Log($"fallback for {invalidKeyException.Key} is not found");
                    return null;
                }
                Debug.Log($"returning fallback for {invalidKeyException.Key}");
                var fallback = await GetFallback();
                return fallback;
            }

            return asset;
        }

        public UniTask<T> GetFallback()
        {
            return GetInternal(FallbackEntryId, true);
        }

        public void Dispose()
        {
            ReleaseAndClear();
        }

        protected async UniTask TryInitialize()
        {
            if (this.isInitialized) {
                return;
            }
            this.isInitialized = true;
            await Initialize();
        }

        protected virtual async UniTask Initialize()
        {
            // ListPool<UniTask>.Get(out var tasks); // TODO
            foreach (var referenceT in this.itemsToPreload) {
                var handle = Addressables.LoadAssetAsync<T>(referenceT);
                var asset = await handle;
                this.entryHandles.Add(GetLibraryEntryId(asset), handle);
            }
        }

        protected void ReleaseAndClear()
        {
            foreach (var handle in this.entryHandles.Values) {
                Addressables.Release(handle);
            }

            this.entryHandles.Clear();
        }

        protected abstract string GetLibraryEntryId(T asset);
    }
}