using Cysharp.Threading.Tasks;
using Amenity.Libraries;
using UnityEngine;

namespace PulpoFeliz.Dice.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpriteLibrary", menuName = "Amenity/Libraries/SpriteLibrary")]
    public class SpriteLibrary : AbstractBundledAssetLibrary<Sprite>
    {
        [SerializeField]
        private string prefix;
        [SerializeField]
        private string postfix;
        [SerializeField]
        private string fallbackId;
        protected override string AddressPrefix => this.prefix;
        protected override string AddressPostfix => this.postfix;
        protected override string FallbackEntryId => this.fallbackId;
        protected override string GetLibraryEntryId(Sprite asset)
        {
            return asset.name;
        }

        public Sprite TransparentSprite { get; private set; }

        protected override async UniTask Initialize()
        {
            await base.Initialize();
            TransparentSprite = CreateTransparentSprite();
        }

        private Sprite CreateTransparentSprite()
        {
            // Create a 1x1 Texture2D
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);

            // Set the pixel to transparent
            texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
            texture.Apply(); // Apply changes to the texture

            // Create a sprite from the texture
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
    }
}