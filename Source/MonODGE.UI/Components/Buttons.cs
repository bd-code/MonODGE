using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Helper methods for easier button creation.
    /// </summary>
    public class OdgeButtonFactory {
        public static TextButton Create(OdgeComponent parent, string text, EventHandler func) {
            var btn = new TextButton(parent.Style.Clone(), text, func);
            btn.Width = parent.Width - parent.Style.Padding.Left - parent.Style.Padding.Right;
            btn.Layout();
            return btn;
        }

        public static ImageButton Create(OdgeComponent parent, Texture2D image, EventHandler func) {
            var btn = new ImageButton(parent.Style.Clone(), image, func);
            btn.Width = parent.Width - parent.Style.Padding.Left - parent.Style.Padding.Right;
            btn.Layout();
            return btn;
        }
    }

    ///////////////////////////////////////////////////////////////////////////

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
            _stytex.StyleMode = StyledText.StyleModes.Unselected;
            PackToSize(new Rectangle(0, 0, MinWidth, MinHeight));
            //Layout(); // No Layout on init for buttons.

            Select += (o, e) => _stytex.StyleMode = StyledText.StyleModes.Selected;
            Unselect += (o, e) => _stytex.StyleMode = StyledText.StyleModes.Unselected;
            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);

            if (IsSelected) 
                DrawBorders(batch);            
            else 
                DrawCorners(batch);
            
            _stytex.Draw(batch, new Rectangle(Dimensions.Location + textPoint, Dimensions.Size));
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);

            if (IsSelected) 
                DrawBorders(batch, parentRect);            
            else 
                DrawCorners(batch, parentRect);
            
            _stytex.Draw(batch, new Rectangle(parentRect.Location + Dimensions.Location + textPoint, Dimensions.Size));
        }


        public override void Layout() {
            if (_stytex.IsMessy)
                _stytex.Layout();
            PackToSize(Dimensions);
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
            PackToSize(new Rectangle(0, 0, MinWidth, MinHeight));
            //Layout(); // No Layout on init for buttons.
        }

        public ImageButton(StyleSheet style, Texture2D image, Rectangle sourceRect, EventHandler action)
            : base(style, action) {
            texture = image;
            srcRect = sourceRect;
            dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            PackToSize(new Rectangle(0, 0, MinWidth, MinHeight));
            //Layout(); // No Layout on init for buttons.
        }


        public override void Draw(SpriteBatch batch) {
            Rectangle dst = new Rectangle(Dimensions.Location + dstRect.Location, dstRect.Size);
            DrawBG(batch);

            if (IsSelected) {
                DrawBorders(batch);
                batch.Draw(texture, dst, srcRect, Color.White);
            }
            else {
                DrawCorners(batch);
                batch.Draw(texture, dst, srcRect, Color.Gray);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            Rectangle where = new Rectangle(parentRect.Location + Dimensions.Location, Dimensions.Size);
            DrawBG(batch, parentRect);

            if (IsSelected) {
                DrawBorders(batch, parentRect);
                batch.Draw(texture, 
                    new Rectangle(where.Location + dstRect.Location, dstRect.Size), 
                    srcRect, Color.White);
            }
            else {
                DrawCorners(batch, parentRect);
                batch.Draw(texture, 
                    new Rectangle(where.Location + dstRect.Location, dstRect.Size), 
                    srcRect, Color.Gray);
            }
        }


        public override void Layout() {
            // No child components, Rectangle adjustments unnecessary.
            //PackToSize(new Point(Width, Height));
            dstRect.Location = Utilities.LayoutHelper.AlignToPoint(this, srcRect);
            base.Layout();
        }
    }

}
