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


        /// <summary>
        /// Removes the specified OdgeButton from the menu options.
        /// </summary>
        /// <param name="option"></param>
        public void Remove(OdgeButton option) {
            int indx = Options.IndexOf(option);

            if (indx > -1) {
                Options.Remove(option);

                if (_selectedIndex == indx) {
                    if (_selectedIndex >= Options.Count && Options.Count > 0)
                        _selectedIndex--;
                    SelectedOption?.OnSelected();
                }

                else if (_selectedIndex > indx)
                    _selectedIndex--; // Do not run events.
            }

            if (Options.Count == 0)
                OnEmptied();

            IsMessy = true;
        }


        /// <summary>
        /// Removes the OdgeButton at the specified index from the menu options.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index) {
            if (index > -1 && index < Options.Count) {
                Remove(Options[index]);
            }
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
