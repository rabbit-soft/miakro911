using System.Windows.Forms;
using System.Drawing;

namespace rabnet.components
{
    public partial class RabGenTreeView : TreeView
    {
        private Color DEAD = Color.Brown;
        private Color IA = Color.SteelBlue;

        private int _maxCnt=1;
        private bool _showStateColors = true;

        public string NameFormat = "n, A, C";

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

        public bool ShowStateColors
        {
            get { return _showStateColors; }
            set { _showStateColors = value; }
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
            //TreeNode tn = this.Nodes.Insert(0, data.NameFormat(NameFormat));
            TreeNode tn =insertNode(null,data);
            tn.ExpandAll();
            tn.EnsureVisible();
            return tn;
        }
        public TreeNode InsertNode(RabTreeData data)
        {
            return InsertNode(data, false);
        }

        private TreeNode insertNode(TreeNode parentNode, RabTreeData data)
        {
            TreeNode tn;
            if (parentNode == null)
                tn = this.Nodes.Insert(0, data.NameFormat(NameFormat));
            else
                tn = parentNode.Nodes.Add(data.NameFormat(NameFormat));

            if (_showStateColors)
                {
                if (data.State == RabAliveState.DEAD)
                    tn.ForeColor = DEAD;
                else if (data.State == RabAliveState.IMPORTED_ASCENDANT)
                    tn.ForeColor = IA;
            }

            if (data.Mother != null)
                    {
                //TreeNode n = parentNode.Nodes.Add(data.Mother.NameFormat(NameFormat));
                //insertNode(n, data.Mother);
                insertNode(tn, data.Mother);
                    }
            if (data.Father != null)
            {
                //TreeNode n = parentNode.Nodes.Add(data.Father.NameFormat(NameFormat));
                //insertNode(n, data.Father);
                insertNode(tn, data.Father);
                }
            tn.Tag = data;
            return tn;
        }
    }
}
