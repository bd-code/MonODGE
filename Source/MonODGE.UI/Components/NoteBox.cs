
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A lightweight text display popup.
    /// </summary>
    public class NoteBox : OdgePopUp {
        private StyledText _stytex;
        private Point textPoint;

        protected override int MinWidth => Style.Padding.Left + (_stytex?.Width ?? 0) + Style.Padding.Right; 
        protected override int MinHeight => Style.Padding.Top + (_stytex?.Height ?? 0) + Style.Padding.Bottom; 

        public NoteBox(StyleSheet style, string text, int lifetime = 300) : base(style) {
            _stytex = new StyledText(Style, text);
            Lifetime = lifetime;
            Layout();
            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        public override void Update() {
            Lifetime--;

            if (Lifetime < 64 && Fade == FadeStyle.FadeOut) {
                Style.BackgroundColor *= 1.0f - (1.0f / (Lifetime+1.0f));
                Style.TextColor *= 1.0f - (1.0f / (Lifetime+1.0f));
                Style.BorderColor *= 1.0f - (1.0f / (Lifetime+1.0f));
            }
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);
            _stytex.Draw(batch, new Rectangle(Dimensions.Location + textPoint, Dimensions.Size));
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);
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
}
