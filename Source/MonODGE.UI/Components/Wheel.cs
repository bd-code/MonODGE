using System;

using Microsoft.Xna.Framework;

using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    public abstract partial class OdgeWheel : OdgeControl {
        protected Vector2[] _uiPositions;
        protected Vector2[] _uiDimensions;

        protected BGTexture _liprompt, _riprompt;
        protected string _lsprompt, _rsprompt;

        protected abstract string ValueToString { get; }

        protected override int MinWidth =>
            Style.Padding.Left + Style.Padding.Right +
            (int)(_uiDimensions[0].X + _uiDimensions[1].X + _uiDimensions[2].X);

        protected override int MinHeight =>  
            Style.Padding.Top +
            (int)(Math.Max(Math.Max(_uiDimensions[0].Y, _uiDimensions[1].Y), _uiDimensions[2].Y)) + 
            Style.Padding.Bottom;


        public OdgeWheel(StyleSheet style) : base(style) {
            _lsprompt = "<< ";
            _rsprompt = " >>";
        }


        public override void Layout() {
            if (_liprompt != null || _riprompt != null) {
                _uiDimensions = new Vector2[] {
                    new Vector2(_liprompt.Width, _liprompt.Height),
                    GetMaxValueSize(),
                    new Vector2(_riprompt.Width, _riprompt.Height)
                };
            }
            else {
                _uiDimensions = new Vector2[] {
                    Style.Fonts?.Normal?.MeasureString(_lsprompt) ?? Vector2.Zero,
                    GetMaxValueSize(),
                    Style.Fonts?.Normal?.MeasureString(_rsprompt) ?? Vector2.Zero
                };
            }

            // All three inner components use Absolute Positioning
            // and only need Vertical Positioning.
            // Value is always centered aligned horizontally.

            float y0, y1, y2;

            if (Style.AlignV == StyleSheet.AlignmentsV.TOP)
                y0 = y1 = y2 = Y + Style.Padding.Top;
            else if (Style.AlignV == StyleSheet.AlignmentsV.CENTER) {
                y0 = Dimensions.Center.Y - (_uiDimensions[0].Y / 2);
                y1 = Dimensions.Center.Y - (_uiDimensions[1].Y / 2);
                y2 = Dimensions.Center.Y - (_uiDimensions[2].Y / 2);
            }
            else {  // Bottom
                y0 = Dimensions.Bottom - _uiDimensions[0].Y - Style.Padding.Bottom;
                y1 = Dimensions.Bottom - _uiDimensions[1].Y - Style.Padding.Bottom;
                y2 = Dimensions.Bottom - _uiDimensions[2].Y - Style.Padding.Bottom;
            }

            var vx = Style.Fonts?.Normal?.MeasureString(ValueToString) ?? Vector2.Zero;

            _uiPositions = new Vector2[] {
                new Vector2(X + Style.Padding.Left, y0),
                new Vector2(Dimensions.Center.X - (vx.X / 2), y1),
                new Vector2(Dimensions.Right - _uiDimensions[2].X - Style.Padding.Right, y2)
            };

            base.Layout();
        }


        public void SetTextPrompts(string leftPrompt, string rightPrompt) {
            if (leftPrompt == null)
                throw new ArgumentNullException("leftPrompt", "SetTexturePrompts() passed null.");
            else if (rightPrompt == null)
                throw new ArgumentNullException("rightPrompt", "SetTexturePrompts() passed null.");

            _lsprompt = leftPrompt; _rsprompt = rightPrompt;
            _liprompt = null;       _riprompt = null;
            IsMessy = true;
        }

        public void SetTexturePrompts(BGTexture leftPrompt, BGTexture rightPrompt) {
            if (leftPrompt == null)
                throw new ArgumentNullException("leftPrompt", "SetTexturePrompts() passed null.");
            else if (rightPrompt == null)
                throw new ArgumentNullException("rightPrompt", "SetTexturePrompts() passed null.");

            _liprompt = leftPrompt; _riprompt = rightPrompt;
            _lsprompt = null;       _rsprompt = null;
            IsMessy = true;
        }


        protected virtual Vector2 GetMaxValueSize() { return Vector2.Zero; }
    }
}
