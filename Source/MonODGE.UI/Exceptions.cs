using System;

namespace MonODGE.UI {
    internal class OdgeNotInitializedException : Exception {
        public OdgeNotInitializedException(string message) : base(message) { }
    }
}
