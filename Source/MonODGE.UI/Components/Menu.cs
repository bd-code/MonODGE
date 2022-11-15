using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    public abstract class OdgeMenu: OdgeControl {
        public OdgeMenu(StyleSheet style) : base(style) { }

        protected List<OdgeButton> Options { get; set; }

        public int Count => Options.Count;


        protected int _selectedIndex;
        public int SelectedIndex {
            get { return _selectedIndex; }
            set {
                value = MathHelper.Clamp(value, 0, Math.Max(0, Options.Count - 1));
                if (_selectedIndex != value) {
                    SelectedOption?.OnUnselected(); // ?'ed in case SelectedOption was removed.
                    _selectedIndex = value;
                    OnSelectedIndexChanged();
                    SelectedOption?.OnSelected();
                }
            }
        }


        public OdgeButton SelectedOption =>
            (Options.Count > _selectedIndex) ? Options[_selectedIndex] : null;


        /// <summary>
        /// This is called when the SelectedIndex property has changed.
        /// </summary>
        protected virtual void OnSelectedIndexChanged() { SelectedIndexChanged?.Invoke(this, EventArgs.Empty); }
        public event EventHandler SelectedIndexChanged;


        /// <summary>
        /// This is called in Update when Options.Count == 0.
        /// </summary>
        protected virtual void OnEmptied() { Emptied?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Emptied;


        public override void AcceptVisitor(OdgeUIVisitor visitor) {
            foreach (OdgeButton butt in Options)
                butt.AcceptVisitor(visitor);
            base.AcceptVisitor(visitor);
        }


        /// <summary>
        /// Removes the specified OdgeButton from the menu options.
        /// </summary>
        /// <param name="option"></param>
        public void Remove(OdgeButton option) {
            int indx = Options.IndexOf(option);

            if (indx > -1) {
                Options.Remove(option);

                if (SelectedIndex > indx)
                    _selectedIndex--; // Do not run events.

                else if (SelectedIndex == indx) {
                    SelectedOption?.OnSelected();
                }
            }

            IsMessy = true;
        }

        /// <summary>
        /// Removes the OdgeButton at the specified index from the menu options.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            if (index > -1 && index < Options.Count) {
                Options.RemoveAt(index);

                if (SelectedIndex > index)
                    _selectedIndex--; // Do not run events.

                else if (SelectedIndex == index) {
                    SelectedOption?.OnSelected();
                }
            }

            IsMessy = true;
        }


        /// <summary>
        /// Cascades Menu's StyleSheet to OdgeButtons.
        /// </summary>
        public void CascadeStyle() {
            if (Options != null) {
                foreach (OdgeButton option in Options)
                    option.Style = Style;

                IsMessy = true;
            }
        }
    }
}
