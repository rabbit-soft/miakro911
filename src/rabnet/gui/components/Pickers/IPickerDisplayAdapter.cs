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

namespace Pickers
{
    /// <summary>
    /// Provides an interface for building a display adapter for picker controls.
    /// </summary>
    public interface IPickerDisplayAdapter
    {
        // properties

        /// <summary>
        /// Gets the control used by this adapter for display.
        /// </summary>
        Control DisplayControl { get; }
        
        // events

        /// <summary>
        /// Occurs when the <see cref="IPickerDisplayAdapter.DisplayControl"/> wants the picker to display the
        /// drop-down selection GUI.
        /// </summary>
        event EventHandler DropDown;
        /// <summary>
        /// Occurs when the <see cref="IPickerDisplayAdapter.DisplayControl"/> wants to give the picker the 
        /// opportunity to paint the text in a custom style.
        /// </summary>
        event EventHandler<OwnerDrawTextEventArgs> OwnerDrawText;
        /// <summary>
        /// Occurs when the <see cref="IPickerDisplayAdapter.DisplayControl"/> wants the picker to draw the icon.
        /// </summary>
        event EventHandler<DrawIconEventArgs> DrawIcon;

        // methods

        /// <summary>
        /// Adjusts the appearance of <see cref="IPickerDisplayAdapter.DisplayControl"/> after the drop-down UI
        /// is closed.
        /// </summary>
        void Adjust4CloseDropDown();
        /// <summary>
        /// Invalidates the display control if required.
        /// </summary>
        void InvalidateDisplay();
    }

    /// <summary>
    /// Provides the event data for <see cref="IPickerDisplayAdapter.OwnerDrawText"/> event.
    /// </summary>
    public class OwnerDrawTextEventArgs : EventArgs
    {
        private System.Drawing.Graphics graphics;
        private Rectangle textRectangle;

        /// <summary>
        /// Initalizes a new instance of <see cref="OwnerDrawTextEventArgs"/> class.
        /// </summary>
        /// <param name="graphics">The <see cref="System.Drawing.Graphics"/> surface to draw text on.</param>
        /// <param name="textRectangle">The <see cref="System.Drawing.Rectangle"/> to draw text in.</param>
        public OwnerDrawTextEventArgs(System.Drawing.Graphics graphics, Rectangle textRectangle)
        {
            this.graphics = graphics;
            this.textRectangle = textRectangle;
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Graphics"/> surface to draw text on.
        /// </summary>
        public System.Drawing.Graphics Graphics
        {
            get { return this.graphics; }
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Rectangle"/> to draw text in.
        /// </summary>
        public Rectangle TextRectangle
        {
            get { return this.textRectangle; }
        }
    }

    /// <summary>
    /// Provides the event data for <see cref="IPickerDisplayAdapter.DrawIcon"/> event.
    /// </summary>
    public class DrawIconEventArgs : EventArgs
    {
        private System.Drawing.Graphics graphics;
        private Rectangle iconRectangle;

        /// <summary>
        /// Initalizes a new instance of <see cref="DrawIconEventArgs"/> class.
        /// </summary>
        /// <param name="graphics">The <see cref="System.Drawing.Graphics"/> surface to draw icon on.</param>
        /// <param name="textRectangle">The <see cref="System.Drawing.Rectangle"/> to draw icon in.</param>
        public DrawIconEventArgs(System.Drawing.Graphics graphics, Rectangle textRectangle)
        {
            this.graphics = graphics;
            this.iconRectangle = textRectangle;
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Graphics"/> surface to draw icon on.
        /// </summary>
        public System.Drawing.Graphics Graphics
        {
            get { return this.graphics; }
        }

        /// <summary>
        /// Gets the <see cref="System.Drawing.Rectangle"/> to draw icon in.
        /// </summary>
        public Rectangle IconRectangle
        {
            get { return this.iconRectangle; }
        }
    }
}
