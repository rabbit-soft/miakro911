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
using System.Windows.Forms.VisualStyles;
using Pickers.ComboBoxAppearance;

namespace Pickers
{
	/// <summary>
	/// Implements the basic functionality required by a picker control.
    /// </summary>
    /// <typeparam name="T">The type of the value of the picker.</typeparam>
    /// <typeparam name="U">The <see cref="System.Windows.Forms.Control"/> that provides the drop-down UI for 
    /// this picker.</typeparam>
	/// <include file='DocContent.xml' path='docContent/member[@name="PickerBase"]/*'/>
    [DefaultProperty("Value"), DefaultEvent("ValueChanged")]
    public abstract class PickerBase<T, U> : Control, IPickerControl where U : Control, IDropDownUI, new()
    {

        #region Fields

        private T _value;
        private IPickerDisplayAdapter displayAdapter;
        private Color backColor = SystemColors.Window;

        private bool isValueSet;

        /// <summary>
        /// The default width of the control in pixels.
        /// </summary>
        protected const int DEFAULT_WIDTH = 121;
        private const string DEFAULT_BACKCOLOR_NAME = "Window";
        
        /// <summary>
        /// The instance of <see cref="DropDownForm"/>, which displays the drop-down UI.
        /// </summary>
        protected DropDownForm DropDownHolder;

        /// <summary>
        /// Occurs when the <see cref="PickerBase&lt;T,U&gt;.Value"/> property is changed.
        /// </summary>
        /// <include file='DocContent.xml' path='docContent/member[@name="PickerBase.ValueChanged"]/*'/>
        public event EventHandler ValueChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerBase&lt;T,U&gt;"/> class.
        /// </summary>
        public PickerBase()
            : base()
        {
            // PickerBase
            base.BackColor = Control.DefaultBackColor;
            this.ForeColor = SystemColors.WindowText;
            this.ResizeRedraw = true;
            this.Appearance = PickerAppearance.ComboBox;
            this.SetValueCore(this.GetDefaultValue());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value assigned to the picker control.
        /// </summary>
        /// <value>
        /// The value assigned to the control.
        /// </value>
        [Category("Behavior"),
        Description("The current value of this picker control.")]
        public T Value
        {
            get { return _value; }
            set
            {
                if (!(_value.Equals(value)))
                    SetValueCore(value);
            }
        }

        /// <summary>
        /// Gets or sets the appearance of this picker control.
        /// </summary>
        /// <value>A <see cref="PickerAppearance"/> value specifing the appearance of this picker control.</value>
        [Category("Appearance"),
        DefaultValue(PickerAppearance.ComboBox),
        Description("The appearance of the this picker control.")]
        public PickerAppearance Appearance
        {
            get
            {
                if (this.displayAdapter != null)
                {
                    Type dcType = this.displayAdapter.DisplayControl.GetType();
                    if (dcType == typeof(ReadonlyComboBoxDisplay))
                        return PickerAppearance.ComboBox;
                    else if (dcType == typeof(EditableComboBoxDisplay))
                        return PickerAppearance.EditableComboBox;
                    else if (dcType == typeof(CheckBox))
                        return PickerAppearance.CheckButton;
                    else
                        return PickerAppearance.Custom;
                }
                else
                    return PickerAppearance.Custom;
            }
            set
            {
                if (value == this.Appearance)
                    return;

                if (value == PickerAppearance.ComboBox)
                    this.DisplayAdapter = new ComboBoxDisplayAdapter(this.HasIcon, 20);
                else if (value == PickerAppearance.EditableComboBox)
                    this.DisplayAdapter = new ComboBoxDisplayAdapter(this.HasIcon, 20, this.ParseTypedText);
                else if (value == PickerAppearance.CheckButton)
                    this.DisplayAdapter = new CheckButtonDisplayAdapter();
                else
                    throw new InvalidOperationException("To set custom appearance, set the DisplayAdapter property at runtime.");
            }
        }

        /// <summary>
        /// Gets or sets the display adapter the renders the appearance of this picker control.
        /// </summary>
        /// <value>A <see cref="IPickerDisplayAdapter"/> object the renders this picker control.</value>
        [Browsable(false),
        Category("Behavior"),
        Description("The the adapter that controls the appearance of the this picker control.")]
        public virtual IPickerDisplayAdapter DisplayAdapter
        {
            get { return this.displayAdapter; }
            set
            {
                this.displayAdapter = value;
                this.Controls.Clear();
                if (this.displayAdapter.GetType() != typeof(CheckButtonDisplayAdapter))
                {
                    this.displayAdapter.DisplayControl.BackColor = this.backColor;
                    this.displayAdapter.DisplayControl.ForeColor = this.ForeColor;
                }
                this.displayAdapter.DisplayControl.Font = this.Font;
                this.displayAdapter.DisplayControl.Dock = DockStyle.Fill;
                this.displayAdapter.DisplayControl.Text = this.Text;
                this.displayAdapter.DropDown += new EventHandler(displayAdapter_DropDown);
                if (this.HasIcon)
                {
                    this.displayAdapter.DrawIcon += new EventHandler<DrawIconEventArgs>(displayAdapter_DrawIcon);
                }
                this.Controls.Add(this.displayAdapter.DisplayControl);
            }
        }


        /// <summary>
        /// Gets the preferred height of the picker control.
        /// </summary>
        /// <value>The preferred height of the control in pixels.</value>
        /// <include file='DocContent.xml' path='docContent/member[@name="PickerBase.PreferredHeight"]/*'/>
        [Browsable(false)]
        public int PreferredHeight
        {
            get
            {
                int r = (int)this.Font.GetHeight();
                // add extra space ( for border )
                r += 8;
                return r;
            }
        }

        /// <summary>
        /// This member overrides the <see cref="Control.BackColor"/> property.
        /// </summary>
        [DefaultValue(typeof(Color), DEFAULT_BACKCOLOR_NAME)]
        public override Color BackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
                if (this.displayAdapter.DisplayControl != null)
                    this.displayAdapter.DisplayControl.BackColor = value;
            }
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.Text"/> property.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Advanced)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.DefaultSize"/> property.
        /// </summary>
        protected override System.Drawing.Size DefaultSize
        {
            get { return new System.Drawing.Size(DEFAULT_WIDTH, PreferredHeight); }
        }

