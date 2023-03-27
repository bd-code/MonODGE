using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A Button with a StyledText component.
    /// </summary>
    public class TextButton : OdgeButton {
        private StyledText _stytex;
        private Point textPoint;

        protected override int MinWidth => _stytex.Width + Style.Padding.Left + Style.Padding.Right;
        protected override int MinHeight => _stytex.Height + Style.Padding.Top + Style.Padding.Bottom;

        public TextButton(StyleSheet style, string optionText, EventHandler action) : base(style, action) {
            _stytex = new StyledText(style, optionText);
            _stytex.Context = ComponentContexts.Normal;
            SizeToMin();
            //Layout(); // No Layout on init for buttons.

            Select += (o, e) => _stytex.Context = ComponentContexts.Active;
            Unselect += (o, e) => _stytex.Context = ComponentContexts.Normal;
            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        public override void Draw(SpriteBatch batch) {
            Draw(batch, Rectangle.Empty);
        }
        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);            
            _stytex.Draw(batch, new Rectangle(parentRect.Location + Location + textPoint, Size));
        }


        public override void Layout() {
            if (_stytex.IsMessy)
                _stytex.Layout();
            textPoint = Utilities.LayoutHelper.AlignToPoint(this, _stytex);
            base.Layout();
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// A Button that displays a Texture2D.
    /// </summary>
    public class ImageButton : OdgeButton {
        private Texture2D texture;
        private Rectangle dstRect;
        private Rectangle srcRect;

        protected override int MinWidth => srcRect.Width + Style.Padding.Left + Style.Padding.Right;
        protected override int MinHeight => srcRect.Height + Style.Padding.Top + Style.Padding.Bottom;

        public ImageButton(StyleSheet style, Texture2D image, EventHandler action)
            : base(style, action) {
            texture = image;
            srcRect = new Rectangle(0, 0, image.Width, image.Height);
            dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            SizeToMin();
            //Layout(); // No Layout on init for buttons.
        }

        public ImageButton(StyleSheet style, Texture2D image, Rectangle sourceRect, EventHandler action)
            : base(style, action) {
            texture = image;
            srcRect = sourceRect;
            dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            SizeToMin();
            //Layout(); // No Layout on init for buttons.
        }


        public override void Draw(SpriteBatch batch) {
            Draw(batch, Rectangle.Empty);
        }
        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);

            Rectangle where = new Rectangle(parentRect.Location + Location + dstRect.Location, dstRect.Size);
            Color tclr = IsSelected ? Color.White : Color.Gray;

            batch.Draw(texture, where, srcRect, tclr);
        }


        public override void Layout() {
            dstRect.Location = Utilities.LayoutHelper.AlignToPoint(this, srcRect);
            base.Layout();
        }
    }

}
