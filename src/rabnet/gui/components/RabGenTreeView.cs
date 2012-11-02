using System.Windows.Forms;

namespace rabnet
{
    public partial class RabGenTreeView : TreeView
    {
        public RabGenTreeView()
        {
            InitializeComponent();
        }

        public TreeNode InsertNode(RabTreeData data, bool append)
        {
            if (!append)
                this.Nodes.Clear();
            TreeNode tn = this.Nodes.Add(data.NameCombined);
            insertNode(tn,data);
            tn.ExpandAll();
            tn.EnsureVisible();
            return tn;
        }
        public TreeNode InsertNode(RabTreeData data)
        {
            return InsertNode(data, false);
        }

        private void insertNode(TreeNode nd, RabTreeData data)
        {
            if (data.Parents != null)
                while (data.Parents.Count > 0)
                {
                    if (data.Parents[0] != null)
                    {
                        TreeNode n = nd.Nodes.Add(data.Parents[0].NameCombined);
                        insertNode(n, data.Parents[0]);
                    }
                    data.Parents.RemoveAt(0);
                }
            nd.Tag = data;
        }
    }
}
