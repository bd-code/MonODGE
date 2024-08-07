﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Styles {
    public class BGTexture {
        private Texture2D _tex2d;
        private Rectangle _srcrect;

        public Texture2D Texture => _tex2d;
        public Point TextureOrigin => _srcrect.Location;
        public int Width => _srcrect.Width;
        public int Height => _srcrect.Height;

        /// <summary>
        /// Constructs a BGTexture with full texture width, height.
        /// </summary>
        /// <param name="image">Texture2D</param>
        public BGTexture(Texture2D image) :
            this(image, new Rectangle(0, 0, image.Width, image.Height)) { }

        /// <summary>
        /// Constructs a BGTexture with sepecified source rectangle.
        /// </summary>
        /// <param name="image">Texture2D</param>
        /// <param name="selection">Texture source rectangle.</param>
        public BGTexture(Texture2D image, Rectangle selection) {
            _tex2d = image; _srcrect = selection;
        }


        public void Draw(SpriteBatch batch, Vector2 position, Color color) {
            batch.Draw(_tex2d, position, _srcrect, color);
        }
        public void Draw(SpriteBatch batch, Rectangle drawRect, Color color) {
            batch.Draw(_tex2d, drawRect, _srcrect, color);
        }
    }
}
