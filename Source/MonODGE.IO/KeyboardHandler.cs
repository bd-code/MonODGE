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


        public bool IsKeyDown(Keys kee) =>
            State.IsKeyDown(kee);
        public bool IsKeyHold(Keys kee) => 
            State.IsKeyDown(kee) && OldState.IsKeyDown(kee);
        public bool IsKeyPress(Keys kee) => 
            State.IsKeyDown(kee) && !OldState.IsKeyDown(kee);
        public bool IsKeyRelease(Keys kee) => 
            !State.IsKeyDown(kee) && OldState.IsKeyDown(kee);


        public bool AnyKeyDown() =>
            State.GetPressedKeys().Length > 0;
        public bool AnyKeyPress() =>
            State.GetPressedKeys().Length > 0 && OldState.GetPressedKeys().Length == 0;
        
    }
}
