using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace rabnet
{
    public class ListViewColumnSorter : IComparer
    {
        int ColumnToSort;
        SortOrder OrderOfSort;
        CaseInsensitiveComparer ObjectCompare;
        private ListView lv = null;
        int[] intSorts = null;
        public ListViewColumnSorter(ListView lv,int[] intsorts)
        {
            ColumnToSort = 0;
            OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
            this.intSorts = intsorts;
            this.lv = lv;
            lv.ColumnClick += new ColumnClickEventHandler(this.OnColumnClick);
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
                {
                    Order = SortOrder.Descending;
                }
                else
                {
                    Order = SortOrder.Ascending;
                }
            }
            else
            {
                SortColumn = e.Column;
                Order = SortOrder.Ascending;
            }
            (sender as ListView).Sort();
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
                    int i1 = int.Parse(listviewX.SubItems[ColumnToSort].Text);
                    int i2 = int.Parse(listviewY.SubItems[ColumnToSort].Text);
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

                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

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

    }
}
