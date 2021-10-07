using System;

using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {

    /// <summary>
    /// Provides support and useful methods for GamePad (controller) input.
    /// </summary>
    public class GamePadHandler {
        private GamePadState[] OldStates { get; set; }
        public GamePadState[] States { get; private set; }

        private readonly int _playercount;

        public GamePadHandler(int numberOfPlayers) {
            if (numberOfPlayers > GamePad.MaximumGamePadCount) {
                string e = "GamePadHandler arg numberOfPlayers exceeds GamePad.MaximumGamePadCount: " + GamePad.MaximumGamePadCount;
                throw new ArgumentOutOfRangeException(e);
            }

            _playercount = numberOfPlayers;
            OldStates = new GamePadState[numberOfPlayers];
            States = new GamePadState[numberOfPlayers];

            for (int p = 0; p < _playercount; p++) {
                OldStates[p] = GamePad.GetState(p);
                States[p] = GamePad.GetState(p);
            }
        }

        public void Update() {
            for (int p = 0; p < _playercount; p++) {
                OldStates[p] = States[p];
                States[p] = GamePad.GetState(p);
            }
        }


        public bool IsButtonDown(int playerIndex, Buttons button) {
            return States[playerIndex].IsButtonDown(button);
        }

        public bool IsButtonHold(int playerIndex, Buttons button) {
            return (States[playerIndex].IsButtonDown(button) && OldStates[playerIndex].IsButtonDown(button));
        }

        public bool IsButtonPress(int playerIndex, Buttons button) {
            return (States[playerIndex].IsButtonDown(button) && !OldStates[playerIndex].IsButtonDown(button));
        }

        public bool IsButtonRelease(int playerIndex, Buttons button) {
            return (!States[playerIndex].IsButtonDown(button) && OldStates[playerIndex].IsButtonDown(button));
        }
    }
}
