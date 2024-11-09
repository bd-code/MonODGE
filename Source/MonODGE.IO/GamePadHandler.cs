using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// This is provided in case user wants to use an external input manager. 
        /// Pass in the GamePadState of the external input manager to effectively 
        /// "sync" OdgeInput's GamePadState with the external.
        /// </summary>
        /// <param name="playerIndex">GamePad player index.</param>
        /// <param name="padstate">GamePadState of the external input library.</param>
        public void Update(int playerIndex, GamePadState padstate) {
            if (playerIndex < _playercount) {
                OldStates[playerIndex] = States[playerIndex];
                States[playerIndex] = padstate;
            }
        }


        public bool IsButtonDown(int playerIndex, Buttons button) =>
            States[playerIndex].IsButtonDown(button);
        public bool IsButtonHold(int playerIndex, Buttons button) =>
            States[playerIndex].IsButtonDown(button) && OldStates[playerIndex].IsButtonDown(button);
        public bool IsButtonPress(int playerIndex, Buttons button) =>
            States[playerIndex].IsButtonDown(button) && !OldStates[playerIndex].IsButtonDown(button);
        public bool IsButtonRelease(int playerIndex, Buttons button) =>
            !States[playerIndex].IsButtonDown(button) && OldStates[playerIndex].IsButtonDown(button);


        public bool AreButtonsDown(int playerIndex, IEnumerable<Buttons> buttons) =>
            buttons?.Any(b => States[playerIndex].IsButtonDown(b)) ?? false;
        public bool AreButtonsHeld(int playerIndex, IEnumerable<Buttons> buttons) =>
            buttons?.Any(b => States[playerIndex].IsButtonDown(b) && OldStates[playerIndex].IsButtonDown(b)) ?? false;
        public bool AreButtonsPressed(int playerIndex, IEnumerable<Buttons> buttons) =>
            buttons?.Any(b => States[playerIndex].IsButtonDown(b) && !OldStates[playerIndex].IsButtonDown(b)) ?? false;
        public bool AreButtonsReleased(int playerIndex, IEnumerable<Buttons> buttons) =>
            buttons?.Any(b => !States[playerIndex].IsButtonDown(b) && OldStates[playerIndex].IsButtonDown(b)) ?? false;

    }
}
