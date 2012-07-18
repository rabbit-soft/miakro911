using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace rabnet
{
    public class ListViewColumnSorter : IComparer
    {
        int ColumnToSort;
        SortOrder OrderOfSort;
        CaseInsensitiveComparer ObjectCompare;
        private ListView lv = null;
        /// <summary>
        /// Номера столбцок, которые надо сортировать как целочисленные 
        /// </summary>
        int[] intSorts = null;
        int selItem = 0;
        Options.OPT_ID option = Options.OPT_ID.NONE;
        public ListViewColumnSorter(ListView lv,int[] intsorts,Options.OPT_ID op)
        {
            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
            this.intSorts = intsorts;
            this.lv = lv;
            lv.ListViewItemSorter = this;
            option = op;
			for (int i = 0; i < lv.Columns.Count; i++)
			{
				lv.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
			}
			lv.ColumnClick += new ColumnClickEventHandler(this.OnColumnClick);
            lv.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.OnColumnWidthChanged);
            lv.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.OnDrawHeader);
            lv.DrawItem += new DrawListViewItemEventHandler(this.OnDrawItem);
            lv.DrawSubItem += new DrawListViewSubItemEventHandler(this.OnDrawSubItem);
            ListViewSaver.load(op, lv);
        }

        public void Prepare()
        {
            MainForm.StillWorking();
            selItem = ListViewSaver.saveItem(lv);
            lv.ListViewItemSorter = null;
            lv.Items.Clear();
			lv.BeginUpdate();
        }
        public void Restore()
        {
			ListViewSaver.load(option, lv);
			lv.ListViewItemSorter = this;
            lv.Sort();
            ListViewSaver.loadItem(lv, selItem);
			lv.EndUpdate();
            lv.Focus();
        }

		public void SemiReady()
		{

		}

        public ListViewColumnSorter Clear()
        {
            Order = SortOrder.None;
            return this;
        }
        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == SortColumn)
            {
                if (Order == SortOrder.Ascending)
                    Order = SortOrder.Descending;
                else
                    Order = SortOrder.Ascending;
            }
            else
            {
                SortColumn = e.Column;
                Order = SortOrder.Ascending;
            }
            ListViewSaver.save(option, lv);
            (sender as ListView).Sort();
            lv.Refresh();
        }

        public int Compare(object x, object y)
        {
            int compareResult=0;
            ListViewItem listviewX, listviewY;

            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            for (int i = 0; i < intSorts.Length;i++ )
                if (intSorts[i] == ColumnToSort)
                {
                    int i1, i2;
                    if (listviewX.SubItems[ColumnToSort].Text == "-" || listviewX.SubItems[ColumnToSort].Text == "") i1 = -1;
                        else i1 = int.Parse(listviewX.SubItems[ColumnToSort].Text);
                    if (listviewY.SubItems[ColumnToSort].Text == "-" || listviewY.SubItems[ColumnToSort].Text == "") i2 = -1;
                        else i2 = int.Parse(listviewY.SubItems[ColumnToSort].Text);                                                
                    compareResult=i1 - i2;
                    if (OrderOfSort == SortOrder.Ascending)
                    {
                        return compareResult;
                    }
                    else if (OrderOfSort == SortOrder.Descending)
                    {
                        return (-compareResult);
                    }
                    else
                    {
                        return 0;
                    }
                }
            
            DateTime dt1,dt2;
            if (DateTime.TryParse(listviewX.SubItems[ColumnToSort].Text, out dt1) && DateTime.TryParse(listviewY.SubItems[ColumnToSort].Text,out dt2))
            {
                compareResult = DateTime.Compare(dt1, dt2);
            }
            else
            {          
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }

            if (OrderOfSort == SortOrder.Ascending)
            {
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }


        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

        private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (!lv.Focused) return;
            ListViewSaver.save(option, lv);
        }

        private void OnDrawHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
            if (SortColumn != e.ColumnIndex) return;
            if (OrderOfSort == SortOrder.None) return;
            Image img=null;
            if (OrderOfSort == SortOrder.Ascending)
                img = ArrowDrawer.get().getImage(0);
            else
                img = ArrowDrawer.get().getImage(1);
            Rectangle rect=new Rectangle(e.Bounds.Right-16,e.Bounds.Bottom-16,11,11);
            e.Graphics.DrawImage(img, rect);
        }

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }
    }

    public class ArrowDrawer
    {
        Bitmap _ArrowDown;
        Bitmap _ArrowUp;

        public ArrowDrawer()
        {
            DrawArrows();
        }

        private void DrawArrows()
        {
            Graphics g;
            _ArrowDown = new Bitmap(11, 11);
            g = Graphics.FromImage(_ArrowDown);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawPolygon(SystemPens.ControlDark, new Point[] { new Point(9, 3), new Point(5, 7), new Point(1, 3) });
            g.FillPolygon(SystemBrushes.ControlDark, new Point[] { new Point(9, 3), new Point(5, 7), new Point(1, 3) });
            g.Dispose();

            _ArrowUp = new Bitmap(11, 11);
            g = Graphics.FromImage(_ArrowUp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawPolygon(SystemPens.ControlDark, new Point[] { new Point(5, 3), new Point(9, 7), new Point(1, 7) });
            g.FillPolygon(SystemBrushes.ControlDark, new Point[] { new Point(5, 3), new Point(9, 7), new Point(1, 7) });
            g.Dispose();
        }

        private static ArrowDrawer obj = null;
        public static ArrowDrawer get()
        {
            if (obj == null)
                obj = new ArrowDrawer();
            return obj;
        }

        public Image getImage(int index)
        {
            if (index == 0)
            {
                return _ArrowDown;
            }
            return _ArrowUp;
        }
    }
}