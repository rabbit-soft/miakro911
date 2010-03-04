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
    /// Specifies the appearance of a picker control.
    /// </summary>
    public enum PickerAppearance
    {
        /// <summary>
        /// Appearance like a drop-down list combo box.
        /// </summary>
        ComboBox,
        /// <summary>
        /// Appearance like a editable combo box.
        /// </summary>
        EditableComboBox,
        /// <summary>
        /// Appearance like a button (pressed on drop-down).
        /// </summary>
        CheckButton,
        /// <summary>
        /// Custom appearance.
        /// </summary>
        Custom
    }
}