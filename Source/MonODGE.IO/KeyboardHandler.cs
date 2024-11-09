using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {
    /// <summary>
    /// Provides support and useful methods for Keyboard input.
    /// </summary>
    public class KeyboardHandler {
        private KeyboardState OldState { get; set; }
        public KeyboardState State { get; private set; }

        public KeyboardHandler() {
            OldState = Keyboard.GetState();
            State = Keyboard.GetState();
        }


        public void Update() {
            OldState = State;
            State = Keyboard.GetState();
        }

        /// <summary>
        /// This is provided in case user wants to use an external input manager. 
        /// Pass in the KeyboardState of the external input manager to effectively 
        /// "sync" OdgeInput's KeyboardState with the external.
        /// </summary>
        /// <param name="keystate">KeyboardState of the external input library.</param>
        public void Update(KeyboardState keystate) {
            OldState = State;
            State = keystate;
        }


        public bool IsKeyDown(Keys key) =>
            State.IsKeyDown(key);
        public bool IsKeyHold(Keys key) => 
            State.IsKeyDown(key) && OldState.IsKeyDown(key);
        public bool IsKeyPress(Keys key) => 
            State.IsKeyDown(key) && !OldState.IsKeyDown(key);
        public bool IsKeyRelease(Keys key) => 
            !State.IsKeyDown(key) && OldState.IsKeyDown(key);


        public bool AreKeysDown(IEnumerable<Keys> keys) =>
            keys?.Any(k => State.IsKeyDown(k)) ?? false;
        public bool AreKeysHeld(IEnumerable<Keys> keys) =>
            keys?.Any(k => State.IsKeyDown(k) && OldState.IsKeyDown(k)) ?? false;
        public bool AreKeysPressed(IEnumerable<Keys> keys) =>
            keys?.Any(k => State.IsKeyDown(k) && !OldState.IsKeyDown(k)) ?? false;
        public bool AreKeysReleased(IEnumerable<Keys> keys) =>
            keys?.Any(k => !State.IsKeyDown(k) && OldState.IsKeyDown(k)) ?? false;


        public bool AnyKeyDown() =>
            State.GetPressedKeys().Length > 0;
        public bool AnyKeyPress() =>
            State.GetPressedKeys().Length > 0 && OldState.GetPressedKeys().Length == 0;
        
    }
}
