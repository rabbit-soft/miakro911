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
        public enum CatalogType { NONE, BREEDS, ZONES };
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
                case CatalogType.ZONES:
                    Text = "Справочник Зон Прибытия";
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
                case CatalogType.ZONES:
                    cd = Engine.db().getZones().getZones();
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
                List<object> objs = new List<object>();
                for (int j = 0; j < cd.colnames.Length; j++)
                    objs.Add(cd.data[i].data[j]);
                objs.Add(cd.data[i].key);
                ds.Rows.Add(objs.ToArray());
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
                case CatalogType.ZONES:
                    if (e.Action == DataRowAction.Add)
                    {
                        e.Row.ItemArray[2] = Engine.db().getZones().AddZone((int)e.Row.ItemArray[0],
                            e.Row.ItemArray[1] as string, e.Row.ItemArray[2] as string);
                    }
                    else
                        Engine.db().getZones().ChangeZone((int)e.Row.ItemArray[3], e.Row.ItemArray[1] as string,
                            e.Row.ItemArray[2] as string);
                    break;
            }
        }

        private void OnRowInsert(object sender,DataTableNewRowEventArgs e)
        {
            e.Row.ItemArray[2]=-1;
        }

    }
}
