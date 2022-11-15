﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Displays a menu as a gallery-like two-dimensional grid.
    /// </summary>
    public class GalleryMenu : OdgeMenu {
        private int _columns;
        public int Columns {
            get { return _columns; }
            set {
                _columns = value;
                Width = Width;
            }
        }


        protected override int MinWidth => 
            Style.Padding.Left + 
            (LayoutHelper.GetMaxSizes(Options).X * Columns) + 
            (Style.Spacing.Horizontal * Columns * 2) + 
            Style.Padding.Right;


        protected int OptionTopBound => Style.Padding.Top;
        protected int OptionLowBound => Dimensions.Height - Style.Padding.Bottom;


        public GalleryMenu(StyleSheet style, Rectangle area, int columns) 
            : base(style) {
            Options = new List<OdgeButton>();
            _columns = columns;
            PackToSize(area);
            Layout();
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
                // Cancel.
                if (CheckCancel) {
                    OnCancel();
                    return;
                }
                else {
                    OnEmptied();
                }
            }
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);

            for (int p = 0; p < Options.Count; p++) {
                if (Options[p].Y >= OptionTopBound && Options[p].Dimensions.Bottom <= OptionLowBound) {
                    Options[p].Draw(batch, Dimensions);
                }
            }
        }


        public override void Layout() {
            foreach (OdgeButton option in Options)
                if (option.IsMessy)
                    option.Layout();

            Point itemsize = LayoutHelper.GetMaxSizes(Options);
            Point cellsize = new Point(itemsize.X + Style.Spacing.Horizontal * 2, itemsize.Y + Style.Spacing.Vertical * 2);
            Point pointer = new Point(Style.Padding.Left, Style.Padding.Top);
            int col = 0;

            foreach (var option in Options) {
                Point offset = new Point();
                if (Style.AlignH == StyleSheet.AlignmentsH.LEFT)
                    offset.X = Style.Spacing.Horizontal;
                else if (Style.AlignH == StyleSheet.AlignmentsH.CENTER)
                    offset.X = (cellsize.X - option.Width) / 2;
                else if (Style.AlignH == StyleSheet.AlignmentsH.RIGHT)
                    offset.X = cellsize.X - option.Width - Style.Spacing.Horizontal;

                if (Style.AlignV == StyleSheet.AlignmentsV.TOP)
                    offset.Y = Style.Spacing.Vertical;
                else if (Style.AlignV == StyleSheet.AlignmentsV.CENTER)
                    offset.Y = (cellsize.Y - option.Height) / 2;
                else if (Style.AlignV == StyleSheet.AlignmentsV.BOTTOM)
                    offset.Y = cellsize.Y - option.Height - Style.Spacing.Vertical;


                option.Location = new Point(
                    pointer.X + offset.X, 
                    pointer.Y + offset.Y);
                col++;
                if (col == _columns) {
                    col = 0;
                    pointer.X = Style.Padding.Left;
                    pointer.Y += cellsize.Y;
                }
                else { pointer.X += cellsize.X; }
            }
            base.Layout();
        }


        /// <summary>
        /// 
        /// Adds an OdgeButton to the GalleryMenu.
        /// </summary>
        /// <param name="option">An OdgeButton to add to the GalleryMenu.</param>
        public void Add(OdgeButton option) {
            Options.Add(option);
            if (Options.Count == 1)
                option.OnSelected();
            PackToSize(Dimensions);
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
                PackToSize(Dimensions);
                Layout();
            }
        }


        private void HandleInput() {            
            // Submit.
            if (CheckSubmit) {
                SelectedOption.OnSubmit();
                // this.OnSubmit()? Maybe containers shouldn't have submit.
            }

            // Right Button.
            else if (InputHelper.RIGHT) {
                SelectedOption.OnUnselected();
                if (SelectedIndex + 1 >= Options.Count)
                    SelectedIndex = 0;
                else
                    SelectedIndex++;
                SelectedOption.OnSelected();
            }

            // Left Button.
            else if (InputHelper.LEFT) {
                SelectedOption.OnUnselected();
                if (SelectedIndex - 1 < 0)
                    SelectedIndex = Options.Count - 1;
                else
                    SelectedIndex--;
                SelectedOption.OnSelected();
            }

            // Down Button.
            else if (InputHelper.DOWN) {
                int x = SelectedIndex;
                SelectedIndex += Columns;
                if (x != SelectedIndex) {
                    Options[x].OnUnselected();
                    Options[SelectedIndex].OnSelected();
                }
            }

            // Up Button.
            else if (InputHelper.UP) {
                int x = SelectedIndex;
                SelectedIndex -= Columns;
                if (x != SelectedIndex) {
                    Options[x].OnUnselected();
                    Options[SelectedIndex].OnSelected();
                }
            }

            // Cancel.
            else if (CheckCancel) {
                OnCancel();
            }
        }


        private void ScrollOptions() {
            int topBound = OptionTopBound;
            int lowBound = OptionLowBound;
            int correction = 0;

            // SelectedOption too high
            if (SelectedOption.Y < topBound) {
                int dy = -(SelectedOption.Y - topBound);
                foreach (OdgeButton op in Options)
                    op.Y += (dy / 2) + 1;
            }

            // SelectedOption too low
            else if (SelectedOption.Dimensions.Bottom > lowBound) {
                int dy = SelectedOption.Dimensions.Bottom - lowBound;
                SelectedOption.Y -= (dy / 2) + 1;
                foreach (OdgeButton op in Options) {
                    if (op == SelectedOption)
                        continue;

                    op.Y -= (dy / 2) + 1;
                    if (op.Y == SelectedOption.Y && op.Dimensions.Bottom > lowBound && SelectedOption.Dimensions.Bottom <= lowBound)
                        correction = MathHelper.Max(op.Dimensions.Bottom - lowBound, correction);
                }
            }

            // Correct if bottom row extends past lowBound.
            if (correction > 0) {
                foreach (OdgeButton op in Options) 
                    op.Y -= correction;                
            }
        }
    }
}
