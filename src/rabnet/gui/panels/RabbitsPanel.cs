using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace rabnet
{
    public partial class RabbitsPanel : RabNetPanel
    {
        private bool manual = true;
        private int gentree=10;
        const int NFIELD = 7;
        const int STATUSFIELD = 5;
        const int SEXFIELD = 1;
        const int NAMEFIELD = 0;

        public RabbitsPanel(): base() { }
        public RabbitsPanel(RabStatusBar rsb):base(rsb,new RabbitsFilter(rsb))
        {
            colSort = new ListViewColumnSorter(listView1, new int[] {2,8,9 },Options.OPT_ID.RAB_LIST);
            listView1.ListViewItemSorter = null;
			miGenetic.Enabled = GeneticsManagerSafe.GeneticsModuleTest();
            MakeExcel = new RabStatusBar.ExcelButtonClickDelegate(this.makeExcel);
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            gentree = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE)-1;
            Options op = Engine.opt();
            f[Filters.SHORT] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            f[Filters.SHOW_BLD_TIERS] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            f[Filters.SHOW_BLD_DESCR] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            f[Filters.DBL_SURNAME] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            f[Filters.SHOW_OKROL_NUM] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            f[Filters.MAKE_BRIDE] = op.getOption(Options.OPT_ID.MAKE_BRIDE);
            //flt["suc"] = op.getOption(Options.OPT_ID.COUNT_SUCKERS);
            f[Filters.MAKE_CANDIDATE] = op.getOption(Options.OPT_ID.MAKE_CANDIDATE);
            _runF = f;
            colSort.Prepare();
            IDataGetter dg = DataThread.db().getRabbits(f);
            _rsb.setText(1, dg.getCount().ToString() + " записей");
            _rsb.setText(2, dg.getCount2().ToString() + " кроликов");
            return dg;
        }

        private delegate void onItemDelegate(IData data);


        protected override void onItem(IData data)
        {
            if (this.listView1.InvokeRequired)
            {
                onItemDelegate onItemDelegateV = new onItemDelegate(onItem);
                this.listView1.Invoke(onItemDelegateV, new object[] { data });
                return;
            }

            if (data == null)
            {
                colSort.Restore();
                return;
            }
            AdultRabbit rab = (data as AdultRabbit);
            ListViewItem li = listView1.Items.Add(rab.NameFull);
            li.Tag = rab.ID;
            li.SubItems.Add(rab.FSex());
            li.SubItems.Add(rab.Age.ToString());
            li.SubItems.Add(rab.BreedName);
            li.SubItems.Add(rab.FWeight());
            li.SubItems.Add(rab.FStatus(_runF.safeBool(Filters.SHORT), _runF.safeInt(Filters.MAKE_CANDIDATE), _runF.safeInt(Filters.MAKE_BRIDE)));
            li.SubItems.Add(rab.FFlags());
            li.SubItems.Add(rab.FGroup());
            li.SubItems.Add(rab.KidsAge == -1 ? "" : rab.KidsAge.ToString());
            li.SubItems.Add(rab.Rate.ToString());
            li.SubItems.Add(rab.FBon(_runF.safeBool(Filters.SHORT)));
            li.SubItems.Add(rab.FAddress(_runF.safeBool(Filters.SHOW_BLD_TIERS), _runF.safeBool(Filters.SHOW_BLD_TIERS)));
            li.SubItems.Add(rab.Notes);
			colSort.SemiReady();
        }

        private void insertNode(TreeNode nd,TreeData data)
        {
            if (data.Childrens!=null)
            for (int i = 0; i < data.Childrens.Count; i++)
                if (data.Childrens[i] != null)
                {
                    TreeNode n = nd.Nodes.Add(data.Childrens[i].Name);
                    insertNode(n, data.Childrens[i]);
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
            if (listView1.SelectedItems.Count != 1)
            {
                return;
            }
            if (gentree < 0)
            {
                genTree.Nodes.Clear();
                return;
            }
            //проверка дерева кроликов на совпадения
            if (listView1.SelectedItems[0].SubItems[0].Text.IndexOf("-") == 0) return;
            for (int ind = 0; ind < genTree.Nodes.Count; ind++)
            {
                int len;
                len = genTree.Nodes[ind].Text.IndexOf("-");
                if (len == -1)
                    len = genTree.Nodes[ind].Text.IndexOf(",");
                else if (genTree.Nodes[ind].Text.IndexOf(",") < len)
                    len = genTree.Nodes[ind].Text.IndexOf(",");
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
            if (dt != null && dt.Name != null)
            {
                TreeNode tn = genTree.Nodes.Insert(0, dt.Name);
                tn.ForeColor = Color.Blue;
                insertNode(tn, dt);
                tn.ExpandAll();
                tn.EnsureVisible();
            }
            else
            {
                MessageBox.Show(@"Не возможно найти информацию по данной записи.
Возможно данного кролика списал другой сетевой пользователь программы.
Во избежании проблем, придется обновить Лист поголовья", "Не могу найти запись");
                _rsb.Run();
            }
        }

        /// <summary>
        /// Устанавливает набор возможных действий
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="multi"></param>
        /// <param name="kids"></param>
        private void setMenu(int sex,int multi,bool kids)
        {
            makeBon.Visible = passportMenuItem.Visible=proholostMenuItem.Visible=false;
            replaceMenuItem.Visible = placeChMenuItem.Visible= false;
            KillMenuItem.Visible = countKidsMenuItem.Visible=false;
            okrolMenuItem.Visible = fuckMenuItem.Visible= false;
            boysoutMenuItem.Visible = replaceYoungersMenuItem.Visible= false;
            svidMenuItem.Visible = realizeMenuItem.Visible= false;
            plemMenuItem.Visible = replacePlanMenuItem.Visible= false;
            toolStripSeparator1.Visible = toolStripSeparator2.Visible = toolStripSeparator3.Visible = false;//separators
            miGenetic.Visible = false;

            if (sex < 0) return;
            toolStripSeparator1.Visible = true;
            miGenetic.Visible = toolStripSeparator3.Visible = true;
            plemMenuItem.Visible = true;
            KillMenuItem.Visible = true;
            realizeMenuItem.Visible = true;
            replaceMenuItem.Visible = true;
            countKidsMenuItem.Visible = replaceYoungersMenuItem.Visible = toolStripSeparator2.Visible = kids;
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
            if (multi > 1 || (multi==1 && listView1.SelectedItems[0].SubItems[NFIELD].Text[0]=='['))
                replacePlanMenuItem.Visible=true;
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(-1,0,false);
            return actMenu;
        }

        private void passportMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            RabbitInfo ri = new RabbitInfo((int)listView1.SelectedItems[0].Tag);
            if (ri.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            listView1_SelectedIndexChanged(null, null);
            actMenu_Opening(null, null);
            passportMenuItem.PerformClick();
        }

        private void makeBon_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count!=1)
                return;
            int rid=(int)listView1.SelectedItems[0].Tag;
            if( (new BonForm(rid)).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void proholostMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            int rid = (int)listView1.SelectedItems[0].Tag;
            if((new Proholost(rid)).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void newRab_Click(object sender, EventArgs e)
        {
#if !DEBUG
            try
            {
#endif
                if ((new IncomeForm()).ShowDialog() == DialogResult.OK)
                    _rsb.Run();
#if !DEBUG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
#endif
        }

        private void replaceMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                rpf.AddRabbit((int)li.Tag);
            if(rpf.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void placeChMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 2)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.AddRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.AddRabbit((int)listView1.SelectedItems[1].Tag);
            rpf.SetAction(ReplaceForm.Action.CHANGE);
            if (rpf.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void KillMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;
            if (Engine.db().getDeadReasons().Get().ColNames.Length == 0)
            {
                MessageBox.Show("Нет ни одной причины списания. Вы можете добавить их в  меню Вид->Причины списания");
                return;
            }
            KillForm f = new KillForm();
            foreach (ListViewItem li in listView1.SelectedItems)
                f.addRabbit((int)li.Tag);
            if(f.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void countKidsMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            CountKids f = new CountKids((int)listView1.SelectedItems[0].Tag);
            if (f.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if ((new OkrolForm((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void fuckMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            if((new MakeFuckForm((int)listView1.SelectedItems[0].Tag)).ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void boysoutMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            ReplaceForm rpf = new ReplaceForm();
            rpf.AddRabbit((int)listView1.SelectedItems[0].Tag);
            rpf.SetAction(ReplaceForm.Action.BOYSOUT);
            if (rpf.ShowDialog() == DialogResult.OK)
                _rsb.Run();
        }

        private void replaceYoungersMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            if(PreReplaceYoungersForm.MakeChoice((int)listView1.SelectedItems[0].Tag) == DialogResult.OK)
                _rsb.Run();
        }

        private int GroupCount()
        {
            String s = listView1.SelectedItems[0].SubItems[NFIELD].Text;
            if (s[0] == '[') return int.Parse(s.Substring(1, s.Length - 2));
            return 1;
        }

        private int selCount(int index)
        {
            if (index < 0) return 0;
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
            _rsb.setText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов",rows,cnt));
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

        private String getBon(String b) //todo проверить на схожесть с RabbitBon
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
            return rabToXml(er, or, null);
        }

        /// <summary>
        /// Нужно для отчета Племенное свидетельство
        /// </summary>
        /// <param name="er"></param>
        /// <param name="or"></param>
        /// <param name="hasdoc"></param>
        /// <returns></returns>
        private XmlDocument rabToXml(RabNetEngRabbit er, OneRabbit or, XmlDocument hasdoc)
        {
            XmlDocument doc = null;
            if (hasdoc == null)
            {
                doc = new XmlDocument();
                doc.AppendChild(doc.CreateElement("Rows"));
            }
            else
                doc = hasdoc;
            XmlElement rw=doc.CreateElement("Row");
            doc.DocumentElement.AppendChild(rw);
            if (er != null)
            {
                or = Engine.db().getLiveDeadRabbit(er.RabID);
                if (hasdoc==null)
                {
                    ReportHelper.Append(rw, doc, "header", Engine.opt().getOption(Options.OPT_ID.SVID_HEAD));
                    ReportHelper.Append(rw, doc, "num", Engine.opt().getOption(Options.OPT_ID.NEXT_SVID));
                    ReportHelper.Append(rw, doc, "date", DateTime.Now.Date.ToShortDateString());
                    ReportHelper.Append(rw, doc, "director", Engine.opt().getOption(Options.OPT_ID.SVID_GEN_DIR));
                    //rw.AppendChild(doc.CreateElement("header")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_HEAD)));
                    //rw.AppendChild(doc.CreateElement("num")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.NEXT_SVID)));
                    //rw.AppendChild(doc.CreateElement("date")).AppendChild(doc.CreateTextNode(DateTime.Now.Date.ToShortDateString()));
                    //rw.AppendChild(doc.CreateElement("director")).AppendChild(doc.CreateTextNode(Engine.opt().getOption(Options.OPT_ID.SVID_GEN_DIR)));
                }
                else
                {
                    rw.AppendChild(doc.CreateElement("group")).AppendChild(doc.CreateTextNode(er.Group.ToString()));
                }
                Catalog zones = Engine.db().catalogs().getZones();
                ReportHelper.Append(rw, doc, "sex", er.Sex == Rabbit.SexType.MALE ? "male" : (er.Sex == Rabbit.SexType.FEMALE ? "female" : "void"));
                ReportHelper.Append(rw, doc, "class", getBon(er.Bon));
                ReportHelper.Append(rw, doc, "name", er.FullName);
                ReportHelper.Append(rw, doc, "breed", er.BreedName);
                ReportHelper.Append(rw, doc, "born_place", zones.ContainsKey(er.Zone) ? zones[er.Zone] : "-");
                ReportHelper.Append(rw, doc, "born_date", er.Born.ToShortDateString());
                ReportHelper.Append(rw, doc, "age", er.Age.ToString());
                ReportHelper.Append(rw, doc, "address", er.SmallAddress);
                ReportHelper.Append(rw, doc, "weight", or.FWeight().ToString());
                ReportHelper.Append(rw, doc, "weight_date", or.WeightDate.Date.ToShortDateString());
                ReportHelper.Append(rw, doc, "weight_age", or.WeightAge.ToString());
                ReportHelper.Append(rw, doc, "born", or.kidsOverAll.ToString());//сколько родила
                ReportHelper.Append(rw, doc, "okrol", or.Okrol.ToString());
                ReportHelper.Append(rw, doc, "genom", er.Genom.Replace(' ', ','));
                ReportHelper.Append(rw, doc, "wclass", getBon("" + er.Bon[1]));
                ReportHelper.Append(rw, doc, "bclass", getBon("" + er.Bon[1]));
                ReportHelper.Append(rw, doc, "hclass", getBon("" + er.Bon[3]));
                ReportHelper.Append(rw, doc, "cclass", getBon("" + er.Bon[4]));
                #region old
                //rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode(er.Sex == Rabbit.SexType.MALE ? "male" : (er.Sex == Rabbit.SexType.FEMALE ? "female" : "void")));
                //rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode());
                //rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(er.FullName));
                //rw.AppendChild(doc.CreateElement("breed")).AppendChild(doc.CreateTextNode(er.BreedName));
                //rw.AppendChild(doc.CreateElement("born_place")).AppendChild(doc.CreateTextNode(zones.ContainsKey(er.Zone)?zones[er.Zone]:"-"));
                //rw.AppendChild(doc.CreateElement("born_date")).AppendChild(doc.CreateTextNode(er.Born.ToShortDateString()));
                //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(er.age.ToString()));
                //rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(er.SmallAddress));
                //rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(or.Rate.ToString()));
                //rw.AppendChild(doc.CreateElement("weight_date")).AppendChild(doc.CreateTextNode(or.EventDate.Date.ToShortDateString()));
                //rw.AppendChild(doc.CreateElement("weight_age")).AppendChild(doc.CreateTextNode(or.weight_age.ToString()));
                //rw.AppendChild(doc.CreateElement("born")).AppendChild(doc.CreateTextNode(or.kidsOverAll.ToString()));
                //rw.AppendChild(doc.CreateElement("okrol")).AppendChild(doc.CreateTextNode(or.BreedID.ToString()));
                //rw.AppendChild(doc.CreateElement("genom")).AppendChild(doc.CreateTextNode(er.Genom.Replace(' ',',')));
                //rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[1])));
                //rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[2])));
                //rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[3])));
                //rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[4])));
                //rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode(er.Sex == Rabbit.SexType.MALE ? "male" : (er.Sex == Rabbit.SexType.FEMALE ? "female" : "void")));
                //rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(getBon(er.Bon)));
                //rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(er.FullName));
                //rw.AppendChild(doc.CreateElement("breed")).AppendChild(doc.CreateTextNode(er.BreedName));
                //rw.AppendChild(doc.CreateElement("born_place")).AppendChild(doc.CreateTextNode(zones.ContainsKey(er.Zone)?zones[er.Zone]:"-"));
                //rw.AppendChild(doc.CreateElement("born_date")).AppendChild(doc.CreateTextNode(er.Born.ToShortDateString()));
                //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(er.age.ToString()));
                //rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(er.SmallAddress));
                //rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(or.Rate.ToString()));
                //rw.AppendChild(doc.CreateElement("weight_date")).AppendChild(doc.CreateTextNode(or.EventDate.Date.ToShortDateString()));
                //rw.AppendChild(doc.CreateElement("weight_age")).AppendChild(doc.CreateTextNode(or.weight_age.ToString()));
                //rw.AppendChild(doc.CreateElement("born")).AppendChild(doc.CreateTextNode(or.kidsOverAll.ToString()));
                //rw.AppendChild(doc.CreateElement("okrol")).AppendChild(doc.CreateTextNode(or.BreedID.ToString()));
                //rw.AppendChild(doc.CreateElement("genom")).AppendChild(doc.CreateTextNode(er.Genom.Replace(' ',',')));
                //rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[1])));
                //rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[2])));
                //rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[3])));
                //rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(getBon("" + er.Bon[4])));
                #endregion old
            }
            else if (or != null)
            {
                ReportHelper.Append(rw, doc, "sex", or.Sex==Rabbit.SexType.MALE?"male":"female");
                ReportHelper.Append(rw, doc, "age", or.Status.ToString()+(or.zone==1?"(списан)":""));
                ReportHelper.Append(rw, doc, "weight", or.Rate.ToString());
                ReportHelper.Append(rw, doc, "class", getBon(or.Bon));
                ReportHelper.Append(rw, doc, "name", or.NameFull);
                ReportHelper.Append(rw, doc, "wclass", getBon("" + or.Bon[1]));
                ReportHelper.Append(rw, doc, "bclass", getBon("" + or.Bon[1]));
                ReportHelper.Append(rw, doc, "hclass", getBon("" + or.Bon[3]));
                ReportHelper.Append(rw, doc, "cclass", getBon("" + or.Bon[4]));
                #region old
                //rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode(or.Sex==Rabbit.SexType.MALE?"male":"female"));
                //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(or.Status.ToString()+(or.zone==1?"(списан)":"")));
                //rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(or.Rate.ToString()));
                //rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(getBon(or.Bon)));
                //rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(or.NameFull));
                //rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(getBon(""+or.Bon[1])));
                //rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(getBon("" + or.Bon[2])));
                //rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(getBon("" + or.Bon[3])));
                //rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(getBon("" + or.Bon[4])));
                #endregion old
            }
            else
            {
                ReportHelper.Append(rw, doc, "sex", "none");
                ReportHelper.Append(rw, doc, "age", "");
                ReportHelper.Append(rw, doc, "weight", "");
                ReportHelper.Append(rw, doc, "class", "");
                ReportHelper.Append(rw, doc, "name", "");
                ReportHelper.Append(rw, doc, "wclass", "");
                ReportHelper.Append(rw, doc, "bclass", "");
                ReportHelper.Append(rw, doc, "hclass", "");
                ReportHelper.Append(rw, doc, "cclass", "");
                #region old
                //rw.AppendChild(doc.CreateElement("sex")).AppendChild(doc.CreateTextNode("none"));
                //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("weight")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("class")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("name")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("wclass")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("bclass")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("hclass")).AppendChild(doc.CreateTextNode(""));
                //rw.AppendChild(doc.CreateElement("cclass")).AppendChild(doc.CreateTextNode(""));
                #endregion old
            }
            return doc;
        }

        private void svidMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count != 1) return;
            XmlDocument[] docs=new XmlDocument[7];
            RabNetEngRabbit r=Engine.get().getRabbit((int)listView1.SelectedItems[0].Tag);
            docs[0]=rabToXml(r,null);
            OneRabbit[] p1 = Engine.db().getParents(r.RabID, r.Age);
            docs[1] = rabToXml(null, p1[0]);
            docs[2] = rabToXml(null, p1[1]);
            OneRabbit[] p2;
            if (p1[0] != null)
                p2 = Engine.db().getParents(p1[0].ID, p1[0].Age);
            else
                p2 = new OneRabbit[] { null, null };
            docs[3] = rabToXml(null, p2[0]);
            docs[4] = rabToXml(null, p2[1]);
            if (p1[1] != null)
                p2 = Engine.db().getParents(p1[1].ID, p1[1].Age);
            else
                p2 = new OneRabbit[] { null, null };
            docs[5] = rabToXml(null, p2[0]);
            docs[6] = rabToXml(null, p2[1]);
            ReportViewForm rf = new ReportViewForm(myReportType.RABBIT, docs);
            rf.ShowDialog();
            if (rf.printed)
            {
                int num = Engine.opt().getIntOption(Options.OPT_ID.NEXT_SVID);
                Engine.opt().setOption(Options.OPT_ID.NEXT_SVID, num + 1);
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void plemMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            int cnt = 0;
            string brd = "";
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                RabNetEngRabbit r = Engine.get().getRabbit((int)li.Tag);
                cnt += r.Group;
                if (brd == "")
                    brd = r.BreedName;
                if (r.BreedName != brd)
                    brd = "none";
                rabToXml(r, null, doc);
            }
            XmlDocument doc2 = new XmlDocument();
            XmlElement rw = (XmlElement)doc2.AppendChild(doc2.CreateElement("Rows")).AppendChild(doc2.CreateElement("Row"));
            ReportHelper.Append(rw, doc2, "date", DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            ReportHelper.Append(rw, doc2, "breed", brd);
            ReportHelper.Append(rw, doc2, "count", cnt.ToString());
            //rw.AppendChild(doc2.CreateElement("date")).AppendChild(doc2.CreateTextNode(DateTime.Now.Date.ToShortDateString()+" "+DateTime.Now.ToLongTimeString()));
            //rw.AppendChild(doc2.CreateElement("breed")).AppendChild(doc2.CreateTextNode(brd));
            //rw.AppendChild(doc2.CreateElement("count")).AppendChild(doc2.CreateTextNode(cnt.ToString()));
            new ReportViewForm(myReportType.PRIDE, new XmlDocument[] { doc, doc2 }).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void realizeMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;
            Filters f = new Filters();
            f["cnt"] = listView1.SelectedItems.Count.ToString();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                f["r" + i.ToString()] = ((int)listView1.SelectedItems[i].Tag).ToString();
            new ReportViewForm(myReportType.REALIZE, Engine.db().makeReport(myReportType.REALIZE, f)).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

		private void GeneticsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count < 1) return;
#if PROTECTED
            if (!RabGRD.GRD.Instance.GetFlag(RabGRD.GRD_Base.FlagType.Genetics))
            {
                MessageBox.Show("Текущая лицензия не распространяется на данный модуль");
                return;
            }
#endif
            if (GeneticsManagerSafe.GeneticsModuleTest())
			{
				for (int i = 0; i < listView1.SelectedItems.Count; i++)
				{
                    GeneticsManagerSafe.AddNewGenetics(((int)listView1.SelectedItems[i].Tag));
				}
			}
			else
			{
				MessageBox.Show("Не найден модуль 'gui_genetics.dll'!\nПроверьте правильность установки программы.", "Модуль генетики", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void actMenu_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                setMenu(-1, 0, false);
                return;
            }
            string sx = "";
            for (int i = 0; i < listView1.SelectedItems.Count && sx.Length < 2; i++)
            {
                String s = listView1.SelectedItems[i].SubItems[SEXFIELD].Text;
                if (s[0] == 'С' || s[0] == 'C')
                    s = "S";
                if (!sx.Contains(s))
                    sx += s;
            }
            int isx = 3;
            if (sx == "?") isx = 0;
            if (sx == "м") isx = 1;
            if (sx == "ж") isx = 2;
            if (sx == "S") isx = 4;
            bool kids = false;
            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].SubItems[NFIELD].Text[0] == '+')
                kids = true;
            setMenu(isx, listView1.SelectedItems.Count, kids);
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
                ReportHelper.Append(rw, doc, "age", li.SubItems[2].Text); //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(li.SubItems[2].Text));
                String cn=li.SubItems[11].Text;
                if (cn.IndexOf('[') > -1)
                    cn = cn.Remove(cn.IndexOf('['));
                ReportHelper.Append(rw, doc, "address", cn); //rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(cn));
                cn=li.SubItems[NFIELD].Text;
                if (cn[0]=='[')
                    cn=int.Parse(cn.Substring(1,cn.Length-2)).ToString();
                if (cn == "-") cn = "1";
                ReportHelper.Append(rw, doc, "count", cn);//rw.AppendChild(doc.CreateElement("count")).AppendChild(doc.CreateTextNode(cn));
            }
            new ReportViewForm(myReportType.REPLACE, doc).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        //private void логЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Directory.Exists("zapis")) Directory.CreateDirectory("zapis");
        //        string path = "zapis/log_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt";
        //        StreamWriter rw = new StreamWriter(path);

        //        foreach (ListViewItem x in listView1.Items)
        //        {
        //            string s = x.SubItems[1].Text;
        //            for (int i = 1; i < x.SubItems.Count - 1; i++) 
        //            {
        //                if (i==2 || i==8) continue;
        //                if (i == 1 || x.SubItems[i].Text.StartsWith("С"))
        //                {
        //                    s += "|С";
        //                    continue;
        //                }
        //                s += "|" + x.SubItems[i].Text;
        //            }
                    
        //            rw.WriteLine(s);
        //        }
        //        rw.Close();
        //        MessageBox.Show("Запись прошла успешно\n\rИмя файла: " + path, "Сохранение в файл");

        //    }
        //    catch (Exception exep)
        //    {
        //        MessageBox.Show(exep.Message);
        //    }
            
        //}

        private void tsmiIDshow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("r_id = "+listView1.SelectedItems[0].Tag.ToString());
        }

        private void makeExcel()
        {         
#if !DEMO
            ExcelMaker.MakeExcelFromLV(listView1, "Поголовье");
#endif
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt || e.Shift || e.Control|| !char.IsLetter((char)e.KeyValue)) return;

            //KeysConverter ks = new KeysConverter();
            //CultureInfo ci = CultureInfo.CurrentUICulture;
            //string letter = cic.ConvertToString(e.KeyData);//todo при нажатии русской буквы переходить к строке
            //listView1.FindItemWithText("п").EnsureVisible(); 
        }
    }
}
