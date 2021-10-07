using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A graphical progress bar.
    /// </summary>
    public class ProgressBar : OdgeComponent {
        private Rectangle _filledRect;
        private Rectangle _unfilledRect;

        private uint _value;
        public uint Value {
            get { return _value; }
            set {
                _value = (uint)MathHelper.Min(value, MaxValue);
                OnValueChanged();
            }
        }

        private uint _maxValue;
        public uint MaxValue {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public float Percent {
            get { return (1.0f * Value) / (1.0f * MaxValue); }
            set {
                if (value >= 0.0f)
                    Value = (uint)(MaxValue * value);
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// If true, draws the incomplete (right) side of the progress bar 
        /// using Style.UnselectedTextColor.
        /// </summary>
        public bool DrawIncompleteBar { get; set; }


        public ProgressBar(StyleSheet style, Rectangle area, 
        uint maxValue, uint value = 0, bool drawIncomplete = false) {
            Style = style;
            MaxValue = maxValue;
            Value = value;
            DrawIncompleteBar = drawIncomplete;

            PackToSize(area);
            Layout();
        }


        public void OnValueChanged() {
            IsMessy = true;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler ValueChanged;


        public override void Layout() {
            _filledRect = new Rectangle(X, Y, (int)(Width * Percent), Height);

            _unfilledRect = new Rectangle(
                _filledRect.Right + 1, Y,
                Width - _filledRect.Width, Height);

            base.Layout();
        }


        public override void Draw(SpriteBatch batch) {
            batch.Draw(Style.Background, _filledRect, Style.SelectedTextColor);
            if (DrawIncompleteBar)
                batch.Draw(Style.Background, _unfilledRect, Style.UnselectedTextColor);
        }

        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            batch.Draw(Style.Background, 
                new Rectangle(parentRect.Location + _filledRect.Location, _filledRect.Size), 
                Style.SelectedTextColor);
            if (DrawIncompleteBar)
                batch.Draw(Style.Background, 
                    new Rectangle(parentRect.Location + _unfilledRect.Location, _unfilledRect.Size), 
                    Style.UnselectedTextColor);
        }
    }
}
