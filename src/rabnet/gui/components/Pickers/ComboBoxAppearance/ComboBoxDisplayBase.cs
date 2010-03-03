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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Pickers.ComboBoxAppearance
{
    /// <summary>
    /// Implements the basic functionality required by a combo box-styled display for picker controls.
    /// </summary>
    [ToolboxItem(false)]
    public abstract class ComboBoxDisplayBase : Control
    {
        private bool displayIcon = false;
        private int iconWidth = 20;

        // Bounds of the different elements painted
        private Rectangle iconRect;
        private RectangleF textRectangle;
        private Rectangle buttonRect;
        
        // Indicates whether the combo button is pushed or not.
        private bool buttonPushed;
        private bool buttonHot;

        /// <summary>
        /// Initializes a new instance of <see cref="ComboBoxDisplayBase"/>.
        /// </summary>
        public ComboBoxDisplayBase()
        {
            this.UpdateLayout(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler DropDownButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<DrawIconEventArgs> DrawIcon;

        /// <summary>
        /// Gets or sets a value that indicates whether the control draws an icon or not.
        /// </summary>
        public bool DisplayIcon
        {
            get { return displayIcon; }
            set
            {
                if (displayIcon != value)
                {
                    displayIcon = value;
                    this.UpdateLayout(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the icon in pixels.
        /// </summary>
        public int IconWidth
        {
            get { return iconWidth; }
            set
            {
                if (iconWidth != value)
                {
                    iconWidth = value;
                    this.UpdateLayout(true);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected RectangleF TextRectangle
        {
            get
            {
                return textRectangle;
            }
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnPaint"/> method.
        /// </summary>
        /// <param name="pe">A <see cref="System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Calling the base class OnPaint
            base.OnPaint(pe);

            // clear everything
            pe.Graphics.Clear(BackColor);

            // draw the combobox behind, if visual styles are enabled and available
            if ((ComboBoxRenderer.IsSupported) & (Application.RenderWithVisualStyles))
                ComboBoxRenderer.DrawTextBox(pe.Graphics, this.ClientRectangle, ComboBoxState.Normal);

            // draw the icon
            if (displayIcon)
            {
                DrawIconEventArgs e = new DrawIconEventArgs(pe.Graphics, iconRect);
                EventHandler<DrawIconEventArgs> handler = this.DrawIcon;
                if (handler != null)
                    handler(this, e);
            }

            // draw the text
            this.DrawTextArea(pe.Graphics);

            // draw the button
            DrawButton(pe.Graphics, ComboBoxState.Normal);

            // draw the border using normal ControlPaint, if visual styles are not available.
            if (!(ComboBoxRenderer.IsSupported) | !(Application.RenderWithVisualStyles))
                ControlPaint.DrawBorder3D(pe.Graphics, 0, 0, Width, Height, Border3DStyle.Sunken);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnMouseDown"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Focused)
                Focus();

            if (buttonRect.Contains(e.X, e.Y))
            {
                DrawButtonIndependent(ComboBoxState.Pressed);
                buttonPushed = true;
                buttonHot = false;
                RaiseDropDownButtonClickedEvent();
            }
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnMouseUp"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (buttonPushed & (buttonRect.Contains(e.X, e.X)))
            {
                DrawButtonIndependent(ComboBoxState.Hot);
                buttonPushed = false;
                buttonHot = true;
            }
            else if (buttonPushed & !(buttonRect.Contains(e.X, e.X)))
            {
                DrawButtonIndependent(ComboBoxState.Normal);
                buttonPushed = false;
                buttonHot = false;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnMouseMove"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (buttonPushed & !(buttonRect.Contains(e.X, e.Y)))
            {
                DrawButtonIndependent(ComboBoxState.Normal);
                buttonPushed = false;
                buttonHot = false;
            }
            else if ((!buttonPushed) & (e.Button == MouseButtons.Left) & (buttonRect.Contains(e.X, e.Y)))
            {
                DrawButtonIndependent(ComboBoxState.Pressed);
                buttonPushed = true;
                buttonHot = false;
            }
            else if ((!buttonPushed) & (e.Button == MouseButtons.None) & (buttonRect.Contains(e.X, e.Y)))
            {
                DrawButtonIndependent(ComboBoxState.Hot);
                buttonHot = true;
            }
            else if ((!buttonPushed) & (e.Button == MouseButtons.None) & !(buttonRect.Contains(e.X, e.Y)))
            {
                DrawButtonIndependent(ComboBoxState.Normal);
                buttonHot = false;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnMouseLeave"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if ((!buttonPushed) & buttonHot)
                DrawButtonIndependent(ComboBoxState.Normal);
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnSizeChanged"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            // update rectangles acc. to new bounds
            this.UpdateLayout(true);

            // Calling the base class OnSizeChanged
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Raises the <see cref="ComboBoxDisplayBase.DropDownButtonClicked"/> event.
        /// </summary>
        protected void RaiseDropDownButtonClickedEvent()
        {
            EventHandler handler = this.DropDownButtonClicked;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the layout of various elements of the control.
        /// </summary>
        /// <param name="textRect"><c>true</c> to update the text rectangle; otherwise <c>false</c>.</param>
        protected virtual void UpdateLayout(bool textRect)
        {
            iconRect = new Rectangle(5, 4, iconWidth, this.Height - 8);
            if (Application.RenderWithVisualStyles)
                buttonRect = new Rectangle(Width - 18, 1, 17, Height - 2);
            else
                buttonRect = new Rectangle(Width - 19, 2, 17, Height - 4);
            if (textRect)
            {
                float x = 4.0F;
                if (displayIcon)
                    x += 1 + iconWidth + 4;
                float y = (Height - this.Font.GetHeight()) / 2;
                textRectangle = new RectangleF(x, y, Width - (20 + x), Height - 8);
            }
        }

        // Draws a combo button using the bounds specified by the buttonRect.
        // state: The state in which to draw the button (i.e. pushed/normal etc.).
        private void DrawButtonIndependent(ComboBoxState state)
        {
            Graphics g = CreateGraphics();
            DrawButton(g, state);
            g.Dispose();
        }

        private void DrawButton(Graphics g, ComboBoxState state)
        {
            if ((ComboBoxRenderer.IsSupported) & (Application.RenderWithVisualStyles))
                ComboBoxRenderer.DrawDropDownButton(g, buttonRect, state);
            else
            {
                ButtonState btnState = ButtonState.Normal;
                switch (state)
                {
                    case ComboBoxState.Hot:
                        btnState = ButtonState.Normal;
                        break;
                    case ComboBoxState.Normal:
                        btnState = ButtonState.Normal;
                        break;
                    case ComboBoxState.Disabled:
                        btnState = ButtonState.Inactive;
                        break;
                    case ComboBoxState.Pressed:
                        btnState = ButtonState.Flat;
                        break;
                }
                ControlPaint.DrawComboButton(g, buttonRect, btnState);
            }
        }

        /// <summary>
        /// Draws the text associated with this control in a specific area, formatting and highlighting.
        /// </summary>
        /// <param name="g">The <see cref="System.Drawing.Graphics"/> surface to draw on.</param>
        /// <include file='DocContent.xml' path='docContent/member[@name="ComboBoxDisplayBase.DrawTextArea"]'/>
        protected abstract void DrawTextArea(Graphics g);

    }
}
