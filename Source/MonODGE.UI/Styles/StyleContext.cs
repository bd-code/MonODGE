
namespace MonODGE.UI.Styles {
    public enum ComponentContexts {
        Normal = 0, Header = 1, Footer = 2, Active = 3
    }


    public class StyleContext<T> {
        private T _normal, _active, _header, _footer;
        public T Normal => _normal; public T Active => _active;
        public T Header => _header; public T Footer => _footer;

        public StyleContext(T normal) :
            this(normal, normal, normal, normal) { }

        public StyleContext(T normal, T active) :
            this(normal, active, normal, normal) { }

        public StyleContext(T normal, T header, T footer) :
            this(normal, normal, header, footer) { }

        public StyleContext(T normal, T active, T header, T footer) {
            _normal = normal; _active = active;
            _header = header; _footer = footer;
        }

        public T Get(ComponentContexts context) {
            switch (context) {
                case ComponentContexts.Active: return _active;
                case ComponentContexts.Header: return _header;
                case ComponentContexts.Footer: return _footer;
                default: return _normal;
            }
        }

        public StyleContext<T> Clone() {
            return new StyleContext<T>(_normal, _active, _header, _footer);
        }
    }
}
