using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;
using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Base class for all MonODGE.UI components.
    /// </summary>
    public abstract class OdgeComponent {
        public enum Anchors {
            LEFTTOP, CENTERTOP, RIGHTTOP,
            LEFTCENTER, CENTER, RIGHTCENTER,
            LEFTBOTTOM, CENTERBOTTOM, RIGHTBOTTOM
        }

        internal OdgeUI _manager;

        public string Name { get; set; }
        public bool IsMessy { get; protected set; }


        private StyleSheet _style;
        public StyleSheet Style {
            get { return _style; }
            set {
                _style = value;
                _style.RegisterChanges();
            }
        }


        private Rectangle _dimensions;
        public Rectangle Dimensions => _dimensions;
        

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

        protected virtual int MinWidth => 0;
        protected virtual int MinHeight => 0;


        /// <summary>
        /// This is called when the OdgeComponent is added to the UI Manager.
        /// </summary>
        protected virtual void OnOpened() { Opened?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Opened;

        /// <summary>
        /// This is called when a property changes in the OdgeComponent's StyleSheet,
        /// or when setting OdgeComponent.Style to a new StyleSheet.
        /// </summary>
        protected virtual void OnStyleChanged() {
            IsMessy = true;
            StyleChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StyleChanged;

        /// <summary>
        /// This is called when the OdgeComponent's Dimensions.X or Dimensions.Y positions change.
        /// This should be overriden to reposition any text, textures, or other elements 
        /// when the Component's position changes.
        /// </summary>
        protected virtual void OnMove() {
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
        protected virtual void OnResize() {
            IsMessy = true;
            Resize?.Invoke(this, EventArgs.Empty); 
        }
        public event EventHandler Resize;


        /// <summary>
        /// This is called when the OdgeComponent is closed.
        /// </summary>
        protected virtual void OnClosed() { Closed?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Closed;


        public virtual void Update() {
            if (Style.IsChanged)
                OnStyleChanged();

            if (IsMessy) {
                Layout();
                if (Width < MinWidth)
                    Width = MinWidth;
                if (Height < MinHeight)
                    Height = MinHeight;
            }
        }


        public virtual void Draw(SpriteBatch batch) { }
        public virtual void Draw(SpriteBatch batch, Rectangle parentRect) { }


        internal void PerformOpen() {
            OnOpened();
        }

        internal void PerformClose() {
            OnClosed();
        }


        /// <summary>
        /// Repositions the OdgeComponent's internal child components.
        /// 
        /// Layout should be called every time changes are made to the StyleSheet or Dimensions 
        /// that result in a repositioning of text or other sub-elements.
        /// </summary>
        public virtual void Layout() {
            IsMessy = false;
        }


        /// <summary>
        /// Resizes the OdgeComponent down to its minimum width and height.
        /// </summary>
        protected void SizeToMin() {
            Size = new Point(MinWidth, MinHeight);
        }


        /// <summary>
        /// Accepts a OdgeUIVisitor, which runs a method on the OdgeComponent.
        /// </summary>
        /// <param name="visitor">OdgeUIVisitor.</param>
        public virtual void AcceptVisitor(OdgeUIVisitor visitor) {
            visitor.Method?.Invoke(this);
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Background in color Style.BackgroundColor
        /// to the component's Dimensions.
        /// </summary>
        /// <param name="batch">SpriteBatch.</param>
        protected void DrawBG(SpriteBatch batch) {
            Style.Background?.Draw(batch, Dimensions, Style.BackgroundColor);
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Background in color Style.BackgroundColor
        /// to the component's Dimensions relative to a parent Rectangle.
        /// </summary>
        /// <param name="batch">SpriteBatch.</param>
        /// <param name="where">Rectangle area to draw.</param>
        protected void DrawBG(SpriteBatch batch, Rectangle parentRect) {
            Style.Background?.Draw(
                batch, 
                new Rectangle(parentRect.Location + Location, Size), 
                Style.BackgroundColor);
        }


        /// <summary>
        /// Draws only the corner tiles of Style.Borders to the four corners of the component.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        protected void DrawCorners(SpriteBatch batch) {
            Style.Borders?.DrawCorners(batch, Dimensions, Style.BorderColor);
        }


        /// <summary>
        /// Draws only the corner tiles of Style.Borders to the four corners of the component 
        /// relative to a parent Rectangle.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="where">Rectangle area to draw.</param>
        protected void DrawCorners(SpriteBatch batch, Rectangle parentRect) {
            Style.Borders?.DrawCorners(
                batch, 
                new Rectangle(parentRect.Location + Location, Size), 
                Style.BorderColor);
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Borders around the OdgeComponent.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        protected void DrawBorders(SpriteBatch batch) {
            Style.Borders?.Draw(batch, Dimensions, Style.BorderColor);
        }


        /// <summary>
        /// Draws the Texture2D saved in Style.Borders around the OdgeComponent
        /// relative to a parent Rectangle.
        /// </summary>
        /// <param name="batch">SpriteBatch</param>
        /// <param name="parentRect">Rectangle area to draw.</param>
        protected void DrawBorders(SpriteBatch batch, Rectangle parentRect) {
            Style.Borders?.Draw(
                batch, 
                new Rectangle(parentRect.Location + Location, Size), 
                Style.BorderColor);
        }


        /// <summary>
        /// Snaps the OdgeComponent to a screen position specified in SnapAnchors.
        /// </summary>
        /// <param name="anchor">A SnapAnchors enum value.</param>
        /// <param name="screenwidth">Screen width.</param>
        /// <param name="screenheight">Screen height.</param>
        public virtual void SnapTo(Anchors anchor, int screenwidth, int screenheight) {
            SnapTo(anchor, new Rectangle(0, 0, screenwidth, screenheight));
        }


        /// <summary>
        /// Snaps the OdgeComponent to a position within a rectangle.
        /// </summary>
        /// <param name="anchor">A SnapAnchors enum value.</param>
        /// <param name="screenrect">The bounding rectangle.</param>
        public virtual void SnapTo(Anchors anchor, Rectangle screenrect) {
            int nx = 0;
            int ny = 0;

            if (anchor == Anchors.LEFTTOP) {
                nx = screenrect.X;
                ny = screenrect.Y;
            }

            else if (anchor == Anchors.CENTERTOP) {
                nx = ((screenrect.Width - Width) / 2) + screenrect.X;
                ny = screenrect.Y;
            }

            else if (anchor == Anchors.RIGHTTOP) {
                nx = screenrect.Width - Width + screenrect.X;
                ny = screenrect.Y;
            }

            else if (anchor == Anchors.LEFTCENTER) {
                nx = screenrect.X;
                ny = ((screenrect.Height - Height) / 2) + screenrect.Y;
            }

            else if (anchor == Anchors.CENTER) {
                nx = ((screenrect.Width - Width) / 2) + screenrect.X;
                ny = ((screenrect.Height - Height) / 2) + screenrect.Y;
            }

            else if (anchor == Anchors.RIGHTCENTER) {
                nx = screenrect.Width - Width + screenrect.X;
                ny = ((screenrect.Height - Height) / 2) + screenrect.Y;
            }

            else if (anchor == Anchors.LEFTBOTTOM) {
                nx = screenrect.X;
                ny = screenrect.Height - Height + screenrect.Y;
            }

            else if (anchor == Anchors.CENTERBOTTOM) {
                nx = ((screenrect.Width - Width) / 2) + screenrect.X;
                ny = screenrect.Height - Height + screenrect.Y;
            }

            else if (anchor == Anchors.RIGHTBOTTOM) {
                nx = screenrect.Width - Width + screenrect.X;
                ny = screenrect.Height - Height + screenrect.Y;
            }

            Location = new Point(nx, ny);
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
     * 3. Set Dimensions: Location and Size.
     * -- Components require a Size set in the constructor.
     * 
     * 2. Layout sub-components using style padding, spacing.
     * 
     * 4. Add any necessary event handlers.
     * 
     * 
     * This also gives Layout() a roadmap:
     * 1. Layout all sub-components. 
     * -- Don't use a visitor.
     * -- This gives us a new MinWidth, MinHeight.
     * 
     * 2. Can then resize if necessary.
     * 
     * 3. Call base.Layout().
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
        protected virtual void OnSubmit() { Submit?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Submit;

        /// <summary>
        /// This is called when the user presses the key assigned in Style.CancelKey,
        /// or the GamePad Button assigned in Style.CancelButton.
        /// </summary>
        protected virtual void OnCancel() {
            Cancel?.Invoke(this, EventArgs.Empty);
            if (Style.CloseOnCancel)
                Close();
        }
        public event EventHandler Cancel;


        /// <summary>
        /// Closes the OdgeControl.
        /// </summary>
        public void Close() {
            if (_manager != null)
                _manager.Close(this);
            else
                throw new Exception($"Control {Name}: Close() called without UI manager.");
        }

        protected bool IsSubmitPressed => 
            OdgeIO.KB.IsKeyPress(Style.SubmitKey) || OdgeIO.GP.IsButtonPress(0, Style.SubmitButton); 

        protected bool IsCancelPressed => 
            OdgeIO.KB.IsKeyPress(Style.CancelKey) || OdgeIO.GP.IsButtonPress(0, Style.CancelButton); 
    }

    ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// PopUps are temporary non-modal components that do not respond to input
    /// and vanish after a specified time.
    /// </summary>
    public abstract class OdgePopUp : OdgeComponent {
        public enum FadeStyle { None, FadeOut };
        public FadeStyle Fade { get; set; }
        public int Lifetime { get; set; }

        protected virtual void OnTimeout() { TimedOut?.Invoke(this, EventArgs.Empty); }
        public event EventHandler TimedOut;

        public OdgePopUp(StyleSheet style) {
            Style = style;
        }

        /// <summary>
        /// Closes the OdgePopUp.
        /// </summary>
        public void Close() {
            if (_manager != null)
                _manager.Close(this);
            else
                throw new Exception($"Popup {Name}: Close() called without UI manager.");
        }
    }

    ///////////////////////////////////////////////////////////////////////////

    public abstract class OdgeButton : OdgeComponent {
        public bool IsSelected {
            get { return _isSelected; } 
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    if (_isSelected)
                        OnSelected();
                    else
                        OnUnselected();
                }
            } 
        }
        private bool _isSelected;

        public OdgeButton(StyleSheet style, EventHandler action) {
            Style = style;
            Submit += action;
        }

        /// <summary>
        /// This is called when the user presses the key assigned in Style.SubmitKey.
        /// </summary>
        protected virtual void OnSubmit() { Submit?.Invoke(this, EventArgs.Empty); }
        public event EventHandler Submit;

        /// <summary>
        /// This is called when an option is highlighted in the ListMenu.
        /// </summary>
        protected virtual void OnSelected() {
            Select?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Select;

        /// <summary>
        /// This is called when an option is unhighlighted (another option is selected) in the
        /// ListMenu.
        /// </summary>
        protected virtual void OnUnselected() {            
            Unselect?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Unselect;

        /// <summary>
        /// Generates a Submit event for the OdgeButton.
        /// </summary>
        public void PerformSubmit() {
            OnSubmit();
        }
    }

}