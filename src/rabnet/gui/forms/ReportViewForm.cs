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
using Excel = Microsoft.Office.Interop.Excel;

namespace rabnet
{
    public partial class ReportViewForm : Form
    {
#if !DEMO
        
        private XmlDocument[] xmls = null;
        private myReportType _repType = myReportType.TEST;
        private string _repName = "";
        private string _repFile = "";

        public bool printed = false;
        public ReportViewForm()
        {
            InitializeComponent();
        }

        public string RName
        {
            get
            {
                if (_repName != "") return _repName;
                switch (_repType)
                {
                    case myReportType.AGE: return "Статистика возрастного поголовья";
                    case myReportType.BREEDS: return "Отчет по породам";
                    case myReportType.BUTCHER_PERIOD: return "Стерильный цех";
                    case myReportType.BY_MONTH: return "Количество по месяцам";
                    case myReportType.DEAD: return "Списания";
                    case myReportType.DEADREASONS: return "Причины списаний";
                    case myReportType.FUCKER: return "Статистика продуктивности";
                    case myReportType.FUCKS_BY_DATE: return "Список случек и вязок";
                    case myReportType.PRIDE: return "Племенной список";
                    case myReportType.RABBIT: return "Племенное свидетельство";
                    case myReportType.REALIZE: return "Кандидаты на реализацию";
                    case myReportType.REPLACE: return "План пересадок";
                    case myReportType.REVISION: return "Ревизия свободных клеток";
                    case myReportType.SHED: return "Шедовый отчет";
                    case myReportType.USER_OKROLS: return "Окролы по пользователям";
                    default: return "test";
                }
            }
        }

        public string RFileName
        {
            get
            {
                if (_repFile != "") return _repFile;
                switch (_repType)
                {
                    case myReportType.AGE: return "age";
                    case myReportType.BREEDS: return "breeds";
                    case myReportType.BUTCHER_PERIOD: return "butcher";
                    case myReportType.BY_MONTH: return "by_month";
                    case myReportType.DEAD: return "dead";
                    case myReportType.DEADREASONS: return "deadreason";
                    case myReportType.FUCKER: return "fucker";
                    case myReportType.FUCKS_BY_DATE: return "fucks_by_date";
                    case myReportType.PRIDE: return "plem";
                    case myReportType.RABBIT: return "rabbit";
                    case myReportType.REALIZE: return "realization";
                    case myReportType.REPLACE: return "replace_plan";
                    case myReportType.REVISION: return "empty_rev";
                    case myReportType.SHED: return "shed";
                    case myReportType.USER_OKROLS: return "okrol_user";
                    default : return "test";
                }
            }
        }

        /// <summary>
        /// Отправляет данные ввиде XML
        /// </summary>
        public void setData()
        {
            fyiReporting.RDL.DataSets ds = rdlViewer1.Report.DataSets;
            ds["Data"].SetData(xmls[0]);
            for (int i = 1; i < xmls.Length; i++)
                ds["Data" + (i + 1).ToString()].SetData(xmls[i]);

        }
        
        public ReportViewForm(myReportType type, XmlDocument xml): this(type,new XmlDocument[]{xml}){}
        public ReportViewForm(XmlDocument xml) : this(myReportType.TEST, xml) { }
        public ReportViewForm(myReportType type, XmlDocument[] xml): this()
        {
            this._repType = type;
            build(xml);
        }

        public ReportViewForm(String name,String fileName, XmlDocument[] xml): this()
        {
            _repName = name;
            _repFile = fileName;
            build(xml);
        }

