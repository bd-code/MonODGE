using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI {
    /// <summary>
    /// Creates a resizable image with fixed corners and stretchable center.
    /// </summary>
    public class NinePatch {
        private Texture2D _tex2d;
        private int _cornerWidth, _cornerHeight;
        private Rectangle[] _srcRects;

        public Texture2D Texture => _tex2d;


        /// <summary>
        /// Constructs a NinePatch by dividing image to equal thirds.
        /// </summary>
        /// <param name="image">Texture2D to be stretched. Must be larger than 3x3px.</param>
        public NinePatch(Texture2D image) {
            _tex2d = image;
            _cornerWidth = image.Width / 3;
            _cornerHeight = image.Height / 3;

            if (_cornerWidth == 0 || _cornerHeight == 0) {
                string e = $"NinePatch: image.Width or image.Height is < 3 px.";
                throw new ArgumentException(e);
            }

            _srcRects = GetPatches(new Rectangle(0, 0, image.Width, image.Height));
        }


        /// <summary>
        /// Constructs a NinePatch with fixed corner area.
        /// </summary>
        /// <param name="image">Texture2D to be stretched. Must be larger than 3x3px.</param>
        /// <param name="cornerWidth">Must be strictly less than image.Width / 2.</param>
        /// <param name="cornerHeight">Must be strictly less than image.Height / 2.</param>
        public NinePatch(Texture2D image, int cornerWidth, int cornerHeight) {
            _tex2d = image;
            _cornerWidth = cornerWidth;
            _cornerHeight = cornerHeight;
            
            if (_cornerWidth >= _tex2d.Width / 2) {
                string e = $"NinePatch: cornerWidth {_cornerWidth} is too large for image.Width {_tex2d.Width}.";
                throw new ArgumentOutOfRangeException(e);
            }
            else if (_cornerHeight >= _tex2d.Height / 2) {
                string e = $"NinePatch: cornerHeight {_cornerHeight} is too large for image.Height {_tex2d.Height}.";
                throw new ArgumentOutOfRangeException(e);
            }

            _srcRects = GetPatches(new Rectangle(0, 0, image.Width, image.Height));
        }


        public void Draw(SpriteBatch batch, Rectangle drawRect, Color color) {
            var dstRects = GetPatches(drawRect);
            for (int p = 0; p < 9; p++)
                batch.Draw(_tex2d, dstRects[p], _srcRects[p], color);
        }
        public void DrawCorners(SpriteBatch batch, Rectangle drawRect, Color color) {
            var dstRects = GetPatches(drawRect);
            batch.Draw(_tex2d, dstRects[0], _srcRects[0], color);
            batch.Draw(_tex2d, dstRects[2], _srcRects[2], color);
            batch.Draw(_tex2d, dstRects[6], _srcRects[6], color);
            batch.Draw(_tex2d, dstRects[8], _srcRects[8], color);
        }


        private Rectangle[] GetPatches(Rectangle rect) {
            return new Rectangle[] {
                new Rectangle(rect.X, rect.Y, _cornerWidth, _cornerHeight),
                new Rectangle(rect.X + _cornerWidth, rect.Y, rect.Width - 2 * _cornerWidth, _cornerHeight),
                new Rectangle(rect.Right - _cornerWidth, rect.Y, _cornerWidth, _cornerHeight),

                new Rectangle(rect.X, rect.Y + _cornerHeight, _cornerWidth, rect.Height - 2 * _cornerHeight),
                new Rectangle(rect.X + _cornerWidth, rect.Y + _cornerHeight, rect.Width - 2 * _cornerWidth, rect.Height - 2 * _cornerHeight),
                new Rectangle(rect.Right - _cornerWidth, rect.Y + _cornerHeight, _cornerWidth, rect.Height - 2 * _cornerHeight),

                new Rectangle(rect.X, rect.Bottom - _cornerHeight, _cornerWidth, _cornerHeight),
                new Rectangle(rect.X + _cornerWidth, rect.Bottom - _cornerHeight, rect.Width - 2 * _cornerWidth, _cornerHeight),
                new Rectangle(rect.Right - _cornerWidth, rect.Bottom - _cornerHeight, _cornerWidth, _cornerHeight)
            };
        }
    }
}
