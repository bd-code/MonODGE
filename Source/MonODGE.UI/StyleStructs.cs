using Microsoft.Xna.Framework;

namespace MonODGE.UI {
    /// <summary>
    /// Represents 4-sided inner padding. 
    /// </summary>
    public struct Padding {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public Padding(int all) { Top = Right = Bottom = Left = all; }
        public Padding(int leftright, int topbottom) {
            Left = Right = leftright;
            Top = Bottom = topbottom;
        }
        public Padding(int top, int right, int bottom, int left) {
            Top = top; Right = right;
            Bottom = bottom; Left = left;
        }
    }


    /// <summary>
    /// Represents inner space around sub-components.
    /// </summary>
    public struct Spacing {
        public int Horizontal { get; set; }
        public int Vertical { get; set; }

        public Spacing(int all) { Horizontal = Vertical = all; }
        public Spacing(int horizontal, int vertical) {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }


    public struct Shadows {
        public Color Color { get; private set; }
        public Vector2[] Distances { get; private set; }

        public Shadows(Color shadowColor, int glow) {
            Color = shadowColor;
            if (Color == Color.Transparent || glow < 1)
                Distances = new Vector2[0];
            else {
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