        /// <summary>
        /// Gets or sets the correction factor to be added to the height of the drop-down displayed.
        /// </summary>
        protected virtual int HeightCorrection
        {
            get { return 0; }
        }

        /// <summary>
        /// Indicates whether this picker has a icon or not.
        /// </summary>
        /// <value><c>true</c> if the picker has an icon; otherwise <c>false</c>.</value>
        protected virtual bool HasIcon
        {
            get { return false; }
        }

        #endregion

        #region Event-handling Methods

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnSizeChanged"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {

            // if height is changed, reset it
            if (Height != PreferredHeight)
                Height = PreferredHeight;

            // Calling the base class OnSizeChanged
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control.OnFontChanged"/> method.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            // font changed, so update the height
            Height = PreferredHeight;
            this.displayAdapter.DisplayControl.Font = this.Font;

            // Calling the base class OnFontChanged
            base.OnFontChanged(e);
        }

        /// <summary>
        /// This member overrides the <see cref="System.Windows.Forms.Control"/> method.
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.displayAdapter.DisplayControl.Focus();
        }

        #endregion

        #region Methods for Managing the Selected Value

        /// <summary>
        /// Sets the given value to the value of this control.
        /// </summary>
        /// <param name="value">The value to set.</param>
        protected virtual void SetValueCore(T value)
        {
            _value = value;
            if (_value.Equals(this.GetDefaultValue()))
                isValueSet = false;
            else
                isValueSet = true;
            this.Text = this.FormatValue(_value);
            this.displayAdapter.DisplayControl.Text = this.Text;
            this.displayAdapter.InvalidateDisplay();
            OnValueChanged(EventArgs.Empty);
        }

        /// <summary>
        /// Provides a "dynamic" default value for the picker control.
        /// </summary>
        /// <returns>The default value of this control.</returns>
        protected abstract T GetDefaultValue();

        /// <summary>
        /// Raises the <see cref="PickerBase&lt;T,U&gt;.ValueChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        /// <include file='DocContent.xml' path='docContent/member[@name="PickerBase.OnValueChanged"]/*'/>
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        /// <summary>
        /// Parses the given string and returns the coressponding value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">The <see cref="System.String"/> value to parse.</param>
        /// <returns>The resulting value (of type <typeparamref name="T"/>) after parsing the string
        /// <paramref name="s"/>.</returns>
        protected virtual T ValueFromString(string s)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            T result = (T)converter.ConvertFromString(s);
            return result;
        }
                
        /// <summary>
        /// Provides a text representation of the given value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted text representation of the given value.</returns>
        protected virtual string FormatValue(T value)
        {
            return value.ToString();
        }

        private bool ParseTypedText(string text)
        {
            T newValue;
            try
            {
                newValue = this.ValueFromString(text);
            }
            catch (Exception e)
            {
                System.Media.SystemSounds.Beep.Play();
                Console.Error.WriteLine(e.ToString());
                return false;
            }
            this.Value = newValue;
            return true;
        }

        private bool ShouldSerializeValue()
        {
            return this.isValueSet;
        }
        
        private void ResetValue()
        {
            this._value = this.GetDefaultValue();
        }

        private bool ShouldSerializeText()
        {
            return false;
        }

        /// <summary>
        /// Resets the <see cref="PickerBase&lt;T,U&gt;.Text"/> property to its default value.
        /// </summary>
        public override void ResetText()
        {
            this.Text = this.FormatValue(this.GetDefaultValue());
        }

        #endregion

        #region Drawing the Icon

        private void displayAdapter_DrawIcon(object sender, DrawIconEventArgs e)
        {
            this.DrawIcon(e.Graphics, e.IconRectangle);
        }

        /// <summary>
        /// Draws the icon associated with value of this picker control.
        /// </summary>
        /// <param name="g">The <see cref="System.Drawing.Graphics"/> surface to draw icon on.</param>
        /// <param name="iconRectangle">The <see cref="System.Drawing.Rectangle"/> to draw icon in.</param>
        protected virtual void DrawIcon(Graphics g, Rectangle iconRectangle)
        {
            // PickerBase does not draw icons
            // so nothing to do here
            // but if the inheritor need to draw icon, it MUST:
            // 1. override HasIcon property and return true
            // 2. override this method and draw the icon on 'g' in 'iconRectangle' only
        }

        #endregion

        #region Methods for Displaying Drop-down

        private void displayAdapter_DropDown(object sender, EventArgs e)
        {
            // displayAdapter tells us that drop-down is needed
            // so we display it
            ShowDropDown();
        }

        /// <summary>
        /// Displays the Drop Down UI used by this picker control and enables the user to edit the value.
        /// </summary>
        /// <include file='DocContent.xml' path='docContent/member[@name="PickerBase.ShowDropDown"]/*'/>
        protected virtual void ShowDropDown()
        {
            try
            {
                // create and init the control...
                U control = new U();
                control.SetSelectedValue(Value);
                this.InitializeDropDownControl(control);
                AddUpdateHandler(control);
                
                // Display the UI
                bool canceled = DoDropDown(control);

                // If the user didn't cancel the selection, remember the new color.
                object newValue = control.GetSelectedValue();
                if ((newValue != null) && !(canceled))
                    this.Value = (T)newValue;

                // dispose off the control
                control.Dispose();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }

        private void AddUpdateHandler(U control)
        {
            if (control is ISyncDropDownUI)
            {
                ISyncDropDownUI syncControl = (ISyncDropDownUI)control;
                syncControl.UpdatePicker += new EventHandler(DropDownControl_UpdatePicker);
            }
        }

        private void DropDownControl_UpdatePicker(object sender, EventArgs e)
        {
            U control = (U)sender;
            object newValue = control.GetSelectedValue();
            this.Value = (T)newValue;
        }

        /// <summary>
        /// Initializes the drop-down control before it gets displayed to the user in the
        /// <see cref="PickerBase&lt;T,U&gt;.ShowDropDown"/> method.
        /// </summary>
        /// <param name="control">The drop-down control to initialize.</param>
        /// <include file='DocContent.xml' path='docContent/member[@name="PickerBase.InitializeDropDownControl"]/*'/>
        protected virtual void InitializeDropDownControl(U control)
        {
            // inheritors may do some initalization
        }

        /// <summary>
        /// Performs actual task of placing drop-down UI on the <see cref="PickerBase&lt;T,U&gt;.DropDownHolder"/> and
        /// displaying it at a proper position on screen.
        /// </summary>
        /// <param name="dropDownControl">The drop-down UI control.</param>
        /// <returns><c>true</c> if the drop-down was canceled; otherwise <c>false</c>.</returns>
        protected bool DoDropDown(Control dropDownControl)
        {
            bool result = false;

            // Initialize the hosting form for the control.
            DropDownHolder = new DropDownForm();
            DropDownHolder.Font = this.Font;
            DropDownHolder.Width = (this.Width < dropDownControl.Width) ? dropDownControl.Width : this.Width;
            DropDownHolder.Height = dropDownControl.Height + this.HeightCorrection;
            //DropDownHolder.Bounds = dropDownControl.Bounds;
            DropDownHolder.SetControl(dropDownControl);

            // Lookup a parent form for the Picker control and make the dropdown form to be owned
            // by it. This prevents to show the dropdown form icon when the user presses the Alt+Tab system 
            // key while the dropdown form is displayed.
            Control pickerForm = GetParentForm(this);
            if (pickerForm != null)
                DropDownHolder.Owner = (Form)pickerForm;
            
            // Ensure the whole drop-down UI is displayed on the screen and show it.
            PositionDropDownHolder();
            if (dropDownControl.GetType() == typeof(U))
                ((IDropDownUI)dropDownControl).CloseDropDownHolder += new EventHandler(dropDownControl_CloseDropDownHolder);
            DropDownHolder.Show(); // ShowDialog would disable clicking outside the dropdown area!

            // Wait for the user to select a new value (in which case the drop-down UI calls our CloseDropDown
            // method) or cancel the selection (no CloseDropDown called, the Cancel flag is set to True).
            DoModalLoop();

            this.displayAdapter.Adjust4CloseDropDown();
            // Remember the cancel flag and get rid of the drop down form.
            result = DropDownHolder.Canceled;
            DropDownHolder.Dispose();
            DropDownHolder = null;

            // return whether if the form was canceled or not
            return result;
        }

        private void dropDownControl_CloseDropDownHolder(object sender, EventArgs e)
        {
            // when dropDownControl wants to end the drop-down, it raises the CloseDropDownHolder event
            // it is handled here and we ask the holder to close
            DropDownHolder.CloseDropDown();
        }

        // Keeps the drop-down on screen, untill the user makes a seletion or cancels the drop-down.
        private void DoModalLoop()
        {
            // When the user makes a seletion (the CloseDropDown method will be called) or cancels the 
            // drop-down, the Visible property of dropDownHolder will become false. But untill it is true, 
            // we have to keep it on screen by using a loop.
            while (DropDownHolder.Visible)
            {
                // process the messages, so the system doesn't freeze.
                Application.DoEvents();
                // The following is an undocumented User32 call. For more information see 
                // Palo Mraz's article at http://www.codeproject.com/vb/net/a_colorpicker.asp.
                NativeMethods.MsgWaitForMultipleObjects(1, IntPtr.Zero, 1, 5, 255);
            }
        }

        // Positions the drop-down holder form at the correct place.
        private void PositionDropDownHolder()
        {
            // convert picker location to screen coordinates.
            Point loc = this.Parent.PointToScreen(this.Location);
            Screen currentScreen = Screen.FromPoint(loc);
            Rectangle screenRect = currentScreen.WorkingArea;

            // Position the dropdown X coordinate in order to be displayed in its entirely.
            if (loc.X < screenRect.X)
                loc.X = screenRect.X;
            else if ((loc.X + DropDownHolder.Width) > screenRect.Right)
                loc.X = screenRect.Right - DropDownHolder.Width;

            // Do the same for the Y coordinate.
            if ((loc.Y + this.Height + DropDownHolder.Height) > screenRect.Bottom)
                loc.Offset(0, -DropDownHolder.Height);  // dropdown will be above the picker control
            else
                loc.Offset(0, this.Height); // dropdown will be below the picker

            DropDownHolder.Location = loc;
        }

        // Finds the form containing the given control.
        // ctl: The control whose parent is to be found.
        // Returns: The parent form containing the control.
        private Control GetParentForm(Control ctl)
        {
            // Using an infinite loop, we can keep looking until a parent form is found.
            // When a parent for is fount, return causes am 'emergency exit'.
            while (true)
            {
                // If control has no parent, it is the form we are searching for.
                if (ctl.Parent == null)
                    return ctl;
                // Parent form not found - proceed to the parent of the current control
                else
                    ctl = ctl.Parent;
            }
        }

        #endregion
    }
}
