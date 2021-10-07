
using MonODGE.IO;

namespace MonODGE.UI.Utilities {
    /// <summary>
    /// Provides easy directional IO checks.
    /// </summary>
    public class InputHelper {
        public static bool UP => OdgeIO.IsCommandPress("_UP");
        public static bool LEFT => OdgeIO.IsCommandPress("_LEFT");
        public static bool RIGHT => OdgeIO.IsCommandPress("_RIGHT");
        public static bool DOWN => OdgeIO.IsCommandPress("_DOWN");
    }
}
