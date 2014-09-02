using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;

namespace rabnet.forms
{
    public partial class ReportViewForm : Form
    {
#if !DEMO
        public delegate void ExcelCallBack(XmlNode[] xmls);

        private XmlDocument[] _xmls = null;
        private myReportType _repType = myReportType.TEST;
        private string _repName = "";
        private string _repFile = "";
        private string[] _xclHeaders;

        public bool IsPrinted = false;

        public ReportViewForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Предусмотрена ли импортирование отчета в Excel
        /// </summary>
        public bool ExcelEnabled
        {
            get { return tbExcel.Visible; }
            set { tbExcel.Visible = value; }
        }

        /// <summary>
        /// Отправляет данные ввиде XML
        /// </summary>
        private void setData()
        {
            fyiReporting.RDL.DataSets ds = rdlViewer1.Report.DataSets;
            ds["Data"].SetData(_xmls[0]);
            for (int i = 1; i < _xmls.Length; i++)
                ds["Data" + (i + 1).ToString()].SetData(_xmls[i]);
        }
        
        public ReportViewForm(myReportType type, XmlDocument xml)
            : this(type, new XmlDocument[]{xml}) {}
        public ReportViewForm(XmlDocument xml) 
            : this(myReportType.TEST, xml) {}
        public ReportViewForm(myReportType type, XmlDocument[] xml)
            : this()
        {
            this._repType = type;
            this._repFile = ReportHelper.GetFileName(type);
            this._repName = ReportHelper.GetRusName(type);
            this._xclHeaders = ReportHelper.GetHeaders(_repType);
            build(xml);
        }

        public ReportViewForm(String name, String fileName, XmlDocument[] xmls) 
            : this(name, fileName, xmls, new string[]{}) { }
        public ReportViewForm(String name, String fileName, XmlDocument[] xmls, string[] xclHeaders)
            :this()
        {
            _repName = name;
            _repFile = fileName;
            _xclHeaders = xclHeaders;
            build(xmls);
        }

        private void build(XmlDocument[] xml)
        {
            _xmls = xml;
            string fn = Path.GetDirectoryName(Application.ExecutablePath) + "\\reports\\" + _repFile + ".rdl";
            rdlViewer1.SourceFile = fn;
            setData();
            rdlViewer1.Rebuild();
            Text = _repName;
        }
        
#endif
        private void tbSave_Click(object sender, EventArgs e)
        {
#if !DEMO
            setData();
            sfd.FileName = _repName;
	        if (sfd.ShowDialog(this) != DialogResult.OK)
		        return;
            string ext = null;
	        int i = sfd.FileName.LastIndexOf('.');
	        if (i < 1)
		        ext = "";
	         else
		        ext = sfd.FileName.Substring(i+1).ToLower();
            switch (ext)
            {
                case "pdf":
                case "xml":
                case "html":
                case "htm":
                case "csv":
                case "rtf":
                case "mht":
                case "mhtml":
                case "xlsx":
                case "tif":
                case "tiff":
                    try { rdlViewer1.SaveAs(sfd.FileName, ext); }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                default:
                    MessageBox.Show(String.Format("Неизвестный формат {0}.", ext));
                    break;
            }   
#endif
        }

        private void tbPrint_Click(object sender, EventArgs e)
        {
#if !DEMO
            print(false);
#endif
        }

        private void tbPreference_Click(object sender, EventArgs e)
        {
#if !DEMO
            print(true);
#endif
        }

#if !DEMO
        private void print(bool options)
        {
            PrintDocument pd = new PrintDocument();
            pd.DocumentName = _repName;
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
                IsPrinted = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка печати: "+ex.Message);
            }
        }
#endif

        private void pageScaleMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitPage;
            scaleBtn.Text = pageScaleMenuItem.Text;
#endif
        }

        private void widthScaleMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.FitWidth;
            scaleBtn.Text = widthScaleMenuItem.Text;
#endif
        }

#if !DEMO
        private void zoom(double value)
        {
            rdlViewer1.Zoom = (float)value;
            rdlViewer1.ZoomMode = fyiReporting.RdlViewer.ZoomEnum.UseZoom;
            scaleBtn.Text = String.Format("{0:d}%", (int)(value * 100));
        }
#endif
        
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
#if !DEMO
            zoom(1.5);
#endif
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
#if !DEMO
            zoom(0.5);
#endif
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
#if !DEMO
            zoom(1);
#endif
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
#if !DEMO
            zoom(2);
#endif
        }

        private void ReportViewForm_Load(object sender, EventArgs e)
        {
#if !DEMO
                timer1.Start();
#endif
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
#if !DEMO
            timer1.Stop();
            WindowState = FormWindowState.Maximized;
#endif
        }

        /// <summary>
        /// Делает выгрузку в EXCEL
        /// </summary>
        private void tbExcel_Click(object sender, EventArgs e)
        {
#if !DEMO
            ExcelMaker.MakeExcelFromXML(_xmls, _repName, _xclHeaders);     
#else
            DemoErr.DemoNoModuleMsg();
#endif
        }
    }
}
