
using System.Collections;
using System.Collections.Generic;

namespace MonODGE.UI.Components {
    public abstract partial class OdgeMenu : OdgeControl {
        /// <summary>
        /// Represents the collection of OdgeButtons in an OdgeMenu.
        /// </summary>
        public class ButtonCollection : IList<OdgeButton> {
            private OdgeMenu _owner;
            private List<OdgeButton> _items;

            public OdgeButton this[int index] {
                get => _items[index];
                set { _items[index] = value; }
            }

            public int Count => _items.Count;

            public bool IsReadOnly => false;


            public ButtonCollection(OdgeMenu owner) {
                _owner = owner;
                _items = new List<OdgeButton>();
            }


            public void Add(OdgeButton option) {
                _items.Add(option);
                if (_items.Count == 1) {
                    _owner._selectedIndex = 0;
                    option.IsSelected = true;
                }
                _owner.IsMessy = true;
            }


            public void AddRange(OdgeButton[] options) {
                _items.Clear();
                _items.AddRange(options);
                if (_items.Count > 0) {
                    _owner._selectedIndex = 0;
                    _items[0].IsSelected = true;
                    _owner.IsMessy = true;
                }
            }


            public void Remove(OdgeButton option) {
                int indx = _items.IndexOf(option);

                if (indx > -1) {
                    _items.Remove(option);
                    _owner.IsMessy = true;

                    if (_owner._selectedIndex == indx && _items.Count > 0) {
                        if (_owner._selectedIndex >= _items.Count)
                            _owner._selectedIndex--;

                        _owner.SelectedOption.IsSelected = true;
                    }

                    else if (_owner._selectedIndex > indx)
                        _owner._selectedIndex--; // Do not run events.

                    if (_items.Count == 0)
                        _owner.OnEmptied();
                }
            }


            bool ICollection<OdgeButton>.Remove(OdgeButton item) {
                int indx = _items.IndexOf(item);
                bool done = false;

                if (indx > -1) {
                    _items.Remove(item);
                    _owner.IsMessy = true;
                    done = true;

                    if (_owner._selectedIndex == indx && _items.Count > 0) {
                        if (_owner._selectedIndex >= _items.Count)
                            _owner._selectedIndex--;

                        _owner.SelectedOption.IsSelected = true;
                    }

                    else if (_owner._selectedIndex > indx)
                        _owner._selectedIndex--; // Do not run events.

                    if (_items.Count == 0)
                        _owner.OnEmptied();
                }

                return done;
            }


            public void RemoveAt(int index) {
                if (index > -1 && index < _items.Count) {
                    Remove(_items[index]);
                }
            }


            public int IndexOf(OdgeButton item) {
                return _items.IndexOf(item);
            }


            public void Insert(int index, OdgeButton item) {
                _items.Insert(index, item);
                if (index <= _owner._selectedIndex)
                    _owner._selectedIndex++;

                _owner.IsMessy = true;
            }


            public void Clear() {
                _items.Clear();
                _owner.OnEmptied();
            }


            public bool Contains(OdgeButton item) {
                return _items.Contains(item);
            }


            public void CopyTo(OdgeButton[] array, int arrayIndex) {
                _items.CopyTo(array, arrayIndex);
            }


            public IEnumerator<OdgeButton> GetEnumerator() {
                return _items.GetEnumerator();
            }


            IEnumerator IEnumerable.GetEnumerator() {
                return _items.GetEnumerator();
            }
        }
    }
}
