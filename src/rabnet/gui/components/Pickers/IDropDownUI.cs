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
using System.Windows.Forms;

namespace Pickers
{
    /// <summary>
    /// Provides an interface to construct custom drop-down UI for the <see cref="PickerBase&lt;T,U&gt;"/> control.
    /// </summary>
    
    [System.Reflection.Obfuscation(Exclude=true,ApplyToMembers=true)]
    public interface IDropDownUI
    {
        /// <summary>
        /// Occurs when the drop-down UI is closed after selecting a value, not canceled by [ESC] key etc.
        /// </summary>
        /// <include file='DocContent.xml' path='docContent/member[@name="IDropDownUI.CloseDropDownHolder"]'/>
        event EventHandler CloseDropDownHolder;

        /// <summary>
        /// Returns the value currently selected in the drop-down UI.
        /// </summary>
        /// <returns>An <see cref="System.Object"/> value, which is the currently selected value in the drop-down UI.</returns>
        /// <include file='DocContent.xml' path='docContent/member[@name="IDropDownUI.GetSelectedValue"]'/>
        object GetSelectedValue();

        /// <summary>
        /// Sets the selected value in the drop-down UI to the given value.
        /// </summary>
        /// <param name="value">An <see cref="System.Object"/> value to select in the drop-down UI.</param>
        /// <include file='DocContent.xml' path='docContent/member[@name="IDropDownUI.SetSelectedValue"]'/>
        void SetSelectedValue(object value);
    }
}
