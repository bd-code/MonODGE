using System;

using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {
    /// <summary>
    /// Provides support for user input using a Keyboard or GamePad (controller).
    /// </summary>
    public class OdgeIO {
        private static OdgeIO _input = null;

        public bool KeyboardEnabled { get; set; }
        public bool GamePadEnabled { get; set; }

        public static OdgeIO Input {
            get {
                if (_input == null)
                    _input = new OdgeIO();
                return _input;
            }
        }

        private int _defaultPlayerIndex;
        public static int DefaultPlayerIndex {
            get { return Input._defaultPlayerIndex; }
            set { Input._defaultPlayerIndex = value; }
        }

        private KeyboardHandler _keyboard;
        public static KeyboardHandler KB { get { return Input._keyboard; } }

        private GamePadHandler _gamepads;
        public static GamePadHandler GP { get { return Input._gamepads; } }

        private OdgeInputMap _map;
        public static OdgeInputMap Map { get { return Input._map; } }

        private OdgeIO() {
            _keyboard = new KeyboardHandler();
            _gamepads = new GamePadHandler(1);
            _map = new OdgeInputMap();
            KeyboardEnabled = true;
            GamePadEnabled = true;
        }


        public void Update() {
            _defaultPlayerIndex = 0;
            _keyboard.Update();
            _gamepads.Update();
        }


        public void SetPlayerCount(int numberOfPlayers) {
            _gamepads = new GamePadHandler(numberOfPlayers);
        }


        public static bool IsCommandDown(string command) {
            return IsCommandDown(0, command);
        }
        public static bool IsCommandDown(int playerIndex, string command) {
            if (Input.GamePadEnabled && Map.HasButtonsCommand(command)) {
                Buttons b = Map.ButtonLookup(command);
                if (GP.IsButtonDown(playerIndex, b))
                    return true;
            }
            else if (Input.KeyboardEnabled && Map.HasKeysCommand(command)) {
                Keys k = Map.KeyboardLookup(command);
                return KB.IsKeyDown(k);
            }
            return false;
        }


        public static bool IsCommandHold(string command) {
            return IsCommandHold(0, command);
        }
        public static bool IsCommandHold(int playerIndex, string command) {
            if (Input.GamePadEnabled && Map.HasButtonsCommand(command)) {
                Buttons b = Map.ButtonLookup(command);
                if (GP.IsButtonHold(playerIndex, b))
                    return true;
            }
            else if (Input.KeyboardEnabled && Map.HasKeysCommand(command)) {
                Keys k = Map.KeyboardLookup(command);
                return KB.IsKeyHold(k);
            }
            return false;
        }


        public static bool IsCommandPress(string command) {
            return IsCommandPress(0, command);
        }
        public static bool IsCommandPress(int playerIndex, string command) {
            if (Input.GamePadEnabled && Map.HasButtonsCommand(command)) {
                Buttons b = Map.ButtonLookup(command);
                if (GP.IsButtonPress(playerIndex, b))
                    return true;
            }
            else
            if (Input.KeyboardEnabled && Map.HasKeysCommand(command)) {
                Keys k = Map.KeyboardLookup(command);
                return KB.IsKeyPress(k);
            }
            return false;
        }


        public static bool IsCommandRelease(string command) {
            return IsCommandRelease(0, command);
        }
        public static bool IsCommandRelease(int playerIndex, string command) {
            if (Input.GamePadEnabled && Map.HasButtonsCommand(command)) {
                Buttons b = Map.ButtonLookup(command);
                if (GP.IsButtonRelease(playerIndex, b))
                    return true;
            }
            else if (Input.KeyboardEnabled && Map.HasKeysCommand(command)) {
                Keys k = Map.KeyboardLookup(command);
                return KB.IsKeyRelease(k);
            }
            return false;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    internal class OdgeInputMapException : Exception {
        public OdgeInputMapException(string message) : base(message) { }
    }
}
