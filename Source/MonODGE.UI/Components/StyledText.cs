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
        int _minWidth, _minHeight;

        public ConponentContexts Context {
            get { return _context; }
            set {
                if (_context != value) {
                    IsMessy = true;
                    _context = value;
                }
            }
        }
        private ConponentContexts _context;

        protected override int MinWidth => _minWidth;
        protected override int MinHeight => _minHeight;

        public string Text => string.Join(Environment.NewLine, _lines);

        private SpriteFont _font {
            get {
                if (Context == ConponentContexts.Header) return Style.Fonts.Header;
                else if (Context == ConponentContexts.Footer) return Style.Fonts.Footer;
                else if (Context == ConponentContexts.Active) return Style.Fonts.Active;
                else return Style.Fonts.Normal;
            }
        }
        private Color _color {
            get {
                if (Context == ConponentContexts.Header) return Style.TextColors.Header;
                else if (Context == ConponentContexts.Footer) return Style.TextColors.Footer;
                else if (Context == ConponentContexts.Active) return Style.TextColors.Active;
                else return Style.TextColors.Normal;
            }
        }


        public StyledText(StyleSheet style, string textblock, ConponentContexts mode = ConponentContexts.Normal) : 
            this(style, textblock.Split(new[] { Environment.NewLine }, StringSplitOptions.None), mode) { }

        public StyledText(StyleSheet style, string[] textlines, ConponentContexts mode = ConponentContexts.Normal) {
            Style = style;
            _context = mode;
            _lines = textlines;
            _positions = new Vector2[_lines.Length];
            Layout();
        }


        public override void Layout() {
            float lasty = 0;
            float maxWidth = 0;
            float lineheight = 0f;

            // Get dimensions and set HEIGHT ONLY of every string line.
            for (int s = 0; s < _lines.Length; s++) {
                Vector2 dims = string.IsNullOrEmpty(_lines[s]) ?
                    (_font?.MeasureString("A") ?? Vector2.Zero) :
                    (_font?.MeasureString(_lines[s]) ?? Vector2.Zero);

                _positions[s] = new Vector2(dims.X, lasty);
                lasty +=  dims.Y + Style.Spacing.Vertical;
                maxWidth = Math.Max(maxWidth, _positions[s].X);
                lineheight = dims.Y;
            }

            // Correct for extra space added.
            lasty -= Style.Spacing.Vertical+2;

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
            _minWidth = (int)maxWidth;
            _minHeight = (int)lasty;
            Size = new Point(_minWidth, _minHeight);

            base.Layout();
        }


        public override void Draw(SpriteBatch batch) {
            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, new Vector2(X, Y));
                batch.DrawString(_font, _lines[d], 
                    new Vector2(X, Y) + _positions[d], 
                    _color);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            Vector2 where = new Vector2(X + parentRect.X, Y + parentRect.Y);
            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, where);
                batch.DrawString(_font, _lines[d], 
                    where + _positions[d], 
                    _color);
            }
        }


        private void DrawShadows(SpriteBatch batch, int index, Vector2 where) {
            foreach (var s in Style.TextShadow.Distances) {
                if (s != Vector2.Zero) {
                    batch.DrawString(_font, _lines[index],
                        where + _positions[index] + s,
                        Style.TextShadow.Color);
                }
            }
        }
    }
}
