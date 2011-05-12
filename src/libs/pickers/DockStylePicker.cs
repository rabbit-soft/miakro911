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
using System.Windows.Forms.Design;

namespace Pickers
{
    /// <summary>
    /// Represents a dock style picker control.
    /// </summary>
    /// <include file='DocContent.xml' path='docContent/member[@name="DockStylePicker"]/*'/>
    public class DockStylePicker : SvcPickerBase<DockStyle, DockEditor>
    {
        /// <summary>
        /// Returns the default initial value for this picker control.
        /// </summary>
        /// <returns>A <see cref="System.Windows.Forms.DockStyle"/> value which is the 
        /// default initial value for this picker control. The value is always 
        /// <see cref="System.Windows.Forms.DockStyle.None"/></returns>
        protected override DockStyle GetDefaultValue()
        {
            return DockStyle.None;
        }

    }
}
