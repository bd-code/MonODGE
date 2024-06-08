using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;
namespace MonODGE.UI.Components {
    public class PopupCollection {
        private Queue<OdgePopUp> _pops;
        private OdgeUIVisitor _acceptVisitor;

        public OdgePopUp ActiveComponent => _pops.Peek();
        public int Count => _pops.Count;

        /// <summary>
        /// Toggles whether to update and draw all open OdgePopUps together or just the current one.
        /// </summary>
        public bool DrawAll { get; set; }

        public PopupCollection() {
            _pops = new Queue<OdgePopUp>();
            _acceptVisitor = new OdgeUIVisitor(oc => {
                if (oc.Style.IsChanged)
                    oc.Style.AcceptChanges();
            });

            DrawAll = false;
        }


        public void Update() {
            if (Count > 0) {
                if (DrawAll)   UpdateAll();
                else                UpdateOne();
                
                _acceptVisitor.Traverse(_pops);
            }
        }
        private void UpdateAll() {
            // Do not fetch Count in the loop since Count may change.
            int c = _pops.Count;
            for (int p = 0; p < c; p++) {
                OdgePopUp pop = _pops.Dequeue();
                if (pop.Lifetime > 0) {
                    pop.Update();
                    if (!pop.IsClosed) { _pops.Enqueue(pop); }
                }
            }
        }
        private void UpdateOne() {
            // We dequeue immediately if IsClosed,
            // Draw and dequeue on next frame if Lifetime expired.
            if (_pops.Peek().Lifetime > 0) {
                _pops.Peek().Update();
                if (_pops.Peek().IsClosed) { _pops.Dequeue(); }
            }
            else
                _pops.Dequeue();
        }


        public void Draw(SpriteBatch batch) {
            if (Count > 0) {
                if (DrawAll) {
                    foreach (OdgePopUp pop in _pops)
                        pop.Draw(batch);
                }
                else
                    _pops.Peek().Draw(batch);
            }
        }


        public void Open(OdgePopUp popup) {
            _pops.Enqueue(popup);
            popup.Open();
        }


        public void Close(OdgePopUp popup) {
            int c = _pops.Count;
            bool success = false;

            for (int p = 0; p < c; p++) {
                OdgePopUp pop = _pops.Dequeue();
                if (pop == popup) {
                    success = true;
                    pop.Close();
                }
                else
                    _pops.Enqueue(pop);
            }

            if (!success) {
                string e = $"PopupCollection.Close() could not find {popup.Name}.";
                throw new ArgumentException(e);
            }
        }


        public void CloseAll() {
            while (_pops.Count > 0)
                _pops.Dequeue().Close();
        }


        public bool Has(string name) {
            foreach (OdgePopUp odge in _pops)
                if (odge.Name == name)
                    return true;
            return false;
        }


        public bool Has(OdgePopUp control) {
            foreach (OdgePopUp odge in _pops)
                if (odge == control)
                    return true;
            return false;
        }


        public OdgePopUp Find(string name) {
            foreach (OdgePopUp odge in _pops)
                if (odge.Name == name)
                    return odge;
            string e = $"PopupCollection.Find() could not find {name}.";
            throw new ArgumentException(e);
        }
    }
}
