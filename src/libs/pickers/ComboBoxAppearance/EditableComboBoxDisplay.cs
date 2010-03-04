/* ===============================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
 * ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 * 
 * (C) MSI 2007. All Rights Reserved.
 *
 * Portions of this code have been ported from Palo Mraz's ColorPicker 
 * in VB.NET to C#. (In particular, THIS FILE SEEMS LIKE A TOTAL CONVERSION)
 * All these portions © 2003-2004 LaMarvin.
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
    /// References the method that parses the given text and sets the picker value.
    /// </summary>
    /// <param name="text">A <see cref="System.String"/> containing the text to parse.</param>
    /// <returns><c>true</c> if the parse was successfull; otherwise false.</returns>
    public delegate bool TypedTextParser(string text);

    /// <summary>
    /// Provides the appearance of an editable combo box control for picker controls.
    /// </summary>
    public class EditableComboBoxDisplay : ComboBoxDisplayBase
    {
        private ComboTextBox textBox;

        private bool enterPressedInTextBox;

        /// <summary>
        /// <c>ParseTypedText</c> is the method that parses the text typed by the user and sets the picker value.
        /// </summary>
        public TypedTextParser ParseTypedText;

        /// <summary>
        /// Initializes a new instance of <see cref="EditableComboBoxDisplay"/> class.
        /// </summary>
        public EditableComboBoxDisplay(TypedTextParser textParser)
            : base()
        {
            textBox = new ComboTextBox();
            textBox.BorderStyle = BorderStyle.None;
            textBox.TabStop = false;
            textBox.Enter += new EventHandler(textBox_Enter);
            textBox.EnterPressed += new EventHandler(textBox_EnterPressed);
            textBox.EscapePressed += new EventHandler(textBox_EscapePressed);
            textBox.AltDownOrF4Pressed += new EventHandler(textBox_AltDownOrF4Pressed);
            textBox.Validated += new EventHandler(textBox_Validated);
            textBox.Name = "textBox";

            this.ParseTypedText = textParser;
            this.Controls.Add(textBox);
        }

        /// <summary>
        /// This member overrides the <see cref="ComboBoxDisplayBase.DrawTextArea"/> method.
        /// </summary>
        protected override void DrawTextArea(System.Drawing.Graphics g)
        {
            // As the text box is drawing text, we need not do it
        }

        /// <summary>
        /// This member overrides the <see cref="ComboBoxDisplayBase.UpdateLayout"/> method.
        /// </summary>
        protected override void UpdateLayout(bool textRect)
        {
            base.UpdateLayout(textRect);
            if (textRect)
            {
                textBox.SetBounds((int)TextRectangle.X, (int)TextRectangle.Y, (int)TextRectangle.Width,
                    (int)TextRectangle.Height);
            }
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnTextChanged"/> method.
        /// </summary>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            textBox.Text = this.Text;
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnEnter"/> method.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            textBox.Focus();
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnKeyPress"/> method.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (textBox.Focused != true)
            {
                this.textBox.Text = new string(e.KeyChar, 1);
                this.textBox.Focus();
                this.textBox.SelectionStart = 1;
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.ProcessDialogKey"/> method.
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                if (!textBox.Focused)
                    textBox.Focus();
                this.textBox.SelectionStart = this.textBox.TextLength;
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            this.enterPressedInTextBox = false;
        }

        private void textBox_AltDownOrF4Pressed(object sender, EventArgs e)
        {
            base.RaiseDropDownButtonClickedEvent();
        }

        private void textBox_EscapePressed(object sender, EventArgs e)
        {
            ProcessTypedValue(base.Text, false);
        }

        private void textBox_EnterPressed(object sender, EventArgs e)
        {
            this.enterPressedInTextBox = true;
            ProcessTypedValue(textBox.Text, false);
        }

        private void textBox_Validated(object sender, EventArgs e)
        {
            if (this.enterPressedInTextBox)
                ProcessTypedValue(textBox.Text, true);
        }

        private void ProcessTypedValue(string value, bool validated)
        {
            bool result = ParseTypedText(value);
            if (!result)
                this.textBox.Text = this.Text;
            if (!validated)
                this.Focus();
        }

        #region ComboTextBox class

        private class ComboTextBox : TextBox
        {
            private static readonly object EVENT_ENTERPRESSED = new object();
            private static readonly object EVENT_ESCAPEPRESSED = new object();
            private static readonly object EVENT_ALTDOWNORF4PRESSED = new object();

            public event EventHandler EnterPressed
            {
                add { base.Events.AddHandler(EVENT_ENTERPRESSED, value); }
                remove { base.Events.RemoveHandler(EVENT_ENTERPRESSED, value); }
            }

            public event EventHandler AltDownOrF4Pressed
            {
                add { base.Events.AddHandler(EVENT_ALTDOWNORF4PRESSED, value); }
                remove { base.Events.RemoveHandler(EVENT_ALTDOWNORF4PRESSED, value); }
            }

            public event EventHandler EscapePressed
            {
                add { base.Events.AddHandler(EVENT_ESCAPEPRESSED, value); }
                remove { base.Events.RemoveHandler(EVENT_ESCAPEPRESSED, value); }
            }

            protected override bool ProcessDialogKey(Keys keyData)
            {
                try
                {
                    if (keyData == Keys.Enter)
                    {
                        RaiseEvent(EVENT_ENTERPRESSED);
                        return true;
                    }
                    else if (keyData == Keys.Escape)
                    {
                        RaiseEvent(EVENT_ESCAPEPRESSED);
                        return true;
                    }
                    else if ((keyData == (Keys.Alt | Keys.Down)) || (keyData == Keys.F4))
                    {
                        RaiseEvent(EVENT_ALTDOWNORF4PRESSED);
                        return true;
                    }
                    else
                        return base.ProcessDialogKey(keyData);

                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.ToString());
                    return base.ProcessDialogKey(keyData);
                }
            }

            private void RaiseEvent(object eventKey)
            {
                EventHandler handler = base.Events[eventKey] as EventHandler;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

        }

        #endregion
    }

}
