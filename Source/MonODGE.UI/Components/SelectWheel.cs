using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;
using MonODGE.UI.Styles;

namespace MonODGE.UI.Components {
    /// <summary>
    /// ...
    /// </summary>
    /// <typeparam name="T">Any object easily identified via its ToString() method.</typeparam>
    public class SelectWheel<T> : OdgeWheel {
        private T[] _options;

        /// <summary>
        /// The index of the selected option.
        /// </summary>
        public int SelectedIndex {
            get => _selectedIndex;
            set {
                if (value < 0 || value >= _options.Length) {
                    string e = $"SelectWheel: SelectedIndex = {value} while options.Length = {_options.Length};";
                    throw new ArgumentOutOfRangeException(e);
                }
                _selectedIndex = value;
            }
        }
        private int _selectedIndex;


        public T Value => _options[_selectedIndex];
        protected override string ValueToString => Value.ToString();


        public SelectWheel(StyleSheet style, T[] options, int selectedOption = 0) : base(style) {
            _options = options;
            if (selectedOption < 0 || selectedOption >= options.Length) {
                string e = $"SelectWheel: selectedOption = {selectedOption} while options.Length = {options.Length};";
                throw new ArgumentOutOfRangeException(nameof(selectedOption), e);
            }
            _selectedIndex = selectedOption;

            Layout();
            Size = new Point(MinWidth, MinHeight);
        }


        /// <summary>
        /// This is called when Value is incremented by pressing Keys.Right.
        /// </summary>
        protected void OnValueChanged() { ValueChanged?.Invoke(this, EventArgs.Empty); }
        public event EventHandler ValueChanged;


        public override void Update() {
            if (IsSubmitPressed) {
                OnSubmit();
            }

            if ((OdgeIO.IsCommandPress("ODGE_UI_LEFT") || OdgeIO.IsCommandPress("ODGE_UI_DOWN")) 
            && _selectedIndex > 0) {
                _selectedIndex--;
                OnValueChanged();
                Layout(); // Because Value width might change.
            }

            else if ((OdgeIO.IsCommandPress("ODGE_UI_RIGHT") || OdgeIO.IsCommandPress("ODGE_UI_UP")) 
            && _selectedIndex < _options.Length - 1) {
                _selectedIndex++;
                OnValueChanged();
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
                if (_selectedIndex > 0)
                    _liprompt.Draw(batch, _uiPositions[0], Style.TextColors.Footer);
                if (_selectedIndex < _options.Length - 1)
                    _riprompt.Draw(batch, _uiPositions[2], Style.TextColors.Footer);
            }
            else {
                if (_selectedIndex > 0)
                    batch.DrawString(Style.Fonts.Footer, _lsprompt, _uiPositions[0], Style.TextColors.Footer);
                if (_selectedIndex < _options.Length - 1)
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
                if (_selectedIndex > 0)
                    _liprompt.Draw(batch, _uiPositions[0] + parentRect.Location.ToVector2(), Style.TextColors.Footer);
                if (_selectedIndex < _options.Length - 1)
                    _riprompt.Draw(batch, _uiPositions[2] + parentRect.Location.ToVector2(), Style.TextColors.Footer);
            }
            else {
                if (_selectedIndex > 0)
                    batch.DrawString(
                        Style.Fonts.Footer, _lsprompt,
                        _uiPositions[0] + parentRect.Location.ToVector2(),
                        Style.TextColors.Footer);

                if (_selectedIndex < _options.Length - 1)
                    batch.DrawString(
                        Style.Fonts.Footer, _rsprompt,
                        _uiPositions[2] + parentRect.Location.ToVector2(),
                        Style.TextColors.Footer);
            }
        }


        protected override Vector2 GetMaxValueSize() {
            Vector2 size = Vector2.Zero;
            Vector2 temp = Vector2.Zero;
            foreach (var option in _options) {
                temp = Style.Fonts?.Normal?.MeasureString(option.ToString()) ?? Vector2.Zero;
                if (temp.X > size.X) size = temp;
            }
            return size;
        }
    }
}
