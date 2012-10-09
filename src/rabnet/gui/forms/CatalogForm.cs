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
                this.Text = text == "" ? "текст" : text;
            }
        }

        private const int D1 = 0;
        private const int D2 = 1;
        private const int D3 = 2;
        private const int D4 = 3;
        private const int D5 = 4;

        /// <summary>
        /// Типы справочников
        /// </summary>C
        public enum CatalogType { NONE, BREEDS, ZONES, DEAD, PRODUCTS, VACCINES };
        private ICatalog _catalog;
        private DataTable ds = new DataTable();
        /// <summary>
        /// Тип показываемого справочника
        /// </summary>
        private CatalogType _catType = CatalogType.NONE;
        /// <summary>
        /// Вносятся ли изменения пользователем
        /// </summary>
        private bool _manual = true;
        private int _hiddenId = 0;
        /// <summary>
        /// Последняя редактируемая ячейка, нужна для того, чтобы если пользователь составит пустой текст, при выходе вставится бывший до этого.
        /// </summary>
        private myCell _lastCell = null;

        public CatalogForm()
        {
            InitializeComponent();
            this._manual = false;
        }

        public CatalogForm(CatalogType type):this()
        {
            _catType = type;
            this.Text = "Справочник ";
            switch (_catType)
            {
                case CatalogType.BREEDS:
                    Text += "Пород";
                    break;
                case CatalogType.ZONES:
                    Text += "Зон Прибытия";
                    break;
                case CatalogType.DEAD:
                    Text += "причин списания";
                    break;
                case CatalogType.PRODUCTS:
                    this.Text += "видов продукции";
                    break;
                case CatalogType.VACCINES:
                    this.Text += "вакцин";
                    break;
            }
			//ds.RowChanged += new DataRowChangeEventHandler(this.OnRowChange);
            //ds.TableNewRow += new DataTableNewRowEventHandler(this.OnRowInsert);
            fillTable(false);
        }

        /// <summary>
        /// Заполнение каталога данными
        /// </summary>
        /// <param name="update"></param>
        private void fillTable(bool update)
        {
            btDeleteImage.Visible = btNewImage.Visible = false;
            _manual = false;
            ds.Clear();
            
            switch (_catType)
            {
                case CatalogType.BREEDS:
                    _catalog = Engine.db().getBreeds();
                    break;
                case CatalogType.ZONES:
                    _catalog = Engine.db().getZones();
                    break;
                case CatalogType.DEAD:
                    _catalog = Engine.db().getDeadReasons();                   
                    break;
                case CatalogType.PRODUCTS:
                    _catalog = Engine.db().getProductTypes();
                    btDeleteImage.Visible = btNewImage.Visible = true;
                    btDeleteImage.Enabled = btNewImage.Enabled = false;
                    break;
                case CatalogType.VACCINES:
                    _catalog = Engine.db().getVaccines();
                    break;
            }
            CatalogData cd = _catalog.Get();
            //int colorExist=-1,imageExist = -1, boolExist=-1; //чтобы удалить слово #color#
            if (!update)//установка колонок
            {
				DataGridViewTextBoxColumn TextColumn;
				for (int i = 0; i < cd.ColNames.Length; i++)
				{
                    if (cd.ColNames[i].StartsWith(CatalogData.COLOR_MARKER))//в породах
					{
                        //colorExist = i;
						ColorPickerColumn colColorPick = new ColorPickerColumn();
						colColorPick.Name = cd.ColNames[i];
						dataGridView1.Columns.Add(colColorPick);
					}
                    else if (cd.ColNames[i].StartsWith(CatalogData.IMAGE_MARKER))//в продукции
                    {
                        //imageExist = i;
                        DataGridViewImageColumn colImage = new DataGridViewImageColumn();
                        colImage.Name = cd.ColNames[i];
                        dataGridView1.Columns.Add(colImage);
                    }
                    else if (cd.ColNames[i].StartsWith(CatalogData.BOOL_MARKER))
                    {
                        DataGridViewCheckBoxColumn colBool = new DataGridViewCheckBoxColumn();
                        colBool.Name = cd.ColNames[i];
                        dataGridView1.Columns.Add(colBool);
                    }
                    else
					{
						TextColumn = new DataGridViewTextBoxColumn();
						TextColumn.Name = cd.ColNames[i];
						dataGridView1.Columns.Add(TextColumn);
					}
				}
                if (_catType == CatalogType.VACCINES)
                {
                    dataGridView1.Columns[D1].Width = 30;
                    dataGridView1.Columns[D1].ReadOnly = true;
                    this.Width += 70;
                }
                /// Далее добавляется одна невидимая ячейка, в коротой содержится ID записи                
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
			for (int i = 0; i < cd.Rows.Length; i++)
            {
				int rownumber = dataGridView1.Rows.Add();
                for (int j = 0; j < cd.ColNames.Length; j++)    //заполнение столбцов
				{
					if (cd.ColNames[j].StartsWith(CatalogData.COLOR_MARKER))
					{
						value = cd.Rows[i].data[j];
						if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out res))
						{
							lclColor = Color.FromArgb(res);
						}
						else lclColor = Color.FromName(value);						
						dataGridView1.Rows[rownumber].Cells[j].Value = lclColor;
					}
                    else if (cd.ColNames[j].StartsWith(CatalogData.IMAGE_MARKER))
                    {
                        if (cd.Rows[i].data.Length == 0) continue;
                        MemoryStream ms = new MemoryStream(Convert.FromBase64String(cd.Rows[i].data[j]));
                        Image img = Image.FromStream(ms);
                        dataGridView1.Rows[rownumber].Cells[j].Value = img;
                        dataGridView1.Rows[rownumber].Height = img.Height;
                    }
                    else if (cd.ColNames[j].StartsWith(CatalogData.BOOL_MARKER))
                    {
                        dataGridView1.Rows[rownumber].Cells[j].Value = cd.Rows[i].data[j] == "1";
                    }
					else 
                        dataGridView1.Rows[rownumber].Cells[j].Value = cd.Rows[i].data[j];
				}
				dataGridView1.Rows[rownumber].Cells[cd.ColNames.Length].Value = cd.Rows[i].key;

			}
            if (_catType == CatalogType.DEAD)
            {
                dataGridView1.Rows[0].ReadOnly =    //на убой
                    dataGridView1.Rows[1].ReadOnly =//продажа племенного поголовья
                    dataGridView1.Rows[2].ReadOnly =//падеж при подсчете
                    dataGridView1.Rows[3].ReadOnly = true;//падеж
            }
            ///убираем маркеры
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name.StartsWith(CatalogData.COLOR_MARKER))
                    col.Name = col.Name.Remove(0, CatalogData.COLOR_MARKER.Length);
                if (col.Name.StartsWith(CatalogData.IMAGE_MARKER))
                    col.Name = col.Name.Remove(0, CatalogData.IMAGE_MARKER.Length);
                if (col.Name.StartsWith(CatalogData.BOOL_MARKER))
                    col.Name = col.Name.Remove(0, CatalogData.BOOL_MARKER.Length);
            }
            /*if (colorExist != -1)
                dataGridView1.Columns[colorExist].Name = dataGridView1.Columns[colorExist].Name.Remove(0, CatalogData.COLOR_MARKER.Length);
            if (imageExist != -1)
                dataGridView1.Columns[imageExist].Name = dataGridView1.Columns[imageExist].Name.Remove(0, CatalogData.IMAGE_MARKER.Length);
            if (boolExist != -1)
                dataGridView1.Columns[boolExist].Name = dataGridView1.Columns[boolExist].Name.Remove(0, CatalogData.BOOL_MARKER.Length);*/              

            _manual = true;
        }

        //private void OnRowChange(object sender, DataRowChangeEventArgs e)
        //{

        //}

        //private void OnRowInsert(object sender,DataTableNewRowEventArgs e)
        //{
        //    //e.Row.ItemArray[2]=-1;
        //}


		private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (!_manual) return;
            DataGridViewRow editRow = dataGridView1.Rows[e.RowIndex];
            if (_lastCell != null)
            {
                if (editRow.Cells[e.ColumnIndex].Value == null && e.ColumnIndex == _lastCell.Coll && e.RowIndex == _lastCell.Row)
                {
                    editRow.Cells[e.ColumnIndex].Value = _lastCell.Text;
                    return;
                }
            }

            try
            {
                /**
                 * Если последняя невидимая ячейка не содержит информации(ID записи),
                 * значит нужно добавить новую запись.
                 * Иначе изменить существующую
                 */
                switch (_catType)
                {
                    case CatalogType.BREEDS: breeds_RowEdited(editRow); break;
                    case CatalogType.ZONES: zones_RowEdited(editRow); break;                   
                    case CatalogType.DEAD: deadReasons_RowEdited(editRow); break;                   
                    case CatalogType.PRODUCTS: products_RowEdited(editRow); break;
                    case CatalogType.VACCINES: vaccines_RowEdited(editRow); break;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
		}

        private void breeds_RowEdited(DataGridViewRow editRow)
        {
            string col0 = "", col1 = "", col2 = "";
            if (editRow.Cells[_hiddenId].Value == null)
            {
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                if (editRow.Cells[D2].Value != null)
                    col1 = editRow.Cells[D2].Value.ToString();

                if (editRow.Cells[D3].Value != null)
                    col2 = ((Color)(editRow.Cells[D3].Value)).Name;

                editRow.Cells[_hiddenId].Value = /*Engine.db().getBreeds().*/ _catalog.Add(col0, col1, col2);
            }
            else
            {
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                if (editRow.Cells[D2].Value != null)
                    col1 = editRow.Cells[D2].Value.ToString();

                if (editRow.Cells[D3].Value != null)
                    col2 = ((Color)(editRow.Cells[D3].Value)).Name;

                //Engine.db().getBreeds().
                _catalog.Change(Convert.ToInt32(editRow.Cells[_hiddenId].Value), col0, col1, col2);
                dataGridView1.Refresh();
            }
        }

        private void zones_RowEdited(DataGridViewRow editRow)
        {
            string col0 = "0", col1 = "", col2 = "";
            if (editRow.Cells[_hiddenId].Value == null)
            {
                
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                if (editRow.Cells[D2].Value != null)
                    col1 = editRow.Cells[D2].Value.ToString();

                if (editRow.Cells[D3].Value != null)
                    col2 = editRow.Cells[D3].Value.ToString();

                editRow.Cells[_hiddenId].Value = /*Engine.db().getZones().*/ _catalog.Add(col0.ToString(), col1, col2);
            }
            else
            {                
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                if (editRow.Cells[D2].Value != null)
                    col1 = editRow.Cells[D2].Value.ToString();

                if (editRow.Cells[D3].Value != null)
                    col2 = editRow.Cells[D3].Value.ToString();
                //Engine.db().getZones()
                _catalog.Change(Convert.ToInt32(editRow.Cells[_hiddenId].Value),col0, col1, col2);
                editRow.Cells[_hiddenId].Value = editRow.Cells[D1].Value;
            }
        }

        private void deadReasons_RowEdited(DataGridViewRow editRow)
        {
            string col0 = "";
            if (editRow.Cells[_hiddenId].Value == null)
            {
                
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                editRow.Cells[_hiddenId].Value = Engine.db().getDeadReasons().Add(col0);
            }
            else
            {
                if (editRow.Cells[D1].Value != null)
                    col0 = editRow.Cells[D1].Value.ToString();

                Engine.db().getDeadReasons().Change(Convert.ToInt32(editRow.Cells[_hiddenId].Value), col0);
            }
        }

        private void products_RowEdited(DataGridViewRow editRow)
        {
            if (editRow.Cells[_hiddenId].Value == null)
            {
                string col0 = "", col1 = "";
                if (editRow.Cells[D1].Value != null)
                {
                    col0 = editRow.Cells[D1].Value.ToString();
                    if (col0.Length > 20)
                    {
                        col0 = col0.Substring(0, 20);
                        editRow.Cells[D1].Value = col0;
                    }
                }

                if (editRow.Cells[D2].Value != null)
                {
                    col1 = editRow.Cells[D2].Value.ToString();
                    if (col1.Length > 10)
                    {
                        col1 = col1.Substring(0, 10);
                        editRow.Cells[D2].Value = col1;
                    }
                }

                byte[] col2 = new byte[0];
                Image img = (editRow.Cells[D3].Value as Image);
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Jpeg);
                    col2 = ms.ToArray();
                    if (col2.Length == 784)
                        col2 = new byte[0];
                }
                editRow.Cells[_hiddenId].Value = Engine.db().getProductTypes().Add(col0, col1, Convert.ToBase64String(col2));
            }
            else
            {
                string col0 = "", col1 = "";
                if (editRow.Cells[D1].Value != null)
                {
                    col0 = editRow.Cells[D1].Value.ToString();
                    if (col0.Length > 20)
                    {
                        col0 = col0.Substring(0, 20);
                        editRow.Cells[D1].Value = col0;
                    }
                }

                if (editRow.Cells[D2].Value != null)
                {
                    col1 = editRow.Cells[D2].Value.ToString();
                    if (col1.Length > 10)
                    {
                        col1 = col1.Substring(0, 10);
                        editRow.Cells[D2].Value = col1;
                    }
                }

                byte[] col2 = new byte[0];
                Image img = (editRow.Cells[D3].Value as Image);
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Jpeg);
                    col2 = ms.ToArray();
                    if (col2.Length == 784)
                        col2 = new byte[0];
                }

                //Engine.db().getProductTypes().
                _catalog.Change(Convert.ToInt32(editRow.Cells[_hiddenId].Value), col0, col1, Convert.ToBase64String(col2));
            }
        }

        private void vaccines_RowEdited(DataGridViewRow editRow)
        {
            string col0 = "", col1 = "", col2 = "", col3 = "";
            if (editRow.Cells[_hiddenId].Value == null)
            {
                if (editRow.Cells[D2].Value != null)
                    col0 = editRow.Cells[D2].Value.ToString();
                if (col0 == "") return;
                col1 = getIntVal(editRow.Cells[D3].Value);
                col2 = getIntVal(editRow.Cells[D4].Value);
                if (editRow.Cells[D5].Value != null)
                    col3 = editRow.Cells[D5].Value.ToString();
                editRow.Cells[_hiddenId].Value = /*Engine.db().getBreeds().*/ _catalog.Add(col0, col1, col2,col3);
                editRow.Cells[D1].Value = editRow.Cells[_hiddenId].Value;
                editRow.Cells[D3].Value = 0;
            }
            else
            {
                if (editRow.Cells[D2].Value != null)
                    col0 = editRow.Cells[D2].Value.ToString();
                if (col0 == "") return;
                col1 = getIntVal(editRow.Cells[D3].Value);
                col2 = getIntVal(editRow.Cells[D4].Value);

                if (editRow.Cells[D5].Value != null)
                    col3 = editRow.Cells[D5].Value.ToString();

                //Engine.db().getBreeds().
                _catalog.Change(Convert.ToInt32(editRow.Cells[_hiddenId].Value), col0, col1, col2,col3);
                dataGridView1.Refresh();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_catType != CatalogType.PRODUCTS) return;
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
            if (_catType == CatalogType.PRODUCTS && dataGridView1.SelectedCells[0].ColumnIndex == 2 && dataGridView1.SelectedCells.Count != 0)
                btNewImage.PerformClick();
        }

        private void btNewImage_Click(object sender, EventArgs e)
        {
            const int mustW = 300;
            const int mustH = 200;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                //double ratio = (double)img.Width / (double)img.Height;
                //var bmp = new Bitmap(img, (int)(mustH * ratio), mustH);

                Bitmap bmp = new Bitmap(mustW, mustH);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img, new Rectangle(0, 0, mustW, mustH), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

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
            if (!_manual || dataGridView1.SelectedCells.Count==0) return;
            if ((_catType == CatalogType.PRODUCTS && e.ColumnIndex == 3) || dataGridView1.SelectedCells[0].Value==null)
            {
                _lastCell = null;
                return;
            }
            _lastCell = new myCell(e.ColumnIndex,e.RowIndex,dataGridView1.SelectedCells[0].Value.ToString());
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_manual && _catType == CatalogType.VACCINES)
            {
                if (dataGridView1.CurrentCell.ColumnIndex == D5)
                {
                    _manual = false;
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    _manual = true;
                    dataGridView1_CellEndEdit(sender, new DataGridViewCellEventArgs(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex));
                }
            }
        }

        protected string getIntVal(object cellValue)
        {
            int intVal = 0;
            string result = cellValue == null ? "0" : cellValue.ToString();
            int.TryParse(result, out intVal);
            result = intVal.ToString();
            return result;
        }
    }
}
