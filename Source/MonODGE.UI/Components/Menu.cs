using System;

using Microsoft.Xna.Framework;

using MonODGE.UI.Styles;
using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    public abstract partial class OdgeMenu: OdgeControl {
        public ButtonCollection Options { get; private set; }


        protected int _selectedIndex;
        public int SelectedIndex {
            get { return _selectedIndex; }
            set {
                value = MathHelper.Clamp(value, 0, Math.Max(0, Options.Count - 1));
                if (_selectedIndex != value) {

                    // Unselect last option if it wasn't just removed.
                    if (_selectedIndex < Options.Count)
                        SelectedOption.IsSelected = false; // ?'ed in case SelectedOption was removed.

                    _selectedIndex = value;
                    OnSelectedIndexChanged();
                    SelectedOption.IsSelected = true;
                }
            }
        }


        public OdgeButton SelectedOption =>
            (Options.Count > _selectedIndex) ? Options[_selectedIndex] : null;


        /// <summary>
        /// If true, SelectedIndex will wrap to 0 or Count-1 
        /// if player input would set SelectedIndex > Count-1 or SelectedIndex < 0. 
        /// True by default.
        /// </summary>
        public bool WrapAround { get; set; }


        public OdgeMenu(StyleSheet style) : base(style) {
            Options = new ButtonCollection(this);
            WrapAround = true;
        }


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
        /// Cascades Menu's StyleSheet to OdgeButtons.
        /// </summary>
        public void CascadeStyle() {
            foreach (OdgeButton option in Options)
                option.Style = Style;

            IsMessy = true;
        }
    }
}
