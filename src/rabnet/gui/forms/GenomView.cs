using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class GenomView : Form
    {
        private Catalog brds = Engine.db().catalogs().getBreeds();
        private Catalog msn = Engine.db().catalogs().getSurNames(1, "ы");
        private Catalog fsn = Engine.db().catalogs().getSurNames(2, "ы");
        private Color bcolor = Color.Red;
        public GenomView()
        {
            InitializeComponent();
        }

        public GenomView(int r1, int r2):this()
        {
            RabNetEngRabbit rb1 = Engine.get().getRabbit(r1);
            RabNetEngRabbit rb2 = Engine.get().getRabbit(r2);
            MakeGenesis(rb1.breed, rb2.breed, rb1.genom, rb2.genom, rb1.fullName, rb2.fullName);
        }

        public GenomView(int b1, int b2, String g1, String g2,String n1,String n2):this()
        {
            MakeGenesis(b1, b2, g1, g2, n1, n2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private String findBreed(int b)
        {
            if (brds.ContainsKey(b))
                return brds[b];
            return "Гибрид";
        }

        public String MakeChildName(String n1,String n2)
        {
            return "";
            //return n1.Split(' ')[0] + "ы-" + n2.Split(' ')[0] + "ы";
        }

        private ListViewItem InsertGen(ListView l, String gen)
        {
            int pos = 0;
            for (int i = 0; i < l.Items.Count; i++)
            {
                if (gen == l.Items[i].Text)
                    return null;
                if (int.Parse(gen) > int.Parse(l.Items[i].Text))
                    pos++;
            }
            return l.Items.Insert(pos, gen);
        }

        private void addGen(ListView li,String gen,string[] ogens)
        {
            bool has=false;
            ListViewItem i = InsertGen(li, gen);
            ListViewItem i2 = InsertGen(listView3, gen);
            foreach (string s in ogens)
                if (s == gen)
                    has = true;
            if (has)
            {
                if (i!=null)
                    i.ForeColor = bcolor;
                if (i2!=null)
                    i2.ForeColor = bcolor;
                label11.Text = "Гетерозис: ДА";
                label11.ForeColor = bcolor;
            }

        }

        private void MakeGenesis(int b1,int b2,String g1,String g2,String n1,String n2)
        {
            label1.Text = n1;
            label4.Text = n2;
            label2.Text = findBreed(b1);
            label6.Text = findBreed(b2);
            label7.Text = MakeChildName(n1, n2);
            if (b1 != b2)
            {
                label10.Text = "Инбридинг: ДА";
                label10.ForeColor = Color.Red;
                label9.Text = "Гибрид";
            }
            else
                label9.Text = label2.Text;
            String[] g1s = g1.Split(' ');
            String[] g2s = g2.Split(' ');
            foreach (string gen in g1s)
                addGen(listView1, gen, g2s);
            foreach (string gen in g2s)
                addGen(listView2,gen,g1s);
        }
    }
}
