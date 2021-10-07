
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
            OdgeIO.Map.Add("_UP", Keys.Up);
            OdgeIO.Map.Add("_LEFT", Keys.Left);
            OdgeIO.Map.Add("_RIGHT", Keys.Right);
            OdgeIO.Map.Add("_DOWN", Keys.Down);
        }

        /// <summary>
        /// Auto-maps Keyboard's WASD Keys to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapWASD() {
            OdgeIO.Map.Add("_UP", Keys.W);
            OdgeIO.Map.Add("_LEFT", Keys.A);
            OdgeIO.Map.Add("_RIGHT", Keys.D);
            OdgeIO.Map.Add("_DOWN", Keys.S);
        }

        /// <summary>
        /// Auto-maps GamePad's DPad Buttons to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapDPad() {
            OdgeIO.Map.Add("_UP", Buttons.DPadUp);
            OdgeIO.Map.Add("_LEFT", Buttons.DPadLeft);
            OdgeIO.Map.Add("_RIGHT", Buttons.DPadRight);
            OdgeIO.Map.Add("_DOWN", Buttons.DPadDown);
        }

        /// <summary>
        /// Auto-maps GamePad's LeftThumbstick to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapLeftThumbstick() {
            OdgeIO.Map.Add("_UP", Buttons.LeftThumbstickUp);
            OdgeIO.Map.Add("_LEFT", Buttons.LeftThumbstickLeft);
            OdgeIO.Map.Add("_RIGHT", Buttons.LeftThumbstickRight);
            OdgeIO.Map.Add("_DOWN", Buttons.LeftThumbstickDown);
        }

        /// <summary>
        /// Auto-maps GamePad's RightThumbstick to the following OdgeIO commands:
        /// { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
        /// </summary>
        public static void MapRightThumbstick() {
            OdgeIO.Map.Add("_UP", Buttons.RightThumbstickUp);
            OdgeIO.Map.Add("_LEFT", Buttons.RightThumbstickLeft);
            OdgeIO.Map.Add("_RIGHT", Buttons.RightThumbstickRight);
            OdgeIO.Map.Add("_DOWN", Buttons.RightThumbstickDown);
        }

        public static bool UP => OdgeIO.IsCommandDown("_UP");
        public static bool LEFT => OdgeIO.IsCommandDown("_LEFT");
        public static bool RIGHT => OdgeIO.IsCommandDown("_RIGHT");
        public static bool DOWN => OdgeIO.IsCommandDown("_DOWN");
    }
}
