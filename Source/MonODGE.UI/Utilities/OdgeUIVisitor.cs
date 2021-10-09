using System.Collections.Generic;

using MonODGE.UI.Components;

namespace MonODGE.UI.Utilities {
    public class OdgeUIVisitor {
        public System.Action<OdgeComponent> Method { get; private set; }

        public OdgeUIVisitor(System.Action<OdgeComponent> method) {
            Method = method;
        }

        public void Traverse(IEnumerable<OdgeComponent> components) {
            foreach (OdgeComponent oc in components)
                oc.AcceptVisitor(this);
        }
    }
}
