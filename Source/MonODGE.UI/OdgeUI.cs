
using Microsoft.Xna.Framework.Input;

using MonODGE.IO;

namespace MonODGE.UI {
    /// <summary>
    /// <para>The core engine of the MonODGE.UI library.</para>
    /// 
    /// <para>OdgeUI.Initialize() must be called before constructing any OdgeUI
    /// components, even if those components are not intended to be used in a
    /// component collection.</para>
    /// </summary>
    public static class OdgeUI {
        public static bool IsInitialized { get; private set; }


        /// <summary>
        /// <para>
        /// Initializes necessary global resources used by OdgeComponents.
        /// OdgeUI must be initialized before creating components.
        /// </para>
        /// 
        /// <para>
        /// Initialize maps the following directional inputs to commands in MonODGE.IO: <br />
        /// ODGE_UI_UP:    Keys.Up,    Keys.W, Buttons.DPadUp,    Buttons.LeftThumbstickUp<br />
        /// ODGE_UI_LEFT:  Keys.Left,  Keys.A, Buttons.DPadLeft,  Buttons.LeftThumbstickLeft<br />
        /// ODGE_UI_RIGHT: Keys.Right, Keys.D, Buttons.DPadRight, Buttons.LeftThumbstickRight<br />
        /// ODGE_UI_DOWN:  Keys.Down,  Keys.S, Buttons.DPadDown,  Buttons.LeftThumbstickDown<br />
        /// </para>
        /// </summary>
        public static void Initialize() {
            OdgeIO.Map.Set("ODGE_UI_UP",
                new Keys[] { Keys.Up, Keys.W },
                new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp });

            OdgeIO.Map.Set("ODGE_UI_LEFT",
                new Keys[] { Keys.Left, Keys.A },
                new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
            
            OdgeIO.Map.Set("ODGE_UI_RIGHT",
                new Keys[] { Keys.Right, Keys.D },
                new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight });
            
            OdgeIO.Map.Set("ODGE_UI_DOWN",
                new Keys[] { Keys.Down, Keys.S },
                new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown });

            IsInitialized = true;
        }


        /// <summary>
        /// <para>
        /// Initializes necessary global resources used by OdgeComponents.
        /// OdgeUI must be initialized before creating components.
        /// </para>
        /// 
        /// <para>
        /// Initialize parameters map to the following commands in MonODGE.IO: <br />
        /// ODGE_UI_UP, ODGE_UI_LEFT, ODGE_UI_RIGHT, ODGE_UI_DOWN<br />
        /// </para>
        /// </summary>
        /// <param name="keysUp">Keys array.</param>
        /// <param name="buttonsUp">Buttons array.</param>
        /// <param name="keysLeft">Keys array.</param>
        /// <param name="buttonsLeft">Buttons array.</param>
        /// <param name="keysRight">Keys array.</param>
        /// <param name="buttonsRight">Buttons array.</param>
        /// <param name="keysDown">Keys array.</param>
        /// <param name="buttonsDown">Buttons array.</param>
        public static void Initialize(Keys[] keysUp,    Buttons[] buttonsUp,
                                      Keys[] keysLeft,  Buttons[] buttonsLeft,
                                      Keys[] keysRight, Buttons[] buttonsRight,
                                      Keys[] keysDown,  Buttons[] buttonsDown) {
            OdgeIO.Map.Set("ODGE_UI_UP",    keysUp,    buttonsUp);
            OdgeIO.Map.Set("ODGE_UI_LEFT",  keysLeft,  buttonsLeft);
            OdgeIO.Map.Set("ODGE_UI_RIGHT", keysRight, buttonsRight);
            OdgeIO.Map.Set("ODGE_UI_DOWN",  keysDown,  buttonsDown);
            IsInitialized = true;
        } 


        //public void SetAllComponentStyle(StyleSheet style) {
        //    OdgeUIVisitor stv = new OdgeUIVisitor(oc => oc.Style = style);
        //    stv.Traverse(_controls);
        //    stv.Traverse(_pops);
        //}
    }
}
