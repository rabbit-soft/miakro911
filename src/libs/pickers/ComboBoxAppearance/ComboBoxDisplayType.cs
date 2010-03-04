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

namespace Pickers.ComboBoxAppearance
{
    /// <summary>
    /// Specifies the display type (editable or read-only) of a <see cref="ComboBoxDisplayAdapter"/> object.
    /// </summary>
    public enum ComboBoxDisplayType
    {
        /// <summary>
        /// A display like a read-only, drop-down list combo-box.
        /// </summary>
        Readonly,
        /// <summary>
        /// A display like a combo-box that allows text to be edited.
        /// </summary>
        Editable
    }
}