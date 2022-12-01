
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonODGE.UI.Utilities;

namespace MonODGE.UI.Components {
    /// <summary>
    /// Displays a message with two answer choice buttons.
    /// </summary>
    public class QuestionBox : OdgeControl {
        public enum AnswerType { Affirmative, Negative, Unanswered }
        public AnswerType Answer { get; private set; }

        private OdgeButton btnYes;
        private OdgeButton btnNo;
        private DialogBox dialog;

        /// <summary>
        /// Sets whether the QuestionBox automatically closes when a Button is chosen.
        /// </summary>
        public bool AutoClose { get; set; }

        private bool _btnInText;
        public bool ButtonsInText {
            get { return _btnInText; }
            set {
                _btnInText = value;
                IsMessy = true;
            }
        }

        private bool _btnOnTop;
        public bool ButtonsOnTop {
            get { return _btnOnTop; }
            set {
                _btnOnTop = value;
                IsMessy = true;
            }
        }

        private bool _btnAnchorIn;
        public bool ButtonsAnchoredTogether {
            get { return _btnAnchorIn; }
            set {
                _btnAnchorIn = value;
                IsMessy = true;
            }
        }

        public QuestionBox(StyleSheet style, Rectangle area, string message, OdgeButton yesBtn, OdgeButton noBtn)
            : base(style) {
            dialog = new DialogBox(Style, new Rectangle(Point.Zero, area.Size), message);
            dialog.ShowMultiPageFooter = false;

            btnYes = yesBtn;
            btnNo = noBtn;
            _btnInText = true;
            _btnOnTop = false;
            _btnAnchorIn = false;

            Answer = AnswerType.Unanswered;
            AutoClose = true;

            Layout();
            PackToSize(area);

            Submit += (o, e) => {
                if (btnYes.IsSelected) {
                    Answer = AnswerType.Affirmative;
                    btnYes.OnSubmit();
                }
                else {
                    Answer = AnswerType.Negative;
                    btnNo.OnSubmit();
                }
            };

            Cancel += (o, e) => {
                if (btnYes.IsSelected) {
                    btnYes.OnUnselected();
                    btnNo.OnSelected();
                }
            };

            Resize += (o, e) => dialog.Size = Size;

            // At first optionNo should be selected.
            btnNo.OnSelected();
        }


        public override void AcceptVisitor(OdgeUIVisitor visitor) {
            btnYes.AcceptVisitor(visitor);
            btnNo.AcceptVisitor(visitor);
            dialog.AcceptVisitor(visitor);
            base.AcceptVisitor(visitor);
        }


        public override void Update() {
            if (CheckSubmit) {
                OnSubmit();
                if (AutoClose) Close();
            }

            else if (!btnYes.IsSelected && InputHelper.LEFT) {
                btnNo.OnUnselected();
                btnYes.OnSelected();
            }

            else if (btnYes.IsSelected && InputHelper.RIGHT) {
                btnYes.OnUnselected();
                btnNo.OnSelected();
            }

            else if (CheckCancel) {
                OnCancel();
                if (AutoClose) Close();
            }

            base.Update();
        }


        public override void Draw(SpriteBatch batch) {
            dialog.Draw(batch, Dimensions);
            btnYes.Draw(batch, Dimensions);
            btnNo.Draw(batch, Dimensions);
        }


        public override void Draw(SpriteBatch batch, Rectangle parentRect) {
            Rectangle where = new Rectangle(parentRect.Location + Location, Location);
            dialog.Draw(batch, where);
            btnYes.Draw(batch, where);
            btnNo.Draw(batch, where);
        }


        public override void Layout() {
            dialog.Layout();
            if (btnYes.IsMessy) btnYes.Layout();
            if (btnNo.IsMessy) btnNo.Layout();

            int by = CalcBtnY();
            Point bxs = CalcBtnX();
            base.Layout();

            btnYes.Location = new Point(bxs.X, by);
            btnNo.Location = new Point(bxs.Y, by);
        }


        private int CalcBtnY() {
            int by;

            if (ButtonsInText) {
                if (ButtonsOnTop) {
                    by = Style.Padding.Top;
                }
                else {
                    by = Height - btnYes.Height - Style.Padding.Bottom;
                }
            }
            else {
                if (ButtonsOnTop) {
                    by = 0 - btnYes.Height - Style.Spacing.Vertical;
                }
                else {
                    by = Height + Style.Spacing.Vertical;
                }
            }

            return by;
        }


        private Point CalcBtnX() {
            Point bxs = new Point();
            //bxs.X is btnYes.X;
            //bxs.Y is btnNo.X

            if (ButtonsAnchoredTogether) {
                bxs.X = (Width - Style.Spacing.Horizontal) / 2 - btnYes.Width;
                bxs.Y = (Width + Style.Spacing.Horizontal) / 2;
            }
            else {
                bxs.X = Style.Padding.Left;
                bxs.Y = Width - btnNo.Width - Style.Padding.Right;
            }

            return bxs;
        }
    }
}
