
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using MonODGE.UI.Components;
using MonODGE.UI.Styles;

namespace MonODGE.UI.Utilities {
    /// <summary>
    /// Provides support and useful methods for Keyboard input.
    /// </summary>
    public static class LayoutHelper {
        /// <summary>
        /// Calculates the proper location point of a child OdgeComponent
        /// according to the parent OdgeComponent's Style
        /// </summary>
        /// <param name="parent">Parent OdgeComponent.</param>
        /// <param name="child">Child OdgeComponent to align.</param>
        /// <returns>Point of the properly aligned location of the child.</returns>
        public static Point AlignToPoint(OdgeComponent parent, OdgeComponent child) {
            return AlignToPoint(parent, child.Dimensions);
        }


        /// <summary>
        /// Calculates the proper location point of a child OdgeComponent's Rectangle
        /// according to the parent OdgeComponent's Style.
        /// </summary>
        /// <param name="parent">Parent OdgeComponent.</param>
        /// <param name="child">Rectangle representing the child component's Dimensions.</param>
        /// <returns>Point of the properly aligned location of the child.</returns>
        public static Point AlignToPoint(OdgeComponent parent, Rectangle child) {
            Point pt = new Point();

            // Horizontal
            switch (parent.Style.AlignH) {
                case StyleSheet.AlignmentsH.LEFT:
                    pt.X = parent.Style.Padding.Left;
                    break;
                case StyleSheet.AlignmentsH.CENTER:
                    pt.X = (parent.Width - child.Width) / 2;
                    break;
                case StyleSheet.AlignmentsH.RIGHT:
                    pt.X = parent.Width - child.Width - parent.Style.Padding.Right;
                    break;
            }

            // Vertical
            switch (parent.Style.AlignV) {
                case StyleSheet.AlignmentsV.TOP:
                    pt.Y = parent.Style.Padding.Top;
                    break;
                case StyleSheet.AlignmentsV.CENTER:
                    pt.Y = (parent.Height - child.Height) / 2;
                    break;
                case StyleSheet.AlignmentsV.BOTTOM:
                    pt.Y = parent.Height - child.Height - parent.Style.Padding.Bottom;
                    break;
            }

            return pt;
        }


        /// <summary>
        /// Returns largest width and largest height of the OdgeComponent.
        /// Largest width and height need not be on the same button.
        /// </summary>
        /// <param name="odges">Sequence of OdgeComponents.</param>
        /// <returns>Point containing largest width and height of the OdgeComponents.</returns>
        public static Point GetMaxSizes(IEnumerable<OdgeComponent> odges) {
            Point size = new Point(0, 0);
            if (odges != null) {
                foreach (OdgeComponent og in odges) {
                    if (og.Width > size.X)
                        size.X = og.Width;
                    if (og.Height > size.Y)
                        size.Y = og.Height;
                }
            }
            return size;
        }
    }
}
