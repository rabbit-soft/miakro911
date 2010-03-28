using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;

namespace rabdump
{
    internal class DataBaseEditor : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,IServiceProvider provider,object value)
        {
            ListBox lb = new ListBox();
            lb.Items.Add(DataBase.AllDataBases);
            if (value == DataBase.AllDataBases) lb.SelectedItem = DataBase.AllDataBases;
            foreach (DataBase db in Options.get().Databases)
            {
                lb.Items.Add(db);
                if (db == value)
                    lb.SelectedItem = db;
            }
            lb.SelectedValueChanged+=new EventHandler(this.lb_SelChanged);
            if (provider != null)
            {
                edSvc = edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                edSvc.DropDownControl(lb);
                value = lb.SelectedItem;
                return value;
            }
            return base.EditValue(context, provider, value);
        }

        public void lb_SelChanged(object sender,EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }

}
