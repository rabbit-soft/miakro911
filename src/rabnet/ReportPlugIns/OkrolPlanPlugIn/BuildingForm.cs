using System;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildingForm : Form
    {
        public BuildingForm()
        {
            InitializeComponent();
            TreeData buildTree = Engine.db().buildingsTree();
            TreeNode n = makenode(null, "Ферма", buildTree);
            n.Tag = "0:0";
            n.Expand();
            treeView1.Sort();
        }

        /// <summary>
        /// Создает ветку в дереве строений.
        /// </summary>
        /// <param name="parent">Ветка-родитель</param>
        /// <param name="name">Название фетки</param>
        /// <param name="td"></param>
        /// <returns></returns>
        private TreeNode makenode(TreeNode parent, String name, TreeData td)
        {
            TreeNode n = null;
            if (parent == null)
                n = treeView1.Nodes.Add(name);
            else
                n = parent.Nodes.Add(name);
            if (td.items != null)
                for (int i = 0; i < td.items.Length; i++)
                {
                    String[] st = td.items[i].caption.Split(':');
                    if (st[1] != "0")
                        continue;
                    TreeNode child = makenode(n, st[2], td.items[i]);
                    
                    child.Tag = st[0] + ":" + st[1];
                }
            return n;
        }

        public string Build { get { return (tn.Tag as String).Split(':')[0]; } }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tn==null)
            {
                MessageBox.Show("Площадка не выбрана");
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private TreeNode tn { get { return treeView1.SelectedNode; } }
    }
}