        private void build(XmlDocument[] xml)
        {
            xmls = xml;
            string fn = Path.GetDirectoryName(Application.ExecutablePath) + "/reports/" + RFileName + ".rdl";
            rdlViewer1.SourceFile = fn;
            setData();
            rdlViewer1.Rebuild();
            Text = RName;
        }
        
#endif
        private void tbSave_Click(object sender, EventArgs e)
        {
#if !DEMO
            setData();
            sfd.FileName = RName;
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
            pd.DocumentName = RName;
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
                printed = true;
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
            try
            {
                var misValue = Type.Missing;
                Excel.Application xlApp = new Excel.ApplicationClass();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                #region folder
                string filename = "";
                if (xmls.Length > 1)
                {
                    filename += RName + " (";
                    foreach (XmlNode nd in xmls[1].FirstChild.ChildNodes)
                    {
                        foreach (XmlNode nd2 in nd.ChildNodes)
                            filename += nd2.InnerText + " ";
                    }
                    filename += ").xls";
                }
                else
                {
                    filename = RName + " " + DateTime.Now.ToShortDateString() + ".xls";
                }
                const string folder = "Miakro 9.11 Excel";
                if (!Directory.Exists(xlApp.DefaultFilePath + "\\" + folder))
                    Directory.CreateDirectory(xlApp.DefaultFilePath + "\\" + folder);
                if (File.Exists(xlApp.DefaultFilePath + "\\" + folder + "\\" + filename))
                    if (MessageBox.Show("Файл \"" + filename + "\" уже существует заменить?", "Файл уже существует", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        File.Delete(xlApp.DefaultFilePath + "\\" + folder + "\\" + filename);
                    else return;
                #endregion folder
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                this.Enabled = false;
                int row, col;
                switch (_repType)
                {
                    case myReportType.USER_OKROLS:
                        string[,] matrix = doMegaMAtr(xmls[0]);
                        progressBar1.Maximum = matrix.GetLength(0) * matrix.GetLength(1);
                        for (int i = 0; i < matrix.GetLength(0); i++)
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                if (matrix[i, j] != null)
                                    xlWorkSheet.Cells[i + 1, j + 1] = matrix[i, j];
                                else xlWorkSheet.Cells[i + 1, j + 1] = "";
                                progressBar1.Value += 1;
                            }
                        break;
                    default:
                        row = 1;
                        progressBar1.Maximum = xmls[0].FirstChild.ChildNodes.Count;
                        foreach (XmlNode nd in xmls[0].FirstChild.ChildNodes)
                        {
                            col = 1;
                            foreach (XmlNode nd2 in nd.ChildNodes)
                            {
                                xlWorkSheet.Cells[row, col] = nd2.InnerText;
                                col++;
                            }
                            row++;
                            progressBar1.Value += 1;
                        }

                        break;
                }
                progressBar1.Visible = false;
                this.Enabled = true;

                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs(folder + "\\" + filename, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                //MessageBox.Show("Выгрузка произошла успешно." + Environment.NewLine + "Сохранено в: " + xlApp.DefaultFilePath + "\\" + folder);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                //убивает процесс excel, т.к. она сам че-то не убивается
                Hashtable myHashtable = new Hashtable();
                Process[] AllProcesses = Process.GetProcessesByName("excel");
                int iCount = 0;
                foreach (Process ExcelProcess in AllProcesses)
                {
                    myHashtable.Add(ExcelProcess.Id, iCount);
                    iCount = iCount + 1;
                }

                AllProcesses = Process.GetProcessesByName("excel");
                // check to kill the right process
                foreach (Process ExcelProcess in AllProcesses)
                {
                    if (myHashtable.ContainsKey(ExcelProcess.Id))
                        ExcelProcess.Kill();
                }
                AllProcesses = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка");
                progressBar1.Visible = false;
                this.Enabled = true;
            }
        }

        private string[,] doMegaMAtr(XmlNode roteNode)
        {
            try
            {
                List<string> horisontal = new List<string>();
                horisontal.Add("");
                List<string> vertical = new List<string>();
                vertical.Add("");
                foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
                {
                    if (!horisontal.Contains(nd.SelectSingleNode("dt").InnerText))
                        horisontal.Add(nd.SelectSingleNode("dt").InnerText);
                    if (!vertical.Contains(nd.SelectSingleNode("name").InnerText))
                        vertical.Add(nd.SelectSingleNode("name").InnerText);
                }
                horisontal.Sort();
                vertical.Sort();
                String[,] megaMatrix = new String[vertical.Count,horisontal.Count];
                for (int i = 0; i < vertical.Count; i++)
                    megaMatrix[i, 0] = vertical[i];
                for (int i = 0; i < horisontal.Count; i++)               
                    megaMatrix[0, i] = horisontal[i];
                foreach (XmlNode nd in roteNode.FirstChild.ChildNodes)
                {
                    int rowIndex = vertical.IndexOf(nd.SelectSingleNode("name").InnerText);
                    int colIndex = horisontal.IndexOf(nd.SelectSingleNode("dt").InnerText);             
                    megaMatrix[rowIndex, colIndex] = nd.SelectSingleNode("state").InnerText;
                }
                return megaMatrix;
            }
            catch {return new string[0,0];}
        }
        
    }
}
