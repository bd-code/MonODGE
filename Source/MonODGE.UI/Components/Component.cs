using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Base class for all MonODGE.UI components.
    /// </summary>
    public abstract class OdgeComponent {
        public enum SnapAnchors {
            TOPLEFT, TOPCENTER, TOPRIGHT,
            LEFT, CENTER, RIGHT,
            BOTTOMLEFT, BOTTOM, BOTTOMRIGHT
        }

        internal OdgeUI _manager;

        public string Name { get; set; }
        public bool IsMessy { get; protected set; }


        private StyleSheet _style;
        public virtual StyleSheet Style {
            get { return _style; }
            set {
                _style = value;
                _style.RegisterChanges();
            }
        }


        private Rectangle _dimensions;
        public virtual Rectangle Dimensions => _dimensions;
        

        public Point Location {
            get { return _dimensions.Location; }
            set {
                if (_dimensions.Location != value) {
                    _dimensions.Location = value;
                    OnMove();
                }
            }
        }

        public Point Size {
            get { return _dimensions.Size; }
            set {
                Point newvalue = new Point(
                    Math.Max(value.X, MinWidth),
                    Math.Max(value.Y, MinHeight)
                    );

                if (_dimensions.Size != newvalue) {
                    _dimensions.Size = newvalue;
                    OnResize();
                }
            }
        }

        public int X {
            get { return _dimensions.X; }
            set { Location = new Point(value, Y); }
        }
        public int Y {
            get { return _dimensions.Y; }
            set { Location = new Point(X, value); }
        }
        public int Width {
            get { return _dimensions.Width; }
            set { Size = new Point(value, Height); }
        }
        public int Height {
            get { return _dimensions.Height; }
            set { Size = new Point(Width, value); }
        }

        protected virtual int MinWidth { get { return 0; } }
        protected virtual int MinHeight { get { return 0; } }


        protected void PackToSize() {
            PackToSize(new Rectangle(X, Y, MinWidth, MinHeight));
        }
        protected void PackToSize(Point size) {
            PackToSize(new Rectangle(_dimensions.Location, size));
        }
        protected void PackToSize(Rectangle area) {
            area.Width = Math.Max(area.Width, MinWidth);
            area.Height = Math.Max(area.Height, MinHeight);
            _dimensions = area;
        }


        /// <summary>
        /// This is called when the OdgeComponent is added to the UI Manager.
        /// </summary>
        public virtual void OnOpened() { Opened?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Opened;

        /// <summary>
        /// This is called when a property changes in the OdgeComponent's StyleSheet,
        /// or when setting OdgeComponent.Style to a new StyleSheet.
        /// </summary>
        public virtual void OnStyleChanged() {
            IsMessy = true;
            StyleChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StyleChanged;

        /// <summary>
        /// This is called when the OdgeComponent's Dimensions.X or Dimensions.Y positions change.
        /// This should be overriden to reposition any text, textures, or other elements 
        /// when the Component's position changes.
        /// </summary>
        public virtual void OnMove() {
            IsMessy = true;
            Move?.Invoke(this, EventArgs.Empty); 
        }
        public event EventHandler Move;

        /// <summary>
        /// This is called when the OdgeComponent's Dimensions.Width or Dimensions.Height variables
        /// change. This can be overriden if any additional calculations or resizing needs to
        /// occur.
        /// 
        /// OnResize should not be used to re-resize Dimensions, as this will recursively call 
        /// OnResize(). If we need to check that the new width and height are large enough to 
        /// contain sub-elements, override the Dimensions property itself to check the values.
        /// </summary>
        public virtual void OnResize() {
            IsMessy = true;
            Resize?.Invoke(this, EventArgs.Empty); 
        }
        public event EventHandler Resize;


        /// <summary>
        /// This is called when the OdgeComponent is closed.
        /// </summary>
        public virtual void OnClosed() { Closed?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Closed;


        /// <summary>
        /// Recalculates the OdgeComponent's internal layout.
        /// 
        /// Layout should be called every time changes are made to the StyleSheet or Dimensions 
        /// that result in a repositioning of text or other sub-elements.
        /// </summary>
        public virtual void Layout() {
            IsMessy = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public virtual void AcceptVisitor(OdgeUIVisitor visitor) {
            visitor.Method?.Invoke(this);
        }


        public virtual void Update() { }
        public virtual void Draw(SpriteBatch batch) { }
        public virtual void Draw(SpriteBatch batch, Rectangle parentRect) { }


        /// <summary>
        /// Draws the Texture2D saved in Style.Background in color Style.BackgroundColor
        /// to the position and area saved in Dimensions.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        protected void DrawBG(SpriteBatch batch) {
            if (Style.Background != null)
                batch.Draw(Style.Background, Dimensions, Style.BackgroundColor);
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Background in color Style.BackgroundColor
        /// to the position and area saved in Rectangle 'where'.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="where">Rectangle area to draw.</param>
        protected void DrawBG(SpriteBatch batch, Rectangle parentRect) {
            if (Style.Background != null) {
                var rect = new Rectangle(parentRect.Location + Dimensions.Location, Dimensions.Size);
                batch.Draw(Style.Background, rect, Style.BackgroundColor);
            }
        }


        /// <summary>
        /// Draws only the corner tiles of Style.Borders to the four corners saved in Dimensions.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        protected void DrawCorners(SpriteBatch batch) {
            if (Style.Borders != null) {
                Style.Borders.DrawCorners(batch, Dimensions, Style.BorderColor);
            }
        }


        /// <summary>
        /// Draws only the corner tiles of Style.Borders to the four corners saved in 
        /// Rectangle 'where'.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="where">Rectangle area to draw.</param>
        protected void DrawCorners(SpriteBatch batch, Rectangle parentRect) {
            if (Style.Borders != null) {
                var rect = new Rectangle(parentRect.Location + Dimensions.Location, Dimensions.Size);
                Style.Borders.DrawCorners(batch, rect, Style.BorderColor);
            }
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Borders around the OdgeComponent.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        protected void DrawBorders(SpriteBatch batch) {
            if (Style.Borders != null) {
                Style.Borders.Draw(batch, Dimensions, Style.BorderColor);
            }
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Borders around Rectangle 'where'.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="parentRect">Rectangle area to draw.</param>
        protected void DrawBorders(SpriteBatch batch, Rectangle parentRect) {
            if (Style.Borders != null) {
                var rect = new Rectangle(parentRect.Location + Dimensions.Location, Dimensions.Size);
                Style.Borders.Draw(batch, rect, Style.BorderColor);
            }
        }


        /// <summary>
        /// Snaps the OdgeComponent to a screen position specified in SnapAnchors.
        /// </summary>
        /// <param name="anchor">A SnapAnchors enum value.</param>
        /// <param name="screenwidth">Screen width.</param>
        /// <param name="screenheight">Screen height.</param>
        public virtual void SnapTo(SnapAnchors anchor, int screenwidth, int screenheight) {
            if (anchor == SnapAnchors.TOPLEFT) {
                Location = new Point(0, 0);
            }

            else if (anchor == SnapAnchors.TOPCENTER) {
                Location = new Point((screenwidth - Dimensions.Width) / 2, 0);
            }

            else if (anchor == SnapAnchors.TOPRIGHT) {
                Location = new Point(screenwidth - Dimensions.Width, 0);
            }

            else if (anchor == SnapAnchors.LEFT) {
                Location = new Point(0, (screenheight - Dimensions.Height) / 2);
            }

            else if (anchor == SnapAnchors.CENTER) {
                Location = new Point(
                    (screenwidth - Dimensions.Width) / 2,
                    (screenheight - Dimensions.Height) / 2);
            }

            else if (anchor == SnapAnchors.RIGHT) {
                Location = new Point(
                    screenwidth - Dimensions.Width,
                    (screenheight - Dimensions.Height) / 2);
            }

            else if (anchor == SnapAnchors.BOTTOMLEFT) {
                Location = new Point(0, screenheight - Dimensions.Height);
            }

            else if (anchor == SnapAnchors.BOTTOM) {
                Location = new Point(
                    (screenwidth - Dimensions.Width) / 2,
                    screenheight - Dimensions.Height);
            }

            else if (anchor == SnapAnchors.BOTTOMRIGHT) {
                Location = new Point(
                    screenwidth - Dimensions.Width,
                    screenheight - Dimensions.Height);
            }

            Layout();
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    /*
     * OdgeComponent subclass constructors should follow this flow:
     * 0. Set style. 
     * -- This is always done in base class.
     * 
     * 1. Construct all known sub-components. 
     * -- This should provide a MinWidth and MinHeight.
     * 
     * 2. Pack to a default or passed Rectangle.
     * -- This requires MinWidth, MinHeight.
     * -- A parent Rectangle is required for some sub-component layouts, alignments.
     * 
     * 3. Layout sub-components.
     * -- Some layout options require as rectangle first.
     * 
     * 4. Add any necessary event handlers.
     * 
     * 
     * This also gives Layout() a roadmap:
     * 1. Layout all sub-components. 
     * -- Don't use a visitor.
     * -- This gives us a new MinWidth, MinHeight.
     * 
     * 2. Run PacktoSize once we have new MinWidth, MinHeight.
     * -- Again, final parent size is a dependency for some layouts.
     * 
     * 3. Layout sub-components using Location property.
     * 4. Call base.Layout().
     */

    ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Controls are modal components that require user input to proceed.
    /// </summary>
    public abstract class OdgeControl : OdgeComponent {
        public OdgeControl(StyleSheet style) {
            Style = style;
        }

        /// <summary>
        /// This is called when the user presses the Key assigned in Style.SubmitKey,
        /// or the GamePad Button assigned in Style.SubmitButton.
        /// </summary>
        public virtual void OnSubmit() { Submit?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Submit;

        /// <summary>
        /// This is called when the user presses the key assigned in Style.CancelKey,
        /// or the GamePad Button assigned in Style.CancelButton.
        /// </summary>
        public virtual void OnCancel() {
            Cancel?.Invoke(this, EventArgs.Empty);
            if (Style.CloseOnCancel)
                Close();
        }
        public event EventHandler Cancel;

        public void Close() {
            if (_manager != null) {
                _manager.Close(this);
                OnClosed();
            }
        }

        protected bool CheckSubmit {
            get { return OdgeIO.KB.IsKeyPress(Style.SubmitKey) || OdgeIO.GP.IsButtonPress(0, Style.SubmitButton); }
        }

        protected bool CheckCancel {
            get { return OdgeIO.KB.IsKeyPress(Style.CancelKey) || OdgeIO.GP.IsButtonPress(0, Style.CancelButton); }
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    /*
     * NOTE: If a "popup" doesn't vanish after the Timeout, THEN IT ISN'T A POPUP!
     * Looking at StyledText(fixed), ProgressBar here.
     * Could add PopImage, Countdown classes.
     * 
     * Also, change type name from OdgePopUp to OdgeNotification.
     */

    /// <summary>
    /// PopUps are temporary non-modal components that do not respond to input
    /// and vanish after a specified time.
    /// </summary>
    public abstract class OdgePopUp : OdgeComponent {
        public enum FadeStyle { None, FadeOut };
        public FadeStyle Fade { get; set; }
        public int Lifetime { get; set; }

        public virtual void OnTimeout() { TimedOut?.Invoke(this, EventArgs.Empty); }
        public event EventHandler TimedOut;

        public OdgePopUp(StyleSheet style) {
            Style = style;
        }

        public void Close() {
            if (_manager != null) {
                _manager.Close(this);
                OnClosed();
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    public abstract class OdgeButton : OdgeComponent {
        public bool IsSelected { get; private set; }

        public OdgeButton(StyleSheet style, EventHandler action) {
            Style = style;
            Submit += action;
        }

        /// <summary>
        /// This is called when the user presses the key assigned in Style.SubmitKey.
        /// </summary>
        public virtual void OnSubmit() { Submit?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Submit;

        /// <summary>
        /// This is called when an option is highlighted in the ListMenu.
        /// </summary>
        public virtual void OnSelected() {
            IsSelected = true;
            Select?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Select;

        /// <summary>
        /// This is called when an option is unhighlighted (another option is selected) in the
        /// ListMenu.
        /// </summary>
        public virtual void OnUnselected() {
            IsSelected = false;
            Unselect?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Unselect;
    }

}