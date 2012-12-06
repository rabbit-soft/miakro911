using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using rabnet.forms;
using rabnet.components;

namespace rabnet
{
    public partial class YoungsPanel : RabNetPanel
    {
        //private int gentree = 10;
        private bool manual = true;
        public YoungsPanel():base()
        {
        }

        public YoungsPanel(RabStatusBar sb): base(sb, null)
        {
            _colSort = new ListViewColumnSorter(listView1, new int[] { 1,2, 8 },Options.OPT_ID.YOUNG_LIST);
            listView1.ListViewItemSorter = null;
            MakeExcel = new RSBEventHandler(this.makeExcel);
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            tvGens.MaxNodesCount = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE)-1;
            f = new Filters();
            Options op = Engine.opt();
            f[Filters.SHORT] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            f[Filters.SHOW_BLD_TIERS] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            f[Filters.SHOW_BLD_DESCR] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            f[Filters.DBL_SURNAME] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            f[Filters.SHOW_OKROL_NUM] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            _runF = f;
            _colSort.PrepareForUpdate();
            IDataGetter dg = Engine.db2().GetYoungers(f); 
            //отображение общей инфы в статус баре
            _rsb.SetText(1, dg.getCount().ToString() + " строк");
            _rsb.SetText(2, dg.getCount2().ToString() + " кроликов");
            _rsb.SetText(3, dg.getCount3().ToString() + " кормилиц");
            _rsb.SetText(4, String.Format("{0:f2} среднее количество подсосных", dg.getCount4()));
            return dg;
        }

        protected override void onItem(IData data)
        {
            YoungRabbit rab = (data as YoungRabbit);
            ListViewItem li = listView1.Items.Add(rab.NameFull);           
            li.SubItems.Add(rab.Group.ToString());
            li.SubItems.Add(rab.Age.ToString());
            li.SubItems.Add(rab.FSex());
            li.SubItems.Add(rab.BreedName);
            li.SubItems.Add(rab.FAddress(_runF.safeBool(Filters.SHOW_BLD_TIERS), _runF.safeBool(Filters.SHOW_BLD_DESCR)));
            li.SubItems.Add(rab.FBon(_runF.safeBool(Filters.SHORT)));
            li.SubItems.Add(rab.ParentName);
            li.SubItems.Add(rab.Neighbours == 0 ? "-" : rab.Neighbours.ToString());
            li.Tag = rab.ID;
		}

        private void insertNode(TreeNode nd, RabTreeData data)
        {
            if (data.Parents != null)
                for (int i = 0; i < data.Parents.Count; i++)
                    if (data.Parents[i] != null)
                    {
                        TreeNode n = nd.Nodes.Add(data.Parents[i].Name);
                        insertNode(n, data.Parents[i]);
                    }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual) return;
            setMenu();
            makeSelectedCount();
            if (listView1.SelectedItems.Count != 1) return;

            //for (int ind = 0; ind < tvGens.Nodes.Count; ind++)
            //{
            //    int len = tvGens.Nodes[ind].Text.IndexOf("-");
            //    string str = tvGens.Nodes[ind].Text.Remove(len);
            //    if (listView1.SelectedItems[0].SubItems[0].Text.StartsWith(str))
            //    {
            //        if (ind == 0) return;
            //        tvGens.Nodes.RemoveAt(ind);
            //        break;
            //    }
            //}
            //if (tvGens.Nodes.Count > 0)
            //    tvGens.Nodes[0].ForeColor = Color.Gray;
            //if (tvGens.Nodes.Count > gentree)
            //    tvGens.Nodes.RemoveAt(gentree);
            RabTreeData dt = Engine.db().rabbitGenTree((int)listView1.SelectedItems[0].Tag);
            if (dt != null)
            {
                TreeNode tn = tvGens.InsertNode(dt, true);
                //TreeNode tn = tvGens.Nodes.Insert(0, dt.Name);
                tn.ForeColor = Color.Blue;
                //insertNode(tn, dt);
                //tn.ExpandAll();
                //tn.EnsureVisible();
            }
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu();
            return actMenu;
        }

        public void setMenu()
        {
            int cnt = listView1.SelectedItems.Count;
            replaceYoungersMenuItem.Visible = cnt == 1;
            replacePlanMenuItem.Visible = cnt > 0;
        }

        private void replaceYoungersMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            if (new ReplaceYoungersForm((int)listView1.SelectedItems[0].Tag).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
                listView1.DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(ListViewItem))) return;
            Point p=listView1.PointToClient(new Point(e.X,e.Y));
            ListViewItem to = listView1.GetItemAt(p.X, p.Y);
            if (to==null)
                return;
            ListViewItem from=e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (from == null) return;
            if (Math.Abs(int.Parse(to.SubItems[2].Text) - int.Parse(from.SubItems[2].Text)) >
                Engine.opt().getIntOption(Options.OPT_ID.COMBINE_AGE))
            {
                if (MessageBox.Show(this, "Крольчата не подходят по возрасту.\nПоказать возможных кормилиц?",
                    "Ошибка", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (new ReplaceYoungersForm((int)from.Tag).ShowDialog() == DialogResult.OK)
                        _rsb.Run();
                }
                return;
            }
            RabNetEngRabbit r = Engine.get().getRabbit((int)to.Tag);
            if (new ReplaceYoungersForm((int)from.Tag, r.ParentID).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void makeSelectedCount()
        {
            int rows = listView1.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                int c = int.Parse(li.SubItems[1].Text);
                cnt += c;
            }
            _rsb.SetText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов", rows, cnt));
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            manual = false;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            manual = true;
            listView1_SelectedIndexChanged(null, null);
        }

        private void replacePlanMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(li.SubItems[2].Text));
                rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(li.SubItems[5].Text.Remove(li.SubItems[5].Text.IndexOf("["))));
                rw.AppendChild(doc.CreateElement("count")).AppendChild(doc.CreateTextNode(li.SubItems[1].Text));
            }
            new ReportViewForm(myReportType.REPLACE, doc).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void makeExcel()
        {
#if !DEMO
            ExcelMaker.MakeExcelFromLV(listView1, "Молодняк");
#endif
        }

    }
}
