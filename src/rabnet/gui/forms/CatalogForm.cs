using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class CatalogForm : Form
    {
        public enum CatalogType { NONE, BREEDS };
        private DataTable ds=new DataTable();
        private CatalogType cat = CatalogType.NONE;
        private bool manual = true;
        public CatalogForm()
        {
            InitializeComponent();
        }

        public CatalogForm(CatalogType type):this()
        {
            cat = type;
            switch (cat)
            {
                case CatalogType.BREEDS:
                    Text = "Справочник Пород";
                    break;
            }
            ds.RowChanged+=new DataRowChangeEventHandler(this.OnRowChange);
            ds.TableNewRow+=new DataTableNewRowEventHandler(this.OnRowInsert);
            FillTable(false);
        }


        public void FillTable(bool update)
        {
            manual = false;
            ds.Clear();
            CatalogData cd = null;
            switch (cat)
            {
                case CatalogType.BREEDS:
                    cd = Engine.db().getBreeds().getBreeds();
                    break;
            }
            if (!update)
            {
                for (int i = 0; i < cd.colnames.Length; i++)
                    ds.Columns.Add(cd.colnames[i]);
                ds.Columns.Add("id", typeof(int)).ColumnMapping = MappingType.Hidden;
                dataGridView1.DataSource = ds;
            }
            for (int i = 0; i < cd.data.Length; i++)
            {
                DataRow rw=ds.Rows.Add(cd.data[i].data[0],cd.data[i].data[1],cd.data[i].key);
            }
            manual = true;
        }

        private void OnRowChange(object sender, DataRowChangeEventArgs e)
        {
            if (!manual)
                return;
            switch (cat)
            {
                case CatalogType.BREEDS:
                    if (e.Action==DataRowAction.Add)
                    {
                        e.Row.ItemArray[2] = Engine.db().getBreeds().AddBreed(e.Row.ItemArray[0] as string,
                            e.Row.ItemArray[1] as string);
                    }
                    else
                        Engine.db().getBreeds().ChangeBreed((int)e.Row.ItemArray[2], e.Row.ItemArray[0] as string,
                            e.Row.ItemArray[1] as string);
                    break;
            }
        }

        private void OnRowInsert(object sender,DataTableNewRowEventArgs e)
        {
            e.Row.ItemArray[2]=-1;
        }

    }
}
