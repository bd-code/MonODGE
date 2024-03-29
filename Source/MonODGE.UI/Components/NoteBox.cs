﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A lightweight text display popup.
    /// </summary>
    public class NoteBox : OdgePopUp {
        private StyledText _stytex;
        private Point textPoint;

        protected override int MinWidth => 
            Style.Padding.Left + 
            (_stytex?.Width ?? 0) + 
            Style.Padding.Right; 

        protected override int MinHeight => 
            Style.Padding.Top + 
            (_stytex?.Height ?? 0) + 
            Style.Padding.Bottom; 

        public NoteBox(StyleSheet style, string text, int lifetime = 300) : base(style) {
            _stytex = new StyledText(Style, text);
            Lifetime = lifetime;
            Size = new Point(MinWidth, MinHeight);
            Layout();            
            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        public override void Update() {
            Lifetime--;

            if (Lifetime < 64 && Fade == FadeStyle.FadeOut) {
                Style.BackgroundColors = new StyleContext<Color>(
                    Style.BackgroundColors.Normal * (1.0f - (1.0f / (Lifetime + 1.0f))),
                    Style.BackgroundColors.Active,
                    Style.BackgroundColors.Header,
                    Style.BackgroundColors.Footer);

                Style.TextColors = new StyleContext<Color>(
                    Style.TextColors.Normal * (1.0f - (1.0f / (Lifetime + 1.0f))),
                    Style.TextColors.Active,
                    Style.TextColors.Header,
                    Style.TextColors.Footer);

                Style.BorderColors = new StyleContext<Color>(
                    Style.BorderColors.Normal * (1.0f - (1.0f / (Lifetime + 1.0f))),
                    Style.BorderColors.Active,
                    Style.BorderColors.Header,
                    Style.BorderColors.Footer);
            }

            if (Lifetime == 0)
                OnTimeout();

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);
            _stytex.Draw(batch, new Rectangle(Location + textPoint, Size));
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
}
