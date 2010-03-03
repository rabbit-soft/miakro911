/* ===============================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
 * ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 * 
 * (C) MSI 2007. All Rights Reserved.
 *
 * Portions of this code have been ported from Palo Mraz's ColorPicker 
 * in VB.NET to C#. All these portions © 2003-2004 LaMarvin.
 * For more information, see http://www.codeproject.com/vb/net/a_colorpicker.asp
 * and http://www.codeproject.com/vb/net/colorpicker2_cp.asp
 *
 * For questions/comments, please contact me at msafderiqbal@hotmail.com.
 * ===============================================================================
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pickers.ComboBoxAppearance
{
    /// <summary>
    /// Provides an appearance of a non-editable combo box for picker controls.
    /// </summary>
    public class ReadonlyComboBoxDisplay : ComboBoxDisplayBase
    {
        private Rectangle focusRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyComboBoxDisplay"/> class.
        /// </summary>
        public ReadonlyComboBoxDisplay()
            : base()
        {
        }

        /// <summary>
        /// This member overrides the <see cref="ComboBoxDisplayBase.DrawTextArea"/> method.
        /// </summary>
        protected override void DrawTextArea(Graphics g)
        {
            if (Focused)
                this.DrawFocusHighlight(g);
            else
            {
                using (SolidBrush br = new SolidBrush(this.ForeColor))
                {
                    DrawText(g, br);
                }
            }
        }

        // Draws a focus rectangle in the control and highlights the text.
        // g: The Graphics surface to draw on.
        private void DrawFocusHighlight(Graphics g)
        {
            g.FillRectangle(SystemBrushes.Highlight, focusRectangle);
            ControlPaint.DrawFocusRectangle(g, focusRectangle, ForeColor, BackColor);
            DrawText(g, SystemBrushes.HighlightText);
        }

        // Erases the focus rectangle in the control and de-highlights the text.
        private void EraseFocusHighlight(Graphics g)
        {
            SolidBrush br = new SolidBrush(BackColor);
            g.FillRectangle(br, focusRectangle);
            br.Color = this.ForeColor;
            DrawText(g, br);
            br.Dispose();
        }

        // Draws the PickerBase<T,U>.Text associated with this control.
        // Parameters:
        // g: The Graphics surface to draw on.
        // br: The Brush to use for drawing.
        private void DrawText(Graphics g, Brush br)
        {
            g.DrawString(this.Text, this.Font, br, TextRectangle);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnEnter"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnEnter(EventArgs e)
        {
            // Draw Focus rectangle
            Graphics g = CreateGraphics();
            this.DrawFocusHighlight(g);
            g.Dispose();

            // Calling the base class OnEnter
            base.OnEnter(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnLeave"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnLeave(EventArgs e)
        {
            // Draw Focus rectangle
            Graphics g = CreateGraphics();
            this.EraseFocusHighlight(g);
            g.Dispose();

            // Calling the base class OnLeave
            base.OnLeave(e);
        }

        /// <summary>
        /// This member overrides the <see cref="ComboBoxDisplayBase.OnSizeChanged"/> method.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateFocusRectangle();
        }

        private void UpdateFocusRectangle()
        {
            int x = 4;
            if (DisplayIcon)
                x += 1 + IconWidth + 4;
            focusRectangle = new Rectangle(x, 3, Width - (20 + x), Height - 6);
        }
    }
}
