using System;

namespace Pickers
{
    /// <summary>
    /// Provides an interface to construct custom drop-down UI for the <see cref="PickerBase&lt;T,U&gt;"/> control.
    /// This version adds ability to sync the picker without closing.
    /// </summary>
    public interface ISyncDropDownUI : IDropDownUI
    {
        /// <summary>
        /// Occurs when the selected value changes in the drop-down UI and it wants the picker control to 
        /// update itself without being closed.
        /// </summary>
        event EventHandler UpdatePicker;
    }
}
