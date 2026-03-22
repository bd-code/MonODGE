using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    public enum TextEffects {
        None = 0, Typewriter = 1
    }


    /// <summary>
    /// Component for custom text styling and positioning.
    /// </summary>
    public class StyledText : OdgeComponent {
        private string[] _lines;
        private Vector2[] _positions;
        int _minWidth, _minHeight;

        // TextEffect variables.
        private int _drawCharAt;
        private int _lastCompleteLine;
        private int _framesToType;

        public new ComponentContexts Context {
            get { return _context; }
            set {
                if (_context != value) {
                    IsMessy = true;
                    _context = value;
                }
            }
        }
        private ComponentContexts _context;

        protected override int MinWidth => _minWidth;
        protected override int MinHeight => _minHeight;

        public string Text => string.Join(Environment.NewLine, _lines);


        public StyledText(StyleSheet style, string textblock, ComponentContexts mode = ComponentContexts.Normal) : 
            this(style, textblock.Split(new[] { Environment.NewLine }, StringSplitOptions.None), mode) { }

        public StyledText(StyleSheet style, string[] textlines, ComponentContexts mode = ComponentContexts.Normal) :
        base() {
            Style = style;
            _context = mode;
            _lines = textlines;
            _positions = new Vector2[_lines.Length];

            _drawCharAt = 0;
            _lastCompleteLine = (style.TextEffect.Get(Context) == TextEffects.Typewriter) ? -1 : _lines.Length;
            _framesToType = style.TextEffectRate;
            
            Layout();
        }


        public override void Layout() {
            float lasty = 0;
            float maxWidth = 0;
            var _font = Style.Fonts.Get(Context);

            // Get dimensions and set HEIGHT ONLY of every string line.
            for (int s = 0; s < _lines.Length; s++) {
                Vector2 dims = string.IsNullOrEmpty(_lines[s]) ?
                    (_font?.MeasureString("A") ?? Vector2.Zero) :
                    (_font?.MeasureString(_lines[s]) ?? Vector2.Zero);

                _positions[s] = new Vector2(dims.X, lasty);
                lasty +=  dims.Y + Style.Spacing.Vertical;
                maxWidth = Math.Max(maxWidth, _positions[s].X);
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
            if (Style.TextEffect.Get(Context) == TextEffects.Typewriter && _lastCompleteLine < _lines.Length - 1) {
                Draw_Typewriter(batch);
            }
            else {
                Draw_Normal(batch);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            if (Style.TextEffect.Get(Context) == TextEffects.Typewriter && _lastCompleteLine < _lines.Length - 1) {
                Draw_Typewriter(batch, parentRect);
            }
            else {
                Draw_Normal(batch, parentRect);
            }
        }


        public void ResetTextEffects() {
            if (Style.TextEffect.Get(Context) == TextEffects.Typewriter) {
                _drawCharAt = 0;
                _lastCompleteLine = -1;
                _framesToType = Style.TextEffectRate;
            }
        }
        public void FinishTextEffects() {
            if (Style.TextEffect.Get(Context) == TextEffects.Typewriter) {
                _drawCharAt = 0;
                _lastCompleteLine = _lines.Length;
                _framesToType = 0;
            }
        }


        ///////////////
        // Draw Effects

        private void Draw_Normal(SpriteBatch batch) {
            var font = Style.Fonts.Get(Context);
            var color = Style.TextColors.Get(Context);

            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, new Vector2(X, Y));
                batch.DrawString(
                    font, _lines[d],
                    new Vector2(X, Y) + _positions[d],
                    color);
            }
        }


        private void Draw_Typewriter(SpriteBatch batch) {
            var font = Style.Fonts.Get(Context);
            var color = Style.TextColors.Get(Context);

            for (int d = 0; d < _lines.Length; d++) {
                // If we've already completed the line, just draw it.
                if (_lastCompleteLine >= d) {
                    DrawShadows(batch, d, new Vector2(X, Y));
                    batch.DrawString(
                        font, _lines[d],
                        new Vector2(X, Y) + _positions[d],
                        color);
                }

                else if (_lastCompleteLine == d - 1) {
                    _framesToType--;

                    if (_framesToType <= 0) {
                        _drawCharAt++;
                        _framesToType = Style.TextEffectRate;
                    }

                    string substring = string.IsNullOrEmpty(_lines[d]) ?
                        string.Empty :
                        _lines[d].Substring(0, _drawCharAt);

                    DrawShadows(batch, d, new Vector2(X, Y));
                    batch.DrawString(
                        font, substring,
                        new Vector2(X, Y) + _positions[d],
                        color);
                    if (_drawCharAt >= _lines[d].Length) {
                        _drawCharAt = 0;
                        _lastCompleteLine = d;
                    }
                }
            }
        }


        private void Draw_Normal(SpriteBatch batch, Rectangle parentRect) {
            Vector2 where = new Vector2(X + parentRect.X, Y + parentRect.Y);
            var font = Style.Fonts.Get(Context);
            var color = Style.TextColors.Get(Context);

            for (int d = 0; d < _lines.Length; d++) {
                DrawShadows(batch, d, where);
                batch.DrawString(
                    font, _lines[d],
                    where + _positions[d],
                    color);
            }
        }


        private void Draw_Typewriter(SpriteBatch batch, Rectangle parentRect) {
            Vector2 where = new Vector2(X + parentRect.X, Y + parentRect.Y);
            var font = Style.Fonts.Get(Context);
            var color = Style.TextColors.Get(Context);

            for (int d = 0; d < _lines.Length; d++) {
                // If we've already completed the line, just draw it.
                if (_lastCompleteLine >= d) {
                    DrawShadows(batch, d, where);
                    batch.DrawString(
                        font, _lines[d],
                        where + _positions[d],
                        color);
                }

                else if (_lastCompleteLine == d - 1) {
                    _framesToType--;

                    if (_framesToType <= 0) {
                        _drawCharAt++;
                        _framesToType = Style.TextEffectRate;
                    }

                    string substring = string.IsNullOrEmpty(_lines[d]) ? 
                        string.Empty : 
                        _lines[d].Substring(0, _drawCharAt);

                    DrawShadows(batch, d, where);
                    batch.DrawString(
                        font, substring,
                        where + _positions[d],
                        color);
                    if (_drawCharAt >= _lines[d].Length) {
                        _drawCharAt = 0;
                        _lastCompleteLine = d;
                    }
                }
            }
        }


        private void DrawShadows(SpriteBatch batch, int lineIndex, Vector2 where) {
            if (string.IsNullOrEmpty(_lines[lineIndex]))
                return;

            var scadu = Style.TextShadow.Get(Context);

            if (!scadu.IsVisible)
                return;

            var font = Style.Fonts.Get(Context);
            string substring = (Style.TextEffect.Get(Context) == TextEffects.Typewriter &&_lastCompleteLine < lineIndex) ?
                _lines[lineIndex].Substring(0, _drawCharAt) :
                _lines[lineIndex];

            foreach (var s in scadu.Distances) {
                if (s != Vector2.Zero) {
                    batch.DrawString(font, substring,
                        where + _positions[lineIndex] + s,
                        scadu.Color);
                }
            }
        }
    }
}
