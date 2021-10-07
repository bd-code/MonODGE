using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {
    /// <summary>
    /// Provides Keyboard and GamePad Button mapping features.
    /// </summary>
    public class OdgeInputMap {
        private Dictionary<string, Keys> _keymap;
        private Dictionary<string, Buttons> _buttonmap;

        public OdgeInputMap() {
            _keymap = new Dictionary<string, Keys>();
            _buttonmap = new Dictionary<string, Buttons>();
        }

        public void Add(string command, Keys keyboardInput, Buttons padInput) {
            Add(command, keyboardInput);
            Add(command, padInput);
        }

        public void Add(string command, Keys keyboardInput) {
            if (_keymap.ContainsKey(command))
                _keymap[command] = keyboardInput;
            else
                _keymap.Add(command, keyboardInput);
        }

        public void Add(string command, Buttons padInput) {
            if (_buttonmap.ContainsKey(command))
                _buttonmap[command] = padInput;
            else
                _buttonmap.Add(command, padInput);
        }


        public bool HasKeysCommand(string command) => _keymap.ContainsKey(command);
        public bool HasButtonsCommand(string command) => _buttonmap.ContainsKey(command);
        public string[] GetKeysCommands() => _keymap.Keys.ToArray();
        public string[] GetButtonsCommands() => _buttonmap.Keys.ToArray();


        /// <summary>
        /// Returns the Keys value paired to a command.
        /// Throws an exception if command not found. Use HasKeysCommand first.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>Keys value paired with the input string command.</returns>
        public Keys KeyboardLookup(string command) => _keymap[command];


        /// <summary>
        /// Returns the GamePadButtons object paired to a command. 
        /// Throws an exception if command not found. Use HasButtonCommand first.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>GamePadButtons button paired with the input string command.</returns>
        public Buttons ButtonLookup(string command) => _buttonmap[command];


        /// <summary>
        /// Returns a clone of the current OdgeInputMap with the same mappings.
        /// </summary>
        /// <returns>A clone of the current OdgeInputMap.</returns>
        public OdgeInputMap Clone() {
            var map = new OdgeInputMap();
            map._buttonmap = _buttonmap;
            map._keymap = _keymap;
            return map;
        }

        /// <summary>
        /// Restores Buttons and Keys mappings from another OdgeInputMap.
        /// </summary>
        /// <param name="map">Another OdgeInputMap.</param>
        public void Restore(OdgeInputMap map) {
            _buttonmap = map._buttonmap;
            _keymap = map._keymap;
        }
    }
}
