using System.Threading;
using Cysharp.Threading.Tasks;

namespace Amenity.SaveSystem
{
    public class Saver<T>
    {
        private const int DeferredSaveDelayMilliseconds = 2000;
        private readonly IStorage<T> storage;
        private CancellationTokenSource deferredSaveCancellation;
        private T dataToSave;
        private bool IsDeferredSaveScheduled => this.deferredSaveCancellation != null;

        public Saver(IStorage<T> storage)
        {
            this.storage = storage;
        }

        public UniTask Save(T dataToSave)
        {
            this.dataToSave = dataToSave;
            return Save();
        }

        public void TryScheduleDeferredSave(T dataToSave)
        {
            this.dataToSave = dataToSave;
            if (this.IsDeferredSaveScheduled) {
                return;
            }
            ScheduleDeferredSave();
        }

        private void ScheduleDeferredSave()
        {
            this.deferredSaveCancellation = new CancellationTokenSource();
            var cancellationToken = this.deferredSaveCancellation.Token;

            UniTask.Void(async () => {
                await UniTask.Delay(DeferredSaveDelayMilliseconds, true, cancellationToken: cancellationToken);
                if (cancellationToken.IsCancellationRequested) {
                    return;
                }

                await Save();
                this.deferredSaveCancellation = null;
            });
        }

        public void CancelDeferredSave()
        {
            this.deferredSaveCancellation?.Cancel();
            this.deferredSaveCancellation = null;
        }

        private UniTask Save()
        {
            // No need to save later if we're saving right now  
            CancelDeferredSave();
            return this.storage.SaveImmediate(this.dataToSave);
        }
    }
}