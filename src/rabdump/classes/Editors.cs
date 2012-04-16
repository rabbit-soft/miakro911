using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace rabdump
{
    internal class DataBaseEditor : UITypeEditor
    {
        private IWindowsFormsEditorService _edSvc;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,IServiceProvider provider,object value)
        {
            ListBox lb = new ListBox();
            lb.Items.Add(DataBase.AllDataBases);
            if (value == DataBase.AllDataBases) 
                lb.SelectedItem = DataBase.AllDataBases;
            foreach (DataBase db in Options.Get().Databases)
            {
                lb.Items.Add(db);
                if (db == value)
                    lb.SelectedItem = db;
            }
            lb.SelectedValueChanged+=lb_SelChanged;
            if (provider != null)
            {
                _edSvc = _edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                _edSvc.DropDownControl(lb);
                value = lb.SelectedItem;
                return value;
            }
            return base.EditValue(context, provider, value);
        }

        public void lb_SelChanged(object sender,EventArgs e)
        {
            _edSvc.CloseDropDown();
        }
    }

}
