using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using ColorPickerCombo;
using System.IO;

namespace rabnet
{
	[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
	public partial class CatalogForm : Form
    {
        private class myCell
        {
            public int Coll;
            public int Row;
            public string Text;

            public myCell(int coll, int row, string text)
            {
                this.Coll = coll;
                this.Row = row;
                this.Text = text;
            }
        }

        /// <summary>
        /// Типы справочников
        /// </summary>
        public enum CatalogType { NONE, BREEDS, ZONES, DEAD, PRODUCTS };
        private DataTable ds = new DataTable();
        /// <summary>
        /// Тип показываемого справочника
        /// </summary>
        private CatalogType catType = CatalogType.NONE;
        /// <summary>
        /// Вносятся ли изменения пользователем
        /// </summary>
        private bool manual = true;
        private int _hiddenId = 0;
        private myCell _lastCell = null;

        public CatalogForm()
        {
            InitializeComponent();
            this.manual = false;
        }

        public CatalogForm(CatalogType type):this()
        {
            catType = type;
            switch (catType)
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
                case CatalogType.PRODUCTS:
                    this.Text = "Справочник видов продукции";
                    break;
            }
			ds.RowChanged += new DataRowChangeEventHandler(this.OnRowChange);
            ds.TableNewRow += new DataTableNewRowEventHandler(this.OnRowInsert);
            FillTable(false);
        }

        /// <summary>
        /// Заполнение каталога данными
        /// </summary>
        /// <param name="update"></param>
        public void FillTable(bool update)
        {
            btDeleteImage.Visible = btNewImage.Visible = false;
            manual = false;
            ds.Clear();
            CatalogData cd = null;
            switch (catType)
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
                case CatalogType.PRODUCTS:
                    cd = Engine.db().getProductTypes().getProducts();
                    btDeleteImage.Visible = btNewImage.Visible = true;
                    btDeleteImage.Enabled = btNewImage.Enabled = false;
                    break;
            }
            int colorExist=-1,imageExist = -1; //чтобы удалить слово #color#
            if (!update)//установка колонок
            {
				DataGridViewTextBoxColumn TextColumn;
				for (int i = 0; i < cd.colnames.Length; i++)
				{
                    if (cd.colnames[i].Contains("#color#"))//в породах
					{
                        colorExist = i;
						ColorPickerColumn colColorPick = new ColorPickerColumn();
						colColorPick.Name = cd.colnames[i];
						dataGridView1.Columns.Add(colColorPick);
					}
                    else if (cd.colnames[i].Contains("#image#"))//в продукции
                    {
                        imageExist = i;
                        var colImage = new DataGridViewImageColumn();
                        colImage.Name = cd.colnames[i];
                        dataGridView1.Columns.Add(colImage);
                    }
                    else
					{
						TextColumn = new DataGridViewTextBoxColumn();
						TextColumn.Name = cd.colnames[i];
						dataGridView1.Columns.Add(TextColumn);
					}
				}              
                /*
                 * Далее добавляется одна невидимая ячейка,
                 * в коротой содержится ID записи
                 */
				TextColumn = new DataGridViewTextBoxColumn();
				TextColumn.Name = "id";
				TextColumn.ValueType = typeof(int);
				TextColumn.Visible = false;
                TextColumn.Width = 0;
				dataGridView1.Columns.Add(TextColumn);
                _hiddenId = dataGridView1.Columns.Count - 1;
                dataGridView1.Columns[_hiddenId - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			}

			string value;
            Color lclColor = SystemColors.ButtonFace;
			int res;
			for (int i = 0; i < cd.data.Length; i++)
            {
				int rownumber = dataGridView1.Rows.Add();
				//List<object> objs = new List<object>();
                for (int j = 0; j < cd.colnames.Length; j++)    //заполнение столбцов
				{
					if (cd.colnames[j].Contains("#color#"))
					{
						value = cd.data[i].data[j];
						if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out res))
						{
							lclColor = Color.FromArgb(res);
						}
						else lclColor = Color.FromName(value);						
						dataGridView1.Rows[rownumber].Cells[j].Value = lclColor;
					}
                    else if (cd.colnames[j].Contains("#image#"))
                    {
                        if (cd.data[i].image.Length == 0) continue;
                        var ms = new MemoryStream(cd.data[i].image);
                        Image img = Image.FromStream(ms);
                        dataGridView1.Rows[rownumber].Cells[j].Value = img;
                        dataGridView1.Rows[rownumber].Height = img.Height;
                    }
					else dataGridView1.Rows[rownumber].Cells[j].Value = cd.data[i].data[j];
				}
				dataGridView1.Rows[rownumber].Cells[cd.colnames.Length].Value = cd.data[i].key;

			}
            if (catType == CatalogType.DEAD) dataGridView1.Rows[0].ReadOnly = true;
            if (colorExist != -1)
                dataGridView1.Columns[colorExist].Name = dataGridView1.Columns[colorExist].Name.Remove(0, "#color#".Length);
            if (imageExist != -1)
                dataGridView1.Columns[imageExist].Name = dataGridView1.Columns[imageExist].Name.Remove(0, "#image#".Length);
            manual = true;
        }

        private void OnRowChange(object sender, DataRowChangeEventArgs e)
        {

        }

        private void OnRowInsert(object sender,DataTableNewRowEventArgs e)
        {
            //e.Row.ItemArray[2]=-1;
		}


		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (!manual) return;
            if (_lastCell != null)
            {
                if (dataGridView1.SelectedCells[0].Value == null && e.ColumnIndex == _lastCell.Coll && e.RowIndex == _lastCell.Row)
                {
                    dataGridView1.SelectedCells[0].Value = _lastCell.Text;
                    return;
                }
            }

            /*
             * Если последняя невидимая ячейка не содержит информации(ID записи),
             * значит нужно добавить новую запись.
             * Иначе изменить существующую
             */
			switch (catType)
			{
				case CatalogType.BREEDS:
                    if (dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value == null)
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
						string col2 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
						{
							col2 = ((Color)(dataGridView1.Rows[e.RowIndex].Cells[2].Value)).Name;
						}
                        dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value = Engine.db().getBreeds().AddBreed(col0, col1, col2);
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
						string col2 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[2].Value != null)
						{
							col2 = ((Color)(dataGridView1.Rows[e.RowIndex].Cells[2].Value)).Name;
						}
						Engine.db().getBreeds().ChangeBreed(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value), col0, col1, col2);
                        dataGridView1.Refresh();
					}
					break;
                ///////////////////////
				case CatalogType.ZONES:
                    if (dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value == null)
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
                        dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value = Engine.db().getZones().AddZone(col0, col1, col2);
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
                /////////////////////////
				case CatalogType.DEAD:
                    if (dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value == null)
					{
						string col0 = "";
						if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
						{
							col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
						}
                        dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value = Engine.db().getDeadReasons().AddReason(col0);
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
                /////////////////////////
                case CatalogType.PRODUCTS:
                    if (dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value == null)
                    {
                        string col0 = "";
                        if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
                        {
                            col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            if (col0.Length > 20)
                            {
                                col0 = col0.Substring(0, 20);
                                dataGridView1.Rows[e.RowIndex].Cells[0].Value = col0;
                            }
                        }

                        string col1 = "";
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                        {
                            col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                            if (col1.Length > 10)
                            {
                                col1 = col1.Substring(0, 10);
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = col1;
                            }
                        }

                        var col2 = new byte[0];
                        Image img = (dataGridView1.Rows[e.RowIndex].Cells[2].Value as Image);
                        if (img != null)
                        {
                            var ms = new MemoryStream();
                            img.Save(ms, ImageFormat.Jpeg);
                            col2 = ms.ToArray();
                            if (col2.Length == 784)
                                col2 = new byte[0];
                        }
                        dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value = Engine.db().getProductTypes().AddProduct(col0, col1,col2);
                    }
                    else
                    {
                        string col0 = "";
                        if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
                        {
                            col0 = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                            if (col0.Length > 20)
                            {
                                col0 = col0.Substring(0, 20);
                                dataGridView1.Rows[e.RowIndex].Cells[0].Value = col0;
                            }
                        }

                        string col1 = "";
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value != null)
                        {
                            col1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                            if (col1.Length > 10)
                            {
                                col1 = col1.Substring(0, 10);
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = col1;
                            }
                        }

                        var col2 = new byte[0];
                        Image img = (dataGridView1.Rows[e.RowIndex].Cells[2].Value as Image);
                        if (img != null)
                        {
                            var ms = new MemoryStream();
                            img.Save(ms, ImageFormat.Jpeg);
                            col2 = ms.ToArray();
                            if (col2.Length == 784)
                                col2 = new byte[0];
                        }

                        Engine.db().getProductTypes().ChangeProduct(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[_hiddenId].Value), col0, col1,col2);
                    }
                    break;
			}
		}

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (catType != CatalogType.PRODUCTS) return;
            if (dataGridView1.SelectedCells.Count != 0 && dataGridView1.SelectedCells[0].ColumnIndex == 2)
            {
                btNewImage.Enabled = true;
                if (dataGridView1.SelectedCells[0].Value != null)
                    btDeleteImage.Enabled = true;
            }
            else
            {
                btNewImage.Enabled = btDeleteImage.Enabled = false;   
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0 && dataGridView1.SelectedCells[0].ColumnIndex == 2)
                btNewImage.PerformClick();
        }

        private void btNewImage_Click(object sender, EventArgs e)
        {
            const int mustH = 200;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var img = Image.FromFile(openFileDialog1.FileName);
                //double ratio = (double)img.Width / (double)img.Height;
                //var bmp = new Bitmap(img, (int)(mustH * ratio), mustH);

                var bmp = new Bitmap(300, 200);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img, new Rectangle(0, 0, 300, 200), new Rectangle(0, 0, img.Width, img.Height),GraphicsUnit.Pixel);

                dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Height = mustH;
                dataGridView1.SelectedCells[0].Value = bmp;
                dataGridView1_CellEndEdit(dataGridView1, new DataGridViewCellEventArgs(dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex));
            }
        }

        private void btDeleteImage_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0 && dataGridView1.SelectedCells[0].ColumnIndex == 2)
                dataGridView1.SelectedCells[0].Value = null;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!manual || dataGridView1.SelectedCells.Count==0) return;
            if ((catType == CatalogType.PRODUCTS && e.ColumnIndex == 3) || dataGridView1.SelectedCells[0].Value==null)
            {
                _lastCell = null;
                return;
            }
            _lastCell = new myCell(e.ColumnIndex,e.RowIndex,dataGridView1.SelectedCells[0].Value.ToString());
        }
    }
}
