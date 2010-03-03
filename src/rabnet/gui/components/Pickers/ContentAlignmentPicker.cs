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
using System.Drawing.Design;
using System.Windows.Forms;

namespace Pickers
{
    /// <summary>
    /// Represents a content alignment picker control.
    /// </summary>
    public class ContentAlignmentPicker : SvcPickerBase<ContentAlignment, ContentAlignmentEditor>
    {
        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.HeightCorrection"/> property.
        /// </summary>
        protected override int HeightCorrection
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Returns the default initial value for this picker control.
        /// </summary>
        /// <returns>A <see cref="System.Drawing.ContentAlignment"/> value which is the 
        /// default initial value for this picker control. The value is always 
        /// <see cref="System.Drawing.ContentAlignment.TopLeft"/></returns>
        protected override ContentAlignment GetDefaultValue()
        {
            return ContentAlignment.TopLeft;
        }

        /// <summary>
        /// Formats the given value into a string, which is used as the text for this control.
        /// </summary>
        /// <param name="value">The <see cref="System.Drawing.ContentAlignment"/> to format.</param>
        /// <returns>A <see cref="System.String"/> value, which is a formatted representation of the given value.</returns>
        protected override string FormatValue(ContentAlignment value)
        {
            return Formatter.InsertSpaces(base.FormatValue(value));
        }

    }
}