
using System.ComponentModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonODGE.UI.Styles {
    public class StyleSheet : IChangeTracking {
        public bool IsChanged { get; private set; }


        ///////////////////
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
        
        

        //////////////////////////////
        //// Background & Borders ////

        /// <summary>
        /// If true, only the four corners of the Borders NinePatch will be drawn.
        /// If false, all nine Borders segments will be drawn.
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
        /// Background texture which fills Component dimensions.
        /// Can be set to null for no background.
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
        /// Background texture color. 
        /// Set to Color.White to match original image.
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
        /// Border texture which, uh, borders Components. 
        /// Can be set to null for no borders.
        /// Border textures are split into 3*3 tiles, 
        /// and thus image dimensions should be divisible by 3.
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
        /// Border texture color. 
        /// Set to Color.White to match original image.
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


        /// <summary>
        /// Texture which masks over inactive Components.
        /// Can be set to null for no inactive mask.
        /// </summary>
        public BGTexture Mask {
            get => _mask;
            set {
                if (value != null) {
                    _mask = value;
                    IsChanged = true;
                }
            }
        }
        private BGTexture _mask;


        /// <summary>
        /// Mask texture color. 
        /// Set to Color.White to match original image.
        /// </summary>
        public Color MaskColor {
            get => _maskColors;
            set {
                if (value != null) {
                    _maskColors = value;
                    IsChanged = true;
                }
            }
        }
        private Color _maskColors;



        //////////////////////////////
        //// Fonts and Text Color ////

        /// <summary>
        /// The fonts of any text displayed by the component.
        /// </summary>
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


        /// <summary>
        /// The colors of any text displayed by the component.
        /// </summary>
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


        /// <summary>
        /// Text shadowing used in StyledText.
        /// </summary>
        public StyleContext<Shadows> TextShadow { 
            get => _shadows;
            set {
                _shadows = value;
                IsChanged = true;
            }
        }
        private StyleContext<Shadows> _shadows;



        //////////////////////////
        //// Padding, Spacing ////

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



        ///////////////////////
        //// Input Mapping ////

        /// <summary>
        /// These Keyboard Keys trigger the Component's OnSubmit().
        /// </summary>
        public Keys[] SubmitKeys {
            get => _submitKeys;
            set {
                _submitKeys = value;
                IsChanged = true;
            }
        }
        private Keys[] _submitKeys;

        /// <summary>
        /// Allows an easy way to set a single value to SubmitKeys.
        /// </summary>
        public Keys SubmitKey {
            set {
                _submitKeys = new[] { value };
                IsChanged = true;
            }
        }


        /// <summary>
        /// These GamePad Buttons trigger the Component's OnSubmit().
        /// </summary>
        public Buttons[] SubmitButtons {
            get => _submitButtons;
            set {
                _submitButtons = value;
                IsChanged = true;
            }
        }
        private Buttons[] _submitButtons;

        /// <summary>
        /// Allows an easy way to set a single value to SubmitButtons.
        /// </summary>
        public Buttons SubmitButton {
            set {
                _submitButtons = new[] { value };
                IsChanged = true;
            }
        }


        /// <summary>
        /// These Keyboard Keys trigger the Component's OnCancel(). 
        /// For most Controls, they also close it.
        /// </summary>
        public Keys[] CancelKeys {
            get => _cancelKeys;
            set {
                _cancelKeys = value;
                IsChanged = true;
            }
        }
        private Keys[] _cancelKeys;

        /// <summary>
        /// Allows an easy way to set a single value to CancelKeys
        /// </summary>
        public Keys CancelKey {
            set {
                _cancelKeys = new[] { value };
                IsChanged = true;
            }
        }


        /// <summary>
        /// These GamePad Buttons trigger the Component's OnCancel(). 
        /// For most Controls, they also close it.
        /// </summary>
        public Buttons[] CancelButtons {
            get => _cancelButtons;
            set {
                _cancelButtons = value;
                IsChanged = true;
            }
        }
        private Buttons[] _cancelButtons;

        /// <summary>
        /// Allows an easy way to set a single value to CancelButtons.
        /// </summary>
        public Buttons CancelButton {
            set {
                _cancelButtons = new[] { value };
                IsChanged = true;
            }
        }


        /// <summary>
        /// If true, Control is closed upon upon pressing CancelKey.
        /// </summary>
        public bool CloseOnCancel { get; set; } // No IsChanged necessary.


        public StyleSheet() {
            _backgrounds = StyleContext<BGTexture>.Default;
            _bgColors = Color.Transparent;

            _borders = StyleContext<NinePatch>.Default;
            _borderColors = Color.Transparent;

            _fonts = StyleContext<SpriteFont>.Default;
            _textColors = new[] { Color.White, Color.Gold };
            
            _alignH = AlignmentsH.LEFT;
            _alignV = AlignmentsV.TOP;

            _padding = new Padding(0, 0, 0, 0);
            _spacing = new Spacing(0, 0);
            _shadows = new Shadows(Color.Transparent, 0);

            IsChanged = false;
        }
        
        /// <summary>
        /// Creates an empty StyleSheet.
        /// </summary>
        public static StyleSheet Empty => new StyleSheet();

        /// <summary>
        /// Creates a clone of this StyleSheet.
        /// </summary>
        /// <returns>A new StyleSheet object with this StyleSheet's values.</returns>
        public StyleSheet Clone() {
            StyleSheet clone = new StyleSheet() {
                Backgrounds = Backgrounds.Clone(),      BackgroundColors = BackgroundColors.Clone(),
                Borders = Borders.Clone(),              BorderColors = BorderColors.Clone(),
                Mask = Mask,                            MaskColor = MaskColor,

                Fonts = Fonts.Clone(),                  TextColors = TextColors.Clone(),

                AlignH = AlignH,                        AlignV = AlignV,
                Padding = Padding,                      Spacing = Spacing,

                SubmitButtons = SubmitButtons,            SubmitKeys = SubmitKeys,
                CancelButtons = CancelButtons,            CancelKeys = CancelKeys,
                CloseOnCancel = CloseOnCancel,

                TextShadow = TextShadow.Clone(),
                DrawOnlyCorners = DrawOnlyCorners,
                IsChanged = false
            };
            return clone;
        }

        public void RegisterChanges() { IsChanged = true; }
        public void AcceptChanges() { IsChanged = false; }
    }
}
