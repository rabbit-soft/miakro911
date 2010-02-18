using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace rabnet
{
    public partial class RabbitsPanel : RabNetPanel
    {
        private bool manual = true;
        private int gentree=10;
        const int NFIELD = 8;
        const int STATUSFIELD = 6;
        const int SEXFIELD = 2;
        const int NAMEFIELD = 1;
        const int SELECTEDFIELD = 0;
        public RabbitsPanel()
            : base()
        {
        }
        public RabbitsPanel(RabStatusBar rsb):base(rsb,new RabbitsFilter(rsb))
        {
            cs = new ListViewColumnSorter(listView1, new int[] {3, 10 },Options.OPT_ID.RAB_LIST);
            listView1.ListViewItemSorter = null;
        }

        protected override IDataGetter onPrepare(Filters flt)
        {
            gentree = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE)-1;
            Options op = Engine.opt();
            flt["shr"] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            flt["sht"] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            flt["sho"] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            flt["dbl"] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            flt["num"] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            flt["brd"] = op.getOption(Options.OPT_ID.MAKE_BRIDE);
            flt["suc"] = op.getOption(Options.OPT_ID.SUCKERS);
            cs.Prepare();
            IDataGetter dg = DataThread.db().getRabbits(flt);
            rsb.setText(1, dg.getCount().ToString() + " записей");
            rsb.setText(2, dg.getCount2().ToString() + " кроликов");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                cs.Restore();
                return;
            }
            Rabbit rab = (data as Rabbit);
            ListViewItem li = listView1.Items.Add(" ");
            li.Checked = false;
            li.SubItems.Add(rab.fname);
            li.Tag = rab.fid;
            li.SubItems.Add(rab.fsex);
            li.SubItems.Add(rab.fage.ToString());
            li.SubItems.Add(rab.fbreed);
            li.SubItems.Add(rab.fweight);
            li.SubItems.Add(rab.fstatus);
            li.SubItems.Add(rab.fbgp);
            li.SubItems.Add(rab.fN);
            li.SubItems.Add(rab.faverage == -1 ? "" : rab.faverage.ToString());
            li.SubItems.Add(rab.frate.ToString());
            li.SubItems.Add(rab.fcls);
            li.SubItems.Add(rab.faddress);
            li.SubItems.Add(rab.fnotes);
        }

        private void insertNode(TreeNode nd,TreeData data)
        {
            if (data.items!=null)
            for (int i = 0; i < data.items.Length; i++)
                if (data.items[i] != null)
                {
                    TreeNode n = nd.Nodes.Add(data.items[i].caption);
                    insertNode(n, data.items[i]);
                }
        }

        private bool isGirl()
        {
            string sd = listView1.SelectedItems[0].SubItems[STATUSFIELD].Text;
            return (sd=="Девочка" || sd=="Дев");
        }

        private bool isBride()
        {
            string sd = listView1.SelectedItems[0].SubItems[STATUSFIELD].Text;
            return (sd == "Невеста" || sd == "Нвс");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual)
                return;
            makeSelectedCount();
            if (listView1.SelectedItems.Count <1)
            {
                setMenu(-1,0,false);
                return;
            }
            string sx = "";
            for (int i = 0; i < listView1.SelectedItems.Count && sx.Length<2;i++ )
            {
                String s = listView1.SelectedItems[i].SubItems[SEXFIELD].Text;
                if (s[0] == 'С' || s[0] == 'C')
                    s = "S";
                if (!sx.Contains(s))
                    sx += s;
            }
            int isx=3;
            if (sx=="?") isx=0;
            if (sx=="м") isx=1;
            if (sx=="ж") isx=2;
            if (sx == "S") isx = 4;
            bool kids = false;
            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].SubItems[NFIELD].Text[0] == '+')
                kids = true;
            setMenu(isx, listView1.SelectedItems.Count,kids);
            if (listView1.SelectedItems.Count != 1)
            {
                return;
            }
            if (gentree < 0)
            {
                genTree.Nodes.Clear();
                return;
            }
            for (int ind = 0; ind < genTree.Nodes.Count; ind++)
            {
                int len ;
                len = genTree.Nodes[ind].Text.IndexOf("-");
                if (len == -1) len = genTree.Nodes[ind].Text.IndexOf(",");
                string str = genTree.Nodes[ind].Text.Remove(len);
                if (listView1.SelectedItems[0].SubItems[NAMEFIELD].Text.StartsWith(str))
                {
                    if (ind == 0) return;
                    genTree.Nodes.RemoveAt(ind);
                    break;
                }
            }
            if (genTree.Nodes.Count > 0)
                genTree.Nodes[0].ForeColor = Color.Gray;
            while (genTree.Nodes.Count > gentree)
                genTree.Nodes.RemoveAt(gentree);
            TreeData dt = Engine.db().rabbitGenTree((int)listView1.SelectedItems[0].Tag);
            if (dt!=null)
            {
                TreeNode tn = genTree.Nodes.Insert(0, dt.caption);
                tn.ForeColor = Color.Blue;
                insertNode(tn, dt);
                tn.ExpandAll();
                tn.EnsureVisible();
            }
        }

        private void setMenu(int sex,int multi,bool kids)
        {
            makeBon.Visible = passportMenuItem.Visible=proholostMenuItem.Visible=false;
            replaceMenuItem.Visible = placeChMenuItem.Visible= false;
            KillMenuItem.Visible = countKidsMenuItem.Visible=false;
            okrolMenuItem.Visible = fuckMenuItem.Visible= false;
            boysoutMenuItem.Visible = replaceYoungersMenuItem.Visible= false;
            svidMenuItem.Visible = realizeMenuItem.Visible= false;
            if (sex < 0) return;
            KillMenuItem.Visible = true;
            realizeMenuItem.Visible = true;
            replaceMenuItem.Visible = true;
            countKidsMenuItem.Visible = replaceYoungersMenuItem.Visible = kids;
                // boysoutMenuItem.Visible=
            if (multi==1)
                makeBon.Visible = true;
            if (multi == 2)
                placeChMenuItem.Visible = true;
            if (sex != 3 && multi == 1)
            {
                passportMenuItem.Visible = true;
            }
            if (sex == 2 && multi == 1)
            {
                fuckMenuItem.Text = isBride() ? "Случка" : "Вязка";
                fuckMenuItem.Visible = !isGirl() && GroupCount()==1;
            }
            if ((sex == 1 || sex == 2 || sex==4) && multi == 1)
            {
                svidMenuItem.Visible = true;
            }
            if (sex == 4)
            {
                proholostMenuItem.Visible = true;
                okrolMenuItem.Visible = true;
            }
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(-1,0,false);
            return actMenu;
        }

        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            manual = false;
            foreach (ListViewItem li in listView1.Items)
                li.Selected=true;
            listView1.Show();
            manual = true;
            listView1_SelectedIndexChanged(null, null);
        }

        private void passportMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            RabbitInfo ri = new RabbitInfo((int)listView1.SelectedItems[0].Tag);
            if (ri.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            passportMenuItem.PerformClick();
        }

        private void makeBon_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count!=1)
                return;
            int rid=(int)listView1.SelectedItems[0].Tag;
            if( (new BonForm(rid)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void proholostMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int rid = (int)listView1.SelectedItems[0].Tag;
            if((new Proholost(rid)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void newRab_Click(object sender, EventArgs e)
        {
            if((new IncomeForm()).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                rpf.addRabbit((int)li.Tag);
            if(rpf.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void placeChMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 2)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.addRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.addRabbit((int)listView1.SelectedItems[1].Tag);
            rpf.setAction(ReplaceForm.Action.CHANGE);
            if (rpf.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void KillMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            KillForm f = new KillForm();
            foreach (ListViewItem li in listView1.CheckedItems)
                f.addRabbit((int)li.Tag);
            if(f.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void countKidsMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            CountKids f = new CountKids((int)listView1.SelectedItems[0].Tag);
            if (f.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if ((new OkrolForm((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void fuckMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if((new MakeFuck((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void boysoutMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.addRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.setAction(ReplaceForm.Action.BOYSOUT);
            if (rpf.ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private void replaceYoungersMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            RabNetEngRabbit r = Engine.get().getRabbit((int)listView1.SelectedItems[0].Tag);
            if (r.youngcount<1) return;
            if((new ReplaceYoungersForm(r.youngers[0].id)).ShowDialog() == DialogResult.OK)
                rsb.run();
        }

        private int GroupCount()
        {
            String s = listView1.SelectedItems[0].SubItems[NFIELD].Text;
            if (s[0] == '[') return int.Parse(s.Substring(1, s.Length - 2));
            return 1;
        }

        private int selCount(int index)
        {
            String s = listView1.Items[index].SubItems[NFIELD].Text;
            int c = 1;
            if (s[0] == '+') c += int.Parse(s.Substring(1));
            if (s[0] == '[') c = int.Parse(s.Substring(1, s.Length - 2));
            return c;
        }

        private void makeSelectedCount()
        {
            int rows = listView1.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in listView1.SelectedItems)
                cnt += selCount(li.Index);
            rsb.setText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов",rows,cnt));
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

        private String getBon(String b)
        {
            if (b.Length == 1)
            {
                switch (b)
                {
                    case "1": return "III";
                    case "2": return "II";
                    case "3": return "I";
                    case "4": return "Элита";
                }
                return "Нет";
            }
            string minbon="5";
            for (int i = 1; i<b.Length; i++)
                if (b[i] < minbon[0])
                    minbon = "" + b[i];
            return getBon(minbon);
        }

        private XmlDocument rabToXml(RabNetEngRabbit er, OneRabbit or)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rw=doc.CreateElement("Row");
            doc.AppendChild(doc.CreateElement("Rows")).AppendChild(rw);
            if (er != null)
            {
                or = Engine.db().getLiveDeadRabbit(er.rid);
                rw.AppendChild(doc.CreateElement("header")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_HEAD)));
                rw.AppendChild(doc.CreateElement("num")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.NEXT_SVID)));
                rw.AppendChild(doc.CreateElement("date")).AppendChild(doc.CreateTextNode(DateTime.Now.Date.ToShortDateString()));
                rw.AppendChild(doc.CreateElement("director")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_GEN_DIR)));
                Catalog zones = Engine.db().catalogs().getZones();
                rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode(er.sex==OneRabbit.RabbitSex.MALE?"male":"female"));
                rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(getBon(er.bon)));
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(er.fullName));
                rw.AppendChild(doc.CreateElement("breed")).AppendChild(doc.CreateTextNode(er.breedName));
                rw.AppendChild(doc.CreateElement("born_place")).AppendChild(doc.CreateTextNode(zones.ContainsKey(er.zone)?zones[er.zone]:"-"));
                rw.AppendChild(doc.CreateElement("born_date")).AppendChild(doc.CreateTextNode(er.born.ToShortDateString()));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(er.age.ToString()));
                rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(er.smallAddress));
                rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(or.rate.ToString()));
                rw.AppendChild(doc.CreateElement("weight_date")).AppendChild(doc.CreateTextNode(or.evdate.Date.ToShortDateString()));
                rw.AppendChild(doc.CreateElement("weight_age")).AppendChild(doc.CreateTextNode(or.lost.ToString()));
                rw.AppendChild(doc.CreateElement("born")).AppendChild(doc.CreateTextNode(or.babies.ToString()));
                rw.AppendChild(doc.CreateElement("okrol")).AppendChild(doc.CreateTextNode(or.breed.ToString()));
                rw.AppendChild(doc.CreateElement("genom")).AppendChild(doc.CreateTextNode(er.genom.Replace(' ',',')));
                rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(getBon("" + er.bon[1])));
                rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(getBon("" + er.bon[2])));
                rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(getBon("" + er.bon[3])));
                rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(getBon("" + er.bon[4])));
            }
            else if (or != null)
            {
                rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode(or.sex==OneRabbit.RabbitSex.MALE?"male":"female"));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(or.status.ToString()+(or.zone==1?"(списан)":"")));
                rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(or.rate.ToString()));
                rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(getBon(or.bon)));
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(or.fullname));
                rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(getBon(""+or.bon[1])));
                rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(getBon("" + or.bon[2])));
                rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(getBon("" + or.bon[3])));
                rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(getBon("" + or.bon[4])));
            }
            else
            {
                rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode("none"));
                rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(""));
                rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(""));
            }
            return doc;
        }

        private void svidMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            XmlDocument[] docs=new XmlDocument[7];
            RabNetEngRabbit r=Engine.get().getRabbit((int)listView1.SelectedItems[0].Tag);
            docs[0]=rabToXml(r,null);
            OneRabbit[] p1 = Engine.db().getParents(r.rid, r.age);
            docs[1] = rabToXml(null, p1[0]);
            docs[2] = rabToXml(null, p1[1]);
            OneRabbit[] p2;
            if (p1[0] != null)
                p2 = Engine.db().getParents(p1[0].id, p1[0].age());
            else
                p2 = new OneRabbit[] { null, null };
            docs[3] = rabToXml(null, p2[0]);
            docs[4] = rabToXml(null, p2[1]);
            if (p1[1] != null)
                p2 = Engine.db().getParents(p1[1].id, p1[1].age());
            else
                p2 = new OneRabbit[] { null, null };
            docs[5] = rabToXml(null, p2[0]);
            docs[6] = rabToXml(null, p2[1]);
            new ReportViewForm("Племенное свидетельство", "rabbit", docs).ShowDialog();
        }

        private void realizeMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            Filters f = new Filters();
            f["cnt"] = listView1.SelectedItems.Count.ToString();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                f["r" + i.ToString()] = ((int)listView1.SelectedItems[i].Tag).ToString();
            new ReportViewForm("Кандидаты на реализацию", "realization", Engine.db().makeReport(ReportType.Type.REALIZE, f)).ShowDialog();
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            e.Item.SubItems[SELECTEDFIELD].Text = e.Item.Checked ? "" : " ";
        }

    }
}
