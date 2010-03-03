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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pickers
{
    /// <summary>
    /// Provides a simple <see cref="System.Windows.Forms.Form"/> descendant that hosts the drop-down control.
    /// </summary>
    public class DropDownForm : Form
    {
        private bool canceled; // did the user cancel the selection?
        private bool closeDropDownCalled; // was the form closed by calling the CloseDropDown method?

        private Panel containerPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownForm"/> class.
        /// </summary>
        public DropDownForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.containerPanel = new Panel();
            this.containerPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // containerPanel
            //
            this.containerPanel.BorderStyle = BorderStyle.FixedSingle;
            this.containerPanel.Dock = DockStyle.Fill;
            this.containerPanel.Location = new Point(0, 0);
            this.containerPanel.Name = "containerPanel";
            // 
            // DropDownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(containerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DropDownForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DropDownForm";
            this.containerPanel.ResumeLayout(false);
            this.containerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Places the given control in drop-down holder form.
        /// </summary>
        /// <param name="value">The control to place in the form.</param>
        public void SetControl(Control value)
        {
            value.Dock = DockStyle.Fill;
            this.containerPanel.Controls.Add(value);
        }

        /// <summary>
        /// Gets the cancelation status (whether the user canceled the selection or not).
        /// </summary>
        /// <value>A <see cref="System.Boolean"/> value, indicating the cancelation status.</value>
        public bool Canceled
        {
            get
            {
                return canceled;
            }
        }

        /// <summary>
        /// Hides this form, consequently closing the drop-down.
        /// </summary>
        public void CloseDropDown()
        {
            closeDropDownCalled = true;
            Hide();
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnKeyDown"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // If only [ESC] was pressed, it is time to cancel.
            if ((e.Modifiers == 0) && (e.KeyCode == Keys.Escape))
                Hide();
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Form.OnDeactivate"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnDeactivate(EventArgs e)
        {
            // We set the Owner to Nothing BEFORE calling the base class.
            // If we wouldn't do it, the form containing the would lose focus after 
            // the dropdown is closed.
            Owner = null;

            base.OnDeactivate(e);

            // If the form was closed by any other means as the CloseDropDown call, it is because
            // the user clicked outside the form, or pressed the ESC key.
            if (!(closeDropDownCalled))
                canceled = true;
            Hide();
        }

    }
}