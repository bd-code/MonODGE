using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;
using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A control for selecting a bounded integer value.
    /// </summary>
    public class NumericWheel : OdgeWheel {
        public int Value {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, MinValue, MaxValue); }
        }
        private int _value;

        protected override string ValueToString => Value.ToString();


        public int Step { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }


        public NumericWheel(StyleSheet style,
        int minVal = 0, int maxVal = int.MaxValue, 
        int step = 1, int initval = 0) : base(style) {
            MinValue = minVal;
            MaxValue = maxVal;
            Step = step;
            Value = initval;
            Layout();
            Size = new Point(MinWidth, MinHeight);
        }


        /// <summary>
        /// This is called when Value is incremented by pressing Keys.Right.
        /// </summary>
        protected void OnIncrement() { Incremented?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Incremented;


        /// <summary>
        /// This is called when Value is decremented by pressing Keys.Left.
        /// </summary>
        protected void OnDecrement() { Decremented?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Decremented;


        public override void Update() {
            if (IsSubmitPressed) {
                OnSubmit();
            }

            if ((OdgeIO.IsCommandPress("ODGE_UI_LEFT") || OdgeIO.IsCommandPress("ODGE_UI_DOWN")) 
            && Value-Step >= MinValue) {
                Decrement();
                Layout(); // Because Value width might change.
            }

            else if ((OdgeIO.IsCommandPress("ODGE_UI_RIGHT") || OdgeIO.IsCommandPress("ODGE_UI_UP")) 
            && Value+Step <= MaxValue) {
                Increment();
                Layout(); // Because Value width might change.
            }

            else if (IsCancelPressed) {
                OnCancel();
            }

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);

            batch.DrawString(Style.Fonts.Normal, Value.ToString(), _uiPositions[1], Style.TextColors.Normal);

            // Draw Prompts
            if (_liprompt != null || _riprompt != null) {
                if (Value > MinValue)
                    _liprompt.Draw(batch, _uiPositions[0], Style.TextColors.Footer);
                if (Value < MaxValue)
                    _riprompt.Draw(batch, _uiPositions[2], Style.TextColors.Footer);
            }
            else {
                if (Value > MinValue)
                    batch.DrawString(Style.Fonts.Footer, _lsprompt, _uiPositions[0], Style.TextColors.Footer);
                if (Value < MaxValue)
                    batch.DrawString(Style.Fonts.Footer, _rsprompt, _uiPositions[2], Style.TextColors.Footer);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);

            batch.DrawString(
                Style.Fonts.Normal, Value.ToString(), 
                _uiPositions[1] + parentRect.Location.ToVector2(),
                Style.TextColors.Normal);

            // Draw Prompts
            if (_liprompt != null || _riprompt != null) {
                if (Value > MinValue)
                    _liprompt.Draw(batch, _uiPositions[0] + parentRect.Location.ToVector2(), Style.TextColors.Footer);
                if (Value < MaxValue)
                    _riprompt.Draw(batch, _uiPositions[2] + parentRect.Location.ToVector2(), Style.TextColors.Footer);
            }
            else {
                if (Value > MinValue)
                    batch.DrawString(
                        Style.Fonts.Footer, _lsprompt,
                        _uiPositions[0] + parentRect.Location.ToVector2(),
                        Style.TextColors.Footer);

                if (Value < MaxValue)
                    batch.DrawString(
                        Style.Fonts.Footer, _rsprompt,
                        _uiPositions[2] + parentRect.Location.ToVector2(),
                        Style.TextColors.Footer);
            }
        }


        public void Increment() { Increment(Step); }
        public void Increment(int val) {
            Value += val;
            OnIncrement();
            Layout(); // Because Value width might change.
        }

        public void Decrement() { Decrement(Step); }
        public void Decrement(int val) {
            Value -= val;
            OnDecrement();
            Layout(); // Because Value width might change.
        }


        protected override Vector2 GetMaxValueSize() {
            return Style.Fonts?.Normal?.MeasureString(MaxValue.ToString()) ?? Vector2.Zero;
        }
    }
}
