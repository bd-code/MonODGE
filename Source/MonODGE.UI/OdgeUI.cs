using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Components;
using MonODGE.UI.Utilities;

namespace MonODGE.UI {
    /// <summary>
    /// <para>The core engine of the MonODGE.UI library.</para>
    /// 
    /// <para>It is comparable to a top-level Window, Screen, or Application 
    /// object found in other UI libraries.</para>
    /// </summary>
    public class OdgeUI {
        private Stack<OdgeControl> _controls;
        private Queue<OdgePopUp> _pops;
        private Texture2D _mask;

        private OdgeUIVisitor _acceptVisitor;

        public int ControlCount { get { return _controls.Count; } }
        public int PopUpCount { get { return _pops.Count; } }


        //// Config settings ////

        /// <summary>
        /// Toggles whether to draw all open OdgeControls or just the active one.
        /// </summary>
        public bool DrawAllControls { get; set; }

        /// <summary>
        /// Toggles whether to draw a mask over inactive OdgeControls when DrawAllControls == true.
        /// </summary>
        public bool DrawInactiveMask { get; set; }

        /// <summary>
        /// Toggles whether to update all open OdgePopUps together or just the current one.
        /// </summary>
        public bool RunAllPopUps { get; set; }

        
        public OdgeUI(GraphicsDevice graphics) {
            _mask = new Texture2D(graphics, 1, 1);
            _mask.SetData(new Color[] { Color.White });

            _controls = new Stack<OdgeControl>();
            _pops = new Queue<OdgePopUp>();

            _acceptVisitor = new OdgeUIVisitor(oc => {
                if (oc.Style.IsChanged)
                    oc.Style.AcceptChanges();
            });

            // Default Config Options.
            DrawAllControls = false;
            DrawInactiveMask = true;
            RunAllPopUps = false;
        }


        public void Update() {
            UpdateControls();
            UpdatePopups();
            _acceptVisitor.Traverse(_controls);
            _acceptVisitor.Traverse(_pops);
        }


        private void UpdateControls() {
            if (ControlCount > 0) {
                _controls.Peek().Update();
            }
        }


        private void UpdatePopups() {
            if (PopUpCount > 0) {

                if (RunAllPopUps) {
                    // Do not fetch Count in the loop since Count may change.
                    int c = _pops.Count;
                    for (int p = 0; p < c; p++) {
                        OdgePopUp pop = _pops.Dequeue();
                        if (pop.Lifetime > 0) {
                            pop.Update();
                            _pops.Enqueue(pop);
                        }
                    }
                }
                else {
                    if (_pops.Peek().Lifetime > 0)
                        _pops.Peek().Update();
                    else
                        _pops.Dequeue();
                }
            }
        }


        public void DrawControl(SpriteBatch batch) {
            if (ControlCount > 0) {
                if (DrawAllControls) {
                    foreach (OdgeControl odge in _controls.Reverse()) {
                        odge.Draw(batch);
                        if (DrawInactiveMask && odge != _controls.Peek())
                            batch.Draw(_mask, odge.Dimensions, new Color(0, 0, 0, 128));
                    }
                }
                else
                    _controls.Peek().Draw(batch);
            }
        }

        public void DrawPopup(SpriteBatch batch) {
            if (PopUpCount > 0) {
                if (RunAllPopUps) {
                    foreach (OdgePopUp pop in _pops)
                        pop.Draw(batch);
                }
                else
                    _pops.Peek().Draw(batch);
            }
        }


        public void Open(OdgeControl control) {
            if (control._manager != null && control._manager != this)
                throw new OdgeComponentUsedException("OdgeControl has already been added to another OdgeUI manager.");
            control._manager = this;

            _controls.Push(control);
            control.OnOpened();
        }


        public void Open(OdgePopUp popup) {
            if (popup._manager != null && popup._manager != this)
                throw new OdgeComponentUsedException("OdgePopUp has already been added to another OdgeUI manager.");
            popup._manager = this;

            _pops.Enqueue(popup);
            popup.OnOpened();
        }


        public void Close(OdgeControl control) {
            Stack<OdgeControl> temp = new Stack<OdgeControl>();

            while (_controls.Count > 0) {
                OdgeControl con = _controls.Pop();
                if (con == control)
                    con.OnClosed();
                else
                    temp.Push(con);
            }

            while (temp.Count > 0)
                _controls.Push(temp.Pop());
        }


        public void Close(OdgePopUp popup) {
            int c = _pops.Count;
            for (int p = 0; p < c; p++) {
                OdgePopUp pop = _pops.Dequeue();
                if (pop == popup)
                    pop.OnClosed();
                else
                    _pops.Enqueue(pop);
            }
        }


        public void CloseAllControls() {
            while (_controls.Count > 0)
                _controls.Pop().OnClosed();
        }
        public void CloseAllPopups() {
            while (_pops.Count > 0)
                _pops.Dequeue().OnClosed();
        }


        /// <summary>
        /// Find an open OdgeComponent by its Name property. Searches both OdgeControls and OdgePopUps.
        /// Note the return value will be an OdgeComponent, and must be cast to the appropriate subtype.
        /// </summary>
        /// <param name="name">string name of OdgeComponent.</param>
        /// <returns></returns>
        public OdgeComponent GetComponentByName(string name) {
            OdgeComponent odge = GetControlByName(name);
            if (odge == null)
                odge = GetPopupByName(name);
            return odge;
        }
        public OdgeControl GetControlByName(string name) {
            foreach (OdgeControl odge in _controls)
                if (odge.Name == name)
                    return odge;
            return null;
        }
        public OdgePopUp GetPopupByName(string name) {
            foreach (OdgePopUp pop in _pops)
                if (pop.Name == name)
                    return pop;
            return null;
        }


        public void SetAllComponentStyle(StyleSheet style) {
            OdgeUIVisitor stv = new OdgeUIVisitor(oc => oc.Style = style);
            stv.Traverse(_controls);
            stv.Traverse(_pops);
        }
    }
}
