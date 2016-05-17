using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using rabnet.forms;

namespace rabnet
{
    public class ListViewColumnSorter : IComparer
    {
        int ColumnToSort;

        protected SortOrder sortOrder = SortOrder.None;

        protected CaseInsensitiveComparer ObjectCompare;

        private ListView _listView = null;

        /// <summary>
        /// Номера столбцок, которые надо сортировать как целочисленные 
        /// </summary>
        int[] _intSorts = null;

        int _selItem = 0;

        Options.OPT_ID option = Options.OPT_ID.NONE;

        public ListViewColumnSorter(ListView lv, int[] intsorts, Options.OPT_ID op)
        {
            ColumnToSort = 0;
            //OrderOfSort = SortOrder.None;
            ObjectCompare = new CaseInsensitiveComparer();
            _intSorts = intsorts;
            _listView = lv;
            lv.ListViewItemSorter = this;
            option = op;
            for (int i = 0; i < lv.Columns.Count; i++) {
                lv.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            lv.ColumnClick += new ColumnClickEventHandler(this.OnColumnClick);
            lv.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.OnColumnWidthChanged);
            lv.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.OnDrawHeader);
            lv.DrawItem += new DrawListViewItemEventHandler(this.OnDrawItem);
            lv.DrawSubItem += new DrawListViewSubItemEventHandler(this.OnDrawSubItem);
            ListViewSaver.load(op, lv);
        }

        public void PrepareForUpdate()
        {
            //MainForm.StillWorking();
            _selItem = ListViewSaver.saveItem(_listView);
            _listView.ListViewItemSorter = null;
            _listView.Items.Clear();
            _listView.BeginUpdate();
        }

        public void RestoreAfterUpdate()
        {            
            _listView.ListViewItemSorter = this;
            _listView.Sort();
            ListViewSaver.loadItem(_listView, _selItem);
            _listView.EndUpdate();
            _listView.Focus();
        }

        public ListViewColumnSorter Clear()
        {
            this.sortOrder = SortOrder.None;
            return this;
        }

        private void OnColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == SortColumn) {
                if (this.sortOrder == SortOrder.Ascending) {
                    this.sortOrder = SortOrder.Descending;
                } else {
                    this.sortOrder = SortOrder.Ascending;
                }
            } else {
                SortColumn = e.Column;
                this.sortOrder = SortOrder.Ascending;
            }
            ListViewSaver.save(option, _listView);
            (sender as ListView).Sort();
            _listView.Refresh();
        }

        public int Compare(object x, object y)
        {
            try {
                bool alreadyCompared = false;
                int compareResult = 0;
                string listViewVal_X = (x as ListViewItem).SubItems[ColumnToSort].Text,
                       listViewVal_Y = (y as ListViewItem).SubItems[ColumnToSort].Text;


                for (int i = 0; i < _intSorts.Length; i++) {
                    if (_intSorts[i] == ColumnToSort) {
                        alreadyCompared = true;
                        int i1 = this.parseIntValue(listViewVal_X),
                            i2 = this.parseIntValue(listViewVal_Y);

                        compareResult = i1.CompareTo(i2);
                        break;
                    }
                }

                if (!alreadyCompared) {
                    DateTime dt1, dt2;
                    if (DateTime.TryParse(listViewVal_X, out dt1) && DateTime.TryParse(listViewVal_Y, out dt2)) {
                        compareResult = DateTime.Compare(dt1, dt2);
                    } else {
                        compareResult = ObjectCompare.Compare(listViewVal_X, listViewVal_Y);
                    }
                }

                if (sortOrder == SortOrder.Ascending) {
                    return compareResult;
                } else if (sortOrder == SortOrder.Descending) {
                    return (-compareResult);
                } else {
                    return 0;
                }

            } catch (Exception) {
                return 0;
            }
        }

        private int parseIntValue(string val)
        {
            int result = -1;
            if (val == "-" || val == "") {
                result = -1;
            } else if (!int.TryParse(val, out result)) {
                string[] tmp = val.Split('|');
                if (tmp.Length > 0) {
                    int.TryParse(tmp[0], out result);
                }
            }
            return result;
        }

        public int SortColumn
        {
            set { ColumnToSort = value; }
            get { return ColumnToSort; }
        }

        /// <summary>
        /// Sort order
        /// </summary>
        public SortOrder Order
        {
            set { sortOrder = value; }
            get { return sortOrder; }
        }

        private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (!_listView.Focused) {
                return;
            }
            ListViewSaver.save(option, _listView);
        }

        private void OnDrawHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawBackground();
            e.DrawText();
            if (SortColumn != e.ColumnIndex) {
                return;
            }
            if (sortOrder == SortOrder.None) {
                return;
            }

            Image img = null;
            if (sortOrder == SortOrder.Ascending) {
                img = ArrowDrawer.get().getImage(0);
            } else {
                img = ArrowDrawer.get().getImage(1);
            }
            Rectangle rect = new Rectangle(e.Bounds.Right - 16, e.Bounds.Bottom - 16, 11, 11);
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
            if (index == 0) {
                return _ArrowDown;
            }
            return _ArrowUp;
        }
    }
}