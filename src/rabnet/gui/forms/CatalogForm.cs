using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ColorPickerCombo;

namespace rabnet
{
    public partial class CatalogForm : Form
    {
        public enum CatalogType { NONE, BREEDS, ZONES, DEAD };
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
                case CatalogType.DEAD:
                    Text = "Справочник причин списания";
                    break;
            }
			ds.RowChanged += new DataRowChangeEventHandler(this.OnRowChange);
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
                case CatalogType.DEAD:
                    cd = Engine.db().getDeadReasons().getReasons();
                    break;
            }
			/*
						if (!update)
						{
							for (int i = 0; i < cd.colnames.Length; i++)
							{
								ds.Columns.Add(cd.colnames[i]);
							}
							ds.Columns.Add("id", typeof(int)).ColumnMapping = MappingType.Hidden;
							ds.Columns.Add("cl", typeof(Color));
				
							ColorPickerColumn colColorPick = new ColorPickerColumn();
							colColorPick.Name = "Color";

							dataGridView1.DataSource = ds;
							dataGridView1.Columns.Add(colColorPick);
						}
						for (int i = 0; i < cd.data.Length; i++)
						{
							List<object> objs = new List<object>();
							for (int j = 0; j < cd.colnames.Length; j++)
								objs.Add(cd.data[i].data[j]);
							objs.Add(cd.data[i].key);
							objs.Add(SystemColors.ActiveBorder);
							ds.Rows.Add(objs.ToArray());
						}
			 */

//			ds.Rows.Add
			if (!update)
            {
				DataGridViewTextBoxColumn TextColumn;
				for (int i = 0; i < cd.colnames.Length; i++)
				{
					if (cd.colnames[i].Contains("#color#"))
					{
//						ds.Columns.Add(cd.colnames[i]);
						ColorPickerColumn2 colColorPick = new ColorPickerColumn2();
						colColorPick.Name = cd.colnames[i];
						dataGridView1.Columns.Add(colColorPick);
					}
					else
					{
//						ds.Columns.Add(cd.colnames[i]);
						TextColumn = new DataGridViewTextBoxColumn();
						TextColumn.Name = cd.colnames[i];
						dataGridView1.Columns.Add(TextColumn);
					}
				}
				TextColumn = new DataGridViewTextBoxColumn();
				TextColumn.Name = "id";
				TextColumn.ValueType = typeof(int);
				TextColumn.Visible = false;
				dataGridView1.Columns.Add(TextColumn);
//				ds.Columns.Add("id", typeof(int)).ColumnMapping = MappingType.Hidden;
//				ds.Columns.Add(
				//dataGridView1.DataSource = ds;
			}
			string value;
			Color lclColor=SystemColors.ButtonFace;
			int res;
			for (int i = 0; i < cd.data.Length; i++)
            {
//                List<object> objs = new List<object>();
				int rownumber = dataGridView1.Rows.Add();
				List<object> objs = new List<object>();
				for (int j = 0; j < cd.colnames.Length; j++)
				{
					if (cd.colnames[j].Contains("#color#"))
					{
						value = cd.data[i].data[j];
						if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out res))
						{
							lclColor = Color.FromArgb(res);
						}
						else
						{
							lclColor = Color.FromName(value);
						}
						dataGridView1.Rows[rownumber].Cells[j].Value = lclColor;
//						objs.Add(lclColor);
					}
					else
					{
//						objs.Add(cd.data[i].data[j]);
						dataGridView1.Rows[rownumber].Cells[j].Value = cd.data[i].data[j];
					}
				}
				dataGridView1.Rows[rownumber].Cells[cd.colnames.Length].Value = cd.data[i].key;
