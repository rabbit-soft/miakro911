using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Drawing.Printing;

namespace rabnet
{
    public partial class ReportViewForm : Form
    {
        private string rname="Отчет";
        private XmlDocument[] xmls = null;
        public ReportViewForm()
        {
            InitializeComponent();
        }

        public void setData()
        {
            fyiReporting.RDL.DataSets ds = rdlViewer1.Report.DataSets;
            ds["Data"].SetData(xmls[0]);
            for (int i = 1; i < xmls.Length; i++)
                ds["Data" + (i + 1).ToString()].SetData(xmls[i]);

        }

        public ReportViewForm(String reportname, String fileName, XmlDocument xml)
            : this(reportname,fileName,new XmlDocument[]{xml}){}
        public ReportViewForm(String reportname, String fileName, XmlDocument[] xml)
            : this()
        {
            xmls = xml;
            string fn = Path.GetDirectoryName(Application.ExecutablePath) + "/reports/" + fileName + ".rdl";
            //string fn = Path.GetDirectoryName(Application.ExecutablePath) + "/" + fileName + ".rdl";
            //if (!File.Exists(fn))
            rdlViewer1.SourceFile = fn;
            setData();
            rdlViewer1.Rebuild();
            rname = reportname;
            Text = rname;
        }
        public ReportViewForm(String fileName, XmlDocument xml)
            : this("Отчет",fileName,xml)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            setData();
            sfd.FileName=rname;
	        if (sfd.ShowDialog(this) != DialogResult.OK)
		        return;
        	string ext=null;
	        int i = sfd.FileName.LastIndexOf('.');
	        if (i < 1)
		        ext = "";
	         else
		        ext = sfd.FileName.Substring(i+1).ToLower();
	        switch(ext)
	        {
		        case "pdf":	case "xml": case "html": case "htm": case "csv": case "rtf": 
                case "mht": case "mhtml": case "xlsx": case "tif": case "tiff":
                try {rdlViewer1.SaveAs(sfd.FileName, ext);}
	    		catch (Exception ex)
		    	{
			    	MessageBox.Show(ex.Message);
			    }
			    break;
		    default:
			    MessageBox.Show(String.Format("Неизвестный формат {0}.", ext));
			break;
	        }       
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            print(false);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            print(true);
        }

        private void print(bool options)
        {
            PrintDocument pd=new PrintDocument();
            pd.DocumentName = rname;
            pd.PrinterSettings.FromPage = pd.PrinterSettings.MinimumPage= 1;
            pd.PrinterSettings.ToPage = pd.PrinterSettings.MaximumPage=rdlViewer1.PageCount;
            pd.DefaultPageSettings.Landscape=(rdlViewer1.PageWidth>rdlViewer1.PageHeight);
            PrintDialog dlg = new PrintDialog();
            dlg.AllowSelection=dlg.AllowSomePages=true;
            dlg.Document = pd;
            if (options)
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
            try
            {
                if (options && (pd.PrinterSettings.PrintRange == PrintRange.Selection))
                    pd.PrinterSettings.FromPage = rdlViewer1.PageCurrent;
                rdlViewer1.Print(pd);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка печати: "+ex.Message);
            }
        }

        private void pageScaleMenuItem_Click(object sender, EventArgs e)
        {
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitPage;
            scaleBtn.Text = pageScaleMenuItem.Text;
        }

        private void widthScaleMenuItem_Click(object sender, EventArgs e)
        {
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            scaleBtn.Text = widthScaleMenuItem.Text;
        }

        private void zoom(double value)
        {
            rdlViewer1.Zoom = (float)value;
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
            scaleBtn.Text = String.Format("{0:d}%", (int)(value * 100));
        }
        
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            zoom(1.5);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            zoom(0.5);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            zoom(1);
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            zoom(2);
        }

        private void ReportViewForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            WindowState = FormWindowState.Maximized;
        }
    }
}
