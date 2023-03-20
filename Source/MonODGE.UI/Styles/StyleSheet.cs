
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonODGE.UI.Styles {
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
        public BGTexture Background {
            get { return _background; }
            set {
                _background = value;
                IsChanged = true;
            }
        }
        private BGTexture _background;

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

        public FontStyleContext Fonts {
            get => _fonts;
            set {
                _fonts = value;
                IsChanged = true;
            }
        }
        private FontStyleContext _fonts;

        public ColorStyleContext TextColors {
            get => _textColors;
            set {
                _textColors = value;
                IsChanged = true;
            }
        }
        private ColorStyleContext _textColors;


        //// Padding, Spacing, and Shadows ////

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


        /// <summary>
        /// Text shadowing used in StyledText.
        /// </summary>
        public Shadows TextShadow { 
            get { return _shadows; } 
            set {
                _shadows = value;
                IsChanged = true;
            }
        }
        private Shadows _shadows;


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

            TextColors = new ColorStyleContext(Color.White, Color.Gold);
            
            AlignH = AlignmentsH.LEFT;
            AlignV = AlignmentsV.TOP;

            _padding = new Padding(0, 0, 0, 0);
            _spacing = new Spacing(0, 0);
            _shadows = new Shadows(Color.Transparent, 0);

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
            clone.AlignH = AlignH;                      clone.AlignV = AlignV;
            clone.Background = Background;              clone.BackgroundColor = BackgroundColor;
            clone.BorderColor = BorderColor;            clone.Borders = Borders;

            clone.Fonts = new FontStyleContext(Fonts.Normal, Fonts.Active, Fonts.Header, Fonts.Footer);
            clone.TextColors = new ColorStyleContext(TextColors.Normal, TextColors.Active, TextColors.Header, TextColors.Footer);

            clone.Padding = Padding;
            clone.Spacing = Spacing;

            clone.SubmitButton = SubmitButton;          clone.SubmitKey = SubmitKey;
            clone.CancelButton = CancelButton;          clone.CancelKey = CancelKey;
            clone.CloseOnCancel = CloseOnCancel;
            
            clone.TextShadow = TextShadow;
            clone.IsChanged = false;
            return clone;
        }

        public void RegisterChanges() { IsChanged = true; }
        public void AcceptChanges() { IsChanged = false; }
    }
}
