﻿using System;
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
        public ReportViewForm()
        {
            InitializeComponent();
        }

        public ReportViewForm(String reportname, String fileName, XmlDocument xml)
            : this()
        {
            string fn="./"+fileName+".rdl";
            if (!File.Exists(fn))
                fn="./reports/" + fileName + ".rdl";
            rdlViewer1.SourceFile = fn;
            rdlViewer1.Report.DataSets["Data"].SetData(xml);
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
            dlg.AllowCurrentPage = true;
            dlg.AllowSelection = dlg.AllowSomePages=true;
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
    }
}