//				objs.Add(cd.data[i].key);
//				ds.Rows.Add(objs.ToArray());
//				ds.
//				dataGridView1.Rows[i].Cells[2].Value = lclColor;

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
                        e.Row.ItemArray[2] = Engine.db().getBreeds().AddBreed(e.Row.ItemArray[0] as string,
                            e.Row.ItemArray[1] as string);
                    else
                        Engine.db().getBreeds().ChangeBreed((int)e.Row.ItemArray[2], e.Row.ItemArray[0] as string,
                            e.Row.ItemArray[1] as string);
                    break;
                case CatalogType.ZONES:
                    if (e.Action == DataRowAction.Add)
                        e.Row.ItemArray[2] = Engine.db().getZones().AddZone((int)e.Row.ItemArray[0],
                            e.Row.ItemArray[1] as string, e.Row.ItemArray[2] as string);
                    else
                        Engine.db().getZones().ChangeZone((int)e.Row.ItemArray[3], (string)e.Row.ItemArray[1],(string)e.Row.ItemArray[2]);
                    break;
                case CatalogType.DEAD:
                    if (e.Action == DataRowAction.Add)
                        e.Row.ItemArray[1] = Engine.db().getDeadReasons().AddReason((string)e.Row.ItemArray[0]);
                    else
                        Engine.db().getDeadReasons().ChangeReason((int)e.Row.ItemArray[1],(string)e.Row.ItemArray[0]);
                    break;
            }
        }

        private void OnRowInsert(object sender,DataTableNewRowEventArgs e)
        {
            //e.Row.ItemArray[2]=-1;
		}


		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (!manual)
				return;
			switch (cat)
			{
				case CatalogType.BREEDS:
//					if (e.Action == DataRowAction.Add)
//					else
					if (dataGridView1.Rows[e.RowIndex].Cells[3].Value == null)
					{
						string col0 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
						}
						string col1 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
						{
							col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
						}
						dataGridView1.Rows[e.RowIndex].Cells[3].Value = Engine.db().getBreeds().AddBreed(col0,col1);
					}
					else
					{
						string col0 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
						}
						string col1 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
						{
							col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
						}
						Engine.db().getBreeds().ChangeBreed(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value), col0, col1);
					}
					break;
				case CatalogType.ZONES:
					//					if (e.Action == DataRowAction.Add)
					//						e.Row.ItemArray[2] = Engine.db().getZones().AddZone((int)e.Row.ItemArray[0],
					//							e.Row.ItemArray[1] as string, e.Row.ItemArray[2] as string);
					//					else
					//                        Engine.db().getZones().ChangeZone((int)e.Row.ItemArray[3], (string)e.Row.ItemArray[1],(string)e.Row.ItemArray[2]);
					//                    break;

					// Must be:
					//					if (e.Action == DataRowAction.Add)
					//						e.Row.ItemArray[3] = Engine.db().getZones().AddZone((int)e.Row.ItemArray[0],
					//							e.Row.ItemArray[1] as string, e.Row.ItemArray[2] as string);
					//					else
					//                        Engine.db().getZones().ChangeZone((int)e.Row.ItemArray[3], (string)e.Row.ItemArray[1],(string)e.Row.ItemArray[2]);
					//                    break;

					if (dataGridView1.Rows[e.RowIndex].Cells[3].Value == null)
					{
						int col0 = 0;
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
						}
						string col1 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
						{
							col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
						}
						string col2 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
						{
							col2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
						}
						dataGridView1.Rows[e.RowIndex].Cells[3].Value = Engine.db().getZones().AddZone(col0, col1, col2);
					}
					else
					{
						dataGridView1.Rows[e.RowIndex].Cells[3].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
						int col0 = 0;
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
						}
						string col1 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
						{
							col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
						}
						string col2 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
						{
							col2 = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
						}
						Engine.db().getZones().ChangeZone(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value), col1,col2);
					}
					break;
				case CatalogType.DEAD:
//					if (e.Action == DataRowAction.Add)
//						e.Row.ItemArray[1] = Engine.db().getDeadReasons().AddReason((string)e.Row.ItemArray[0]);
//					else
					if (dataGridView1.Rows[e.RowIndex].Cells[1].Value == null)
					{
						string col0 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
						}
						dataGridView1.Rows[e.RowIndex].Cells[1].Value = Engine.db().getDeadReasons().AddReason(col0);
					}
					else
					{
						string col0 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
						}
						Engine.db().getDeadReasons().ChangeReason(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value), col0);
					}
					break;
			}
		}

		private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
		{

		}


    }
}
