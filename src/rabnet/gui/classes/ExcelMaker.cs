#if !DEMO
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using X_Tools;

namespace rabnet
{
    internal static class ExcelMaker
    {
        private static XmlNode[] _xmls;
        private static myReportType _repType;

        public static void MakeExcelFromXML(XmlNode[] xmls, myReportType repType)///Для плагинов сделать excel тоже
        {
            _repType = repType;
            _xmls = xmls;
            string path = location();
            if (path == "") return;

            path = XTools.DuplicateName(path+ "\\" + filename());
            object misValue = Type.Missing;
            Excel.Application xlApp = new Excel.ApplicationClass();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            WaitForm wf = new WaitForm();
            try
            {              
                wf.Flush(); wf.MaxValue = 100; wf.Show(); wf.Style = ProgressBarStyle.Blocks;
                int row, col;
                switch (repType)
                {
                    case myReportType.USER_OKROLS:
                        string[,] matrix = doMegaMAtr(xmls[0]);
                        wf.MaxValue = matrix.GetLength(0) * matrix.GetLength(1);
                        for (int i = 0; i < matrix.GetLength(0); i++)
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                if (matrix[i, j] != null)
                                    xlWorkSheet.Cells[i + 1, j + 1] = matrix[i, j];
                                else xlWorkSheet.Cells[i + 1, j + 1] = "";
                                wf.Inc();
                            }
                        break;
                    default:
                        drawHeader(repType, ref xlWorkSheet);
                        row = 2;
                        wf.MaxValue = xmls[0].FirstChild.ChildNodes.Count;
                        foreach (XmlNode nd in xmls[0].FirstChild.ChildNodes)
                        {
                            col = 1;
                            foreach (XmlNode nd2 in nd.ChildNodes)
                            {
                                xlWorkSheet.Cells[row, col] = nd2.InnerText;
                                col++;
                            }
                            row++;
                            wf.Inc();
                        }

                        break;
                }
                wf.Hide();               

                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка");
                wf.Visible = false;
            }
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            killExcelProcess();
        }

        public static void MakeExcelFromLV(ListView lv, string name)
        {
            WaitForm wf = new WaitForm();
            object misValue = Type.Missing;
            Excel.Application xlApp = new Excel.ApplicationClass();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            try
            {
                string path = location();
                if (path == "") return;
                path = XTools.DuplicateName(path + "\\" + name + " " + DateTime.Now.ToShortDateString() + ".xls");
                wf.Flush(); wf.MaxValue = 100; wf.Show(); wf.Style = ProgressBarStyle.Blocks;
                int rate = lv.Items.Count / 100;
                int cols = lv.Columns.Count;//для обеспечения быстроты заполнения

                for (int h = 0; h < lv.Columns.Count; h++)//Заполнение названиями когонок
                {
                    xlWorkSheet.Cells[1, h + 1] = lv.Columns[h].Text;
                }
                ((Excel.Range)xlWorkSheet.Rows[1, Type.Missing]).Font.Bold = true;
                    
                for (int i = 0; i < lv.Items.Count; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        xlWorkSheet.Cells[i + 2, j + 1] = lv.Items[i].SubItems[j].Text;
                    }
                    if (rate != 0 && (i % rate == 0 || i == lv.Items.Count))
                    {
                        Application.DoEvents();
                        if(!wf.isFull)
                            wf.Inc();
                    }
                }
                wf.Hide();
                xlWorkSheet.Columns.AutoFit();
                xlWorkBook.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
            }
            catch (Exception ex)
            {
                wf.Hide();
                MessageBox.Show(ex.Message, "Произошла ошибка");
            }
            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            killExcelProcess();
        }

        private static string location()
        {
            if (Engine.opt().getIntOption(Options.OPT_ID.XLS_ASK) == 1)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                    return dlg.SelectedPath;
                else return "";
            }
            else
            {
                string path = Engine.opt().getOption(Options.OPT_ID.XLS_FOLDER);
                if (!Directory.Exists(path))
                    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                else return path;
            }
        }

        private static string[,] doMegaMAtr(XmlNode roteNode)
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

        private static void killExcelProcess()
        {
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

        private static string filename()
        {
            string filename = "";
            if (_xmls.Length > 1)
            {
                filename += ReportHelper.getRusName(_repType) + " (";
                foreach (XmlNode nd in _xmls[1].FirstChild.ChildNodes)
                {
                    foreach (XmlNode nd2 in nd.ChildNodes)
                        filename += nd2.InnerText + " ";
                }
                filename += ").xls";
            }
            else
            {
                filename = ReportHelper.getRusName(_repType) + " " + DateTime.Now.ToShortDateString() + ".xls";
            }
            return filename;
        }

        /// <summary>
        /// Заполняет заголовки колонок.
        /// (по хорошему их нужно передавать при формировании ХМЛ)
        /// </summary>
        /// <param name="repType">Тип отчета</param>
        /// <param name="xlWorkSheet"></param>
        private static void drawHeader(myReportType repType, ref Excel.Worksheet xlWS)
        {
            xlWS.get_Range("A1", "Z1").Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
            xlWS.get_Range("A1", "Z1").Font.Bold = true;
            int col = 1;
            int row = 1;
            switch (repType)
            {
                case myReportType.BREEDS:
                    xlWS.Cells[row, col++] = "№";
                    xlWS.Cells[row, col++] = "Порода";
                    xlWS.Cells[row, col++] = "Производители";
                    xlWS.Cells[row, col++] = "Кандидаты";
                    xlWS.Cells[row, col++] = "Мальчики";
                    xlWS.Cells[row, col++] = "Штатные";
                    xlWS.Cells[row, col++] = "Первокролки";
                    xlWS.Cells[row, col++] = "Невесты";
                    xlWS.Cells[row, col++] = "Девочки";
                    xlWS.Cells[row, col++] = "Безполые";
                    xlWS.Cells[row, col++] = "Всего";
                    break;
                case myReportType.AGE:
                    xlWS.Cells[row, col++] = "Возраст";
                    xlWS.Cells[row, col++] = "Количество";
                    break;
                case myReportType.BY_MONTH:
                    xlWS.Cells[row, col++] = "Дата";
                    xlWS.Cells[row, col++] = "Всего";
                    xlWS.Cells[row, col++] = "Осталось";
                    break;
                case myReportType.DEADREASONS:
                    xlWS.Cells[row, col++] = "Причина";
                    xlWS.Cells[row, col++] = "Количество";
                    break;
                case myReportType.DEAD:
                    xlWS.Cells[row, col++] = "Дата";
                    xlWS.Cells[row, col++] = "Имя";
                    xlWS.Cells[row, col++] = "Количество";
                    xlWS.Cells[row, col++] = "Причина";
                    xlWS.Cells[row, col++] = "Заметки";
                    break;
                case myReportType.FUCKS_BY_DATE:
                    xlWS.Cells[row, col++] = "Дата";
                    xlWS.Cells[row, col++] = "Самка";
                    xlWS.Cells[row, col++] = "Самец";
                    xlWS.Cells[row, col++] = "Работник";
                    break;

            }
        }
    }
    

}
#endif
