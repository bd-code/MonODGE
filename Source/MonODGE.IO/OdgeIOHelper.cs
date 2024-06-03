
using Microsoft.Xna.Framework.Input;

namespace MonODGE.IO {
    /// <summary>
    /// Provides static functions for simple input checks.
    /// </summary>
    public static class OdgeIOHelper {
        /// <summary>
        /// Auto-maps Keyboard's Arrow Keys to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapArrowKeys() {
            OdgeIO.Map.Set("_UP", Keys.Up);
            OdgeIO.Map.Set("_LEFT", Keys.Left);
            OdgeIO.Map.Set("_RIGHT", Keys.Right);
            OdgeIO.Map.Set("_DOWN", Keys.Down);
        }

        /// <summary>
        /// Auto-maps Keyboard's WASD Keys to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapWASD() {
            OdgeIO.Map.Set("_UP", Keys.W);
            OdgeIO.Map.Set("_LEFT", Keys.A);
            OdgeIO.Map.Set("_RIGHT", Keys.D);
            OdgeIO.Map.Set("_DOWN", Keys.S);
        }

        /// <summary>
        /// Auto-maps GamePad's DPad Buttons to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapDPad() {
            OdgeIO.Map.Set("_UP", Buttons.DPadUp);
            OdgeIO.Map.Set("_LEFT", Buttons.DPadLeft);
            OdgeIO.Map.Set("_RIGHT", Buttons.DPadRight);
            OdgeIO.Map.Set("_DOWN", Buttons.DPadDown);
        }

        /// <summary>
        /// Auto-maps GamePad's LeftThumbstick to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapLeftThumbstick() {
            OdgeIO.Map.Set("_UP", Buttons.LeftThumbstickUp);
            OdgeIO.Map.Set("_LEFT", Buttons.LeftThumbstickLeft);
            OdgeIO.Map.Set("_RIGHT", Buttons.LeftThumbstickRight);
            OdgeIO.Map.Set("_DOWN", Buttons.LeftThumbstickDown);
        }

        /// <summary>
        /// Auto-maps GamePad's RightThumbstick to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapRightThumbstick() {
            OdgeIO.Map.Set("_UP", Buttons.RightThumbstickUp);
            OdgeIO.Map.Set("_LEFT", Buttons.RightThumbstickLeft);
            OdgeIO.Map.Set("_RIGHT", Buttons.RightThumbstickRight);
            OdgeIO.Map.Set("_DOWN", Buttons.RightThumbstickDown);
        }

        public static bool UP => OdgeIO.IsCommandDown("_UP");
        public static bool LEFT => OdgeIO.IsCommandDown("_LEFT");
        public static bool RIGHT => OdgeIO.IsCommandDown("_RIGHT");
        public static bool DOWN => OdgeIO.IsCommandDown("_DOWN");
    }
}
