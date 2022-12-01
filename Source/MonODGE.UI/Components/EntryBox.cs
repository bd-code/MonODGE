using System;
using System.Text.RegularExpressions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A text input component.
    /// </summary>
    public class EntryBox : OdgeControl {
        public enum CharsAllowed {
            Any, Alpha, Numeric, AlphaNumeric
        }

        private KeyboardState _keystate;
        private KeyboardState _oldstate;

        private string _text;
        public string Text {
            get { return _text; }
            set {
                _text = value;
                OnTextChanged();
            }
        }

        public int TextLength => Text?.Length ?? 0;
        public int MaxLength { get; private set; }
        public CharsAllowed InputRules { get; private set; }

        protected override int MinWidth => (int)charSize.Y * 8;
        protected override int MinHeight => Style.Padding.Bottom + Style.Padding.Top + (int)charSize.Y;
        
        private Vector2 textPosition;
        private Vector2 charSize;


        public EntryBox(StyleSheet style, CharsAllowed allowed = CharsAllowed.Any, 
        string text = "", int maxLength = 255) : base(style) {
            Text = text ?? string.Empty;
            MaxLength = maxLength;
            InputRules = allowed;

            _keystate = Keyboard.GetState();
            _oldstate = _keystate;

            Layout();
        }


        public void OnTextChanged() {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler TextChanged;


        public override void Update() {
            _oldstate = _keystate;
            _keystate = Keyboard.GetState();

            bool isShiftDown = 
                (!_keystate.CapsLock && 
                    (_keystate.IsKeyDown(Keys.LeftShift) || _keystate.IsKeyDown(Keys.RightShift)))
                    || 
                (_keystate.CapsLock && 
                    !(_keystate.IsKeyDown(Keys.LeftShift) || _keystate.IsKeyDown(Keys.RightShift)));

            // StyleSheet.SubmitKey should choose a non-AlphaNumeric key, like Keys.Enter, 
            // otherwise that key cannot output a printable char.
            // Do NOT close on submit, as this kills validation scripts.
            if (CheckSubmit)
                OnSubmit();
            else if (CheckCancel)
                OnCancel();

            // Only control char we need is Backspace, as we don't allow cursor movement.
            else if (IsKeyPress(Keys.Back) && TextLength > 0) 
                Text = Text.Remove(TextLength - 1, 1);

            // Handle character input.
            else {
                foreach (Keys kee in _keystate.GetPressedKeys()) {
                    if (TextLength >= MaxLength)
                        break;

                    string k = convertToCharString(kee, isShiftDown);

                    if (charTest(k) && IsKeyPress(kee)) {
                        Text = Text + k;
                        OnTextChanged();
                        break;
                    }
                }
            }

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);
            batch.DrawString(Style.Font, Text, Location.ToVector2() + textPosition, Style.TextColor);
        }
        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);
            batch.DrawString(Style.Font, Text, 
                (Location + parentRect.Location).ToVector2() + textPosition, Style.TextColor);
        }


        public override void Layout() {
            charSize = Style.Font?.MeasureString("M") ?? Vector2.One;

            if (Text != null) {
                // Horizontal: always left, otherwise we would have to recalc
                // textPosition on every keystroke.
                textPosition.X = Style.Padding.Left;

                // Vertical
                if (Style.AlignV == StyleSheet.AlignmentsV.TOP)
                    textPosition.Y = Style.Padding.Top;
                else if (Style.AlignV == StyleSheet.AlignmentsV.CENTER)
                    textPosition.Y = Height / 2 - (charSize.Y / 2);
                else // Bottom
                    textPosition.Y = Height - charSize.Y - Style.Padding.Bottom;
            }
            base.Layout();
        }


        private bool IsKeyPress(Keys kee) {
            return (_keystate.IsKeyDown(kee) && !_oldstate.IsKeyDown(kee));
        }


        private bool isAlpha(string s) {
            return new Regex(@"^[a-zA-Z]$").IsMatch(s);
        }
        private bool isNumeric(string s) {
            return new Regex(@"^[0-9]$").IsMatch(s);
        }
        private bool isAlphaNumeric(string s) {
            return new Regex(@"^[a-zA-Z0-9]$").IsMatch(s);
        }


        private bool charTest(string s) {
            if (InputRules == CharsAllowed.Alpha)
                return isAlpha(s);
            else if (InputRules == CharsAllowed.Numeric)
                return isNumeric(s);
            else if (InputRules == CharsAllowed.AlphaNumeric)
                return isAlphaNumeric(s);
            else
                return new Regex(@"^.$").IsMatch(s);
        }


        private string convertToCharString(Keys kee, bool shift) {
            string k = kee.ToString();

            // Alphabet
            if (isAlpha(k)) {
                if (shift) return k.ToUpper();
                else return k.ToLower();
            }

            // Numbers
            if (new Regex("^D[0-9]$").IsMatch(k)) {
                if (shift) {
                    if (k == "D1") return "!";
                    else if (k == "D2") return "@";
                    else if (k == "D3") return "#";
                    else if (k == "D4") return "$";
                    else if (k == "D5") return "%";
                    else if (k == "D6") return "^";
                    else if (k == "D7") return "&";
                    else if (k == "D8") return "*";
                    else if (k == "D9") return "(";
                    else if (k == "D0") return ")";
                    else return k.Remove(0, 1);
                }

                return k.Remove(0, 1);
            }

            // NumPad
            else if (k.StartsWith("NumPad"))
                return k.Remove(0, 6);
            else if (kee == Keys.Add)
                return "+";
            else if (kee == Keys.Subtract)
                return "-";
            else if (kee == Keys.Multiply)
                return "*";
            else if (kee == Keys.Divide)
                return "/";
            else if (kee == Keys.Decimal)
                return ".";

            // Special Chars
            else if (kee == Keys.OemComma && shift)
                return "<";
            else if (kee == Keys.OemComma && !shift)
                return ",";
            else if (kee == Keys.OemMinus && shift)
                return "_";
            else if (kee == Keys.OemMinus && !shift)
                return "-";
            else if (kee == Keys.OemPeriod && shift)
                return ">";
            else if (kee == Keys.OemPeriod && !shift)
                return ".";
            else if (kee == Keys.OemPlus && shift)
                return "+";
            else if (kee == Keys.OemPlus && !shift)
                return "=";
            else if (kee == Keys.OemQuestion && shift)
                return "?";
            else if (kee == Keys.OemQuestion && !shift)
                return "/";
            else if (kee == Keys.OemSemicolon && shift)
                return ":";
            else if (kee == Keys.OemSemicolon && !shift)
                return ";";
            else if (kee == Keys.OemTilde && shift)
                return "~";
            else if (kee == Keys.OemTilde && !shift)
                return "`";
            else if (kee == Keys.Space)
                return " ";

            // Not included: Keys.OemBackslash, Keys.OemOpenBrackets, 
            // Keys.OemCloseBrackets, Keys.OemPipe, Keys.OemQuotes

            else
                return string.Empty;
        }
    }
}
