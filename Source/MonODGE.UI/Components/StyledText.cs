using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Component for custom text styling and positioning.
    /// </summary>
    public class StyledText : OdgeComponent {
        private string[] _lines;
        private Vector2[] _positions;

        public enum StyleModes {
            Standard = 0, Header = 1, Footer = 2, Selected = 3, Unselected = 4
        }

        private StyleModes _stymode;
        public StyleModes StyleMode {
            get { return _stymode; }
            set {
                _stymode = value;
                Layout();
            }
        }

        public string Text => string.Join(Environment.NewLine, _lines);

        private SpriteFont _font {
            get {
                if (StyleMode == StyleModes.Header) return Style.HeaderFont;
                else if (StyleMode == StyleModes.Footer) return Style.FooterFont;
                else return Style.Font;
            }
        }
        private Color _color {
            get {
                if (StyleMode == StyleModes.Header) return Style.HeaderColor;
                else if (StyleMode == StyleModes.Footer) return Style.FooterColor;
                else if (StyleMode == StyleModes.Selected) return Style.SelectedTextColor;
                else if (StyleMode == StyleModes.Unselected) return Style.UnselectedTextColor;
                else return Style.TextColor;
            }
        }


        public StyledText(StyleSheet style, string textblock, StyleModes mode = StyleModes.Standard) : 
            this(style, textblock.Split(new[] { Environment.NewLine }, StringSplitOptions.None), mode) { }

        public StyledText(StyleSheet style, string[] textlines, StyleModes mode = StyleModes.Standard) {
            Style = style;
            _stymode = mode;
            _lines = textlines;
            _positions = new Vector2[_lines.Length];
            Layout();
        }


        public override void Layout() {
            float lasty = 2;
            float maxWidth = 0;
            float lineheight = 0f;

            // Get dimensions and set HEIGHT ONLY of every string line.
            for (int s = 0; s < _lines.Length; s++) {
                Vector2 dims = string.IsNullOrEmpty(_lines[s]) ?
                    _font?.MeasureString("A") ?? Vector2.Zero :
                    _font?.MeasureString(_lines[s]) ?? Vector2.Zero;
                _positions[s] = new Vector2(dims.X, lasty);
                lasty +=  dims.Y + Style.Spacing.Vertical;
                maxWidth = Math.Max(maxWidth, _positions[s].X);
                lineheight = dims.Y;
            }

            // Correct for extra space added.
            lasty -= Style.Spacing.Vertical + (lineheight / 8f) - 1;

            // At this point _position.X is the width of the string, not it's X position!
            // Alignment X repositioning is below!

            if (Style.AlignH == StyleSheet.AlignmentsH.LEFT) {
                for (int p = 0; p < _positions.Length; p++)
                    _positions[p].X = 0;
            }
            else if (Style.AlignH == StyleSheet.AlignmentsH.CENTER) {
                for (int p = 0; p < _positions.Length; p++)
                    _positions[p].X = (maxWidth - _positions[p].X) / 2;                  
            }
            else if (Style.AlignH == StyleSheet.AlignmentsH.RIGHT) {
                for (int p = 0; p < _positions.Length; p++)
                    _positions[p].X = (maxWidth - _positions[p].X);
            }

            // Lastly, calculate new width + height based on alignment and style.
            Size = new Point((int)maxWidth, (int)lasty);

            base.Layout();
        }


        public override void Draw(SpriteBatch batch) {
            //DrawBorders(batch);
            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, new Vector2(X, Y));
                batch.DrawString(_font, _lines[d], 
                    new Vector2(X, Y) + _positions[d], 
                    _color);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            Vector2 where = new Vector2(X + parentRect.X, Y + parentRect.Y);
            //DrawCorners(batch, new Rectangle(where.ToPoint(), Size));
            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, where);
                batch.DrawString(_font, _lines[d], 
                    where + _positions[d], 
                    _color);
            }
        }


        private void DrawShadows(SpriteBatch batch, int index, Vector2 where) {
            for (int s = 0; s < Style.TextShadows.Length; s++) {
                if (Style.TextShadows[s] != Vector2.Zero) {
                    batch.DrawString(_font, _lines[index],
                        where + _positions[index] + Style.TextShadows[s],
                        Style.TextShadowColor);
                }
            }
        }
    }
}
