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
    /// Implements the basic functionality required for building a display adapter for picker controls.
    /// </summary>
    public abstract class PickerDisplayAdapterBase : IPickerDisplayAdapter
    {
        /// <summary>
        /// Occurs when the <see cref="PickerDisplayAdapterBase.DisplayControl"/> wants the picker to display the
        /// drop-down selection GUI.
        /// </summary>
        public event EventHandler DropDown;
        /// <summary>
        /// Occurs when the <see cref="PickerDisplayAdapterBase.DisplayControl"/> wants to give the picker the 
        /// opportunity to paint the text in a custom style.
        /// </summary>
        public event EventHandler<OwnerDrawTextEventArgs> OwnerDrawText;
        /// <summary>
        /// Occurs when the <see cref="PickerDisplayAdapterBase.DisplayControl"/> wants the picker to draw the icon.
        /// </summary>
        public event EventHandler<DrawIconEventArgs> DrawIcon;

        /// <summary>
        /// Gets the control used by this adapter for display.
        /// </summary>
        public abstract Control DisplayControl
        {
            get;
        }

        /// <summary>
        /// Raises the <see cref="PickerDisplayAdapterBase.DropDown"/> event.
        /// </summary>
        protected void RaiseDropDownEvent()
        {
            EventHandler handler = this.DropDown;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="PickerDisplayAdapterBase.OwnerDrawText"/> event.
        /// </summary>
        /// <param name="e">An <see cref="OwnerDrawTextEventArgs"/> object that contains the event data.</param>
        protected void RaiseOwnerDrawTextEvent(OwnerDrawTextEventArgs e)
        {
            EventHandler<OwnerDrawTextEventArgs> handler = this.OwnerDrawText;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PickerDisplayAdapterBase.DrawIcon"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DrawIconEventArgs"/> object that contains the event data.</param>
        protected void RaiseDrawIconEvent(DrawIconEventArgs e)
        {
            EventHandler<DrawIconEventArgs> handler = this.DrawIcon;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Adjusts the appearance of <see cref="PickerDisplayAdapterBase.DisplayControl"/> after the drop-down UI
        /// is closed.
        /// </summary>
        public virtual void Adjust4CloseDropDown()
        {
            // there is nothing interesting to do here, but a inheritor may do
        }

        /// <summary>
        /// Invalidates the display control if required.
        /// </summary>
        public virtual void InvalidateDisplay()
        {
        }
    }
}
