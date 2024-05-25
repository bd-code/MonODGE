using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A graphical progress bar.
    /// </summary>
    public class ProgressBar : OdgeComponent {
        private Rectangle _filledRect;
        private Rectangle _unfilledRect;

        private StyledText _stytex;
        private Point textPoint;


        public enum ProgressBarTextDisplayMode {
            None, Value, Fraction, Percent, Custom
        }
        public ProgressBarTextDisplayMode TextMode { get; set; }
        private string _customText;

        protected override int MinWidth =>
            Style.Padding.Left +
            (_stytex?.Width ?? 0) +
            Style.Padding.Right;

        protected override int MinHeight =>
            Style.Padding.Top +
            (_stytex?.Height ?? 0) +
            Style.Padding.Bottom;

        /// <summary>
        /// Value clamps to min and max, so if setting all three properties, set Value last.
        /// </summary>
        public float Value {
            get => _value; 
            set {
                _value = MathHelper.Clamp(value, _min, _max);
                OnValueChanged();
            }
        }
        private float _value;

        
        public float Minimum {
            get => _min;
            set {
                if (value >= _max) {
                    string e = $"ProgressBar.Minimum passed value {value} not less than than Maximum {_max}";
                    throw new ArgumentOutOfRangeException("value", e);
                }
                _min = value;
                OnLimitsChanged();
            }
        }
        private float _min;

        
        public float Maximum {
            get => _max;
            set { 
                if (value <= _min) {
                    string e = $"ProgressBar.Maximum passed value {value} not greater than than Minimum {_min}";
                    throw new ArgumentOutOfRangeException("value", e);
                }
                _max = value;
                OnLimitsChanged();
            }
        }
        private float _max;


        public float Step {
            get => _step;
            set {
                _step = MathHelper.Min(value, _max - _min);
                OnStepChanged();
            }
        }
        private float _step;


        public float Percent {
            get => (_value - _min) / (_max - _min);
            set {
                if (value >= 0.0f && value <= 1.0f)
                    Value = ((_max - _min) * value) + _min;
                else {
                    string e = $"ProgressBar.Percent passed value {value} outside range 0.0f to 1.0f.";
                    throw new ArgumentOutOfRangeException("value", e);
                }
            }
        }

        /// <summary>
        /// If true, draws the incomplete (right) side of the progress bar 
        /// using Style.BackgroundColors.Normal.
        /// </summary>
        public bool DrawIncompleteBar { get; set; }


        public ProgressBar(StyleSheet style, Rectangle area, 
        float startValue = 0, float maxValue = 100, float minValue = 0, 
        bool drawIncomplete = false, string text = null) {
            Style = style;

            if (maxValue <= minValue) {
                string e = $"ProgressBar: maxValue {maxValue} is not greater than minValue {minValue}.";
                throw new ArgumentOutOfRangeException("maxValue", e);
            }

            _min = minValue;
            _max = maxValue;
            // Keep Value last otherwise the Clamp fails.
            _value = MathHelper.Clamp(startValue, _min, _max);
            DrawIncompleteBar = drawIncomplete;

            if (text != null) {
                _customText = text;
                TextMode = ProgressBarTextDisplayMode.Custom;
                _stytex = new StyledText(Style, text);
            }
            else { TextMode = ProgressBarTextDisplayMode.None; }

            Location = area.Location;
            Size = area.Size;
            Layout();
        }


        protected void OnValueChanged() {
            IsMessy = true;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ValueChanged;

        protected void OnLimitsChanged() {
            IsMessy = true;
            LimitsChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler LimitsChanged;

        protected void OnStepChanged() {
            StepChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StepChanged;


        public override void Update() {
            if ((_step < 0.0f && _min < _value) || (_step > 0.0f && _value < _max)) {
                Value = _value + _step;
            }

            // Now Text:
            if (TextMode == ProgressBarTextDisplayMode.None) {
                if (_stytex != null) { 
                    _stytex = null;
                    IsMessy = true;
                }
            }
            else if (TextMode == ProgressBarTextDisplayMode.Value) {
                if (_stytex == null || _stytex.Text != Value.ToString()) { 
                    _stytex = new StyledText(Style, Value.ToString());
                    IsMessy = true;
                }
            }
            else if (TextMode == ProgressBarTextDisplayMode.Fraction) {
                string f = $"{Value}/{Maximum}";
                if (_stytex == null || _stytex.Text != f) { 
                    _stytex = new StyledText(Style, f);
                    IsMessy = true;
                }
            }
            else if (TextMode == ProgressBarTextDisplayMode.Percent) {
                string p = $"{Percent*100}%";
                if (_stytex == null || _stytex.Text != p) {
                    _stytex = new StyledText(Style, p);
                    IsMessy = true;
                }
            }
            else if (TextMode == ProgressBarTextDisplayMode.Custom) {
                if (_stytex == null || _stytex.Text != _customText) {
                    _stytex = new StyledText(Style, _customText);
                    IsMessy = true;
                }
            }
            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            Style.Backgrounds.Active?.Draw(batch, _filledRect, Style.BackgroundColors.Active);
            if (DrawIncompleteBar)
                Style.Backgrounds.Normal?.Draw(batch, _unfilledRect, Style.BackgroundColors.Normal);

            _stytex?.Draw(batch, new Rectangle(Location + textPoint, Size));
        }

        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            Style.Backgrounds.Active?.Draw(batch, 
                new Rectangle(parentRect.Location + _filledRect.Location, _filledRect.Size), 
                Style.TextColors.Active);
            if (DrawIncompleteBar)
                Style.Backgrounds.Normal?.Draw(batch, 
                    new Rectangle(parentRect.Location + _unfilledRect.Location, _unfilledRect.Size), 
                    Style.TextColors.Normal);
            
            _stytex?.Draw(batch, new Rectangle(parentRect.Location + Location + textPoint, Size));
        }


        public override void Layout() {
            if (_stytex != null) {
                if (_stytex.IsMessy)
                    _stytex.Layout();
                textPoint = Utilities.LayoutHelper.AlignToPoint(this, _stytex);
            }

            _filledRect = new Rectangle(X, Y, (int)(Width * Percent), Height);

            _unfilledRect = new Rectangle(
                _filledRect.Right + 1, Y,
                Width - _filledRect.Width, Height);

            base.Layout();
        }


        public void SetTextMode(ProgressBarTextDisplayMode mode, string customText = "") { 
            TextMode = mode;
            if (mode == ProgressBarTextDisplayMode.Custom) { 
                _customText = customText;
            }
            else { _customText = null; }
        }
    }
}
