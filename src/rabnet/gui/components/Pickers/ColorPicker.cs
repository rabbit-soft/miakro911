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
    /// Represents a color picker control.
    /// </summary>
    /// <include file='DocContent.xml' path='docContent/member[@name="ColorPicker"]/*'/>
    [ToolboxBitmap(typeof(ColorPicker))]
    public class ColorPicker : SvcPickerBase<Color, System.Drawing.Design.ColorEditor>
    {
        // default color's name
        private const string DEFAULT_COLOR_NAME = "Black";

        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.DisplayAdapter"/> property.
        /// </summary>
        public override IPickerDisplayAdapter DisplayAdapter
        {
            get
            {
                return base.DisplayAdapter;
            }
            set
            {
                base.DisplayAdapter = value;
                SetCheckButtonColors(this.Value);
            }
        }

        /// <summary>
        /// Returns the default initial value for this picker control.
        /// </summary>
        /// <returns>A <see cref="System.Drawing.Color"/> value which is the 
        /// default initial value for this picker control. The value is always 
        /// <see cref="System.Drawing.Color.Black"/> (RGB: 0, 0, 0).</returns>
        protected override Color GetDefaultValue()
        {
            return Color.White;
        }

        /// <summary>
        /// Formats the given value into a string, which is used as the text for this control.
        /// </summary>
        /// <param name="value">The <see cref="System.Drawing.Color"/> to format.</param>
        /// <returns>A <see cref="System.String"/> value, which is a formatted representation of the given value.</returns>
        /// <include file='DocContent.xml' path='docContent/member[@name="ColorPicker.FormatValue"]'/>
        protected override string FormatValue(Color value)
        {
            string colorName = "";
            if (value.IsNamedColor)
                colorName = Formatter.InsertSpaces(value.Name);
            else
                colorName = value.Name.ToUpper();
            return colorName;
        }

        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.SetValueCore"/> method.
        /// </summary>
        protected override void SetValueCore(Color value)
        {
            base.SetValueCore(value);
            SetCheckButtonColors(value);
        }

        private void SetCheckButtonColors(Color value)
        {
            if (this.DisplayAdapter.GetType() == typeof(CheckButtonDisplayAdapter))
            {
                this.DisplayAdapter.DisplayControl.BackColor = value;
                this.DisplayAdapter.DisplayControl.ForeColor = GetInvertedColor(value);
            }
        }

        private Color GetInvertedColor(Color c)
        {
            int r = c.R;
            int g = c.G;
            int b = c.B;

            if ((r + g + b) > ((255 * 3) / 2))
                return Color.Black;
            else
                return Color.White;
        }
    }
}
