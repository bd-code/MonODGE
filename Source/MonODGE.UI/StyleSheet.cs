
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonODGE.UI {
    public class StyleSheet : IChangeTracking {
        public bool IsChanged { get; private set; }

        //// Alignment ////

        public enum AlignmentsH { LEFT, CENTER, RIGHT }
        public enum AlignmentsV { TOP, CENTER, BOTTOM }

        /// <summary>
        /// Horizontal alignment for inner text and elements.
        /// </summary>
        public AlignmentsH AlignH {
            get { return _alignH; }
            set {
                _alignH = value;
                IsChanged = true;
            }
        }
        private AlignmentsH _alignH;
        
        /// <summary>
        /// Vertical alignment for inner text and elements.
        /// </summary>
        public AlignmentsV AlignV {
            get { return _alignV; }
            set {
                _alignV = value;
                IsChanged = true;
            }
        }
        private AlignmentsV _alignV;
        
        
        //// Background & Borders ////

        /// <summary>
        /// Background texture which fills Component dimensions. Can be set to null for no background.
        /// </summary>
        public Texture2D Background {
            get { return _background; }
            set {
                _background = value;
                IsChanged = true;
            }
        }
        private Texture2D _background;

        /// <summary>
        /// Border texture color. Set to Color.White to match original image.
        /// </summary>
        public Color BackgroundColor {
            get { return _bgColor; }
            set {
                _bgColor = value;
                IsChanged = true;
            }
        }
        private Color _bgColor;


        /// <summary>
        /// Border texture which, uh, borders Components. Can be set to null for no borders.
        /// Border textures are split into 3*3 tiles, and thus image dimensions should be divisible by 3.
        /// </summary>
        public NinePatch Borders {
            get { return _borders; }
            set {
                _borders = value;
                IsChanged = true;
            }
        }
        private NinePatch _borders;


        /// <summary>
        /// Border texture color. Set to Color.White to match original image.
        /// </summary>
        public Color BorderColor {
            get { return _borderColor; }
            set {
                _borderColor = value;
                IsChanged = true;
            }
        }
        private Color _borderColor;


        //// Fonts and Text Color ////

        /// <summary>
        /// Header text font.
        /// </summary>
        public SpriteFont HeaderFont {
            get { return _headerFont; }
            set {
                _headerFont = value;
                IsChanged = true;
            }
        }
        private SpriteFont _headerFont;

        /// <summary>
        /// Header text color.
        /// </summary>
        public Color HeaderColor {
            get { return _headerColor; }
            set {
                _headerColor = value;
                IsChanged = true;
            }
        }
        private Color _headerColor;

        /// <summary>
        /// Main text font.
        /// </summary>
        public SpriteFont Font {
            get { return _font; }
            set {
                _font = value;
                if (_headerFont == null)
                    _headerFont = value;
                if (_footerFont == null)
                    _footerFont = value;
                IsChanged = true;
            }
        }
        private SpriteFont _font;

        /// <summary>
        /// Main text color.
        /// </summary>
        public Color TextColor {
            get { return _textColor; }
            set {
                _textColor = value;
                if (_headerColor == null)
                    _headerColor = value;
                if (_footerColor == null)
                    _footerColor = value;
                IsChanged = true;
            }
        }
        private Color _textColor;

        /// <summary>
        /// Footer text font.
        /// </summary>
        public SpriteFont FooterFont {
            get { return _footerFont; }
            set {
                _footerFont = value;
                IsChanged = true;
            }
        }
        private SpriteFont _footerFont;

        /// <summary>
        /// Footer text color.
        /// </summary>
        public Color FooterColor {
            get { return _footerColor; }
            set {
                _footerColor = value;
                IsChanged = true;
            }
        }
        private Color _footerColor;
        
        /// <summary>
        /// For menus and other controls, selected item text is displayed in this color rather than TextColor.
        /// </summary>
        public Color SelectedTextColor {
            get { return _selectedColor; }
            set {
                _selectedColor = value;
                IsChanged = true;
            }
        }
        private Color _selectedColor;

        /// <summary>
        /// For menus and other controls, unselected item text is displayed in this color rather than TextColor.
        /// </summary>
        public Color UnselectedTextColor {
            get { return _unselectedColor; }
            set {
                _unselectedColor = value;
                IsChanged = true;
            }
        }
        private Color _unselectedColor;


        //// Text Shadows ////

        /// <summary>
        /// A Vector[4] array that determines the position of the four text shadows. 
        /// Padding order: 0-TopLeft, 1-TopRight, 2-BottomLeft, 3-BottomRight.
        /// </summary>
        public Vector2[] TextShadows {
            get { return _textShadows; }
            set {
                if (value.Length >= 4) {
                    _textShadows[0] = value[0];
                    _textShadows[1] = value[1];
                    _textShadows[2] = value[2];
                    _textShadows[3] = value[3];
                }
                else if (value.Length == 3) {
                    _textShadows[0] = value[0];
                    _textShadows[1] = _textShadows[3] = value[1];
                    _textShadows[2] = value[2];
                }
                else if (value.Length == 2) {
                    _textShadows[0] = _textShadows[2] = value[0];
                    _textShadows[1] = _textShadows[3] = value[1];
                }
                else if (value.Length == 1) {
                    _textShadows[0] = _textShadows[1] = _textShadows[2] = _textShadows[3] = value[0];
                }
                IsChanged = true;
            }
        }
        private Vector2[] _textShadows;

        /// <summary>
        /// Sets top-left text shadow position.
        /// </summary>
        public Vector2 TextShadowTopLeft {
            get { return _textShadows[0]; }
            set {
                _textShadows[0] = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Sets top-right text shadow position.
        /// </summary>
        public Vector2 TextShadowTopRight {
            get { return _textShadows[1]; }
            set {
                _textShadows[1] = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Sets bottom-left text shadow position.
        /// </summary>
        public Vector2 TextShadowBottomLeft {
            get { return _textShadows[2]; }
            set {
                _textShadows[2] = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Sets bottom-right text shadow position.
        /// </summary>
        public Vector2 TextShadowBottomRight {
            get { return _textShadows[3]; }
            set {
                _textShadows[3] = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Text outline color.
        /// </summary>
        public Color TextShadowColor {
            get { return _textShadowColor; }
            set {
                _textShadowColor = value;
                IsChanged = true;
            }
        }
        private Color _textShadowColor;

        /// <summary>
        /// TextShadow Preset: Adds a 1px "glow" around text. Great for readability.
        /// </summary>
        public static Vector2[] TextShadow_1pxGlow {
            get { return new[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) }; }
        }


        //// Padding and Spacing ////

        /// <summary>
        /// Represents the Component's inner padding. 
        /// </summary>
        public Padding Padding { 
            get { return _padding; }
            set { 
                _padding = value; 
                IsChanged = true; 
            }
        }
        private Padding _padding;

        /// <summary>
        /// Determines distance between child Components in OdgeComponents that have them.
        /// </summary>
        public Spacing Spacing {
            get { return _spacing; }
            set { 
                _spacing = value;
                IsChanged = true;
            }
        }
        private Spacing _spacing;
       

        //// Input Mapping ////

        /// <summary>
        /// This Keyboard Key triggers the Component's OnSubmit().
        /// </summary>
        public Keys SubmitKey {
            get { return _submitKey; }
            set {
                _submitKey = value;
                IsChanged = true;
            }
        }
        private Keys _submitKey;

        /// <summary>
        /// This GamePad Button triggers the Component's OnSubmit().
        /// </summary>
        public Buttons SubmitButton {
            get { return _submitButton; }
            set {
                _submitButton = value;
                IsChanged = true;
            }
        }
        private Buttons _submitButton;

        /// <summary>
        /// This Keyboard Key triggers the Component's OnCancel(). 
        /// For most Controls, it also closes it.
        /// </summary>
        public Keys CancelKey {
            get { return _cancelKey; }
            set {
                _cancelKey = value;
                IsChanged = true;
            }
        }
        private Keys _cancelKey;

        /// <summary>
        /// This GamePad Button triggers the Component's OnCancel(). 
        /// For most Controls, it also closes it.
        /// </summary>
        public Buttons CancelButton {
            get { return _cancelButton; }
            set {
                _cancelButton = value;
                IsChanged = true;
            }
        }
        private Buttons _cancelButton;

        /// <summary>
        /// If true, Control is closed upon upon pressing CancelKey.
        /// </summary>
        public bool CloseOnCancel { get; set; } // No IsChanged necessary.


        public StyleSheet() {
            BackgroundColor = Color.Transparent;            
            BorderColor = Color.White;

            TextColor = Color.White;
            SelectedTextColor = Color.Gold;
            UnselectedTextColor = Color.Gray;
            
            AlignH = AlignmentsH.LEFT;
            AlignV = AlignmentsV.TOP;

            _padding = new Padding(0, 0, 0, 0);
            _spacing = new Spacing(0, 0);
            _textShadows = new Vector2[4] {
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero
            };

            IsChanged = false;
        }
        
        /// <summary>
        /// Creates an empty StyleSheet.
        /// </summary>
        public static StyleSheet Empty { get { return new StyleSheet(); } }

        /// <summary>
        /// Creates a clone of this StyleSheet.
        /// </summary>
        /// <returns>A new StyleSheet object with this StyleSheet's values.</returns>
        public StyleSheet Clone() {
            StyleSheet clone = new StyleSheet();
            clone.Background = Background;              clone.BackgroundColor = BackgroundColor;
            clone.BorderColor = BorderColor;            clone.Borders = Borders;
            clone.CancelButton = CancelButton;          clone.CancelKey = CancelKey;
            clone.CloseOnCancel = CloseOnCancel;
            clone.Font = Font;
            clone.FooterColor = FooterColor;            clone.FooterFont = FooterFont;
            clone.HeaderColor = HeaderColor;            clone.HeaderFont = HeaderFont;
            clone.Padding = Padding;
            clone.SelectedTextColor = SelectedTextColor;    clone.UnselectedTextColor = UnselectedTextColor;
            clone.Spacing = Spacing;
            clone.SubmitButton = SubmitButton;          clone.SubmitKey = SubmitKey;
            clone.AlignH = AlignH;              clone.AlignV = AlignV;
            clone.TextColor = TextColor;
            clone.TextShadowColor = TextShadowColor;    clone.TextShadows = TextShadows;
            clone.IsChanged = false;
            return clone;
        }

        public void RegisterChanges() { IsChanged = true; }
        public void AcceptChanges() { IsChanged = false; }
    }


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
            Top = top;             Right = right;
            Bottom = bottom;       Left = left;
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
}
