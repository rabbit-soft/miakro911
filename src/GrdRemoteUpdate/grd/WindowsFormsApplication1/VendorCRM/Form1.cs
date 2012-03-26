using System;
using System.Net;
using System.Windows.Forms;
using CookComputing.XmlRpc;

namespace VendorCRM
{
    public struct StructLogin
    {
        public int Error;
        public string Hash;
    }
    public partial class Form1 : Form
    {
        private IRPC rpcProxy;
        public Form1()
        {
            InitializeComponent();
            rpcProxy = XmlRpcProxyGen.Create<IRPC>();
            rpcProxy.Url = "http://svc.9-bits.ru/crm/rpc.php";
            rpcProxy.NonStandard = XmlRpcNonStandard.All;
        }

        public Int32 OnWriteData(Byte[] buf, Int32 size, Int32 nmemb,
            Object extraData)
        {
            Console.Write(System.Text.Encoding.UTF8.GetString(buf));
            textBox1.Text = System.Text.Encoding.UTF8.GetString(buf);
            return size * nmemb;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
//                textBox1.Text = rpcProxy.GetTime(textBox2.Text);
                StructLogin sl=rpcProxy.Login("xplora","sss");
                textBox1.Text = sl.Hash+" "+sl.Error.ToString();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void HandleException(Exception ex)
        {
            string msgBoxTitle = "Error";
            try
            {
                throw ex;
            }
            catch (XmlRpcFaultException fex)
            {
                MessageBox.Show("Fault Response: " + fex.FaultCode + " "
                    + fex.FaultString, msgBoxTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (WebException webEx)
            {
                MessageBox.Show("WebException: " + webEx.Message, msgBoxTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (webEx.Response != null)
                    webEx.Response.Close();
            }
            catch (XmlRpcServerException xmlRpcEx)
            {
                MessageBox.Show("XmlRpcServerException: " + xmlRpcEx.Message,
                    msgBoxTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception defEx)
            {
                MessageBox.Show("Exception: " + defEx.Message, msgBoxTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text=rpcProxy.ResponseHeaders.ToString();
            }
        }
    }
    public interface IRPC : IXmlRpcProxy
    {
        [XmlRpcMethod("get_time")]
        string GetTime(string name);
        [XmlRpcMethod("Login")]
        StructLogin Login(string name, string pass);
    }
}
