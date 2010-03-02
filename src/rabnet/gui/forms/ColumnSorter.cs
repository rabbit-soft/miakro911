using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace rabnet
{
    public class ListViewColumnSorter : IComparer
    {
        int ColumnToSort;
        SortOrder OrderOfSort;
        CaseInsensitiveComparer ObjectCompare;
        private ListView lv = null;
        int[] intSorts = null;
        int selItem=0;
        Options.OPT_ID option=Options.OPT_ID.NONE;
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
            selItem = ListViewSaver.saveItem(lv);
            lv.ListViewItemSorter = null;
            lv.Items.Clear();
            lv.Hide();
        }
        public void Restore()
        {
            lv.ListViewItemSorter = this;
            lv.Sort();
            ListViewSaver.loadItem(lv, selItem);
            lv.Show();
            lv.Focus();
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
                    if (listviewX.SubItems[ColumnToSort].Text == "-" )i1 = 0;
                        else i1 = int.Parse(listviewX.SubItems[ColumnToSort].Text);
                    if (listviewY.SubItems[ColumnToSort].Text == "-" )i2 = 0;
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
            
            if (listviewX.SubItems[ColumnToSort].Text.IndexOf(".") != -1 && listviewY.SubItems[ColumnToSort].Text.IndexOf(".") != -1)
            {
                DateTime dt1 = DateTime.Parse(listviewX.SubItems[ColumnToSort].Text);
                DateTime dt2 = DateTime.Parse(listviewY.SubItems[ColumnToSort].Text);
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
                img = ImageHolder.get().getImage(0);
            else
                img = ImageHolder.get().getImage(1);
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
}