using System;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class BuildTreeForm : Form
    {
        public BuildTreeForm()
        {
            InitializeComponent();
            BldTreeData buildTree = Engine.db().buildingsTree();
            TreeNode n = makenode(null, "Ферма", buildTree);
            n.Tag = new BldTreeData(0, 0, "Ферма");
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
        private TreeNode makenode(TreeNode parent, String name, BldTreeData td)
        {
            TreeNode n = null;
            if (parent == null) {
                n = treeView1.Nodes.Add(name);
            } else {
                n = parent.Nodes.Add(name);
            }

            if (td.ChildNodes != null)
                for (int i = 0; i < td.ChildNodes.Count; i++) {
                    BldTreeData td1 = td.ChildNodes[i];
                    if (td1.FarmId != 0) {
                        continue;
                    }
                    TreeNode child = makenode(n, td1.Name, td.ChildNodes[i]);

                    child.Tag = td1;
                }
            return n;
        }

        public int Build { get { return (tn.Tag as TreeData).ID; } }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tn == null) {
                MessageBox.Show("Площадка не выбрана");
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private TreeNode tn { get { return treeView1.SelectedNode; } }
    }
}
