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
    /// Provides a button-like appearance for picker controls.
    /// </summary>
    public class CheckButtonDisplayAdapter : PickerDisplayAdapterBase
    {
        private CheckBox displayControl;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckButtonDisplayAdapter"/> class.
        /// </summary>
        public CheckButtonDisplayAdapter()
        {
            displayControl = new CheckBox();
            displayControl.Appearance = Appearance.Button;
            displayControl.AutoCheck = false;
            displayControl.TextAlign = ContentAlignment.MiddleCenter;
            displayControl.UseVisualStyleBackColor = true;
            displayControl.Click += new EventHandler(displayControl_Click);
        }

        private void displayControl_Click(object sender, EventArgs e)
        {
            displayControl.Checked = true;
            this.RaiseDropDownEvent();
        }

        /// <summary>
        /// This member overrides the <see cref="PickerDisplayAdapterBase.DisplayControl"/> method.
        /// </summary>
        public override Control DisplayControl
        {
            get { return displayControl; }
        }

        /// <summary>
        /// This member overrides the <see cref="PickerDisplayAdapterBase.Adjust4CloseDropDown"/> method.
        /// </summary>
        public override void Adjust4CloseDropDown()
        {
            this.displayControl.Checked = false;
        }
    }
}
