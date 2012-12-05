using System.Windows.Forms;

namespace rabnet.components
{
    public partial class RabGenTreeView : TreeView
    {
        private int _maxCnt=1;

        public RabGenTreeView()
        {
            InitializeComponent();
        }

        public int MaxNodesCount
        {
            get { return _maxCnt; }
            set
            {
                if (value <= 1)
                    value = 1;
                _maxCnt = value;
            }
        }

        public TreeNode InsertNode(RabTreeData data, bool append)
        {
            if (!append)
                this.Nodes.Clear();
            else
            {
                //проверка на уже существование данной ветки
                for (int i = 0; i < this.Nodes.Count; )
                {
                    if ((this.Nodes[i].Tag != null) && (this.Nodes[i].Tag as RabTreeData).ID == data.ID)
                    {
                        this.Nodes[i].Remove();
                        continue;
                    }
                    i++;
                }
                //удаляем последнюю если
                while (this.Nodes.Count >= _maxCnt)
                    this.Nodes.RemoveAt(this.Nodes.Count-1);
            }
            TreeNode tn = this.Nodes.Insert(0,data.NameCombined);
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
