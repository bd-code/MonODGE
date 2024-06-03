using System;

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

        public static int DefaultPlayerIndex {
            get { return Input._defaultPlayerIndex; }
            set { Input._defaultPlayerIndex = value; }
        }
        private int _defaultPlayerIndex;

        public static KeyboardHandler KB => Input._keyboard;
        private KeyboardHandler _keyboard;

        public static GamePadHandler GP => Input._gamepads;
        private GamePadHandler _gamepads;

        public static OdgeInputMap Map => Input._map;
        private OdgeInputMap _map;

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


        public static bool IsCommandDown(string command) => 
            Map.IsCommandDown(command, 0, KB, GP);
        public static bool IsCommandDown(int playerIndex, string command) =>
            Map.IsCommandDown(command, playerIndex, KB, GP);
        

        public static bool IsCommandHold(string command) =>
            Map.IsCommandHold(command, 0, KB, GP);
        public static bool IsCommandHold(int playerIndex, string command) =>
            Map.IsCommandHold(command, playerIndex, KB, GP);


        public static bool IsCommandPress(string command) =>
            Map.IsCommandPress(command, 0, KB, GP);
        public static bool IsCommandPress(int playerIndex, string command) =>
            Map.IsCommandPress(command, playerIndex, KB, GP);
            

        public static bool IsCommandRelease(string command) =>
            Map.IsCommandRelease(command, 0, KB, GP);
        public static bool IsCommandRelease(int playerIndex, string command) =>
            Map.IsCommandRelease(command, playerIndex, KB, GP);
            
    }

    //////////////////////////////////////////////////////////////////////////

    internal class OdgeInputMapException : Exception {
        public OdgeInputMapException(string message) : base(message) { }
    }
}
