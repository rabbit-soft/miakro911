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

namespace Pickers
{
    /// <summary>
    /// Allows a control to act like a picker, that uses <see cref="IPickerDisplayAdapter"/> objects, for rendering
    /// display.
    /// </summary>
    public interface IPickerControl
    {
        /// <summary>
        /// Gets or sets the appearance of this picker control.
        /// </summary>
        /// <value>A <see cref="PickerAppearance"/> value specifing the appearance of this picker control.</value>
        PickerAppearance Appearance { get; set; }

        /// <summary>
        /// Gets or sets the display adapter the renders the appearance of this picker control.
        /// </summary>
        /// <value>A <see cref="IPickerDisplayAdapter"/> object the renders this picker control.</value>
        IPickerDisplayAdapter DisplayAdapter { get; set; }
    }
}
