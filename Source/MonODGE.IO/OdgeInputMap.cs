using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {
    /// <summary>
    /// Provides Keyboard and GamePad Button mapping features.
    /// </summary>
    public class OdgeInputMap {
        private struct MappedIO {
            internal Keys[] keys; 
            internal Buttons[] buttons;
            internal MappedIO(Keys[] k, Buttons[] b) { keys = k; buttons = b; }
        }
        private Dictionary<string, MappedIO> _map;


        public OdgeInputMap() {
            _map = new Dictionary<string, MappedIO>();
        }


        public void Set(string command, Keys key, Buttons butt) {
            Set(command, new[] { key }, new[] { butt });
        }
        public void Set(string command, Keys key) {
            Set(command, new[] { key });
        }
        public void Set(string command, Buttons butt) {
            Set(command, new[] { butt });
        }
        public void Set(string command, Keys[] keys, Buttons[] butts) {
            _map[command] = new MappedIO(keys, butts);
        }
        public void Set(string command, Keys[] keys) {
            _map[command] = new MappedIO(keys, new Buttons[0]);
        }
        public void Set(string command, Buttons[] butts) {
            _map[command] = new MappedIO(new Keys[0], butts);
        }
        public void Unset(string command) {
            _map.Remove(command);
        }



        public bool Has(string command) => _map.ContainsKey(command);
        public string[] AllCommands() => _map.Keys.ToArray();


        /// <summary>
        /// Returns the Keys array paired to a command.
        /// Throws an exception if command not found. Use Has(command) first.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>Keys array paired with the input string command.</returns>
        public Keys[] GetKeys(string command) => _map[command].keys;


        /// <summary>
        /// Returns the GamePad Buttons array paired to a command. 
        /// Throws an exception if command not found. Use Has(command) first.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>Buttons array paired with the input string command.</returns>
        public Buttons[] GetButtons(string command) => _map[command].buttons;


        public bool IsCommandDown(string command, int playerIndex, KeyboardHandler kb, GamePadHandler gp) {
            if (!Has(command)) 
                throw new OdgeInputMapException($"OdgeIO command not mapped: {command}");

            foreach (var key in _map[command].keys) {
                if (kb.IsKeyDown(key))
                    return true;
            }
            foreach (var butt in _map[command].buttons) {
                if (gp.IsButtonDown(playerIndex, butt))
                    return true;
            }
            return false;
        }


        public bool IsCommandHold(string command, int playerIndex, KeyboardHandler kb, GamePadHandler gp) {
            if (!Has(command))
                throw new OdgeInputMapException($"OdgeIO command not mapped: {command}");

            foreach (var key in _map[command].keys) {
                if (kb.IsKeyHold(key))
                    return true;
            }
            foreach (var butt in _map[command].buttons) {
                if (gp.IsButtonHold(playerIndex, butt))
                    return true;
            }
            return false;
        }
        
        
        public bool IsCommandPress(string command, int playerIndex, KeyboardHandler kb, GamePadHandler gp) {
            if (!Has(command))
                throw new OdgeInputMapException($"OdgeIO command not mapped: {command}");

            foreach (var key in _map[command].keys) {
                if (kb.IsKeyPress(key))
                    return true;
            }
            foreach (var butt in _map[command].buttons) {
                if (gp.IsButtonPress(playerIndex, butt))
                    return true;
            }
            return false;
        }
        
        
        public bool IsCommandRelease(string command, int playerIndex, KeyboardHandler kb, GamePadHandler gp) {
            if (!Has(command))
                throw new OdgeInputMapException($"OdgeIO command not mapped: {command}");

            foreach (var key in _map[command].keys) {
                if (kb.IsKeyRelease(key))
                    return true;
            }
            foreach (var butt in _map[command].buttons) {
                if (gp.IsButtonRelease(playerIndex, butt))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Returns a clone of the current OdgeInputMap with the same mappings.
        /// </summary>
        /// <returns>A clone of the current OdgeInputMap.</returns>
        public OdgeInputMap Clone() {
            var map = new OdgeInputMap();
            map._map = _map;
            return map;
        }


        /// <summary>
        /// Restores Buttons and Keys mappings from another OdgeInputMap.
        /// </summary>
        /// <param name="map">Another OdgeInputMap.</param>
        public void Restore(OdgeInputMap other) { _map = other._map; }
    }
}
