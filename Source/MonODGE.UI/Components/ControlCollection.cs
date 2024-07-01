using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    public class ControlCollection {
        private Stack<OdgeControl> _controls;
        private OdgeUIVisitor _acceptVisitor;
        private bool _hasUpdated;

        public OdgeControl ActiveComponent => _controls.Peek();
        public int Count => _controls.Count;

        /// <summary>
        /// Toggles whether to draw all open OdgeControls or just the active one.
        /// </summary>
        public bool DrawAll { get; set; }

        public ControlCollection() {
            _controls = new Stack<OdgeControl>();
            _acceptVisitor = new OdgeUIVisitor(oc => {
                if (oc.Style.IsChanged)
                    oc.Style.AcceptChanges();
            });

            DrawAll = false;
        }


        public void Update() {
            _hasUpdated = false;

            // Check Count again because Controls can call CloseAllControls().
            while (Count > 0 && !_hasUpdated) {
                if (_controls.Peek().IsClosed)
                    _controls.Pop();
                else {
                    _controls.Peek().Update();
                    _hasUpdated = true;
                }
            }
            
            _acceptVisitor.Traverse(_controls);
        }


        public void Draw(SpriteBatch batch) {
            if (Count > 0) {
                if (DrawAll) {
                    foreach (OdgeControl odge in _controls.Reverse()) {
                        odge.Draw(batch);
                    }
                }
                else
                    _controls.Peek().Draw(batch);
            }
        }


        public void Open(OdgeControl control) {
            if (Count > 0)
                _controls.Peek().SetFocus(false);

            _controls.Push(control);
            control.Open();
        }


        public void Close(OdgeControl control) {
            Stack<OdgeControl> temp = new Stack<OdgeControl>();
            bool success = false;

            while (_controls.Count > 0) {
                OdgeControl con = _controls.Pop();
                if (con == control) {
                    success = true;
                    con.Close();
                }
                else
                    temp.Push(con);
            }

            while (temp.Count > 0)
                _controls.Push(temp.Pop());

            if (success) {
                if (Count > 0) _controls.Peek().SetFocus(true);
            }
            else {
                string e = $"ControlCollection.Close() could not find {control.Name}.";
                throw new ArgumentException(e);
            }
        }


        public void CloseAll() {
            while (_controls.Count > 0)
                _controls.Pop().Close();
        }


        public bool Has(string name) {
            foreach (OdgeControl odge in _controls)
                if (odge.Name == name)
                    return true;
            return false;
        }


        public bool Has(OdgeControl control) {
            foreach (OdgeControl odge in _controls)
                if (odge == control)
                    return true;
            return false;
        }


        public OdgeControl Find(string name) {
            foreach (OdgeControl odge in _controls)
                if (odge.Name == name)
                    return odge;
            string e = $"ControlCollection.Find() could not find {name}.";
            throw new ArgumentException(e);
        }


        public void SetAllStyles(Styles.StyleSheet style) {
            OdgeUIVisitor stv = new OdgeUIVisitor(oc => oc.Style = style);
            stv.Traverse(_controls);
        }
    }
}
