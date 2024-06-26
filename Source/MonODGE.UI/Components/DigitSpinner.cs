﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;
using MonODGE.UI.Styles;
using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A control for selecting a bounded integer value.
    /// </summary>
    public class DigitSpinner : OdgeControl {
        private Vector2[] textPositions;
        private Vector2[] textDimensions;

        public int Value {
            get { return _value; }
            set { _value = MathHelper.Clamp(value, MinValue, MaxValue); }
        }
        private int _value;

        public int Step { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        protected override int MinWidth => 
            Style.Padding.Left + Style.Padding.Right +
            (int)(textDimensions[0].X + textDimensions[1].X + textDimensions[2].X);

        protected override int MinHeight => Style.Padding.Bottom + Style.Padding.Top + (int)(textDimensions?[0].Y ?? 0);


        public DigitSpinner(StyleSheet style,
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

            if ((OdgeIO.IsCommandPress("ODGE_UI_LEFT") || OdgeIO.IsCommandPress("ODGE_UI_DOWN")) && Value-Step >= MinValue) {
                Decrement();
            }

            else if ((OdgeIO.IsCommandPress("ODGE_UI_RIGHT") || OdgeIO.IsCommandPress("ODGE_UI_UP")) && Value+Step <= MaxValue) {
                Increment();
            }

            else if (IsCancelPressed) {
                OnCancel();
            }

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);

            if (Value > MinValue)
                batch.DrawString(Style.Fonts.Footer, "<< ", textPositions[0], Style.TextColors.Footer);

            batch.DrawString(Style.Fonts.Normal, Value.ToString(), textPositions[1], Style.TextColors.Normal);

            if (Value < MaxValue)
                batch.DrawString(Style.Fonts.Footer, " >>", textPositions[2], Style.TextColors.Footer);
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


        public override void Layout() {
            textDimensions = new Vector2[] {
                Style.Fonts?.Normal?.MeasureString("<< ") ?? Vector2.Zero,
                Style.Fonts?.Normal?.MeasureString(_value.ToString()) ?? Vector2.Zero,
                Style.Fonts?.Normal?.MeasureString(" >>") ?? Vector2.Zero
            };

            // All three text components use Absolute Positioning
            // and only need Vertical Positioning.
            // Value is always centered aligned horizontally.
            float ny;
            if (Style.AlignV == StyleSheet.AlignmentsV.TOP)
                ny = Y + Style.Padding.Top;
            else if (Style.AlignV == StyleSheet.AlignmentsV.CENTER)
                ny = Dimensions.Center.Y - (textDimensions[0].Y / 2);
            else // Bottom
                ny = Dimensions.Bottom - textDimensions[0].Y - Style.Padding.Bottom;

            textPositions = new Vector2[] {
                new Vector2(X + Style.Padding.Left, ny),
                new Vector2(Dimensions.Center.X - (textDimensions[1].X / 2), ny),
                new Vector2(Dimensions.Right - textDimensions[2].X - Style.Padding.Right, ny)
            };

            base.Layout();
        }
    }
}
