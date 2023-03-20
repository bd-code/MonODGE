
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI {
    public enum ConponentContexts {
        Normal = 0, Header = 1, Footer = 2, Active = 3
    }


    public class ColorStyleContext {
        public Color Normal { get; set; }       public Color Active { get; set; }
        public Color Header { get; set; }       public Color Footer { get; set; }

        public ColorStyleContext() : 
            this(Color.White, Color.White, Color.White, Color.White) { }

        public ColorStyleContext(Color normal) :
            this(normal, normal, normal, normal) { }

        public ColorStyleContext(Color normal, Color active) :
            this(normal, active, normal, normal) { }

        public ColorStyleContext(Color normal, Color header, Color footer) :
            this(normal, normal, header, footer) { }

        public ColorStyleContext(Color normal, Color active, Color header, Color footer) {
            Normal = normal; Active = active;
            Header = header; Footer = footer;
        }
    }


    public class FontStyleContext {
        private SpriteFont _normal, _active, _header, _footer;
        public SpriteFont Normal => _normal;        public SpriteFont Active => _active;
        public SpriteFont Header => _header;        public SpriteFont Footer => _footer;

        public FontStyleContext(SpriteFont normal) :
            this(normal, normal, normal, normal) { }

        public FontStyleContext(SpriteFont normal, SpriteFont active) :
            this(normal, active, normal, normal) { }

        public FontStyleContext(SpriteFont normal, SpriteFont header, SpriteFont footer) :
            this(normal, normal, header, footer) { }

        public FontStyleContext(SpriteFont normal, SpriteFont active, SpriteFont header, SpriteFont footer) {
            _normal = normal; _active = active;
            _header = header; _footer = footer;
        }
    }


    public class TextureStyleContext {
        public BGTexture Normal { get; set; }        public BGTexture Active { get; set; }
        public BGTexture Header { get; set; }        public BGTexture Footer { get; set; }

        public TextureStyleContext(BGTexture normal, BGTexture active = null, BGTexture header = null, BGTexture footer = null) {
            Normal = normal; Active = active;
            Header = header; Footer = footer;
        }
    }


    public class NinePatchStyleContext {
        public NinePatch Normal { get; set; }        public NinePatch Active { get; set; }
        public NinePatch Header { get; set; }        public NinePatch Footer { get; set; }

        public NinePatchStyleContext(NinePatch normal, NinePatch active = null, NinePatch header = null, NinePatch footer = null) {
            Normal = normal; Active = active;
            Header = header; Footer = footer;
        }
    }
}
