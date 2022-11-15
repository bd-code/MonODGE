using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonODGE.UI.Components {
    /// <summary>
    /// An unframed (windowless) text display intended for damage numbers and other 
    /// "point" text, exclamations, etc.
    /// </summary>
    public class PopText : OdgePopUp {
        public enum MoveType {
            Static, Rising, Falling, Bouncing
        };

        private StyledText _stytex;
        private MoveType _movetype;
        private int bounce_c;
        private float bounce_y;
        private float bounce_vy;
        private float bounce_ay;
        private int _dy;


        public PopText(StyleSheet style, string message, Point position, MoveType motion = MoveType.Static, int lifetime = 80)
            : base(style){
            _stytex = new StyledText(style, message);
            Lifetime = lifetime;

            _movetype = motion;
            if (_movetype == MoveType.Bouncing) {
                bounce_c = 1;
                bounce_y = 0.0f;
                bounce_vy = -3.9f;
                bounce_ay = 0.26f;
            }
            _dy = 0;

            PackToSize(new Rectangle(position.X, position.Y, _stytex.Width, _stytex.Height));
            Layout(); // Just to unset IsMessy.
        }


        public override void Layout() {
            if (_stytex.IsMessy)
                _stytex.Layout();

            base.Layout();
        }


        public override void Update() {
            if (_movetype == MoveType.Rising)
                UpdateRising();
            if (_movetype == MoveType.Falling)
                UpdateFalling();
            if (_movetype == MoveType.Bouncing)
                UpdateBouncing();
            else { // Static, stays put.
                if (Lifetime > 0)
                    Lifetime -= 1;
            }

            // Fade
            if (Lifetime < 64 && Fade == FadeStyle.FadeOut) {
                Style.BackgroundColor *= 1.0f - (1.0f / (Lifetime + 1.0f));
                Style.TextColor *= 1.0f - (1.0f / (Lifetime + 1.0f));
                Style.BorderColor *= 1.0f - (1.0f / (Lifetime + 1.0f));
            }

            if (Lifetime == 0)
                OnTimeout();
        }


        private void UpdateRising() {
            if (Lifetime > 0 && Lifetime % 3 == 0)
                _dy -= 1;
            Lifetime -= 1;
        }
        private void UpdateFalling() {
            if (Lifetime > 0 && Lifetime % 3 == 0)
                _dy += 1;
            Lifetime -= 1;
        }
        private void UpdateBouncing() {
            bounce_y += bounce_vy;
            bounce_vy += bounce_ay;
            _dy = (int)Math.Round(bounce_y);

            if (_dy >= 0) {
                if (bounce_c == 1) {
                    bounce_c++;
                    bounce_y = 0.0f;
                    bounce_vy = -3.6f;
                    bounce_ay = 0.36f;
                }
                else if (bounce_c == 2) {
                    bounce_c++;
                    bounce_y = 0.0f;
                    bounce_vy = -3.5f;
                    bounce_ay = 0.70f;
                }
                else {
                    _movetype = MoveType.Static;
                }
            }

            if (Lifetime > 1) // Do NOT hit zero while bouncing!
                Lifetime--;
        }


        public override void Draw(SpriteBatch batch) {
            if (Lifetime > 0) 
                _stytex.Draw(batch, new Rectangle(Location + new Point(0, _dy), Size));
        }
        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            if (Lifetime > 0)
                _stytex.Draw(batch, new Rectangle(Location + parentRect.Location + new Point(0, _dy), Size));
        }
    }
}
