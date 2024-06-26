﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.IO;
using MonODGE.UI.Styles;
using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// A multi-page text display box, intended for NPC dialog.
    /// </summary>
    public class DialogBox : OdgeControl {
        protected string[] messages;
        protected int messageIndex;

        protected StyledText _stytex;
        protected Point textPoint;

        protected string[] footerStrings;
        protected Vector2[] footerDimensions;

        public bool ShowMultiPageFooter { get; set; }

        public string Text {
            get {
                if (messages != null && messageIndex < messages.Length)
                    return messages[messageIndex];
                else
                    return string.Empty;
            }
        }


        public DialogBox(StyleSheet style, Rectangle area, string text) :
            this(style, area, new string[] { text }) { }
        public DialogBox(StyleSheet style, Rectangle area, string[] texts) 
            : base(style) {
            ShowMultiPageFooter = true;
            messages = texts;
            messageIndex = 0;

            footerStrings = new string[3]{
                "<< . . .",
                $"[Page {messageIndex + 1} of {messages.Length}]",
                ". . . >>"
                };

            string s = TextWrapper.WrapToWidth(messages[messageIndex], Style.Fonts.Normal, 
                area.Width - style.Padding.Left - style.Padding.Right);
            _stytex = new StyledText(Style, s);

            Location = area.Location;
            Size = area.Size;
            Layout();

            Submit += (o, e) => {
                messageIndex++;
                if (messageIndex >= messages.Length)
                    Close();
                else
                    OnTextChanged();
            };
            StyleChanged += (o, e) => _stytex.Style = Style;
        }


        /// <summary>
        /// This is called when the dialog page is changed and new text displays.
        /// It is not called on style or position changes to the current text, only when a
        /// completely new string displays.
        /// </summary>
        protected void OnTextChanged() {
            string s = TextWrapper.WrapToWidth(messages[messageIndex], Style.Fonts.Normal, 
                Width - Style.Padding.Left - Style.Padding.Right);
            _stytex = new StyledText(Style, s);
            footerStrings[1] = $"[Page {messageIndex + 1} of {messages.Length}]";
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler TextChanged;


        public override void Update() {
            if (IsSubmitPressed) {
                OnSubmit();
            }

            else if (messageIndex > 0 && OdgeIO.IsCommandPress("ODGE_UI_LEFT")) {
                messageIndex--;
                OnTextChanged();
            }

            else if (messageIndex < messages.Length - 1 && OdgeIO.IsCommandPress("ODGE_UI_RIGHT")) {
                messageIndex++;
                OnTextChanged();
            }

            else if (IsCancelPressed) {
                OnCancel();
            }

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            DrawBG(batch);
            DrawBorders(batch);

            if (messageIndex < messages.Length) {
                _stytex.Draw(batch, new Rectangle(Location + textPoint, Size));
                if (messages.Length > 1)
                    DrawFooter(batch, Dimensions);
            }
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            DrawBG(batch, parentRect);
            DrawBorders(batch, parentRect);

            if (messageIndex < messages.Length) {
                _stytex.Draw(batch, new Rectangle(parentRect.Location + Location + textPoint, Size));
                if (messages.Length > 1) {
                    Rectangle where = new Rectangle(parentRect.Location + Location, Size);
                    DrawFooter(batch, where);
                }
            }
        }


        private void DrawFooter(SpriteBatch batch, Rectangle where) {
            // << . . .
            if (messageIndex > 0)
                batch.DrawString(Style.Fonts.Footer, footerStrings[0],
                    new Vector2(
                        where.X + Style.Padding.Left,
                        where.Bottom - footerDimensions[0].Y - Style.Padding.Bottom),
                    Style.TextColors.Footer);

            // [Page i of n]
            if (ShowMultiPageFooter) {
                batch.DrawString(Style.Fonts.Footer, footerStrings[1],
                    new Vector2(
                        where.Center.X - (footerDimensions[1].X / 2),
                        where.Bottom - footerDimensions[1].Y - Style.Padding.Bottom),
                    Style.TextColors.Footer);
            }

            // . . . >>
            if (messageIndex < messages.Length - 1)
                batch.DrawString(Style.Fonts.Footer, footerStrings[2],
                    new Vector2(
                        where.Right - footerDimensions[2].X - Style.Padding.Right,
                        where.Bottom - footerDimensions[2].Y - Style.Padding.Bottom
                        ),
                    Style.TextColors.Footer);
        }


        public override void Layout() {
            if (messages != null) {
                if (_stytex.IsMessy)
                    _stytex.Layout();

                footerDimensions = new Vector2[] {
                    Style.Fonts?.Footer?.MeasureString(footerStrings[0]) ?? Vector2.Zero,
                    Style.Fonts?.Footer?.MeasureString(footerStrings[1]) ?? Vector2.Zero,
                    Style.Fonts?.Footer?.MeasureString(footerStrings[2]) ?? Vector2.Zero
                };

                textPoint = LayoutHelper.AlignToPoint(this, _stytex);
                if (Style.AlignV == StyleSheet.AlignmentsV.BOTTOM && messages.Length > 1)
                    textPoint.Y -= (int)footerDimensions[0].Y - Style.Spacing.Vertical;

            }
            base.Layout();
        }
    }
}
