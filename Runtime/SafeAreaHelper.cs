using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Amenity.SafeArea
{
    /// <summary>
    /// This component helps developer design UIs with banners and Unity's safe area.
    ///
    /// When placed on a Canvas GameObject, it will take its child named "SafeArea", and resize
    /// it to fit the inside of <see cref="Screen.safeArea">Screen.safeArea</see>, while also taking
    /// in account the banners displayed on screen. 
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class SafeAreaHelper : MonoBehaviour
    {
        private static readonly TimeSpan RegularUpdateDelay = TimeSpan.FromSeconds(5);

        private RectTransform safeAreaTransform;
        private Canvas canvas;
        private Rect lastAdjustedSafeArea;

        private bool updateLoopRunning;

        private readonly CancellationTokenSource updateLoopCancellationTokenSource = new();

        private void Awake()
        {
            this.safeAreaTransform = transform.Find("SafeArea") as RectTransform;
            this.canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            UpdateSafeArea();

            StartUpdateLoopIfNecessary();
        }

        private void OnDestroy()
        {
            this.updateLoopCancellationTokenSource.Cancel();
        }

        private void StartUpdateLoopIfNecessary()
        {
            if (this.updateLoopRunning) {
                return;
            }

            this.updateLoopRunning = true;

            UpdateAfterDelayInfinite(this.updateLoopCancellationTokenSource.Token)
                .ListenForErrors();
        }

        private async Task UpdateAfterDelayInfinite(CancellationToken cancellationToken)
        {
            while (true) {
                try {
                    await Task.Delay(RegularUpdateDelay, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) {
                        return;
                    }
                    if (this.isActiveAndEnabled) {
                        UpdateSafeArea();
                    }
                }
                catch (OperationCanceledException) {
                    break;
                }
                catch (Exception e) {
                    Debug.LogException(e);
                }
            }
        }

        private void UpdateSafeArea()
        {
            var safeArea = GetSafeArea();

            if (safeArea == this.lastAdjustedSafeArea) {
                return;
            }

            this.lastAdjustedSafeArea = safeArea;
            SetSafeAreaRectTransform(this.lastAdjustedSafeArea);
        }

        private static Rect GetSafeArea()
        {
            var safeArea = Screen.safeArea;
            return safeArea;
        }

        private void SetSafeAreaRectTransform(Rect newRect)
        {
            if (!this.safeAreaTransform) {
                return;
            }

            var anchorMin = newRect.position;
            var anchorMax = newRect.position + newRect.size;

            var canvasPixelRect = this.canvas.pixelRect;
            anchorMin.x /= canvasPixelRect.width;
            anchorMin.y /= canvasPixelRect.height;
            anchorMax.x /= canvasPixelRect.width;
            anchorMax.y /= canvasPixelRect.height;

            this.safeAreaTransform.anchorMin = anchorMin;
            this.safeAreaTransform.anchorMax = anchorMax;
        }
    }

    internal static class TaskExtensions
    {
        internal static void ListenForErrors(this Task voidedTask)
        {
            voidedTask.ContinueWith(task => {
                if (!task.IsCanceled) {
                    Debug.LogException(task.Exception);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}