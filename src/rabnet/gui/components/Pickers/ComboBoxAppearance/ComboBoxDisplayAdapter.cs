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

namespace Pickers.ComboBoxAppearance
{
    /// <summary>
    /// Provides a combo box-style (readonly and editable) display for picker controls.
    /// </summary>
    public class ComboBoxDisplayAdapter : PickerDisplayAdapterBase
    {
        private ComboBoxDisplayBase displayControl;

        
        private ComboBoxDisplayAdapter(ComboBoxDisplayBase displayControl, bool displayIcon, int iconWidth)
        {
            this.displayControl = displayControl;
            this.displayControl.DisplayIcon = displayIcon;
            this.displayControl.IconWidth = iconWidth;
            this.displayControl.DropDownButtonClicked += new EventHandler(displayControl_DropDownButtonClicked);
            this.displayControl.DrawIcon += new EventHandler<DrawIconEventArgs>(displayControl_DrawIcon);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComboBoxDisplayAdapter"/> with read-only display.
        /// </summary>
        /// <param name="displayIcon">Indicates whether to display icon or not.</param>
        /// <param name="iconWidth">The width of icon in pixels.</param>
        public ComboBoxDisplayAdapter(bool displayIcon, int iconWidth)
            : this(new ReadonlyComboBoxDisplay(), displayIcon, iconWidth)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComboBoxDisplayAdapter"/> with editable display.
        /// </summary>
        /// <param name="displayIcon">Indicates whether to display icon or not.</param>
        /// <param name="iconWidth">The width of icon in pixels.</param>
        /// <param name="textParser">The method that parses the edited text.</param>
        public ComboBoxDisplayAdapter(bool displayIcon, int iconWidth, TypedTextParser textParser)
            : this(new EditableComboBoxDisplay(textParser), displayIcon, iconWidth)
        {
        }

        private void displayControl_DrawIcon(object sender, DrawIconEventArgs e)
        {
            this.RaiseDrawIconEvent(e);
        }

        private void displayControl_DropDownButtonClicked(object sender, EventArgs e)
        {
            this.RaiseDropDownEvent();
        }

        /// <summary>
        /// This member overrides the <see cref="PickerDisplayAdapterBase.DisplayControl"/> property.
        /// </summary>
        public override Control DisplayControl
        {
            get { return this.displayControl; }
        }

        /// <summary>
        /// Gets the type of display of this <see cref="ComboBoxDisplayAdapter"/>.
        /// </summary>
        /// <value>A <see cref="ComboBoxDisplayType"/> value which indicates whether the combo box is 
        /// editable or readonly.</value>
        public ComboBoxDisplayType ComboBoxType
        {
            get
            {
                if (this.displayControl.GetType() == typeof(ReadonlyComboBoxDisplay))
                    return ComboBoxDisplayType.Readonly;
                else
                    return ComboBoxDisplayType.Editable;
            }
        }

        /// <summary>
        /// This member overrides the <see cref="PickerDisplayAdapterBase.InvalidateDisplay"/> method.
        /// </summary>
        public override void InvalidateDisplay()
        {
            this.displayControl.Invalidate();
        }

    }
}
