using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Displays a scrollable list of option buttons extended from OdgeButton.
    /// </summary>
    public class ListMenu : OdgeMenu {
        protected StyledText _stytex;
        protected Point textPoint;
        protected Rectangle _bpanel;


        protected override int MinWidth =>
            Style.Padding.Left +
            (Options.Count > 0 ? 
                Math.Max(Options[0].Width, _stytex.Width) : 
                _stytex.Width) +
            Style.Padding.Right;

        protected override int MinHeight => 
            Style.Padding.Top + _stytex.Height + 
            (Options.Count > 0 ? Style.Spacing.Vertical + Options[0].Height : 0) + 
            Style.Padding.Bottom;


        public ListMenu(StyleSheet style, Rectangle area, string heading)
            : base(style) {
            _stytex = new StyledText(style, heading ?? string.Empty, StyledText.StyleModes.Header);

            Options = new List<OdgeButton>();

            _bpanel = new Rectangle(
               X + Style.Padding.Left,
               Y + Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical,
               Width - Style.Padding.Left - Style.Padding.Right,
               Height - (Style.Padding.Top + _stytex.Height + Style.Spacing.Vertical + Style.Padding.Bottom)
               );

            Location = area.Location;
            Size = area.Size;
            Layout();

            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        public override void AcceptVisitor(OdgeUIVisitor visitor) {
            _stytex.AcceptVisitor(visitor);
            base.AcceptVisitor(visitor);
        }


        public override void Update() {
            if (Options.Count > 0) {
                HandleInput();
                ScrollOptions();

                // Update each option.
                for (int p = 0; p < Options.Count; p++) {
                    Options[p].Update();
                }
            }
            
            else {
                if (CheckSubmit) OnSubmit();
                else if (CheckCancel) OnCancel();                
            }

            base.Update();
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


        /// <summary>
        /// Adds an OdgeButton to the ListMenu.
        /// </summary>
        /// <param name="option">An OdgeButton to add to the ListMenu.</param>
        public void Add(OdgeButton option) {
            Options.Add(option);
            if (Options.Count == 1) {
                _selectedIndex = 0;
                option.OnSelected();
            }
            IsMessy = true;
        }


        /// <summary>
        /// Adds a group of OdgeButtons to the list of menu options. 
        /// </summary>
        /// <param name="options">An array of OdgeButtons to add to the menu.</param>
        public void AddRange(OdgeButton[] options) {
            Options = new List<OdgeButton>(options);
            if (Options.Count > 0) {
                _selectedIndex = 0;
                Options[0].OnSelected();
                IsMessy = true;
            }
        }


        private void HandleInput() {
            // Submit.
            if (CheckSubmit) {
                SelectedOption.OnSubmit();
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


        private void ScrollOptions() {
            if (Options.Count == 0)
                return;

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
