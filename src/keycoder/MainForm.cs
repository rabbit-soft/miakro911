using System;
using System.Windows.Forms;
using Guardant;

namespace KeyCoder
{
    public partial class MainForm : Form
    {
        Handle key;
        public MainForm()
        {
            InitializeComponent();
            Api32.GrdStartup(GrdFMR.Local);
            key = Api32.GrdCreateHandle(GrdCHM.SingleThread);
            if (key.Address == 0)
                log("Error creating key");
            else button1.Enabled = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Api32.GrdCleanup();
        }

        private void log(String str)
        {
            textBox2.Text += str + "\r\n";
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.SelectionLength = 0;
            textBox2.ScrollToCaret();
        }
        private void LogError(GrdE res,String action)
        {
            String err="";
            Api32.GrdFormatMessage(res,GrdLNG.Russian,out err);
            log(String.Format("GRD ERROR {0:s} in {1:s}: {2:s}",res.ToString(),action,err));
        }
        private void lodException(Exception ex)
        {
            log("Exception:" + ex.GetType().ToString() + ":" + ex.Message);
        }

        private void check(GrdE res,String action)
        {
            if (res!=GrdE.OK)
            {
                LogError(res, action);
                throw new ApplicationException("Cant write key");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint id = 0;
            pFindInfo pf=new pFindInfo();
            try{
            button1.Enabled = false;
            check(Api32.GrdSetAccessCodes(key, 0, 0, uint.Parse(textBox1.Text), 0),"SetAccessCodes");
            check(Api32.GrdSetFindMode(key, GrdFMR.Local, GrdFM.Type, 0, 0, 0, 0, 0, GrdDT.Win, GrdFMM.ALL, GrdFMI.USB),"SetFindMode");
            check(Api32.GrdFind(key, GrdF.First, out id,out pf), "Find");
            log("All OK");
            button1.Enabled = true;
            }
            catch (Exception ex)
            {
                lodException(ex);
            }
        }
    }

}
