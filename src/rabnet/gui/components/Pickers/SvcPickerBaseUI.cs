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
using System.Windows.Forms;

namespace Pickers
{
    /// <summary>
    /// Provides the drop-down UI for the <see cref="SvcPickerBase&lt;T,E&gt;"/> picker control.
    /// </summary>
    [ToolboxItem(false)]
    public sealed class SvcPickerBaseUI : Control, IDropDownUI
	{
        #region IDropDownUI Members

        /// <summary>
        /// Occurs when the drop-down UI is closed after selecting a value, not canceled by [ESC] key etc.
        /// </summary>
        public event EventHandler CloseDropDownHolder;

        /// <summary>
        /// Returns the value currently selected in the drop-down UI.
        /// </summary>
        /// <returns>An <see cref="System.Object"/> value, which is the currently selected value in the drop-down UI.</returns>
        object IDropDownUI.GetSelectedValue()
        {
            return null;
        }

        /// <summary>
        /// Sets the selected value in the drop-down UI to the given value.
        /// </summary>
        /// <param name="value">An <see cref="System.Object"/> value to select in the drop-down UI.</param>
        void IDropDownUI.SetSelectedValue(object value)
        {

        }

        #endregion

        /// <summary>
        /// This members overrides <see cref="System.Windows.Forms.Control.OnVisibleChanged"/> method.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if ((this.Visible == false) & (CloseDropDownHolder != null))
                CloseDropDownHolder(this, EventArgs.Empty);
        }

		private void InitializeComponent()
		{
			this.SuspendLayout();
			this.ResumeLayout(false);

		}

    }
}
