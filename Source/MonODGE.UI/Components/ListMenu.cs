using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Displays a scrollable list of option buttons extended from OdgeButton.
    /// </summary>
    public class ListMenu : OdgeControl {
        protected StyledText _stytex;
        protected Point textPoint;

        protected List<OdgeButton> Options { get; set; }
        protected Rectangle _bpanel;

        private int _selectedIndex;
        public int SelectedIndex {
            get { return _selectedIndex; }
            set {
                if (Options.Count == 0) {
                    _selectedIndex = 0;
                    return;
                }

                value = MathHelper.Clamp(value, 0, Options.Count - 1);
                if (_selectedIndex != value) {
                    SelectedOption?.OnUnselected(); // ?'ed in case SelectedOption was removed.
                    _selectedIndex = value;
                    SelectedOption.OnSelected();
                }
            }
        }

        public OdgeButton SelectedOption => 
            (Options.Count > _selectedIndex) ? Options[_selectedIndex] : null;


        protected override int MinWidth {
            get {
                int mw = Style.Padding.Left + Style.Padding.Right;
                if (Options.Count > 0)
                    mw += Options[0].Width;
                return mw;
            }
        }


        public ListMenu(StyleSheet style, Rectangle area, string heading)
            : base(style) {
            heading = heading ?? string.Empty;
            _stytex = new StyledText(style, heading);
            _stytex.StyleMode = StyledText.StyleModes.Header;

            Options = new List<OdgeButton>();
            PackToSize(area);

            _bpanel = new Rectangle(
               X + Style.Padding.Left,
               Y + Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical,
               Width - Style.Padding.Left - Style.Padding.Right,
               Height - (Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical + Style.Padding.Bottom)
               );

            Layout();

            Opened += (o, e) => {
                foreach (OdgeButton option in Options)
                    option.OnOpened();
            };

            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        /// <summary>
        /// This is called in Update when Options.Count == 0.
        /// </summary>
        public virtual void OnEmpty() { Emptied?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Emptied;


        public override void AcceptVisitor(OdgeUIVisitor visitor) {
            _stytex.AcceptVisitor(visitor);
            foreach (OdgeButton butt in Options)
                butt.AcceptVisitor(visitor);
            base.AcceptVisitor(visitor);
        }


        public override void Update() {
            if (Options.Count > 0) {
                HandleInput();

                // Scroll if Selection Offscreen
                ScrollOptions();

                // Update each option.
                for (int p = 0; p < Options.Count; p++) {
                    Options[p].Update();
                }
            }
            
            else {
                // Cancel.
                if (CheckCancel) {
                    OnCancel();
                    return;
                }
                else {
                    OnEmpty();
                }
            }
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);
            _stytex.Draw(batch, new Rectangle(Location + textPoint, Size));
            
            for (int p = 0; p < Options.Count; p++) {
                if (Options[p].Y >= 0 && Options[p].Dimensions.Bottom <= _bpanel.Height) {
                    Options[p].Draw(batch, _bpanel);
                }
            }
        }
        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);
            _stytex.Draw(batch, new Rectangle(Location + parentRect.Location + textPoint, Size));

            Rectangle bp2 = new Rectangle(_bpanel.Location + parentRect.Location, _bpanel.Size);
            for (int p = 0; p < Options.Count; p++) {
                if (Options[p].Y >= 0 && Options[p].Dimensions.Bottom <= bp2.Height) {
                    Options[p].Draw(batch, bp2);
                }
            }
        }


        private void HandleInput() {
            // Submit.
            if (CheckSubmit) {
                SelectedOption.OnSubmit();
                // this.OnSubmit()? Maybe containers shouldn't have submit.
            }

            // Move Down.
            else if (InputHelper.DOWN) {
                if (SelectedIndex + 1 >= Options.Count)
                    SelectedIndex = 0;
                else
                    SelectedIndex++;
            }

            // Move Up!
            else if (InputHelper.UP) {
                if (SelectedIndex - 1 < 0)
                    SelectedIndex = Options.Count - 1;
                else
                    SelectedIndex--;
            }

            // Jump Down.
            else if (InputHelper.RIGHT) {
                SelectedIndex += 8;
            }

            // Jump Up!
            else if (InputHelper.LEFT) {
                SelectedIndex -= 8;
            }

            // Cancel.
            else if (CheckCancel) {
                OnCancel();
            }
        }


        public override void Layout() {
            if (_stytex.IsMessy)
                _stytex.Layout();

            /// Reset Buttons ///
            int wdh = Math.Max(
                Width - Style.Padding.Left - Style.Padding.Right, 
                LayoutHelper.GetMaxSizes(Options).X);
            int ypos = Options.Count > 0 ? Options[0]?.Y ?? 0 : 0;

            foreach (OdgeButton option in Options) {
                option.Location = new Point(0, ypos);
                option.Size = new Point(wdh, option.Height);
                if (option.IsMessy)
                    option.Layout();
                ypos += option.Height + Style.Spacing.Vertical;
            }

            // Since widening options affect MinWidth, check for that too.
            PackToSize(Dimensions);


            /// Calc Panel ///
            _bpanel = new Rectangle(
                X + Style.Padding.Left,
                Y + Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical,
                wdh,
                Height - (Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical + Style.Padding.Bottom)
                );


            /// Calc textPoint X ///
            if (Style.AlignH == StyleSheet.AlignmentsH.LEFT)
                textPoint.X = Style.Padding.Left;
            else if (Style.AlignH == StyleSheet.AlignmentsH.CENTER)
                textPoint.X = Width / 2 - (_stytex.Width / 2);
            else // Right
                textPoint.X = Width - _stytex.Width - Style.Padding.Right;

            /// Calc textPoint Y ///
            textPoint.Y = Style.Padding.Top;

            base.Layout();
        }


        public void AddOption(OdgeButton option) {
            Options.Add(option);
            if (Options.Count == 1) {
                _selectedIndex = 0;
                option.Y = 0;
                option.OnSelected();
            }
            else {
                option.Location = new Point(0, Options[Options.Count - 2].Dimensions.Bottom + Style.Spacing.Vertical);
                option.Size = Options[0].Size;
                option.Layout();
            }
            IsMessy = true;
        }

        public void SetOptions(OdgeButton[] options) {
            Options = new List<OdgeButton>(options);
            if (Options.Count > 0) {
                _selectedIndex = 0;
                Options[0].OnSelected();
                Layout();
            }
        }

        public void RemoveOption(OdgeButton option) {
            bool onDeletion = option == SelectedOption;
            Options.Remove(option);

            if (SelectedIndex > Options.Count - 1)
                SelectedIndex = Options.Count - 1;
            else if (onDeletion)
                SelectedOption?.OnSelected();

            IsMessy = true;
        }


        /// <summary>
        /// Cascades ListMenu's StyleSheet to OdgeButtons.
        /// </summary>
        public void CascadeStyle() {
            if (Options != null) {
                foreach (OdgeButton option in Options)
                    option.Style = Style;

                IsMessy = true;
            }
        }


        private void ScrollOptions() {
            // SelectedOption too high
            if (SelectedOption.Y < 0) {
                int dy = -(SelectedOption.Y);
                foreach (OdgeButton op in Options)
                    op.Y += (dy / 2) + 1;
            }

            // SelectedOption too low
            else if (SelectedOption.Dimensions.Bottom > _bpanel.Height) {
                int dy = SelectedOption.Dimensions.Bottom - _bpanel.Height;
                foreach (OdgeButton op in Options)
                    op.Y -= (dy / 2) + 1;
            }
        }
    }
}
