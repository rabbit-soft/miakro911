using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace butcher
{
    public partial class NumPad : UserControl
    {
        private List<Control> _ctrls = new List<Control>();
        private double _ratio = 0;
        private double _thisRatio;
        private bool _manual = false;

        public event EventHandler OkButtonClick;

        public override Font Font
        {
            get { return button1.Font; }
            set
            {
                foreach (Control bt in this.Controls)
                {
                    if (bt is Button)
                        bt.Font = value;
                }
            }
        }

        public NumPad()
        {
            InitializeComponent();
            _thisRatio = (double)this.Width / (double)this.Height;
            _ratio = (double)button1.Width / (double)this.Width;
            _manual = true;
        }

        public bool OnlyDigits
        {
            get { return !btDot.Visible; }
            set { btDot.Visible = !value; }
        }

        public bool OkButtonEnable
        {
            get { return btOk.Enabled; }
            set { btOk.Enabled = value; }
        }

        public bool OkButtonVisible
        {
            get { return btOk.Visible; }
            set { btOk.Visible = value;}
        }

        /// <summary>
        /// Добавляет компонент, текст которого будет редактироваться сиим компонентом
        /// </summary>
        /// <param name="ctrl">Компонент</param>
        /// <returns>Успешно ли прошла операция</returns>
        public bool AddControl(Control ctrl)
        {
            if (_ctrls.Contains(ctrl)) return false;
            _ctrls.Add(ctrl);
            return true;
        }
        /// <summary>
        /// Удаляет компонент, текст которого будет редактироваться сиим компонентом
        /// </summary>
        /// <param name="ctrl">Компонент</param>
        /// <returns>Успешно ли прошла операция</returns>
        public bool RemoveControl(Control ctrl)
        {
            if (!_ctrls.Contains(ctrl)) return false;
            _ctrls.Remove(ctrl);
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in _ctrls)
            {
                if (sender == btDot)
                {
                    if (c.Text.Contains(btDot.Text)) return;
                    if (c.Text.Length == 0) c.Text += "0";
                } 
                c.Text += (sender as Button).Text;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            foreach (Control c in _ctrls)
            {
                if (c.Text.Length != 0)
                    c.Text = c.Text.Remove(c.Text.Length-1);
            }
        }

        private void NumPad_Resize(object sender, EventArgs e)
        {
            if (!_manual) return;
            int margin = -1;
            this.Width = (int)((double)this.Height * _thisRatio);
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    c.Width = (int)((double)this.Width * _ratio);
                    c.Height = c.Width;
                    if (margin == -1)
                        margin = (int)((double)(this.Width - 3 * c.Width) / 6);
                    if (c == btOk)
                        c.Width = c.Width * 3 + margin * 4;
                    int rowInd = int.Parse(c.Tag.ToString()[0].ToString());
                    int colInd = int.Parse(c.Tag.ToString()[1].ToString());

                    c.Top = (rowInd - 1) * c.Height + (rowInd-1) * 2 * margin + margin;
                    c.Left = (colInd - 1) * c.Width + (colInd-1) * 2 * margin + margin;
                    c.Font = new Font(c.Font.Name,c.Height/2);
                }
            }            
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            OkButtonClick(sender, e);
        }

        private void NumPad_Load(object sender, EventArgs e)
        {
            btDel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
        }

    }
}
