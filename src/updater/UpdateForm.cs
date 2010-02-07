using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace updater
{
    public partial class UpdateForm : Form
    {
        public int result = 0;
        private string filename="";
        private Dictionary<int, String> scr = new Dictionary<int, string>();
        public UpdateForm()
        {
            InitializeComponent();
        }
        public UpdateForm(String fl):this()
        {
            filename = fl;
            label1.Text = getScripts().ToString();
        }
        public int getScripts()
        {
            int i = 2;
            string prefix = "";
            try
            {
                new StreamReader(GetType().Assembly.GetManifestResourceStream("2.sql"));
            }catch(Exception)
            {
                prefix="updater.sql.";
            }
            try{
                while (true)
                {
                    StreamReader stm = new StreamReader(GetType().Assembly.GetManifestResourceStream(prefix + i.ToString()+".sql"), Encoding.UTF8);
                    String cmd = stm.ReadToEnd();
                    stm.Close();
                    scr[i] = cmd;
                    i++;
                }
            }catch(Exception)
            {
                i--;
            }
            return i;
        }

    }
}
