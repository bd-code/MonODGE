using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonODGE.UI {
    internal class OdgeComponentUsedException : Exception {
        public OdgeComponentUsedException(string message) : base(message) { }
    }

    internal class OdgeInputMapException : Exception {
        public OdgeInputMapException(string message) : base(message) { }
    }
}
