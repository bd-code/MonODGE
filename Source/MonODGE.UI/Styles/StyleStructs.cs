using Microsoft.Xna.Framework;

namespace MonODGE.UI.Styles {
    /// <summary>
    /// Represents 4-sided inner padding. 
    /// </summary>
    public struct Padding {
        private int _top, _left, _right, _bottom;
        public int Top => _top;
        public int Left => _left;
        public int Right => _right;
        public int Bottom => _bottom;

        public Padding(int all) { _top = _left = _right = _bottom = all; }
        public Padding(int leftright, int topbottom) {
            _left = _right = leftright;
            _top = _bottom = topbottom;
        }
        public Padding(int top, int right, int bottom, int left) {
            _top = top; _right = right;
            _bottom = bottom; _left = left;
        }
    }


    /// <summary>
    /// Represents inner space around sub-components.
    /// </summary>
    public struct Spacing {
        private int _h, _v;
        public int Horizontal => _h;
        public int Vertical => _v;

        public Spacing(int all) { _h = _v = all; }
        public Spacing(int horizontal, int vertical) {
            _h = horizontal;
            _v = vertical;
        }
    }


    public struct Shadows {
        public Color Color { get; private set; }
        public Vector2[] Distances { get; private set; }
        public bool IsVisible => Color != Color.Transparent && Distances.Length > 0;

        public Shadows(Color shadowColor, int glow = 1) {
            if (shadowColor == Color.Transparent || glow < 1) {
                Color = Color.Transparent;
                Distances = new Vector2[0];
            }
            else {
                Color = shadowColor;
                Distances = new Vector2[] {
                    new Vector2(-glow, -glow),
                    new Vector2(-glow, glow),
                    new Vector2(glow, -glow),
                    new Vector2(glow, glow)
                };
            }
        }

        public Shadows(Color shadowColor, Vector2[] shadows) {
            Color = shadowColor; 
            if (Color == Color.Transparent)
                Distances = new Vector2[0];
            else
                Distances = shadows;
        }
    }
}
