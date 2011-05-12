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
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Pickers
{
    /// <summary>
    /// Implements the basic functionality required by a picker control, that uses type editors
    /// for display drop-down UI.
    /// </summary>
    /// <typeparam name="T">The type of the value of the picker.</typeparam>
    /// <typeparam name="E">The type of the UITypeEditor used by the picker.</typeparam>
    /// <include file='DocContent.xml' path='docContent/member[@name="SvcPickerBase"]/*'/>
    public abstract class SvcPickerBase<T, E> : PickerBase<T, SvcPickerBaseUI> where E : UITypeEditor, new()
    {
        // Instance of the service that provides a drop-down holder for the picker.
        private PickerEditorService<T, E> valueEditorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvcPickerBase&lt;T,E&gt;"/> class.
        /// </summary>
        public SvcPickerBase() : base()
        {
            valueEditorService = new PickerEditorService<T, E>(this);
        }

        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.HasIcon"/> property.
        /// </summary>
        protected override bool HasIcon
        {
            get
            {
                E editor = new E();
                bool result = editor.GetPaintValueSupported();
                editor = null;
                return result;
            }
        }

        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.ShowDropDown"/> method.
        /// </summary>
        protected override void ShowDropDown()
        {
            try
            {
                // create the UITypeEditor instance to display ui
                E editor = new E();

                // Display the UI.
                T oldValue = Value;
                object newValue = editor.EditValue(valueEditorService, oldValue);

                // If the user didn't cancel the selection, remember the new color.
                if ((newValue != null) && !(valueEditorService.Canceled))
                    Value = (T)newValue;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Error.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// This member overrides the <see cref="PickerBase&lt;T,U&gt;.DrawIcon"/> method.
        /// </summary>
        protected override void DrawIcon(Graphics g, Rectangle iconRectangle)
        {
            E editor = new E();
            if (editor.GetPaintValueSupported())
            {
                editor.PaintValue(Value, g, iconRectangle);
                g.DrawRectangle(Pens.Black, iconRectangle);
            }
            editor = null;
        }

        #region PickerEditorService class

        /* Implements the IWindowsFormsEditor interface to emulate the PropertyGrid-style behavior for displaying 
         * drop-down UI, as provided by the UITypeEdior our picker will use.
         * This class is the real 'meat' of SvcPickerBase. It does the main work of emulation of PropertyGrid by:
         * 1. Providing an implementation of IWindowsFormsEditorService
         *      The UITypeEditor uses this class to display its UI on to the dropdown.
         * 2. Providing an implementaion of the IServiceProvider
         *      The UITypeEditor uses this to get an instance of IWindowsFormsEditorSerivce
         */
        private class PickerEditorService<ST, SE> : IWindowsFormsEditorService, IServiceProvider where SE : UITypeEditor, new()
        {
            // the associated picker control
            private SvcPickerBase<ST, SE> ownerPicker;

            // this field saves the ownerPicker.DropDownHolder.Canceled flag, so that the ownerPicker can retrive it
            private bool canceled;

            public PickerEditorService(SvcPickerBase<ST, SE> owner)
            {
                this.ownerPicker = owner;
            }

            // Indicates whether the user canceled the selection or not.
            public bool Canceled
            {
                get { return canceled; }
            }

            /* When the user will cancel the selection by clicking outside the drop-down, or by pressing the 
		     * escape key, the drop-down UI, provided by the type editor using this service, will call this 
		     * method.
            */
            void IWindowsFormsEditorService.CloseDropDown()
            {
                // As our DropDownForm class contains a method for closing the drop-down, we just call it.
                if (ownerPicker.DropDownHolder != null)
                    ownerPicker.DropDownHolder.CloseDropDown();
            }

            /* When the picker, which owns this service, calls the EditValue method of the a type editor,
             * this method will be called. So this method initializes a Form instance for displaying the 
             * UI control given to us. It shows the form at correct position on the screen.
             */
            void IWindowsFormsEditorService.DropDownControl(Control control)
            {
                // give the ownerPicker the reference to the control for the drop-down UI and
                // tell it to display in a dropDownHolder
                canceled = ownerPicker.DoDropDown(control);
            }

            DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
            {
                // We dont show a dialog for editing the value, instead we show a dropdown! So, we don't need
                // to do work in this method; we just throw an exception saying "not supported".
                // Note: It is the responsibilty of the owner picker control to pass this service to the EditValue
                // method of such a type editor that only shows drop-down UI.
                // Otherwise, this exception will be thrown.
                throw new NotSupportedException("Dialogs are not supported by drop-down pickers.");
            }

            // The UITypeEdior will use this method get an instance of our class.
            object IServiceProvider.GetService(Type serviceType)
            {
                // If the type of service asked for is 'IWindowsFormsEditorService', return it.
                if (serviceType.Equals(typeof(IWindowsFormsEditorService)))
                    return this;
                // We don't have any other service, so we return a null in other cases.
                else
                    return null;
            }
        }

        #endregion

    }
}