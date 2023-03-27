
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            get => _alignH;
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
            get => _alignV;
            set {
                _alignV = value;
                IsChanged = true;
            }
        }
        private AlignmentsV _alignV;
        
        
        //// Background & Borders ////

        /// <summary>
        /// If true, only the four corners of the Borders NinePatch will be drawn.
        /// If false the entire all nine Borders segments will be drawn.
        /// </summary>
        public bool DrawOnlyCorners {
            get => _drawOnlyCorners;
            set { 
                _drawOnlyCorners = value; 
                IsChanged = true; 
            }
        }
        private bool _drawOnlyCorners;

        /// <summary>
        /// Background texture which fills Component dimensions. Can be set to null for no background.
        /// </summary>
        public StyleContext<BGTexture> Backgrounds {
            get => _backgrounds;
            set {
                if (value != null) {
                    _backgrounds = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<BGTexture> _backgrounds;

        /// <summary>
        /// Border texture color. Set to Color.White to match original image.
        /// </summary>
        public StyleContext<Color> BackgroundColors {
            get => _bgColors;
            set {
                if (value != null) {
                    _bgColors = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<Color> _bgColors;


        /// <summary>
        /// Border texture which, uh, borders Components. Can be set to null for no borders.
        /// Border textures are split into 3*3 tiles, and thus image dimensions should be divisible by 3.
        /// </summary>
        public StyleContext<NinePatch> Borders {
            get => _borders;
            set {
                if (value != null) {
                    _borders = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<NinePatch> _borders;


        /// <summary>
        /// Border texture color. Set to Color.White to match original image.
        /// </summary>
        public StyleContext<Color> BorderColors {
            get => _borderColors;
            set {
                if (value != null) {
                    _borderColors = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<Color> _borderColors;


        //// Fonts and Text Color ////

        public StyleContext<SpriteFont> Fonts {
            get => _fonts;
            set {
                if (value != null) {
                    _fonts = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<SpriteFont> _fonts;

        public StyleContext<Color> TextColors {
            get => _textColors;
            set {
                if (value != null) {
                    _textColors = value;
                    IsChanged = true;
                }
            }
        }
        private StyleContext<Color> _textColors;


        //// Padding, Spacing, and Shadows ////

        /// <summary>
        /// Represents the Component's inner padding. 
        /// </summary>
        public Padding Padding { 
            get => _padding;
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
            get => _spacing;
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
            get => _shadows;
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
            get => _submitKey;
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
            get => _submitButton;
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
            get => _cancelKey;
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
            get => _cancelButton;
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
            Backgrounds = new StyleContext<BGTexture>(null);
            BackgroundColors = new StyleContext<Color>(Color.Transparent);
            Borders = new StyleContext<NinePatch>(null);
            BorderColors = new StyleContext<Color>(Color.White);

            TextColors = new StyleContext<Color>(Color.White, Color.Gold);
            
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
            StyleSheet clone = new StyleSheet() {
                AlignH = AlignH,                        AlignV = AlignV,
                Backgrounds = Backgrounds.Clone(),      BackgroundColors = BackgroundColors.Clone(),
                Borders = Borders.Clone(),              BorderColors = BorderColors.Clone(),

                Fonts = Fonts.Clone(),                  TextColors = TextColors.Clone(),

                Padding = Padding,                      Spacing = Spacing,

                SubmitButton = SubmitButton,            SubmitKey = SubmitKey,
                CancelButton = CancelButton,            CancelKey = CancelKey,
                CloseOnCancel = CloseOnCancel,

                TextShadow = TextShadow,
                DrawOnlyCorners = DrawOnlyCorners,
                IsChanged = false
            };
            return clone;
        }

        public void RegisterChanges() { IsChanged = true; }
        public void AcceptChanges() { IsChanged = false; }
    }
}
